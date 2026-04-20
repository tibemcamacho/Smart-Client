using Elipgo.SmartClient.Common.Enum;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class TestDeviceFeatureAvailableDTO
    {
        public int DveId { get; set; }
        public int SitId { get; set; }
        public string Status { get; set; }
        public int Count { get; set; }
    }
}
