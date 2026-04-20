using Elipgo.SmartClient.Common.DTOs;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IIotService
    {
        Task Update(IotDTO request, string token);
    }
}
