using System.Collections.Generic;
using System.Management;

namespace MediaSuite.Common
{
    public class GraphicsEnumerator
    {
        private const string DeviceIdPrefix = "VideoController";

        public IList<GraphicsAdapterInformation> Devices
        {
            get
            {
                var retList = new List<GraphicsAdapterInformation>();
                var query = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (var moProp in query.Get())
                {
                    var managmentObject = (ManagementObject)moProp;

                    var currentProperty = managmentObject.Properties["DeviceID"];

                    if (currentProperty?.Value != null)
                    {
                        retList.Add(new GraphicsAdapterInformation(managmentObject));
                    }
                }

                return retList;
            }
        }

        public GraphicsAdapterInformation GetDevice(int deviceId)
        {
            foreach (var device in Devices)
            {
                if (device.DeviceId == $"{DeviceIdPrefix}{deviceId}")
                {
                    return device;
                }
            }

            return null;
        }
    }
}
