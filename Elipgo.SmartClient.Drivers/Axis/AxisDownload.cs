using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Reflections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;

namespace Elipgo.SmartClient.Drivers.Axis
{
    public class AxisDownload : IDriverDownload, IDisposable
    {
        public BookmarkGroupElementDTO BookmarkGroupElement { get; set; }
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }
        private List<Recording> RecordingList { get; set; }
        private WebClient ClientDownload { get; set; } = new WebClient();
        private int retryCount = 1;
        private readonly int tryLimit;
        private string DownloadEncodeAxisUrl = string.Empty;
        private Recording recordingEncode = new Recording();

        public event OnDownloadCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadProgressEventHandler OnDownloadProgress;
        public event OnDownloadErrorEventHandler OnDownloadError;
        public event OnDriverDispose OnDispose;

        public AxisDownload(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName)
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
            DownloadEncodeAxisUrl = config.AppSettings.Settings["DownloadEncodeAxisUrl"].Value;

            BookmarkGroupElement = bookmarkGroupElement;
            Camera = camera;
            StartTime = DateTime.Parse(bookmarkGroupElement.TimeStart).AddMinutes(Camera.Gmt * -1);
            EndTime = DateTime.Parse(bookmarkGroupElement.TimeEnd).AddMinutes(Camera.Gmt * -1);
            FileName = fileName + ".mkv";

