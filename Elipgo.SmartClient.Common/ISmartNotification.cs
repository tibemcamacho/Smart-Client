
using System;

namespace Elipgo.SmartClient.Common
{
    public delegate void NotificationWindowsEventHandler(string message, Action callBack);

    public interface ISmartNotification
    {
        event NotificationWindowsEventHandler NotificationWindowsEventHandler;

        void Show(string message, Action callBack);
    }
}
