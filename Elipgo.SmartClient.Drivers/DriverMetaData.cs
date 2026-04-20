using System;
using System.Collections.Generic;
using EnumDrivers = Elipgo.SmartClient.Common.Enum.Drivers;

namespace Elipgo.SmartClient.Drivers
{
    public class DriverMetaData
    {
        public EnumDrivers Driver;
        public UInt32 CountReference;
        public List<string> NameTab = new List<string>();
    }
}
