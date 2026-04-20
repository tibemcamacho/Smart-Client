using Elipgo.SmartClient.Common.Enum;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.DTOs
{
    public class CameraDTO 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public Drivers Driver { get; set; }
        public string Host { get; set; }
        public Manufacturer ManufactureCode { get; set; }
        public int HttpPort { get; set; }
        public int RtspPort { get; set; }
        public int VideoPort { get; set; }
        public int Channel { get; set; }
        public ChannelType ChannelType { get; set; }

        public string ProxyUrl { get; set; }
        public string ProxyUser { get; set; }
        public string ProxyPassword { get; set; }
        public string Protocol { get; set; }
        public bool PtzEnabled { get; set; }
        public bool AudioEnabled { get; set; }
        public bool TalkEnabled { get; set; }
        public int Gmt { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public double ZoomSensitivity { get; set; }
        public double MovementSensitivity { get; set; }
        public string MainStream { get; set; }
        public string SubStream { get; set; }
        public string Codec { get; set; }
        public List<RecorderDTO> Recorders { get; set; }
        public int DeviceId { get; set; }
        public string Ws_Uri { get; set; }
        public string Ws_Protocol { get; set; }
        public int Ws_HttpPort { get; set; }
        public bool EdgeEnabled { get; set; }
        public List<SyncServerDTO> SyncServers { get; set; }
        public SequencingDTO Sequencing { get; set; }
        public string RecordingMode { get; set; }
        public int RecorderId { get; set; }
        public string IdGuid { get; set; }
        public bool Dst { get; set; }
    }

    public class CameraStateDTO
    {
        public int Id { get; set; }
        public bool State { get; set; }
        public int SiteId { get; set; }
        public string RecorderDriver { get; set; }
        public int RecorderId { get; set; }
        public string Apps { get; set; }
    }

    public class CameraStatesDTO
    {
        public int Id { get; set; }
        public string RecorderDriver { get; set; }
        public int RecorderId { get; set; }

        public string Apps { get; set; }
        public string RecorderType { get; set; }
    }

    public class CameraStatesLiveDTO
    {
        public int Id { get; set; }
        public string Apps { get; set; }
    }
}

