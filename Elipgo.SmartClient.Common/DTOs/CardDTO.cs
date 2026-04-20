using System;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class CardDto
    {
        public int IdAlarm { get; set; }

        public string Type { get; set; }

        public string Site { get; set; }

        public string DeviceName { get; set; }

        public DateTime Time { get; set; }
        public DateTime TimeUTC { get; set; }

        // Valores para aplicar filtros
        public int IdSite { get; set; }

        // Valores utilizados para futuras busquedas
        public string DeviceType { get; set; }
        public int ObjectId { get; set; }
        public bool HighLightCamera { get; set; }
        public string Message { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Snapshot { get; set; }
        public VideoChannelRelated[] dvfs { get; set; }
        public string Personalized_Message { get; set; }
        public string Cad_Invoice { get; set; }
        public string SubType { get; set; }
        public bool Diagnostic { get; set; }
        public List<int> Profile { get; set; }
        public DateTime? TimeAttendedUTC { get; set; }
        public string UserAttended { get; set; }
        public int AlarmLevel { get; set; }
        public string Comments { get; set; }
        public int TotalFilteredAlarms { get; set; }
        public int ObjectGroupId { get; set; }
    }

    public class VideoChannelRelated
    {
        public int IdDvf;
        public string Name;
    }
}
