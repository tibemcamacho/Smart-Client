using Elipgo.SmartClient.Worker.Shared.Models.Pipes;
using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Worker.Shared
{
    public static class PipeClient
    {
        private const int ConnectionTimeoutMs = 5000;

        public static async Task<PipeResponse> SendMessageAsync<T>(PipeMessage<T> message, int timeoutMs = ConnectionTimeoutMs) where T : class
        {
            try
            {
                using (var pipe = new NamedPipeClientStream(
                    ".",
                    PipeConstants.PipeName,
                    PipeDirection.InOut,
                    PipeOptions.Asynchronous))
                {
                    await pipe.ConnectAsync(timeoutMs);

                    var json = JsonSerializer.Serialize(message);
                    var payload = Encoding.UTF8.GetBytes(json);
                    var lengthPrefix = System.BitConverter.GetBytes(payload.Length);

                    await pipe.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
                    await pipe.WriteAsync(payload, 0, payload.Length);
                    await pipe.FlushAsync();

                    var responseLengthBuffer = new byte[4];
                    await ReadExactAsync(pipe, responseLengthBuffer, 4);
                    var responseLength = System.BitConverter.ToInt32(responseLengthBuffer, 0);

                    var responseBuffer = new byte[responseLength];
                    await ReadExactAsync(pipe, responseBuffer, responseLength);

                    var responseJson = Encoding.UTF8.GetString(responseBuffer);
                    return JsonSerializer.Deserialize<PipeResponse>(responseJson)
                        ?? new PipeResponse { Success = false, Error = "Failed to deserialize response" };
                }
            }
            catch (TimeoutException)
            {
                return new PipeResponse { Success = false, Error = "Worker pipe not available" };
            }
            catch (IOException ex)
            {
                return new PipeResponse { Success = false, Error = ex.Message };
            }
            catch (Exception ex)
            {
                return new PipeResponse { Success = false, Error = ex.Message };
            }
        }

        private static async Task ReadExactAsync(Stream stream, byte[] buffer, int count)
        {
            var offset = 0;
            while (offset < count)
            {
                var read = await stream.ReadAsync(buffer, offset, count - offset);
                if (read == 0)
                    throw new EndOfStreamException("Pipe closed before all data was received.");
                offset += read;
            }
        }
    }
}
