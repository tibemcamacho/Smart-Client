using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class SyncServerRecordersDTO
    {
        public int SyncServerId { get; set; }
        public int EntityId { get; set; }
        public int DeviceId { get; set; }
        public int SiteId { get; set; }
        public string TypeRecorder { get; set; }
        public string Host { get; set; }
        public int? HttpPort { get; set; }
        public int? Channel { get; set; }
        //public string Url { get; set; }
        public int? State { get; set; }
        public int? Recycle { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        //public int? Port { get; set; }
        public string Component { get; set; }
        public string Path { get; set; }
        public string Cron { get; set; }
        public int? Duration { get; set; }
    }
}
