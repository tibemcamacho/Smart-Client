using Elipgo.SmartClient.Common.DTOs;

namespace Elipgo.SmartClient.ViewModels
{
    public interface IGenericViewModel
    {
        long UserId { get; set; }

        string UserIdGuid { get; set; }

        string Token { get; set; }

        long EntityId { get; set; }

        CatalogDTO Catalog { get; set; }

        System.Resources.ResourceManager Resource { get; set; }

    }
}
