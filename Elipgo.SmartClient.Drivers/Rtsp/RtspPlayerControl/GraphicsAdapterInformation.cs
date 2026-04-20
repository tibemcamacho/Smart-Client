using System.Management;
using StreamCoders.Additional.Plugins;

namespace MediaSuite.Common
{
    public class GraphicsAdapterInformation
    {
        private readonly ManagementObject managmentObject;

        internal GraphicsAdapterInformation(ManagementObject managmentObject)
        {
            this.managmentObject = managmentObject;
        }

        public MediaSuiteExtensionVendor Vendor
        {
            get
            {
                var vendor = MediaSuiteExtensionVendor.Software;

                if (Name.ToUpper().Contains("NVIDIA"))
                {
                    vendor = MediaSuiteExtensionVendor.Nvidia;
                }

                if (Name.ToUpper().Contains("AMD"))
                {
                    vendor = MediaSuiteExtensionVendor.Amd;
                }

                if (Name.ToUpper().Contains("INTEL"))
                {
                    vendor = MediaSuiteExtensionVendor.Intel;
                }

                return vendor;
            }
        }

        public string DeviceId => GetProperty<string>("DeviceID");
        public string Name => GetProperty<string>("Name");

        public string Description => GetProperty<string>("Description");
        public string DriverVersion => GetProperty<string>("DriverVersion");

        protected T GetProperty<T>(string propertyName) where T : class
        {
            var currentProperty = managmentObject.Properties[propertyName];

            return currentProperty.Value as T;
        }
    }
}