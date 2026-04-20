using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class Recording
    {
        public string RecordingId { get; set; }

        public string StartTime { get; set; }

        public string StopTime { get; set; }

        public string RecordingStatus { get; set; }

        public string MimeType { get; set; }

        public string FrameRate { get; set; }

        public string Audio { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
    }
}
