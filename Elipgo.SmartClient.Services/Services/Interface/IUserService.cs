using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IUserService
    {
        Task<List<EmailNotifyDTO>> GetUserNotify(string token);

        Task SendEmailNotify(UsersNotifyAlarmDTO request, string token);
        Task<List<AppSettings>> GetConfigs(string clientId, string token);
    }
}
