namespace Elipgo.SmartClient.Worker.Shared.Models.Pipes
{
    public class PipeMessage<T> where T : class
    {
        public T Payload { get; set; }
        public PipeMessageTypes MessageType { get; set; }
    }
}
