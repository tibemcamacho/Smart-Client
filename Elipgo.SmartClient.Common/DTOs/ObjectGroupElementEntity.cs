using Elipgo.SmartClient.Common.Enum;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class ObjectGroupElementEntity 
    {
        public int Id { get; set; }
        public int ObjectGroupId { get; set; }
        public int ObjectId { get; set; }
        public string Type { get; set; }
        public int Time { get; set; }
        public int ContainerId { get; set; }
        public bool IsDeleted { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public int Zoom { get; set; }
        public int SiteId { get; set; }
        public int Order { get; set; }
        public Profile? ProfileStream { get; set; }
    }
}
