using Elipgo.SmartClient.Drivers.Dahua351;
using Elipgo.SmartClient.Drivers.Dahua351v2;
using Elipgo.SmartClient.Drivers.Dahua351v3;
using Elipgo.SmartClient.Drivers.Dahua351v4;
using Elipgo.SmartClient.Drivers.Dahua351v5;
using Elipgo.SmartClient.Drivers.Dahua351v6;
using Elipgo.SmartClient.Drivers.Dahua351v7;
using Elipgo.SmartClient.Drivers.Dahua351v8;
using Elipgo.SmartClient.Drivers.Dahua351v9;
using Elipgo.SmartClient.Drivers.Dahua351v10;
using Elipgo.SmartClient.Drivers.Dahua351v11;
using Elipgo.SmartClient.Drivers.Dahua351v12;
using EnumDrivers = Elipgo.SmartClient.Common.Enum.Drivers;


namespace Elipgo.SmartClient.Drivers
{
    public static class DriverFactoryNetSDK
    {
        public static AbsNetSDK GetDriver(EnumDrivers driver)
        {
            switch (driver)
            {
                case EnumDrivers.NETSDK_351:
                    return new NetSDK351();
                case EnumDrivers.NETSDK_351v2:
                    return new NetSDK351v2();
                case EnumDrivers.NETSDK_351v3:
                    return new NetSDK351v3();
                case EnumDrivers.NETSDK_351v4:
                    return new NetSDK351v4();
                case EnumDrivers.NETSDK_351v5:
                    return new NetSDK351v5();
                case EnumDrivers.NETSDK_351v6:
                    return new NetSDK351v6();
                case EnumDrivers.NETSDK_351v7:
                    return new NetSDK351v7();
                case EnumDrivers.NETSDK_351v8:
                    return new NetSDK351v8();
                case EnumDrivers.NETSDK_351v9:
                    return new NetSDK351v9();
                case EnumDrivers.NETSDK_351v10:
                    return new NetSDK351v10();
                case EnumDrivers.NETSDK_351v11:
                    return new NetSDK351v11();
                case EnumDrivers.NETSDK_351v12:
                    return new NetSDK351v12();
            }

            return null;
        }
    }
}
