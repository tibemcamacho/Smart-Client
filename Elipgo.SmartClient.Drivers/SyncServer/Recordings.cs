using System;

namespace Elipgo.SmartClient.Drivers.SyncServer
{
    public class Recordings
    {
        public string Id { get; set; }
        public int CameraId { get; set; }
        public int EntityId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
