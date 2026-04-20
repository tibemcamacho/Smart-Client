using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using StreamCoders;
using StreamCoders.Additional.Plugins;
using StreamCoders.Decoder;

namespace MediaSuite.Common
{
    [ClassInterface(ClassInterfaceType.None)]
    public class DecoderFactory : CodecFactory<IVideoDecoderModule>
    {
        public DecoderFactory(PluginInformation<IVideoDecoderModule> pluginInformation, GraphicsAdapterInformation graphicsAdapterInformation) : base(MediaSuiteExtensionOperationType.Decoder,
            pluginInformation, graphicsAdapterInformation)
        {
        }

        public override ITransform CreateCodec(Codec codec)
        {
            var decoderTransform = base.CreateCodec(codec);

            return decoderTransform ?? VideoDecoderFactory.CreateDecoder(codec);
        }

        /// <summary>
        /// Creates specific codec for a specific hardware architecture from the plugin system. If that plugin is not available, the method returns a software implementation.
        /// </summary>
        /// <param name="codec">Codec ID of the implementation.</param>
        /// <param name="preferredVendor">The vendor for the platform to be used</param>
        /// <param name="extraPath">An extra path to look for the plugin</param>
        /// <returns>Returns a decoder interface instance</returns>
        public static IVideoDecoderBase Create(Codec codec, MediaSuiteExtensionVendor preferredVendor = MediaSuiteExtensionVendor.Software, string extraPath = "")
        {
            var pluginPaths = new List<string>();
            if (string.IsNullOrEmpty(extraPath) == false)
            {
                pluginPaths.Add(extraPath);
            }

            if (preferredVendor == MediaSuiteExtensionVendor.Software)
            {
                return VideoDecoderFactory.CreateDecoder(codec);
            }

            var pInfo = new PluginInformation<IVideoDecoderModule>(pluginPaths);
            var gEnum = new GraphicsEnumerator();

            var pluginAdapterPairs = new List<Tuple<IVideoDecoderModule, GraphicsAdapterInformation>>();
            foreach (var plugin in pInfo.Modules)
            {
                var existingAdapterMatch = gEnum.Devices.FirstOrDefault(p => p.Vendor == plugin.Item2.Vendor);
                if (existingAdapterMatch != null)
                {
                    pluginAdapterPairs.Add(new Tuple<IVideoDecoderModule, GraphicsAdapterInformation>(plugin.Item1, existingAdapterMatch));
                }
            }

            if (pluginAdapterPairs.Count > 0 && pluginAdapterPairs.Exists(match => match.Item2.Vendor == preferredVendor) == false)
            {
                preferredVendor = pluginAdapterPairs.FirstOrDefault().Item2.Vendor;
            }

            var gAdapter = gEnum.Devices.FirstOrDefault(t => t.Vendor == preferredVendor) ?? gEnum.Devices.FirstOrDefault();

            var codecFactory = new DecoderFactory(pInfo, gAdapter);
            var codecImpl = codecFactory.CreateCodec(codec);

            return codecImpl as IVideoDecoderBase;
        }

    }
}
