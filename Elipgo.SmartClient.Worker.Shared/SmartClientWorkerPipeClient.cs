using Elipgo.SmartClient.Worker.Shared.Models.Pipes;
using Elipgo.SmartClient.Worker.Shared.Models.Pipes.Messages;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Worker.Shared
{
    public class SmartClientWorkerPipeClient : ISmartClientWorkerPipeClient
    {
        private const int InstallerPipeTimeoutMs = 2000;

        public async Task<PipeResponse> SendTokenAsync(string token)
        {
            var message = new PipeMessage<TokenMessage>
            {
                MessageType = PipeMessageTypes.SCToken,
                Payload = new TokenMessage { Token = token }
            };
            return await PipeClient.SendMessageAsync(message);
        }

        public async Task<PipeResponse> SendStopForInstallAsync()
        {
            var message = new PipeMessage<StopForInstallMessage>
            {
                MessageType = PipeMessageTypes.StopForInstall,
                Payload = new StopForInstallMessage()
            };
            return await PipeClient.SendMessageAsync(message, InstallerPipeTimeoutMs);
        }

        public async Task<PipeResponse> SendInstallUpdateAsync(string installerPath, string installerVersion)
        {
            var message = new PipeMessage<InstallUpdateMessage>
            {
                MessageType = PipeMessageTypes.InstallUpdate,
                Payload = new InstallUpdateMessage
                {
                    InstallerPath    = installerPath,
                    InstallerVersion = installerVersion
                }
            };
            return await PipeClient.SendMessageAsync(message, InstallerPipeTimeoutMs);
        }

        public async Task<PipeResponse> SendInstallFailedAsync(string reason)
        {
            var message = new PipeMessage<InstallFailedMessage>
            {
                MessageType = PipeMessageTypes.InstallFailed,
                Payload = new InstallFailedMessage { Reason = reason }
            };
            return await PipeClient.SendMessageAsync(message, InstallerPipeTimeoutMs);
        }
    }
}
