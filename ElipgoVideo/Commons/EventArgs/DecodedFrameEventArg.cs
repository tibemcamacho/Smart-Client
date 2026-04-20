using ElipgoVideo.Commons.Data;
using System.Drawing;

namespace ElipgoVideo.Commons.EventArgs
{
    public class DecodedFrameEventArg : System.EventArgs
    {
        private Image decodedFrame;

        private DecodedFrame decodedFrameData;

        private int camId;

        private string frameTimeStamp;

        public DecodedFrameEventArg(Image decodedFrame, int camId, string frameTimeStamp)
        {
            this.decodedFrame = decodedFrame;
            this.camId = camId;
            this.frameTimeStamp = frameTimeStamp;
        }

        public DecodedFrameEventArg(DecodedFrame decodedFrame, int camId, string frameTimeStamp)
        {
            this.decodedFrameData = decodedFrame;
            this.camId = camId;
            this.frameTimeStamp = frameTimeStamp;
        }

        public Image DecodedFrame
        {
            get { return this.decodedFrame; }
        }

        public DecodedFrame DecodedFrameData
        {
            get { return this.decodedFrameData; }
        }

        public int CamId
        {
            get { return this.camId; }
        }

        public string FrameTimeStamp
        {
            get { return this.frameTimeStamp; }
        }
    }
}
