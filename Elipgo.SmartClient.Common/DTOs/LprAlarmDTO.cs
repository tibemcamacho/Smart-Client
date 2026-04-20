using System;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class LprAlarmDTO
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string TimeAction { get; set; }
        public string TimeActionUtc { get; set; }
        public int DeviceFeatureId { get; set; }
        public int VideoContentAnalyticId { get; set; }
        public float Confidence { get; set; }
        public string PositionImage { get; set; }
        public List<string> LprLists { get; set; }
    }
}
