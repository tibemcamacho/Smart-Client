using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class UserService : IUserService
    {
        //private static readonly string AUTH = ConfigurationManager.AppSettings["Authority"];

        public async Task<List<EmailNotifyDTO>> GetUserNotify(string token)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<List<EmailNotifyDTO>>($"/v1/users/notifyemail", token);

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task SendEmailNotify(UsersNotifyAlarmDTO request, string token)
        {
            try
            {
                await Client.Client.Instance.PostAsync<dynamic>(request, $"/v1/sendalarmnotify", token);
            }
            catch (Exception)
            {
            }
        }

        public async Task<List<AppSettings>> GetConfigs(string clientId, string token)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<List<AppSettings>>($"/v1/clientconfigs/" + clientId, token);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
