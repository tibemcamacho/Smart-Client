using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System.Collections.Generic;
using System.Drawing;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    public class ConfigGenericForm
    {
        public LiveBarButtom ObjectBarSelected { get; set; }
        public string NameEntity { get; set; }
        public Image IconEntity { get; set; }
        public bool CanEditOrCreate { get; set; }
        public bool CanPrivate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanMultiSelect { get; set; }
        public bool ShowPlay { get; set; }
        public bool ShowSwitch { get; set; }
        public bool ShowAddButton { get; set; } = true;
        public bool ShowFilterControls { get; set; }

        public List<ContentFormDTO> Elements { get; set; }
    }
    public class ContentFormDTO
    {
        public int Id { get; set; }
        public Image EntityIcon { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsPrivate { get; set; }
        public bool? Switch { get; set; }
        public string Label1 { get; set; }
        public string Label2 { get; set; }
        public ElementType DeviceType { get; set; }
        public object ObjectOrigin { get; set; }
    }
}
