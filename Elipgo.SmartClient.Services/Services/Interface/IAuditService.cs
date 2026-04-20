using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IAuditService
    {
        Task Add(AuditDTO request, string token);
        Task AddRange(IEnumerable<AuditDTO> request, string token);
    }
}
