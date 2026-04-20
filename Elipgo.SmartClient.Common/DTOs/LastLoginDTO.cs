using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class LastLoginDTO
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}
