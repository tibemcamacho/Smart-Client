using StreamCoders.Rtsp.Extensions;

namespace MediaSuite.Common.Rtsp
{
    public class RtspClientConfiguration
    {
        public SubStreamDescriptor.OnAudioFrameEvent OnAudioFrame;

        public RtspClientConfiguration()
        {
            PlayerConfiguration = new RtspPlayerConfiguration();
        }

        public bool                    UPnpEnabled         { get; set; } = false;
        public RtspPlayerConfiguration PlayerConfiguration { get; set; }

        public LogDelegate Log { get; set; }

        public bool           SaveToFile     { get; set; } = false;
        public string         Mp4FileName    { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }

        public SubStreamDescriptor.VideoDecoderProcessWorker VideoFrameProcessor { get; set; }
    }
}