using Elipgo.SmartClient.Common.DTOs;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IAlarmWorkflowService
    {
        Task<AlarmWorkflowDTO> Get(string token, int alarmWorkflowId);
    }
}
