namespace Elipgo.SmartClient.SignalR.Connection.Responses
{
    public class GeneralEventResult<T>
    {
        public int EventType { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public T ResultEvent { get; set; }
    }
}
