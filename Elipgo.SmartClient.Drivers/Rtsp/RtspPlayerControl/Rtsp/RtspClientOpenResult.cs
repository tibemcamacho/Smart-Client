using StreamCoders.Rtsp.Extensions;

namespace MediaSuite.Common.Rtsp
{
    public class RtspClientOpenResult
    {
        private RtspClientOpenResult(bool success)
        {
            Success = success;
        }

        public bool            Success { get; }
        public DiscoveryReport Report  { get; private set; }

        public static RtspClientOpenResult CreateSuccess(DiscoveryReport report)
        {
            return new RtspClientOpenResult(true)
            {
                Report = report
            };
        }

        public static RtspClientOpenResult CreateFailed()
        {
            return new RtspClientOpenResult(false);
        }
    }
}