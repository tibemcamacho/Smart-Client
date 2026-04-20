using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class ObjectGroupEntity 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserIdGuid { get; set; }
        public string Name { get; set; }
        public string GridId { get; set; }
        public bool IsPrivate { get; set; }
        public int Type { get; set; }
        public bool IsDeleted { get; set; }
        public ObjectGroupElementEntity[] Elements { get; set; }
    }

}
