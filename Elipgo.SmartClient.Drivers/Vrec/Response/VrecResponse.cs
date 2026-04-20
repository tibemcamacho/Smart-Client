using System.Xml.Serialization;


namespace Elipgo.SmartClient.Drivers.Vrec.Response
{
    [XmlRoot(ElementName = "vrecResponse")]
    public class VrecResponse
    {
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
        [XmlAttribute(AttributeName = "message")]
        public string Message { get; set; }
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
    }
}
