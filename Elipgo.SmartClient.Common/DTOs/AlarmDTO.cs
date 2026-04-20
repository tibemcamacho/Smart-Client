using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public enum AlarmLevels
    {
        CriticalWSound = 1,
        NormalWSound = 3,
        CriticalWoSound = 4,
        CriticalDiscard = 5,
        NormalWoSound = 6,
        CriticalChecked = 7,
        CriticalAttendedToggle = 8
    }

    public class AlarmDTO
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public string ObjectType { get; set; }
        public string DeviceName { get; set; }
        public string SiteName { get; set; }
        public int SiteId { get; set; }
        public int AlarmWorkFlowId { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public bool Priority { get; set; }
        public AlarmLevels Level { get; set; }
        public bool HasSnapshot { get; set; }
        public string Snapshot { get; set; }
        public bool Notify { get; set; }
        public string Message { get; set; }
        public int HelperId { get; set; }
        public int HelperId2 { get; set; }
        public string Comments { get; set; }
        public int AttendedBy { get; set; }
        public string TimeUtc { get; set; }
        public string TimeAttended { get; set; }
        public string TimeAttendedUtc { get; set; }
        public string UserAttended { get; set; }
        public string Callback { get; set; }
        public int EntityId { get; set; }

        public string[] Mails { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }

        public int LastAlarmId { get; set; }
    }

    public class DiscardAllAlarms
    {
        public int DicardFromAlarmId;
        public string Message;
        public List<string> AlarmType;
        public int Site;
    }
}
