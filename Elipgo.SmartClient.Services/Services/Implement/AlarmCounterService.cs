using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.SignalR.Connection;
using Splat;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    /// <summary>
    /// Servicio centralizado para el contador de alarmas.
    /// Singleton que maneja SignalR y notifica a todos los listeners.
    /// SIMPLIFICADO: Sin batching, incremento inmediato cuando llega CardDto.
    /// </summary>
    public class AlarmCounterService : IAlarmCounterService
    {
        private readonly IAlarmService _alarmService;
        private readonly VmonitoringManagerConnection _signal;
        private IAppAuthorization _appAuthorization;

        private string _userToken;
        private volatile bool _isInitialized = false;
        private int _listAlarm = 0;
        private int _lastNotifiedCount = -1;
        private int _isSyncingWithServer = 0;

        // Cache para evitar reflexion repetitiva
        private static readonly ConcurrentDictionary<string, (bool hasAttribute, string authorization)> _alarmTypeCache
            = new ConcurrentDictionary<string, (bool, string)>();

        // Cache de UserRoles parseados
        private HashSet<int> _cachedUserProfiles = null;

        public event Action<int> AlarmCountChanged;
        public event Action<object> NewAlarmReceived;

        public int CurrentCount => _listAlarm;
        public bool IsInitialized => _isInitialized;

        public AlarmCounterService()
        {
            _alarmService = Locator.Current.GetService<IAlarmService>();
            _signal = Locator.Current.GetService<VmonitoringManagerConnection>();
        }

        public async Task Initialize(string userToken, string userRoles)
        {
            if (_isInitialized)
            {
                Logger.Log("AlarmCounterService: Ya inicializado, ignorando", LogPriority.Warning);
                return;
            }

            _userToken = userToken;
            ParseUserRoles(userRoles);

            // Cargar contador inicial del servidor
            try
            {
                var count = _alarmService == null ? 0 : await _alarmService.GetUnattended(_userToken);
                if (count < 0) count = 0;

                Interlocked.Exchange(ref _listAlarm, count);
                Logger.Log($"AlarmCounterService: Inicializado con {count} alarmas", LogPriority.Information);

                // Forzar notificación inicial (establecer _lastNotifiedCount DESPUÉS de notificar)
                _lastNotifiedCount = -1;  // Valor diferente para forzar la notificación
                NotifyCountChanged(count);
            }
            catch (Exception ex)
            {
                Logger.Log($"AlarmCounterService: Error al obtener contador inicial: {ex.Message}", LogPriority.Warning);
            }

            // Marcar como inicializado ANTES de suscribirse a eventos
            _isInitialized = true;

            // Suscribirse a eventos de SignalR
            if (_signal != null)
            {
                _signal.AlarmEventAction += OnAlarmEvent;
                _signal.OnReconected += OnReconnected;
                _signal.RefreshAlarmsEventAction += OnRefreshAlarms;
            }
        }

        private void ParseUserRoles(string userRoles)
        {
            try
            {
                if (!string.IsNullOrEmpty(userRoles) && userRoles.Length > 2)
                {
                    string cleanRoles = userRoles.Substring(1, userRoles.Length - 2);
                    int[] profiles = Array.ConvertAll(cleanRoles.Split(','), s => int.Parse(s.Trim()));
                    _cachedUserProfiles = new HashSet<int>(profiles);
                }
                else
                {
                    _cachedUserProfiles = new HashSet<int>();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"AlarmCounterService: Error parseando roles: {ex.Message}", LogPriority.Warning);
                _cachedUserProfiles = new HashSet<int>();
            }
        }

        private void OnAlarmEvent(dynamic d)
        {
            if (!_isInitialized || d == null) return;

            try
            {
                var t = Newtonsoft.Json.JsonConvert.DeserializeObject<CardDto>(d.ToString());
                string alarmType = (t as CardDto)?.Type;

                if (string.IsNullOrEmpty(alarmType)) return;

                var cached = _alarmTypeCache.GetOrAdd(alarmType, (type) =>
                {
                    Type enumType = typeof(AlarmType);
                    MemberInfo mi = enumType.GetMember(type).FirstOrDefault(m => m.GetCustomAttribute(typeof(AlarmTypeOf)) != null);
                    if (mi != null)
                    {
                        AlarmTypeOf subAttr = (AlarmTypeOf)mi.GetCustomAttribute(typeof(AlarmTypeOf));
                        GroupAlarmType groupAlarmType = subAttr.AlarmType;
                        string auth = groupAlarmType.GetDescription();
                        return (true, auth);
                    }
                    return (false, null);
                });

                if (cached.hasAttribute)
                {
                    if (_appAuthorization == null)
                    {
                        _appAuthorization = Locator.Current.GetService<IAppAuthorization>();
                    }

                    if (_appAuthorization != null && _appAuthorization.Exist(cached.authorization))
                    {
                        if (ShouldShowAlarm(t))
                        {
                            var newCount = Interlocked.Increment(ref _listAlarm);
                            NotifyCountChanged(newCount);
                            NewAlarmReceived?.Invoke(t);
                        }
                    }
                }
                else
                {
                    // Tipo sin atributo - incrementar contador
                    var newCount = Interlocked.Increment(ref _listAlarm);
                    NotifyCountChanged(newCount);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"AlarmCounterService.OnAlarmEvent Exception: {ex.Message}", LogPriority.Warning);
            }
        }

        private bool ShouldShowAlarm(CardDto t)
        {
            if (_cachedUserProfiles == null || _cachedUserProfiles.Count == 0)
                return true;

            if (t.Profile != null && t.Profile.Count > 0)
            {
                return _cachedUserProfiles.Overlaps(t.Profile);
            }

            return true;
        }

        private async void OnReconnected(bool connected)
        {
            if (connected && _isInitialized)
            {
                await SyncWithServer();
            }
        }

        private async void OnRefreshAlarms(dynamic idAlarm)
        {
            if (!_isInitialized) return;

            // RefreshAlarms indica que se atendió una alarma, sincronizar con servidor
            await SyncWithServer();
        }

        public async Task SyncWithServer()
        {
            if (string.IsNullOrEmpty(_userToken) || _alarmService == null)
                return;

            // Evitar llamadas concurrentes
            if (Interlocked.CompareExchange(ref _isSyncingWithServer, 1, 0) != 0)
                return;

            try
            {
                var serverCount = await _alarmService.GetUnattended(_userToken);
                if (serverCount < 0)
                {
                    return;
                }

                if (serverCount != _listAlarm)
                {
                    Logger.Log($"AlarmCounterService: Sincronizando {_listAlarm} -> {serverCount}", LogPriority.Information);
                    Interlocked.Exchange(ref _listAlarm, serverCount);
                    NotifyCountChanged(serverCount);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"AlarmCounterService.SyncWithServer error: {ex.Message}", LogPriority.Warning);
            }
            finally
            {
                Interlocked.Exchange(ref _isSyncingWithServer, 0);
            }
        }

        private void NotifyCountChanged(int count)
        {
            if (_lastNotifiedCount != count)
            {
                _lastNotifiedCount = count;
                AlarmCountChanged?.Invoke(count);
            }
        }

        public void Stop()
        {
            if (!_isInitialized) return;

            try
            {
                if (_signal != null)
                {
                    _signal.AlarmEventAction -= OnAlarmEvent;
                    _signal.OnReconected -= OnReconnected;
                    _signal.RefreshAlarmsEventAction -= OnRefreshAlarms;
                }

                _isInitialized = false;
                Logger.Log("AlarmCounterService: Detenido", LogPriority.Information);
            }
            catch (Exception ex)
            {
                Logger.Log($"AlarmCounterService.Stop error: {ex.Message}", LogPriority.Warning);
            }
        }
    }
}
