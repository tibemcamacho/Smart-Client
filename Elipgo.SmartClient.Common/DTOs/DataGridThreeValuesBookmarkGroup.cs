using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class DataGridThreeValuesBookmarkGroup
    {
        public string Site;
        public string DeviceName;
        public string RecorderName;
        public int DeviceFeatureId;
        public int? RecorderId;
        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }
    }
}
