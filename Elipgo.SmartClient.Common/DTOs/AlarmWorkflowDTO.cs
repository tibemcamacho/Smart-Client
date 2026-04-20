using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class AlarmWorkflowDTO
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string Actions { get; set; }
        public bool IsDeleted { get; set; }
    }
}
