using System.Threading.Tasks;

namespace Elipgo.SmartClient.Drivers.ONVIF
{
    //HIKVISION
    public class Onvifv2 : AbsOnvif
    {

        public Onvifv2(string user, string pass, string host, int http, int rtcp) : base(user, pass, host, http, rtcp)
        {
            this.ONVIF_GET_PROFILE = "/onvif/media/GetProfile";
            this.ONVIF_GET_STREAM_URI = "/onvif/media/GetStreamUri";
            this.ONVIF_PTZ = "/onvif/ptz/AbsoluteMove";
            this.ONVIF_PTZ_STOP = "/onvif/ptz/Stop";
            this.ONVIF_SNAPSHOT = "/onvif/media/GetSnapshotUri";
            this.ONVIF_PRESET_LIST = "/onvif/ptz/GetPresets";
            this.ONVIF_CALL_PRESET = "/onvif/ptz/GotoPreset";
            this.ONVIf_REMOVE_PRESET = "/onvif/ptz/RemovePreset";
            this.ONVIf_SAVE_PRESET = "/onvif/ptz/SetPreset";
        }

        public override async Task<string> getStreamUri()
        {
            await getInternalBaseUri();
            string urlStr = await getInternalStreamUri();
            return urlStr;
        }
    }
}
