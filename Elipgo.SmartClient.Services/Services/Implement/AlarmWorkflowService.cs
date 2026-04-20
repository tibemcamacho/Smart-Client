using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services.Services.Interface;
using System;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class AlarmWorkflowService : IAlarmWorkflowService
    {
        public async Task<AlarmWorkflowDTO> Get(string token, int alarmWorkflowId)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<AlarmWorkflowDTO>($"/v1/alarmworkflows/{alarmWorkflowId}", token);

                return result as AlarmWorkflowDTO;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
