using StreamCoders;

namespace MediaSuite.Common.TransformContext
{
    public class TransformContextResult
    {
        public TransformContextResult(long sequenceNumber, CodecOperationStatus codecOperationStatus, MediaBuffer<byte> buffer)
        {
            SequenceNumber       = sequenceNumber;
            CodecOperationStatus = codecOperationStatus;
            Buffer               = buffer;
        }

        public long                 SequenceNumber       { get; }
        public CodecOperationStatus CodecOperationStatus { get; internal set; }
        public MediaBuffer<byte>    Buffer               { get; internal set; }
    }
}