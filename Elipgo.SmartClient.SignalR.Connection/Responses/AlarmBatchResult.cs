using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;

namespace Elipgo.SmartClient.SignalR.Connection.Responses
{
    /// <summary>
/// DTO para recibir batches de alarmas desde el servidor SignalR.
    /// Cuando el backend tiene el "SignalR Notification Batcher" habilitado,
    /// puede enviar múltiples alarmas en un solo mensaje para reducir overhead.
    /// </summary>
    public class AlarmBatchResult
    {
        /// <summary>
        /// Total de alarmas en este batch
   /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Indica si este es un batch completo o un resumen
     /// </summary>
        public bool IsSummary { get; set; }

   /// <summary>
        /// Lista de alarmas en el batch
        /// </summary>
        public List<CardDto> Alarms { get; set; } = new List<CardDto>();

     /// <summary>
    /// Timestamp del servidor cuando se creó el batch
        /// </summary>
        public long Timestamp { get; set; }

      /// <summary>
    /// ID del batch para correlación/deduplicación
        /// </summary>
        public string BatchId { get; set; }
    }
}
