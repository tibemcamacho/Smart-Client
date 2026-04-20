using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class IdentityResultDTO
    {
        public bool Succeeded { get; set; }
        public IEnumerable<IdentityErrorDTO> Errors { get; set; }
    }
    public class IdentityErrorDTO
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
