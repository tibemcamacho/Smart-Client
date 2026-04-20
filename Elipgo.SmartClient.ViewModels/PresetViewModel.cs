using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Drivers;
using System.Resources;

namespace Elipgo.SmartClient.ViewModels
{
    public class PresetViewModel : IGenericViewModel
    {
        public long UserId { get; set; }
        public string UserIdGuid { get; set; }
        public string Token { get; set; }
        public long EntityId { get; set; }
        public CatalogDTO Catalog { get; set; }
        public ResourceManager Resource { get; set; }
        public IDriverLive Driver { get; set; }
    }
}
