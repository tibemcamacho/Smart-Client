using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Reflections;
using Microsoft.Web.WebView2.Core;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;
using Recording = Elipgo.SmartClient.Common.DTOs.Recording;

namespace Elipgo.SmartClient.Drivers.Kurento
{
    public partial class KurentoInstantPlaybackUserControl : UserControl, IDriverInstantPlayback, IDisposable
    {
        public struct JsonObject
        {
            public string Key;
            public string Value;
        }
        public event OnDriverDispose OnDispose;
        public CameraDTO Camera { get; set; }
        public List<ButtonsContextBar> Commands => GetButtons();
        public Profile Profile { get; set; }
        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();
        public event OnVideoEventHandler OnVideo;
        public event EventHandler<bool> OpenBookmark;
        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnTimeChangedEventHandler OnTimeChanged;
        public event OnStateChangedEventHandler OnStateChanged;
        private const int MAX_MINUTES = 360;
        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ActualTime { get; set; }
        private DateTime InitDateTime { get; set; }
        private DateTime EndDatetime { get; set; }
        public bool ClipStatus { get; set; } = false;
        public bool ZoomStatus { get; set; }
        public bool BookmarkState { get; set; }
        private bool _painted = false;
        private IManufactureUri manufactureUri;
        private List<Recording> listViewRecording;
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private RecorderDTOSmall Recorder;
        private bool offLine = false;
        private readonly int _tryLimit;
        private int intentNumber = 0;

        public KurentoInstantPlaybackUserControl(CameraDTO camera, Profile profile, bool hideControls, DateTime selectedDateTime, RecorderDTOSmall recorder)
        {
            Camera = camera;
            Profile = profile;
            Recorder = recorder;
            StartTime = DateTime.Now.AddDays(-1).AddMinutes(-1);
            InitDateTime = DateTime.Now.AddDays(-1).AddMinutes(-1 * MAX_MINUTES);
            EndTime = DateTime.Now.AddDays(-1).AddMinutes(MAX_MINUTES);
            this.listViewRecording = new List<Recording>();
            manufactureUri = ManufactureUriFactory.Instance(this.Camera, this.Profile);

            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            _tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);

            LoadDevice();
            InitializeComponent();

            panelNoConnection.Visible = this.offLine;
            this.Paint += KurentoInstantPlackbackserControl_Paint;
        }

