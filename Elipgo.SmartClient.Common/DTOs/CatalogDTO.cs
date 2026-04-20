using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class CatalogDTO
    {
        public List<CatalogObjectGroup> Carousels { get; set; } = new List<CatalogObjectGroup>();
        public List<CatalogSite> Sites { get; set; } = new List<CatalogSite>();
        public  CatalogInvoiceModule InvoiceModule { get; set; }
        public AccessControlModule ControlAcceso { get; set; }
        public CatalogDTO()
        {
            InvoiceModule = new CatalogInvoiceModule();
            ControlAcceso = new AccessControlModule();
        }
    }

    public class CatalogSite
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public List<CatalogCamera> Cameras { get; set; } = new List<CatalogCamera>();
        public List<CatalogIot> Iots { get; set; } = new List<CatalogIot>();
        public List<CatalogKpi> Kpis { get; set; } = new List<CatalogKpi>();
        public List<CatalogLpr> Lprs { get; set; } = new List<CatalogLpr>();
        public List<CatalogVCA> VCAs { get; set; } = new List<CatalogVCA>();
        public List<CatalogFace> Faces { get; set; } = new List<CatalogFace>();
        public List<CatalogLocation> Locations { get; set; } = new List<CatalogLocation>();
        public List<CatalogGeoLocation> GeoLocations { get; set; } = new List<CatalogGeoLocation>();
    }

    public class CatalogCamera : IElementGroup
    {
        public int ObjectId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool? LiveRead { get; set; }
        public bool? LiveReadAudio { get; set; }
        public bool? LiveReadPTZ { get; set; }
        public bool? LiveWrite { get; set; }
        public bool? LiveWritePTZ { get; set; }
        public bool? AlarmRead { get; set; }
        public bool? PlaybackRead { get; set; }
        public bool? PlaybackWrite { get; set; }
        public bool? EdgeEnabled { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public List<RecorderDTO> Recorders { get; set; } = new List<RecorderDTO>();
        public string DeviceName { get; set; }
        public List<SyncServerDTO> SyncServers { get; set; } = new List<SyncServerDTO>();
        public int DveId { get;set; }

    }

    public class CatalogIot : IElementGroup
    {
        public int ObjectId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        
        public string SubType { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public int count { get; set; }
        public bool IsPtz { get; set; }
        public int DveId { get; set; }
    }

    public class CatalogLocation : IElementGroup
    {
        public int ObjectId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        
        //public string Image { get; set; }
        //public double LocationLatitude { get; set; }
        //public double LocationLongitude { get; set; }
    }

    public class CatalogGeoLocation : IElementGroup
    {
        public int ObjectId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
    }

    public class CatalogKpi : IElementGroup
    {
        public int ObjectId { get; set; }
        public string Name { get; set; }
        
        public string PrdCode { get; set; }
        public bool Negative { get; set; }
        public string Type { get; set; }
    }

    public class CatalogFace : IElementGroup
    {
        public int ObjectId { get; set; }
        public string Name { get; set; }
        
        public int DvfId { get; set; }
        public string Type { get; set; }
        public int SiteId { get; set; }
    }

    public class CatalogVCA : IElementGroup
    {
        public int ObjectId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public int SiteId { get; set; }
        public int DvfId { get; set; }
    }

    public class CatalogLpr : IElementGroup
    {
        public int ObjectId { get; set; }
        public string Name { get; set; }
        
        public string PrdCode { get; set; }
        public string Type { get; set; }

    }

    public class CatalogGroups
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupGridId { get; set; }
        public bool GroupIsPrivate { get; set; }
        public int GroupType { get; set; }
        public int ElementId { get; set; }
        public int ObjectId { get; set; }
        public int? ElementSiteId { get; set; }
        public string ElementType { get; set; }
        public string ElementTime { get; set; }
        public int ElementContainerId { get; set; }
    }

    public class CatalogObjectGroup 
    {
        public int Id { get; set; }
        public string UserIdGuid { get; set; }
        public string Name { get; set; }
        
        public string GridId { get; set; }
        public bool IsPrivate { get; set; }
        public int Type { get; set; }
        public List<CatalogObjectGroupElement> Elements { get; set; } = new List<CatalogObjectGroupElement>();
    }

    public class CatalogObjectGroupElement
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public int? ContainerId { get; set; }
        public Element Element { get; set; }
    }

    public class CatalogDvfs
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public double SiteLatitude { get; set; }
        public double SiteLongitude { get; set; }
        public int ObjectId { get; set; }
        public string ObjectName { get; set; }
        public string ObjectImage { get; set; }
        public string ObjectType { get; set; }
        public string ObjectSubType { get; set; }
        public bool? LiveRead { get; set; }
        public bool? LiveReadAudio { get; set; }
        public bool? LiveReadPTZ { get; set; }
        public bool? LiveWrite { get; set; }
        public bool? LiveWritePTZ { get; set; }
        public bool? AlarmRead { get; set; }
        public bool? PlaybackRead { get; set; }
        public bool? PlaybackWrite { get; set; }
        public bool? EdgeEnabled { get; set; }
        public double ObjectLatitude { get; set; }
        public double ObjectLongitude { get; set; }
    }

    public class CatalogVehicle
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Plate { get; set; }
        public bool? LiveRead { get; set; }
        public bool? PlaybackRead { get; set; }
    }

    public class CatalogWorkspace
    {
        public string Guid { get; set; }
        public string GroupId { get; set; }
        public string Name { get; set; }
        public string GridId { get; set; }
        public bool IsEditable { get; set; }
        public List<CatalogObjectGroupElement> Elements { get; set; } = new List<CatalogObjectGroupElement>();
    }

    public class CatalogWorkspaceState
    {
        public bool IsCollapsed { get; set; }
        public int SelectedCount { get; set; }
        public string SelectedIcon { get; set; }
    }

    public class CatalogWorkspaceStatus
    {
        public bool IsSelected { get; set; }
    }

    public class Element
    {
        public int Id { get; set; }
        public int? SiteId { get; set; }
        public string Type { get; set; }
    }

    public class CatalogRecorders
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Driver { get; set; }
    }

    public class CatalogInvoiceModule
    {
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string ApiUrl { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class AccessControlModule
    {
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string ApiUrl { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class OptionSidebarEnabled
    {
        public bool IsEnabled { get; set; }
        public int SidebarElement { get; set; }
    }
}
