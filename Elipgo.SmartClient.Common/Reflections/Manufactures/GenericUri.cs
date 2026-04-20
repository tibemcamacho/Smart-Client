using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Compilation;

namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{
    public class GenericUri : ManufactureUriAbstract
    {
        public GenericUri(CameraDTO camera) : base(camera)
        {
        }
        public GenericUri(CameraDTO camera, Profile profile) : base(camera, profile)
        {
        }
        public override string AudioConfigUri()
        {
            throw new NotImplementedException();
        }

        public override string AudioTrasmitUri()
        {
            throw new NotImplementedException();
        }

        public override string CallGuardUri(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string CallPresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string DeletePresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string ExportRecordingUri(string recordingId, string starttime, string stoptime)
        {
            throw new NotImplementedException();
        }

        public override string GetFirmwareVersionUri()
        {
            throw new NotImplementedException();
        }

        public override IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false)
        {
            throw new NotImplementedException();
        }

        public override string GuardTourGetUri(int guardId)
        {
            throw new NotImplementedException();
        }

        public override string GuardTourUri()
        {
            throw new NotImplementedException();
        }

        public override IOPortState InputPortState()
        {
            throw new NotImplementedException();
        }

        public override void OuputPortChangeState(IOPortState state)
        {
            throw new NotImplementedException();
        }

        public override IOPortState OuputPortState()
        {
            throw new NotImplementedException();
        }

        public override string PresetListUri()
        {
            throw new NotImplementedException();
        }

        public override string PtzControlUri(string code = "", string param1 = "", string param2 = "", string action = "stop")
        {
            throw new NotImplementedException();
        }

        public override string RecordingPlaybackUri()
        {
            throw new NotImplementedException();
        }

        public override string RemoveGuardTourUri(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string SavePresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string StopGuardUri(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string StreamLiveKurentoUri()
        {
            throw new NotImplementedException();
        }

        public override string StreamLiveUri()
        {
            //Camera.Host = "cctvlafayette.ddns.net";
            //Camera.RtspPort = 4054;
            //Camera.User = "view";
            //Camera.Password = "QRPi2wNFYU$A1av";
            var pathUrl = string.IsNullOrEmpty(Camera.MainStream) ? Camera.SubStream : Camera.MainStream;

            return $"rtsp://{Camera.User}:{Camera.Password}@{Camera.Host}:{Camera.RtspPort}/{pathUrl}";
            //return new System.UriBuilder()
            //{
            //    Scheme = "rtsp",
            //    UserName = Camera.User,
            //    Password = Camera.Password,
            //    Host = Camera.Host,
            //    Port = Camera.RtspPort,
            //    //Query = "/cam/realmonitor?channel=2&subtype=1"
            //    Query = $"/live/{(int)Profile + 1}"
            //}.ToString();
        }

        public override string StreamPlaybackKurentoUri(DateTime starttime, DateTime stoptime)
        {
            throw new NotImplementedException();
        }

        public override string StreamPlaybackUri()
        {
            throw new NotImplementedException();
        }
    }
}
