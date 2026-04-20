using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class Vrec5ChunkList
    {
        [JsonProperty("camera")]
        public string CameraId { get; set; }
        [JsonProperty("video")]
        public string Video { get; set; }
        [JsonProperty("from")]
        public string From { get; set; }
        [JsonProperty("to")]
        public string To { get; set; }
    }
}
