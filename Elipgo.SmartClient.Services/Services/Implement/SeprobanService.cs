using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class SeprobanService : ISeprobanService
    {
        public async Task<IList<CameraDTO>> GetCamerasBySite(int siteId, string token)
        {
            var result = await Client.Client.Instance.GetAsync<List<CameraDTO>>($"/v1/seproban/entity/sites/{siteId}/cameras", token);
            foreach (var it in result)
            {
                it.Password = Security.AESDecrypt(it.Password);
            }
            return result;
        }

        public async Task<IList<RecorderDTO>> GetNvrsBySite(int siteId, string token)
        {
            var result = await Client.Client.Instance.GetAsync<List<RecorderDTO>>($"/v1/seproban/entity/sites/{siteId}/nvrs", token);

            return result;
        }

        public async Task<IList<SiteDTO>> GetSites(string token)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<List<SiteDTO>>($"/v1/seproban/entity/sites/", token);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
