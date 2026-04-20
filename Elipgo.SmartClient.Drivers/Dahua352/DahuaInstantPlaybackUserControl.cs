using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers.Dahua352.NetSDKCS;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Elipgo.SmartClient.Drivers.Dahua352
{
    public partial class DahuaInstantPlaybackUserControl : UserControl, IDriverInstantPlayback, IDisposable, IConectionNotification
    {
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        public ILoginControl loginControl = Locator.Current.GetService<ILoginControl>();
        private EM_RealPlayType dahuaProfile { get; set; }
        private bool _listenStatus = false;
        private IntPtr loginHandle = IntPtr.Zero;
        private IntPtr playbackHandle = IntPtr.Zero;
        public event OnDriverDispose OnDispose;
        // private fDisConnectCallBack m_DisConnectHandle;
        private fDownLoadPosCallBack m_DownLoadPosHandle;
        private PlaySpeed CurrentSpeed = PlaySpeed.NORMAL;
        //manage savedata handel.
        private List<IntPtr> m_SaveDataHandleList = new List<IntPtr>();

        private DateTime m_PlayBack_StartTime = DateTime.Now.AddMinutes(-30);
        private DateTime initialTime = DateTime.Now.AddMinutes(-30);
        private DateTime m_PlayBack_EndTime = DateTime.Now.AddMinutes(-2);
        private NET_TIME m_OsdTime = new NET_TIME();
        private NET_TIME m_OsdStartTime = new NET_TIME();
        private NET_TIME m_OsdEndTime = new NET_TIME();
        private fDisConnectCallBack m_DisConnectCallback;
        private fHaveReConnectCallBack m_ReConnectCallBack;
        private DispatcherTimer m_DispatcherTimer = new DispatcherTimer();
        private DateTime actualTime = DateTime.Now.AddMinutes(-30);
        private DateTime CameraTime = DateTime.Now;
        public bool BookmarkState { get; set; }
        private string filePathSnap;
        public bool ClipStatus { get; set; }
        public bool ListenStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ActualTime { get; set; }
        private bool offLine = false;
        private const int MAX_HOUR_END = 12;
        private int MAX_MINUTES = 360;
        private bool instantPlayback = true;
        private bool isDiagnostic = false;
        private static int SecondsBefore = 30;
        private delegate void SafeCallDelegate(PlaybackConnectionState connectionState);
        public bool IsPlaying { get; set; }
        private int retryCount = 0;
        private readonly int retryLimit;
        private int tryCount = 0;
        private readonly int tryLimit;
        private int retryCallbackCount = 0;
        private readonly int retryCallbackLimit;
        private readonly Random _random = new Random();
        private const uint TimeOut = 3000;
        private double TimeReConnectionCheck;
        private RecorderDTOSmall Recorder;
        private bool _isBusyRew = false;
        private bool _isBusyFwd = false;

        private bool _secondScale;
        private PlayScaleTimeLine _currentScale;
        private int _previewSliderValue;
        private int _sliderCurrentValue;
        private bool _scaleActive;
        private int _currentBlock;
        private int _additionalSeconds;
        private DateTime _selectedDateTime;
        public DahuaInstantPlaybackUserControl(CameraDTO camera, Profile profile, bool hideControls, DateTime selectedDateTime, RecorderDTOSmall recorder, bool isDiagnostic = false, DateTime? selectedEndDateTime = null)
        {
            InitializeComponent();
            Task.Run(() =>
            {
                m_DisConnectCallback = new fDisConnectCallBack(DisconnectCallBack); //set disconnect callback.
                bool initNETClient = NETClient.Init(m_DisConnectCallback, IntPtr.Zero, null); //init NetClient.
                if (!initNETClient)
                {
                    notification.Show("NetClient init failed!", null);
                }
            });
            this.Resize += DahuaInstantPlaybackUserControl_Resize;

            Recorder = recorder;

            this.isDiagnostic = isDiagnostic;
            if (isDiagnostic)
            {
                MAX_MINUTES = 5;
            }

            Camera = camera;
            Profile = profile;
            ClipStatus = false;

            switch (profile)
            {
                case Profile.SubStream:
                    dahuaProfile = (EM_RealPlayType)3;
                    break;
                case Profile.MainStream:
                    dahuaProfile = (EM_RealPlayType)2;
                    break;
                default:
                    dahuaProfile = (EM_RealPlayType)3;
                    break;
            }
            //ChangeProfile(Profile);

            this.Load += DahuaPlaybackUserControl_Load;
            this.Click += DahuaInstantPlaybackUserControl_Click;
            this.picture.Click += DahuaInstantPlaybackUserControl_Click;

            this.DoubleClick += DahuaInstantPlaybackUserControl_DoubleClick;
            this.picture.DoubleClick += DahuaInstantPlaybackUserControl_DoubleClick;

            m_ReConnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack); //instance reconnect callback.
            NETClient.SetAutoReconnect(m_ReConnectCallBack, IntPtr.Zero); //set reconnect callback.
            this.Paint += DahuaPlaybackUserControl_Paint;
            if (hideControls)
            {
                this.PanelControls.Hide();
                this.slider.Hide();
                this.PanelVideo.Dock = DockStyle.Fill;
                this.picture.Size = this.PanelVideo.Size;
                instantPlayback = false;
            }

            this.slider.ValueChanged += Slider_ValueChanged;
            this.slider.ValueChangeComplete += Slider_ValueChangeComplete;

            CameraTime = selectedDateTime;

            if (selectedEndDateTime != null)
            {
                m_PlayBack_StartTime = CameraTime;
                initialTime = CameraTime;
                m_PlayBack_EndTime = (DateTime)selectedEndDateTime;
                this.ActualTime = CameraTime;
                actualTime = CameraTime;
                instantPlayback = false;
            }
            else
            {
                m_PlayBack_StartTime = isDiagnostic ? CameraTime.AddMinutes(-1 * MAX_MINUTES).AddSeconds(-SecondsBefore) : CameraTime.AddMinutes(-1 * MAX_MINUTES).AddMinutes(-1);
                initialTime = isDiagnostic ? CameraTime.AddMinutes(-1 * MAX_MINUTES).AddSeconds(-SecondsBefore) : CameraTime.AddMinutes(-1 * MAX_MINUTES);
                m_PlayBack_EndTime = CameraTime;
                this.ActualTime = CameraTime.AddSeconds(-75);
                actualTime = isDiagnostic ? CameraTime.AddMinutes(-1 * MAX_MINUTES).AddSeconds(-SecondsBefore) : CameraTime.AddMinutes(-1 * MAX_MINUTES);
            }

            ButtonBookmark.Click += ButtonBookmark_Click;
            BookmarkState = false;
            ListenStatus = false;

            CultureInfo ci = CultureInfo.InstalledUICulture;
            bunifuToolTip1.SetToolTip(this.ButtonBookmark, ci.Name.Contains("es") ? ButtonsContextBar.Bookmark.GetDescription() : ButtonsContextBar.Bookmark.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonSnapshot, ci.Name.Contains("es") ? ButtonsContextBar.Snapshot.GetDescription() : ButtonsContextBar.Snapshot.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonFullScreen, ci.Name.Contains("es") ? ButtonsContextBar.Fullscreen.GetDescription() : ButtonsContextBar.Fullscreen.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonPlay, ci.Name.Contains("es") ? ButtonsContextBar.Play.GetDescription() : ButtonsContextBar.Play.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonFwdSecs, ci.Name.Contains("es") ? ButtonsContextBar.FwdSecs.GetDescription() : ButtonsContextBar.FwdSecs.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonRewSecs, ci.Name.Contains("es") ? ButtonsContextBar.RewSecs.GetDescription() : ButtonsContextBar.RewSecs.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonPause, ci.Name.Contains("es") ? ButtonsContextBar.Pause.GetDescription() : ButtonsContextBar.Pause.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonFast, ci.Name.Contains("es") ? ButtonsContextBar.Fast.GetDescription() : ButtonsContextBar.Fast.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonSlow, ci.Name.Contains("es") ? ButtonsContextBar.Slow.GetDescription() : ButtonsContextBar.Slow.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonListen, ci.Name.Contains("es") ? ButtonsContextBar.Listen.GetDescription() : ButtonsContextBar.Listen.GetAttribute<DescriptionEN>().Descripcion);

            ShowButtons();

            panelNoConnection.Visible = this.offLine;
            this.MouseWheel += Picture_MouseWheel;
            ZoomStatus = false;

            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            retryLimit = int.Parse(config.AppSettings.Settings["retryLimit"].Value);
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
            retryCallbackLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
            this.TimeReConnectionCheck = 5;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ZoomStatus)
            {
                Point cursorPosition = Cursor.Position;
                Rectangle controlBounds = new Rectangle(this.PointToScreen(Point.Empty), this.Size);

                if (!controlBounds.Contains(cursorPosition))
                {
                    return false;
                }

                Point mousePosition = Cursor.Position;
                Point relativePosition = this.PointToClient(mousePosition);

                if (keyData == (Keys.Control | Keys.Oemplus) || keyData == (Keys.Control | Keys.Add))
                {
                    ZoomPicture(true, relativePosition.X, relativePosition.Y);
                }

                if (keyData == (Keys.Control | Keys.OemMinus) || keyData == (Keys.Control | Keys.Subtract))
                {
                    ZoomPicture(false, relativePosition.X, relativePosition.Y);
                }

                this.BringToFront();
                this.Focus();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DahuaInstantPlaybackUserControl_Resize(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Length >= 2 && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                Form instantPlayerView = Application.OpenForms.OfType<Form>().FirstOrDefault(f => f.Name == "InstantPlayerView" && (string)f.Tag == Camera.IdGuid);

                if (instantPlayerView != null && instantPlayerView.WindowState == FormWindowState.Maximized)
                {
                    this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.745M), 2));
                    slider.Location = new Point(0, 800);
                    if (main.Width >= 1366 && main.Width < 1400)
                    {
                        this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.705M), 2));
                        slider.Location = new Point(0, Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.705M), 2)));
                    }
                    slider.BringToFront();
                }
                else if (instantPlayerView != null && instantPlayerView.WindowState == FormWindowState.Normal)
                {
                    var panelControlsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.371M), 2));
                    var panelControlsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.047M), 2));
                    this.PanelControls.Size = new Size(panelControlsWidth, panelControlsHeight);

                    var panelVideoWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.371M), 2));
                    var panelVideoHeigth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.325M), 2));
                    this.PanelVideo.Size = new Size(panelVideoWidth, panelVideoHeigth);
                    var sliderWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.371M), 2));
                    var sliderHeigth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.325M), 2));
                    this.PanelVideo.Size = new Size(panelVideoWidth, panelVideoHeigth);

                    var sliderY = 357;
                    if (main.Width > 1700)
                    {
                        sliderY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3305M), 2));
                    }
                    else
                    {
                        sliderY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3194M), 2));
                    }
                    slider.Location = new Point(0, sliderY);

                }

                var btn = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                var btnLocationX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.021M), 2));
                var btnLocation = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014M), 2));

                ButtonPlay.Size = new Size(btn, btn);
                ButtonPlay.Location = new Point(btnLocationX, btnLocation);

                var buttonPauseX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.0843), 2));
                var buttonPauseY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonPause.Size = new Size(btn, btn);
                ButtonPause.Location = new Point(buttonPauseX, buttonPauseY);

                var buttonSlowX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.3090), 2));
                var buttonSlowY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonSlow.Size = new Size(btn, btn);
                ButtonSlow.Location = new Point(buttonSlowX, buttonSlowY);

                var ButtonFastX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.3940), 2));
                var ButtonFastY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonFast.Size = new Size(btn, btn);
                ButtonFast.Location = new Point(ButtonFastX, ButtonFastY);

                var ButtonRewSecsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.4790), 2));
                var ButtonRewSecsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonRewSecs.Size = new Size(btn, btn);
                ButtonRewSecs.Location = new Point(ButtonRewSecsX, ButtonRewSecsY);

                var ButtonFwdSecsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.5399), 2));
                var ButtonFwdSecsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonFwdSecs.Size = new Size(btn, btn);
                ButtonFwdSecs.Location = new Point(ButtonFwdSecsX, ButtonFwdSecsY);

                var ButtonFullScreenX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.705), 2));
                var ButtonFullScreenY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonFullScreen.Size = new Size(btn, btn);
                ButtonFullScreen.Location = new Point(ButtonFullScreenX, ButtonFullScreenY);

                var ButtonBookmarkX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.754), 2));
                var ButtonBookmarkY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonBookmark.Size = new Size(btn, btn);
                ButtonBookmark.Location = new Point(ButtonBookmarkX, ButtonBookmarkY);

                var ButtonZoomX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.803), 2));
                var ButtonZoomY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonZoom.Size = new Size(btn, btn);
                ButtonZoom.Location = new Point(ButtonZoomX, ButtonZoomY);

                var ButtonSnapshotX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.852), 2));
                var ButtonSnapshotY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonSnapshot.Size = new Size(btn, btn);
                ButtonSnapshot.Location = new Point(ButtonSnapshotX, ButtonSnapshotY);

                var ButtonVideoclipX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.901), 2));
                var ButtonVideoclipY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonVideoclip.Size = new Size(btn, btn);
                ButtonVideoclip.Location = new Point(ButtonVideoclipX, ButtonVideoclipY);
                var ButtonListenX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.950), 2));
                var ButtonListenY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonListen.Size = new Size(btn, btn);
                ButtonListen.Location = new Point(ButtonListenX, ButtonListenY);
                /*---------------------------*/

                var labelSpeedWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.016M), 2));
                var labelSpeedHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.017M), 2));

                var LabelSpeedX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.347), 2));
                var LabelSpeedY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.017M), 2));

                LabelSpeed.Size = new Size(labelSpeedWidth, labelSpeedHeight);
                LabelSpeed.Location = new Point(LabelSpeedX, LabelSpeedY);

                if (main.Width <= 1400)
                {
                    this.LabelSpeed.Font = FontHelper.Get(FontSizes.Medium_0, FontName.ROBOTO_MEDIUM);
                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    this.LabelSpeed.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_MEDIUM);
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    this.LabelSpeed.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_MEDIUM);
                }
                else
                {
                    this.LabelSpeed.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_MEDIUM);
                }
            }
        }

        private void ShowButtons()
        {
            this.ButtonSnapshot.Visible = appAuthorization.Exist(ButtonsContextBar.Snapshot.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonBookmark.Visible = appAuthorization.Exist(ButtonsContextBar.Bookmark.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonListen.Visible = (appAuthorization.Exist(ButtonsContextBar.Listen.GetAttribute<PermissionPlayback>().PermissionKey) && Camera.AudioEnabled);
        }

        private void TryToReConnect()
        {
            try
            {
                if (tryCount >= tryLimit)
                {
                    SetVisivility(PlaybackConnectionState.Disconnected);
                    Logger.Log($"Dahua TryToReConnect reached max retry number, then it is  disconnected:  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                    return;
                }

                SetVisivility(PlaybackConnectionState.Reconnecting);
                tryCount++;

                if (PlayBackByTime() == false)
                {
                    int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                    Logger.Log($" TryToReConnect PlayBackByTime failed, current count {tryCount} of {tryLimit}   {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                    Task.Delay(r).ContinueWith(t => TryToReConnect());
                }
                else
                {
                    Logger.Log($" TryToReConnect PlayBackByTime Connected  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Connected);
                    IsPlaying = true;
                    tryCount = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($" TryToReConnect Exception: {ex.Message}  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Fatal);
            }
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

            try
            {
                switch (connectionState)
                {
                    case PlaybackConnectionState.Disconnected:
                        this.panelNoConnection.BringToFront();
                        this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_es);
                        Logger.Log($"setVisivility Disconnected: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.Disconnected} ", LogPriority.Fatal);
                        break;
                    case PlaybackConnectionState.Connected:
                        this.panelNoConnection.SendToBack();
                        picture.BringToFront();
                        Logger.Log($"setVisivility Connected: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.Connected} ", LogPriority.Fatal);
                        break;
                    case PlaybackConnectionState.NoRecording:
                        this.panelNoConnection.BringToFront();
                        this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_es);
                        Logger.Log($"setVisivility NoRecording: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.NoRecording} ", LogPriority.Fatal);
                        break;
                    case PlaybackConnectionState.Reconnecting:
                        this.panelNoConnection.BringToFront();
                        this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_es);
                        Logger.Log($"setVisivility Reconnecting: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.Reconnecting} ", LogPriority.Fatal);
                        break;
                    case PlaybackConnectionState.Connecting:
                        this.panelNoConnection.BringToFront();
                        this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_es);
                        Logger.Log($"setVisivility Connecting: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.Connecting} ", LogPriority.Fatal);
                        break;
                }
                Reconnecting.DisplayLogo(picture.Width, picture.Height, ref panelNoConnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"setVisivility Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        private void DahuaInstantPlaybackUserControl_Click(object sender, EventArgs e)
        {
            if (CameraSelected != null)
            {
                CameraSelected(this, Camera);
            }

            if (ZoomStatus)
            {
                if (picture.Location.X < 0)
                {
                    var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);
                    var mp = new Point((100 * mouse.X) / this.Width, (100 * mouse.Y) / this.Height);
                    var p = new Point((picture.Width * mp.X) / 100, (picture.Height * mp.Y) / 100);
                    var picPosition = new Point((mouse.X - p.X), (mouse.Y - p.Y));
                    picture.Location = picPosition;
                }

                this.BringToFront();
                this.Focus();
            }
        }

        private void DahuaInstantPlaybackUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick(this);

                if (ZoomStatus)
                {
                    if (picture.Location.X < 0)
                    {
                        var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);
                        var mp = new Point((100 * mouse.X) / this.Width, (100 * mouse.Y) / this.Height);
                        var p = new Point((picture.Width * mp.X) / 100, (picture.Height * mp.Y) / 100);
                        var picPosition = new Point((mouse.X - p.X), (mouse.Y - p.Y));
                        picture.Location = picPosition;
                    }
                }
            }
        }

        private void ButtonBookmark_Click(object sender, EventArgs e)
        {
            BookmarkState = !BookmarkState;

            this.ActualTime = actualTime;
            OpenBookmark?.Invoke(this, BookmarkState);
        }

        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            var datetime = initialTime.AddSeconds(slider.Value);
            SliderTooltip.Text = this.actualTime.AddSeconds(slider.Value + _additionalSeconds).ToString("MM/dd HH:mm:ss");
            SliderTooltip.Location = new Point(slider.Location.X, slider.Location.Y - 13);
            SliderTooltip.Visible = true;

            SliderValueChangeComplete();
        }

        private void Slider_ValueChangeComplete(object sender, EventArgs e)
        {
            SliderValueChangeComplete();
        }

        private void SliderValueChangeComplete()
        {
            try
            {
                int sliderValueTemp = slider.Value;
                if (!_scaleActive)
                {
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
                }

                _sliderCurrentValue = (int)slider.Value;

                if (_paused)
                {
                    IsPlaying = true;
                }

                SliderTooltip.Visible = false;
                m_PlayBack_StartTime = initialTime.AddSeconds(sliderValueTemp);
                LabelSpeed.Text = "1X";
                CurrentSpeed = PlaySpeed.NORMAL;
                ButtonFast.Enabled = true;
                ButtonSlow.Enabled = true;
                this.IsPlaying = OpenCamera(GetHandle());
                _paused = false;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("SliderValueChangeComplete Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
        }

        private void DahuaPlaybackUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (_painted)
            {
                return;
            }

            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            ButtonRewSecs.Image = FileResources.icon_replay_30;
            ButtonFwdSecs.Image = FileResources.icon_forward_30;
            ButtonBookmark.Image = FileResources.icon_bookmarks;
            ButtonSnapshot.Image = FileResources.icon_snapshot;
            ButtonFullScreen.Image = FileResources.icon_full_screen;
            ButtonVideoclip.Image = FileResources.icon_recorder;
            ButtonListen.Image = FileResources.icon_sound_off;
            ButtonFast.Image = FileResources.icon_fast;
            ButtonZoom.Image = FileResources.icon_digital_zoom_off;
            ButtonSlow.Image = FileResources.icon_slow;
            LabelSpeed.Text = "1X";
            LabelSpeed.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);

            _painted = true;
            var tSecs = (m_PlayBack_EndTime - m_PlayBack_StartTime).TotalSeconds;
            slider.MaximumValue = (int)tSecs;
            slider.Enabled = true;
            this.picture.Size = this.PanelVideo.Size;
            this.Resize += DahuaPlaybackResize;
        }

        private void DahuaPlaybackResize(object sender, EventArgs e)
        {
            Reconnecting.DisplayLogo(this.Width, this.Height, ref panelNoConnection, ref panelFondoLogo);
        }

        public new void Dispose()
        {
            this.CustomDispose();
        }

        private void CustomDispose(bool removeControls = true)
        {
            if (loginControl.RemoveChannelAndCanLogout(Camera, this, Common.Enum.Drivers.NETSDK_352))
            {
                Logger.Log(String.Format(" Dispose Logout Dahua {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                if (loginHandle != IntPtr.Zero)
                {
                    NETClient.Logout(loginHandle);
                    loginHandle = IntPtr.Zero;
                }
            }
            if (playbackHandle != IntPtr.Zero)
            {
                NETClient.CloseSound();
                NETClient.StopSaveRealData(playbackHandle);
                NETClient.StopRealPlay(playbackHandle);
                playbackHandle = IntPtr.Zero;
                NETClient.Cleanup();
            }
            this.MouseWheel -= Picture_MouseWheel;
            if (removeControls)
            {
                this.Controls.Clear();
            }

            m_DispatcherTimer.Tick -= DispatcherTimer_Tick;
        }

        private void DahuaPlaybackUserControl_Load(object sender, EventArgs e)
        {

            //         m_DisConnectHandle = new fDisConnectCallBack(DisConnectCallBack);
            //m_TimeDownLoadPosHandle = new fTimeDownLoadPosCallBack(TimeDownLoadPosCallBack);
            m_DownLoadPosHandle = new fDownLoadPosCallBack(DownLoadPosCallBack);
            //m_SnapRevCallBack = new fSnapRevCallBack(SnapRevCallBack);
            //           NETClient.Init(m_DisConnectHandle, IntPtr.Zero, null);
            DispatcherTimerInit();
        }

        private void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            Logger.Log(string.Format(" ----> ReConnectCallBack Playback {0} ", Camera.Name), LogPriority.Information);
            if (offLine == true)
            {//if 
                offLine = false;
                retryCallbackCount = 0;
                SetVisivility(PlaybackConnectionState.Connected);
                //  loginControl.Connect(lLoginID, this.realHandle);
                //setPanelNoConnectionVisibility();
            }
        }

        private void DispatcherTimerInit()
        {
            m_DispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
            m_DispatcherTimer.Tick += DispatcherTimer_Tick;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_paused)
                {
                    return;
                }

                var t = Task.Factory.StartNew(() =>
                {
                    bool b = NETClient.GetPlayBackOsdTime(playbackHandle, ref m_OsdTime, ref m_OsdStartTime, ref m_OsdEndTime);
                    if (m_OsdTime.ToString() != "0000-00-00 00:00:00" && m_OsdTime.ToDateTime().Date == m_PlayBack_StartTime.Date)
                    {
                        if (m_OsdTime.ToDateTime() >= m_PlayBack_EndTime)//Current Time > End Time
                        {
                            m_OsdTime = NET_TIME.FromDateTime(m_PlayBack_EndTime);
                            Stop();
                            return;
                        }
                        actualTime = m_OsdTime.ToDateTime();
                        OnTimeChanged?.Invoke(actualTime, this);

                        var sliderValue = (int)(actualTime - initialTime).TotalSeconds;
                        if (sliderValue <= slider.MaximumValue)
                        {
                            slider.Value = sliderValue;
                        }
                    }
                }).ContinueWith(x => x.Dispose());
            }
            catch { }
        }

        private void DisconnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            try
            {
                if (offLine == false)
                {
                    offLine = true;
                    if (retryCallbackCount < retryCallbackLimit)
                    {
                        retryCallbackCount++;
                        Logger.Log(String.Format("DisconnectCallBack current {4} of {5}  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, retryCallbackCount, retryCallbackLimit), LogPriority.Information);
                        SetVisivility(PlaybackConnectionState.Reconnecting);
                    }
                    else
                    {
                        Logger.Log(String.Format("DisconnectCallBack reached maximum retry value {4} , {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, retryCallbackLimit), LogPriority.Information);
                        SetVisivility(PlaybackConnectionState.Disconnected);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Disconnect Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
        }

        private bool ChangeProfile(Profile profile)
        {
            switch (profile)
            {
                case Profile.SubStream:
                    dahuaProfile = (EM_RealPlayType)3;
                    break;
                case Profile.MainStream:
                    dahuaProfile = (EM_RealPlayType)2;
                    break;
                default:
                    dahuaProfile = (EM_RealPlayType)3;
                    break;
            }
            if (Profile != profile && playbackHandle != IntPtr.Zero)
            {
                Stop();
                Profile = profile;
                return Play();
            }
            return true;
        }

        public CameraDTO Camera { get; set; }

        public List<ButtonsContextBar> Commands => GetButtons();

        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();

        private List<ButtonsContextBar> GetButtons()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>
            {
                ButtonsContextBar.Fullscreen,
                ButtonsContextBar.Snapshot,
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

        public Profile Profile { get; set; }

        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private bool _listen = false;
        private bool _paused = false;
        private bool _painted = false;
        public IntPtr prevHandle;
        public int m_PlayBack_StreamType { get; set; }
        public EM_RECORD_TYPE m_PlayBack_recordType { get; set; }

        public event OnVideoEventHandler OnVideo;
        public event OnTimeChangedEventHandler OnTimeChanged;
        public event OnStateChangedEventHandler OnStateChanged;

        public event EventHandler<bool> OpenBookmark;
        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;

        public bool SetStartDateTime(DateTime dateTime, bool changeSlider = true, bool isVault = false)
        {
            IsPlaying = false;
            m_PlayBack_StartTime = dateTime;
            StartTime = dateTime;
            initialTime = dateTime;
            actualTime = dateTime;
            _paused = true;
            //var date = DateTime.Now - dateTime;
            //if (isDiagnostic)
            //    m_PlayBack_EndTime = date.TotalMinutes > MAX_MINUTES ? dateTime.AddMinutes(MAX_MINUTES).AddSeconds(SecondsBefore) : DateTime.Now.AddMinutes(-2).AddSeconds(-SecondsBefore);
            //else
            //m_PlayBack_EndTime = DateTime.Now.AddHours(MAX_HOUR_END); //date.TotalMinutes > MAX_MINUTES ? dateTime.AddMinutes(MAX_MINUTES) : DateTime.Now.AddMinutes(-2);
            m_PlayBack_EndTime = m_PlayBack_StartTime.AddDays(1);
            EndTime = m_PlayBack_EndTime;

            var tSecs = (m_PlayBack_EndTime - m_PlayBack_StartTime).TotalSeconds;
            slider.MaximumValue = (int)tSecs;

            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            return true;
        }

        public bool Pause()
        {
            if (!_paused)
            {
                CapacityNotAvailable(false);
                this._paused = !this._paused;
                NETClient.PlayBackControl(playbackHandle, PlayBackType.Pause);
                if (_paused)
                {
                    ButtonPause.Image = FileResources.icon_pause;
                    ButtonPlay.Image = FileResources.icon_play;
                    m_DispatcherTimer.Stop();
                }
            }
            return this._paused;
        }

        bool firstTime = true;
        public bool Play()
        {
            if (IsPlaying)
            {
                return true;
            }

            var t = Task.Factory.StartNew(() =>
            {
                try
                {
                    if (Login())
                    {
                        if (instantPlayback && firstTime)
                        {
                            m_PlayBack_StartTime = isDiagnostic ? m_PlayBack_EndTime.AddMinutes(-5).AddSeconds(-SecondsBefore) : m_PlayBack_EndTime.AddMinutes(-1);
                            slider.Value = (int)(m_PlayBack_EndTime - m_PlayBack_StartTime).TotalSeconds;
                            firstTime = false;
                        }
                        Logger.Log(String.Format(" Play Dahua 352 entered  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);

                        //CapacityNotAvailable(false);
                        SetVisivility(PlaybackConnectionState.Connecting);

                        ButtonPlay.Image = FileResources.icon_play;
                        ButtonPause.Image = FileResources.icon_pause;
                        if (_paused)
                        {
                            IsPlaying = true;
                            _paused = !_paused;
                        }
                        IsPlaying = OpenCamera(GetHandle());
                        if (IsPlaying)
                        {
                            updateToSelectedSpeed();
                            Logger.Log(String.Format("Play Dahua Connected  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                            SetVisivility(PlaybackConnectionState.Connected);
                        }
                    }
                    else
                    {
                        IsPlaying = false;
                        if (retryCount <= retryLimit)
                        {
                            SetVisivility(PlaybackConnectionState.Reconnecting);
                            Threads.RunInOtherThread(new Action[] { () => System.Threading.Thread.Sleep(2000 * retryCount) }, () => Play());
                            Logger.Log(String.Format("Dahua Play() Error to Camera login current {4} of {5}:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, retryCount, retryLimit), LogPriority.Information);
                            retryCount++;
                        }
                        else
                        {
                            SetVisivility(PlaybackConnectionState.Disconnected);
                            Logger.Log(String.Format("Dahua Play() reached max retry number, then it is  disconnected: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                        }
                    }

                }
                catch (Exception ex)
                {
                    Logger.Log(String.Format("Play Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                    notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                }
            }).ContinueWith(x => x.Dispose());

            return IsPlaying;
        }

        private bool PlayBackByTime()
        {
            NET_IN_PLAY_BACK_BY_TIME_INFO stuInfo = new NET_IN_PLAY_BACK_BY_TIME_INFO();
            NET_OUT_PLAY_BACK_BY_TIME_INFO stuOut = new NET_OUT_PLAY_BACK_BY_TIME_INFO();
            stuInfo.stStartTime = NET_TIME.FromDateTime(m_PlayBack_StartTime);
            stuInfo.stStopTime = NET_TIME.FromDateTime(m_PlayBack_EndTime);
            StartTime = m_PlayBack_StartTime;
            EndTime = m_PlayBack_EndTime;
            stuInfo.hWnd = prevHandle;
            stuInfo.cbDownLoadPos = m_DownLoadPosHandle;
            stuInfo.dwPosUser = IntPtr.Zero;
            stuInfo.fDownLoadDataCallBack = null;
            stuInfo.dwDataUser = IntPtr.Zero;
            stuInfo.nPlayDirection = 0;
            stuInfo.nWaittime = 15000;
            var playbackRetry = PlaybackRetry.StartRetry;
            retry:
            playbackHandle = NETClient.PlayBackByTime(loginHandle, Camera.Channel - 1, stuInfo, ref stuOut);


            Logger.Log("Device PlayBackByTime is Zero " + NETClient.GetLastError(), LogPriority.Information);
            switch (playbackRetry)
            {
                case PlaybackRetry.StartRetry:
                    if (playbackHandle == IntPtr.Zero && NETClient.GetLastError() == "There is no searched result")
                    {
                        playbackRetry = PlaybackRetry.ProcessRetry;
                        NET_RECORDFILE_INFO[] m_stFileInfo = new NET_RECORDFILE_INFO[100];
                        for (int i = 0; i < m_stFileInfo.Length; i++)
                        {
                            m_stFileInfo[i] = new NET_RECORDFILE_INFO();
                        }
                        int nFileCount = 0;
                        var resul = NETClient.QueryRecordFile(loginHandle, Camera.Channel - 1, 0, Convert.ToDateTime(m_PlayBack_StartTime.ToString("yyyy-MM-dd")), Convert.ToDateTime(m_PlayBack_EndTime.ToString("yyyy-MM-dd HH:mm:ss")), "", ref m_stFileInfo, ref nFileCount, 15000, true);
                        if (resul)
                        {
                            var filesInfo = m_stFileInfo.Where(x => x.filename != null).ToList();
                            if (filesInfo != null && filesInfo.Count > 0)
                            {
                                var fileInfo = filesInfo[filesInfo.Count() - 1];
                                stuInfo.stStartTime = fileInfo.starttime;
                                goto retry;
                            }
                        }
                    }
                    break;
                case PlaybackRetry.ProcessRetry:
                    playbackRetry = PlaybackRetry.EndRetry;
                    if (playbackHandle != IntPtr.Zero)
                    {
                        IsPlaying = true;
                        Stop();
                        //stuInfo.hWnd = GetHandle();
                        stuInfo.stStartTime = NET_TIME.FromDateTime(m_PlayBack_StartTime);
                        goto retry;
                    }
                    break;
            }


            return playbackHandle != IntPtr.Zero;
        }

        private bool OpenCamera(IntPtr? handle)
        {
            if (handle == IntPtr.Zero)
            {
                return false;
            }

            if (IntPtr.Zero == loginHandle)
            {
                return false;
            }

            //stop play
            if (IntPtr.Zero != playbackHandle)
            {
                Stop();
            }

            SetDeviceModel(m_PlayBack_StreamType, m_PlayBack_recordType);//Set stream type, and record type

            //start play
            IntPtr pStream = IntPtr.Zero;
            IntPtr pRecordType = IntPtr.Zero;
            prevHandle = handle ?? prevHandle;
            try
            {
                //package playback param
                PlayBackByTime();
            }
            catch (NETClientExcetion nex)
            {
                notification.Show(nex.Message, null);
                return false;
            }
            catch (Exception ex)
            {
                notification.Show(ex.Message, null);
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(pStream);
                Marshal.FreeHGlobal(pRecordType);
            }

            if (IntPtr.Zero != playbackHandle)
            {
                SetVisivility(PlaybackConnectionState.Connected);
                OnVideo?.Invoke(true, this);
                m_DispatcherTimer.Start();
                return true;
            }
            else
            {
                if (NETClient.GetLastError() == "There is no searched result")
                {
                    var message = string.Format("{0} - {1}", Camera.Name, m_PlayBack_StartTime.ToString("yyyy/MM/dd HH:mm:ss"));
                    notification.Show(string.Format(Resources.NoRecordingAvailable, message), null);
                    Logger.Log(String.Format("Dahua no recording available {0} {1} {2} {3} StartTime {4} EndTime {5} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, m_PlayBack_StartTime, m_PlayBack_EndTime), LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.NoRecording);
                }
                else
                {
                    tryCount = 0;
                    TryToReConnect();
                }
            }

            return false;
        }

        public void SetDeviceModel(int nStreamType, EM_RECORD_TYPE emRecordType)
        {
            //start play
            IntPtr pStream = IntPtr.Zero;
            IntPtr pRecordType = IntPtr.Zero;
            try
            {
                //set streamType
                pStream = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
                Marshal.StructureToPtr(nStreamType, pStream, true);
                NETClient.SetDeviceMode(loginHandle, EM_USEDEV_MODE.RECORD_STREAM_TYPE, pStream);

                //set recordType
                int nRecordType = (int)emRecordType;
                pRecordType = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
                Marshal.StructureToPtr(nRecordType, pRecordType, true);
                NETClient.SetDeviceMode(loginHandle, EM_USEDEV_MODE.RECORD_TYPE, pRecordType);
            }
            catch (NETClientExcetion nex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, nex.Message), null);
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
            finally
            {
                Marshal.FreeHGlobal(pStream);
                Marshal.FreeHGlobal(pRecordType);
            }
        }

        private IntPtr GetHandle()
        {
            try
            {
                return picture.Handle;
            }
            catch (ObjectDisposedException)
            {
                return IntPtr.Zero;
            }

        }

        private bool Login()
        {

            if (loginControl.GetDeviceLogin(Camera, out loginHandle, Common.Enum.Drivers.NETSDK_352, this))
            {
                loginControl.AddChannel(Camera, this, Common.Enum.Drivers.NETSDK_352);
                Logger.Log(String.Format("Reusing a Dahua Login to Camara {0}", Camera.Name), LogPriority.Information);
                return true;
            }

            NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
            try
            {
                //call login function.
                ushort dev_port = (ushort)Camera.VideoPort;

                loginHandle = NETClient.Login(Camera.Host, dev_port, Camera.User, Camera.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);
                if (loginHandle == IntPtr.Zero)
                {
                    Logger.Log("Device Login is Zero " + NETClient.GetLastError(), LogPriority.Information);
                }
                else
                {
                    Logger.Log(String.Format("New Dahua Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                    loginControl.AddDevice(Camera, loginHandle, this, Common.Enum.Drivers.NETSDK_352);
                }
                return loginHandle != IntPtr.Zero;
            }
            catch (Exception ex)
            {
                if (ex is NETClientExcetion)
                {
                    notification.Show(string.Format("{0} - {1}", Camera.Name, (ex as NETClientExcetion).Message), null);
                }
                else
                {
                    notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                }
                Logger.Log(String.Format("Login Excepotion: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                return false;
            }
        }

        public bool Snapshot(string path)
        {
            filePathSnap = path;
            try
            {
                NETClient.CapturePicture(playbackHandle, path, EM_NET_CAPTURE_FORMATS.JPEG);
                return true;
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                return false;
            }
        }

        public bool Stop()
        {
            tryCount = tryLimit;
            if (!IsPlaying && !_paused)
            {
                return true;
            }

            bool ret = NETClient.PlayBackControl(playbackHandle, PlayBackType.Stop);
            playbackHandle = IntPtr.Zero;
            IsPlaying = false;
            return ret;
        }

        public void ToggleFullScreen()
        {
            try
            {
                var tempProfile = this.Profile;
                DahuaFullscreen fullscreen = new DahuaFullscreen();
                Task.Run(() =>
                {
                    Stop();
                    Profile = Profile.MainStream;
                    dahuaProfile = (EM_RealPlayType)2;
                    this.IsPlaying = OpenCamera(fullscreen.pHandle);
                });
                fullscreen.ShowDialog();

                Profile = tempProfile;
                switch (tempProfile)
                {
                    case Profile.SubStream:
                        dahuaProfile = (EM_RealPlayType)3;
                        break;
                    case Profile.MainStream:
                        dahuaProfile = (EM_RealPlayType)2;
                        break;
                    default:
                        dahuaProfile = (EM_RealPlayType)3;
                        break;
                }
                this.IsPlaying = OpenCamera(GetHandle());
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("ToggleFullScreen Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
        }

        public bool ToggleListen()
        {
            try
            {
                _listen = !_listen;

                if (_listen)
                {
                    NETClient.OpenSound(playbackHandle); //call opensound function.
                }
                else
                {
                    NETClient.CloseSound();
                }

                ListenStatus = _listen;
                return _listen;
            }
            catch (Exception ex)
            {
                notification.Show(ex.Message, null);
                ListenStatus = false;
                return false;
            }
        }

        public bool VideoClipStart(string path)
        {
            try
            {
                path = path.Replace(".mp4", ".dav");
                bool ret = NETClient.SaveRealData(playbackHandle, path);
                if (ret)
                {
                    m_SaveDataHandleList.Add(playbackHandle);
                }
                ClipStatus = !ret;
                return ret;
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                ClipStatus = true;
                return false;
            }
        }

        public bool VideoClipStop()
        {
            try
            {
                bool ret = NETClient.StopSaveRealData(playbackHandle);
                if (ret)
                {
                    m_SaveDataHandleList.Remove(playbackHandle);
                }
                //ClipStatus = false;
                return ret;
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                ClipStatus = true;
                return false;
            }
        }

        public bool Volume(int value)
        {
            throw new NotImplementedException();
        }

        // Driver Functionality
        //
        private void DisConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
        }

        private void TimeDownLoadPosCallBack(IntPtr lPlayHandle, uint dwTotalSize, uint dwDownLoadSize, int index, NET_RECORDFILE_INFO recordfileinfo, IntPtr dwUser)
        {
        }

        private void DownLoadPosCallBack(IntPtr lPlayHandle, uint dwTotalSize, uint dwDownLoadSize, IntPtr dwUser)
        {

        }

        private void SnapRevCallBack(IntPtr lLoginID, IntPtr pBuf, uint RevLen, uint EncodeType, uint CmdSerial, IntPtr dwUser)
        {
            if (EncodeType == 10) //.jpg
            {
                byte[] data = new byte[RevLen];
                Marshal.Copy(pBuf, data, 0, (int)RevLen);
                using (FileStream stream = new FileStream(filePathSnap, FileMode.OpenOrCreate))
                {
                    stream.Write(data, 0, (int)RevLen);
                    stream.Flush();
                    stream.Dispose();
                }
            }
            notification.Show(Resources.SnapshotSaved, () => Process.Start(Settings.Default["DefaultLocation"].ToString() + "\\Snapshot"));
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            PlayVideo();
        }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            IsPlaying = false;
            m_DispatcherTimer.Stop();
            Pause();
        }
        private bool updateToSelectedSpeed()
        {
            PlaySpeed temp = PlaySpeed.NORMAL;
            bool result = true;
            while (result && temp != CurrentSpeed)
            {
                if (CurrentSpeed >= PlaySpeed.MIN && CurrentSpeed < PlaySpeed.NORMAL)
                {
                    result = NETClient.PlayBackControl(playbackHandle, PlayBackType.Slow);
                    temp = (PlaySpeed)((int)temp - 1);
                }
                else
                {
                    result = NETClient.PlayBackControl(playbackHandle, PlayBackType.Fast);
                    temp = (PlaySpeed)((int)temp + 1);
                }
            }

            return result;
        }
        public bool Jump(int sec, bool asc)
        {
            m_DispatcherTimer.Stop();
            Stop();
            IsPlaying = false;
            actualTime = actualTime.AddSeconds(asc == true ? sec : sec * -1);
            m_PlayBack_StartTime = actualTime;
            CapacityNotAvailable(false);
            IsPlaying = OpenCamera(GetHandle());
            if (IsPlaying)
            {
                updateToSelectedSpeed();
            }

            _paused = false;
            slider.Enabled = IsPlaying;
            return true;
        }

        private async void ButtonRewSecs_Click(object sender, EventArgs e)
        {
            if (_isBusyRew)
                return;

            _isBusyRew = true;
            ButtonRewSecs.Enabled = false;

            try
            {
                await Task.Run(() => Jump(30, false));
            }
            finally
            {

                await Task.Delay(500);

                _isBusyRew = false;
                ButtonRewSecs.Enabled = true;
            }
        }

        private async void ButtonFwdSecs_Click(object sender, EventArgs e)
        {

            if (_isBusyFwd)
                return;

            _isBusyFwd = true;
            ButtonFwdSecs.Enabled = false;

            try
            {
                await Task.Run(() => Jump(30, true));
            }
            finally
            {

                await Task.Delay(500);

                _isBusyFwd = false;
                ButtonFwdSecs.Enabled = true;
            }
        }

        private void ButtonSlow_Click(object sender, EventArgs e)
        {
            Slow();
        }

        public bool Slow()
        {
            bool result = false;
            if (IntPtr.Zero == playbackHandle)
            {
                return false;
            }
            try
            {
                result = NETClient.PlayBackControl(playbackHandle, PlayBackType.Slow);
            }
            catch (NETClientExcetion nex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, nex.Message), null);
                Logger.Log(string.Format("Slow {0} - {1}", Camera.Name, nex.Message), LogPriority.Fatal);
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                Logger.Log(string.Format("Slow {0} - {1}", Camera.Name, ex.Message), LogPriority.Fatal);
            }
            if (result)
            {
                ShowPlaySpeed(-1);
            }
            return result;
        }

        public bool SyncSpeed(PlaySpeed masterSpeed, bool updateLabelSpeed)
        {

            bool result = true;
            try
            {
                if (this.CurrentSpeed == masterSpeed)
                {
                    return result;
                }
                if (IntPtr.Zero == playbackHandle)
                {
                    return false;
                }
                if (masterSpeed < CurrentSpeed)
                {
                    while (CurrentSpeed != masterSpeed && result)
                    {
                        result = NETClient.PlayBackControl(playbackHandle, PlayBackType.Slow);
                        //if (result)
                        CurrentSpeed = (PlaySpeed)((int)CurrentSpeed - 1);
                    }
                }
                else
                {
                    while (CurrentSpeed != masterSpeed && result)
                    {
                        result = NETClient.PlayBackControl(playbackHandle, PlayBackType.Fast);
                        //if (result)
                        CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
                    }

                }
            }
            catch (NETClientExcetion nex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, nex.Message), null);
                Logger.Log(string.Format("SyncSpeed {0} - {1}", Camera.Name, nex.Message), LogPriority.Fatal);
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                Logger.Log(string.Format("SyncSpeed {0} - {1}", Camera.Name, ex.Message), LogPriority.Fatal);
            }
            if (updateLabelSpeed == true)
            {//actualizo el label de velocidad
                UpdateControlsToShowSpeed();
            }
            return result;
        }

        private void ShowPlaySpeed(int nMode)
        {
            if (nMode == 0)
            {
                CurrentSpeed = PlaySpeed.NORMAL;
            }
            else if (nMode > 0)	// Speed up
            {
                if (CurrentSpeed >= PlaySpeed.MIN && CurrentSpeed < PlaySpeed.MAX)
                {
                    CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
                }
            }
            else if (nMode < 0)	// Speed down
            {
                if (CurrentSpeed > PlaySpeed.MIN && CurrentSpeed <= PlaySpeed.MAX)
                {
                    CurrentSpeed = (PlaySpeed)((int)CurrentSpeed - 1);
                }
            }
            UpdateControlsToShowSpeed();
        }

        private void UpdateControlsToShowSpeed()
        {
            switch (CurrentSpeed)
            {

                case PlaySpeed.DOWN_16:
                    LabelSpeed.Text = "1/16";
                    break;
                case PlaySpeed.DOWN_8:
                    LabelSpeed.Text = "1/8X";
                    break;
                case PlaySpeed.DOWN_4:
                    LabelSpeed.Text = "1/4X";
                    break;
                case PlaySpeed.DOWN_2:
                    LabelSpeed.Text = "1/2X";
                    break;
                case PlaySpeed.NORMAL:
                    LabelSpeed.Text = "1X";
                    break;
                case PlaySpeed.UP_2:
                    LabelSpeed.Text = "2X";
                    break;
                case PlaySpeed.UP_4:
                    LabelSpeed.Text = "4X";
                    break;
                case PlaySpeed.UP_8:
                    LabelSpeed.Text = "8X";
                    break;
                case PlaySpeed.UP_16:
                    LabelSpeed.Text = "16X";
                    break;
                default:
                    LabelSpeed.Text = "";
                    break;
            }

            if (PlaySpeed.MAX == CurrentSpeed)
            {
                ButtonFast.Enabled = false;
                ButtonSlow.Enabled = true;
            }
            else if (PlaySpeed.MIN == CurrentSpeed)
            {
                ButtonFast.Enabled = true;
                ButtonSlow.Enabled = false;
            }
            else
            {
                ButtonFast.Enabled = true;
                ButtonSlow.Enabled = true;
            }
        }

        private void ButtonFast_Click(object sender, EventArgs e)
        {
            Fast();
        }

        public bool Fast()
        {
            bool result = false;
            if (IntPtr.Zero == playbackHandle)
            {
                return false;
            }
            try
            {
                result = NETClient.PlayBackControl(playbackHandle, PlayBackType.Fast);
            }
            catch (NETClientExcetion nex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, nex.Message), null);
                Logger.Log(string.Format("Fast {0} - {1}", Camera.Name, nex.Message), LogPriority.Fatal);
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                Logger.Log(string.Format("Fast {0} - {1}", Camera.Name, ex.Message), LogPriority.Fatal);
            }
            if (result)
            {
                ShowPlaySpeed(1);
            }
            return result;
        }

        public bool SetStartUpSpeed(int speed)
        {
            bool result = false;
            if (speed == 0)
            {
                return true;
            }

            if (IntPtr.Zero == playbackHandle)
            {
                return false;
            }
            try
            {
                int variance = speed > 0 ? -1 : 1;
                while (speed != 0)
                {
                    result = NETClient.PlayBackControl(playbackHandle, PlayBackType.Fast);
                    speed += variance;
                    CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + (variance * -1));
                }
                UpdateControlsToShowSpeed();
            }
            catch (NETClientExcetion nex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, nex.Message), null);
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
            return result;
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

        private void ButtonVideoclip_Click(object sender, EventArgs e)
        {
            if (!ClipStatus)
            {
                VideoClipStart(PathUtils.CreatePathAndFileName("Videoclip", "mp4"));
                ButtonVideoclip.Image = FileResources.icon_recorder_on;
            }
            else
            {
                var result = VideoClipStop();
                if (result)
                {
                    notification.Show(Resources.VideoclipSaved, () => Process.Start(Settings.Default["DefaultLocation"].ToString() + "\\Videoclip"));
                }

                ButtonVideoclip.Image = FileResources.icon_recorder;
            }
            ClipStatus = !ClipStatus;
        }
        public bool Rewind()
        {
            if (!_paused)
            {
                Pause();
            }

            CapacityNotAvailable(true);
            return true;
        }

        public bool CapacityNotAvailable(bool show)
        {
            this.LabelCapacityNotAvailable.Text = Resources.CapacityNotAvailable;
            this.LabelCapacityNotAvailable.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            this.LabelCapacityNotAvailable.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
            this.LabelCapacityNotAvailable.Left = (this.LabelCapacityNotAvailable.Parent.Width - this.LabelCapacityNotAvailable.Width) / 2;
            this.LabelCapacityNotAvailable.Top = (this.LabelCapacityNotAvailable.Parent.Height - this.LabelCapacityNotAvailable.Height) / 2;

            this.LabelCapacityNotAvailable.Visible = show;
            return true;
        }

        public bool ZoomStatus { get; set; }
        private int _actualSize = 0;
        private int _zoomLimit = int.Parse(Settings.Default["ZoomLimit"].ToString());

        public bool ToogleDigitalZoom()
        {
            ButtonZoom.Image = FileResources.icon_digital_zoom_on;

            if (ZoomStatus)
            {
                picture.Size = picture.Parent.Size;
                picture.Location = new Point(0, 0);
                picture.Visible = true;
                ButtonZoom.Image = FileResources.icon_digital_zoom_off;
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.Cross;
                //this.BringToFront();
                this.Focus();
            }

            ZoomStatus = !ZoomStatus;
            return ZoomStatus;
        }

        private void Picture_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!ZoomStatus)
            {
                return;
            }

            var direction = e.Delta > 0;
            ZoomPicture(direction, e.X, e.Y);
        }

        private void ZoomPicture(bool direction, int eventPositionX, int eventPositionY)
        {
            Rectangle main = Screen.PrimaryScreen.Bounds;

            Size picSize;
            if (direction)
            {
                picSize = new Size((int)(picture.Width * 1.1), (int)(picture.Height * 1.1));
            }
            else
            {
                picSize = new Size((int)(picture.Width * 0.9), (int)(picture.Height * 0.9));
                if (picSize.Width < picture.Parent.Size.Width)
                {
                    picSize = picture.Parent.Size;
                }
            }

            var mp = new Point((100 * eventPositionX) / this.Width, (100 * eventPositionY) / this.Height);
            var p = new Point((picSize.Width * mp.X) / 100, (picSize.Height * mp.Y) / 100);
            var picPosition = new Point((eventPositionX - p.X), (eventPositionY - p.Y));
            if (picSize == picture.Parent.Size)
            {
                picPosition = new Point(0, 0);
            }

            if (main.Width >= 2000)
            {
                this._zoomLimit = 15;
            }

            if ((this._actualSize == 0 && !direction) || (this._actualSize > this._zoomLimit && direction))
            {
                return;
            }

            PictureBox temp = new PictureBox
            {
                Image = picture.Image,
                Size = picture.Size,
                Location = picture.Location,
                Visible = true
            };

            this.Controls.Add(temp);
            temp.BringToFront();
            //picture.Visible = false;

            this._actualSize = direction ? this._actualSize + 1 : this._actualSize - 1;

            picture.Size = picSize;
            picture.Location = picPosition;
            picture.Visible = true;
            this.Controls.Remove(temp);
        }

        private void ButtonZoom_Click(object sender, EventArgs e)
        {
            ToogleDigitalZoom();
        }

        public PlaySpeed GetCurrentSpeed()
        {
            return this.CurrentSpeed;
        }

        public List<ButtonsPlayBackBar> ButtonsNotAllowed()
        {
            return new List<ButtonsPlayBackBar>();
        }

        public void Disconect(IntPtr HandledBrocaster)
        {

        }

        public void Connect(IntPtr HandledBrocaster)
        {

        }

        public int Hash()
        {
            //return string.Format("{0}-{1}-{2}", ElementId, RecorderType, RecorderId).GetHashCode();
            return string.Format("{0}-{1}-{2}", Camera.Id, Recorder.RecorderType, Recorder.Id).GetHashCode();
        }

        public bool PlayVideo()
        {
            if (_paused)
            {
                CapacityNotAvailable(false);
                this._paused = !this._paused;
                IsPlaying = true;
                NETClient.PlayBackControl(playbackHandle, PlayBackType.Play);

                ButtonPlay.Image = FileResources.icon_play;
                ButtonPause.Image = FileResources.icon_pause;
                m_DispatcherTimer.Start();
            }
            else if (!IsPlaying)
            {
                Play();
            }
            return true;
        }

        public bool PlayNoAsync()
        {
            if (_paused)
            {
                IsPlaying = true;
            }

            CapacityNotAvailable(false);

            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            if (_paused)
            {
                IsPlaying = true;
                _paused = !_paused;
            }
            IsPlaying = OpenCamera(GetHandle());
            if (IsPlaying)
            {
                updateToSelectedSpeed();
                Logger.Log(String.Format("Play Dahua Connected  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                SetVisivility(PlaybackConnectionState.Connected);
            }
            return IsPlaying;
        }

        public void SelectSpeed(PlaySpeed speed)
        {
            PlaySpeed temp = CurrentSpeed;
            bool result = true;
            while (result && temp != speed)
            {
                if (temp >= PlaySpeed.MIN && temp < PlaySpeed.NORMAL)
                {
                    result = NETClient.PlayBackControl(playbackHandle, PlayBackType.Fast);
                    temp = (PlaySpeed)((int)temp + 1);
                }
                else
                {
                    result = NETClient.PlayBackControl(playbackHandle, PlayBackType.Slow);
                    temp = (PlaySpeed)((int)temp - 1);

                }
            }
            CurrentSpeed = speed;
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

            _previewSliderValue = (int)sliderMaxMinutes;

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
