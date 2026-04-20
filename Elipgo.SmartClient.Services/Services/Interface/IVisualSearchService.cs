using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IVisualSearchService
    {
        Task<IList<SiteCatalogVscDTO>> Get(string token);
        Task<System.Net.Http.HttpResponseMessage> GetCapture(string url, string token = "");
    }
}
