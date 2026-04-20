using StreamCoders;
using StreamCoders.Additional.Plugins;
using System.Runtime.InteropServices;

namespace MediaSuite.Common
{
    [ComVisible(false)]
    public abstract class CodecFactory<T> where T : ICodecModule
    {
        private readonly GraphicsAdapterInformation graphicsAdapterInformation;
        private readonly MediaSuiteExtensionOperationType operationsType;
        private readonly PluginInformation<T> pluginInformation;

        internal CodecFactory(MediaSuiteExtensionOperationType operationsType, PluginInformation<T> pluginInformation, GraphicsAdapterInformation graphicsAdapterInformation)
        {
            this.operationsType = operationsType;
            this.pluginInformation = pluginInformation;
            this.graphicsAdapterInformation = graphicsAdapterInformation;
        }

        private string CodecEnumToString(Codec codec)
        {
            switch (codec)
            {
                case Codec.H264:
                    return "H264";
                case Codec.H265:
                    return "HEVC";
                default:
                    return "UNKNOWN";
            }
        }

        public virtual ITransform CreateCodec(Codec codec)
        {
            var preferredVendor = MediaSuiteExtensionVendor.Software;

            if (graphicsAdapterInformation != null)
            {
                preferredVendor = graphicsAdapterInformation.Vendor;
            }

            var module = pluginInformation.GetCodecModule(operationsType, preferredVendor, CodecEnumToString(codec));

            return module == null ? null : module.CreateInstance();
        }
    }
}
