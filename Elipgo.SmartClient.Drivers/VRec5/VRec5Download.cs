using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Elipgo.SmartClient.Drivers.VRec5
{
    public class VRec5Download : IDriverDownload, IDisposable
    {
        public BookmarkGroupElementDTO BookmarkGroupElement { get; set; }
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private DateTime StartTime { get; set; }
        private DateTime EndTime { get; set; }
        private RecorderDTO Recorder { get; set; }
        private DownloadMeta DownloadMeta { get; set; }
        private WebClient ClientDownload { get; set; } = new WebClient();

        private readonly Configuration _config;

        private readonly int _tryLimit;
        private Int16 _retryCount = 1;

        public event OnDownloadCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadProgressEventHandler OnDownloadProgress;
        public event OnDownloadErrorEventHandler OnDownloadError;
        public event OnDriverDispose OnDispose;

        enum Step
        {
            Login,
            Process,
            Download
        }
        private Step SelectStep;

        public VRec5Download(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName)
        {
            _config = SmartClientEnvironmentUtils.GetConfiguration();

            _tryLimit = int.Parse(_config.AppSettings.Settings["tryLimit"].Value);
            BookmarkGroupElement = bookmarkGroupElement;
            Camera = camera;
            Recorder = Camera.Recorders.Find(x => x.Id == bookmarkGroupElement.NvrId);
            StartTime = DateTime.Parse(bookmarkGroupElement.TimeStart).AddMinutes(Camera.Gmt * -1);
            EndTime = DateTime.Parse(bookmarkGroupElement.TimeEnd).AddMinutes(Camera.Gmt * -1);
            FileName = fileName + ".mp4";
        }

        public void Dispose()
        {
            if (ClientDownload != null)
                ClientDownload.Dispose();
        }

        public void Start()
        {
            SelectStep = Step.Login;
            CreateClientDownload();
        }

        public void Stop()
        {
            ClientDownload.CancelAsync();
        }

        private void CreateClientDownload()
        {
            var uri = "";
            string password = string.Empty;
            try
            {
                password = Security.AESDecrypt(Recorder.Password);
            }
            catch (Exception)
            {
                password = Recorder.Password;
            }

            switch (SelectStep)
            {
                case Step.Login:
                    var ClientLogin = new WebClient();
                    ClientLogin.Credentials = new NetworkCredential(Recorder.Username, Recorder.Password);
                    ClientLogin.Headers.Add($"Authorization", $"Basic {Security.Base64Encode($"{Recorder.Username}:{password}")}");
                    uri = string.Format("{0}://{1}:{2}/player/download?from={3}&to={4}&cameraId={5}", Recorder.HttpProtocol, Recorder.Host, Recorder.HttpPort, StartTime.ToString("yyyyMMddHHmmss"), EndTime.ToString("yyyyMMddHHmmss"), Camera.Id.ToString().PadLeft(8, '0'));
                    ClientLogin.DownloadDataCompleted += ClientDownload_DownloadDataCompleted;
                    ClientLogin.DownloadDataAsync(new Uri(uri));
                    break;
                case Step.Process:
                    break;
                case Step.Download:
                    ClientDownload.Credentials = new NetworkCredential(Recorder.Username, Recorder.Password);
                    ClientDownload.Headers.Add($"Authorization", $"Basic {Security.Base64Encode($"{Recorder.Username}:{password}")}");
                    uri = string.Format("{0}://{1}:{2}/{3}", Recorder.HttpProtocol, Recorder.Host, Recorder.HttpPort, DownloadMeta.path);
                    ClientDownload.DownloadProgressChanged += ClientDownload_DownloadProgressChanged;
                    ClientDownload.DownloadFileCompleted += ClientDownload_DownloadFileCompleted;
                    Thread.Sleep(6000);
                    ClientDownload.DownloadFileAsync(new Uri(uri), FileName);
                    break;
            }
        }

        private void ClientDownload_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                if (Step.Login == SelectStep)
                {
                    DownloadMeta = JsonConvert.DeserializeObject<DownloadMeta>(Encoding.UTF8.GetString(e.Result));
                    SelectStep = Step.Download;
                    CreateClientDownload();
                }
            }
            else
            {
                if (e.Error is WebException)
                {
                    WebException we = (WebException)e.Error;
                    if (we.Response is HttpWebResponse)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                        }
                    }
                }

                OnDownloadError?.Invoke(this, e.Error.Message);
            }
        }

        private void ClientDownload_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var client = (WebClient)sender;
            client.DownloadProgressChanged -= ClientDownload_DownloadProgressChanged;
            client.DownloadFileCompleted -= ClientDownload_DownloadFileCompleted;

            if (e.Cancelled)
            {
                try
                {
                    if (File.Exists(FileName))
                        File.Delete(FileName);
                }
                catch { }
                return;
            }

            if (e.Error != null)
            {
                if (e.Error is WebException)
                {
                    WebException we = (WebException)e.Error;
                    if (we.Response is HttpWebResponse)
                    {
                        HttpWebResponse response = (HttpWebResponse)we.Response;
                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            using (Stream stream = response.GetResponseStream())
                            {
                                if (stream.CanRead)
                                {
                                    using (var reader = new StreamReader(stream))
                                    {
                                        var responseText = reader.ReadToEnd();
                                        if (responseText.Contains("File is in process"))
                                        {
                                            if (_retryCount <= _tryLimit)
                                            {
                                                Thread.Sleep(2000 * _retryCount);
                                                CreateClientDownload();
                                                _retryCount++;
                                            }
                                            return;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                try
                {
                    if (File.Exists(FileName))
                        File.Delete(FileName);
                }
                catch { }
                OnDownloadError?.Invoke(this, e.Error.Message);
                return;
            }

            BookmarkGroupElement.Progress = 100;
            BookmarkGroupElement.Success = true;
            OnDownloadProgress?.Invoke(this, 100);
            OnDownloadCompleted?.Invoke(this, FileName);
        }

        private void ClientDownload_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            BookmarkGroupElement.Progress = e.ProgressPercentage;
            OnDownloadProgress?.Invoke(this, e.ProgressPercentage);
        }
    }
}
