using System;
using System.Collections.Generic;
using StreamCoders;
using StreamCoders.Container;
using StreamCoders.Network;
using StreamCoders.Rtsp;
using StreamCoders.Rtsp.Extensions;

namespace MediaSuite.Common.Rtsp
{
    public class RtspPlayerSession
    {
        private readonly RtspPlayerConfiguration   configuration;
        private readonly List<SubStreamDescriptor> descriptors    = new List<SubStreamDescriptor>();
        private readonly RtspSessionManager        sessionManager = new RtspSessionManager();
        public           DiscoveryReport           DiscoveryReport;
        public           LogDelegate               Log;
        private          Mp4Writer                 mp4Writer;
        public           bool                      SaveToFile = false;

        public RtspPlayerSession(RtspPlayerConfiguration configuration)
        {
            this.configuration                 = configuration;
            sessionManager.KeepAliveMethodType = MethodType.Options;
        }

        public string Mp4FileName { get; set; }

        public event EventHandler<TransportOperationCompleteEventArgs> DisconnectOccured;

        // Attempts to discover RTSP resource and returns a report describing the result. 
        public DiscoveryReport Discover(DeliveryMethod deliveryMethod)
        {
            var url = new Url(configuration.ResourceUri);

            if (deliveryMethod == DeliveryMethod.RtspOverHttp)
            {
                sessionManager.AddTransportModifier(new RtspOverHttp());
            }

            DiscoveryReport = sessionManager.Discover(url, configuration.Proxy, configuration.Username, configuration.Password);

            // If unsuccessful make sure that underlying RTSP manager terminates all threads.
            if (DiscoveryReport.Successful == false)
            {
                Log?.Invoke("Unable to discover resource.");
                sessionManager.TearDownSession();
            }
            else
            {
                sessionManager.DisconnectOccured += sessionManager_OnDisconnect;

                // See if the response contains Public: GET-PARAMETER, then we set the keep alive method to GetParameter which is usually more appropriate
                if (DiscoveryReport.Transaction.Response.FindHeader(HeaderType.Public) != null)
                {
                    var pHeader = DiscoveryReport.Transaction.Response.FindHeader(HeaderType.Public) as PublicHeader;
                    if (pHeader.Content.ToUpper().Contains("GET-PARAMETER"))
                    {
                        sessionManager.KeepAliveMethodType = MethodType.GetParameter;
                    }
                }
            }

            return DiscoveryReport;
        }

        private void sessionManager_OnDisconnect(object sender, TransportOperationCompleteEventArgs e)
        {
            var myOnDisconnect = DisconnectOccured;
            if (myOnDisconnect != null)
            {
                myOnDisconnect(this, e);
            }
        }

        // Parses session information and sets up tracks/subStream-streams.
        public List<SubStreamDescriptor> ParseSession()
        {
            if (DiscoveryReport.Session == null)
            {
                return null;
            }

            if (SaveToFile)
            {
                mp4Writer = new Mp4Writer
                {
                    Configuration =
                    {
                        UseFragments    = false,
                        SamplesPerChunk = 1
                    }
                };
            }

            SubStreamDescriptor primaryVideo = null;
            SubStreamDescriptor primaryAudio = null;

            foreach (var sub in DiscoveryReport.Session.RtspSubStreams)
            {
                if (sub.RtspTrackInfo.MediaContentType == MediaContentType.Video && primaryVideo != null)
                {
                    continue;
                }

                if (sub.RtspTrackInfo.MediaContentType == MediaContentType.Audio && primaryAudio != null)
                {
                    continue;
                }

                var descriptor = new SubStreamDescriptor(sub)
                {
                    Log = Log
                };
                if (SaveToFile)
                {
                    descriptor.SaveToFile = true;
                    descriptor.Mp4Writer  = mp4Writer;
                }

                if (descriptor.Parse())
                {
                    switch (descriptor.PrimaryContentType)
                    {
                        case MediaContentType.Video when primaryVideo == null:
                            primaryVideo                      = descriptor;
                            descriptor.SubStream.UseSubStream = true;
                            descriptors.Add(descriptor);
                            break;
                        case MediaContentType.Audio when primaryAudio == null:
                            primaryAudio                      = descriptor;
                            descriptor.SubStream.UseSubStream = true;
                            descriptors.Add(descriptor);
                            break;
                        default:
                        {
                            descriptor.Detach();
                            break;
                        }
                    }
                }
            }

            if (SaveToFile)
            {
                mp4Writer.Init(Mp4FileName);

                foreach (var descriptor in descriptors)
                {
                    descriptor.WriteMp4InitBuffer();
                }
            }

            return descriptors.Count == 0 ? null : descriptors;
        }

        /// <summary>
        ///     Gets a SubStreamDescriptor with a specific MediaContentType
        /// </summary>
        /// <param name="contentType">The MediaContentType of the SubStreamDescriptor</param>
        /// <returns>If successful the descriptor is returned, otherwise false.</returns>
        public SubStreamDescriptor GetDescriptor(MediaContentType contentType)
        {
            foreach (var descr in descriptors)
            {
                if (descr.PrimaryContentType == contentType)
                {
                    return descr;
                }
            }

            return null;
        }

        /// <summary>
        ///     Plays a session starting from a specified time range.
        /// </summary>
        /// <param name="rangeParameters">Optional range parameters. If null, then the session will assume from a zero position.</param>
        /// <returns></returns>
        public bool Play(RangeParameters rangeParameters)
        {
            if (DiscoveryReport.Session.Play(rangeParameters, out var transaction))
            {
                if (transaction.Response.FindHeader(HeaderType.Range) is Range rangeHeader)
                {
                    if (rangeHeader.ToTime == TimeSpan.Zero)
                    {
                        DiscoveryReport.MediaStreamType = MediaStreamType.Live;
                    }
                    else
                    {
                        DiscoveryReport.MediaStreamType = MediaStreamType.Vod;
                        DiscoveryReport.Duration        = rangeHeader.ToTime - rangeHeader.FromTime;
                    }
                }
                else
                {
                    DiscoveryReport.MediaStreamType = MediaStreamType.Live;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        ///     Tears down all descriptions and finally the streaming session.
        /// </summary>
        public void Teardown()
        {
            if (DiscoveryReport.Session == null)
            {
                return;
            }

            DiscoveryReport.Session.Teardown();

            foreach (var descriptor in descriptors)
            {
                descriptor.Detach();
            }

            descriptors.Clear();

            sessionManager.TearDownSession();

            if (SaveToFile)
            {
                mp4Writer.Close();
            }
        }

        /// <summary>
        ///     Calls seek on each SubStreamDescriptor and then finally seeks the session to the new position.
        /// </summary>
        /// <param name="rangeParameters">Range or position to seek to</param>
        /// <returns>Returns the new effective range of session playback</returns>
        public RangeParameters Seek(RangeParameters rangeParameters)
        {
            sessionManager.Pause();
            foreach (var descriptor in descriptors)
            {
                if (descriptor.SubStream.UseSubStream)
                {
                    descriptor.Seek(rangeParameters);
                }
            }

            sessionManager.Seek(rangeParameters, out var newRangeParameters);
            return newRangeParameters;
        }
    }
}