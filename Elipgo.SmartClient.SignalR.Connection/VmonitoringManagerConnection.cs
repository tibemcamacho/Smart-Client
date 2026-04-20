using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.SignalR.Connection.Responses;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace Elipgo.SmartClient.SignalR.Connection
{
    public delegate void VmonitoringManagerConnectionEventHandler(bool connected);

    public class VmonitoringManagerConnection : IDisposable
    {
        public SignalRConectionManager SignarR { get; } = new SignalRConectionManager();

        /// <summary>
        /// Instancia estatica para singleton
        /// </summary>
        private readonly static Lazy<VmonitoringManagerConnection> _instance = new Lazy<VmonitoringManagerConnection>(() => new VmonitoringManagerConnection());
        private static readonly string _clientID = $"{Guid.NewGuid()}-{Guid.NewGuid()}";
        private string _userToken;
        private bool _disconnected;
        private readonly SemaphoreSlim _connLock = new SemaphoreSlim(1, 1);
        private volatile bool _isConnecting = false;

        /// <summary>
        /// Instancia
        /// </summary>
        public static VmonitoringManagerConnection Instance => _instance.Value;
        private VmonitoringManagerConnection()
        {
            this._disconnected = false;
            SignarR.onClosed = onClosed;
            SignarR.onReconected = onReconected;
            SignarR.OnConnectionSlow += OnConnectionSlow;
            SignarR.OnReconnecting += OnReconnecting;
            SignarR.OnError += OnError;
            SignarR.OnStateChanged += OnStateChanged;

            var config = SmartClientEnvironmentUtils.GetConfiguration();
            var logSaveSnapshot = config.AppSettings.Settings["LogSaveSnapshot"].Value;

            //TODO:Configurar todos los eventos a los que hay que suscribirse
            SignarR.configActions = (proxy) =>
            {
                proxy.On<GeneralEventResult<dynamic>>("Event", (e) =>
                {

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(e);

                    switch ((EventTypes)e.EventType)
                    {
                        case EventTypes.Alarms:

                            if (!String.IsNullOrEmpty(logSaveSnapshot) && logSaveSnapshot == "0")
                            {
                                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                jsonObj["ResultEvent"]["Snapshot"] = String.Empty;
                                json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj);
                            }

                            AlarmEventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.AVL:
                            AVLEventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.Cameras:
                            CamerasEventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.FaceRecognition:
                            FaceRecognitionEventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.General:
                            GeneralEventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.LPR:
                            LPREventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.Security:
                            SecurityEventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.Iot:
                            IotStatusEventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.LogoutUser:
                            LogoutUserEventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.RefreshAlarms:
                            RefreshAlarmsEventAction?.Invoke(e.ResultEvent);
                            break;
                        case EventTypes.CamerasState:
                            CameraStateEventAction?.Invoke(e.ResultEvent);
                            break;

                        default:
                            break;
                    }

                    //Logger.Log("Event Received from signalR " + json.ToString(), LogPriority.Information);
                });
            };
        }

        private void OnStateChanged()
        {

        }

        private void OnError()
        {

        }

        private void OnReconnecting()
        {

        }

        private void OnConnectionSlow()
        {

        }

        /// <summary>
        /// Evento que se ejecuta cuando se dispara una accion en el server
        /// </summary>
        public Action<dynamic> AlarmEventAction;
        public Action<dynamic> AVLEventAction;
        public Action<dynamic> CamerasEventAction;
        public Action<dynamic> FaceRecognitionEventAction;
        public Action<dynamic> GeneralEventAction;
        public Action<dynamic> LPREventAction;
        public Action<dynamic> SecurityEventAction;
        public Action<dynamic> IotStatusEventAction;
        public Action<dynamic> LogoutUserEventAction;
        public Action<dynamic> RefreshAlarmsEventAction;
        public Action<dynamic> CameraStateEventAction;

        /// <summary>
        /// Accion que se dispara cuando se cierra la conexion
        /// </summary>
        //public Action onClosed;
        public event VmonitoringManagerConnectionEventHandler OnClosed;

        /// <summary>
        /// Accion que se dispara cuando se reestablece la conexion
        /// </summary>
        //public Action onReconected;
        public event VmonitoringManagerConnectionEventHandler OnReconected;

        /// <summary>
        /// Metodo para conectarse al server e inicializar signalR
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ConnectoToServer(string token)
        {
            //if ((string.IsNullOrEmpty(token) || token == this.userToken) && bDisconnected == true)
            if (string.IsNullOrEmpty(token) && _disconnected == true)
            {
                Logger.Log($"SignalR Can't Connect, missed UserToken - {(string.IsNullOrEmpty(token) ? "" : token)}, CliendID: {_clientID}", LogPriority.Information);
                return;
            }

            if (_disconnected)
            {
                Logger.Log($"SignalR already connected. Skipping connect. {_clientID}", LogPriority.Information);
                return;
            }

            if (_isConnecting)
            {
                Logger.Log($"SignalR connect in progress. Skipping duplicate attempt. {_clientID}", LogPriority.Information);
                return;
            }

            await _connLock.WaitAsync().ConfigureAwait(false);
            try
            {
                if (_disconnected)
                {
                    Logger.Log($"SignalR already connected (double-check). Skipping connect. {_clientID}", LogPriority.Information);
                    return;
                }
                if (_isConnecting)
                {
                    Logger.Log($"SignalR connect in progress (double-check). Skipping duplicate attempt. {_clientID}", LogPriority.Information);
                    return;
                }

                _isConnecting = true;
                this._userToken = token;
                try
                {
                    var result = await SignarR.StartConnection(token, _clientID).ConfigureAwait(false);
                    if (result)
                    {
                        Logger.Log($"SIGNAL R START CONNECTION COMPLETED {_clientID}", LogPriority.Information);
                        await SignarR.Invoke("RegisterToAllEvents").ConfigureAwait(false);
                        // Use safe raiser to invoke subscribers
                        RaiseOnClosed(false);
                        Logger.Log($"SignalR Connected {_clientID}", LogPriority.Information);
                        _disconnected = true;
                    }
                }
                catch (Exception ex)
                {
                    var e = new Exception($"SignalR Exception {_clientID}", ex);
                    Logger.Log(e, LogPriority.Sentry);
                    await Task.Delay(15000).ContinueWith(t => ConnectoToServer(_userToken)).ConfigureAwait(false);
                }
            }
            finally
            {
                _isConnecting = false;
                _connLock.Release();
            }
        }

        private void onReconected()
        {
            Logger.Log($"SignalR onReconected {_clientID}", LogPriority.Information);
            // Use safe raiser
            RaiseOnReconected(true);
        }

        private void onClosed()
        {
            Logger.Log($"SignalR onClosed disconected {_clientID}", LogPriority.Information);
            _disconnected = false;
            Task.Run(() => ConnectoToServer(_userToken));
            // Use safe raiser
            RaiseOnClosed(true);
        }

        private void RaiseOnClosed(bool connected)
        {
            try
            {
                var handlers = OnClosed;
                var list = handlers?.GetInvocationList();
                Logger.Log($"RaiseOnClosed handlers={(list?.Length ?? 0)}, connected={connected}", LogPriority.Information);
                if (list == null) return;
                foreach (VmonitoringManagerConnectionEventHandler h in list)
                {
                    try { h.BeginInvoke(connected, null, null); }
                    catch (Exception ex) { Logger.Log(ex, LogPriority.Sentry); }
                }
            }
            catch (Exception ex) { Logger.Log(ex, LogPriority.Sentry); }
        }

        private void RaiseOnReconected(bool connected)
        {
            try
            {
                var handlers = OnReconected;
                var list = handlers?.GetInvocationList();
                Logger.Log($"RaiseOnReconected handlers={(list?.Length ?? 0)}, connected={connected}", LogPriority.Information);
                if (list == null) return;
                foreach (VmonitoringManagerConnectionEventHandler h in list)
                {
                    try { h.BeginInvoke(connected, null, null); }
                    catch (Exception ex) { Logger.Log(ex, LogPriority.Sentry); }
                }
            }
            catch (Exception ex) { Logger.Log(ex, LogPriority.Sentry); }
        }

        public async Task Reconnect(string token)
        {
            await SignarR.Reconnect(token, _clientID);
        }

        public void Dispose()
        {
            Logger.Log("SignalR Dispose " + _clientID, LogPriority.Information);
            SignarR.Dispose();
        }
    }
}
