using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System.Collections.Generic;
using System.Net.Http;

namespace Elipgo.SmartClient.Common.Reflections
{
    public interface IManufactureUri
    {
        string StreamLiveUri();
        string StreamLiveKurentoUri();
        string KurentoServer();
        string RecordingPlaybackUri();
        string StreamPlaybackUri();
        string StreamPlaybackKurentoUri(System.DateTime starttime, System.DateTime stoptime);
        string PtzControlUri(string code = "", string param1 = "", string param2 = "", string action = "stop");
        string AudioConfigUri();
        string AudioTrasmitUri();
        string PresetListUri();
        string GuardTourUri();
        string GuardTourGetUri(int guardId);
        string CallPresetUri(PresetDTO preset);
        string SavePresetUri(PresetDTO preset);
        string DeletePresetUri(PresetDTO preset);
        string CallGuardUri(ActivateGuardDTO guard);
        string StopGuardUri(ActivateGuardDTO guard);
        string RemoveGuardTourUri(GuardDTO guard);
        IOPortState InputPortState();
        IOPortState OuputPortState();
        void OuputPortChangeState(IOPortState state);
        string ExportRecordingUri(string recordingId, string starttime, string stoptime);
        string SendRequest(string url, HttpMethod method, string data = "", bool vRec5 = false);
        IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false);

        string GetFirmwareVersionUri();
    }
}
