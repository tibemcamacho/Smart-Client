using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Xml.Linq;


namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{


    public class HikvisionUri : ManufactureUriAbstract
    {
        int numberRetry = 0;
        string HikvisionXML = @"&lt;?xml version=""1.0"" encoding=""UTF-8""?&gt;&lt;CMSearchDescription version = ""1.0"" xmlns=""http://www.isapi.org/ver20/XMLSchema""&gt;&lt;searchID&gt;GUID&lt;/searchID&gt;
                                    &lt;trackIDList&gt;
                                        &lt;trackID&gt;CHANNELRECORDERMODE&lt;/trackID&gt;
                                    &lt;/trackIDList&gt;
                                    &lt;timeSpanList&gt;
                                        &lt;timeSpan&gt;
                                            &lt;startTime&gt;STARTDATET00:00:00Z&lt;/startTime&gt;
                                            &lt;endTime&gt;ENDDATET23:59:00Z&lt;/endTime&gt;
                                        &lt;/timeSpan&gt;
                                    &lt;/timeSpanList&gt;
                                    &lt;contentTypeList&gt;
                                        &lt;contentType&gt;video&lt;/contentType&gt;
                                    &lt;/contentTypeList&gt;
                                    &lt;maxResults&gt;40&lt;/maxResults&gt;
                                    &lt;metadataList&gt;
                                        &lt;metadataDescriptor&gt;recordType.meta.std-cgi.com/TYPE&lt;/metadataDescriptor&gt;
                                    &lt;/metadataList&gt;
                                &lt;/CMSearchDescription&gt;";
        public HikvisionUri(CameraDTO camera) : base(camera)
        {
        }

        public HikvisionUri(CameraDTO camera, Profile profile) : base(camera, profile)
        {
        }

        public override IOPortState InputPortState()
        {
            ///ISAPI/ContentMgmt/InputProxy/channels/
            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = string.Format(@"/ISAPI/System/IO/inputs/{0}/status", Camera.Channel)
            }.ToString();

            try
            {
                var httpResponseBody = SendRequest(url, HttpMethod.Get);
                string strResult = httpResponseBody.ToString().Replace("\r\n", "").Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Replace("version=\"2.0\" xmlns=\"http://www.hikvision.com/ver20/XMLSchema\"", "").Trim();
                var xdoc = XDocument.Parse(strResult);
                if (xdoc.Root.Elements("ioState").FirstOrDefault().Value == "active")
                {
                    Logger.Log(String.Format("HikVision InputPortState {0} changed to active", Camera.Name), LogPriority.Information);
                    return IOPortState.Active;
                }
                else
                {
                    Logger.Log(String.Format("HikVision InputPortState {0} changed to inactive", Camera.Name), LogPriority.Information);
                    return IOPortState.Inactive;
                }
            }
            catch (Exception e)
            {
                Logger.Log(String.Format("HikVision InputPortState Exception:{0}", e), LogPriority.Fatal);
                return IOPortState.Offline;
            }
        }

        public override void OuputPortChangeState(IOPortState state)
        {
            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = string.Format(@"/ISAPI/System/IO/outputs/{0}/trigger", Camera.Channel),
            }.ToString();

            try
            {
                var httpResponseBody = SendRequest(url, HttpMethod.Put, string.Format(@"<IOPortData><outputState>{0}</outputState></IOPortData>", state == IOPortState.Active ? "high" : "low"));
                //string strResult = httpResponseBody.ToString().Replace("\r\n", "").Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Replace("version=\"2.0\" xmlns=\"http://www.hikvision.com/ver20/XMLSchema\"", "").Trim();
                //var xdoc = XDocument.Parse(strResult);

            }
            catch (Exception e)
            {
                Logger.Log(String.Format("HikVision OuputPortChangeState Exception:{0}", e), LogPriority.Fatal);
            }
        }

        public override IOPortState OuputPortState()
        {
            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = string.Format(@"/ISAPI/System/IO/outputs/{0}/status", Camera.Channel)
            }.ToString();

            try
            {
                var httpResponseBody = SendRequest(url, HttpMethod.Get);
                string strResult = httpResponseBody.ToString().Replace("\r\n", "").Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Replace("version=\"2.0\" xmlns=\"http://www.hikvision.com/ver20/XMLSchema\"", "").Trim();
                var xdoc = XDocument.Parse(strResult);
                if (xdoc.Root.Elements("ioState").FirstOrDefault().Value == "active")
                {
                    Logger.Log(String.Format("HikVision OuputPortState {0} changed to active", Camera.Name), LogPriority.Information);
                    return IOPortState.Active;
                }
                else
                {
                    Logger.Log(String.Format("HikVision OuputPortState {0} changed to inactive", Camera.Name), LogPriority.Information);
                    return IOPortState.Inactive;
                }

            }
            catch (Exception e)
            {
                Logger.Log(String.Format("HikVision OuputPortState Exception:{0}", e), LogPriority.Fatal);
                return IOPortState.Offline;
            }
        }

        public override string PtzControlUri(string code = "", string param1 = "", string param2 = "", string action = "stop")
        {
            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = string.Format(@"/ISAPI/PTZCtrl/channels/{0}/continuous", Camera.Channel.ToString())
            }.ToString();
        }

        public override string StreamLiveUri()
        {
            var path = "/ISAPI/streaming/channels/" + Camera.Channel.ToString();
            switch (Profile)
            {
                case Profile.SubStream:
                    path += "01";
                    break;
                case Profile.MainStream:
                default:
                    path += "01";
                    break;
            }

            return new System.UriBuilder()
            {
                Scheme = "rtsp",
                Host = Camera.Host,
                Port = Camera.RtspPort,
                Path = path,
            }.ToString();
        }
        public override string StreamLiveKurentoUri()
        {
            return StreamLiveUri();
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
            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = string.Format(@"/ISAPI/PTZCtrl/channels/{0}/presets", Camera.Channel.ToString())
            }.ToString();
        }

        public override string GuardTourUri()
        {
            throw new NotImplementedException();
        }

        public override string CallPresetUri(PresetDTO preset)
        {
            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = string.Format(@"/ISAPI/PTZCtrl/channels/{0}/presets/{1}/goto", Camera.Channel.ToString(), preset.Id)
            }.ToString();
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
            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = string.Format(@"/ISAPI/PTZCtrl/channels/{0}/presets/{1}", Camera.Channel.ToString(), preset.Id)
            }.ToString();
        }

        public override string DeletePresetUri(PresetDTO preset)
        {
            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = string.Format(@"/ISAPI/PTZCtrl/channels/{0}/presets/{1}", Camera.Channel.ToString(), preset.Id)
            }.ToString();
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
        public string SearchRecordings(CameraDTO item)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);


            return new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = string.Format(@"/ISAPI/ContentMgmt/search/"),
                Query = query.ToString()
            }.ToString();
        }

        public override IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false)
        {
            Guid newGuid = Guid.NewGuid();
            string xmlToString = String.Empty;

            retry:
            using (StringReader stringReader = new StringReader(HikvisionXML))
            {
                xmlToString = stringReader.ReadToEnd();
                if (startDate.ToUpper().Contains("Z"))
                    xmlToString = xmlToString.Replace("T00:00:00Z", "");

                if (endDate.ToUpper().Contains("Z"))
                    xmlToString = xmlToString.Replace("T23:59:00Z", "");

                switch (numberRetry)
                {
                    case 0:
                        xmlToString = xmlToString.Replace("RECORDERMODE", "");
                        break;
                    case 1:
                        xmlToString = xmlToString.Replace("RECORDERMODE", "");
                        xmlToString = xmlToString.Replace("&lt;", "<");
                        xmlToString = xmlToString.Replace("&gt;", ">");
                        break;
                    case 2:
                        xmlToString = xmlToString.Replace("&lt;", "<");
                        xmlToString = xmlToString.Replace("&gt;", ">");
                        xmlToString = xmlToString.Replace("RECORDERMODE", (Camera.RecordingMode == null || Camera.RecordingMode == "MainStream") ? "01" : "02");
                        break;
                }

                xmlToString = xmlToString.Replace("STARTDATE", startDate);
                xmlToString = xmlToString.Replace("ENDDATE", endDate);
                xmlToString = xmlToString.Replace("GUID", newGuid.ToString());
                xmlToString = xmlToString.Replace("CHANNEL", Camera.Channel.ToString() + ((Camera.RecordingMode == null || Camera.RecordingMode == "MainStream") ? "01" : "02"));
                xmlToString = xmlToString.Replace("TYPE", "");
                //xmlToString = xmlToString.Replace("TYPE", "timing");
            }

            try
            {
                var uri = SearchRecordings(Camera);
                var resultSearch = SendRequest(uri, HttpMethod.Post, xmlToString);

                IList<TimelineDTO> timeLineList = new List<TimelineDTO>();
                bool hasStart = false;
                bool hasEnd = false;
                TimelineDTO timeLine = new TimelineDTO();
                string line = "";

                using (StringReader resultReader = new StringReader(resultSearch))
                {
                    while ((line = resultReader.ReadLine()) != null)
                    {
                        if (line.Contains("<startTime>"))
                        {
                            timeLine.StartTime = Regex.Match(line, @"<startTime>(.*?)</startTime>").Groups[1].Value;
                            if (!string.IsNullOrEmpty(timeLine.StartTime))
                            {
                                //timeLine.StartTime = line.Substring(11, 20);
                                if (DateTime.Parse(timeLine.StartTime, CultureInfo.InvariantCulture).ToUniversalTime().Hour <
                                    DateTime.Parse(startDate, CultureInfo.InvariantCulture).ToUniversalTime().Hour)
                                {
                                    timeLine.StartTime = startDate;
                                }
                                hasStart = true;
                            }
                        }
                        if (line.Contains("<endTime>"))
                        {
                            if (timeLine.StartTime == null)
                            {
                                timeLine.StartTime = startDate;
                                hasStart = true;
                            }
                            timeLine.EndTime = Regex.Match(line, @"<endTime>(.*?)</endTime>").Groups[1].Value;
                            if (!string.IsNullOrEmpty(timeLine.EndTime))
                            {
                                //timeLine.EndTime = line.Substring(9, 20);
                                var currentEndTimeUtc = DateTime.Parse(timeLine.EndTime, CultureInfo.InvariantCulture).ToUniversalTime();
                                var targetEndDateUtc = DateTime.Parse(endDate, CultureInfo.InvariantCulture).ToUniversalTime();

                                bool isAfterDate = currentEndTimeUtc.Date > targetEndDateUtc.Date;
                                bool isSameDateButAfterHour = currentEndTimeUtc.Date == targetEndDateUtc.Date &&
                                    currentEndTimeUtc.Hour > targetEndDateUtc.Hour;
                                bool isSameHourButAfterMinute = currentEndTimeUtc.Date == targetEndDateUtc.Date &&
                                    currentEndTimeUtc.Hour == targetEndDateUtc.Hour &&
                                    currentEndTimeUtc.Minute >= targetEndDateUtc.Minute;

                                if (isAfterDate || isSameDateButAfterHour || isSameHourButAfterMinute)
                                {
                                    timeLine.EndTime = endDate;
                                }
                                hasEnd = true;
                            }
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
                numberRetry = 0;
                var timeList = timeSegmentation(timeLineList, startDate, endDate);
                return timeList;
            }

            catch (Exception ex)
            {
                if (numberRetry <= 2)
                {
                    numberRetry++;
                    goto retry;
                }
                else
                {
                    Logger.Log(string.Format("Conection to the camera {0} failed: {1}", Camera.Name, ex), LogPriority.Fatal);
                    Logger.Log(string.Format("GetTimeLine Hikvision {0} failed: startdate: {1}, enddate {2}", Camera.Name, startDate, endDate), LogPriority.Information);
                    numberRetry = 0;
                    throw ex;
                }
            }
        }

        public override string GetFirmwareVersionUri()
        {
            return string.Empty;
        }

        private IList<TimelineDTO> timeSegmentation(IList<TimelineDTO> timeSegmentationList, string startDate, string endDate)
        {
            List<TimelineDTO> timeSegmentation = new List<TimelineDTO>();

            // Internal cleaning function to normalize regional formats (a.m. / p.m.)
            string Clean(string date) => date.Replace("a. m.", "AM").Replace("p. m.", "PM")
                                             .Replace("a.m.", "AM").Replace("p.m.", "PM");

            try
            {
                var dtStarTime = Convert.ToDateTime(Clean(startDate), CultureInfo.InvariantCulture).ToUniversalTime();
                var dtEndDate = Convert.ToDateTime(Clean(endDate), CultureInfo.InvariantCulture).ToUniversalTime();

                foreach (var time in timeSegmentationList)
                {
                    try
                    {
                        // Process each item in the list individually
                        var star = Convert.ToDateTime(Clean(time.StartTime), CultureInfo.InvariantCulture).ToUniversalTime();
                        if (star < dtStarTime) star = dtStarTime;

                        var end = Convert.ToDateTime(Clean(time.EndTime), CultureInfo.InvariantCulture).ToUniversalTime();

                        while (star < end)
                        {
                            var item = new TimelineDTO();
                            var newEnd = (star.Minute > 0 || star.Second > 0)
                                         ? star.AddMinutes(-star.Minute).AddSeconds(-star.Second).AddHours(1)
                                         : star.AddHours(1);

                            item.StartTime = star.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
                            item.EndTime = (newEnd > end)
                                           ? end.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")
                                           : newEnd.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

                            timeSegmentation.Add(item);
                            star = newEnd;
                        }
                    }
                    catch (Exception exItem)
                    {
                        Logger.Log($"Error timeSegmentation Hikvision individual Start: {time.StartTime}, End: {time.EndTime}. Error: {exItem.Message}", LogPriority.Information);
                    }
                }
            }
            catch (Exception exGlobal)
            {
                Logger.Log($"Error timeSegmentation Hikvision. Global Start: {startDate}, Global End: {endDate}. Exception: {exGlobal}", LogPriority.Fatal);
            }

            return timeSegmentation;
        }
    }
}
