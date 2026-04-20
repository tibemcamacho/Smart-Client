
using StreamCoders;
using StreamCoders.Container;
using StreamCoders.Rtsp;
using StreamCoders.Rtsp.Extensions;
using System;
using System.Threading;

namespace Elipgo.SmartClient.Drivers.Rtsp.RtspPlayerControl
{
    public enum CameraCaptureStatus
    {
        Connected,
        Disconnected,
        Stopped,
        Error
    }

    class RtspCameraCapture
    {
        public delegate void NewVideoFrameEventHandler(PictureMediaBuffer e);
        public event NewVideoFrameEventHandler NewVideoFrame;
        public delegate void NewAudioFrameEventHandler(AudioMediaBuffer e);
        public event NewAudioFrameEventHandler NewAudioFrame;
        public delegate void ErrorEventHandler(string error);
        public event ErrorEventHandler Error;

        protected virtual void OnNewVideoFrame(PictureMediaBuffer frame)
        {
            NewVideoFrame?.Invoke(frame);
        }
        protected virtual void OnNewAudioFrame(AudioMediaBuffer frame)
        {
            NewAudioFrame?.Invoke(frame);
        }

        private Thread _rtspClientThread;

        private MediaSuite.Common.Rtsp.RtspClient _rtspClient;
        private string _cameraId;
        private string _captureUri;
        private string _cameraUser;
        private string _cameraPassword;
        private bool _useProxy;
        private string _proxyUrl;
        private bool _tcpInterleaved;
        private bool _keepAlive;

        private bool saveToFile;
        private bool mp4Initialized = false;
        private TrackInfo mp4TrackInfo;
        public Mp4Writer mp4Writer = null;

        public CameraCaptureStatus Status { get; set; }

        public void Init(string cameraId, string captureUri, string cameraUser, string cameraPassword, bool useProxy, string proxyUrl, bool tcpInterleaved, bool keepAlive)
        {
            _rtspClientThread = new Thread(new ThreadStart(RunRtspClient));

            _cameraId = cameraId;
            _captureUri = captureUri;
            _cameraUser = cameraUser;
            _cameraPassword = cameraPassword;
            _useProxy = useProxy;
            _proxyUrl = proxyUrl;
            _tcpInterleaved = tcpInterleaved;
            _keepAlive = keepAlive;
        }

        public void Start()
        {
            _rtspClientThread.Start();
        }

        private void RunRtspClient()
        {
            try
            {
                Status = CameraCaptureStatus.Disconnected;
                var clientConfiguration = new MediaSuite.Common.Rtsp.RtspClientConfiguration
                {
                    VideoFrameProcessor = VideoFrameProcessor,
                    OnAudioFrame = PlayoutAudio,
                    DeliveryMethod = DeliveryMethod.TcpInterleave,
                    Mp4FileName = null,
                    Log = Log,
                    UPnpEnabled = false,
                    PlayerConfiguration = new MediaSuite.Common.Rtsp.RtspPlayerConfiguration
                    {
                        ResourceUri = _captureUri,
                        Username = _cameraUser,
                        Password = _cameraPassword
                    }
                };

                _rtspClient = new MediaSuite.Common.Rtsp.RtspClient(clientConfiguration);
                var openResult = _rtspClient.Open();
                if (openResult.Success)
                {
                    Status = CameraCaptureStatus.Connected;
                    _rtspClient.Play(RangeParameters.DefaultZeroInterval);
                }
            }
            catch (ThreadAbortException aborted)
            {
                _rtspClient?.Stop();
                Status = CameraCaptureStatus.Error;
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        private void Log(string str)
        {
            Error(str);
        }

        private void PlayoutAudio(MediaSuite.Common.Rtsp.SubStreamDescriptor descriptor, AudioMediaBuffer frame, bool isTerminated)
        {
            if (isTerminated)
            {
                return;
            }

            var audioDescriptor = _rtspClient.GetDescriptor(MediaContentType.Audio);
            if (audioDescriptor != null)
            {
                OnNewAudioFrame(frame);
            }
        }

        private void VideoFrameProcessor(MediaSuite.Common.Rtsp.SubStreamDescriptor descriptor, PictureMediaBuffer frame)
        {
            OnNewVideoFrame(frame);

            if (saveToFile)
            {
                if (mp4Initialized == false)
                {
                    descriptor.WriteMp4InitBuffer();

                    switch (descriptor.SubStream.RtspTrackInfo.Codec)
                    {
                        case Codec.H264:
                            mp4TrackInfo = new TrackInfo(MediaContentType.Video)
                            {
                                Codec = Codec.H264
                            };
                            var videoTrack = new VideoTrackDescriptor
                            {
                                Bitrate = 128000,
                                Width = frame.Width,
                                Height = frame.Height,
                                DecoderSpecificData = descriptor.SubStream.RtspTrackInfo.Media.DecoderSpecificData
                            };
                            mp4TrackInfo.Media = videoTrack;
                            mp4TrackInfo = mp4Writer.AddTrack(mp4TrackInfo);
                            descriptor.Mp4TrackInfo = mp4TrackInfo;
                            break;
                    }
                    mp4Initialized = true;
                    descriptor.InitialMp4TrackVideoOffset = TimeSpan.Zero;
                    descriptor.Mp4Writer = mp4Writer;
                }
            }
            descriptor.SaveToFile = saveToFile;

        }

        public void Stop()
        {
            Status = CameraCaptureStatus.Disconnected;
            _rtspClientThread.Abort();
        }

        public CameraCaptureStatus GetStatus()
        {
            return Status;
        }

        public bool StartRecordMedia(string mp4FileName)
        {
            mp4Writer = new Mp4Writer
            {
                Configuration =
                    {
                        UseFragments    = false,
                        SamplesPerChunk = 1
                    }
            };
            bool result = mp4Writer.Init(mp4FileName);
            if (result)
            {
                saveToFile = true;
                mp4Initialized = false;
            }
            return result;
        }

        public void StopRecordMedia()
        {
            if (saveToFile)
            {
                saveToFile = false;
                mp4Initialized = false;

                Thread.Sleep(10);
                mp4Writer.EndTrack(mp4TrackInfo);
                mp4Writer.Close();
                mp4Writer.Dispose();

                mp4Writer = null;
            }
        }

    }
}
