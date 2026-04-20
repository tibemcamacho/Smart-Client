using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.SignalR.Connection.Configuration;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Transports;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.SignalR.Connection
{
    public class SignalRConectionManager : IDisposable
    {
        /// <summary>
        /// Hub para inicializar la conexion con el servidor
        /// </summary>
        HubConnection hub = null;

        /// <summary>
        /// Proxy para conectarse con el hub del server
        /// </summary>
        IHubProxy proxy = null;

        /// <summary>
        /// Configuracion de los endpoints
        /// </summary>
        public Action<IHubProxy> configActions;

        /// <summary>
        /// Valores por querystring para enviar en la conexion
        /// </summary>
        public Dictionary<string, string> queryString = new Dictionary<string, string>
        {
            {"vesion", SignalRConfigurationSection.Settings.Version}
        };

        /// <summary>
        /// Accion que se dispara cuando se cierra la conexion
        /// </summary>
        public Action onClosed;


        /// <summary>
        /// Accion que se dispara cuando se reestablece la conexion
        /// </summary>
        public Action onReconected;
        public Action OnReconnecting;
        public Action OnError;
        public Action OnConnectionSlow;
        public Action OnStateChanged;

        /// <summary>
        /// Dispose interface
        /// </summary>
        public void Dispose()
        {
            hub?.Dispose();
        }

        /// <summary>
        /// Inicializa la conexion
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> StartConnection(string token, string clientID)
        {
            try
            {
                if (hub != null && !string.IsNullOrEmpty(hub.ConnectionId))
                {
                    Logger.Log("DONT START HUB CONNECTION " + clientID, LogPriority.Information);
                    return false;
                }

                Logger.Log($"Url connection {SignalRConfigurationSection.Settings.UrlServer} clientID {clientID}");

                hub = new HubConnection(SignalRConfigurationSection.Settings.UrlServer, queryString, false);
                hub.Headers.Add("Authorization", $"Bearer {token}");
                hub.Headers.Add("ClientID", $"{clientID}");
                hub.Closed += Hub_Closed;
                hub.Reconnected += Hub_Reconnected;
                hub.Reconnecting += Hub_Reconnecting;
                hub.Error += Hub_Error;
                hub.ConnectionSlow += Hub_ConnectionSlow;
                hub.StateChanged += Hub_StateChanged;

                proxy = hub.CreateHubProxy(SignalRConfigurationSection.Settings.Hub);

                ConfigConnection(proxy);
                Logger.Log("STARTING HUB CONNECTION " + clientID, LogPriority.Information);
                await hub.Start();
                Logger.Log("HUB CONNECTION COMPLETED " + clientID, LogPriority.Information);
            }
            catch (Exception ex)
            {
                if (hub != null)
                {
                    hub.Closed -= Hub_Closed;
                    hub.Reconnected -= Hub_Reconnected;
                    hub.Reconnecting -= Hub_Reconnecting;
                    hub.Error -= Hub_Error;
                    hub.ConnectionSlow -= Hub_ConnectionSlow;
                    hub.StateChanged -= Hub_StateChanged;
                }

                var e = new Exception("STARTING CONNECTION ERROR" + clientID, ex);
                Logger.Log($"Error Message {e.Message} StackTrace {e.StackTrace}", LogPriority.Sentry);
                return false;
            }
            return true;
        }

        private void Hub_StateChanged(StateChange obj)
        {
            Logger.Log("Hub_StateChanged OldState " + obj.OldState + " NewState: " + obj.NewState, LogPriority.Information);
        }

        private void Hub_ConnectionSlow()
        {
            Logger.Log("Hub_ConnectionSlow ", LogPriority.Information);
        }

        private void Hub_Error(Exception obj)
        {
            Logger.Log("Hub_Error: " + hub.LastError?.Message, LogPriority.Information);
            if (obj is WebSocketException)
            {
                Logger.Log("Hub_Error: FORCE DISCONNECT BECAUSE WebSocketException", LogPriority.Information);

                hub?.Stop(new TimeSpan(1000));
                //hub?.Dispose();
                //onClosed?.Invoke();
            }
        }

        private void Hub_Reconnecting()
        {
            Logger.Log("Hub_Reconnecting ", LogPriority.Information);
        }

        public async Task Invoke(string method, params object[] data)
        {
            await proxy.Invoke(method, data);
        }
        /// <summary>
        /// Evento al reestablecer la conexion
        /// </summary>
        private void Hub_Reconnected()
        {
            //if (hub.State == ConnectionState.Connected)
            Logger.Log("Hub_Reconnected ", LogPriority.Information);
            onReconected?.Invoke();
        }

        /// <summary>
        /// Evento que se ejecuta cuando se cierra la conexion
        /// </summary>
        private void Hub_Closed()
        {
            Logger.Log("Hub_Closed ", LogPriority.Information);

            hub?.Stop(new TimeSpan(1000));
            hub?.Dispose();
            onClosed?.Invoke();
        }

        /// <summary>
        /// Configura todos las subscripciones y configuraciones de la conexion
        /// </summary>
        /// <param name="hub"></param>
        private void ConfigConnection(IHubProxy proxy)
        {
            configActions?.Invoke(proxy);
        }

        public async Task Reconnect(string token, string clientID)
        {
            await StartConnection(token, clientID);
        }
    }
}
