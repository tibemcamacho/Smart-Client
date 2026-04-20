namespace Elipgo.SmartClient.Drivers.HCNet616
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
