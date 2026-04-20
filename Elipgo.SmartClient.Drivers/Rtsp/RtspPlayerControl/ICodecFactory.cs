using StreamCoders;

namespace MediaSuite.Common
{
    internal interface ICodecFactory
    {
        ITransform CreateCodec(Codec codec);
    }
}
