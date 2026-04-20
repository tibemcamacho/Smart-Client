using ElipgoVideo.Core.Controllers.Impl;

namespace ElipgoVideo.Core.Controllers
{
    public static class VideoControllerBuilder
    {
        private static IVideoController videoController;

        public static IVideoController GetInstance()
        {
            if (videoController == null)
            {
                videoController = new VideoControllerImpl();
            }
            return videoController;
        }
    }
}