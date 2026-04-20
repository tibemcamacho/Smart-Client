using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class BlueprintDTO
    {
        public BlueprintDTO()
        {
            Elements = new HashSet<BlueprintElementDTO>();
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int EntityId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Image { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool UseGeoSite { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<BlueprintElementDTO> Elements { get; set; }
    }
}