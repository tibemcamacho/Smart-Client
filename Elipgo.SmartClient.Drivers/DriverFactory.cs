using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Reflections;
using System;
using System.Linq;
using EnumDrivers = Elipgo.SmartClient.Common.Enum.Drivers;

namespace Elipgo.SmartClient.Drivers
{
    public class DriverFactory : IDriverFactory
    {
        Dahua351DriverFactory dahuaFactory;
        public DriverFactory()
        {
            dahuaFactory = new Dahua351DriverFactory();
        }

        public IDriverLive GetDriverLive(CameraDTO camera, Profile profile, bool initAudio, string nameTab)
        {
            if (camera.Recorders != null)
            {
                if (camera.Recorders.Exists(x => x.ProxyEnabled))
                {
                    var recorder = camera.Recorders.Find(x => x.ProxyEnabled);
                    camera.RecorderId = recorder.Id;
                    var driver = string.IsNullOrEmpty(recorder.Driver) ? camera.Driver : Enum.Parse(typeof(EnumDrivers), recorder.Driver);
                    switch (driver)
                    {
                        case EnumDrivers.RTSP_VREC5_CODER:
                            return new GenericDriver.VlcLiveUserControl(camera, profile, initAudio);
                        default:
                            return new GenericDriver.AxisLiveUserControl(camera, profile, initAudio);
                    }
                }
            }

            switch (camera.Driver)
            {
                case EnumDrivers.RTSPGENERIC:
                    return new GenericDriver.VlcLiveUserControl(camera, profile, initAudio);
                case EnumDrivers.AMC_741_ENCODER:
                case EnumDrivers.AMC_741:
                    return new Axis.AxisLiveUserControl(camera, profile, initAudio);
                case EnumDrivers.GenericDriver:
                    if (camera.ManufactureCode == Manufacturer.Axis)
                    {
                        return new GenericDriver.AxisLiveUserControl(camera, profile, initAudio);
                    }
                    else
                    {
                        return new GenericDriver.VlcLiveUserControl(camera, profile, initAudio);
                    }
                //case EnumDrivers.RTSP_STREAMCODERS:
                //    return new RtspLiveUserControl(camera, profile);
                case EnumDrivers.NETSDK_352:
                    return new Dahua352.DahuaLiveUserControl(camera, profile, initAudio);
                case EnumDrivers.KURENTO:
                    return new Kurento.KurentoLiveUserControl(camera, profile, initAudio);
                case EnumDrivers.HCNetSDK_616:
                    return new HCNet616.HikvisionLiveUserControl(camera, profile, initAudio);
                case EnumDrivers.HCNetSDK_619:
                    return new HCNet619.HikvisionLiveUserControl(camera, profile, initAudio);
                case EnumDrivers.NETSDK_351:
                case EnumDrivers.NETSDK_351v2:
                case EnumDrivers.NETSDK_351v3:
                case EnumDrivers.NETSDK_351v4:
                case EnumDrivers.NETSDK_351v5:
                case EnumDrivers.NETSDK_351v6:
                case EnumDrivers.NETSDK_351v7:
                case EnumDrivers.NETSDK_351v8:
                case EnumDrivers.NETSDK_351v9:
                case EnumDrivers.NETSDK_351v10:
                case EnumDrivers.NETSDK_351v11:
                case EnumDrivers.NETSDK_351v12:
                case EnumDrivers.NETSDK_351GENERIC:
                    return dahuaFactory.GetDriverLive(camera, profile, initAudio, nameTab);
                case EnumDrivers.UNVNetSDK_231:
                    return new UNVNet231.UniviewLiveUserControl(camera, profile, initAudio);
                case EnumDrivers.UNVNetSDK_260:
                    return new UNVNet260.UniviewLiveUserControl(camera, profile, initAudio);
                case EnumDrivers.ONVIFV1:
                case EnumDrivers.ONVIFV2:
                case EnumDrivers.ONVIFV3:
                    return new ONVIF.OnvifLiveUserControl(camera, profile, initAudio);
                default:
                    throw new NotSupportedException();
            }
        }

