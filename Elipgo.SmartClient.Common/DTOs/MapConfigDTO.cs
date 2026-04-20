using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class MapConfigDTO
    {
        public string Type { get; set; }

        public double LocationLatitude { get; set; }
            
        public double LocationLongitude { get; set; }

        public string Name { get; set; }
    }
}
