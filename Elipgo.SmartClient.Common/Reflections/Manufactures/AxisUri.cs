using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;

namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{
    public class AxisUri : ManufactureUriAbstract
    {
        public AxisUri(CameraDTO camera) : base(camera)
        {
        }

        public AxisUri(CameraDTO camera, Profile profile) : base(camera, profile)
        {
        }

        public override IOPortState InputPortState()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["checkactive"] = Camera.Channel.ToString();
            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-cgi/io/port.cgi",
                Query = query.ToString()
            }.ToString();

            try
            {
                var response = SendRequest(url, HttpMethod.Get);
                return (response.Contains("=active")) ? IOPortState.Active : IOPortState.Inactive;
            }
            catch (Exception)
            {
                return IOPortState.Offline;
            }
        }

        public override void OuputPortChangeState(IOPortState state)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            switch (state)
            {
                case IOPortState.Active:
                    query["action"] = string.Format("{0}:/", Camera.Channel);
                    break;
                case IOPortState.Inactive:
                    query["action"] = string.Format("{0}:\\", Camera.Channel);
                    break;
                default:
                    break;
            }

            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-cgi/io/port.cgi",
                Query = query.ToString()
            }.ToString();

            SendRequest(url, HttpMethod.Get);
        }

        public override IOPortState OuputPortState()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["checkactive"] = Camera.Channel.ToString();

            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-cgi/io/port.cgi",
                Query = query.ToString()
            }.ToString();

            try
            {
                var response = SendRequest(url, HttpMethod.Get);
                return (response.Contains("=active")) ? IOPortState.Active : IOPortState.Inactive;
            }
            catch (Exception)
            {
                return IOPortState.Offline;
            }
        }

        public override string StreamLiveUri()
        {
            if (!this.Camera.Recorders.Exists(x => x.ProxyEnabled))
            {
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                query["camera"] = Camera.Channel.ToString();
                switch (Profile)
                {
                    case Profile.SubStream:
                        query["streamprofile"] = string.IsNullOrEmpty(Camera.SubStream) ? "substream" : Camera.SubStream;
                        break;
                    case Profile.MainStream:
                        query["streamprofile"] = string.IsNullOrEmpty(Camera.MainStream) ? "mainstream" : Camera.MainStream;
                        break;
                    default:
                        break;
                }

                return new UriBuilder()
                {
                    Scheme = string.Format("axrtsp{0}", Camera.Protocol.ToLower()),
                    Host = Camera.Host,
                    Port = Camera.HttpPort,
                    Path = "/axis-media/media.amp",
                    Query = query.ToString()
                }.ToString();
            }
            else
            {
                return new UriBuilder()
                {
                    Scheme = "rtsp",
                    Host = Camera.Recorders[0].Host,
                    Port = Camera.Recorders[0].RtspPort,
                    Path = "/" + Camera.Id.ToString().PadLeft(8, '0'),
                }.ToString();
            }
        }

        public override string StreamLiveKurentoUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["camera"] = Camera.Channel.ToString();
            switch (Profile)
            {
                case Profile.SubStream:
                    query["streamprofile"] = string.IsNullOrEmpty(Camera.SubStream) ? "substream" : Camera.SubStream;
                    break;
                case Profile.MainStream:
                    query["streamprofile"] = string.IsNullOrEmpty(Camera.MainStream) ? "mainstream" : Camera.MainStream;
                    break;
                default:
                    break;
            }

            return new UriBuilder()
            {
                Scheme = "rtsp",
                UserName = Camera.User,
                Password = Camera.Password,
                Host = Camera.Host,
                Port = Camera.RtspPort,
                Path = "/axis-media/media.amp",
                Query = query.ToString()
            }.ToString();
        }

        public override string StreamPlaybackUri()
        {
            return new UriBuilder()
            {
                Scheme = string.Format("axrtsp{0}://", Camera.Protocol.ToLower()),
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-media/media.amp",
                Query = "recordingid="
            }.ToString();
        }

        public override string StreamPlaybackKurentoUri(System.DateTime starttime, System.DateTime stoptime)
        {
            return new UriBuilder()
            {
                Scheme = "rtsp",
                UserName = Camera.User,
                Password = Camera.Password,
                Host = Camera.Host,
                Port = Camera.RtspPort,
                Path = "/axis-media/media.amp",
                Query = "recordingid="
            }.ToString();
        }

        public override string RecordingPlaybackUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["source"] = Camera.Channel.ToString();
            query["recordingid"] = "all";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-cgi/record/list.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string PtzControlUri(string code = "", string param1 = "", string param2 = "", string action = "stop")
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["camera"] = Camera.Channel.ToString();
            if (code.Contains("zoom"))
            {
                query[code] = param2;
            }
            else if (code == "")
            {
            }
            else
            {
                query["move"] = code;
            }

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-cgi/com/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string AudioConfigUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["usergroup"] = "anonymous";
            query["action"] = "list";
            query["group"] = "Audio,AudioSource";
            query["camera"] = Camera.Channel.ToString();

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-cgi/view/param.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string AudioTrasmitUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["camera"] = Camera.Channel.ToString();

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/audio/transmit.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string PresetListUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["camera"] = Camera.Channel.ToString();
            query["query"] = "presetposcam";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/com/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string GuardTourUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "list";
            query["group"] = "GuardTour";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/param.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string CallPresetUri(PresetDTO preset)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["camera"] = Camera.Channel.ToString();
            query["gotoserverpresetno"] = preset.Id.ToString();

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/com/ptz.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string CallGuardUri(ActivateGuardDTO guard)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "update";
            query[$"GuardTour.G{guard.Id}.running"] = "yes";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/param.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string StopGuardUri(ActivateGuardDTO guard)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "update";
            query[$"GuardTour.G{guard.Id}.running"] = "no";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/param.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string SavePresetUri(PresetDTO preset)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["camera"] = Camera.Channel.ToString();
            query["home"] = "no";
            query["setserverpresetname"] = preset.Name;

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/com/ptzconfig.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string DeletePresetUri(PresetDTO preset)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["camera"] = Camera.Channel.ToString();
            query["removeserverpresetname"] = preset.Name;

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/com/ptzconfig.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string RemoveGuardTourUri(GuardDTO guard)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "remove";
            query["group"] = $"GuardTour.G{guard.Id}";

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/param.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string GuardTourGetUri(int guardId)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "list";
            query["group"] = $"GuardTour.G{guardId}";
            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/param.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override string ExportRecordingUri(string recordingId, string starttime, string stoptime)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["schemaversion"] = "1";
            query["recordingid"] = recordingId;
            query["exportformat"] = "matroska";
            query["diskid"] = "SD_DISK";
            query["starttime"] = starttime;
            query["stoptime"] = stoptime;

            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-cgi/record/export/exportrecording.cgi",
                Query = query.ToString()
            }.ToString();
        }

        public override IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false)
        {
            var RecordingList = DownloadRecordingList(Camera.Channel);
            List<TimelineDTO> timeLineList = new List<TimelineDTO>();

            RecordingList.ToList().ForEach(x =>
            {
                var findRecord = (x.RecordingStatus == "completed" && DateTime.Parse(x.StartTime).Date >= DateTime.Parse(startDate).ToUniversalTime().Date && DateTime.Parse(x.StopTime).Date <= DateTime.Parse(endDate).ToUniversalTime().Date) ||
                (x.RecordingStatus == "completed" && DateTime.Parse(x.StartTime).Date <= DateTime.Parse(startDate).ToUniversalTime().Date && DateTime.Parse(x.StopTime).ToUniversalTime().Date >= DateTime.Parse(startDate).ToUniversalTime().Date) ||
                (x.RecordingStatus == "recording" && DateTime.Parse(x.StartTime).Date <= DateTime.Parse(startDate).ToUniversalTime().Date);
                if (findRecord == true)
                {
                    TimelineDTO timeLine = new TimelineDTO();
                    //timeLine.StartTime = DateTime.Parse(x.StartTime).ToUniversalTime().Date <= DateTime.Parse(startDate).ToUniversalTime().Date && x.StopTime == "" ? startDate : x.StartTime;
                    timeLine.StartTime = x.StartTime != String.Empty ? x.StartTime : DateTime.Parse(x.StartTime).ToUniversalTime().Date <= DateTime.Parse(startDate).ToUniversalTime().Date && x.StopTime == "" ? startDate : x.StartTime;
                    //timeLine.EndTime = x.StopTime != "" ? x.StopTime : DateTime.Parse(endDate).ToUniversalTime().Date < DateTime.Parse(DateTime.Now.ToString()).ToUniversalTime().Date ? endDate : dst ? DateTime.Now.ToUniversalTime().AddHours(-1).AddMinutes(Camera.Gmt).ToString("yyyy-MM-ddTHH:mm:ss.fffffZ") : DateTime.Now.ToUniversalTime().AddMinutes(Camera.Gmt).ToString("yyyy-MM-ddTHH:mm:ss.fffffZ");
                    timeLine.EndTime = x.StopTime != "" ? x.StopTime : DateTime.Now.ToUniversalTime().AddMinutes(Camera.Gmt).ToString("yyyy-MM-ddTHH:mm:ss.fffffZ");
                    //timeLineList.Add(timeLine);
                    var start = Convert.ToDateTime(timeLine.StartTime).AddMinutes(-1 * Camera.Gmt);
                    var stop = Convert.ToDateTime(timeLine.EndTime).AddMinutes(-1 * Camera.Gmt);
                    //var start = DateTime.ParseExact(timeLine.StartTime, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture).AddMinutes(-1 * item.Gmt);
                    //var stop = DateTime.ParseExact(timeLine.EndTime, "yyyy-MM-ddTHH:mm:ss.fffffZ", CultureInfo.InvariantCulture).AddMinutes(-1 * item.Gmt);
                    while (start < stop)
                    {
                        DateTime end = start;
                        end = end.AddMinutes(-end.Minute);
                        end = end.AddSeconds(-end.Second - 1);
                        end = end.AddHours(1);
                        if (end > stop)
                        {
                            end = stop;
                        }

                        if (start.Date == DateTime.ParseExact(endDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture).Date && end.Date == DateTime.ParseExact(endDate, "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture).Date)
                        {
                            if (!timeLineList.Exists(y => y.StartTime == start.ToString("yyyy-MM-ddTHH:mm:ss.fffffZ")) && !timeLineList.Exists(y => y.EndTime == end.ToString("yyyy-MM-ddTHH:mm:ss.fffffZ")))
                            {
                            	timeLineList.Add(new TimelineDTO { StartTime = Convert.ToDateTime(start).AddHours(Camera.Dst ? -1 : 0).ToString("yyyy-MM-ddTHH:mm:ss.fffffZ"), EndTime = Convert.ToDateTime(end).AddHours(Camera.Dst ? -1 : 0).ToString("yyyy-MM-ddTHH:mm:ss.fffffZ") });
                            }

                        }

                        start = end.AddSeconds(1);
                    }

                }
            });
            return timeLineList.Select(x => new TimelineDTO { StartTime = x.StartTime, EndTime = x.EndTime }).OrderBy(tl => tl.StartTime).Distinct().ToList();
        }

        private IList<Recording> DownloadRecordingList(int channel)
        {
            try
            {
                Uri uri = new Uri(this.RecordingPlaybackUri());

                using (WebClient webClient = new WebClient())
                {
                    webClient.Credentials = new NetworkCredential(this.Camera.User, this.Camera.Password);
                    string result = webClient.DownloadString(uri);

                    using (StringReader stringReader = new StringReader(result))
                    {
                        XDocument xmlDoc = XDocument.Load(stringReader);
                        var query = from r in xmlDoc.Descendants("recording")
                                    select new Recording
                                    {
                                        RecordingId = (string)r.Attribute("recordingid").Value,
                                        StartTime = Convert.ToDateTime((string)r.Attribute("starttime").Value).ToUniversalTime() >= Convert.ToDateTime((string)r.Attribute("starttimelocal").Value) ?
                                                            (string)r.Attribute("starttimelocal").Value.Replace(r.Attribute("starttimelocal").Value.ToString().Substring(r.Attribute("starttimelocal").Value.ToString().Length - 6), "Z") : (string)r.Attribute("starttime").Value,
                                        StopTime = string.IsNullOrEmpty(r.Attribute("stoptime").Value) ? string.Empty : (Convert.ToDateTime((string)r.Attribute("stoptime").Value).ToUniversalTime() >= Convert.ToDateTime((string)r.Attribute("stoptimelocal").Value) ?
                                                            (string)r.Attribute("stoptimelocal").Value.Replace(r.Attribute("starttimelocal").Value.ToString().Substring(r.Attribute("starttimelocal").Value.ToString().Length - 6), "Z") : (string)r.Attribute("stoptime").Value), //(string)r.Attribute("stoptime").Value,
                                        RecordingStatus = (string)r.Attribute("recordingstatus").Value,
                                        MimeType = r.Descendants("video").Count() > 0 ?
                                            (string)r.Descendants("video").FirstOrDefault().Attribute("mimetype").Value : "",
                                        FrameRate = r.Descendants("video").Count() > 0 ?
                                            (string)r.Descendants("video").FirstOrDefault().Attribute("framerate").Value : "",
                                        Audio = (string)((r.Descendants("audio").Count() > 0) ? "yes" : "no")
                                    };

                        IList<Recording> RecordingList = new List<Recording>();
                        if (query.Count() > 0)
                        {
                            foreach (Recording r in query.ToList<Recording>())
                            {
                                RecordingList.Add(r);
                            }
                        }
                        return RecordingList;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("DownloadRecordingList Exception Camera {0} : {1}", Camera.Name, ex), LogPriority.Fatal);
                throw ex;
            }
        }

        public override string GetFirmwareVersionUri()
        {
            try
            {
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                query["action"] = "list";
                query["group"] = "root.Properties.Firmware.Version";

                return new UriBuilder()
                {
                    Scheme = Camera.Protocol,
                    Host = Camera.Host,
                    Port = Camera.HttpPort,
                    Path = "/axis-cgi/param.cgi",
                    Query = query.ToString()
                }.ToString();
            }
            catch
            {
                return "Firmware version: N/A";
            }
        }
    }
}