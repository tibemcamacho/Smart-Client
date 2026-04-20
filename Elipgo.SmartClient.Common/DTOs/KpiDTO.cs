using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class KpiDTO
    {
        public int ObjectId { get; set; }
        public int SiteId { get; set; }
        public string Type { get; set; }
        public string Token { get; set; }
    }
}
