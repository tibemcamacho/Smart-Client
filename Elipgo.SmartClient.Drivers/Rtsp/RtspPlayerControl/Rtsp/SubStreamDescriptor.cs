using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading;
using MediaSuite.Common.TransformContext;
using StreamCoders;
using StreamCoders.Additional.Plugins;
using StreamCoders.Container;
using StreamCoders.Decoder;
using StreamCoders.Encoder;
using StreamCoders.Helpers;
using StreamCoders.Network;
using StreamCoders.Rtp;
using StreamCoders.Rtsp;
using StreamCoders.Rtsp.Extensions;

namespace MediaSuite.Common.Rtsp
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Each substream stream represents a single track in an RTSP stream. The substream handles
    ///     the setup and decoding of media streams and passes them to the UI thread for playback.
    /// </summary>
    /// -------------------------------------------------------------------------------------------------
    public class SubStreamDescriptor
    {
        public delegate void OnAudioFrameEvent(SubStreamDescriptor descriptor, AudioMediaBuffer frame, bool isTerminated);

        public delegate void VideoDecoderProcessWorker(SubStreamDescriptor descriptor, PictureMediaBuffer frame);

        // Reduce this value to decrease memory pressure
        private const int MaxCompleteFrames = 2;

        private readonly AacAccessUnitTool aacAuTool = new AacAccessUnitTool();

        private readonly LockableOffsetBufferProvider<OffsetBuffer<byte>> bufferProvider = new LockableOffsetBufferProvider<OffsetBuffer<byte>>();

        private readonly SmartJitterBuffer<RtpFrame> jitterBuffer = new SmartJitterBuffer<RtpFrame>(SmartJitterStrategy.Ignore, TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(250));

        private readonly MediaBuffer<byte> mp4InitBuffer = null;

        private readonly CountdownEvent processingThreadCountDownEvent = new CountdownEvent(1);

        // Note: Please make sure you copy a accelerated plugins to the execution path
        private readonly ManualResetEventSlim processingThreadSeekIndicationEvent = new ManualResetEventSlim(true);
        public readonly  RtspSubStream        SubStream;

        private int                       aacFrameSize = 1024;
        private AudioDecoderConfiguration audioConfig;
        private IAudioDecoderBase         audioDecoder;

        private BlockingCollection<MediaBuffer<byte>> completedFrames = new BlockingCollection<MediaBuffer<byte>>(MaxCompleteFrames);
        private Thread                                decoderThread;
        public  DeliveryMethod                        DeliveryMethod;

        public  LogDelegate          Log;
        private UPnpMappingOperation mapOpRtcp;
        private UPnpMappingOperation mapOpRtp;
        private TrackInfo            mp4TrackInfo;
        public  Mp4Writer            Mp4Writer = null;
        public  OnAudioFrameEvent    OnAudioFrame;

        private CancellationTokenSource            operationCancellation = new CancellationTokenSource();
        private IRtpParticipant                    participant;
        private PlayoutBuffer                      playoutBuffer;
        private int                                presentationBufferMaximumDepth = 10;
        public  VideoDecoderProcessWorker          PresentationProcessor;
        private Thread                             presentationThread;
        private TimeSpan                           previousPresentationTime = TimeSpan.Zero;
        public  MediaContentType                   PrimaryContentType;
        private Thread                             rtpFrameProcessorThread;
        public  bool                               SaveToFile;
        private int                                udpMediaPort;
        public  bool                               UseUpnP = false;
        private VideoDecoderConfiguration          videoConfig;
        private VideoDecoderTransformContextQueued videoDecoder;

        public SubStreamDescriptor(RtspSubStream sub)
        {
            IsRunning                              = false;
            SubStream                              = sub;
            SubStream.Configuration.MaintainJitter = false;
        }

        public  long DecodedFrames      { get; private set; }
        private bool IsRunning          { get; set; }
        public  long RtpPacketsReceived { get; private set; }

        ~SubStreamDescriptor()
        {
            Log?.Invoke("SubStreamDescriptor destructed.");
        }

        private void participant_OnRtcpReceive(object sender, RtcpPacketEventArgs e)
        {
            ProcessRtcpPacket(e.Packet);
        }

        private void ProcessRtcpPacket(RtcpCompoundPacket packet)
        {
            if (packet.Contains(RtcpType.SenderReport))
            {
                var senderReport = packet.Find<RtcpSenderReport>(RtcpType.SenderReport);
                if (senderReport != null)
                {
                    if (SubStream.RtspTrackInfo.SyncSource == senderReport.Ssrc && SubStream.RtspTrackInfo.SyncSourceProvided)
                    {
                        playoutBuffer.Update(senderReport);
                    }

                    if (DeliveryMethod == DeliveryMethod.Unicast)
                    {
                        var unicastParticipant = participant as RtpParticipant<UdpNetworkClient>;
                        var report             = unicastParticipant.ReceiverInformation.GenerateReceiverReport();

                        var sdes = new RtcpSourceDescription();
                        report.AddPacket(sdes);
                        sdes.AddChunk();

                        participant.Send(report);
                    }
                }
            }
        }

        private void participant_OnRtpReceive(object sender, RtpPacketEventArgs e)
        {
            ProcessRtpPacket(e.RtpPacket);
        }

        // This function takes the rtp packets from any transport and adds them to the jitter. The
        // jitter will create an RTPFrame and calculate the playout times based on RTP-Info and RTCP-
        // SR. 
        private void ProcessRtpPacket(RtpPacket packet)
        {
            // Discard any packets during a seek operation
            if (processingThreadSeekIndicationEvent.Wait(0) == false)
            {
                return;
            }

            if (packet.Ssrc != SubStream.RtspTrackInfo.SyncSource && SubStream.RtspTrackInfo.SyncSourceProvided || SubStream.RtspTrackInfo.PayloadType != packet.PayloadType)
            {
                return;
            }

            ++RtpPacketsReceived;

            if (playoutBuffer.HasReceivedRtpInfo == false)
            {
                if (SubStream.RtspTrackInfo.InfoHeader != null)
                {
                    playoutBuffer.Update(SubStream.RtspTrackInfo.TrackUrl, SubStream.RtspTrackInfo.InfoHeader);
                }
                else
                {
                    var info = new RtpInfo(new RtpInfoStreamUrl
                    {
                        Url      = SubStream.RtspTrackInfo.TrackUrl,
                        RtpTime  = packet.Timestamp,
                        Sequence = packet.SequenceNumber
                    });

                    playoutBuffer.Update(SubStream.RtspTrackInfo.TrackUrl, info);

                    Log?.Invoke("RTP-Info header not available.");
                }
            }

            switch (SubStream.RtspTrackInfo.Codec)
            {
                case Codec.H264:
                    jitterBuffer.AddPacket<H264Frame>(packet);
                    break;
                case Codec.H265:
                    jitterBuffer.AddPacket<H265Frame>(packet);
                    break;
                case Codec.Mpeg4V:
                    jitterBuffer.AddPacket<RtpFrame>(packet);
                    break;
                case Codec.Jpeg:
                    jitterBuffer.AddPacket<JpegFrame>(new RtpPacketJpeg(packet));
                    break;
                case Codec.Aac:
                case Codec.Mp3:
                case Codec.G711U:
                case Codec.G711A:
                case Codec.G726:
                case Codec.L16:
                case Codec.Mjpeg:
                    jitterBuffer.AddPacket(packet);
                    break;
                default:
                    Log?.Invoke("Caution. Unsupported codec selected.");
                    break;
            }
        }

        private void sub_OnInterleaveData(RtspSubStream sender, InterleaveSlice slice)
        {
            lock (this)
            {
                // RTP & RTCP 
                if (slice.ChannelId == sender.RtspTrackInfo.ChannelId)
                {
                    var rtpPacket = sender.CreateRtpFromSlice(slice);
                    if (SubStream.RtspTrackInfo.PayloadType == rtpPacket.PayloadType)
                    {
                        ProcessRtpPacket(rtpPacket);
                    }
                }
                else if (slice.ChannelId == sender.RtspTrackInfo.ChannelId + 1)
                {
                    ProcessRtcpPacket(sender.CreateRtcpFromSlice(slice));
                }
            }
        }


        private void WriteMp4Data(MediaBuffer<byte> buffer)
        {
            if (SaveToFile == false)
            {
                return;
            }

            lock (Mp4Writer)
            {
                if (Mp4Writer != null) {
                    try
                    {
                        var duration = (buffer.EndTime - buffer.StartTime);
                        buffer.StartTime = InitialMp4TrackVideoOffset;
                        buffer.EndTime = duration + InitialMp4TrackVideoOffset;
                        Mp4Writer.WriteTrack(mp4TrackInfo, buffer);
                        InitialMp4TrackVideoOffset = buffer.EndTime;
                    }
                    catch (Exception) { }

                }
            }
        }

        private void WriteMp4Data(IEnumerable<MediaBuffer<byte>> collection)
        {
            foreach (var mb in collection)
            {
                WriteMp4Data(mb);
            }
        }

        public void Attach()
        {
            playoutBuffer = new PlayoutBuffer(SubStream.RtspTrackInfo.Media.SampleFrequency) {
                PresentationBuffer = new PresentationBuffer(presentationBufferMaximumDepth) {
                    MaximumReorderDelay = presentationBufferMaximumDepth
                }
            };
            jitterBuffer.SetPlayoutBuffer(playoutBuffer);

            if (DeliveryMethod == DeliveryMethod.Unicast)
            {
                // if UPnP is available and the unicast address is external, we can use it to map UDP ports on the router.

                if (UseUpnP)
                {
                    if (Singleton<UPnpPortMap>.Instance.GetServiceEndpoints().Count != 0)
                    {
                        var result    = Singleton<UPnpPortMap>.Instance.GetServiceEndpoints();
                        var serviceEp = result[0];

                        var hostName  = Dns.GetHostName();
                        var addresses = Dns.GetHostAddresses(hostName);

                        var thisAddress = addresses.FirstOrDefault(address => address.AddressFamily == AddressFamily.InterNetwork);

                        if (thisAddress == null)
                        {
                            Log?.Invoke("No local interface found for port mapping.");
                            return;
                        }

                        mapOpRtp = new UPnpMappingOperation
                        {
                            ExternalPort    = udpMediaPort,
                            InternalPort    = udpMediaPort,
                            InternalAddress = thisAddress,
                            ServiceEndpoint = serviceEp,
                            Protocol        = ProtocolType.Udp,
                            MetaDescription = "RTSP Player RTP"
                        };
                        Log?.Invoke($"UPnP RTP forward {mapOpRtp}");
                        Singleton<UPnpPortMap>.Instance.Map(mapOpRtp);

                        mapOpRtcp = new UPnpMappingOperation
                        {
                            ExternalPort    = udpMediaPort + 1,
                            InternalPort    = udpMediaPort + 1,
                            InternalAddress = thisAddress,
                            ServiceEndpoint = serviceEp,
                            Protocol        = ProtocolType.Udp,
                            MetaDescription = "RTSP Player RTCP"
                        };

                        Log?.Invoke($"UPnP RTCP forward {mapOpRtcp}");

                        Singleton<UPnpPortMap>.Instance.Map(mapOpRtcp);
                    }
                }
            }
            else
            {
                if (DeliveryMethod == DeliveryMethod.Multicast)
                {
                    if (SubStream.RtspTrackInfo.MulticastPresent == false)
                    {
                        throw new Exception("Multicast not available on server.");
                    }

                    var localRtpEp  = new IPEndPoint(IPAddress.Any, SubStream.RtspTrackInfo.MulticastGroupRtpEndpoint.Port);
                    var localRtcpEp = new IPEndPoint(IPAddress.Any, SubStream.RtspTrackInfo.MulticastGroupRtcpEndpoint.Port);

                    participant = new RtpParticipant<UdpNetworkMulticastClient>(new RtpParticipantInfo
                    {
                        Rtp  = localRtpEp,
                        Rtcp = localRtcpEp
                    }, new RtpParticipantInfo
                    {
                        Rtp  = SubStream.RtspTrackInfo.MulticastGroupRtpEndpoint,
                        Rtcp = SubStream.RtspTrackInfo.MulticastGroupRtcpEndpoint
                    });
                    participant.RtpReceive  += participant_OnRtpReceive;
                    participant.RtcpReceive += participant_OnRtcpReceive;

                    participant.Start();
                }
                else
                {
                    if (DeliveryMethod == DeliveryMethod.TcpInterleave || DeliveryMethod == DeliveryMethod.RtspOverHttp)
                    {
                        SubStream.OnInterleaveData += sub_OnInterleaveData;
                    }
                } // Start processor threads
            }

            IsRunning = true;

            if (PrimaryContentType == MediaContentType.Video)
            {
                decoderThread = new Thread(VideoDecoderThreadProc)
                {
                    Name = nameof(VideoDecoderThreadProc)
                };

                presentationThread = new Thread(PresentationThreadProc)
                {
                    Name = nameof(PresentationThreadProc)
                };
                presentationThread.Start();

                rtpFrameProcessorThread = new Thread(RtpFrameProcessorThreadProc)
                {
                    Name = nameof(RtpFrameProcessorThreadProc)
                };
                rtpFrameProcessorThread.Start();
            }
            else
            {
                decoderThread = new Thread(AudioDecoderThreadProc)
                {
                    Name = nameof(AudioDecoderThreadProc)
                };
            }

            decoderThread.Start();
        }

        private void PresentationThreadProc()
        {
            processingThreadCountDownEvent.AddCount();
            while (IsRunning)
            {
                BlockIfSeeking();
                MediaBuffer<byte> frame = null;
                try
                {
                    if (playoutBuffer.PresentationBuffer.OutputQueue.TryTake(out frame, Timeout.Infinite, operationCancellation.Token) == false)
                    {
                        continue;
                    }
                }
                catch (OperationCanceledException)
                {
                    Log?.Invoke($"{Thread.CurrentThread.Name} operation cancelled.");
                    continue;
                }

                PresentationProcessor?.Invoke(this, frame as PictureMediaBuffer);
            }

            Log?.Invoke($"{Thread.CurrentThread.Name} terminated.");
        }

        private void AudioDecoderThreadProc()
        {
            var previousPresentationTimeSet = false;
            var previousTotalSampleInstant  = 0.0;
            processingThreadCountDownEvent.AddCount();
            while (IsRunning)
            {
                BlockIfSeeking();
                try
                {
                    if (jitterBuffer.ProcessFrameCompletion().TryTake(out var nextFrame, 50, operationCancellation.Token) == false)
                    {
                        continue;
                    }

                    if (previousPresentationTimeSet == false)
                    {
                        previousPresentationTime    = nextFrame.RelativePresentationTime;
                        previousPresentationTimeSet = true;
                    }

                    var totalSampleInstant = 0.0;
                    while (nextFrame.PacketCount > 0 && IsRunning)
                    {
                        var              p   = nextFrame.GetNextPacket();
                        AudioMediaBuffer pcm = null;

                        var predictedDuration = 0.0;

                        switch (SubStream.RtspTrackInfo.Codec)
                        {
                            case Codec.Aac:
                                if (SaveToFile)
                                {
                                    var tempMb = new MediaBuffer<byte>(p.DataPointer) {
                                        StartTime = nextFrame.RelativePresentationTime
                                    };
                                    var writableMb = aacAuTool.ExpandAu(tempMb, aacFrameSize);
                                    WriteMp4Data(writableMb);
                                }

                                predictedDuration = aacAuTool.GetNumberOfFramesInAu(p.DataPointer) * aacFrameSize / (double) audioConfig.Samplerate;
                                pcm = audioDecoder.Transform(new MediaBuffer<byte>(p.DataPointer, nextFrame.RelativePresentationTime,
                                          nextFrame.RelativePresentationTime + TimeSpan.FromMilliseconds(predictedDuration))) == CodecOperationStatus.Success
                                    ? audioDecoder.OutputQueue.Take() as AudioMediaBuffer
                                    : null;
                                break;
                            case Codec.Mp3:
                                var mp3Stream = p.DataPointer.ToMemoryStream();
                                while (true)
                                {
                                    var frame = Mp3StorageFrame.CreateFrom(mp3Stream);
                                    if (frame == null)
                                    {
                                        break;
                                    }

                                    if (audioDecoder == null)
                                    {
                                        var config = new Mp3AudioDecoderConfiguration
                                        {
                                            Samplerate    = frame.Samplerate,
                                            Channels      = frame.ChannelMode == StereoMode.Mono ? 1 : 2,
                                            BitsPerSample = 16
                                        };

                                        audioDecoder = new MP3Decoder();
                                        audioConfig  = audioDecoder.Init(config);
                                        if (audioConfig.InitializationStatus != CodecOperationStatus.Success)
                                        {
                                            audioDecoder = null;
                                        }
                                    }

                                    var mb = new MediaBuffer<byte>(frame.RawData)
                                    {
                                        StartTime = p.RelativePresentationTime,
                                        EndTime   = p.RelativePresentationTime + (frame.SampleCount / (double) frame.Samplerate).SecondsToTimeSpanAccurate()
                                    };
                                    pcm = audioDecoder.Transform(mb) == CodecOperationStatus.Success ? audioDecoder.OutputQueue.Take() as AudioMediaBuffer : null;

                                    if (pcm == null)
                                    {
                                        continue;
                                    }

                                    OnAudioFrame?.Invoke(this, pcm, false);
                                }

                                continue;
                            case Codec.G711A:
                            case Codec.G711U:
                                if (SaveToFile)
                                {
                                    var tempMb = new MediaBuffer<byte>(p.DataPointer) {
                                        StartTime = nextFrame.RelativePresentationTime
                                    };
                                    tempMb.EndTime   = tempMb.StartTime + (p.DataSize / (double) audioConfig.Samplerate).SecondsToTimeSpanAccurate();
                                    WriteMp4Data(tempMb);
                                }

                                predictedDuration = p.DataSize / (double) audioConfig.Samplerate * audioConfig.Channels;
                                pcm = audioDecoder.Transform(new MediaBuffer<byte>(p.DataPointer)) == CodecOperationStatus.Success
                                    ? audioDecoder.OutputQueue.Take() as AudioMediaBuffer
                                    : null;
                                break;
                            case Codec.G726:
                                pcm = audioDecoder.Transform(new MediaBuffer<byte>(p.DataPointer)) == CodecOperationStatus.Success
                                    ? audioDecoder.OutputQueue.Take() as AudioMediaBuffer
                                    : null;
                                predictedDuration = pcm.Buffer.Count / ((double) audioConfig.Samplerate * audioConfig.Channels * 2);
                                break;
                            case Codec.L16:
                                pcm = new AudioMediaBuffer(p.DataPointer);
                                for (var i = 0; i < pcm.Buffer.Count; i += 2)
                                {
                                    var a = pcm.Buffer[i];
                                    pcm.Buffer[i]     = pcm.Buffer[i + 1];
                                    pcm.Buffer[i + 1] = a;
                                }

                                predictedDuration = (pcm.Buffer.Count >> 1) / (double) audioConfig.Samplerate;
                                break;
                        }

                        var serveProducedSamples = true;
                        if (pcm != null)
                        {
                            var currentSampleInstant = pcm.Buffer.Count / (double) ((audioConfig.Channels * audioConfig.Samplerate * audioConfig.BitsPerSample) >> 3);

                            var dDecodeDuration = currentSampleInstant - predictedDuration;

                            if (Math.Abs(dDecodeDuration) > 0.01)
                            {
                                Log?.Invoke("Transform duration is not equal to predicted duration.");
                            }

                            totalSampleInstant += currentSampleInstant;
                        }
                        else
                        {
                            // If decode failed, we can handle it either as discontinuity in the next frame, or we could
                            // alternatively create silence frames for this pts. 
                            Log?.Invoke("failed to decode audio data.");
                            serveProducedSamples = false;
                        }

                        // If the decoding worked and we are still play then we serve up the new audio samples. 
                        if (serveProducedSamples)
                        {
                            OnAudioFrame?.Invoke(this, pcm, false);
                        }
                    }

                    // Since all audio is continuous, we should be able to calculate backwards to the previous
                    // timestamp, by looking at the total duration of the pcm samples and see if we have gaps. If
                    // gaps are detected, the waveout timeline must be updated so we can maintain AV lip sync. 
                    if (previousPresentationTime != nextFrame.RelativePresentationTime)
                    {
                        var calcBack      = nextFrame.RelativePresentationTime.TotalSeconds - previousTotalSampleInstant;
                        var previousDelta = calcBack - previousPresentationTime.TotalSeconds;

                        if (Math.Abs(previousDelta) > 0.00)
                        {
                            //Log?.Invoke(string.Format("Possible discontinuity detected. {0}ms, absolute error {1}", previousDelta, totalDiscontinuityError));
                        }
                    }

                    previousPresentationTime   = nextFrame.RelativePresentationTime;
                    previousTotalSampleInstant = totalSampleInstant;
                }
                catch (OperationCanceledException)
                {
                    Log?.Invoke($"{Thread.CurrentThread.Name} operation cancelled.");
                }
            }

            // Serve the audio frame to the player
            OnAudioFrame?.Invoke(this, null, true);
            Log?.Invoke($"{Thread.CurrentThread.Name} terminated.");
        }

        public void Detach()
        {
            if (SaveToFile)
            {
                Mp4Writer.EndTrack(mp4TrackInfo);
            }

            IsRunning = false;
            operationCancellation.Cancel(true);
            presentationThread?.Join();

            decoderThread?.Join();

            if (participant != null)
            {
                if (participant.IsStarted)
                {
                    var bye = new RtcpBye
                    {
                        Reason = "Stopped"
                    };
                    var cp = new RtcpCompoundPacket();
                    cp.AddPacket(bye);
                    participant.Send(cp);
                    participant.Stop();
                }

                if (DeliveryMethod == DeliveryMethod.Unicast)
                {
                    if (mapOpRtp != null)
                    {
                        Singleton<UPnpPortMap>.Instance.UnMap(mapOpRtp);
                    }

                    if (mapOpRtcp != null)
                    {
                        Singleton<UPnpPortMap>.Instance.UnMap(mapOpRtcp);
                    }
                }
            }

            videoDecoder?.Dispose();

            jitterBuffer.Clear();
            completedFrames.Dispose();
            bufferProvider.ClearAllClients();
        }

        public bool Parse()
        {
            var result = false;
            jitterBuffer.Name         =  SubStream.RtspTrackInfo.MediaContentType.ToString();
            jitterBuffer.FrameDropped += (sender, args) => Log?.Invoke($"{jitterBuffer.Name}: Frame TS {args.Frame.Timestamp} dropped with age {args.Frame.Age}.");

            switch (SubStream.RtspTrackInfo.Codec)
            {
                case Codec.Mpeg4V:
                {
                    PrimaryContentType                      = MediaContentType.Video;
                    jitterBuffer.SkipLateOrIncompleteFrames = false;
                    videoDecoder                            = Mpeg42VideoDecoderTransformContextQueued.FromInstance(new MPEG4Decoder(), operationCancellation);
                    videoConfig = videoDecoder.Init(new Mpeg42VideoDecoderConfiguration
                    {
                        DecoderSpecificData     = SubStream.RtspTrackInfo.Media.DecoderSpecificData,
                        OutputQueueCancellation = operationCancellation
                    });
                    if (videoConfig.InitializationStatus != CodecOperationStatus.Success)
                    {
                            Log?.Invoke("MPEG4VideoDecoder initialization failed.");
                    }

                    result = true;

                    if (SaveToFile)
                    {
                        mp4TrackInfo = new TrackInfo(MediaContentType.Video)
                        {
                            Codec = Codec.Mpeg4V
                        };
                        var videoTrack = new VideoTrackDescriptor
                        {
                            Bitrate             = 128000,
                            Width               = videoConfig.InputWidth,
                            Height              = videoConfig.InputHeight,
                            DecoderSpecificData = SubStream.RtspTrackInfo.Media.DecoderSpecificData
                        };
                        mp4TrackInfo.Media = videoTrack;
                        mp4TrackInfo       = Mp4Writer.AddTrack(mp4TrackInfo);
                    }
                }
                    break;
                case Codec.H265:
                    PrimaryContentType                      = MediaContentType.Video;
                    jitterBuffer.SkipLateOrIncompleteFrames = true;
                    videoDecoder =
                        VideoDecoderTransformContextQueued.FromInstance(DecoderFactory.Create(SubStream.RtspTrackInfo.Codec, MediaSuiteExtensionVendor.Nvidia), operationCancellation);

                    videoConfig = videoDecoder.Init(new H265VideoDecoderConfiguration
                    {
                        Encapsulation           = Encapsulation.StartCodes,
                        InputWidth              = SubStream.RtspTrackInfo.Media.As<VideoTrackDescriptor>().Width,
                        InputHeight             = SubStream.RtspTrackInfo.Media.As<VideoTrackDescriptor>().Height,
                        DecoderSpecificData     = SubStream.RtspTrackInfo.Media.DecoderSpecificData,
                        OutputQueueCancellation = operationCancellation
                    });
                    if (videoConfig.InitializationStatus != CodecOperationStatus.Success)
                    {
                        Log?.Invoke("H265VideoDecoder initialization failed.");
                    }

                    result = true;

                    if (SaveToFile)
                    {
                        mp4TrackInfo = new TrackInfo(MediaContentType.Video)
                        {
                            Codec = Codec.H265
                        };
                        var videoTrack = new VideoTrackDescriptor
                        {
                            Bitrate             = 128000,
                            Width               = videoConfig.InputWidth,
                            Height              = videoConfig.InputHeight,
                            DecoderSpecificData = SubStream.RtspTrackInfo.Media.DecoderSpecificData
                        };
                        mp4TrackInfo.Media = videoTrack;
                        mp4TrackInfo       = Mp4Writer.AddTrack(mp4TrackInfo);
                    }

                    break;
                case Codec.H264:
                {
                    PrimaryContentType                      = MediaContentType.Video;
                    jitterBuffer.SkipLateOrIncompleteFrames = true;

                    videoDecoder = H264VideoDecoderTransformContextQueued.FromInstance(DecoderFactory.Create(SubStream.RtspTrackInfo.Codec, MediaSuiteExtensionVendor.Nvidia, @".\"),
                        operationCancellation);
                    videoConfig = videoDecoder.Init(new H264VideoDecoderConfiguration
                    {
                        Encapsulation           = Encapsulation.StartCodes,
                        Scraping                = true,
                        HardwareAccelerated     = true,
                        DecoderSpecificData     = SubStream.RtspTrackInfo.Media.DecoderSpecificData,
                        GpuDeviceId             = -1,
                        OutputQueueCancellation = operationCancellation
                    });
                    if (videoConfig.InitializationStatus != CodecOperationStatus.Success)
                    {
                        Log?.Invoke("H264VideoDecoder initialization failed.");
                    }

                    if (videoConfig.DecoderSpecificData != null)
                    {
                        // Configure the PresentationBuffer depth
                        var parameterset = new H264ParameterSet(videoConfig.DecoderSpecificData.ToStringRepresentation(DecoderSpecificDataOutputType.Stream)).GetMetrics();
                        presentationBufferMaximumDepth = (int) parameterset.NumberOfReferenceFrames;
                    }

                    result = true;

                    if (SaveToFile)
                    {
                        mp4TrackInfo = new TrackInfo(MediaContentType.Video)
                        {
                            Codec = Codec.H264
                        };
                        var videoTrack = new VideoTrackDescriptor
                        {
                            Bitrate             = 128000,
                            Width               = videoConfig.InputWidth,
                            Height              = videoConfig.InputHeight,
                            DecoderSpecificData = SubStream.RtspTrackInfo.Media.DecoderSpecificData
                        };
                        mp4TrackInfo.Media = videoTrack;
                        mp4TrackInfo       = Mp4Writer.AddTrack(mp4TrackInfo);
                    }
                }
                    break;

                case Codec.Jpeg:
                {
                    PrimaryContentType                      = MediaContentType.Video;
                    jitterBuffer.SkipLateOrIncompleteFrames = false;

                    videoDecoder = VideoDecoderTransformContextQueued.FromInstance(new JPegDecoder(), operationCancellation);

                    videoConfig = videoDecoder.Init(new JPegDecoderConfiguration());
                    if (videoConfig.InitializationStatus != CodecOperationStatus.Success)
                    {
                        Log?.Invoke("JPegDecoder initialization failed.");
                    }

                    result = true;
                }
                    break;

                case Codec.Aac:
                {
                    PrimaryContentType                      = MediaContentType.Audio;
                    jitterBuffer.SkipLateOrIncompleteFrames = false;

                    audioDecoder = new AACDecoder();
                    audioConfig = new AacAudioDecoderConfiguration
                    {
                        Channels            = SubStream.RtspTrackInfo.Media.As<AudioTrackDescriptor>().Channels,
                        BitsPerSample       = 16,
                        Samplerate          = SubStream.RtspTrackInfo.Media.SampleFrequency,
                        Encapsulation       = Encapsulation.AccessUnit,
                        DecoderSpecificData = SubStream.RtspTrackInfo.Media.DecoderSpecificData
                    };

                    audioConfig = audioDecoder.Init(audioConfig);
                    if (audioConfig.InitializationStatus != CodecOperationStatus.Success)
                    {
                        Log?.Invoke("AACDecoder initialization failed.");
                    }

                    result = true;
                    if (SaveToFile)
                    {
                        var config = SubStream.RtspTrackInfo.Media.DecoderSpecificData.ToBinaryRepresentation(DecoderSpecificDataOutputType.Storage);

                        mp4TrackInfo = new TrackInfo(MediaContentType.Audio)
                        {
                            Codec = Codec.Aac
                        };
                        var audioTrack = new AudioTrackDescriptor
                        {
                            Bitrate             = 0,
                            BitsPerSample       = audioConfig.BitsPerSample,
                            SampleFrequency     = audioConfig.Samplerate,
                            Channels            = audioConfig.Channels,
                            DecoderSpecificData = new DecoderSpecificData(config)
                        };
                        mp4TrackInfo.Media = audioTrack;
                        mp4TrackInfo       = Mp4Writer.AddTrack(mp4TrackInfo);

                        aacFrameSize = (((ushort) (((ushort) config[0] << 8) | (ushort) config[1]) >> 2) & 1) == 1 ? 960 : 1024;

                        (aacFrameSize / (double) audioTrack.SampleFrequency).SecondsToTimeSpanAccurate();
                        aacAuTool.NetworkConfiguration.SampleFrequency = audioConfig.Samplerate;
                    }
                }
                    break;
                case Codec.Mp3:
                {
                    PrimaryContentType = MediaContentType.Audio;
                    result             = true;
                    audioDecoder       = null; // We have to initialize late, because the first frame will let us know what the parameters are
                }
                    break;
                case Codec.G711A:
                    PrimaryContentType = MediaContentType.Audio;

                    if (SaveToFile)
                    {
                        mp4TrackInfo = new TrackInfo(MediaContentType.Audio)
                        {
                            Codec = Codec.G711U
                        };
                        var audioTrack = new AudioTrackDescriptor
                        {
                            Bitrate         = 0,
                            BitsPerSample   = 16,
                            SampleFrequency = 8000,
                            Channels        = SubStream.RtspTrackInfo.Media.As<AudioTrackDescriptor>().Channels
                        };
                        mp4TrackInfo.Media = audioTrack;
                        mp4TrackInfo       = Mp4Writer.AddTrack(mp4TrackInfo);
                    }

                    audioDecoder = AudioDecoderFactory.CreateDecoder(Codec.G711A);
                    audioConfig = new CommonAudioDecoderConfiguration
                    {
                        Channels      = SubStream.RtspTrackInfo.Media.As<AudioTrackDescriptor>().Channels,
                        BitsPerSample = 16,
                        Samplerate    = SubStream.RtspTrackInfo.Media.SampleFrequency
                    };
                    audioConfig = audioDecoder.Init(audioConfig);

                    // G.711 might not send markers.
                    jitterBuffer.CheckMarkersForCompletion = false;
                    result                                 = true;
                    break;
                case Codec.G711U:
                    PrimaryContentType = MediaContentType.Audio;

                    if (SaveToFile)
                    {
                        mp4TrackInfo = new TrackInfo(MediaContentType.Audio)
                        {
                            Codec = Codec.G711U
                        };
                        var audioTrack = new AudioTrackDescriptor
                        {
                            Bitrate         = 0,
                            BitsPerSample   = 16,
                            SampleFrequency = 8000,
                            Channels        = SubStream.RtspTrackInfo.Media.As<AudioTrackDescriptor>().Channels
                        };
                        mp4TrackInfo.Media = audioTrack;
                        mp4TrackInfo       = Mp4Writer.AddTrack(mp4TrackInfo);
                    }

                    audioDecoder = AudioDecoderFactory.CreateDecoder(Codec.G711U);
                    audioConfig = new CommonAudioDecoderConfiguration
                    {
                        Channels      = SubStream.RtspTrackInfo.Media.As<AudioTrackDescriptor>().Channels,
                        BitsPerSample = 16,
                        Samplerate    = SubStream.RtspTrackInfo.Media.SampleFrequency
                    };
                    audioConfig = audioDecoder.Init(audioConfig);

                    // G.711 might not send markers.
                    jitterBuffer.CheckMarkersForCompletion = false;

                    result = true;
                    break;
                case Codec.G726:
                    PrimaryContentType = MediaContentType.Audio;

                    audioDecoder = AudioDecoderFactory.CreateDecoder(Codec.G726);
                    audioConfig = new CommonAudioDecoderConfiguration
                    {
                        Bitrate       = SubStream.RtspTrackInfo.Media.As<AudioTrackDescriptor>().Bitrate,
                        Channels      = SubStream.RtspTrackInfo.Media.As<AudioTrackDescriptor>().Channels,
                        BitsPerSample = 16,
                        Samplerate    = SubStream.RtspTrackInfo.Media.SampleFrequency
                    };
                    audioConfig = audioDecoder.Init(audioConfig);

                    jitterBuffer.CheckMarkersForCompletion = false;

                    result = true;

                    break;
                case Codec.Mjpeg:
                {
                    PrimaryContentType = MediaContentType.Video;
                    result             = true;
                    break;
                }
                case Codec.L16:
                    PrimaryContentType = MediaContentType.Audio;

                    audioConfig = new CommonAudioDecoderConfiguration
                    {
                        Channels      = SubStream.RtspTrackInfo.Media.As<AudioTrackDescriptor>().Channels,
                        BitsPerSample = 16,
                        Samplerate    = SubStream.RtspTrackInfo.Media.SampleFrequency
                    };

                    result     = true;
                    SaveToFile = false;
                    break;
            }
            return result;
        }

        private void VideoDecoder_TransformResultEvent(object sender, TransformContextResultCompleteEventArgs e)
        {
            if (e.Result.CodecOperationStatus != CodecOperationStatus.Success)
            {
                return;
            }

            if (playoutBuffer.PresentationBuffer.AddFrame(e.Result.Buffer) != CodecOperationStatus.Success)
            {
                e.Result.Buffer.Buffer.TryUnlock();
            }
        }

        public bool Setup()
        {
            if (DeliveryMethod == DeliveryMethod.Unicast)
            {
                var portNumber  = PortFinder.FindAvailablePort(ProtocolType.Udp, 10000, 64000, NumberParity.Even, 1);
                var localRtpEp  = new IPEndPoint(IPAddress.Any, portNumber);
                var localRtcpEp = new IPEndPoint(IPAddress.Any, portNumber + 1);

                var remoteRtpEp  = new IPEndPoint(SubStream.RtspTrackInfo.SdpSessionOwnerAddress, SubStream.RtspTrackInfo.UdpRtpServerPort);
                var remoteRtcpEp = new IPEndPoint(SubStream.RtspTrackInfo.SdpSessionOwnerAddress, SubStream.RtspTrackInfo.UdpRtcpServerPort);

                participant = new RtpParticipant<UdpNetworkClient>(new RtpParticipantInfo
                {
                    Rtp  = localRtpEp,
                    Rtcp = localRtcpEp
                }, new RtpParticipantInfo
                {
                    Rtp  = remoteRtpEp,
                    Rtcp = remoteRtcpEp
                });
                participant.RtpReceive  += participant_OnRtpReceive;
                participant.RtcpReceive += participant_OnRtcpReceive;

                participant.Start();

                udpMediaPort = participant.LocalInfo.Rtp.Port;
                Log?.Invoke(participant.LocalInfo.Rtp.ToString());
            }

            var result = SubStream.Setup(udpMediaPort, DeliveryMethod);

            if (result == false)
            {
                if (DeliveryMethod == DeliveryMethod.Unicast)
                {
                    participant.Stop();
                }
            }

            return result;
        }

        private void VideoDecoderThreadProc()
        {
            processingThreadCountDownEvent.AddCount();
            while (IsRunning)
            {
                BlockIfSeeking();
                try
                {
                    if (completedFrames.TryTake(out var nextPackage, Timeout.Infinite, operationCancellation.Token) == false)
                    {
                        continue;
                    }

                    videoDecoder.Transform(nextPackage);

                    while (videoDecoder.OutputQueue.Count > 0)
                    {
                        var picture = videoDecoder.OutputQueue.Take(operationCancellation.Token);
                        ++DecodedFrames;
                        if (playoutBuffer.PresentationBuffer.AddFrame(picture, operationCancellation.Token) != CodecOperationStatus.Success)
                        {
                            picture.Buffer.TryUnlock();
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    Log?.Invoke($"{Thread.CurrentThread.Name} operation cancelled.");
                }
            }

            Log?.Invoke($"{Thread.CurrentThread.Name} terminated.");
        }

        private void RtpFrameProcessorThreadProc()
        {
            processingThreadCountDownEvent.AddCount();
            while (IsRunning)
            {
                BlockIfSeeking();
                try
                {
                    if (jitterBuffer.ProcessFrameCompletion().TryTake(out var nextFrame, 50, operationCancellation.Token) == false)
                    {
                        continue;
                    }

                    if (nextFrame.HasSequenceGaps)
                    {
                        Log?.Invoke($"Frame has gaps. (TS {(ulong) nextFrame.Timestamp}) Unable to play.");
                        continue;
                    }

                    if (SubStream.RtspTrackInfo.Codec == Codec.H265 || SubStream.RtspTrackInfo.Codec == Codec.H264 || SubStream.RtspTrackInfo.Codec == Codec.Mpeg4V ||
                        SubStream.RtspTrackInfo.Codec == Codec.Jpeg)
                    {
                        // Initialize previous presentation time, so we have a 0 offset.
                        if (previousPresentationTime == TimeSpan.Zero)
                        {
                            previousPresentationTime = nextFrame.RelativePresentationTime;
                        }

                        var duration = TimeSpan.FromSeconds(Math.Max(nextFrame.RelativePresentationTime.TotalSeconds, previousPresentationTime.TotalSeconds) -
                                                            Math.Min(nextFrame.RelativePresentationTime.TotalSeconds, previousPresentationTime.TotalSeconds));

                        var mp = nextFrame.GetAssembledFrame();

                        if (mp == null)
                        {
                            continue;
                        }

                        mp.EndTime = nextFrame.RelativePresentationTime + duration;

                        if (SaveToFile)
                        {
                            WriteMp4Data(mp);
                        }

                        lock (completedFrames)
                        {
                            completedFrames.Add(mp, operationCancellation.Token);
                        }

                        previousPresentationTime = nextFrame.RelativePresentationTime;
                    }
                }
                catch (OperationCanceledException)
                {
                    Log?.Invoke($"{Thread.CurrentThread.Name} operation cancelled.");
                }
            }

            Log?.Invoke($"{Thread.CurrentThread.Name} terminated.");
        }

        public void WriteMp4InitBuffer()
        {
            if (mp4InitBuffer == null)
            {
                return;
            }

            WriteMp4Data(mp4InitBuffer);
        }

        /// <summary>
        ///     This method effectively blocks the "processing" threads during a seek operation
        ///     so that queues can be emptied.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void BlockIfSeeking()
        {
            if (processingThreadSeekIndicationEvent.Wait(0))
            {
                return;
            }

            processingThreadCountDownEvent.Signal();
            if (processingThreadSeekIndicationEvent.Wait(0) == false)
            {
                Log?.Invoke($"{Thread.CurrentThread.Name} is blocked.");
            }
            else
            {
                Log?.Invoke($"{Thread.CurrentThread.Name} is NOT blocked.");
            }

            processingThreadSeekIndicationEvent.Wait();
            processingThreadCountDownEvent.AddCount();
            Log?.Invoke($"{Thread.CurrentThread.Name} resuming.");
        }

        public void Seek(RangeParameters rangeParameters)
        {
            processingThreadSeekIndicationEvent.Reset();
            operationCancellation.Cancel(true);
            processingThreadCountDownEvent.Signal();
            processingThreadCountDownEvent.Wait(); // Wait for all processing threads to align 
            lock (completedFrames)
            {
                previousPresentationTime = TimeSpan.Zero;
                completedFrames          = new BlockingCollection<MediaBuffer<byte>>(MaxCompleteFrames);

                jitterBuffer.Clear();
                playoutBuffer.Reset();
                playoutBuffer.PresentationBuffer.Clear();
                videoDecoder?.Dispose();
                audioDecoder?.Dispose();
                operationCancellation = new CancellationTokenSource();
                Parse();
                playoutBuffer.PlayoutStartTime = rangeParameters.FromTime.TotalSeconds;
            }

            processingThreadSeekIndicationEvent.Set();
            processingThreadCountDownEvent.Reset();
        }

        public override string ToString()
        {
            return
                $"{SubStream.RtspTrackInfo.Codec}: Packets Recv: {RtpPacketsReceived}, Jitter-Dropped: {jitterBuffer.TotalDroppedFramesCount}, Jitter-Completed: {jitterBuffer.TotalCompleteCount} Queued Frames: {completedFrames.Count} Decoded Frames: {DecodedFrames}";
        }

        public TrackInfo Mp4TrackInfo {
            get {
                return this.mp4TrackInfo;
            }
            set {
                this.mp4TrackInfo = value;
            }
        }
        public TimeSpan InitialMp4TrackVideoOffset { get; set; } = TimeSpan.Zero;

    }
}