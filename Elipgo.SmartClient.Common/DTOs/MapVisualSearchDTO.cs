using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class MapVisualSearchDTO
    {
        public string Url;
        public string DateTimeName;
        public bool isValid;
        public DateTime dt;

        public MapVisualSearchDTO()
        {
            isValid = true;
        }
    }
}
