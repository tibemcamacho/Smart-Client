using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using StreamCoders.Additional.Plugins;

namespace MediaSuite.Common
{
    [ComVisible(false)]
    public class PluginInformation<T> where T : ICodecModule
    {
        private readonly ExtensionsModuleManager<T> extensionManager;
        private readonly IList<string> paths;

        public PluginInformation(IList<string> paths)
        {
            this.paths = paths;
            extensionManager = new ExtensionsModuleManager<T>(paths);
        }

        public IReadOnlyList<Tuple<T, IMediaSuiteModuleInfo>> Modules => extensionManager.Modules;

        public T GetCodecModule(MediaSuiteExtensionOperationType operationType, MediaSuiteExtensionVendor vendor, string codecImplementation)
        {
            var modules = extensionManager.Modules.Where(m => m.Item2.OperationType == operationType && m.Item2.Implements == codecImplementation && m.Item2.Vendor == vendor).Select(m => m.Item1)
                .ToList();
            if (modules.Count == 0)
            {
                return default(T);
            }

            var sModule = modules.First();
            return sModule;
        }
    }
}