            DownloadRecordingList();
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Start()
        {
            try
            {

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
                        var sversion = GetFirmwareVersion();

                        if (sversion > 0)
                        {
                            //recordingEncode = recording;
                            IManufactureUri manufactureUri = ManufactureUriFactory.Instance(this.Camera, Common.Enum.Profile.None);
                            Uri uri = new Uri(manufactureUri.ExportRecordingUri(recording.RecordingId, StartTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'"), EndTime.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")));
                            ClientDownload.Credentials = new NetworkCredential(Camera.User, Camera.Password);
                            ClientDownload.DownloadProgressChanged += Client_DownloadProgressChanged;
                            ClientDownload.DownloadFileCompleted += Client_DownloadFileCompleted;
                            ClientDownload.DownloadFileAsync(uri, FileName);
                        }
                        else
                        {

                            recordingEncode = recording;
                            try
                            {
                                if (File.Exists(FileName))
                                    File.Delete(FileName);

                                EncodeAxisDTO encodeAxisDTO = new EncodeAxisDTO();
                                encodeAxisDTO.Host = Camera.Host;
                                encodeAxisDTO.RtspPort = Camera.RtspPort.ToString();
                                encodeAxisDTO.RecordingId = recordingEncode.RecordingId;// RecordingId;
                                encodeAxisDTO.User = Camera.User;
                                encodeAxisDTO.Password = Camera.Password;
                                encodeAxisDTO.Filename = System.IO.Path.GetFileNameWithoutExtension(FileName);
                                encodeAxisDTO.Width = recordingEncode.Width;
                                encodeAxisDTO.Height = recordingEncode.Height;
                                encodeAxisDTO.Fps = recordingEncode.FrameRate.Split(':')[0];
                                encodeAxisDTO.Duration = (Convert.ToDateTime(BookmarkGroupElement.TimeEnd) - Convert.ToDateTime(BookmarkGroupElement.TimeStart)).TotalSeconds.ToString();
                                DateTime Starttime = DateTime.ParseExact(recordingEncode.StartTime, "yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'", System.Globalization.CultureInfo.InvariantCulture);
                                DateTime TimeStart = Convert.ToDateTime(BookmarkGroupElement.TimeStart).AddMinutes(Camera.Gmt * -1);
                                double outset = (TimeStart - Starttime).TotalSeconds;
                                encodeAxisDTO.OffSet = Math.Round(outset).ToString();
                                var httpClient = new HttpClient() { BaseAddress = new Uri(DownloadEncodeAxisUrl) };


                                string json = JsonConvert.SerializeObject(encodeAxisDTO);
                                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                                HttpResponseMessage response = httpClient.PostAsync(DownloadEncodeAxisUrl, content).Result;
                                if (response.IsSuccessStatusCode)
                                {
                                    var readAsStream = response.Content.ReadAsStreamAsync().Result;

                                    FileName = FileName.Replace("mkv", "mp4");
                                    using (var fs = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                                    {
                                        readAsStream.CopyToAsync(fs).Wait();
                                        BookmarkGroupElement.Success = true;
                                        BookmarkGroupElement.Progress = 100;
                                        OnDownloadProgress?.Invoke(this, 100);
                                        OnDownloadCompleted?.Invoke(this, FileName);
                                    }
                                }
                                response.Content = null;

                            }
                            catch (Exception ex)
                            {
                                Logger.Log(string.Format("Error to Download encode Axis {0}", ex.Message), LogPriority.Fatal);
                                OnDownloadError?.Invoke(this, ex.Message);
                            }
                        }
                    }
                    else
                    {
                        if (retryCount <= tryLimit)
                        {
                            Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * retryCount) }, () => Start());
                            Logger.Log(String.Format("No recordings are available {4} of {5}:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, retryCount, tryLimit), LogPriority.Information);
                            retryCount++;
                        }
                        else
                            OnDownloadError?.Invoke(this, "No recordings are available");
                    }
                }
                else
                {
                    if (retryCount <= tryLimit)
                    {
                        Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * retryCount) }, () => Start());
                        Logger.Log(String.Format("No recordings are available {4} of {5}:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, retryCount, tryLimit), LogPriority.Information);
                        retryCount++;
                    }
                    else
                        OnDownloadError?.Invoke(this, "No recordings are available");
                }
            }
            catch (Exception e)
            {
                if (retryCount <= tryLimit)
                {
                    Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * retryCount) }, () => Start());
                    Logger.Log(String.Format("No recordings are available {4} of {5}:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, retryCount, tryLimit), LogPriority.Information);
                    retryCount++;
                }
                else
                    OnDownloadError?.Invoke(this, e.Message);
            }
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
                if (retryCount <= tryLimit)
                {
                    Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * retryCount) }, () => Start());
                    retryCount++;
                }
                else
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

            BookmarkGroupElement.Success = true;
            BookmarkGroupElement.Progress = 100;
            OnDownloadProgress?.Invoke(this, 100);
            OnDownloadCompleted?.Invoke(this, FileName);
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            BookmarkGroupElement.Progress = e.ProgressPercentage;
            OnDownloadProgress?.Invoke(this, e.ProgressPercentage);
        }

        private void DownloadRecordingList()
        {
            try
            {
                IManufactureUri manufactureUri = ManufactureUriFactory.Instance(this.Camera, Common.Enum.Profile.None);
                Uri uri = new Uri(manufactureUri.RecordingPlaybackUri());

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
                                        Audio = (string)((r.Descendants("audio").Count() > 0) ? "yes" : "no"),
                                        Width = r.Descendants("video").Count() > 0 ?
                                            (string)r.Descendants("video").FirstOrDefault().Attribute("width").Value : "",
                                        Height = r.Descendants("video").Count() > 0 ?
                                            (string)r.Descendants("video").FirstOrDefault().Attribute("height").Value : ""
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
                            OnDownloadError?.Invoke(this, "No recordings are available");
                        }
                    }

                }
                //WebClient webClient = new WebClient
                //{
                //    Credentials = new NetworkCredential(Camera.User, Camera.Password)
                //};
                //string result = webClient.DownloadString(uri);
                //webClient.Dispose();

                //XDocument xmlDoc = XDocument.Load(new System.IO.StringReader(result));
                //var query = from r in xmlDoc.Descendants("recording")
                //            select new Recording
                //            {
                //                RecordingId = (string)r.Attribute("recordingid").Value,
                //                StartTime = (string)r.Attribute("starttime").Value,
                //                StopTime = (string)r.Attribute("stoptime").Value,
                //                RecordingStatus = (string)r.Attribute("recordingstatus").Value,
                //                MimeType = r.Descendants("video").Count() > 0 ?
                //                    (string)r.Descendants("video").FirstOrDefault().Attribute("mimetype").Value : "",
                //                FrameRate = r.Descendants("video").Count() > 0 ?
                //                    (string)r.Descendants("video").FirstOrDefault().Attribute("framerate").Value : "",
                //                Audio = (string)((r.Descendants("audio").Count() > 0) ? "yes" : "no")
                //            };

                //RecordingList = new List<Recording>();
                //if (query.Count() > 0)
                //{
                //    foreach (Recording r in query.ToList<Recording>())
                //    {
                //        RecordingList.Add(r);
                //    }
                //}
                //else
                //{
                //    OnDownloadError?.Invoke(this, "No recordings are available");
                //}
            }
            catch (Exception)
            {
                OnDownloadError?.Invoke(this, "Failed to download recording list");
            }
        }

        private int GetFirmwareVersion()
        {
            try
            {
                IManufactureUri manufactureUri = ManufactureUriFactory.Instance(this.Camera, Common.Enum.Profile.None);
                Uri uri = new Uri(manufactureUri.GetFirmwareVersionUri());

                // <httpWebRequest useUnsafeHeaderParsing="true" /> in app.config
                WebClient webClient = new WebClient();
                webClient.Credentials = new NetworkCredential(Camera.User, Camera.Password);
                string result = webClient.DownloadString(uri);
                Match match = Regex.Match(result, @"[^=]+=(.+)");
                try
                {
                    var firmwareVersion = new Version(match.Groups[1].Value);
                    int iCompareVersion = firmwareVersion.CompareTo(new Version("5.51.7.6"));
                    return iCompareVersion;
                }
                catch
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

    }
}
