
using System;

namespace Elipgo.SmartClient.Common
{
    public class SmartNotification : ISmartNotification
    {

        public event NotificationWindowsEventHandler NotificationWindowsEventHandler;

        public void Show(string message, Action callBack)
        {
            NotificationWindowsEventHandler(message, callBack);
        }

    }
}
