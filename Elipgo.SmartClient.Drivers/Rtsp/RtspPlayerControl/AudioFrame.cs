namespace MediaSuite.Common
{
    public class AudioFrame
    {
        public int Channels;
        public int Frequency;
        public byte[] RawBuffer;
        public double Discontinuity;
        public double TotalDiscontinuity;
    }
}
