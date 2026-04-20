using ElipgoVideo.Commons.Enums;

namespace ElipgoVideo.Commons.Data
{
    public class CameraData
    {
        public CameraData(int finalOffset)
        {
            this.CameraState = CameraState.Active;
            this.FinalOffset = finalOffset;
        }

        public CameraState CameraState { get; set; }

        public int FinalOffset { get; set; }

        public int Gov { get; set; }
    }
}
