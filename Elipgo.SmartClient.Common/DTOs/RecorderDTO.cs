
using Elipgo.SmartClient.Common.Enum;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class RecorderDTO 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public int HttpPort { get; set; }
        public int VideoPort { get; set; }
        public int RtspPort { get; set; }
        public double Gmt { get; set; }
        public bool Dst { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Driver { get; set; }
        public bool ProxyEnabled { get; set; }
        public string HttpProtocol { get; set; }
        public RecorderType RecorderType { get; set; }
        public int Channel { get; set; }
    }

    public class RecorderDTOSmall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Driver { get; set; }
        public RecorderType RecorderType { get; set; }
        public string Host { get; set; }
        public int VideoPort { get; set; }
        public int Channel { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
