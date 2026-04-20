using DiskQueue;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.SignalR.Connection.Queue;
using Newtonsoft.Json;
using System.Text;

namespace Elipgo.SmartClient.SignalR.Connection
{
    public class MessageQueue
    {
        private IPersistentQueue _queue;
        private readonly static MessageQueue _instance = new MessageQueue();

        private MessageQueue()
        {
            _queue = new PersistentQueue(SmartClientEnvironmentUtils.GetMessageQueueFolder());
        }

        public static MessageQueue Instance => _instance;
        public void AddMessage(Message message)
        {
            using (var session = _queue.OpenSession())
            {
                var data = JsonConvert.SerializeObject(message);
                session.Enqueue(Encoding.ASCII.GetBytes(data));
                session.Flush();
            }
        }

        public Message GetMessage()
        {
            using (var session = _queue.OpenSession())
            {
                string output = Encoding.UTF8.GetString(session.Dequeue());
                if (output == null)
                    return null;

                var result = JsonConvert.DeserializeObject<Message>(output);
                session.Flush();
                return result;
            }
        }
    }
}
