using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class UsersNotifyAlarmDTO
    {
        public string[] Emails { get; set; }
        public string Message { get; set; }
        public int AlarmId { get; set; }
    }
}
