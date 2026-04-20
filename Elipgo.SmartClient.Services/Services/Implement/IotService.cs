using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services.Services.Interface;
using System;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class IotService : IIotService
    {
        public async Task Update(IotDTO request, string token)
        {
            try
            {
                await Client.Client.Instance.PostAsync<dynamic>(request, "/v1/iot/state", token);
            }
            catch (Exception ex)
            {
                var e = ex;
            }
        }
    }
}
