using System;

namespace Elipgo.SmartClient.SignalR.Connection.Queue
{
    public class Message
    {
        public DateTime Expired { get; set; }
        public string Notification { get; set; }
    }
}
