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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Elipgo.SmartClient.Drivers.UNVNet231
{
    public partial class UniviewInstantPlaybackUserControl : UserControl, IDriverInstantPlayback, IDisposable, IConectionNotification
    {
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        public ILoginControl loginControl = Locator.Current.GetService<ILoginControl>();
        public event OnDriverDispose OnDispose;
        private bool offLine = false;
        private bool instantPlayback = true;
        private DateTime m_PlayBack_StartTime = DateTime.Now.AddMinutes(-30);
        private DateTime initialTime = DateTime.Now.AddMinutes(-30);
        private DateTime m_PlayBack_EndTime = DateTime.Now.AddMinutes(-2);
        private DispatcherTimer m_DispatcherTimer = new DispatcherTimer();
        private DateTime actualTime = DateTime.Now.AddMinutes(-30);
        private DateTime CameraTime = DateTime.Now;
        private const int MAX_MINUTES = 360;
        private bool _listenStatus = false;
        public bool ListenStatus { get; set; }
        private int _actualSize = 0;
        private int _zoomLimit = int.Parse(Settings.Default["ZoomLimit"].ToString());
        private bool _paused = false;
        private PlaySpeed CurrentSpeed = PlaySpeed.NORMAL;
        private IntPtr playbackHandle = IntPtr.Zero;
        private bool _painted = false;
        IntPtr loginHandle = IntPtr.Zero;
        private int tryCount = 0;
        private const int MAX_HOUR_END = 12;
        private readonly int tryLimit;

        private double TimeReConnectionCheck;
        private readonly Random _random = new Random();
        private int retryCount = 0;
        private readonly int retryLimit;
        private bool _listen = false;
        private RecorderDTOSmall Recorder;

        public UniviewInstantPlaybackUserControl(CameraDTO camera, Profile profile, bool hideControls, DateTime selectedDateTime, RecorderDTOSmall recorder, DateTime? selectedEndDateTime = null)
        {
            InitializeComponent();
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
            retryLimit = int.Parse(config.AppSettings.Settings["retryLimit"].Value);
            Camera = camera;
            Profile = profile;
            ClipStatus = false;
            Recorder = recorder;

            this.Load += UniviewInstantPlaybackUserControl_Load;
            this.Click += UniviewInstantPlaybackUserControl_Click;
            this.picture.Click += UniviewInstantPlaybackUserControl_Click;

            this.DoubleClick += UniviewInstantPlaybackUserControl_DoubleClick;
            this.picture.DoubleClick += UniviewInstantPlaybackUserControl_DoubleClick;

            this.Paint += UniviewInstantPlaybackUserControl_Paint;
            this.Resize += UniviewInstantPlaybackUserControl_Resize;
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

                m_PlayBack_StartTime = CameraTime.AddMinutes(-1 * MAX_MINUTES);
                initialTime = CameraTime.AddMinutes(-1 * MAX_MINUTES);
                m_PlayBack_EndTime = CameraTime;
                this.ActualTime = CameraTime.AddSeconds(-75);
                actualTime = CameraTime.AddMinutes(-1 * MAX_MINUTES);
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

        private void ButtonBookmark_Click(object sender, EventArgs e)
        {
            BookmarkState = !BookmarkState;

            this.ActualTime = actualTime;
            OpenBookmark?.Invoke(this, BookmarkState);
        }

        private void Slider_ValueChangeComplete(object sender, EventArgs e)
        {
            SliderValueChangeComplete();
        }

        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            var datetime = initialTime.AddSeconds(slider.Value);
            SliderTooltip.Text = datetime.ToString("MM/dd HH:mm:ss");

            SliderTooltip.Location = new Point(slider.Location.X, slider.Location.Y - 13);
            SliderTooltip.Visible = true;
            SliderValueChangeComplete();
        }

        private void SliderValueChangeComplete()
        {
            try
            {
                SliderTooltip.Visible = false;
                m_PlayBack_StartTime = initialTime.AddSeconds(slider.Value);
                LabelSpeed.Text = "1X";
                CurrentSpeed = PlaySpeed.NORMAL;
                ButtonFast.Enabled = true;
                ButtonSlow.Enabled = true;
                OpenCamera(Camera.Channel);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("SliderValueChangeComplete Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
        }

        private void UniviewInstantPlaybackUserControl_Paint(object sender, PaintEventArgs e)
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
            ButtonFast.Image = FileResources.icon_fast;
            ButtonZoom.Image = FileResources.icon_digital_zoom_off;
            ButtonSlow.Image = FileResources.icon_slow;
            LabelSpeed.Text = "1X";
            LabelSpeed.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);

            _painted = true;
            var tSecs = (m_PlayBack_EndTime - m_PlayBack_StartTime).TotalSeconds;
            slider.MaximumValue = (int)tSecs;
            slider.Enabled = true;

            int iRet = NETDEVSDK.NETDEV_Init();

            if (NETDEVSDK.TRUE == iRet)
            {
                init();
            }
            else
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, "NetClient init failed!"), null);
            }

            this.picture.Size = this.PanelVideo.Size;
        }

        private void init()
        {
            if (Login())
            {
                if (instantPlayback)
                {
                    m_PlayBack_StartTime = m_PlayBack_EndTime.AddMinutes(-1);
                    slider.Value = (int)(m_PlayBack_EndTime - m_PlayBack_StartTime).TotalSeconds;
                }
                Play();
            }
            else
            {
                retryCount++;
                if (retryCount <= retryLimit)
                {
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * retryCount) }, () => init());
                    Logger.Log(String.Format("Error al realizar el Login Camera:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    retryCount++;
                }
                else
                {
                    SetVisivility(PlaybackConnectionState.Disconnected);
                    Logger.Log(String.Format("Uniview Error al realizar el Login Camera alcanzo el maximo de reintentos, estado desconectado:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                }
            }
        }

        private void UniviewInstantPlaybackUserControl_Click(object sender, EventArgs e)
        {
            if (CameraSelected != null)
            {
                CameraSelected(this, Camera);
            }

            if (ZoomStatus)
            {
                var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);
                var mp = new Point((100 * mouse.X) / this.Width, (100 * mouse.Y) / this.Height);
                var p = new Point((picture.Width * mp.X) / 100, (picture.Height * mp.Y) / 100);
                var picPosition = new Point((mouse.X - p.X), (mouse.Y - p.Y));
                picture.Location = picPosition;

                this.Focus();
                this.BringToFront();
            }
        }

        private void UniviewInstantPlaybackUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick(this);

                if (ZoomStatus)
                {
                    var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);
                    var mp = new Point((100 * mouse.X) / this.Width, (100 * mouse.Y) / this.Height);
                    var p = new Point((picture.Width * mp.X) / 100, (picture.Height * mp.Y) / 100);
                    var picPosition = new Point((mouse.X - p.X), (mouse.Y - p.Y));
                    picture.Location = picPosition;
                }
            }
        }

        private void UniviewInstantPlaybackUserControl_Load(object sender, EventArgs e)
        {
            DispatcherTimerInit();
        }

        private void ButtonFast_Click(object sender, EventArgs e)
        {
            Fast();
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

        private void ButtonListen_Click(object sender, EventArgs e)
        {
            ButtonListen.Image = _listenStatus ? FileResources.icon_sound_off : FileResources.icon_sound_on;
            ToggleListen();
            _listenStatus = !_listenStatus;
        }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            m_DispatcherTimer.Stop();
            Pause();
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            m_PlayBack_StartTime = actualTime;
            bool ret = Play();
            slider.Enabled = ret;
            if (ret)
            {
                m_DispatcherTimer.Start();
            }
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

        private void ButtonZoom_Click(object sender, EventArgs e)
        {
            ToogleDigitalZoom();
        }

        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }
        public List<ButtonsContextBar> Commands => GetButtons();
        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();

        public bool ClipStatus { get; set; }
        public bool ZoomStatus { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ActualTime { get; set; }
        public bool BookmarkState { get; set; }

        public event OnVideoEventHandler OnVideo;
        public event EventHandler<bool> OpenBookmark;
        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnTimeChangedEventHandler OnTimeChanged;
        public event OnStateChangedEventHandler OnStateChanged;

        public List<ButtonsPlayBackBar> ButtonsNotAllowed()
        {
            return new List<ButtonsPlayBackBar>();
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

        public bool Fast()
        {
            if (playbackHandle == IntPtr.Zero)
            {
                return false;
            }

            long enSpeed = 0;
            if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYSPEED, ref enSpeed))
            {
                Logger.Log(string.Format("Get play speed {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                return false;
            }
            else
            {
                if (enSpeed <= (int)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_2_FORWARD)
                {
                    enSpeed++;

                    if (enSpeed < (long)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_1_FORWARD && enSpeed > (long)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_1_BACKWARD)//5-8
                    {
                        enSpeed = (long)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_1_FORWARD;
                    }
                    else if (enSpeed > (long)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_16_FORWARD)
                    {
                        enSpeed--;
                    }
                    if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_SETPLAYSPEED, ref enSpeed))
                    {
                        Logger.Log(string.Format("Set play speed {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                        return false;
                    }
                    ShowPlaySpeed(1);
                }
                return true;
            }
        }

        private void ShowPlaySpeed(int nMode)
        {
            if (nMode == 0)
            {
                CurrentSpeed = PlaySpeed.NORMAL;
            }
            else if (nMode > 0)	// Speed up
            {
                if (CurrentSpeed >= PlaySpeed.MIN && CurrentSpeed < PlaySpeed.UP_4)
                {
                    CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
                }
            }
            else if (nMode < 0)	// Speed down
            {
                if (CurrentSpeed > PlaySpeed.MIN && CurrentSpeed <= PlaySpeed.UP_4)
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

        public PlaySpeed GetCurrentSpeed()
        {
            return this.CurrentSpeed;
        }

        public bool Jump(int sec, bool asc)
        {
            actualTime = actualTime.AddSeconds(asc == true ? sec : sec * -1);
            m_PlayBack_StartTime = actualTime;
            bool ret = Play();
            slider.Enabled = ret;
            if (ret)
            {
                m_DispatcherTimer.Start();
            }

            long iPlayTime = 0;
            int iRet = NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYTIME, ref iPlayTime);
            if (NETDEVSDK.TRUE == iRet)
            {
                OnTimeChanged?.Invoke(Convert.ToDateTime(getStrTime(iPlayTime)), this);
                if (iPlayTime <= slider.MaximumValue)
                {
                    slider.Value = (int)iPlayTime;
                }
            }

            return true;
        }

        public bool Pause()
        {
            CapacityNotAvailable(false);
            this._paused = !this._paused;
            long temp = 0;
            if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_PAUSE, ref temp))
            {
                Logger.Log(string.Format("Pause playBack {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                return false;
            }
            else
            {
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

        }

        public bool Play()
        {
            CapacityNotAvailable(false);
            if (loginHandle != IntPtr.Zero)
            {
                ButtonPlay.Image = FileResources.icon_play;
                ButtonPause.Image = FileResources.icon_pause;
                if (_paused)
                {
                    _paused = !_paused;
                }

                SetVisivility(PlaybackConnectionState.Connecting);

                bool bSuccess = OpenCamera(Camera.Channel);
                if (!bSuccess)
                {
                    tryCount = 0;
                    TryToReConnect();
                    return false;
                }
                else
                {
                    Logger.Log(String.Format(" Play Uniview Connected {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Connected);
                    updateToSelectedSpeed();
                    return bSuccess;

                }
            }
            else
            {
                Logger.Log(String.Format("Error al realizar el Login Camera:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                return false;
            }
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

        public bool SetStartDateTime(DateTime dateTime, bool changeSlider = true, bool isVault = false)
        {
            m_PlayBack_StartTime = dateTime;
            StartTime = dateTime;
            initialTime = dateTime;
            actualTime = dateTime;

            var date = DateTime.Now - dateTime;

            m_PlayBack_EndTime = DateTime.Now.AddHours(MAX_HOUR_END);  //date.TotalMinutes > MAX_MINUTES ? dateTime.AddMinutes(MAX_MINUTES) : DateTime.Now.AddMinutes(-2);
            EndTime = m_PlayBack_EndTime;

            var tSecs = (m_PlayBack_EndTime - m_PlayBack_StartTime).TotalSeconds;
            slider.MaximumValue = (int)tSecs;

            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            if (_paused)
            {
                _paused = !_paused;
            }

            bool ret = OpenCamera(Camera.Channel);
            updateToSelectedSpeed();
            slider.Enabled = ret;

            return ret;
        }

        public bool SetStartUpSpeed(int speed)
        {
            throw new NotImplementedException();
        }

        public bool Slow()
        {
            if (playbackHandle == IntPtr.Zero)
            {
                return false;
            }

            long enSpeed = 0;
            if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYSPEED, ref enSpeed))
            {
                Logger.Log(string.Format("Get play speed {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                return false;
            }

            enSpeed--;

            if (enSpeed < (long)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_1_FORWARD && enSpeed > (long)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_1_BACKWARD)//5-8
            {
                enSpeed = (long)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_1_BACKWARD;
            }
            else if (enSpeed < (long)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_16_BACKWARD)
            {
                enSpeed++;
            }
            enSpeed = enSpeed == 0 ? 1 : enSpeed;
            if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_SETPLAYSPEED, ref enSpeed))
            {
                Logger.Log(string.Format("Set play speed {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                return false;
            }
            ShowPlaySpeed(-1);
            return true;
        }

        public bool Snapshot(string path)
        {
            byte[] picSavePath;
            GetUTF8Buffer(path, NETDEVSDK.NETDEV_LEN_260, out picSavePath);
            int iRet = NETDEVSDK.NETDEV_CapturePicture(playbackHandle, picSavePath, (int)NETDEV_PICTURE_FORMAT_E.NETDEV_PICTURE_BMP);
            if (NETDEVSDK.FALSE == iRet)
            {
                Logger.Log(string.Format("Error to Snapshot {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                return false;
            }
            return true;
        }

        public bool Stop()
        {
            if (playbackHandle == IntPtr.Zero)
            {
                return false;
            }
            else
            {
                if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_StopPlayBack(playbackHandle))
                {
                    Logger.Log(string.Format("Close playBack {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                    return false;
                }
                else
                {
                    playbackHandle = IntPtr.Zero;
                    return true;
                }
            }
        }

        public bool SyncSpeed(PlaySpeed masterSpeed, bool updateLabelSpeed)
        {
            bool result = true;
            if (this.CurrentSpeed == masterSpeed)
            {
                return result;
            }
            if (playbackHandle == IntPtr.Zero)
            {
                return false;
            }
            if (masterSpeed < CurrentSpeed)
            {
                while (CurrentSpeed != masterSpeed && result)
                {
                    long iOutValue = 0;
                    var pb = NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_SETPLAYSPEED, ref iOutValue);

                    if (pb == NETDEVSDK.TRUE)
                    {
                        CurrentSpeed = (PlaySpeed)((int)CurrentSpeed - 1);
                    }
                }
            }
            else
            {
                while (CurrentSpeed != masterSpeed && result)
                {
                    long iOutValue = 0;

                    if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYSPEED, ref iOutValue))
                    {
                        Logger.Log(string.Format("Get play speed {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                        return false;
                    }
                    else
                    {
                        iOutValue++;
                        var pb = NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_SETPLAYSPEED, ref iOutValue);

                        if (pb == NETDEVSDK.TRUE)
                        {
                            CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
                        }
                    }
                }

            }
            return result;
        }

        public void ToggleFullScreen()
        {
            try
            {
                var tempProfile = this.Profile;
                UniviewFullscreen fullscreen = new UniviewFullscreen();
                Task.Run(() =>
                {
                    Stop();
                    Profile = Profile.MainStream;
                    OpenCamera(Camera.Channel - 1);
                });
                fullscreen.ShowDialog();

                fullscreen.Dispose();
                Profile = tempProfile;
                OpenCamera(Camera.Channel - 1);
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
                    NETDEVSDK.NETDEV_OpenSound(playbackHandle); //call opensound function.
                }
                else
                {
                    NETDEVSDK.NETDEV_CloseSound(playbackHandle);
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
                this.BringToFront();
                this.Focus();
            }

            ZoomStatus = !ZoomStatus;
            return ZoomStatus;
        }

        public bool VideoClipStart(string path)
        {
            byte[] localRecordPath;
            GetUTF8Buffer(path, NETDEVSDK.NETDEV_LEN_260, out localRecordPath);
            int iRet = NETDEVSDK.NETDEV_SaveRealData(playbackHandle, localRecordPath, (int)NETDEV_MEDIA_FILE_FORMAT_E.NETDEV_MEDIA_FILE_MP4);
            if (NETDEVSDK.FALSE == iRet)
            {
                Logger.Log(string.Format("Error in start Record: {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool VideoClipStop()
        {
            int iRet = NETDEVSDK.NETDEV_StopSaveRealData(playbackHandle);
            if (NETDEVSDK.FALSE == iRet)
            {
                Logger.Log(string.Format("Error in stop Record: {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool Volume(int value)
        {
            throw new NotImplementedException();
        }

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

        private void ShowButtons()
        {
            this.ButtonSnapshot.Visible = appAuthorization.Exist(ButtonsContextBar.Snapshot.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonBookmark.Visible = appAuthorization.Exist(ButtonsContextBar.Bookmark.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonListen.Visible = (appAuthorization.Exist(ButtonsContextBar.Listen.GetAttribute<PermissionPlayback>().PermissionKey) && Camera.AudioEnabled);
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

        public new void Dispose()
        {
            if (!instantPlayback)
            {
                if (loginControl.RemoveChannelAndCanLogout(Camera, this, Common.Enum.Drivers.UNVNetSDK_231))
                {
                    Logger.Log(String.Format(" Dispose Logout uniview {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    NETDEVSDK.NETDEV_Logout(loginHandle);
                }
                NETDEVSDK.NETDEV_StopRealPlay(playbackHandle);
            }
            NETDEVSDK.NETDEV_CloseSound(playbackHandle);
            NETDEVSDK.NETDEV_StopPlayBack(playbackHandle);
            m_DispatcherTimer.Stop();
            this.MouseWheel -= Picture_MouseWheel;
            playbackHandle = IntPtr.Zero;
            loginHandle = IntPtr.Zero;
        }

        private bool updateToSelectedSpeed()
        {
            PlaySpeed temp = PlaySpeed.NORMAL;
            int result = NETDEVSDK.TRUE;


            while (result == NETDEVSDK.TRUE && temp != CurrentSpeed)
            {
                long iOutValue = 0;
                if (CurrentSpeed >= PlaySpeed.MIN && CurrentSpeed < PlaySpeed.NORMAL)
                {
                    result = NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYSPEED, ref iOutValue);
                    if (result == NETDEVSDK.TRUE)
                    {
                        iOutValue--;
                        iOutValue = iOutValue <= 0 ? 1 : iOutValue;
                        if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_SETPLAYSPEED, ref iOutValue))
                        {
                            Logger.Log(string.Format("NETDEV_PlayBackControl {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                        }
                        temp = (PlaySpeed)((int)temp - 1);
                    }

                }
                else
                {
                    result = NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYSPEED, ref iOutValue);
                    if (result == NETDEVSDK.TRUE)
                    {
                        iOutValue++;
                        if (iOutValue < 12)
                        {
                            if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_SETPLAYSPEED, ref iOutValue))
                            {
                                Logger.Log(string.Format("NETDEV_PlayBackControl {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                            }
                        }

                        temp = (PlaySpeed)((int)temp + 1);
                    }

                }
            }

            return result == NETDEVSDK.TRUE;
        }

        private bool OpenCamera(int channel)
        {
            if (loginHandle == IntPtr.Zero)
            {
                return false;
            }

            //stop play
            if (playbackHandle != IntPtr.Zero)
            {
                Stop();
            }
            try
            {
                NETDEV_PLAYBACKCOND_S playBackByTimeInfo = new NETDEV_PLAYBACKCOND_S();

                String beginDateTimeStr = getInputStartDataTime();
                String endDateTimeStr = getInputEndDataTime();

                playBackByTimeInfo.tBeginTime = this.getLongTime(beginDateTimeStr);
                playBackByTimeInfo.tEndTime = this.getLongTime(endDateTimeStr);

                playBackByTimeInfo.dwChannelID = channel;
                playBackByTimeInfo.dwLinkMode = (int)NETDEV_PROTOCAL_E.NETDEV_TRANSPROTOCAL_RTPTCP;
                playBackByTimeInfo.dwPlaySpeed = (int)NETDEV_VOD_PLAY_STATUS_E.NETDEV_PLAY_STATUS_1_FORWARD;
                playBackByTimeInfo.hPlayWnd = GetHandle();

                StartTime = m_PlayBack_StartTime;
                EndTime = m_PlayBack_EndTime;
                playbackHandle = NETDEVSDK.NETDEV_PlayBackByTime(loginHandle, ref playBackByTimeInfo);
                if (playbackHandle == IntPtr.Zero)
                {
                    Logger.Log(string.Format("Error Playback by time {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                    this.offLine = true;
                    return false;
                }
                else
                {
                    this.offLine = false;
                }
            }
            catch (ObjectDisposedException oex)
            {
                Logger.Log(string.Format("Closed windows Playback, reconnect ending {0}", oex), LogPriority.Fatal);
                //Se terminan los reintentos
                tryCount = tryLimit;
                this.offLine = true;
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                this.offLine = true;
                return false;
            }
            if (playbackHandle != IntPtr.Zero)
            {
                OnVideo?.Invoke(true, this);
                m_DispatcherTimer.Start();
                this.offLine = false;
                return true;
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

            if (this.panelNoConnection.InvokeRequired)
            {
                panelNoConnection.Invoke((MethodInvoker)delegate
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
                        panelNoConnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_es);
                        }
                        else
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_en);
                        }
                        // this.CustomDispose(false);
                        break;
                    case PlaybackConnectionState.Connected:
                        panelNoConnection.BringToFront();
                        panelNoConnection.Visible = false;
                        panelNoConnection.BackgroundImage = null;
                        break;
                    case PlaybackConnectionState.NoRecording:
                        panelNoConnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_es);
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
            }
            catch (Exception ex)
            {
                Logger.Log($"setVisivility Exception: {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        private bool Login()
        {
            try
            {
                if (loginControl.GetDeviceLogin(Camera, out loginHandle, Common.Enum.Drivers.UNVNetSDK_231, this))
                {
                    loginControl.AddChannel(Camera, this, Common.Enum.Drivers.UNVNetSDK_231);
                    Logger.Log($"Reusing a Uniview Login to Camara {Camera.Name}", LogPriority.Information);
                    return true;
                }
                NETDEV_DEVICE_LOGIN_INFO_S pstDevLoginInfo = new NETDEV_DEVICE_LOGIN_INFO_S();
                NETDEV_SELOG_INFO_S pstSELogInfo = new NETDEV_SELOG_INFO_S();
                pstDevLoginInfo.szIPAddr = Camera.Host;
                try
                {
                    pstDevLoginInfo.dwPort = Convert.ToInt32(Camera.HttpPort);
                }
                catch (Exception ex)
                {
                    notification.Show("ID_PORTERROR", null);
                    Logger.Log($"ID_PORTERROR port: {Camera.HttpPort} Exception: {ex.Message}", LogPriority.Fatal);
                    return false;
                }

                pstDevLoginInfo.szUserName = Camera.User;
                pstDevLoginInfo.szPassword = Camera.Password;
                pstDevLoginInfo.dwLoginProto = (int)NETDEV_LOGIN_PROTO_E.NETDEV_LOGIN_PROTO_ONVIF;

                loginHandle = NETDEVSDK.NETDEV_Login_V30(ref pstDevLoginInfo, ref pstSELogInfo);

                if (loginHandle == IntPtr.Zero)
                {
                    Logger.Log("Device Login is Zero " + NETDEVSDK.NETDEV_GetLastError(), LogPriority.Fatal);
                    this.offLine = true;
                    return false;
                }
                else
                {
                    Logger.Log(String.Format("New Uniview Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                    loginControl.AddDevice(Camera, (IntPtr)loginHandle, this, Common.Enum.Drivers.UNVNetSDK_231);
                    this.offLine = false;
                    return true;
                }
            }
            catch (Exception ex)
            {
                notification.Show(ex.Message, null);
                Logger.Log(String.Format("Login Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
                return false;
            }

        }

        private long getLongTime(String strTime)
        {
            DateTime dateTime = Convert.ToDateTime(strTime).AddHours(-1);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(dateTime - startTime).TotalSeconds;
        }

        private string getInputStartDataTime()
        {
            String beginDateTimeStr = this.m_PlayBack_StartTime.Year.ToString();
            beginDateTimeStr += ("-" + this.m_PlayBack_StartTime.Month.ToString());
            beginDateTimeStr += ("-" + this.m_PlayBack_StartTime.Day.ToString());

            beginDateTimeStr += (" " + this.m_PlayBack_StartTime.Hour.ToString());
            beginDateTimeStr += (":" + this.m_PlayBack_StartTime.Minute.ToString());
            beginDateTimeStr += (":" + this.m_PlayBack_StartTime.Second.ToString());

            return beginDateTimeStr;
        }

        private string getInputEndDataTime()
        {
            String endDateTimeStr = this.m_PlayBack_EndTime.Year.ToString();
            endDateTimeStr += ("-" + this.m_PlayBack_EndTime.Month.ToString());
            endDateTimeStr += ("-" + this.m_PlayBack_EndTime.Day.ToString());

            endDateTimeStr += (" " + this.m_PlayBack_EndTime.Hour.ToString());
            endDateTimeStr += (":" + this.m_PlayBack_EndTime.Minute.ToString());
            endDateTimeStr += (":" + this.m_PlayBack_EndTime.Second.ToString());
            return endDateTimeStr;
        }

        private IntPtr GetHandle()
        {
            return picture.Handle;
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

                long iPlayTime = 0;
                int iRet = NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYTIME, ref iPlayTime);
                if (NETDEVSDK.TRUE == iRet)
                {
                    actualTime = Convert.ToDateTime(getStrTime(iPlayTime)).AddHours(1);
                    OnTimeChanged?.Invoke(actualTime, this);

                    var sliderValue = (int)(actualTime - initialTime).TotalSeconds;
                    if (sliderValue <= slider.MaximumValue)
                    {
                        slider.Value = sliderValue;
                    }
                }
            }
            catch { }
        }

        private string getStrTime(long time)
        {
            DateTime startDateTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return startDateTime.AddSeconds(time).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public void GetUTF8Buffer(string inputString, int bufferLen, out byte[] utf8Buffer)
        {
            utf8Buffer = new byte[bufferLen];
            byte[] tempBuffer = Encoding.UTF8.GetBytes(inputString);
            for (int i = 0; i < tempBuffer.Length; ++i)
            {
                utf8Buffer[i] = tempBuffer[i];
            }
        }

        private void TryToReConnect()
        {
            try
            {
                if (tryCount >= tryLimit)
                {
                    SetVisivility(PlaybackConnectionState.Disconnected);
                    Logger.Log(String.Format("Uniview TryToReConnect reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    return;
                }

                tryCount++;
                if (OpenCamera(Camera.Channel))
                {
                    Logger.Log(String.Format(" TryToReConnect RealStar Connected  {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    tryCount = 0;
                }
                else
                {
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                    Logger.Log(String.Format(" TryToReConnect RealStar failed  {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    Task.Delay(r).ContinueWith(t => TryToReConnect());

                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format(" TryToReConnect Exception: {4}  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex.Message), LogPriority.Fatal);
            }
        }

        public void Disconect(IntPtr HandledBrocaster)
        {
            throw new NotImplementedException();
        }

        public void Connect(IntPtr HandledBrocaster)
        {
            throw new NotImplementedException();
        }

        public int Hash()
        {
            //return string.Format("{0}-{1}-{2}", ElementId, RecorderType, RecorderId).GetHashCode();
            return string.Format("{0}-{1}-{2}", Camera.Id, Recorder.RecorderType, Recorder.Id).GetHashCode();
        }

        public bool PlayVideo()
        {
            return Play();
        }

        public bool PlayNoAsync()
        {
            return Play();
        }

        private void UniviewInstantPlaybackUserControl_Resize(object sender, EventArgs e)
        {
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                Reconnecting.DisplayLogo(this.Width, this.Height, ref panelNoConnection, ref picture);
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                /*---------------------------*/
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
                if (main.Width >= 1300 && main.Width <= 1400)
                {
                    var btnRewSecsLocationX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.8550), 2));
                    var btnRewSecsLocationY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0168M), 2 + 3));

                    ButtonRewSecs.Location = new Point(btnRewSecsLocationX, btnRewSecsLocationY);
                }
            }
        }
        public void SelectSpeed(PlaySpeed speed)
        {
            PlaySpeed temp = CurrentSpeed;
            int result = NETDEVSDK.TRUE;

            while (result == NETDEVSDK.TRUE && temp != speed)
            {
                long iOutValue = 0;
                if (temp >= PlaySpeed.MIN && temp < PlaySpeed.NORMAL)
                {
                    result = NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYSPEED, ref iOutValue);
                    if (result == NETDEVSDK.TRUE)
                    {
                        iOutValue++;
                        if (iOutValue < 12)
                        {
                            if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_SETPLAYSPEED, ref iOutValue))
                            {
                                Logger.Log(string.Format("NETDEV_PlayBackControl {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                            }
                        }

                        temp = (PlaySpeed)((int)temp + 1);
                    }
                }
                else
                {
                    result = NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_GETPLAYSPEED, ref iOutValue);
                    if (result == NETDEVSDK.TRUE)
                    {
                        iOutValue--;
                        iOutValue = iOutValue <= 0 ? 1 : iOutValue;
                        if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_PlayBackControl(playbackHandle, (int)NETDEV_VOD_PLAY_CTRL_E.NETDEV_PLAY_CTRL_SETPLAYSPEED, ref iOutValue))
                        {
                            Logger.Log(string.Format("NETDEV_PlayBackControl {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                        }
                        temp = (PlaySpeed)((int)temp - 1);
                    }

                }
            }
            CurrentSpeed = temp;
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
