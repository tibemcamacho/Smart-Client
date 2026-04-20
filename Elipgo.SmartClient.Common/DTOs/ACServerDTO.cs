using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class ACServerDTO
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string ApiUrl { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }
}
