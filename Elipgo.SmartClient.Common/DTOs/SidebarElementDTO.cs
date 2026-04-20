using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class SidebarElementDTO
    {
        public int ElementId { get; set; }
        public string Name { get; set; }
        public DeviceStatus? Status { get; set; }
        public ElementType DeviceType { get; set; }
        public string GroupName { get; set; }
        public int SiteId { get; set; }
        public string DeviceTypeStr { get; set; }
        public Guid Key { get; set; }
        public string RecorderName { get; set; }
        public RecorderType RecorderType { get; set; }
        public int RecorderId { get; set; }
        public string RecorderDriver { get; set; }
        public bool ShowDvfId { get; set; }
        public Profile? ProfileStream { get; set; }
        public LiveBarButtom ObjectSelected { get; set; }
        public int IdGroup { get; set; }

        public int Hash()
        {
            var s = string.Format("{0}-{1}-{2}", ElementId, RecorderType, RecorderId);
            var hash = string.Format("{0}-{1}-{2}", ElementId, RecorderType, RecorderId).GetHashCode();
            return hash;
        }

        public RecorderDTOSmall GetRecorderDTO()
        {
            return new RecorderDTOSmall()
            {
                Id = RecorderId,
                Name = (RecorderType == RecorderType.EDGE) ? Name : RecorderName,
                RecorderType = RecorderType,
                Driver = RecorderDriver == null ? "" : RecorderDriver

            };
        }
    }
    public class CatalogObjectDetails
    {
        public int Count { get; set; }
        public List<SidebarElementDTO> Elements { get; set; }
    }
    public class CatalogObject
    {
        public int Count { get; set; }
        public int countDetails { get; set; }
        public IList<CatalogObjectElement> Elements { get; set; }
    }

    public class CatalogObjectElement
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FiltersSidebarGroup
    {
        public int IdSite { get; set; }
        public int TypeElement { get; set; }
        public string Filter { get; set; }
    }
}