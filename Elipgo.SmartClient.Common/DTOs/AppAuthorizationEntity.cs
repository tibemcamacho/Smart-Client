using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class AppAuthorizationEntity
    {
        public int AppAuthorizationTitleId { get; set; }
        public string Key { get; set; }
        public char Type { get; set; }
        public int Sort { get; set; }
        public bool Enabled { get; set; }
    }
}
