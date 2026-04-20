using System;
using System.Collections.Generic;
using Elipgo.SmartClient.Common.Enum;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class SidebarGroupElementDTO
    {
        public string Name { get; set; }
        public ElementType DeviceType { get; set; }
        public List<SidebarElementDTO> SidebarElements { get; set; }
        public Guid Key { get; set; }
        public int ElementId { get; set; }
        public string DeviceTypeStr { get; set; }
    }
}
