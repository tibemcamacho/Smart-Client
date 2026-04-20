using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services.Services.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class AuditService : IAuditService
    {
        public async Task Add(AuditDTO request, string token)
        {
            try
            {
                await Client.Client.Instance.PostAsync<dynamic>(request, $"/v1/audit", token);
            }
            catch (Exception)
            {
            }
        }

        public  async Task AddRange(IEnumerable<AuditDTO> request, string token)
        {
            try
            {
                await Client.Client.Instance.PutAsync<dynamic>(request, $"/v1/auditAddRange", token);
            }
            catch (Exception)
            {
            }
        }
    }
}
