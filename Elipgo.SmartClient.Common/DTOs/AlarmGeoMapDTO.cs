using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class AlarmGeoMapDTO
    {
        public int AlarmId { get; set; }
        public int ObjectId { get; set; }
        public string Name { get; set; }
        public string AlarmType { get; set; }
        public string SiteName { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public string AlarmMessage { get; set; }
        public string AlarmTimeAction { get; set; }
    }
}
