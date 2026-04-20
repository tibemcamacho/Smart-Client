using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{
    public class Vrec5Uri : ManufactureUriAbstract
    {
        public Vrec5Uri(CameraDTO camera) : base(camera)
        {
        }
        public Vrec5Uri(CameraDTO camera, Profile profile) : base(camera, profile)
        {
        }

        public override string AudioConfigUri()
        {
            return String.Empty;
            //throw new NotImplementedException();
        }

        public override string AudioTrasmitUri()
        {
            return String.Empty;
            //throw new NotImplementedException();
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
            return new UriBuilder()
            {
                Scheme = "rtsp",
                Host = Camera.Recorders[0].Host,
                Port = Camera.Recorders[0].RtspPort,
                Path = "/" + Camera.Id.ToString().PadLeft(8, '0'),
            }.ToString();
        }

        public override string StreamLiveUri()
        {
            return new UriBuilder()
            {
                Scheme = "rtsp",
                Host = Camera.Recorders[0].Host,
                Port = Camera.Recorders[0].RtspPort,
                Path = "/" + Camera.Id.ToString().PadLeft(8, '0'),
            }.ToString();
        }

        public override string StreamPlaybackKurentoUri(DateTime starttime, DateTime stoptime)
        {
            throw new NotImplementedException();
        }

        public override string StreamPlaybackUri()
        {
            return new UriBuilder()
            {
                Scheme = "rtsp",
                Host = Camera.Recorders[0].Host,
                Port = Camera.Recorders[0].RtspPort,
                Path = "/" + Camera.Id.ToString().PadLeft(8, '0'),
            }.ToString();
        }

        public override IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false)
        {
            IList<TimelineDTO> timeLineList = new List<TimelineDTO>();

            try
            {
                var timeZoneInMinutes = Camera.Gmt;
                var recorder = new RecorderDTO();
                if (this.Camera.RecorderId != 0)
                    recorder = this.Camera.Recorders.Where(r => r.Id == this.Camera.RecorderId).First();
                else
                    recorder = this.Camera.Recorders[0];

                var cameraId = this.Camera.Id.ToString().PadLeft(8, '0');
                startDate = DateTime.ParseExact(RemoveDateTimeFormat(startDate), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).AddMinutes(timeZoneInMinutes * -1).ToString("yyyyMMddHHmmss");
                endDate = DateTime.ParseExact(RemoveDateTimeFormat(endDate), "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).AddMinutes(timeZoneInMinutes * -1).ToString("yyyyMMddHHmmss");
                var url = $"{recorder.HttpProtocol}://{recorder.Host}:{recorder.HttpPort}/player/chunklist?from={startDate}&to={endDate}&cameraId={cameraId}";

                try
                {
                    var data = SendRequest(url, HttpMethod.Get, string.Empty, true);
                    var chunkList = JsonConvert.DeserializeObject<List<Vrec5ChunkList>>(data);

                    if (chunkList.Count > 0)
                    {
                        DateTime rangeStart = DateTime.MinValue;
                        DateTime rangeEnd = DateTime.MinValue;
                        DateTime maxValue = DateTime.ParseExact(endDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture).AddMinutes(timeZoneInMinutes);
                        List<RangeDateTime> rangeList = new List<RangeDateTime>();
                        bool first = true;
                        foreach (var chunk in chunkList)
                        {
                            DateTime chunkFrom = DateTime.ParseExact(chunk.From, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            chunkFrom = chunkFrom.AddMinutes(timeZoneInMinutes);
                            DateTime chunkTo = DateTime.ParseExact(chunk.To, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                            chunkTo = chunkTo.AddMinutes(timeZoneInMinutes);

                            if (chunkTo > maxValue)
                            {
                                var diff = maxValue.Subtract(chunkTo).TotalSeconds;
                                chunkTo = chunkTo.AddSeconds(diff);
                            }

                            if (first)
                            {
                                rangeStart = chunkFrom;
                                rangeEnd = chunkTo;
                                first = false;
                            }
                            else
                            {
                                if (chunkFrom >= rangeEnd.AddSeconds(-5) && chunkFrom < rangeEnd.AddSeconds(5))
                                {
                                    rangeEnd = chunkTo;
                                }
                                else
                                {
                                    rangeList.Add(new RangeDateTime { Start = rangeStart, End = rangeEnd });
                                    rangeStart = chunkFrom;
                                    rangeEnd = chunkTo;
                                }
                            }
                        }
                        rangeList.Add(new RangeDateTime { Start = rangeStart, End = rangeEnd });

                        foreach (RangeDateTime item in rangeList)
                        {
                            var start = item.Start;
                            var end = item.End;
                            while (start < end)
                            {
                                DateTime current = start;
                                current = current.AddMinutes(-current.Minute);
                                current = current.AddSeconds(-current.Second);
                                current = current.AddHours(1);
                                if (current > end)
                                    current = end;

                                timeLineList.Add(new TimelineDTO { StartTime = start.ToString("yyyy-MM-ddTHH:mm:ss.fffffZ"), EndTime = current.ToString("yyyy-MM-ddTHH:mm:ss.fffffZ") });
                                start = current.AddSeconds(1);
                            }
                        }
                    }
                }
                catch (Exception) { }
            }
            catch (Exception) { }
            return timeLineList;
        }

        private string RemoveDateTimeFormat(string dateTimeString)
        {
            return dateTimeString.Replace("-", "").Replace("T", "").Replace(":", "").Replace("Z", "");
        }
        public override string GetFirmwareVersionUri()
        {
            return string.Empty;
        }
    }

    internal class RangeDateTime
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

}
