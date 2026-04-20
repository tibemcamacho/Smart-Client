using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Drivers.Vrec.Response;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace Elipgo.SmartClient.Drivers.Vrec
{
    public class VrecDownload : IDriverDownload, IDisposable
    {
        private readonly int tryLimit;
        private static readonly object _lock = new object();
        private readonly string _vrecLogin;
        private readonly string _vrecUser;
        private readonly string _vrecPassword;
        private readonly string _vrecStartProcess;
        private readonly string _vrecStatusProcess;
        private readonly string _vrecDownload;
        public event OnDriverDispose OnDispose;

        enum Step
        {
            Login,
            StartProcess,
            Process,
            Download
        }

        private Step SelectStep;
        private Uri uri;
        public BookmarkGroupElementDTO BookmarkGroupElement { get; set; }
        public CameraDTO Camera { get; set; }
        public string FileName { get; set; }
        private string StartTime { get; set; }
        private string EndTime { get; set; }
        private string ProcessID = string.Empty;
        private RecorderDTO Recorder { get; set; }

        public event OnDownloadCompletedEventHandler OnDownloadCompleted;
        public event OnDownloadProgressEventHandler OnDownloadProgress;
        public event OnDownloadErrorEventHandler OnDownloadError;

        public VrecDownload(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName)
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

            _vrecLogin = config.AppSettings.Settings["VRECLogin"].Value;
            _vrecUser = Common.Properties.Settings.Default["VRECUser"].ToString();
            _vrecPassword = Common.Properties.Settings.Default["VRECPassword"].ToString();
            _vrecStartProcess = config.AppSettings.Settings["VRECStartProcess"].Value;
            _vrecStatusProcess = config.AppSettings.Settings["VRECStatusProcess"].Value;
            _vrecDownload = config.AppSettings.Settings["VRECDownload"].Value;

            BookmarkGroupElement = bookmarkGroupElement;
            Camera = camera;
            Recorder = Camera.Recorders.Find(x => x.Id == bookmarkGroupElement.NvrId);
            StartTime = Convert.ToDateTime(bookmarkGroupElement.TimeStart).ToString("yyyyMMddHHmm");
            EndTime = Convert.ToDateTime(bookmarkGroupElement.TimeEnd).ToString("yyyyMMddHHmm");
            FileName = fileName + ".tmp";
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
        }

        private WebClient ClientDownload { get; set; } = new WebClient();

        public void Stop()
        {
        }

        public void Start()
        {
            // 18-Agosto-2021 * vmon-4389 * ddvl * Se espera que intente 3 veces antes de informar el error.
            Int16 _try = 0;
            while (_try <= (tryLimit + 1)) // prevent indefinite cycle
            {
                try
                {
                    LoginVrec();
                    break;
                }
                catch (Exception ex)
                {
                    if (TryLimit(ref _try, ex.Message)) break;
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

        public void Dispose()
        {
        }

        private void LoginVrec()
        {
            if (Recorder == null)
            {
                Logger.Log(String.Format(" VrecDownload start faild camera:{0} star:{1} end:{2} filename:{3}", Camera.Id, StartTime, EndTime, FileName), LogPriority.Information);
                OnDownloadError?.Invoke(this, "No exists recorder");
                return;
            }
            uri = new Uri(string.Format(_vrecLogin, Recorder.Host, Recorder.HttpPort, _vrecUser, _vrecPassword, Recorder.HttpProtocol));
            SelectStep = Step.Login;
            CreateClientDownload();
        }

        private void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            if (!CompletedResponse(sender, e))
            {
                switch (SelectStep)
                {
                    case Step.Login:
                        SelectStep = Step.StartProcess;
                        uri = new Uri(string.Format(_vrecStartProcess, Recorder.Host, Recorder.HttpPort, Camera.Id, StartTime, EndTime, Recorder.HttpProtocol));
                        CreateClientDownload();
                        break;
                    case Step.StartProcess:
                        SelectStep = Step.Process;
                        using (TextReader reader = new StringReader(Encoding.UTF8.GetString(e.Result)))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(VrecJobResponse));
                            var result = (VrecJobResponse)serializer.Deserialize(reader);
                            if (result.Status == "OK")
                            {
                                uri = new Uri(string.Format(_vrecStatusProcess, Recorder.Host, Recorder.HttpPort, Camera.Id, Recorder.HttpProtocol));
                                ProcessID = result.Job.ProcessID;
                                CreateClientDownload();
                            }
                            else
                                return;
                        }
                        break;
                    case Step.Process:
                        using (TextReader reader = new StringReader(Encoding.UTF8.GetString(e.Result)))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(VrecProgressResponse));
                            var result = (VrecProgressResponse)serializer.Deserialize(reader);
                            if (result.Status == "OK")
                            {
                                if (result.Job.Exists(x => x.ProcessID == ProcessID && x.Status == "Finished"))
                                {
                                    SelectStep = Step.Download;
                                    uri = new Uri(string.Format(_vrecDownload, Recorder.Host, Recorder.HttpPort, ProcessID, FileName, Recorder.HttpProtocol));
                                }
                                else
                                    Thread.Sleep(2000);

                                CreateClientDownload();
                            }
                        }
                        break;
                }
            }
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            lock (_lock)
            {
                BookmarkGroupElement.Progress = e.ProgressPercentage;
                OnDownloadProgress?.Invoke(this, e.ProgressPercentage);
            }
        }

        private void CreateClientDownload()
        {
            ClientDownload.Credentials = new NetworkCredential();
            ClientDownload.DownloadProgressChanged += Client_DownloadProgressChanged;
            if (Step.Download == SelectStep)
            {
                ClientDownload.DownloadFileCompleted += Client_DownloadFileCompleted;
                Thread.Sleep(6000);
                ClientDownload.DownloadFileAsync(uri, FileName);
            }
            else
            {
                ClientDownload.DownloadDataCompleted += Client_DownloadDataCompleted;
                ClientDownload.DownloadDataAsync(uri);
            }
        }

        private bool CompletedResponse(object sender, dynamic ev)
        {
            lock (_lock)
            {
                var client = (WebClient)sender;
                client.DownloadProgressChanged -= Client_DownloadProgressChanged;

                if (SelectStep == Step.Download)
                    client.DownloadFileCompleted -= Client_DownloadFileCompleted;
                else
                    client.DownloadDataCompleted -= Client_DownloadDataCompleted;


                var e = SelectStep == Step.Download ? (ev as System.ComponentModel.AsyncCompletedEventArgs) : (ev as DownloadDataCompletedEventArgs);
                if (e.Error != null)
                {
                    OnDownloadError?.Invoke(this, e.Error.Message);
                    return true;
                }

                if (e.Cancelled)
                {
                    if (SelectStep == Step.Download)
                    {
                        try
                        {
                            if (File.Exists(FileName))
                                File.Delete(FileName);
                        }
                        catch { }
                    }
                    return true;
                }

                Thread.Sleep(1000);
                BookmarkGroupElement.Success = true;
                BookmarkGroupElement.Progress = 100;
                OnDownloadProgress?.Invoke(this, 100);
                if (SelectStep == Step.Download)
                    OnDownloadCompleted?.Invoke(this, FileName);
                return false;
            }
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            CompletedResponse(sender, e);
        }
    }
}
