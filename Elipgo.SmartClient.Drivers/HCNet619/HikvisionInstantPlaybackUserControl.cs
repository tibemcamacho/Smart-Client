using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Elipgo.SmartClient.Drivers.HCNet619
{
    public partial class HikvisionInstantPlaybackUserControl : UserControl, IDriverInstantPlayback, IDisposable, IConectionNotification
    {
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        public ILoginControl loginControl = Locator.Current.GetService<ILoginControl>();
        private bool _listenStatus = false;

        private Int32 loginHandle = -1;
        private Int32 playbackHandle = -1;
        private CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo;
        private CHCNetSDK.EXCEPYIONCALLBACK m_HikVisionExceptionCallBack;

        private PlaySpeed CurrentSpeed = PlaySpeed.NORMAL;
        public event OnDriverDispose OnDispose;

        private DateTime m_PlayBack_StartTime = DateTime.Now.AddMinutes(-30);
        private DateTime initialTime = DateTime.Now.AddMinutes(-30);
        private DateTime m_PlayBack_EndTime = DateTime.Now.AddMinutes(-2);
        private DispatcherTimer m_DispatcherTimer = new DispatcherTimer();
        private DateTime actualTime = DateTime.Now.AddMinutes(-30);
        private DateTime CameraTime = DateTime.Now;

        private bool _offLine = false;
        private int _retryCount = 0;
        private readonly int _retryLimit;
        private int tryCount = 0;
        private readonly int tryLimit;
        private readonly int retryCallbackLimit;
        private const int MAX_HOUR_END = 12;
        private const int MAX_MINUTES = 360;
        private bool instantPlayback = true;
        private int StartDigitalChannel = 0;
        private delegate void SafeCallDelegate(PlaybackConnectionState connectionState);
        private readonly Random _random = new Random();
        private double TimeReConnectionCheck;
        private RecorderDTOSmall Recorder;

        private bool _secondScale;
        private PlayScaleTimeLine _currentScale;
        private int _previewSliderValue;
        private int _sliderCurrentValue;
        private bool _scaleActive;
        private int _currentBlock;
        private int _additionalSeconds;
        private DateTime _selectedDateTime;
        //private bool _isVault = false;
        public bool _isPlaying { get; set; }

        public HikvisionInstantPlaybackUserControl(CameraDTO camera, Profile profile, bool hideControls, DateTime selectedDateTime, RecorderDTOSmall recorder, DateTime? selectedEndDateTime = null)
        {
            InitializeComponent();

            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

            _retryLimit = int.Parse(config.AppSettings.Settings["retryLimit"].Value);
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
            retryCallbackLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);

            this.TimeReConnectionCheck = 5;
            this.Resize += DahuaInstantPlaybackUserControl_Resize;
            Camera = camera;
            Profile = profile;
            ClipStatus = false;
            Recorder = recorder;

            this.Load += HikvisionInstantPlaybackUserControl_Load;
            this.Click += HikvisionInstantPlaybackUserControl_Click;
            this.picture.Click += HikvisionInstantPlaybackUserControl_Click;

            this.DoubleClick += HikvisionInstantPlaybackUserControl_DoubleClick;
            this.picture.DoubleClick += HikvisionInstantPlaybackUserControl_DoubleClick;

            this.Paint += HikvisionInstantPlaybackUserControl_Paint;

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

                m_PlayBack_StartTime = CameraTime.AddMinutes(-2 * MAX_MINUTES);
                initialTime = CameraTime.AddMinutes(-2 * MAX_MINUTES);
                m_PlayBack_EndTime = CameraTime;
                this.ActualTime = CameraTime.AddSeconds(-75);
                actualTime = CameraTime.AddMinutes(-2 * MAX_MINUTES);
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

            panelNoConnection.Visible = this._offLine;
            this.MouseWheel += Picture_MouseWheel;
            ZoomStatus = false;
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

                // Convert parent-relative coords to picture-relative (ZoomPicture expects picture coords)
                int picRelX = relativePosition.X - picture.Location.X;
                int picRelY = relativePosition.Y - picture.Location.Y;

                if (keyData == (Keys.Control | Keys.Oemplus) || keyData == (Keys.Control | Keys.Add))
                {
                    ZoomPicture(true, picRelX, picRelY);
                }

                if (keyData == (Keys.Control | Keys.OemMinus) || keyData == (Keys.Control | Keys.Subtract))
                {
                    ZoomPicture(false, picRelX, picRelY);
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
                Form instantPlayerView = Application.OpenForms.OfType<Form>().FirstOrDefault(f => f.Name == "InstantPlayerView" && (string)f.Tag == Camera.IdGuid);
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

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

                var ButtonZoomX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.801), 2));
                var ButtonZoomY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonZoom.Size = new Size(btn, btn);
                ButtonZoom.Location = new Point(ButtonZoomX, ButtonZoomY);

                var ButtonFullScreenX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.705), 2));
                var ButtonFullScreenY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonFullScreen.Size = new Size(btn, btn);
                ButtonFullScreen.Location = new Point(ButtonFullScreenX, ButtonFullScreenY);


                var ButtonBookmarkX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.754), 2));
                var ButtonBookmarkY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonBookmark.Size = new Size(btn, btn);
                ButtonBookmark.Location = new Point(ButtonBookmarkX, ButtonBookmarkY);

                var ButtonSnapshotX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.801), 2));
                var ButtonSnapshotY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));
                int difference = ButtonZoom.Location.X - ButtonBookmark.Location.X;

                ButtonSnapshot.Size = new Size(btn, btn);
                ButtonSnapshot.Location = new Point(ButtonSnapshotX + (ButtonZoom.Visible ? difference : 0), ButtonSnapshotY);

                var ButtonListenX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.896), 2));
                var ButtonListenY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonListen.Size = new Size(btn, btn);
                ButtonListen.Location = new Point(ButtonListenX, ButtonListenY);

                var ButtonVideoclipX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.848), 2));
                var ButtonVideoclipY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonVideoclip.Size = new Size(btn, btn);
                ButtonVideoclip.Location = new Point(ButtonVideoclipX, ButtonVideoclipY);
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
                    this.LabelSpeed.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_MEDIUM);
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    this.LabelSpeed.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_MEDIUM);
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

        private void setPanelNoConnectionVisibility()
        {
            panelNoConnection.Visible = this._offLine;
        }

        private void HikvisionInstantPlaybackUserControl_Click(object sender, EventArgs e)
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

                    int contentX = mouse.X - picture.Location.X;
                    int contentY = mouse.Y - picture.Location.Y;

                    int newX = Math.Min(0, Math.Max(this.Width - picture.Width, (this.Width / 2) - contentX));
                    int newY = Math.Min(0, Math.Max(this.Height - picture.Height, (this.Height / 2) - contentY));

                    picture.Location = new Point(newX, newY);
                }
            }

            this.BringToFront();
            this.Focus();
        }

        private void HikvisionInstantPlaybackUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick(this);

                if (ZoomStatus)
                {
                    if (picture.Location.X < 0)
                    {
                        var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);

                        int contentX = mouse.X - picture.Location.X;
                        int contentY = mouse.Y - picture.Location.Y;

                        int newX = Math.Min(0, Math.Max(this.Width - picture.Width, (this.Width / 2) - contentX));
                        int newY = Math.Min(0, Math.Max(this.Height - picture.Height, (this.Height / 2) - contentY));

                        picture.Location = new Point(newX, newY);
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

            SliderTooltip.Location = new Point(slider.Location.X, slider.Location.Y - 20);
            SliderTooltip.Visible = true;
            SliderTooltip.BringToFront();
            //SliderValueChangeComplete();
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

                SliderTooltip.Visible = false;
                m_PlayBack_StartTime = initialTime.AddSeconds(sliderValueTemp);
                LabelSpeed.Text = "1X";
                CurrentSpeed = PlaySpeed.NORMAL;
                ButtonFast.Enabled = true;
                ButtonSlow.Enabled = true;
                _isPlaying = OpenCamera(Camera.Channel);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("SliderValueChangeComplete Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
        }

        private void HikvisionInstantPlaybackUserControl_Paint(object sender, PaintEventArgs e)
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

            Task.Run(() => { IntializeCamera(); });

            this.picture.Size = this.PanelVideo.Size;
        }

        public new void Dispose()
        {
            if (!instantPlayback)
            {
                if (loginControl.RemoveChannelAndCanLogout(Camera, this, Common.Enum.Drivers.HCNetSDK_619))
                {
                    Logger.Log(String.Format(" Dispose Logout Hik {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    CHCNetSDK.NET_DVR_Logout(loginHandle);
                }

                CHCNetSDK.NET_DVR_StopRealPlay(playbackHandle);
                CHCNetSDK.NET_DVR_Cleanup();
            }

            CHCNetSDK.NET_DVR_CloseSound();
            //CHCNetSDK.NET_DVR_StopPlayBackSave(playbackHandle);
            CHCNetSDK.NET_DVR_StopPlayBack(playbackHandle);
            m_DispatcherTimer.Stop();
            this.MouseWheel -= Picture_MouseWheel;
        }

        private void HikvisionInstantPlaybackUserControl_Load(object sender, EventArgs e)
        {
            DispatcherTimerInit();
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

                CHCNetSDK.NET_DVR_TIME osdTime = new CHCNetSDK.NET_DVR_TIME();
                if (CHCNetSDK.NET_DVR_GetPlayBackOsdTime(playbackHandle, ref osdTime))
                {
                    var sTime = string.Format("{0}-{1}-{2} {3}:{4}:{5}", osdTime.dwYear, osdTime.dwMonth, osdTime.dwDay, osdTime.dwHour, osdTime.dwMinute, osdTime.dwSecond);
                    actualTime = DateTime.Parse(sTime);
                    OnTimeChanged?.Invoke(actualTime, this);

                    var sliderValue = (int)(actualTime - initialTime).TotalSeconds;
                    if (sliderValue <= slider.MaximumValue)
                    {
                        slider.Value = sliderValue;
                    }

                    int playbackPos = CHCNetSDK.NET_DVR_GetPlayBackPos(playbackHandle);
                    if (playbackHandle >= 0 && (playbackPos == 100 || playbackPos < 0))
                    {
                        m_DispatcherTimer.Stop();
                        ShowNoRecordingMessage(actualTime);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private bool ChangeProfile(Profile profile)
        {
            if (Profile != profile && playbackHandle != -1)
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
        public bool ClipStatus { get; set; }
        public bool ListenStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ActualTime { get; set; }
        public bool BookmarkState { get; set; }

        public event OnVideoEventHandler OnVideo;
        public event OnTimeChangedEventHandler OnTimeChanged;
        public event OnStateChangedEventHandler OnStateChanged;
        public event EventHandler<bool> OpenBookmark;
        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;

        public bool SetStartDateTime(DateTime dateTime, bool changeSlider = true, bool isVault = false)
        {
            m_PlayBack_StartTime = dateTime;
            StartTime = dateTime;
            initialTime = dateTime;
            actualTime = dateTime;

            //var date = DateTime.Now - dateTime;

            if (changeSlider)
            {
                m_PlayBack_EndTime = DateTime.Now.AddHours(MAX_HOUR_END);// date.TotalMinutes > MAX_MINUTES ? dateTime.AddMinutes(MAX_MINUTES) : DateTime.Now.AddMinutes(-2);

                var tSecs = (m_PlayBack_EndTime - m_PlayBack_StartTime).TotalSeconds;
                slider.MaximumValue = (int)tSecs;
            }
            else
            {
                if (!isVault)
                {
                    m_PlayBack_EndTime = Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd 23:59:59"));
                }
            }

            EndTime = m_PlayBack_EndTime;
            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            if (_paused)
            {
                _paused = !_paused;
            }

            bool ret = OpenCamera(Camera.Channel);
            updateToSelectedSpeed();
            _isPlaying = ret;
            slider.Enabled = ret;

            return ret;
        }
        public bool Pause()
        {
            if (!_paused)
            {
                CapacityNotAvailable(false);
                this._paused = !this._paused;
                uint iOutValue = 0;

                CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYPAUSE, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
            }
            if (_paused)
            {
                ButtonPause.Image = FileResources.icon_pause;
                ButtonPlay.Image = FileResources.icon_play;
            }
            else
            {
                ButtonPlay.Image = FileResources.icon_play;
                ButtonPause.Image = FileResources.icon_pause;
            }
            return this._paused;
        }

        public bool Play()
        {
            if (_isPlaying)
            {
                return true;
            }
            CapacityNotAvailable(false);
            if (loginHandle != -1)
            {
                ButtonPlay.Image = FileResources.icon_play;
                ButtonPause.Image = FileResources.icon_pause;
                if (_paused)
                {
                    _isPlaying = true;
                    _paused = !_paused;
                }

                SetVisivility(PlaybackConnectionState.Connecting);

                bool bSuccess = OpenCamera(Camera.Channel);
                _isPlaying = bSuccess;
                if (!bSuccess)
                {
                    TryToReConnect();
                }
                else
                {

                    Logger.Log(String.Format(" Play Hik Connected {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    if (StartTime.ToUniversalTime() > DateTime.UtcNow)
                    {
                        ShowNoRecordingMessage(m_PlayBack_StartTime);
                    }
                    else
                    {
                        SetVisivility(PlaybackConnectionState.Connected);
                        updateToSelectedSpeed();
                        tryCount = 0;
                    }
                }
                return bSuccess;
            }
            else
            {
                Logger.Log(String.Format("Error al realizar el Login Camera:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                return false;
            }
        }

        public bool PlayVideo()
        {
            if (_paused)
            {
                uint iOutValue = 0;
                CapacityNotAvailable(false);
                this._paused = !this._paused;

                CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYNORMAL, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);

                ButtonPlay.Image = FileResources.icon_play;
                ButtonPause.Image = FileResources.icon_pause;

                m_DispatcherTimer.Start();
            }
            return true;
        }



        private void TryToReConnect()
        {
            try
            {
                if (tryCount >= tryLimit)
                {
                    SetVisivility(PlaybackConnectionState.Disconnected);
                    Logger.Log(String.Format("HikVsion TryToReConnect reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    return;
                }

                SetVisivility(PlaybackConnectionState.Reconnecting);
                tryCount++;
                int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                Logger.Log(String.Format(" TryToReConnect failed  {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                Task.Delay(r).ContinueWith(t => Play());

            }
            catch (Exception ex)
            {
                Logger.Log(String.Format(" TryToReConnect Exception: {4}  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex.Message), LogPriority.Fatal);
                throw ex;
            }
        }

        private bool OpenCamera(int channel)
        {
            if (loginHandle == -1)
            {
                return false;
            }

            //stop play
            if (playbackHandle != -1)
            {
                Stop();
            }
            try
            {
                CHCNetSDK.NET_DVR_VOD_PARA struVodPara = new CHCNetSDK.NET_DVR_VOD_PARA();
                struVodPara.dwSize = (uint)Marshal.SizeOf(struVodPara);
                struVodPara.struIDInfo.dwChannel = (uint)channel;//(uint)HikvisionHelper.GetChannelNumber(channel, this.StartDigitalChannel); //Channel number  
                struVodPara.hWnd = GetHandle();//handle of playback

                //Set the starting time to search video files
                struVodPara.struBeginTime.dwYear = (uint)m_PlayBack_StartTime.Year;
                struVodPara.struBeginTime.dwMonth = (uint)m_PlayBack_StartTime.Month;
                struVodPara.struBeginTime.dwDay = (uint)m_PlayBack_StartTime.Day;
                struVodPara.struBeginTime.dwHour = (uint)m_PlayBack_StartTime.Hour;
                struVodPara.struBeginTime.dwMinute = (uint)m_PlayBack_StartTime.Minute;
                struVodPara.struBeginTime.dwSecond = (uint)m_PlayBack_StartTime.Second;

                //Set the stopping time to search video files
                struVodPara.struEndTime.dwYear = (uint)m_PlayBack_EndTime.Year;
                struVodPara.struEndTime.dwMonth = (uint)m_PlayBack_EndTime.Month;
                struVodPara.struEndTime.dwDay = (uint)m_PlayBack_EndTime.Day;
                struVodPara.struEndTime.dwHour = (uint)m_PlayBack_EndTime.Hour;
                struVodPara.struEndTime.dwMinute = (uint)m_PlayBack_EndTime.Minute;
                struVodPara.struEndTime.dwSecond = (uint)m_PlayBack_EndTime.Second;

                StartTime = m_PlayBack_StartTime;
                EndTime = m_PlayBack_EndTime;

                playbackHandle = CHCNetSDK.NET_DVR_PlayBackByTime_V40(loginHandle, ref struVodPara);
                if (playbackHandle < 0)
                {
                    ShowNoRecordingMessage(m_PlayBack_StartTime);
                    return false;
                }
                uint iOutValue = 0;
                bool bSucess = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYSTART, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
                if (bSucess)
                {
                    SetVisivility(PlaybackConnectionState.Connected);
                    m_HikVisionExceptionCallBack = new CHCNetSDK.EXCEPYIONCALLBACK(HikVisionExceptionCallBack);
                    CHCNetSDK.NET_DVR_SetExceptionCallBack_V30(0, IntPtr.Zero, m_HikVisionExceptionCallBack, IntPtr.Zero);
                }

            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                return false;
            }
            if (playbackHandle != -1)
            {
                OnVideo?.Invoke(true, this);
                m_DispatcherTimer.Start();
                return true;
            }

            return false;
        }

        private void HikVisionExceptionCallBack(uint dwType, int lUserID, int lHandle, IntPtr pUser)
        {
            switch (dwType)
            {
                case CHCNetSDK.EXCEPTION_RECONNECT:
                    tryCount++;
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    Logger.Log(String.Format("HikVsion HikVisionExceptionCallBack Reconnect Event received :  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    break;
                //estos valores de eventos no figuran en la api, pero de manera empirica pude validar que se reciben cuando conecta la camara
                case 32789:
                case 32791:
                    tryCount = 0;
                    SetVisivility(PlaybackConnectionState.Connected);
                    Logger.Log(String.Format("HikVsion HikVisionExceptionCallBack Reconnection Event received :  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    break;
                default:
                    break;

            }
            if (tryCount >= tryLimit)
            {
                SetVisivility(PlaybackConnectionState.Disconnected);
                Logger.Log(String.Format("HikVsion HikVisionExceptionCallBack alcanzo el maximo de reintento, estado desconectado:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
            }
        }

        private IntPtr GetHandle()
        {
            return picture.Handle;
        }
        private bool IntializeCamera()
        {
            bool res = CHCNetSDK.NET_DVR_Init();
            if (res)
            {
                if (Login())
                {
                    if (instantPlayback)
                    {
                        m_PlayBack_StartTime = m_PlayBack_EndTime.AddMinutes(-1);
                        slider.Value = (int)(m_PlayBack_EndTime - m_PlayBack_StartTime).TotalSeconds;
                    }
                    return Play();
                }
                else
                {
                    _offLine = true;
                    _retryCount++;
                    if (_retryCount <= _retryLimit)
                    {
                        SetVisivility(PlaybackConnectionState.Reconnecting);
                        Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * _retryCount) }, () => IntializeCamera());
                        Logger.Log(String.Format("Error al realizar el Login Camera:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                        _retryCount++;
                    }
                    else
                    {
                        SetVisivility(PlaybackConnectionState.Disconnected);
                        Logger.Log(String.Format("HikVision Error al realizar el Login Camera alcanzo el maximo de reintentos, estado desconectado:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    }

                }
            }
            else
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, "NetClient init failed!"), null);
            }

            return false;
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
            if (this.picture.InvokeRequired)
            {
                var d = new SafeCallDelegate(SetVisivility);
                panelNoConnection.Invoke(d, new object[] { connectionState });
            }
            try
            {
                switch (connectionState)
                {
                    case PlaybackConnectionState.Disconnected:
                        panelNoConnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_es);
                        }
                        else
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_en);
                        }
                        break;
                    case PlaybackConnectionState.Connected:
                        panelNoConnection.SendToBack();
                        panelNoConnection.Visible = false;
                        PanelVideo.BringToFront();
                        break;
                    case PlaybackConnectionState.NoRecording:
                        this.panelNoConnection.Visible = true;
                        this.panelNoConnection.Size = PanelVideo.Size;
                        this.panelNoConnection.Location = PanelVideo.Location;
                        PanelVideo.SendToBack();
                        this.panelNoConnection.BringToFront();
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_es);
                        }
                        else
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_en);
                        }
                        break;
                    case PlaybackConnectionState.Reconnecting:
                        panelNoConnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_es);
                        }
                        else
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_en);
                        }
                        break;
                    case PlaybackConnectionState.Connecting:
                        panelNoConnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_es);
                        }
                        else
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_en);
                        }
                        break;
                }
                System.Windows.Forms.PictureBox panelFondoLogo = new PictureBox();
                Reconnecting.DisplayLogo(picture.Width, picture.Height, ref panelNoConnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("setVisivility Exception: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
            }

        }

        private bool Login()
        {

            try
            {
                IntPtr ptr;
                if (loginControl.GetDeviceLogin(Camera, out ptr, Common.Enum.Drivers.HCNetSDK_619, this))
                {
                    loginHandle = (Int32)ptr;
                    loginControl.AddChannel(Camera, this, Common.Enum.Drivers.HCNetSDK_619);
                    Logger.Log(String.Format("Reusing a Hikivion Login to Camara {0}", Camera.Name), LogPriority.Information);
                    return true;
                }

                int dev_port = Convert.ToUInt16(Camera.VideoPort);
                loginHandle = CHCNetSDK.NET_DVR_Login_V30(Camera.Host, dev_port, Camera.User, Camera.Password, ref DeviceInfo);
                if (loginHandle != -1)
                {
                    Logger.Log(String.Format("New Hikivion Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                    this.StartDigitalChannel = DeviceInfo.byStartDChan;
                    loginControl.AddDevice(Camera, (IntPtr)loginHandle, this, Common.Enum.Drivers.HCNetSDK_619);
                }
                return loginHandle != -1;
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                return false;
            }
        }

        public bool Snapshot(string path)
        {
            try
            {
                CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA
                {
                    wPicQuality = 0,
                    wPicSize = 0xff
                };

                //bool result = CHCNetSDK.NET_DVR_CaptureJPEGPicture(loginHandle, HikvisionHelper.GetChannelNumber(Camera.Channel, this.StartDigitalChannel), ref lpJpegPara, path);
                bool result = CHCNetSDK.NET_DVR_PlayBackCaptureFile(playbackHandle, path);
                return result;
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                return false;
            }
        }

        public bool Stop()
        {
            bool result = CHCNetSDK.NET_DVR_StopPlayBack(playbackHandle);
            playbackHandle = -1;
            return result;
        }

        public void ToggleFullScreen()
        {

        }

        public bool ToggleListen()
        {
            try
            {
                _listen = !_listen;
                uint iOutValue = 0;
                if (_listen)
                {
                    CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYSTARTAUDIO, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
                }
                else
                {
                    CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYSTOPAUDIO, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
                }

                ListenStatus = _listen;
                return _listen;
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                ListenStatus = false;
                return false;
            }
        }

        public bool VideoClipStart(string path)
        {
            try
            {
                CHCNetSDK.NET_DVR_MakeKeyFrame(loginHandle, Camera.Channel); //HikvisionHelper.GetChannelNumber(Camera.Channel, this.StartDigitalChannel)
                bool ret = CHCNetSDK.NET_DVR_PlayBackSaveData(playbackHandle, path);
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
                bool ret = CHCNetSDK.NET_DVR_StopPlayBackSave(playbackHandle);
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

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            m_PlayBack_StartTime = actualTime;
            bool ret = PlayVideo();
            slider.Enabled = ret;
            if (ret)
            {
                m_DispatcherTimer.Start();
            }
        }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            m_DispatcherTimer.Stop();
            Pause();
        }

        private bool updateToSelectedSpeed()
        {
            PlaySpeed temp = PlaySpeed.NORMAL;
            bool result = true;

            uint iOutValue = 0;
            while (result && temp != CurrentSpeed)
            {
                if (CurrentSpeed >= PlaySpeed.MIN && CurrentSpeed < PlaySpeed.NORMAL)
                {
                    result = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYSLOW, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
                    temp = (PlaySpeed)((int)temp - 1);
                }
                else
                {
                    result = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYFAST, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
                    temp = (PlaySpeed)((int)temp + 1);
                }
            }

            return result;
        }

        public bool Jump(int sec, bool asc)
        {
            actualTime = actualTime.AddSeconds(asc == true ? sec : sec * -1);
            m_PlayBack_StartTime = actualTime;
            _isPlaying = false;
            bool ret = Play();
            slider.Enabled = ret;
            if (ret)
            {
                m_DispatcherTimer.Start();
            }

            return true;
        }


        private void ButtonRewSecs_Click(object sender, EventArgs e)
        {
            Jump(30, false);
        }

        private void ButtonFwdSecs_Click(object sender, EventArgs e)
        {
            Jump(30, true);
        }

        private void ButtonSlow_Click(object sender, EventArgs e)
        {
            Slow();
        }

        public bool Slow()
        {
            bool result = false;
            if (playbackHandle == -1)
            {
                return false;
            }
            try
            {
                uint iOutValue = 0;
                result = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYSLOW, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
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
            if (this.CurrentSpeed == masterSpeed)
            {
                return result;
            }
            if (playbackHandle == -1)
            {
                return false;
            }
            if (masterSpeed < CurrentSpeed)
            {
                while (CurrentSpeed != masterSpeed && result)
                {
                    uint iOutValue = 0;
                    result = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYSLOW, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
                    if (result)
                    {
                        CurrentSpeed = (PlaySpeed)((int)CurrentSpeed - 1);
                    }
                }
            }
            else
            {
                while (CurrentSpeed != masterSpeed && result)
                {
                    uint iOutValue = 0;
                    result = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYFAST, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
                    if (result)
                    {
                        CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
                    }
                }

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
            if (playbackHandle == -1)
            {
                return false;
            }
            try
            {
                uint iOutValue = 0;
                result = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYFAST, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
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

            if (playbackHandle == -1)
            {
                return false;
            }
            try
            {
                int variance = speed > 0 ? -1 : 1;
                while (speed != 0)
                {
                    uint iOutValue = 0;
                    result = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYFAST, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
                    speed += variance;
                    CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + (variance * -1));
                }
                UpdateControlsToShowSpeed();
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
                this._actualSize = 0;
            }
            else
            {
                this.Cursor = Cursors.Cross;
                this.BringToFront();
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

            // Convert picture-relative coords to parent-relative coords
            int parentX = eventPositionX + picture.Location.X;
            int parentY = eventPositionY + picture.Location.Y;

            // Calculate content fraction under cursor using current picture dimensions
            double fracX = (double)eventPositionX / picture.Width;
            double fracY = (double)eventPositionY / picture.Height;

            // Where that content point will be in the new size
            int newContentX = (int)(fracX * picSize.Width);
            int newContentY = (int)(fracY * picSize.Height);

            // Position picture so content stays under cursor
            int newX = parentX - newContentX;
            int newY = parentY - newContentY;

            // Clamp position so the picture always covers the visible area
            newX = Math.Min(0, Math.Max(this.Width - picSize.Width, newX));
            newY = Math.Min(0, Math.Max(this.Height - picSize.Height, newY));

            var picPosition = new Point(newX, newY);
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


        public bool PlayNoAsync()
        {
            if (loginHandle == -1)
            {
                return IntializeCamera();
            }
            else
            {
                return Play();
            }
        }

        private void ShowNoRecordingMessage(DateTime noRecordTime)
        {
            var message = string.Format("{0} - {1}", Camera.Name, noRecordTime.ToString("yyyy/MM/dd HH:mm:ss"));
            notification.Show(string.Format(Resources.NoRecordingAvailable, message), null);
            Logger.Log(String.Format("HikVision no recording available {0} {1} {2} {3} StartTime {4} EndTime {5} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, noRecordTime, m_PlayBack_EndTime), LogPriority.Information);
            SetVisivility(PlaybackConnectionState.NoRecording);
        }

        public void SelectSpeed(PlaySpeed speed)
        {
            PlaySpeed temp = CurrentSpeed;
            bool result = true;
            while (result && temp != speed)
            {
                uint iOutValue = 0;
                if (temp >= PlaySpeed.MIN && temp < PlaySpeed.NORMAL)
                {
                    result = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYFAST, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
                    temp = (PlaySpeed)((int)temp + 1);
                }
                else
                {
                    result = CHCNetSDK.NET_DVR_PlayBackControl_V40(playbackHandle, CHCNetSDK.NET_DVR_PLAYSLOW, IntPtr.Zero, 0, IntPtr.Zero, ref iOutValue);
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
