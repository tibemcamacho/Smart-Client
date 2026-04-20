using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{
    public class ElipgoUri : ManufactureUriAbstract
    {
        public ElipgoUri(CameraDTO camera) : base(camera)
        {
        }

        public ElipgoUri(CameraDTO camera, Profile profile) : base(camera, profile)
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
            //http://mqtt-c5cdmx.viva-telmex.com:1880/activacion?vinId=3314&chId=3
            //vind -- DVE
            // Chid numeo canal
            string url = $"{Camera.Protocol}://{Camera.Host}:{Camera.HttpPort}/activacionIO?vinId={Camera.DeviceId}&chId={Camera.Channel}";
            var response = SendRequest(url, HttpMethod.Get);
        }

        public override IOPortState OuputPortState()
        {
            return IOPortState.Inactive;
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
            throw new NotImplementedException();
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
