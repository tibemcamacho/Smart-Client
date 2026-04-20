using System;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class DataViewScenes
    {
        public int Id { get; set; }
        public int ObjectId { get; set; }
        public string DeviceName { get; set; }
        public int Action { get; set; }
        public string ActionName { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
        public string ElementType { get; set; }
        public string ActionStr { get => Action.ToString(); set => Action = Convert.ToInt32(value); }
        public SubType ObjectSubType { get; set; }
        public int? ObjectSubId { get; set; }
        public string NameObjectSubType { get; set; }
    }
}
