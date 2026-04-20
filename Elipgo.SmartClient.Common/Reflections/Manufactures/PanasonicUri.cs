using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;

namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{
    public class PanasonicUri : ManufactureUriAbstract
    {
        public PanasonicUri(CameraDTO camera, Profile profile) : base(camera, profile)
        {
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

        public override string PtzControlUri(string code = "", string param1 = "", string param2 = "", string action = "stop")
        {
            throw new NotImplementedException();
        }

        public override string StreamLiveUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            switch (Profile)
            {
                case Profile.SubStream:
                    query["profile"] = "def_profile2";
                    break;
                case Profile.MainStream:
                default:
                    query["profile"] = "def_profile1";
                    break;
            }

            return new System.UriBuilder()
            {
                Scheme = "rtsp",
                Host = Camera.Host,
                Port = Camera.RtspPort,
                Path = "/ONVIF/MediaInput",
                Query = query.ToString()
            }.ToString();
        }
        public override string StreamLiveKurentoUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            switch (Profile)
            {
                case Profile.SubStream:
                    query["profile"] = "def_profile2";
                    break;
                case Profile.MainStream:
                default:
                    query["profile"] = "def_profile1";
                    break;
            }

            return new System.UriBuilder()
            {
                Scheme = "rtsp",
                UserName = Camera.User,
                Password = Camera.Password,
                Host = Camera.Host,
                Port = Camera.RtspPort,
                Path = "/ONVIF/MediaInput",
                Query = query.ToString()
            }.ToString();
        }
        public override string RecordingPlaybackUri()
        {
            throw new NotImplementedException();
        }

        public override string StreamPlaybackUri()
        {
            throw new NotImplementedException();
        }
        public override string StreamPlaybackKurentoUri(System.DateTime starttime, System.DateTime stoptime)
        {
            throw new NotImplementedException();
        }

        public override string AudioConfigUri()
        {
            throw new NotImplementedException();
        }

        public override string AudioTrasmitUri()
        {
            throw new NotImplementedException();
        }

        public override string PresetListUri()
        {
            throw new NotImplementedException();
        }

        public override string GuardTourUri()
        {
            throw new NotImplementedException();
        }

        public override string CallPresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string CallGuardUri(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string StopGuardUri(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string SavePresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string DeletePresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string RemoveGuardTourUri(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string GuardTourGetUri(int guardId)
        {
            throw new NotImplementedException();
        }

        public override string ExportRecordingUri(string recordingId, string starttime, string stoptime)
        {
            throw new NotImplementedException();
        }
        public override IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false)
        {
            throw new NotImplementedException();
        }
        public override string GetFirmwareVersionUri()
        {
            return string.Empty;
        }
    }
}
