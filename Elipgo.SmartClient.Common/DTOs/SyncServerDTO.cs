using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class SyncServerDTO
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public IList<SyncServerRecordersDTO> SyncServerRecordersEntity { get; set; }
    }
}
