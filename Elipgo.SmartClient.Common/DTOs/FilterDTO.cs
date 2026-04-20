using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class FilterDTO
    {
        public List<string> AlarmType { get; set; } //Tipo Alarma

        public string ObjectType { get; set; } //Tipo Dispositivo

        public string AlarmTime { get; set; }

        public string Device { get; set; }

        // Filtro de Sidebar
        public int Site { get; set; }

        public int EntityId { get; set; }

        public int Page { get; set; }

        public int? Take { get; set; }


        public override string ToString()
        {
            return $"?AlarmType={AlarmType}&ObjectType={ObjectType}";
        }
    }
}
