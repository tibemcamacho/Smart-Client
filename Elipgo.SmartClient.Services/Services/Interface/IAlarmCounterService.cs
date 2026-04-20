using System;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    /// <summary>
    /// Servicio centralizado para el contador de alarmas.
    /// Singleton que maneja SignalR y notifica a todos los listeners.
    /// </summary>
    public interface IAlarmCounterService
    {
        /// <summary>
        /// Evento que se dispara cuando el contador cambia
        /// </summary>
        event Action<int> AlarmCountChanged;

        /// <summary>
        /// Evento que se dispara cuando llega una nueva alarma (para UI)
        /// </summary>
        event Action<object> NewAlarmReceived;

        /// <summary>
        /// Contador actual de alarmas sin atender
        /// </summary>
        int CurrentCount { get; }

        /// <summary>
        /// Inicializa el servicio con el token de usuario
        /// </summary>
        Task Initialize(string userToken, string userRoles);

        /// <summary>
        /// Fuerza sincronización con el servidor
        /// </summary>
        Task SyncWithServer();

        /// <summary>
        /// Detiene el servicio
        /// </summary>
        void Stop();

        /// <summary>
        /// Indica si el servicio está inicializado y activo
        /// </summary>
        bool IsInitialized { get; }
    }
}
