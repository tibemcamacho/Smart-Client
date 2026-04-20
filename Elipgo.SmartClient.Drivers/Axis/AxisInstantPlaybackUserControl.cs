using AxAXISMEDIACONTROLLib;
using AXISMEDIACONTROLLib;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Common.Reflections;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Elipgo.SmartClient.Drivers.Axis
{
    public partial class AxisInstantPlaybackUserControl : UserControl, IDriverInstantPlayback, IDisposable
    {
        private PlaybackState state = PlaybackState.Stopped;
        private ulong duration = 0;
        private bool _listenStatus = false;
        private AxAxisMediaControl amc;
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        public event OnVideoEventHandler OnVideo;
        public event OnTimeChangedEventHandler OnTimeChanged;
        public event OnStateChangedEventHandler OnStateChanged;
        public event EventHandler<bool> OpenBookmark;
        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnDriverDispose OnDispose;

        private bool _painted = false;
        public bool BookmarkState { get; set; }

        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ActualTime { get; set; }
        private DateTime InitDateTime { get; set; }
        private DateTime EndDatetime { get; set; }
        public bool ListenStatus { get; set; }

        private bool offLine = false;
        private bool ImageReceived = false;
        private ulong thirtySecond;
        private int maxTryReconnection;
        private int currentTryReconnection;
        private int tryCount = 0;
        private readonly int tryLimit;
        private readonly Configuration _config;
        private const int MAX_HOUR_END = 12;
        private double _maxMinutes = 360;
        private bool instantPlayBack = true;
        private bool InstantPlaybackVault = false;
        private bool isDiagnostic = false;
        private static int SecondsBefore = 30;
        private PlaySpeed CurrentSpeed = PlaySpeed.NORMAL;
        private RecorderDTOSmall Recorder;
        private List<Recording> recordingList = new List<Recording>();
        private bool stoploginDevice = false;
        bool firstTime = true;
        private DateTime _selectedDateTime;
        private int _sliderCurrentValue;
        private bool _scaleActive = false;
        private double _previewSliderValue;
        private PlayScaleTimeLine _currentScale = PlayScaleTimeLine.Normal;
        private int _currentBlock;
        private int _additionalSeconds;
        private bool _secondScale = false;
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_MOUSEWHEEL = 0x020A;

        public AxisInstantPlaybackUserControl(CameraDTO camera, Profile profile, DateTime selectedDateTime, bool hideControls, RecorderDTOSmall recorder, bool isDiagnostic = false, DateTime? selectedEndDateTime = null)
        {
            InitializeComponent();

            _config = SmartClientEnvironmentUtils.GetConfiguration();

            Camera = camera;
            Profile = profile;
            Recorder = recorder;


            this.Paint += AxisInstantPlaybackUserControl_Paint;
            this.slider.MouseDown += Slider_MouseDown;
            this.slider.MouseUp += Slider_MouseUp;
            this.slider.ValueChanged += Slider_ValueChanged;
            this.slider.ValueChangeComplete += Slider_ValueChangeComplete;
            this.Click += AxisInstantPlaybackUserControl_Click;
            this.DoubleClick += AxisInstantPlaybackUserControl_DoubleClick;
            ListenStatus = false;
            _selectedDateTime = selectedDateTime;
            this.isDiagnostic = isDiagnostic;

            if (isDiagnostic)
            {
                _maxMinutes = 5;
            }

            if (hideControls)
            {
                this.PanelControls.Hide();
                this.slider.Hide();
                this._panelVideo.Dock = DockStyle.Fill;
                instantPlayBack = false;
            }

            this.Resize += AxisPlaybackUserControl_Resize;

            if (selectedEndDateTime != null)
            {
                StartTime = selectedDateTime;
                InitDateTime = selectedDateTime;
                EndDatetime = (DateTime)selectedEndDateTime;
                instantPlayBack = false;
                InstantPlaybackVault = true;
            }
            else
            {
                StartTime = selectedDateTime.AddMinutes(-1 * _maxMinutes);
                InitDateTime = selectedDateTime.AddMinutes(-1 * _maxMinutes);
                EndDatetime = selectedDateTime;
            }

            ButtonBookmark.Click += ButtonBookmark_Click;
            BookmarkState = false;
            ClipStatus = false;
            thirtySecond = (ulong)new TimeSpan(0, 0, 30).Ticks / 10000;

            CultureInfo ci = CultureInfo.InstalledUICulture;
            bunifuToolTip1.SetToolTip(this.ButtonBookmark, ci.Name.Contains("es") ? ButtonsContextBar.Bookmark.GetDescription() : ButtonsContextBar.Bookmark.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonSnapshot, ci.Name.Contains("es") ? ButtonsContextBar.Snapshot.GetDescription() : ButtonsContextBar.Snapshot.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonFullScreen, ci.Name.Contains("es") ? ButtonsContextBar.Fullscreen.GetDescription() : ButtonsContextBar.Fullscreen.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonPlay, ci.Name.Contains("es") ? ButtonsContextBar.Play.GetDescription() : ButtonsContextBar.Play.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonFwdSecs, ci.Name.Contains("es") ? ButtonsContextBar.FwdSecs.GetDescription() : ButtonsContextBar.FwdSecs.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonRewSecs, ci.Name.Contains("es") ? ButtonsContextBar.RewSecs.GetDescription() : ButtonsContextBar.RewSecs.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonPause, ci.Name.Contains("es") ? ButtonsContextBar.Pause.GetDescription() : ButtonsContextBar.Pause.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonListen, ci.Name.Contains("es") ? ButtonsContextBar.Listen.GetDescription() : ButtonsContextBar.Listen.GetAttribute<DescriptionEN>().Descripcion);

            ShowButtons();
            tryLimit = int.Parse(_config.AppSettings.Settings["tryLimit"].Value);
            panelNoConnection.Visible = this.offLine;
            CreateACM();
            ConfigureAmc();

            Task.Run(() => { LoginDevice(); });
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ZoomStatus)
            {
                //Ctl++
                if (keyData == (Keys.Control | Keys.Oemplus) || keyData == (Keys.Control | Keys.Add))
                {
                    SimulateMouseWheel(120);
                }
                //Ctl+-
                if (keyData == (Keys.Control | Keys.OemMinus) || keyData == (Keys.Control | Keys.Subtract))
                {
                    SimulateMouseWheel(-120);
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SimulateMouseWheel(int delta)
        {
            Point mousePos = amc.PointToClient(Cursor.Position);
            int lParam = (mousePos.Y << 16) | (mousePos.X & 0xFFFF);

            SendMessage(this.amc.Handle, WM_MOUSEWHEEL, delta << 16, lParam);
        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    string result = e.Result;
                    XDocument xmlDoc = XDocument.Load(new System.IO.StringReader(result));
                    var query = from r in xmlDoc.Descendants("recording")
                                select new Recording
                                {
                                    RecordingId = (string)r.Attribute("recordingid").Value,
                                    StartTime = (string)r.Attribute("starttime").Value,
                                    StopTime = (string)r.Attribute("stoptime").Value,
                                    StartTimeLocal = (string)r.Attribute("starttimelocal").Value,
                                    StopTimeLocal = (string)r.Attribute("stoptimelocal").Value,
                                    RecordingStatus = (string)r.Attribute("recordingstatus").Value,
                                    MimeType = r.Descendants("video").Count() > 0 ?
                                        (string)r.Descendants("video").FirstOrDefault().Attribute("mimetype").Value : "",
                                    FrameRate = r.Descendants("video").Count() > 0 ?
                                        (string)r.Descendants("video").FirstOrDefault().Attribute("framerate").Value : "",
                                    Audio = (string)((r.Descendants("audio").Count() > 0) ? "yes" : "no")
                                };

                    if (query.Count() > 0)
                    {
                        recordingList.Clear();
                        foreach (Recording r in query.ToList<Recording>())
                        {
                            recordingList.Add(r);
                        }
                        PlayCamera();
                    }
                    else
                    {
                        var message = $"{Camera.Name} - {StartTime:yyyy/MM/dd HH:mm:ss}";
                        notification.Show(string.Format(Resources.NoRecordingAvailable, message), null);
                        this.offLine = true;
                        SetVisivility(PlaybackConnectionState.NoRecording);
                        Logger.Log($"No Recording are available:  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} ", LogPriority.Information);
                    }
                }
                else
                {
                    Logger.Log($"No Recording are available:  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} , Error: {e.Error.Message}", LogPriority.Information);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"No Recording Exception are available :  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} , Exception: {ex.Message}, StackTrace : {ex.StackTrace}", LogPriority.Information);
            }
        }

        private void ShowButtons()
        {
            this.ButtonSnapshot.Visible = appAuthorization.Exist(ButtonsContextBar.Snapshot.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonBookmark.Visible = appAuthorization.Exist(ButtonsContextBar.Bookmark.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonListen.Visible = (appAuthorization.Exist(ButtonsContextBar.Listen.GetAttribute<PermissionPlayback>().PermissionKey) && Camera.AudioEnabled);
        }

        private void SetVisivility(PlaybackConnectionState connectionState)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    SetVisivility(connectionState);
                });
                return;
            }

            CultureInfo ci = CultureInfo.InstalledUICulture;
            try
            {
                switch (connectionState)

                {
                    case PlaybackConnectionState.Disconnected:
                        _panelVideo.Visible = true;
                        panelNoConnection.BringToFront();
                        panelNoConnection.Visible = this.offLine;
                        panelNoConnection.BackgroundImage = null;
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_en);
                        }
                        break;
                    case PlaybackConnectionState.NoRecording:
                        this._panelVideo.SendToBack();
                        this._panelVideo.Visible = false;
                        this.panelNoConnection.Size = _panelVideo.Size;
                        this.panelNoConnection.Location = _panelVideo.Location;
                        panelNoConnection.BringToFront();
                        panelNoConnection.Visible = this.offLine;
                        panelNoConnection.BackgroundImage = null;
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_en);
                        }
                        break;
                    case PlaybackConnectionState.Reconnecting:
                        _panelVideo.Visible = true;
                        panelNoConnection.BringToFront();
                        panelNoConnection.BackgroundImage = null;
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_en);
                        }
                        break;
                    case PlaybackConnectionState.Connecting:
                        _panelVideo.Visible = true;
                        panelNoConnection.BringToFront();
                        panelNoConnection.Visible = this.offLine;
                        panelNoConnection.BackgroundImage = null;
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_en);
                        }
                        break;
                    case PlaybackConnectionState.Connected:
                        _panelVideo.Visible = true;
                        panelNoConnection.Visible = false;
                        break;
                }
                Reconnecting.DisplayLogo(_panelVideo.Width, _panelVideo.Height, ref panelNoConnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"setVisivility Exception: {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        private void AxisInstantPlaybackUserControl_Click(object sender, EventArgs e)
        {
            if (CameraSelected != null)
            {
                CameraSelected(this, Camera);
            }
        }

        private void AxisInstantPlaybackUserControl_DoubleClick(object sender, EventArgs e)
        {
            CameraSelectedDoubleClick(this);
        }

        private void ButtonBookmark_Click(object sender, EventArgs e)
        {
            BookmarkState = !BookmarkState;
            if (BookmarkState)
            {
                ActualTime = StartTime.AddTicks((new DateTime((long)amc.CurrentPosition64 * 10000)).Ticks);
            }

            OpenBookmark?.Invoke(this, BookmarkState);
        }

        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            _scaleActive = false;
            SliderTooltip.Text = this.InitDateTime.AddSeconds(slider.Value + _additionalSeconds).ToString("MM/dd HH:mm:ss");
            SliderTooltip.Location = new Point(slider.Location.X, slider.Location.Y - 20);
            SliderTooltip.Visible = true;
            SliderTooltip.BringToFront();
        }

        private void Slider_ValueChangeComplete(object sender, EventArgs e)
        {
            if ((amc.Status & (int)AMC_STATUS.AMC_STATUS_PAUSED) == 0)
            {
                amc.Play();
            }
            _selectedDateTime = InitDateTime.AddSeconds(slider.Value);

            if (!_scaleActive)
            {
                int sliderValueTemp = slider.Value;
                switch (_currentScale)
                {
                    case PlayScaleTimeLine.m15:
                    case PlayScaleTimeLine.x1_2:
                        sliderValueTemp += (_currentBlock == 1) ? 0 : slider.MaximumValue;
                        _scaleActive = true;
                        break;

                    case PlayScaleTimeLine.m10:
                    case PlayScaleTimeLine.m5:
                    case PlayScaleTimeLine.x1_3:
                        if (_currentBlock >= 1)
                        {
                            sliderValueTemp += slider.MaximumValue * (_currentBlock + (_secondScale ? 2 : -1));
                        }
                        _scaleActive = true;
                        break;

                    case PlayScaleTimeLine.Normal:
                    case PlayScaleTimeLine.x1:
                        _scaleActive = true;
                        break;

                    default:
                        _scaleActive = true;
                        break;

                }

                ulong newPosMilliSec = (ulong)(this.InitDateTime.AddSeconds(sliderValueTemp) - this.StartTime).TotalMilliseconds;
                amc.CurrentPosition64 = newPosMilliSec;
                _sliderCurrentValue = (int)slider.Value;
            }
            else
            {
                ulong newPosMilliSec = (ulong)(this.InitDateTime.AddSeconds(slider.Value) - this.StartTime).TotalMilliseconds;
                amc.CurrentPosition64 = newPosMilliSec;
                _sliderCurrentValue = (int)slider.Value;
            }

            SliderTooltip.Visible = false;
        }

        private void Slider_MouseUp(object sender, MouseEventArgs e)
        {
            Slider_ValueChanged(sender, e);
        }

        private void Slider_MouseDown(object sender, MouseEventArgs e)
        {
            Slider_ValueChanged(sender, e);
        }

        private void AxisInstantPlaybackUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
            {
                return;
            }

            this._painted = true;

            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            ButtonRewSecs.Image = FileResources.icon_replay_30;
            ButtonFwdSecs.Image = FileResources.icon_forward_30;
            ButtonBookmark.Image = FileResources.icon_bookmarks;
            ButtonFullScreen.Image = FileResources.icon_full_screen;
            ButtonSnapshot.Image = FileResources.icon_snapshot;
            ButtonListen.Image = FileResources.icon_sound_off;
            this.SizeChanged += AxisInstantPlaybackUserControl_SizeChanged;
            this.Resize += AxisPlaybackUserControl_Resize;
        }

        public new void Dispose()
        {
            if (amc == null)
            {
                return;
            }

            if (!amc.IsDisposed)
            {
                try
                {
                    amc.Mute = true;
                    if ((amc.Status & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0)
                    {
                        amc.StopRecordMedia();
                    }
                    amc.Stop();
                    amc.Dispose();
                    this.amc = null;
                }
                catch (Exception ex)
                {
                    Logger.Log($"Dispose Exception {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Information);
                }
            }
            stoploginDevice = true;
        }

        private async void LoginDevice()
        {
            try
            {
                recordingList.Clear();
                IManufactureUri manufactureUri = ManufactureUriFactory.Instance(this.Camera, this.Profile);

                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
                    if (!string.IsNullOrEmpty(Camera.User))
                    {
                        webClient.Credentials = new NetworkCredential(Camera.User, Camera.Password);
                    }
                    await webClient.DownloadStringTaskAsync(manufactureUri.RecordingPlaybackUri());
                }
            }
            catch (Exception ex)
            {
                this.offLine = true;
                if (stoploginDevice)
                {
                    return;
                }
                if (tryCount < tryLimit)
                {
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    tryCount++;
                    Threads.RunInOtherThread(new Action[] { () => System.Threading.Thread.Sleep(2000 * tryCount) }, () => LoginDevice());
                    Logger.Log($"Axis LoginDevice  Error  Login to Camera:  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} current {tryCount} of {tryLimit} ", LogPriority.Information);
                }
                else
                {
                    notification.Show($"{Camera.Name} - {ex.Message}", null);
                    SetVisivility(PlaybackConnectionState.Disconnected);
                    Logger.Log($"Axis  LoginDevice reached max retry number, then it is  disconnected: {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} ", LogPriority.Information);
                }
            }
        }

        private void PlayCamera()
        {
            if (amc == null)
            {
                Logger.Log($"Axis PlayCamera  Error  AMC is null to Camera:  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                return;
            }
            if (recordingList.Count > 0)
            {
                amc.Stop();
                IManufactureUri manufactureUri = ManufactureUriFactory.Instance(this.Camera, this.Profile);

                DateTime dt = this.StartTime;
                Recording selected = new Recording();
                foreach (Recording element in recordingList)
                {
                    string DateTimeFormat = "yyyy-MM-dd'T'HH:mm:ss.ffffff'Z'";
                    string xt = Convert.ToDateTime(element.StartTime).ToUniversalTime() > DateTime.Now.AddHours(-1) ? Convert.ToDateTime(element.StartTime).ToString(DateTimeFormat) : element.StartTime;

                    DateTime d = DateTime.ParseExact(xt, DateTimeFormat, CultureInfo.InvariantCulture);
                    if (element.RecordingStatus != "completed")
                    {
                        if (d < dt.ToUniversalTime())
                        {
                            selected = element;
                            break;
                        }
                    }
                    else
                    {

                        string d2 = element.StopTime;
                        DateTime dt2 = DateTime.ParseExact(d2, DateTimeFormat, CultureInfo.InvariantCulture);
                        double diffSec = d.Subtract(dt).TotalSeconds;
                        dt = (dt.ToString("yyyy-MM-dd'T'HH:mm'Z'") == d.ToString("yyyy-MM-dd'T'HH:mm'Z'") ? dt.AddSeconds(diffSec + 1) : dt);
                        if (Convert.ToDateTime(element.StartTime).ToUniversalTime() >= Convert.ToDateTime(element.StartTimeLocal.Replace("-06:00", "")))
                        {

                            string d2local = element.StopTimeLocal.Replace("-06:00", "Z");
                            DateTime dt2local = DateTime.ParseExact(d2local, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                            var xtLocal = (Convert.ToDateTime(element.StartTimeLocal.Replace("-06:00", "Z")).ToUniversalTime() > DateTime.Now ? Convert.ToDateTime(element.StartTimeLocal.Replace("-06:00", "Z")).ToString(DateTimeFormat) : element.StartTimeLocal.Replace("-06:00", "Z"));
                            var dlocal = DateTime.ParseExact(xtLocal, DateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
                            if (dt.AddSeconds(1) > dlocal && dt < dt2local)
                            {
                                selected = element;

                                break;
                            }
                        }
                        else
                        {
                            if (dt > d && dt < dt2)
                            {
                                selected = element;

                                break;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(selected.RecordingId))
                {
                    string recordingId = selected.RecordingId;
                    DateTime StartTime = DateTime.Parse(selected.StartTime).AddHours(Camera.Dst ? -1 : 0);
                    DateTime StopTime = (InstantPlaybackVault ? EndDatetime : DateTime.UtcNow);

                    double offset = 0;

                    this.StartTime = StartTime.AddMinutes(offset);
                    this.EndTime = StopTime;

                    if (selected.RecordingStatus == "completed")
                    {
                        StopTime = DateTime.Parse(selected.StopTime).ToUniversalTime();
                    }
                    if (dt > StopTime)
                    {
                        this.offLine = true;
                        var message = $"{Camera.Name} - {dt:yyyy/MM/dd HH:mm:ss}";
                        notification.Show(string.Format(Resources.NoRecordingAvailable, message), null);
                        SetVisivility(PlaybackConnectionState.NoRecording);
                        return;
                    }
                    TimeSpan currentDuration = (StopTime.AddSeconds(2) - StartTime);
                    MediaDuration = (ulong)currentDuration.Ticks / 10000;

                    amc.BackgroundColor = 0;
                    amc.StretchToFit = true;
                    amc.Popups &= ~((int)AMC_POPUPS.AMC_POPUPS_NO_VIDEO);
                    amc.EnableOverlays = false;
                    amc.PlaybackMode = (int)AMC_PLAYBACK_MODE.AMC_PM_RECORDING;
                    amc.UIMode = "none";
                    amc.MediaURL = manufactureUri.StreamPlaybackUri() + recordingId;
                    amc.MediaUsername = Camera.User;
                    amc.MediaPassword = Camera.Password;

                    var t4 = StopTime - dt;

                    var t = currentDuration.Add(-t4);
                    amc.CurrentPosition64 = (ulong)t.Ticks / 10000;
                    amc.Play();

                    slider.MaximumValue = (int)((InstantPlaybackVault) ? EndDatetime - InitDateTime : InitDateTime.AddMinutes(_maxMinutes) - InitDateTime).TotalSeconds;
                    slider.Enabled = true;

                    if (instantPlayBack && firstTime)
                    {
                        ulong newPosMilliSec = (ulong)((isDiagnostic ? EndDatetime.AddMinutes(-5).AddSeconds(-SecondsBefore) : EndDatetime.AddMinutes(-1)) - this.StartTime).TotalMilliseconds;

                        amc.CurrentPosition64 = newPosMilliSec;
                        firstTime = false;
                        if (EndDatetime.ToUniversalTime().AddMinutes(-1) > DateTime.UtcNow)
                        {
                            var message = $"{Camera.Name} - {EndDatetime.AddMinutes(-1).ToString("yyyy/MM/dd HH:mm:ss")}";
                            notification.Show(string.Format(Resources.NoRecordingAvailable, message), null);
                            amc.Stop();
                            Logger.Log($"No Recording are available:  {Camera.Name} {Camera.Host} {Camera.VideoPort.ToString()} {Camera.User} ", LogPriority.Information);
                        }
                    }
                }
                else
                {
                    var message = $"{Camera.Name} - {StartTime.ToString("yyyy/MM/dd HH:mm:ss")}";
                    notification.Show(string.Format(Resources.NoRecordingAvailable, message), null);
                    this.offLine = true;
                    SetVisivility(PlaybackConnectionState.NoRecording);
                    Logger.Log($"No Recording are available:  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} ", LogPriority.Information);
                }

            }

        }

        private void ConfigureAmc()
        {
            this.amc.StretchToFit = true;
            this.amc.MaintainAspectRatio = false;
            this.amc.ShowStatusBar = false;
            this.amc.BackgroundColor = 0; // black
            this.amc.VideoRenderer = (int)AMC_VIDEO_RENDERER.AMC_VIDEO_RENDERER_EVR;
            this.amc.EnableOverlays = true;
            this.amc.Mute = true;

            // Configure context menu
            this.amc.EnableContextMenu = false;
            this.amc.ToolbarConfiguration = "-play,-fullscreen,-settings";

            // AMC messaging setting
            this.amc.Popups = 0;
            this.amc.UIMode = "none";
            this.amc.NetworkTimeout = 4000;
            this.amc.EnableReconnect = true;
            this.amc.SetReconnectionStrategy(60000, 10000, 300000, 30000, 300000, 60000, true);
            this.currentTryReconnection = 0;
            this.maxTryReconnection = (60000 / 10000) + (300000 / 30000) + (300000 / 60000);
        }

        private void CreateACM()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(AxisLiveUserControl));
            this.amc = new AxAxisMediaControl();
            ((ISupportInitialize)(this.amc)).BeginInit();
            this.SuspendLayout();

            this.amc.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right;
            this.amc.Enabled = true;
            this.amc.Location = new Point(0, 0);
            this.amc.Name = "amc";
            this.amc.OcxState = ((AxHost.State)(resources.GetObject("amc.OcxState")));
            this.amc.Dock = DockStyle.Fill;
            this.amc.TabIndex = 0;
            this.amc.TabStop = false;
            this.amc.OnError += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.amc_OnError);
            this.amc.OnMouseMove += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnMouseMoveEventHandler(this.amc_OnMouseMove);
            this.amc.OnStatusChange += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnStatusChangeEventHandler(this.Amc_OnStatusChange);
            this.amc.OnNewVideoSize += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnNewVideoSizeEventHandler(this.amc_OnNewVideoSize);
            this._panelVideo.Controls.Add(this.amc);
            ((ISupportInitialize)(this.amc)).EndInit();
            this.ResumeLayout(false);

            amc.OnClick += Amc_OnClick;
            amc.OnDoubleClick += Amc_OnDoubleClick;
            amc.OnNewImage += Amc_OnNewImage;
        }

        private void amc_OnNewVideoSize(object sender, _IAxisMediaControlEvents_OnNewVideoSizeEvent e)
        {

        }

        private void amc_OnMouseMove(object sender, _IAxisMediaControlEvents_OnMouseMoveEvent e)
        {

        }

        private void AxisPlaybackUserControl_Resize(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    AxisPlaybackUserControl_Resize(sender, e);
                });
                return;
            }

            Reconnecting.DisplayLogo(this.Width, this.Height, ref panelNoConnection, ref panelFondoLogo);
            this.slider.Location = new Point(this._panelVideo.Location.X, this._panelVideo.Location.Y + this._panelVideo.Height - 12);
            this._panelVideo.Dock = DockStyle.Fill;
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                Form instantPlayerView = Application.OpenForms.OfType<Form>().FirstOrDefault(f => f.Name == "InstantPlayerView" && (string)f.Tag == Camera.IdGuid);
                var playBack = Application.OpenForms["PlaybackView"];
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                this._panelVideo.Dock = DockStyle.None;

                if (main.Width > 1700 && main.Width < 2000)
                {
                    slider.Height = 30;
                }
                if (main.Width > 1400 && main.Width < 1700)
                {
                    slider.Height = 25;
                }
                else if (main.Width >= 1366 && main.Width < 1400)
                {
                    slider.Height = 20;
                }

                if (instantPlayerView?.WindowState == FormWindowState.Maximized)
                {
                    var panelVideoHeight = (int)Math.Round(main.Height * 0.745M, 2);
                    _panelVideo.Width = this.Width;
                    if (main.Width >= 1366 && main.Width < 1400)
                    {
                        slider.Height = 80;
                        panelVideoHeight = (int)Math.Round(main.Height * 0.6783M, 2);
                    }
                    else if (main.Width == 1024 && main.Height == 768)
                    {
                        panelVideoHeight = (int)Math.Round(main.Height * 0.745M, 2) - 25;
                    }

                    _panelVideo.Height = panelVideoHeight;
                    slider.BringToFront();
                }
                else if (instantPlayerView?.WindowState == FormWindowState.Normal)
                {
                    int panelControlsWidth = (int)Math.Round(main.Width * 0.371M, 2);
                    int panelControlsHeight = (int)Math.Round(main.Height * 0.047M, 2);
                    
                    int panelVideoWidth = (int)Math.Round(main.Width * 0.371M, 2);
                    int panelVideoHeigth = (int)Math.Round(main.Height * 0.325M, 2);

                    if (main.Width == 1024 && main.Height == 768)
                    {
                        panelControlsWidth = this.Width;
                        panelControlsHeight = (int)Math.Round(this.Height * 0.152M, 2);
                        this.PanelControls.Location = new Point(0, 320);

                        panelVideoWidth = this.Width;
                        panelVideoHeigth = (int)Math.Round(this.Height * 0.805M, 2);
                    }

                    this.PanelControls.Size = new Size(panelControlsWidth, panelControlsHeight);
                    this._panelVideo.Size = new Size(panelVideoWidth, panelVideoHeigth);
                }
                else if (instantPlayerView == null && _panelVideo.Height > 890)
                {
                    this._panelVideo.Height = (int)Math.Round(main.Height * 0.825M, 2);
                    this._panelVideo.Width = this.Width;
                }
                else
                {
                    if (isDiagnostic)
                    {
                        _panelVideo.Dock = DockStyle.None;
                        _panelVideo.Height = Height - PanelControls.Height;
                        _panelVideo.Width = slider.Width;
                        slider.Visible = false;
                    }
                    else
                    {
                        _panelVideo.Dock = DockStyle.Fill;
                        _panelVideo.Width = this.Width;
                    }

                }

                var btn = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                var btnLocationX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.0208M), 2));
                var btnLocation = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138M), 2));

                ButtonPlay.Size = new Size(btn, btn);
                ButtonPlay.Location = new Point(btnLocationX, btnLocation);

                var buttonPauseX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.0849), 2));
                var buttonPauseY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138), 2));

                ButtonPause.Size = new Size(btn, btn);
                ButtonPause.Location = new Point(buttonPauseX, buttonPauseY);

                var ButtonRewSecsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.4791), 2));
                var ButtonRewSecsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138), 2));

                ButtonRewSecs.Size = new Size(btn, btn);
                ButtonRewSecs.Location = new Point(ButtonRewSecsX, ButtonRewSecsY);

                var ButtonFwdSecsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.5403), 2));
                var ButtonFwdSecsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138), 2));

                ButtonFwdSecs.Size = new Size(btn, btn);
                ButtonFwdSecs.Location = new Point(ButtonFwdSecsX, ButtonFwdSecsY);

                var ButtonBookmarkX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.7534), 2));
                var ButtonBookmarkY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138), 2));

                ButtonBookmark.Size = new Size(btn, btn);
                ButtonBookmark.Location = new Point(ButtonBookmarkX, ButtonBookmarkY);

                var ButtonFullScreenX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.7019), 2));
                var ButtonFullScreenY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138), 2));

                ButtonFullScreen.Size = new Size(btn, btn);
                ButtonFullScreen.Location = new Point(ButtonFullScreenX, ButtonFullScreenY);

                var ButtonSnapshotX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.8050), 2));
                var ButtonSnapshotY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138), 2));

                ButtonSnapshot.Size = new Size(btn, btn);
                ButtonSnapshot.Location = new Point(ButtonSnapshotX, ButtonSnapshotY);

                var ButtonListenX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.8550), 2));
                var ButtonListenY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138), 2));

                ButtonListen.Size = new Size(btn, btn);
                ButtonListen.Location = new Point(ButtonListenX, ButtonListenY);

                if (main.Width == 1024 && main.Height == 768)
                {
                    ButtonBookmark.Location = new Point(ButtonBookmarkX + 40, ButtonBookmarkY);
                    ButtonFullScreen.Location = new Point(ButtonFullScreenX + 40, ButtonFullScreenY);
                    ButtonSnapshot.Location = new Point(ButtonSnapshotX + 40, ButtonSnapshotY);
                    ButtonListen.Location = new Point(ButtonListenX + 40, ButtonListenY);
                }

                slider.BringToFront();
            }
        }

        private void amc_OnError(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEvent e)
        {
            this.offLine = true;
            if (ImageReceived == false)
            {// es la primera vez cuando offline esta en true entonces voy a desconetar directamente
                Logger.Log($"OnError Camera : {Camera.Name} Error code {e.theErrorCode:X8} ", LogPriority.Information);
                notification.Show($"Camera :  {Camera.Name}  Error code {e.theErrorCode:X8}", null);
                SetVisivility(PlaybackConnectionState.Disconnected);
                Logger.Log($"---------> Se ha desconectado Axis Playback, Camara {Camera.Name}", LogPriority.Information);
                this.amc.OnError -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.amc_OnError);
            }
            else
            {
                if (currentTryReconnection <= maxTryReconnection)
                {
                    currentTryReconnection++;
                    Logger.Log($"OnError Camera {Camera.Name} Try to Connect {currentTryReconnection} of {maxTryReconnection}", LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    Logger.Log($"---------> Intentando conectar Axis Playback, Intento [{currentTryReconnection}]. Camara: {Camera.Name}", LogPriority.Information);
                }
                else
                {
                    Logger.Log($"OnError Disconnected Camera {Camera.Name} reached max retry number {maxTryReconnection} ", LogPriority.Information);
                    notification.Show($"Camera : {Camera.Name} Error code {e.theErrorCode:X8} ", null);
                    SetVisivility(PlaybackConnectionState.Disconnected);
                    Logger.Log($"---------> Se ha desconectado Axis Playback, Camara {Camera.Name}", LogPriority.Information);
                    this.amc.OnError -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.amc_OnError);
                }
            }
        }

        private void Amc_OnClick(object sender, _IAxisMediaControlEvents_OnClickEvent e)
        {
            if (CameraSelected != null)
            {
                CameraSelected(this, Camera);
            }
        }

        private void Amc_OnDoubleClick(object sender, _IAxisMediaControlEvents_OnDoubleClickEvent e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick(this);
            }
        }

        private void Amc_OnNewImage(object sender, EventArgs e)
        {
            if (offLine == true)
            {
                this.currentTryReconnection = 0;
                offLine = false;
                Logger.Log($"---------> Conexion Axis Playback, Camara {Camera.Name}", LogPriority.Information);
                SetVisivility(PlaybackConnectionState.Reconnecting);
                this.amc.OnError += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.amc_OnError);
            }
            ImageReceived = true;
            ulong scale = (ulong)(slider.MaximumValue);
            if (amc.CurrentPosition64 > MediaDuration)
            {
                slider.Value = slider.MaximumValue;
                Stop();
            }
            else
            {
                if (!_scaleActive)
                {
                    int tSecs = (int)(StartTime.AddMilliseconds(amc.CurrentPosition64) - InitDateTime).TotalSeconds;
                    if (tSecs < slider.MaximumValue)
                    {
                        if (tSecs < 0)
                        {
                            slider.Value = 0;
                        }
                        else
                        {
                            slider.Value = (int)(StartTime.AddMilliseconds(amc.CurrentPosition64) - InitDateTime).TotalSeconds;
                        }
                    }
                    else
                    {
                        if (!_scaleActive)
                        {
                            slider.Value = slider.MaximumValue;
                            Stop();
                        }

                    }
                }
            }
            var currentDateTime = StartTime.AddMilliseconds(amc.CurrentPosition64);
            _selectedDateTime = StartTime.AddMilliseconds(amc.CurrentPosition64);
            //currentDateTime.AddMinutes(DateTimeOffset.Now.Offset.TotalMinutes + (Camera.Gmt * 1))
            OnTimeChanged?.Invoke(currentDateTime, this);
            OnVideo?.Invoke(true, this);
            SetVisivility(PlaybackConnectionState.Connected);
        }

        private void Amc_OnStatusChange(object sender, _IAxisMediaControlEvents_OnStatusChangeEvent e)
        {
            if ((e.theNewStatus & (int)AMC_STATUS.AMC_STATUS_PAUSED) > 0)
            {
                SetState(PlaybackState.Paused);
            }
            else if ((e.theNewStatus & (int)AMC_STATUS.AMC_STATUS_PLAYING) > 0)
            {
                SetState(PlaybackState.Playing);
            }
            else
            {
                SetState(PlaybackState.Stopped);
            }
        }

        private void SetState(PlaybackState state)
        {
            this.state = state;

            switch (state)
            {
                case PlaybackState.Stopped:
                    break;
                case PlaybackState.Paused:
                    break;
                case PlaybackState.Playing:
                    break;
            }
            OnStateChanged?.Invoke(state, this);
        }

        public bool Snapshot(string path)
        {
            try
            {
                amc.SaveCurrentImage(0, path);
                notification.Show(Resources.SnapshotSaved, () => Process.Start(path));
                return File.Exists(path);
            }
            catch (Exception ex)
            {
                notification.Show(ex.Data.ToString(), null);
                return false;
            }
        }

        public void ToggleFullScreen()
        {
            try
            {
                amc.FullScreen = !amc.FullScreen;
            }
            catch (Exception ex)
            {
                notification.Show(ex.Data.ToString(), null);
            }
        }
        string _path = string.Empty;
        public bool VideoClipStart(string path)
        {
            try
            {
                path = path.Replace(".mp4", ".asf");
                if ((amc.Status & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0)
                {
                    amc.StopRecordMedia();
                }
                else
                {
                    int recordingFlag = (int)AMC_RECORD_FLAG.AMC_RECORD_FLAG_AUDIO_VIDEO;
                    recordingFlag = (int)AMC_RECORD_FLAG.AMC_RECORD_FLAG_VIDEO;

                    amc.StartRecordMedia(path, recordingFlag, "");
                }
                _path = path;
                ClipStatus = true;
                return true;
            }
            catch (Exception ex)
            {
                notification.Show($"{Camera.Name} - {ex.Data}", null);
                ClipStatus = false;
                return false;
            }
        }

        public bool VideoClipStop()
        {
            try
            {
                amc.StopRecordMedia();
                notification.Show(Common.Properties.Resources.VideoclipSaved, () => Process.Start(_path));
                _path = string.Empty;
                ClipStatus = false;
                return true;
            }
            catch (Exception ex)
            {
                notification.Show($"{Camera.Name} - {ex.Data}", null);
                ClipStatus = false;
                return false;
            }
        }

        public bool ToggleListen()
        {
            try
            {
                amc.Mute = !amc.Mute;
                ListenStatus = !amc.Mute;
                return true;
            }
            catch (Exception)
            {
                ListenStatus = false;
                return false;
            }
        }

        public bool Volume(int value)
        {
            throw new NotImplementedException();
        }

        public bool Play()
        {
            try
            {
                Logger.Log($"Play Axis entered  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} ", LogPriority.Information);
                SetVisivility(PlaybackConnectionState.Connecting);

                PlayCamera();
                Logger.Log($"Play Axis Connected  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} ", LogPriority.Information);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Play Exception  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} {ex.Message} ", LogPriority.Fatal);
                notification.Show($"{Camera.Name} - {1}", null);
                return false;
            }
        }

        public bool Stop()
        {
            //    CapacityNotAvailable(false);
            amc.Stop();
            return true;
        }

        public bool Pause()
        {
            if (state == PlaybackState.Playing)
            {
                amc.TogglePause();
            }

            return true;
        }


        public bool SetStartDateTime(DateTime dateTime, bool changeSlider = true, bool isVault = false)
        {
            StartTime = dateTime;
            InitDateTime = dateTime;
            //actualTime = dateTime;
            ActualTime = dateTime;

            if (changeSlider)
            {
                EndTime = DateTime.Now.AddHours(MAX_HOUR_END); //date.TotalMinutes > 60 ? dateTime.AddMinutes(60) : DateTime.Now.AddMinutes(-2);
                var tSecs = (EndTime - StartTime).TotalSeconds;
                slider.MaximumValue = (int)tSecs;
            }
            else
            {
                EndTime = Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd 23:59:59"));
            }


            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            return true;
        }

        public ulong MediaDuration
        {
            get
            {
                if (duration > 0)
                {
                    return duration;
                }
                else
                {
                    return amc.Duration64;
                }
            }

            set => duration = value;
        }

        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }

        public List<ButtonsContextBar> Commands => GetButtons();
        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();

        private List<ButtonsContextBar> GetButtons()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>
            {
                ButtonsContextBar.Fullscreen,
                ButtonsContextBar.Snapshot,
                ButtonsContextBar.Videoclip,
                ButtonsContextBar.DigitalZoom,
                ButtonsContextBar.Bookmark
            };



            return commands;
        }

        private List<ButtonsContextBar> GetButtonsAudioPtz()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>();

            if (Camera.AudioEnabled)
            {
                commands.Add(ButtonsContextBar.Listen);
            }
            if (Camera.TalkEnabled)
            {
                commands.Add(ButtonsContextBar.Talk);
            }

            return commands;
        }

        public bool ClipStatus { get; set; } = false;
        public bool ZoomStatus { get; set; }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            Pause();
            CapacityNotAvailable(false);
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            if (state == PlaybackState.Paused || state == PlaybackState.Stopped)
            {
                amc.Play();
            }
            //Play();
            CapacityNotAvailable(false);
        }

        public bool Rewind()
        {
            Pause();
            CapacityNotAvailable(true);
            return false;
        }

        public bool Slow()
        {//La funcionalidad no esta implementado para axis es por ello cuando vuelvo a Normal speed hago play
            CurrentSpeed = (PlaySpeed)((int)CurrentSpeed - 1);
            if (CurrentSpeed == PlaySpeed.NORMAL)
            {
                Play();
                CapacityNotAvailable(false);
                return true;
            }
            else
            {
                Pause();
                CapacityNotAvailable(true);
                return false;
            }

        }
        public bool SyncSpeed(PlaySpeed masterSpeed, bool updateLabelSpeed)
        {
            if (masterSpeed == PlaySpeed.NORMAL)
            {
                if (state != PlaybackState.Playing)
                {
                    Play();
                }
                CapacityNotAvailable(false);
            }
            else
            {
                CurrentSpeed = masterSpeed;
                CapacityNotAvailable(true);
            }
            return true;
        }

        public bool Fast()
        {//La funcionalidad no esta implementado para axis es por ello cuando vuelvo a Normal speed hago play
            CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
            if (CurrentSpeed == PlaySpeed.NORMAL)
            {
                Play();
                CapacityNotAvailable(false);
                return true;
            }
            else
            {
                Pause();
                CapacityNotAvailable(true);
                return false;
            }
        }
        public bool Fast(int numoftimes)
        {
            Pause();
            CapacityNotAvailable(true);
            return false;
        }

        public bool SetStartUpSpeed(int speed)
        {
            Pause();
            CapacityNotAvailable(true);
            return false;
        }

        public bool CapacityNotAvailable(bool show)
        {
            if (this.LabelCapacityNotAvailable.Visible != show)
            {
                this.LabelCapacityNotAvailable.Text = Resources.CapacityNotAvailable;
                this.LabelCapacityNotAvailable.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
                this.LabelCapacityNotAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                this.LabelCapacityNotAvailable.Left = (this.LabelCapacityNotAvailable.Parent.Width - this.LabelCapacityNotAvailable.Width) / 2;
                this.LabelCapacityNotAvailable.Top = (this.LabelCapacityNotAvailable.Parent.Height - this.LabelCapacityNotAvailable.Height) / 2;
            }
            this.LabelCapacityNotAvailable.Visible = show;
            return true;
        }

        private void AxisInstantPlaybackUserControl_SizeChanged(object sender, EventArgs e)
        {
            //le resto -12 para que se ubique dentro del panel video
            this.slider.Location = new Point(this._panelVideo.Location.X, this._panelVideo.Location.Y + this._panelVideo.Height);
        }

        private void ButtonRewSecs_Click(object sender, EventArgs e)
        {
            this.EndTime = DateTime.UtcNow;
            MediaDuration = (ulong)(this.EndTime - StartTime).Ticks / 10000;
            Jump(30, false);
            //slider.Value = (int)(((amc.CurrentPosition64 * (ulong)(slider.MaximumValue)) / MediaDuration));
            if (slider.Value - 30 > 0)
            {
                slider.Value -= 30;
            }
        }

        private void ButtonFwdSecs_Click(object sender, EventArgs e)
        {
            this.EndTime = DateTime.UtcNow;
            MediaDuration = (ulong)(this.EndTime - StartTime).Ticks / 10000;
            Jump(30, true);
            //slider.Value = (int)(((amc.CurrentPosition64 * (ulong)(slider.MaximumValue)) / MediaDuration));
            if ((slider.Value + 30) < slider.MaximumValue)
            {
                slider.Value += 30;
            }
        }

        public bool Jump(int sec, bool asc)
        {//La funcionalidad solo esta implementada para Normal speed
            if (CurrentSpeed == PlaySpeed.NORMAL)
            {
                ulong delta = (ulong)new TimeSpan(0, 0, sec).Ticks / 10000;
                if (asc)
                {
                    amc.CurrentPosition64 = amc.CurrentPosition64 + delta > MediaDuration ? MediaDuration : amc.CurrentPosition64 + this.thirtySecond;
                }
                else
                {
                    amc.CurrentPosition64 = amc.CurrentPosition64 - delta < 0 ? 0 : amc.CurrentPosition64 - this.thirtySecond;
                }
                var currentDateTime = StartTime.AddMilliseconds(amc.CurrentPosition64);
                OnTimeChanged?.Invoke(currentDateTime, this);
            }
            return true;
        }

        private void ButtonSnapshot_Click(object sender, EventArgs e)
        {
            string fileName = String.Format("{0}_{1}.{2}", DateTime.Now.ToString("yyyyMMddHHmmss"), Camera.Id.ToString(), "jpg");
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog.Title = "Save an Image File";
            saveFileDialog.FileName = fileName;
            saveFileDialog.InitialDirectory = string.IsNullOrEmpty(Settings.Default.LastSavedRoute) ? Elipgo.SmartClient.Common.Properties.Settings.Default["DefaultLocation"].ToString() + "\\Snapshot" : Elipgo.SmartClient.Common.Properties.Settings.Default.LastSavedRoute;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Elipgo.SmartClient.Common.Properties.Settings.Default.LastSavedRoute = Path.GetDirectoryName(saveFileDialog.FileName);
                Properties.Settings.Default.Save();
                //var path = Settings.Default["DefaultLocation"].ToString() + "\\Snapshot";
                Snapshot(saveFileDialog.FileName);
                notification.Show(Resources.SnapshotSaved, () => Process.Start(saveFileDialog.FileName));
            }
        }

        private void ButtonListen_Click(object sender, EventArgs e)
        {
            ButtonListen.Image = _listenStatus ? FileResources.icon_sound_off : FileResources.icon_sound_on;
            ToggleListen();
            _listenStatus = !_listenStatus;
        }

        public bool ToogleDigitalZoom()
        {
            try
            {
                amc.UIMode = (amc.UIMode == "none") ? "digital-zoom" : "none";
                ZoomStatus = amc.UIMode == "digital-zoom";
            }
            catch (Exception)
            {
                ZoomStatus = false;
            }

            return ZoomStatus;
        }

        public PlaySpeed GetCurrentSpeed()
        {
            return PlaySpeed.NORMAL;
        }

        public List<ButtonsPlayBackBar> ButtonsNotAllowed()
        {
            return new List<ButtonsPlayBackBar>() { ButtonsPlayBackBar.Fast, ButtonsPlayBackBar.Slow, ButtonsPlayBackBar.Rewind };
        }

        public int Hash()
        {
            //return string.Format("{0}-{1}-{2}", ElementId, RecorderType, RecorderId).GetHashCode();
            return string.Format($"{Camera.Id}-{Recorder.RecorderType}-{Recorder.Id}").GetHashCode();
        }

        public bool PlayVideo()
        {
            try
            {
                Logger.Log($"Play Axis entered  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} ", LogPriority.Information);
                CapacityNotAvailable(false);
                amc.Play();
                Logger.Log($"Play Axis Connected  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} ", LogPriority.Information);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Play Exception  {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
                notification.Show($"{Camera.Name} - {ex.Message}", null);
                return false;
            }
        }

        public bool PlayNoAsync()
        {
            PlayCamera();
            return true;
        }

        public void SelectSpeed(PlaySpeed speed)
        {
            PlaySpeed temp = speed;
            //La funcionalidad no esta implementado para axis es por ello cuando vuelvo a Normal speed hago play
            while (temp != CurrentSpeed)
            {
                if (CurrentSpeed >= PlaySpeed.MIN && CurrentSpeed < PlaySpeed.NORMAL)
                {
                    CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
                    temp = (PlaySpeed)((int)temp + 1);
                }
                else
                {
                    temp = (PlaySpeed)((int)temp - 1);

                }
            }
        }


        public void UpdateSlider(double sliderMaxMinutes, PlayScaleTimeLine playScaleTimeLine, bool secondScale, DateTime start, DateTime end, int blockNumber, int totalBlocks)
        {
            _secondScale = secondScale;
            _currentScale = playScaleTimeLine;
            slider.MaximumValue = (int)sliderMaxMinutes;

            if (playScaleTimeLine == PlayScaleTimeLine.Normal && _previewSliderValue > 0)
            {
                _sliderCurrentValue = (int)((_previewSliderValue * (int)playScaleTimeLine) + slider.Value);
            }

            _scaleActive = playScaleTimeLine != PlayScaleTimeLine.Normal;

            if (playScaleTimeLine == PlayScaleTimeLine.Normal)
            {
                slider.Value = _sliderCurrentValue;
            }
            else
            {
                _currentBlock = blockNumber;
                int valueDecrement = (int)sliderMaxMinutes * (_currentBlock - 1);
                slider.Value = _sliderCurrentValue - valueDecrement;
            }

            _additionalSeconds = 0;

            if (_currentBlock > 1)
            {
                int adjustment = _currentBlock - 1;

                switch (playScaleTimeLine)
                {
                    case PlayScaleTimeLine.m15:
                        _additionalSeconds = (int)sliderMaxMinutes * adjustment;
                        break;

                    case PlayScaleTimeLine.m10:

                        _additionalSeconds = (int)((!secondScale ? sliderMaxMinutes * (_currentBlock - 1) : sliderMaxMinutes * (adjustment * 2.5)));
                        break;

                    case PlayScaleTimeLine.m5:
                        if (_currentBlock <= 6)
                        {
                            _additionalSeconds = (int)sliderMaxMinutes * adjustment;
                        }
                        break;
                }
            }

            _previewSliderValue = sliderMaxMinutes;

        }

        public int GetCurrentSliderValue()
        {
            _sliderCurrentValue = slider.Value;
            return _sliderCurrentValue;
        }

        public int GetMaxSliderValue()
        {
            return slider.MaximumValue;
        }

        public DateTime GetDateSelected()
        {
            return _selectedDateTime;
        }
    }
}