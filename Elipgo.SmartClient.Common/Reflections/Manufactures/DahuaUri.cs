using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{
    public class DahuaUri : ManufactureUriAbstract
    {
        public DahuaUri(CameraDTO camera) : base(camera)
        {
        }
        public DahuaUri(CameraDTO camera, Profile profile) : base(camera, profile)
        {
        }

        public override IOPortState InputPortState()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "getInStates";
            query["channel"] = (Camera.Channel - 1).ToString();

            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/alarm.cgi",
                Query = query.ToString()
            }.ToString();
            var result = SendRequest(url, HttpMethod.Get);
            if (result.Trim().Replace("result=", "") == "1")
            {
                return IOPortState.Active;
            }
            else
            {
                return IOPortState.Inactive;
            }
        }

        public override void OuputPortChangeState(IOPortState state)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            var url = new UriBuilder();
            if (Camera.ChannelType.ToString() == "VI")
            {
                query["action"] = "control";
                query["channel"] = Camera.Channel.ToString();
                query[string.Format("info[0].Type")] = Camera.Channel.ToString();
                query[string.Format("info[0].IO")] = state == IOPortState.Active ? "1" : "2";
                url.Scheme = Camera.Protocol;
                url.Host = Camera.Host;
                url.Port = Camera.HttpPort;
                url.Path = "/cgi-bin/coaxialControlIO.cgi";
                url.Query = query.ToString();
                url.Query = url.Query.Replace(string.Format("%5b"), "[").Replace(string.Format("%5d"), "]");
            }
            else { 
            query["action"] = "setConfig";
            query[string.Format("AlarmOut[{0}].Mode", (Camera.Channel - 1).ToString())] = (state == IOPortState.Active ? "1" : "0");

            url.Scheme = Camera.Protocol;
            url.Host = Camera.Host;
            url.Port = Camera.HttpPort;
            url.Path = "/cgi-bin/configManager.cgi";
            url.Query = query.ToString();
            }
        

            var response = SendRequest(url.ToString().Replace("??", "?"), HttpMethod.Get);
        }

        public override IOPortState OuputPortState()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (Camera.ChannelType.ToString() == "VI")
            {
                query["action"] = "getStatus";
                query["channel"] = (Camera.Channel).ToString();

                var url = new UriBuilder()
                {
                    Scheme = Camera.Protocol,
                    Host = Camera.Host,
                    Port = Camera.HttpPort,
                    Path = "/cgi-bin/coaxialControlIO.cgi",
                    Query = query.ToString()
                }.ToString();

                try
                {
                    var result = SendRequest(url, HttpMethod.Get);
                    var values = new Dictionary<string, string>();

                    foreach (string line in result.Split('\n'))
                    {
                        var parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            string key = parts[0].Trim();
                            string value = parts[1].Trim();
                            values[key] = value;
                        }
                    }
                    if (Camera.Channel == 1)
                    {
                        if (values["status.status.WhiteLight"] == "Off")
                        {
                            return IOPortState.Inactive;
                        }
                        else
                        {
                            return IOPortState.Active;
                        }
                    }
                    else
                    {
                        if (values["status.status.Speaker"] == "Off")
                        {
                            return IOPortState.Inactive;
                        }
                        else
                        {
                            return IOPortState.Active;
                        }
                    }
                }
                catch (Exception)
                {
                    return IOPortState.Offline;
                }
            }
            else {
                query["action"] = "getOutStates";
                query["channel"] = (Camera.Channel - 1).ToString();

                var url = new UriBuilder()
                {
                    Scheme = Camera.Protocol,
                    Host = Camera.Host,
                    Port = Camera.HttpPort,
                    Path = "/cgi-bin/alarm.cgi",
                    Query = query.ToString()
                }.ToString();

                try
                {
                    var result = SendRequest(url, HttpMethod.Get);
                    if (result.Trim().Replace("result=", "") == "0")
                    {
                        return IOPortState.Inactive;
                    }
                    else
                    {
                        return IOPortState.Active;
                    }
                }
                catch (Exception)
                {
                    return IOPortState.Offline;
                }
            }
        }

        public override string PtzControlUri(string code, string param1, string param2, string action = "stop")
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = action;
            query["channel"] = Camera.Channel.ToString();
            query["code"] = code;
            query["arg1"] = param1;
            query["arg2"] = param2;
            query["arg3"] = "0";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string StreamLiveUri()
        {
            var vrec = Camera?.Recorders?.FirstOrDefault(c => c.Id == Camera.RecorderId);

            if (vrec == null)
            {
                return string.Empty;
            }

            return new UriBuilder()
            {
                Scheme = "rtsp",
                Host = vrec.Host,
                Port = vrec.RtspPort,
                Path = "/" + Camera.Id.ToString("D8")
            }.ToString();
        }

        public override string StreamLiveKurentoUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["channel"] = Camera.Channel.ToString();
            switch (Profile)
            {
                case Profile.SubStream:
                    query["subtype"] = "1";
                    break;
                case Profile.MainStream:
                default:
                    query["subtype"] = "0";
                    break;
            }

            return new System.UriBuilder()
            {
                Scheme = "rtsp",
                UserName = Camera.User,
                Password = Camera.Password,
                Host = Camera.Host,
                Port = Camera.RtspPort,
                Path = "/cam/realmonitor",
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
            //PROTOCOLO://USUARIO:CLAVE@HOST:PUERTO_RTSP/cam/playback?channel=NUMERO_CANAL&subtype=0&starttime=2021_02_14_06_00_10&endtime=2021_02_14_06_30_00 (la fecha va en ese formato)
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["channel"] = Camera.Channel.ToString();
            query["starttime"] = starttime.ToString("yyyy_MM_dd_HH_mm_ss");
            query["endtime"] = stoptime.ToString("yyyy_MM_dd_HH_mm_ss");
            //rtsp://<username>:<password>@<ip>:<port>/<filename>
            //rtsp://admin:admin@10.7.6.67:554//mnt/sd/2012-07-13/001/dav/09/09.30.37-09.30.47[R][0@0][0].dav
            return new System.UriBuilder()
            {
                Scheme = "rtsp",
                UserName = Camera.User,
                Password = Camera.Password,
                Host = Camera.Host,
                Port = Camera.RtspPort,
                Path = "/cam/playback",
                Query = query.ToString()
            }.ToString();
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
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "start";
            query["channel"] = Camera.Channel.ToString();
            query["code"] = "GotoPreset";
            query["arg1"] = "0";
            query["arg2"] = preset.Id.ToString();
            query["arg3"] = "0";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string CallGuardUri(ActivateGuardDTO guard)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "start";
            query["channel"] = Camera.Channel.ToString();
            query["code"] = "StartTour";
            query["arg1"] = guard.Id.ToString();
            query["arg2"] = "0";
            query["arg3"] = "1";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string StopGuardUri(ActivateGuardDTO guard)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            //query["action"] = "stop";
            query["action"] = "start";
            query["channel"] = Camera.Channel.ToString();
            //  query["code"] = "StartTour";
            query["code"] = "StopTour";
            query["arg1"] = guard.Id.ToString();
            query["arg2"] = "0";
            query["arg3"] = "3";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string SavePresetUri(PresetDTO preset)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "start";
            query["channel"] = Camera.Channel.ToString();
            query["code"] = "SetPreset";
            query["arg1"] = "0";
            query["arg2"] = preset.Id.ToString();
            query["arg3"] = "0";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string DeletePresetUri(PresetDTO preset)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "start";
            query["channel"] = Camera.Channel.ToString();
            query["code"] = "ClearPreset";
            query["arg1"] = "0";
            query["arg2"] = preset.Id.ToString();
            query["arg3"] = "0";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string RemoveGuardTourUri(GuardDTO guard)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "start";
            query["channel"] = Camera.Channel.ToString();
            query["code"] = "ClearTour";
            query["arg1"] = guard.Id.ToString();
            query["arg2"] = "0";
            query["arg3"] = "0";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string GuardTourGetUri(int guardId)
        {
            throw new NotImplementedException();
        }

        public override string ExportRecordingUri(string recordingId, string starttime, string stoptime)
        {
            throw new NotImplementedException();
        }

        public string DestroyObjectUri(CameraDTO item, dynamic objectPlayback)
        {
            try
            {
                return new UriBuilder()
                {
                    Scheme = item.Protocol,
                    UserName = Camera.User,
                    Password = Camera.Password,
                    Host = item.Host,
                    Port = item.HttpPort,
                    Path = "/cgi-bin/mediaFileFind.cgi",
                    Query = "action=destroy&object=" + objectPlayback
                }.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string CloseObjectUri(CameraDTO item, dynamic objectPlayback)
        {
            try
            {
                return new UriBuilder()
                {
                    Scheme = item.Protocol,
                    UserName = Camera.User,
                    Password = Camera.Password,
                    Host = item.Host,
                    Port = item.HttpPort,
                    Path = "/cgi-bin/mediaFileFind.cgi",
                    Query = "action=close&object=" + objectPlayback
                }.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string FindObjectUri(CameraDTO item, dynamic objectPlayback)
        {
            try
            {
                return new UriBuilder()
                {
                    Scheme = item.Protocol,
                    UserName = Camera.User,
                    Password = Camera.Password,
                    Host = item.Host,
                    Port = item.HttpPort,
                    Path = "/cgi-bin/mediaFileFind.cgi",
                    Query = "action=findNextFile&object=" + objectPlayback + "&count=50"
                }.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public string ObjectPlaybackUri(CameraDTO camera)
        {
            try
            {
                return new UriBuilder()
                {
                    Scheme = Camera.Protocol,
                    UserName = Camera.User,
                    Password = Camera.Password,
                    Host = Camera.Host,
                    Port = Camera.HttpPort,
                    Path = "/cgi-bin/mediaFileFind.cgi",
                    Query = "action=factory.create"
                }.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false)
        {
            byte numberRetry = 0;
            string resultFind;
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            var retryLimit = int.Parse(config.AppSettings.Settings["retryLimit"].Value);
            double timeReConnectionCheck = 8;
            Random randomReConnection = new Random();

            retry:
            try
            {

                var dahuaObject = this.ObjectPlaybackUri(Camera);
                var resultObject = SendRequest(dahuaObject, HttpMethod.Post);

                //Logger.Log(@"Object 1 ----------> Dahua " +  resultObject, LogPriority.Information);// string.Format("Object 1 ----------> Dahua {0}", resultObject)
                var dahuaObjectString = resultObject.Split('=')[1];
                Logger.Log(string.Format($"Object 2 ----------> Dahua {dahuaObjectString}"), LogPriority.Information);

                dahuaObjectString = dahuaObjectString.Replace("\r", "").Replace("\n", "");
                byte numberRetrylimit = 0;
                retryRetry:
                try
                {
                    var recordingPlaybackUri = RecordingPlaybackUriJuan(Camera.Channel, Camera, dahuaObjectString, startDate.Replace("T", "%20").Substring(0, 21), endDate.Replace("T", "%20").Substring(0, 21), Elipgo.SmartClient.Common.Enum.Profile.MainStream.ToString());
                    var resultRecording = SendRequest(recordingPlaybackUri, HttpMethod.Post);
                    if (resultRecording.Contains("287637505"))
                    {
                        numberRetry++;
                        Logger.Log(string.Format($"Error code 287637505 to GetTimeline retry {numberRetry} "), LogPriority.Fatal);
                        if (numberRetry <= retryLimit)
                        {
                            goto retry;
                        }
                        else
                            return new List<TimelineDTO>();

                    }
                }
                catch (Exception ex)
                {
                    numberRetrylimit++;
                    var endDateTem = Convert.ToDateTime(endDate).ToUniversalTime();
                    DateTime TimeEnd = endDateTem.AddMinutes(-30);
                    endDate = TimeEnd.ToString("yyyy-MM-ddTHH:mm:ssZ");
                    if (numberRetrylimit <= retryLimit)
                    {
                        goto retryRetry;
                    }
                    else
                    {
                        Logger.Log(string.Format($"timeLineList retryRetry -----> count: {numberRetrylimit} , Exception : {ex.Message}"), LogPriority.Important);
                        return new List<TimelineDTO>();
                    }

                }

                var findObjectUri = FindObjectUri(Camera, dahuaObjectString);
                resultFind = SendRequest(findObjectUri, HttpMethod.Post);
                var closeObjectUri = CloseObjectUri(Camera, dahuaObjectString);
                SendRequest(closeObjectUri, HttpMethod.Post);
                var destroyObjectUri = DestroyObjectUri(Camera, dahuaObjectString);
                SendRequest(destroyObjectUri, HttpMethod.Post);

            }
            catch (System.Net.WebException we)
            {
                numberRetry++;
                Logger.Log(string.Format($"Error WebException to GetTimeline retry {numberRetry} Message --> {we.Message} StackTrace --> {we.StackTrace}"), LogPriority.Fatal);
                if (numberRetry <= retryLimit)
                {
                    int r = ((int)(((randomReConnection.NextDouble() * timeReConnectionCheck) + 1) * 1000));
                    Thread.Sleep(r);
                    goto retry;
                }
                else
                    return new List<TimelineDTO>();
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format($"Error Exception ----------> Conection to the camera {Camera.Name} failed: {ex}"), LogPriority.Fatal);
                throw ex;
            }

            IList<TimelineDTO> timeLineList = new List<TimelineDTO>();
            bool hasStart = false;
            bool hasEnd = false;
            TimelineDTO timeLine = new TimelineDTO();
            string line = "";

            using (StringReader resultReader = new StringReader(resultFind))
            {
                while ((line = resultReader.ReadLine()) != null)
                {
                    if (line.Contains("StartTime"))
                    {
                        line = line.Replace("- ", "-0").Replace(": ", ":0");
                        timeLine.StartTime = (line.Contains("UTC") ? line.Substring(26, 19) : line.Substring(19, 19)) + "Z";
                        if (timeLine.StartTime.Contains("="))
                        {
                            timeLine.StartTime = (line.Contains("UTC") ? line.Substring(27, 19) : line.Substring(20, 19)) + "Z";
                        }
                        timeLine.StartTime = timeLine.StartTime.Replace(" ", "T");
                        if (DateTime.Parse(timeLine.StartTime).ToUniversalTime().Hour < DateTime.Parse(startDate).ToUniversalTime().Hour || (DateTime.Parse(timeLine.StartTime).ToUniversalTime().Hour == DateTime.Parse(startDate).ToUniversalTime().Hour && DateTime.Parse(timeLine.StartTime).ToUniversalTime().Minute <= DateTime.Parse(startDate).ToUniversalTime().Minute))
                        {
                            timeLine.StartTime = startDate;
                        }
                        hasStart = true;
                    }
                    if (line.Contains("EndTime"))
                    {
                        line = line.Replace("- ", "-0").Replace(": ", ":0");
                        timeLine.EndTime = (line.Contains("UTC") ? line.Substring(24, 19) : line.Substring(17, 19)) + "Z";


                        if (timeLine.EndTime.Contains("="))
                        {
                            timeLine.EndTime = (line.Contains("UTC") ? line.Substring(25, 19) : line.Substring(18, 19)) + "Z";
                        }
                        timeLine.EndTime = timeLine.EndTime.Replace(" ", "T");
                        if (DateTime.Parse(timeLine.EndTime).ToUniversalTime() >= DateTime.Parse(endDate).ToUniversalTime() || DateTime.Parse(timeLine.EndTime).ToUniversalTime().Hour > DateTime.Parse(endDate).ToUniversalTime().Hour || (DateTime.Parse(timeLine.EndTime).ToUniversalTime().Hour == DateTime.Parse(endDate).ToUniversalTime().Hour && DateTime.Parse(timeLine.EndTime).ToUniversalTime().Minute >= DateTime.Parse(endDate).ToUniversalTime().Minute))
                        {
                            timeLine.EndTime = endDate;
                        }
                        hasEnd = true;
                    }
                    if (hasEnd && hasStart)
                    {
                        timeLineList.Add(timeLine);
                        timeLine = new TimelineDTO();
                        hasStart = false;
                        hasEnd = false;
                    }
                }
            }
            return timeLineList;
        }
        public string RecordingPlaybackUriJuan(int channel, CameraDTO item, dynamic objectPlayback, string startTime, string stopTime, string videoStream)
        {
            try
            {
                return new UriBuilder()
                {
                    Scheme = Camera.Protocol,
                    UserName = Camera.User,
                    Password = Camera.Password,
                    Host = Camera.Host,
                    Port = Camera.HttpPort,
                    Path = "/cgi-bin/mediaFileFind.cgi",
                    Query = $"action=findFile&object=" + objectPlayback + "&condition.Channel=" + channel + "&condition.StartTime=" + startTime + "&condition.EndTime=" + stopTime + "&condition.Types[0]=dav&condition.VideoStream=%23" + videoStream + "%23"
                }.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public override string GetFirmwareVersionUri()
        {
            return string.Empty;
        }

    }
}
