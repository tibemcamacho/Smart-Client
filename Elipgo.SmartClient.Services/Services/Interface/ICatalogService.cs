using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface ICatalogService
    {
        List<RecorderDTO> GetRecorders(CameraDTO camera);
        CatalogDTO CurrentCatalog { get; set; }
        Task<CatalogDTO> GetValueCatalog(string token, int dvfid);
        Task<CatalogDTO> GetValueCatalog(string token, List<int> dvfIds);
        void SaveCatalog(CatalogDTO catalog);
    }
}
