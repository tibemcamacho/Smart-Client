using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Drivers.HCNet619
{
    internal class HikvisionHelper
    {
        public static int GetChannelNumber(int channelNum, int startDChannelNum)
        {
            if (startDChannelNum != 0)
            {
                return channelNum + (startDChannelNum - 1);
            }
            return channelNum;
        }
    }
}
