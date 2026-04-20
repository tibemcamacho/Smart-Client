using StreamCoders;
using System;

namespace MediaSuite.Common
{
    public class VideoFrame
    {
        public int Width;
        public int Height;
        public MediaBuffer<byte> RawBuffer;
        public TimeSpan Timestamp;
    }
}
