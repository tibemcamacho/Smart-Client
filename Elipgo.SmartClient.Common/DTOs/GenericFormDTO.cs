using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class GenericFormDTO
    {
        public int Id { get; set; }
        public string EntityName { get; set; }
        public string EntityIcon { get; set; }
        public bool? CanEditOrCreate { get; set; }
        public bool? CanPrivate { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanMultiSelect { get; set; }
        public bool? HasSwitch { get; set; }
        public List<ContentFormDTO> Elements { get; set; }
    }
    public class ContentFormDTO
    {
        public int Id { get; set; }
        public string PrivateIcon { get; set; }
        public string Name{ get; set; }
        public bool? IsActive { get; set; }
        public string Label1 { get; set; }
        public string Label2 { get; set; }
    }
}
