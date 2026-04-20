using Elipgo.SmartClient.Worker.Shared.Models.Pipes;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Worker.Shared
{
    public interface ISmartClientWorkerPipeClient
    {
        Task<PipeResponse> SendTokenAsync(string token);
        Task<PipeResponse> SendStopForInstallAsync();
        Task<PipeResponse> SendInstallUpdateAsync(string installerPath, string installerVersion);
        Task<PipeResponse> SendInstallFailedAsync(string reason);
    }
}
