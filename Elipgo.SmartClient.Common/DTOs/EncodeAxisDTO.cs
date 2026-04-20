using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class EncodeAxisDTO
    {
        public string Host { get; set; }
        public string RtspPort { get; set; }
        public string RecordingId { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Filename { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Fps { get; set; }
        public string Duration { get; set; }
        public string OffSet { get; set; }
    }
}