        private void LoadDevice()
        {
            if (Camera.ManufactureCode == Manufacturer.Axis)
            {
                try
                {
                    listViewRecording.Clear();

                    using (WebClient webClient = new WebClient())
                    {
                        if (!string.IsNullOrEmpty(Camera.User))
                        {
                            webClient.Credentials = new NetworkCredential(Camera.User, Camera.Password);
                        }
                        string url = manufactureUri.RecordingPlaybackUri();
                        string result = webClient.DownloadString(url);
                        webClient.Dispose();
                        XDocument xmlDoc = XDocument.Load(new System.IO.StringReader(result));
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

                        if (query.Count() > 0)
                        {
                            foreach (Recording r in query.ToList<Recording>())
                            {
                                listViewRecording.Add(r);
                            }
                        }
                        else
                        {
                            var message = string.Format("{0} - {1}", Camera.Name, StartTime.ToString("yyyy/MM/dd HH:mm:ss"));
                            notification.Show(string.Format(Elipgo.SmartClient.Common.Properties.Resources.NoRecordingAvailable, message), null);

                            Logger.Log(String.Format("Error camara no tiene recording disponible:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                    Logger.Log(String.Format("Error al realizar el Login Camera:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Fatal);
                }
            }
        }

        private void KurentoInstantPlackbackserControl_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (this._painted)
                {
                    return;
                }

                this._painted = true;
                PaintBrowser();
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error: {0} ", ex.Message), LogPriority.Fatal);
            }
        }

        private string GetAxisConnectionString()
        {
            DateTime dt = this.StartTime;
            Recording selected = new Recording();
            foreach (Recording element in listViewRecording)
            {
                //listViewRecording.Items.Add(new ListViewItem(new string[] { r.RecordingId, r.StartTime, r.StopTime, r.RecordingStatus, r.MimeType, r.FrameRate, r.Audio }));
                string DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'";
                string xt = element.StartTime;

                DateTime d = DateTime.ParseExact(xt, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                if (element.RecordingStatus != "completed")
                {
                    if (d < dt)
                    {
                        selected = element;
                    }
                }
                else
                {
                    string d2 = element.StopTime;
                    DateTime dt2 = DateTime.ParseExact(d2, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);

                    if (dt > d && dt < dt2)
                    {
                        selected = element;
                    }
                }
            }
            return string.Format(@"file:///{0}/Resources/Html/kurento/index.html?destination={1}{2}&ws_uri={3}", Application.StartupPath.Replace("\\", "/"), manufactureUri.StreamPlaybackKurentoUri(StartTime, EndTime), selected.RecordingId, manufactureUri.KurentoServer());
        }
        private string GetConnectionString()
        {
            if (Camera.ManufactureCode == Manufacturer.Axis)
            {
                return GetAxisConnectionString();
            }
            else
            {
                return string.Format(@"file:///{0}/Resources/Html/kurento/index.html?destination={1}&ws_uri={2}", Application.StartupPath.Replace("\\", "/"), manufactureUri.StreamPlaybackKurentoUri(StartTime, EndTime), manufactureUri.KurentoServer());
            }
        }

        private async void PaintBrowser()
        {
            try
            {
                string page = GetConnectionString();

                string instanceId = System.Diagnostics.Process.GetCurrentProcess().Id.ToString();
                string cachePath = System.IO.Path.Combine(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
                var browserExecutablePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "WebView2.131.0.2903.86.x64");
                PathUtils.CleanOldWebView2Caches(SmartClientEnvironmentUtils.GetWebView2CacheFolder(), instanceId);
                var environment = await CoreWebView2Environment.CreateAsync(browserExecutablePath, cachePath);
                await browser.EnsureCoreWebView2Async(environment);
                browser.SourceChanged += WebView2Control_SourceChanged;
                browser.Click += KurentoLiveClick;
                browser.DoubleClick += Browser_DoubleClick;
                browser.CoreWebView2.OpenDevToolsWindow();
                browser.Source = new Uri(page);
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}", LogPriority.Fatal);
            }
        }

        private void Browser_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick?.Invoke(this);
            }
        }

        private void WebView2Control_SourceChanged(object sender, CoreWebView2SourceChangedEventArgs e)
        {

        }

        private void WebView2Control_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {

        }

        private void WebView2Control_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {

        }

        private void WebView2Control_NavigationStarted(object sender, CoreWebView2NavigationStartingEventArgs e)
        {

        }

        private void KurentoLiveClick(object sender, EventArgs e)
        {
            CameraSelected(this, Camera);
        }
        private List<ButtonsContextBar> GetButtonsAudioPtz()
        {
            return new List<ButtonsContextBar>();
        }
        private List<ButtonsContextBar> GetButtons()
        {
            return new List<ButtonsContextBar>();
        }
        public List<ButtonsPlayBackBar> ButtonsNotAllowed()
        {
            return new List<ButtonsPlayBackBar>();
        }

        public bool CapacityNotAvailable(bool show)
        {
            return false;
        }

        public bool Fast()
        {
            return false;
        }

        public PlaySpeed GetCurrentSpeed()
        {
            return PlaySpeed.NORMAL;
        }

        public bool Jump(int sec, bool asc)
        {
            return false;
        }

        public bool Pause()
        {
            return false;
        }

        public bool Play()
        {
            return false;
        }

        public bool Rewind()
        {
            return false;
        }

        public bool SetStartDateTime(DateTime dateTime, bool changeSlider = true, bool isVault = false)
        {
            return false;
        }

        public bool SetStartUpSpeed(int speed)
        {
            return false;
        }

        public bool Slow()
        {
            return false;
        }

        public bool Snapshot(string path)
        {
            return false;
        }

        public bool Stop()
        {
            return false;
        }

        public bool SyncSpeed(PlaySpeed masterSpeed, bool updateLabelSpeed)
        {
            return false;
        }

        public void ToggleFullScreen()
        {
        }

        public bool ToggleListen()
        {
            return false;
        }

        public bool ToogleDigitalZoom()
        {
            return false;
        }

        public bool VideoClipStart(string path)
        {
            return false;
        }

        public bool VideoClipStop()
        {
            return false;
        }

        public bool Volume(int value)
        {
            return false;
        }

        private void Browser_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            JsonObject jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonObject>(e.WebMessageAsJson);
            switch (jsonObject.Key)
            {
                case "click":
                    CameraSelected(this, Camera);
                    break;
                case "Error":
                    Logger.Log($"Intento de reconexión: {intentNumber} \n Error: {jsonObject.Value}", LogPriority.Fatal);
                    if (intentNumber == _tryLimit)
                    {
                        this.offLine = true;
                        SetPanelNoConnectionVisibility();
                        browser.Visible = false;
                        browser.Stop();
                        browser.Dispose();
                    }
                    else
                    {
                        TryReconnection();
                    }

                    break;
            }
        }

        private void SetPanelNoConnectionVisibility(bool reconnection = false)
        {
            if (panelNoConnection.InvokeRequired)
            {
                panelNoConnection.Invoke((MethodInvoker)delegate
                {
                    SetPanelNoConnectionVisibility(reconnection);
                });
                return;
            }
            panelNoConnection.Visible = this.offLine;
            panelNoConnection.BackgroundImage = null;
            if (reconnection)
            {
                panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_en);
            }
            else
            {
                panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_en);
            }

            Reconnecting.DisplayLogo(this.Width, this.Height, ref panelNoConnection, ref panelFondoLogo);
        }

        public bool PlayVideo()
        {
            throw new NotImplementedException();
        }

        public bool PlayNoAsync()
        {
            throw new NotImplementedException();
        }

        private void TryReconnection()
        {
            browser.Visible = true;
            browser.CoreWebView2.Stop();
            browser.Refresh();
            browser.Reload();
            intentNumber++;
        }

        public int Hash()
        {
            return string.Format("{0}-{1}-{2}", Camera.Id, Recorder.RecorderType, Recorder.Id).GetHashCode();
        }

        public void SelectSpeed(PlaySpeed speed)
        {
            PlaySpeed temp = speed;
        }

        public void UpdateSlider(double sliderMaxMinutes, PlayScaleTimeLine playScaleTimeLine, bool isVault, DateTime start, DateTime end, int blockNumber, int totalBlocks)
        {
            throw new NotImplementedException();
        }

        public int GetCurrentSliderValue()
        {
            throw new NotImplementedException();
        }

        public int GetMaxSliderValue()
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateSelected()
        {
            throw new NotImplementedException();
        }
    }
}
