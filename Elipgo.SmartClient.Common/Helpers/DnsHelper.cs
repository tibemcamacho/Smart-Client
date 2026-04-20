using System;
using System.Net;

namespace Elipgo.SmartClient.Common.Helpers
{
    public class DnsHelper
    {
        public static IPAddress Resolve(string host)
        {
            try
            {
                return IPAddress.Parse(host);
            }
            catch (FormatException)
            {
                try
                {
                    return Dns.GetHostEntry(host).AddressList[0];
                }
                catch
                {
                    throw new Exception("Unable to resolve Host: " + host);
                }
            }
        }
    }
}
