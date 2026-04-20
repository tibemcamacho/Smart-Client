using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Reflections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace Elipgo.SmartClient.Drivers.Axis
{
    public class AxisDownloadVisualSearch : IDriverDownloadVisualSearch, IDisposable
    {
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }
        private WebClient ClientDownload { get; set; } = new WebClient();
        private List<Recording> RecordingList { get; set; }

        public event OnDownloadVisualSearchCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadVisualSearchProgressEventHandler OnDownloadProgress;
        public event OnDownloadVisualSearchErrorEventHandler OnDownloadError;
        public event OnDriverDispose OnDispose;

        private readonly int tryLimit;

        public AxisDownloadVisualSearch(CameraDTO camera)
        {
            Camera = camera;

            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);

        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Start(string fileName, DateTime startTime, DateTime endTime)
        {
            Logger.Log(String.Format("Axis start entered camera {0} Channel{1} start: {2} end:{3} filaname:{4} ", Camera.Name, Camera.Channel - 1, startTime, endTime, fileName), LogPriority.Information);
            Int16 _try = 0;
            StartTime = startTime;
            EndTime = endTime;
            FileName = fileName + ".mkv";
            while (_try <= (tryLimit + 1))
            {
                try
                {
                    DownloadRecordingList();
                    if (RecordingList != null && RecordingList.Count > 0)
                    {
                        Recording recording = new Recording();
                        foreach (Recording r in RecordingList)
                        {
                            DateTime starttime = DateTime.ParseExact(r.StartTime, "yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'", System.Globalization.CultureInfo.InvariantCulture);
                            if (r.RecordingStatus == "completed")
                            {
                                DateTime stoptime = DateTime.ParseExact(r.StopTime, "yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'", System.Globalization.CultureInfo.InvariantCulture);

                                if (StartTime > starttime && StartTime < stoptime)
                                    recording = r;
                            }
                            else if (r.RecordingStatus == "recording")
                            {
                                DateTime stoptime = DateTime.UtcNow;
                                if (starttime < StartTime)
                                    recording = r;

                            }
                        }

                        if (recording.RecordingId != null)
                        {
                            IManufactureUri manufactureUri = ManufactureUriFactory.Instance(this.Camera, Common.Enum.Profile.None);
                            Uri uri = new Uri(manufactureUri.ExportRecordingUri(recording.RecordingId, StartTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"), EndTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")));

                            ClientDownload.Credentials = new NetworkCredential(Camera.User, Camera.Password);
                            ClientDownload.DownloadProgressChanged += Client_DownloadProgressChanged;
                            ClientDownload.DownloadFileCompleted += Client_DownloadFileCompleted;
                            Logger.Log(string.Format("Device {0} has started download", Camera.Name), LogPriority.Information);
                            ClientDownload.DownloadFileAsync(uri, FileName);
                            break;
                        }
                        else
                        {
                            OnDownloadError?.Invoke(this, string.Format("No recordings are available {0}", Camera.Name));
                            break;

                        }
                    }
                    else
                    {
                        OnDownloadError?.Invoke(this, string.Format("No recordings are available {0}", Camera.Name));
                        break;
                    }
                }

                catch (Exception ex)
                {
                    if (TryLimit(ref _try, ex.Message))
                    {
                        OnDownloadError?.Invoke(this, "Failed to download recording list " + ex.Message);
                        break;
                    }
                }
            }
        }

        private bool TryLimit(ref Int16 _try, string errMsg)
        {
            bool blimit = false;
            _try++;
            System.Threading.Thread.Sleep(3000);
            if (_try >= tryLimit)
            {
                OnDownloadError?.Invoke(this, errMsg);
                blimit = true;
            }
            return blimit;
        }

        public void Stop()
        {
            ClientDownload.CancelAsync();
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var client = (WebClient)sender;
            client.DownloadProgressChanged -= Client_DownloadProgressChanged;
            client.DownloadFileCompleted -= Client_DownloadFileCompleted;

            if (e.Error != null)
            {
                OnDownloadError?.Invoke(this, e.Error.Message);
                return;
            }

            if (e.Cancelled)
            {
                try
                {
                    if (System.IO.File.Exists(FileName))
                        System.IO.File.Delete(FileName);
                }
                catch (Exception) { }

                return;
            }

            OnDownloadProgress?.Invoke(this, 100);
            OnDownloadCompleted?.Invoke(this, FileName);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Logger.Log(string.Format("Device {0} is downloading a file, progress {1}", Camera.Name, e.ProgressPercentage), LogPriority.Information);
            OnDownloadProgress?.Invoke(this, e.ProgressPercentage);
        }

        private void DownloadRecordingList()
        {
            Uri uri = null;
            try
            {
                IManufactureUri manufactureUri = ManufactureUriFactory.Instance(this.Camera, Common.Enum.Profile.None);
                uri = new Uri(manufactureUri.RecordingPlaybackUri());

                using (WebClient webClient = new WebClient())
                {
                    webClient.Credentials = new NetworkCredential(Camera.User, Camera.Password);
                    string result = webClient.DownloadString(uri);

                    using (StringReader stringReader = new StringReader(result))
                    {
                        XDocument xmlDoc = XDocument.Load(stringReader);
                        var query = from r in xmlDoc.Descendants("recording")
                                    select new Recording
                                    {
                                        RecordingId = (string)r.Attribute("recordingid").Value,
                                        StartTime = (string)r.Attribute("starttime").Value,
                                        StopTime = (string)r.Attribute("stoptime").Value,
                                        RecordingStatus = (string)r.Attribute("recordingstatus").Value,
                                        MimeType = r.Descendants("video").Count() > 0 ?
                                            (string)r.Descendants("video").FirstOrDefault().Attribute("mimetype").Value : "",
                                        FrameRate = r.Descendants("video").Count() > 0 ?
                                            (string)r.Descendants("video").FirstOrDefault().Attribute("framerate").Value : "",
                                        Audio = (string)((r.Descendants("audio").Count() > 0) ? "yes" : "no")
                                    };

                        RecordingList = new List<Recording>();
                        if (query.Count() > 0)
                        {
                            foreach (Recording r in query.ToList<Recording>())
                            {
                                RecordingList.Add(r);
                            }
                        }
                        else
                        {
                            Logger.Log(String.Format("No recordings are available  {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, uri), LogPriority.Information);
                            OnDownloadError?.Invoke(this, "No recordings are available");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format(" DownloadRecordingList Exception:{0} Camera:{1} Host:{2} VideoPort:{3} User:{4} Uri:{5}", ex.Message, Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, uri), LogPriority.Information);
                throw ex;
            }
        }
    }
}
