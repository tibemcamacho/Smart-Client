using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class DeviceFeatureDTO
    {
        public int DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string DeviceNameAndSite { get; set; }
        public int? GlobalServicePlanId { get; set; }
        public string ChannelType { get; set; }
        public string ChannelSubType { get; set; }
        public int ChannelNumber { get; set; }
        public string Name { get; set; }
        public bool PtzEnabled { get; set; }
        public bool EdgeEnabled { get; set; }
        public int IconId { get; set; }
        public bool MonitorStatus { get; set; }
        public bool UseGeoSite { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string VideoStandard { get; set; }
        public bool Enabled { get; set; }
        public bool IsDeleted { get; set; }
        public bool AudioEnabled { get; set; }
        public bool TalkEnabled { get; set; }
        public bool SeprobanEnabled { get; set; }

        public double MovementSensitivity { get; set; }

        public double ZoomSensitivity { get; set; }
        public string MainStream { get; set; }
        public string SubStream { get; set; }
        public string Codec { get; set; }
    }
}
