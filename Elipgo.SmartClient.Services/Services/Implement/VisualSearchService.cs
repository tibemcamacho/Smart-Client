using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services.Services.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class VisualSearchService : IVisualSearchService
    {
        public async Task<IList<SiteCatalogVscDTO>> Get(string token)
        {
            var result = await Client.Client.Instance.GetAsync<List<SiteCatalogVscDTO>>("/v1/visualsearchcache/entities/cameras", token);

            return result;
        }

        public Task<System.Net.Http.HttpResponseMessage> GetCapture(string url, string token = "")
        {
            var result = Client.Client.Instance.GetAsyncByUri(url, token);

            return Task.FromResult<System.Net.Http.HttpResponseMessage>(result);
        }
    }
}