        public IDriverInstantPlayback GetDriverInstantPlayback(CameraDTO camera, Profile profile, RecorderDTOSmall recorder, DateTime selectedDateTime, string nameTab,
            bool hideControls = false, bool isDiagnostic = false, DateTime? selectedEndDateTime = null)
        {
            var driver = string.IsNullOrEmpty(recorder.Driver) ? camera.Driver : Enum.Parse(typeof(EnumDrivers), recorder.Driver, ignoreCase: true);

            if (recorder != null && (EnumDrivers)driver != EnumDrivers.RTSP_STREAMCODERS && (EnumDrivers)driver != EnumDrivers.RTSP_VREC5_CODER)
            {

                var rec = camera.Recorders.FirstOrDefault(x => x.Id == recorder.Id);
                if (recorder != null)
                {
                    RewriteCameraRecorder(rec, ref camera);
                }
            }

            switch (driver)
            {
                case EnumDrivers.AMC_741_ENCODER:
                case EnumDrivers.AMC_741:
                    return new Axis.AxisInstantPlaybackUserControl(camera, profile, selectedDateTime, hideControls, recorder, isDiagnostic, selectedEndDateTime);
                case EnumDrivers.VREC_4:
                case EnumDrivers.RTSP_STREAMCODERS:
                    return new VRec.VRecInstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder, selectedEndDateTime);
                case EnumDrivers.KURENTO:
                    return new Kurento.KurentoInstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder);
                case EnumDrivers.HCNetSDK_616:
                    return new HCNet616.HikvisionInstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder, isDiagnostic, selectedEndDateTime);
                case EnumDrivers.HCNetSDK_619:
                    return new HCNet619.HikvisionInstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder, selectedEndDateTime);
                case EnumDrivers.SYNC_SERVER:
                    return new SyncServer.SyncServerInstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder);
                case EnumDrivers.GenericDriver:
                case EnumDrivers.NETSDK_351:
                case EnumDrivers.NETSDK_351v2:
                case EnumDrivers.NETSDK_351v3:
                case EnumDrivers.NETSDK_351v4:
                case EnumDrivers.NETSDK_351v5:
                case EnumDrivers.NETSDK_351v6:
                case EnumDrivers.NETSDK_351v7:
                case EnumDrivers.NETSDK_351v8:
                case EnumDrivers.NETSDK_351v9:
                case EnumDrivers.NETSDK_351v10:
                case EnumDrivers.NETSDK_351v11:
                case EnumDrivers.NETSDK_351v12:
                case EnumDrivers.NETSDK_351GENERIC:
                    if (recorder.RecorderType != RecorderType.EDGE)
                    {
                        var r = camera.Recorders.FirstOrDefault(x => x.Id == recorder.Id && x.RecorderType == recorder.RecorderType);
                        RewriteCameraRecorder(r, ref camera);
                    }
                    return dahuaFactory.GetDriverInstantPlayback(camera, profile, recorder, selectedDateTime, nameTab, hideControls, isDiagnostic, selectedEndDateTime);
                case EnumDrivers.NETSDK_352:
                    if (recorder.RecorderType != RecorderType.EDGE)
                    {
                        var r = camera.Recorders.FirstOrDefault(x => x.Id == recorder.Id && x.RecorderType == recorder.RecorderType);
                        RewriteCameraRecorder(r, ref camera);
                    }
                    return new Dahua352.DahuaInstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder, isDiagnostic, selectedEndDateTime);
                case EnumDrivers.UNVNetSDK_231:
                    return new UNVNet231.UniviewInstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder, selectedEndDateTime);
                case EnumDrivers.UNVNetSDK_260:
                    return new UNVNet260.UniviewInstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder, selectedEndDateTime);
                case EnumDrivers.RTSP_VREC5_CODER:
                    return new VRec5.VRec5InstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder, selectedEndDateTime);
                default:
                    return null; // throw new NotSupportedException();
            }
        }

        public IManufactureUri GetDriverApiCgi(CameraDTO camera, int recorderId = 0)
        {
            if (recorderId != 0)
            {
                var recorder = camera.Recorders.FirstOrDefault(x => x.Id == recorderId);
                if (recorder != null)
                {
                    RewriteCameraRecorder(recorder, ref camera);
                }
            }

            return ManufactureUriFactory.Instance(camera, recorderId);
        }

        public IDriverLive GetDriverLive(CameraDTO camera)
        {
            return (IDriverLive)ManufactureUriFactory.Instance(camera);
        }

        public IDriverDownload GetDriverDownload(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName, bool isEdge = false, bool isSyncServer = false)
        {
            try
            {
                if (isSyncServer)
                {
                    return new SyncServer.SynServerDownload(bookmarkGroupElement, camera, fileName);
                }

                IDriverDownload driverDownload;

                if (bookmarkGroupElement.NvrId != null && camera.Recorders.Count > 0 && camera.Recorders.Find(x => x.Id == bookmarkGroupElement.NvrId) != null)
                {
                    var recorder = camera.Recorders.FirstOrDefault(x => x.Id == bookmarkGroupElement.NvrId);
                    if (recorder != null)
                    {
                        RewriteCameraRecorder(recorder, ref camera);
                    }
                }
                //if (bookmarkGroupElement.NvrId != null && camera.Recorders.Count > 0 && camera.Recorders.Find(x => x.Id == bookmarkGroupElement.NvrId) != null)
                //{
                //    var recorder = camera.Recorders.FirstOrDefault(x => x.Id == bookmarkGroupElement.NvrId);
                //    //var driver = Enum.Parse(typeof(EnumDrivers), recorder.Driver);
                //    if (recorder.Driver == EnumDrivers.RTSP_VREC5_CODER.ToString())
                //    {
                //        driverDownload = new VRec5.VRec5Download(bookmarkGroupElement, camera, fileName);
                //    }
                //    else 
                //    {
                //        driverDownload = new Vrec.VrecDownload(bookmarkGroupElement, camera, fileName);
                //    }
                //}
                //else
                //{
                switch (camera.Driver)
                {
                    case EnumDrivers.NETSDK_351:
                    case EnumDrivers.NETSDK_351v2:
                    case EnumDrivers.GenericDriver:
                    case EnumDrivers.NETSDK_351v3:
                    case EnumDrivers.NETSDK_351v4:
                    case EnumDrivers.NETSDK_351v5:
                    case EnumDrivers.NETSDK_351v6:
                    case EnumDrivers.NETSDK_351v7:
                    case EnumDrivers.NETSDK_351v8:
                    case EnumDrivers.NETSDK_351v9:
                    case EnumDrivers.NETSDK_351v10:
                    case EnumDrivers.NETSDK_351v11:
                    case EnumDrivers.NETSDK_351v12:
                    case EnumDrivers.NETSDK_351GENERIC:
                        driverDownload = dahuaFactory.GetDriverDownload(bookmarkGroupElement, camera, fileName);
                        break;
                    case EnumDrivers.NETSDK_352:
                        driverDownload = new Dahua352.DahuaDownload(bookmarkGroupElement, camera, fileName);
                        break;
                    case EnumDrivers.AMC_741_ENCODER:
                        driverDownload = new Axis.AxisDownloadEncoder(bookmarkGroupElement, camera, fileName);
                        break;
                    case EnumDrivers.AMC_741:
                        driverDownload = new Axis.AxisDownload(bookmarkGroupElement, camera, fileName);
                        break;
                    case EnumDrivers.HCNetSDK_616:
                        driverDownload = new HCNet616.HikvisionDownload(bookmarkGroupElement, camera, fileName);
                        break;
                    case EnumDrivers.HCNetSDK_619:
                        driverDownload = new HCNet619.HikvisionDownload(bookmarkGroupElement, camera, fileName);
                        break;
                    case EnumDrivers.UNVNetSDK_231:
                        driverDownload = new UNVNet231.UniviewDownload(bookmarkGroupElement, camera, fileName);
                        break;
                    case EnumDrivers.VREC_4:
                    case EnumDrivers.RTSP_STREAMCODERS:
                        driverDownload = new Vrec.VrecDownload(bookmarkGroupElement, camera, fileName);
                        break;
                    case EnumDrivers.RTSP_VREC5_CODER:
                        driverDownload = new VRec5.VRec5Download(bookmarkGroupElement, camera, fileName);
                        break;
                    default:
                        return null;
                }
                //}
                //driverDownload.OnDispose += DriverDispose;
                return driverDownload;
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" GetDriverDownload {0} ", e), LogPriority.Fatal);
                return null;
            }
        }

        public IDriverDownloadVisualSearch GetDriverVisualSerachDownload(CameraDTO camera)
        {
            switch (camera.Driver)
            {
                case EnumDrivers.NETSDK_351:
                case EnumDrivers.NETSDK_351v2:
                case EnumDrivers.GenericDriver:
                case EnumDrivers.NETSDK_351v3:
                case EnumDrivers.NETSDK_351v4:
                case EnumDrivers.NETSDK_351v5:
                case EnumDrivers.NETSDK_351v6:
                case EnumDrivers.NETSDK_351v7:
                case EnumDrivers.NETSDK_351v8:
                case EnumDrivers.NETSDK_351v9:
                case EnumDrivers.NETSDK_351v10:
                case EnumDrivers.NETSDK_351v11:
                case EnumDrivers.NETSDK_351v12:
                case EnumDrivers.NETSDK_351GENERIC:
                    return dahuaFactory.GetDriverVisualSerachDownload(camera);
                case EnumDrivers.NETSDK_352:
                    return new Dahua352.DahuaDownloadVisualSearch(camera);
                case EnumDrivers.AMC_741:
                    return new Axis.AxisDownloadVisualSearch(camera);
                case EnumDrivers.UNVNetSDK_231:
                    return new UNVNet231.UniviewDownloadVisualSearch(camera);
                case EnumDrivers.UNVNetSDK_260:
                    return null;
                case EnumDrivers.HCNetSDK_616:
                    return new HCNet616.HikVisionDownloadVisualSearch(camera);
                case EnumDrivers.HCNetSDK_619:
                    return new HCNet619.HikVisionDownloadVisualSearch(camera);
                default:
                    return null;
            }
        }

        private void RewriteCameraRecorder(RecorderDTO recorder, ref CameraDTO camera)
        {
            if (recorder != null && recorder.RecorderType != RecorderType.EDGE)
            {
                camera.Host = recorder.Host;
                camera.HttpPort = recorder.HttpPort;
                camera.RtspPort = recorder.RtspPort;
                camera.VideoPort = recorder.VideoPort;
                camera.Channel = recorder.Channel;
                camera.User = recorder.Username;
                camera.Password = recorder.Password;
                camera.Driver = (EnumDrivers)Enum.Parse(typeof(EnumDrivers), recorder.Driver, ignoreCase: true);
            }
        }
    }
}
