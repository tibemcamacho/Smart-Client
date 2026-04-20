using System.Collections.Generic;
using System.Xml.Serialization;

namespace Elipgo.SmartClient.Drivers.Vrec.Response
{
    [XmlRoot(ElementName = "job")]
    public class JobProgress
    {
        [XmlAttribute(AttributeName = "processID")]
        public string ProcessID { get; set; }
        [XmlAttribute(AttributeName = "cameraID")]
        public string CameraID { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "progress")]
        public string Progress { get; set; }
        [XmlAttribute(AttributeName = "fileSize")]
        public string FileSize { get; set; }
        [XmlAttribute(AttributeName = "startTime")]
        public string StartTime { get; set; }
        [XmlAttribute(AttributeName = "stopTime")]
        public string StopTime { get; set; }
    }

    [XmlRoot(ElementName = "vrecResponse")]
    public class VrecProgressResponse
    {
        [XmlElement(ElementName = "job")]
        public List<JobProgress> Job { get; set; }

        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }

        [XmlAttribute(AttributeName = "message")]
        public string Message { get; set; }
    }
}
