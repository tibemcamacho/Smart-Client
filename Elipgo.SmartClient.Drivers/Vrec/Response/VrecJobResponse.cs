using System.Xml.Serialization;

namespace Elipgo.SmartClient.Drivers.Vrec.Response
{
    [XmlRoot(ElementName = "vrecResponse")]
    public class VrecJobResponse
    {
        [XmlElement(ElementName = "job")]
        public Job Job { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
    }


    [XmlRoot(ElementName = "job")]
    public class Job
    {
        [XmlAttribute(AttributeName = "processID")]
        public string ProcessID { get; set; }
    }
}
