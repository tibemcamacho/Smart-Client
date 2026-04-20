using System;
using System.Collections.Generic;
using System.Net.Sockets;
using StreamCoders;
using StreamCoders.Helpers;
using StreamCoders.Network;
using StreamCoders.Rtsp;
using StreamCoders.Rtsp.Extensions;

namespace MediaSuite.Common.Rtsp
{
    public class RtspClient
    {
        public LogDelegate Log;
        private readonly RtspClientConfiguration   configuration;
        private          RtspPlayerSession         playerSession;
        private          List<SubStreamDescriptor> rtspDescriptors;

        public RtspClient(RtspClientConfiguration configuration)
        {
            this.configuration = configuration;

            if (configuration.UPnpEnabled)
            {
                Singleton<UPnpPortMap>.Instance.GetServiceEndpointsAsync().ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Opens an RTSP endpoint and attempts to discover server side capabilities.
        /// </summary>
        /// <returns></returns>
        public RtspClientOpenResult Open()
        {
            var             iterations = 0;
            DiscoveryReport report     = null;
            // Iterate at most twice, to be able to handle possible redirects
            while (iterations < 2)
            {
                playerSession = new RtspPlayerSession(configuration.PlayerConfiguration) {
                    Log         = configuration.Log,
                    SaveToFile  = configuration.SaveToFile,
                    Mp4FileName = configuration.Mp4FileName
                };


                try
                {
                    report = playerSession.Discover(configuration.DeliveryMethod);
                }
                catch (SocketException)
                {
                    playerSession = null;
                    return RtspClientOpenResult.CreateFailed();
                }

                // Something went wrong. Let's check response codes.
                if (report.Successful == false)
                {
                    // See if we were even able to establish a connection.
                    if (report.TransportEstablished == false)
                    {
                        Log?.Invoke("Unable to connect.");
                        break;
                    }

                    // Check if we even got a response.
                    if (report.Transaction.Response == null)
                    {
                        break;
                    }

                    // If we did get a response, check if it was perhaps a redirect.
                    var statusCode = playerSession.DiscoveryReport.Transaction.Response.ResponseLine.ResponseCode;
                    if (statusCode == 301 || statusCode == 302)
                    {
                        var locationHeader = playerSession.DiscoveryReport.Transaction.Response.FindHeader(HeaderType.Location);
                        if (locationHeader == null)
                        {
                            break;
                        }

                        configuration.PlayerConfiguration.ResourceUri = (locationHeader as Location).Url.ToString();
                    }
                    else
                    {
                        if (statusCode > 400)
                        {
                           Log?.Invoke($"Unable to proceed: {statusCode} - {ResponseHeader.GetResponseCodeText(statusCode)}");
                        }

                        break;
                    }
                }
                else
                {
                    break;
                }

                ++iterations;
            }

            rtspDescriptors = playerSession.ParseSession();

            if (rtspDescriptors == null)
            {
                playerSession.Teardown();
                playerSession = null;
                return RtspClientOpenResult.CreateFailed();
            }

            try
            {
                foreach (var descriptor in rtspDescriptors)
                {
                    descriptor.DeliveryMethod = configuration.DeliveryMethod;
                    if (descriptor.Setup())
                    {
                        descriptor.PresentationProcessor = configuration.VideoFrameProcessor;
                        descriptor.UseUpnP               = configuration.UPnpEnabled;

                        descriptor.Attach();

                        if (descriptor.PrimaryContentType == MediaContentType.Audio)
                        {
                            descriptor.OnAudioFrame += configuration.OnAudioFrame;
                        }
                    }
                    else
                    {
                        if (descriptor.DeliveryMethod == DeliveryMethod.TcpInterleave && descriptor.SubStream.RtspTrackInfo.InterleavePresent == false)
                        {
                            Log?.Invoke("TCP interleaving is not supported by the server.");
                        }
                        else
                        {
                            if (descriptor.DeliveryMethod == DeliveryMethod.Multicast && descriptor.SubStream.RtspTrackInfo.MulticastPresent == false)
                            {
                                Log?.Invoke("Multicast is not supported by the server.");
                            }
                            else
                            {
                                Log?.Invoke("Setup failed.");
                            }
                        }

                        Teardown();
                        return RtspClientOpenResult.CreateFailed();
                    }
                }
            }
            catch (Exception)
            {
                Teardown();
                return RtspClientOpenResult.CreateFailed();
            }

            playerSession.DiscoveryReport.Session.DisconnectOccured += Session_DisconnectOccured;

            return RtspClientOpenResult.CreateSuccess(report);
        }

        /// <summary>
        ///     Plays a session starting from a specified time range.
        /// </summary>
        /// <param name="rangeParameters">Optional range parameters. If null, then the session will assume from a zero position.</param>
        public void Play(RangeParameters rangeParameters = null)
        {
            playerSession.Play(rangeParameters);
        }

        /// <summary>
        ///     Seeks the RTSP session to a new position.
        /// </summary>
        /// <param name="rangeParameters">Range or position to seek to</param>
        /// <returns>Returns the effective new position of the stream.</returns>
        public RangeParameters Seek(RangeParameters rangeParameters)
        {
            return playerSession.Seek(rangeParameters);
        }

        /// <summary>
        ///     Tears down a session and finalizes descriptors.
        /// </summary>
        private void Teardown()
        {
            if (playerSession == null)
            {
                return;
            }

            playerSession.Teardown();
            playerSession = null;
            foreach (var descriptor in rtspDescriptors)
            {
                descriptor.PresentationProcessor = null;
                descriptor.OnAudioFrame          = null;
            }

            rtspDescriptors.Clear();
            rtspDescriptors = null;
        }

        public void Stop()
        {
            Teardown();
            playerSession = null;
        }

        /// <summary>
        ///     Gets a SubStreamDescriptor with a specific MediaContentType
        /// </summary>
        /// <param name="mediaContentType">The MediaContentType of the SubStreamDescriptor</param>
        /// <returns>If successful the descriptor is returned, otherwise false.</returns>
        public SubStreamDescriptor GetDescriptor(MediaContentType mediaContentType)
        {
            return playerSession.GetDescriptor(mediaContentType);
        }

        private void Session_DisconnectOccured(object sender, TransportOperationCompleteEventArgs e)
        {
            Stop();
        }
    }
}