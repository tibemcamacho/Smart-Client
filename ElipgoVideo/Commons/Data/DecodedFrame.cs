using System.Drawing.Imaging;

namespace ElipgoVideo.Commons.Data
{
    public class DecodedFrame
    {
        private int width;
        private int height;
        private int stride;
        private int fps;
        private PixelFormat pixelFormat;
        private byte[] imageData;
        private long time;

        public DecodedFrame(int width, int height, int stride, PixelFormat pixelFormat, byte[] imageData)
        {
            this.Width = width;
            this.Height = height;
            this.Stride = stride;
            this.PixelFormat = pixelFormat;
            this.ImageData = imageData;
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Stride
        {
            get { return stride; }
            set { stride = value; }
        }

        public PixelFormat PixelFormat
        {
            get { return pixelFormat; }
            set { pixelFormat = value; }
        }

        public byte[] ImageData
        {
            get { return imageData; }
            set { imageData = value; }
        }

        public int Fps
        {
            get { return fps; }
            set { fps = value; }
        }

        public long Time
        {
            get { return time; }
            set { time = value; }
        }
    }
}
