using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface ISeprobanService
    {
        Task<IList<SiteDTO>> GetSites(string token);
        Task<IList<CameraDTO>> GetCamerasBySite(int siteId, string token);
        Task<IList<RecorderDTO>> GetNvrsBySite(int siteId, string token);
    }
}
