using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers.Vrec.Player;
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
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Elipgo.SmartClient.Drivers.VRec
{
    public partial class VRecInstantPlaybackUserControl : UserControl, IDriverInstantPlayback, IDisposable
    {
        #region Constants
        private bool _secondScale;
        private PlayScaleTimeLine _currentScale;
        private int _previewSliderValue;
        private int _sliderCurrentValue;
        private bool _scaleActive;
        private int _currentBlock;
        private int _additionalSeconds;
        private DateTime _selectedDateTime;
        private const int MAX_MINUTES = 360;

        #endregion

        #region Attributes

        private decimal playbackRate = 1;
        private bool painted = false;
        private PlayerControl4 player;
        private PlaybackState state = PlaybackState.Stopped;
        private PlaySpeed CurrentSpeed = PlaySpeed.NORMAL;
        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        private bool offLine = true;
        private bool instantPlayback = true;
        private DispatcherTimer m_DispatcherTimer = new DispatcherTimer();
        private readonly Configuration _config;
        public event OnDriverDispose OnDispose;
        private DateTime? endTimeVault = null;
        private bool _listenStatus = false;

        #endregion

        #region Events

        public event OnVideoEventHandler OnVideo;
        public event OnTimeChangedEventHandler OnTimeChanged;
        public event OnStateChangedEventHandler OnStateChanged;
        public event EventHandler<bool> OpenBookmark;
        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;

        #endregion

        #region Properties

        public bool ClipStatus { get; set; } = false;
        public bool BookmarkState { get; set; }
        public DateTime StartTime { get; set; }
        private DateTime InitDateTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ActualTime { get; set; }
        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }
        public ulong MediaDuration { get; set; }
        public List<ButtonsContextBar> Commands => GetButtons();
        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();
        public bool ZoomStatus { get; set; }
        private readonly Random _random = new Random();
        private double TimeReConnectionCheck;
        private int tryCount = 0;
        private readonly int tryLimit;
        private RecorderDTOSmall Recorder;

        #endregion

        #region Constructors

        public VRecInstantPlaybackUserControl(CameraDTO camera, Profile profile, bool hideControls, DateTime selectedDateTime, RecorderDTOSmall recorder, DateTime? selectedEndDateTime = null)
        {
            try
            {
                InitializeComponent();

                _config = SmartClientEnvironmentUtils.GetConfiguration();
                Camera = camera;
                Profile = profile;
                BookmarkState = false;
                ClipStatus = false;
                Recorder = recorder;

                selectedDateTime = selectedDateTime.ToUniversalTime().AddMinutes(Camera.Gmt);

                if (selectedEndDateTime != null)
                {
                    StartTime = selectedDateTime;
                    InitDateTime = selectedDateTime;
                    EndTime = (DateTime)selectedEndDateTime;
                    instantPlayback = false;
                }
                else
                {
                    StartTime = selectedDateTime.AddMinutes(-1);
                    InitDateTime = selectedDateTime.AddMinutes(-1 * MAX_MINUTES);
                    EndTime = selectedDateTime.AddMinutes(MAX_MINUTES);
                }

                this.Paint += VRecInstantPlaybackUserControl_Paint;
                this.SliderTooltip.MouseDown += Slider_MouseDown;
                this.slider.MouseUp += Slider_MouseUp;
                this.slider.ValueChanged += Slider_ValueChanged;
                this.slider.ValueChangeComplete += Slider_ValueChangeComplete;
                this.slider.MaximumValue = (int)(EndTime - InitDateTime).TotalSeconds;
                this.Resize += DahuaInstantPlaybackUserControl_Resize;
                this.Click += VRecInstantPlaybackUserControl_Click;
                this.DoubleClick += VRecInstantPlaybackUserControl_DoubleClick;
                ButtonBookmark.Click += ButtonBookmark_Click;

                if (hideControls)
                {
                    this.PanelControls.Hide();
                    this.slider.Hide();
                    this.PanelVideo.Dock = DockStyle.Fill;
                    instantPlayback = false;
                }

                CultureInfo ci = CultureInfo.InstalledUICulture;
                bunifuToolTip1.SetToolTip(this.ButtonBookmark, ci.Name.Contains("es") ? ButtonsContextBar.Bookmark.GetDescription() : ButtonsContextBar.Bookmark.GetAttribute<DescriptionEN>().Descripcion);
                bunifuToolTip1.SetToolTip(this.ButtonSnapshot, ci.Name.Contains("es") ? ButtonsContextBar.Snapshot.GetDescription() : ButtonsContextBar.Snapshot.GetAttribute<DescriptionEN>().Descripcion);
                bunifuToolTip1.SetToolTip(this.ButtonFullScreen, ci.Name.Contains("es") ? ButtonsContextBar.Fullscreen.GetDescription() : ButtonsContextBar.Fullscreen.GetAttribute<DescriptionEN>().Descripcion);
                bunifuToolTip1.SetToolTip(this.ButtonPlay, ci.Name.Contains("es") ? ButtonsContextBar.Play.GetDescription() : ButtonsContextBar.Play.GetAttribute<DescriptionEN>().Descripcion);
                bunifuToolTip1.SetToolTip(this.ButtonFwdSecs, ci.Name.Contains("es") ? ButtonsContextBar.FwdSecs.GetDescription() : ButtonsContextBar.FwdSecs.GetAttribute<DescriptionEN>().Descripcion);
                bunifuToolTip1.SetToolTip(this.ButtonRewSecs, ci.Name.Contains("es") ? ButtonsContextBar.RewSecs.GetDescription() : ButtonsContextBar.RewSecs.GetAttribute<DescriptionEN>().Descripcion);
                bunifuToolTip1.SetToolTip(this.ButtonPause, ci.Name.Contains("es") ? ButtonsContextBar.Pause.GetDescription() : ButtonsContextBar.Pause.GetAttribute<DescriptionEN>().Descripcion);
                bunifuToolTip1.SetToolTip(this.ButtonListen, ci.Name.Contains("es") ? ButtonsContextBar.Listen.GetDescription() : ButtonsContextBar.Listen.GetAttribute<DescriptionEN>().Descripcion);

                tryLimit = int.Parse(_config.AppSettings.Settings["tryLimit"].Value);
                ShowButtons();
                InitializePanelVideo();
                this.TimeReConnectionCheck = 5;
                //PlayCamera();
                SetVisivility(VRecLastOperationResult.Connecting);
                this.MouseWheel += Picture_MouseWheel;
            }
            catch (Exception)
            {
                panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_es);
                panelNoConnection.Visible = true;
            }
        }

        private void VRecInstantPlaybackUserControl_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (CameraSelectedDoubleClick != null)
                {
                    CameraSelectedDoubleClick?.Invoke(this);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        private void DahuaInstantPlaybackUserControl_Resize(object sender, EventArgs e)
        {
            try
            {
                if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
                {
                    Form instantPlayerView = Application.OpenForms.OfType<Form>().FirstOrDefault(f => f.Name == "InstantPlayerView" && (string)f.Tag == Camera.IdGuid);
                    var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                    var sliderHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.028M), 2));
                    var sliderLocationY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.304M), 2));

                    slider.Height = sliderHeight;
                    slider.BringToFront();
                    slider.Location = new Point(0, 350); //new Point(0, sliderLocationY);


                    if (instantPlayerView != null && instantPlayerView.WindowState == FormWindowState.Maximized)
                    {
                        this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.745M), 2));
                        if (main.Width >= 1366 && main.Width < 1400)
                        {
                            this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.705M), 2));
                        }
                    }
                    else if (instantPlayerView != null && instantPlayerView.WindowState == FormWindowState.Normal)
                    {
                        var panelControlsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.371M), 2));
                        var panelControlsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.047M), 2));
                        this.PanelControls.Size = new Size(panelControlsWidth, panelControlsHeight);

                        var panelVideoWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.371M), 2));
                        var panelVideoHeigth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.325M), 2));
                        if (main.Width >= 1366 && main.Width < 1700)
                        {
                            panelVideoHeigth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3194M), 2));
                        }

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


                    var ButtonZoomX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.8527), 2));
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

                    ButtonSnapshot.Size = new Size(btn, btn);
                    ButtonSnapshot.Location = new Point(ButtonSnapshotX, ButtonSnapshotY);

                    var ButtonListenX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.856), 2));
                    var ButtonListenY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                    ButtonListen.Size = new Size(btn, btn);
                    ButtonListen.Location = new Point(ButtonListenX, ButtonListenY);

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
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        #endregion

        #region Private Methods

        private List<ButtonsContextBar> GetButtonsAudioPtz()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>();

            if (Camera.AudioEnabled)
            {
                commands.Add(ButtonsContextBar.Listen);
            }

            return commands;
        }

        private void ShowButtons()
        {
            this.ButtonSnapshot.Visible = appAuthorization.Exist(ButtonsContextBar.Snapshot.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonBookmark.Visible = appAuthorization.Exist(ButtonsContextBar.Bookmark.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonListen.Visible = appAuthorization.Exist(ButtonsContextBar.Listen.GetAttribute<PermissionPlayback>().PermissionKey) && Camera.AudioEnabled;
        }

        private void SetVisivility(VRecLastOperationResult connectionState)
        {

            try
            {
                CultureInfo ci = CultureInfo.InstalledUICulture;
                if (this.PanelVideo.InvokeRequired)
                {

                    panelNoConnection.Invoke((MethodInvoker)delegate
                    {
                        SetVisivility(connectionState);
                    });
                    return;
                }
                panelNoConnection.Visible = this.offLine;
                switch (connectionState)
                {
                    case VRecLastOperationResult.Success:
                        Logger.Log($"Vrec Success  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                        break;
                    case VRecLastOperationResult.VRecErrorInexperado:
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_en);
                        }
                        Logger.Log($"Vrec VRecErrorInexperado  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                        break;
                    case VRecLastOperationResult.VRecMaximumClientNumbers:
                        Logger.Log($"Vrec VRecMaximumClientNumbers  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_en);
                        }
                        break;
                    case VRecLastOperationResult.VRecVideoEmpty:
                        Logger.Log($"Vrec VRecVideoEmpty  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_en);
                        }
                        break;
                    case VRecLastOperationResult.VRecVideoNotExists:
                        Logger.Log($"Vrec Play VRecVideoNotExists  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_en);
                        }
                        break;
                    case VRecLastOperationResult.Disconnect:
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_en);
                        }
                        Logger.Log($"Vrec Disconnect  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                        break;
                    case VRecLastOperationResult.Connecting:
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_en);
                        }
                        Logger.Log($"Vrec Connecting  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                        break;
                }
                PictureBox pic = new PictureBox();
                Reconnecting.DisplayLogo(PanelVideo.Width, PanelVideo.Height, ref panelNoConnection, ref pic);
            }
            catch (Exception ex)
            {
                Logger.Log($"setVisivility Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        private void VRecError(string errorMsg)
        {
            try
            {
                if (errorMsg.ToLower() != "No es posible conectar con el servidor remoto".ToLower())
                {
                    this.offLine = true;
                    panelNoConnection.Visible = this.offLine;
                    SetVisivility(player.LastError);
                    int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                    Logger.Log(String.Format("VRecError failed, start reconnection   {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, tryCount, tryLimit), LogPriority.Information);
                    Task.Delay(r).ContinueWith(t => PlayCamera());
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        private void PlayCamera()
        {
            try
            {
                if (tryCount < tryLimit)
                {
                    var rec = Camera.Recorders.Where(x => x.Id == Recorder.Id).First();
                    player.ServerUrl = string.Concat(rec.HttpProtocol + "://", rec.Host, ":", rec.HttpPort, "/WSNDVR/PlayerService.asmx");
                    player.SetPortTransfer(rec.VideoPort);
                    bool ok = player.Open(this.Camera, this.StartTime.ToString("yyyyMMddHHmmss"));

                    if (ok)
                    {
                        player.Start();
                        this.offLine = false;
                        tryCount = 0;
                        SetVisivility(player.LastError);
                        OnVideo?.Invoke(true, this);
                        m_DispatcherTimer.Start();
                        Logger.Log(String.Format(" PlayCamera connected {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, tryCount, tryLimit), LogPriority.Information);

                    }
                    else
                    {

                        notification.Show(player.GetErrorMsg(), null);
                        this.offLine = true;
                        Logger.Log(String.Format("Error al realizar el Login Camera:  {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, player.GetErrorMsg()), LogPriority.Fatal);
                        SetVisivility(player.LastError);
                        if (player.LastError == VRecLastOperationResult.VRecMaximumClientNumbers || player.LastError == VRecLastOperationResult.VRecErrorInexperado)
                        {
                            if (tryCount < tryLimit)
                            {
                                tryCount++;
                                int r = (int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000);
                                Logger.Log(String.Format("Vrec PlayCamera failed, current count {4} of {5}   {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, tryCount, tryLimit), LogPriority.Information);
                                Task.Delay(r).ContinueWith(t =>
                                {
                                    try
                                    {
                                        PlayCamera();
                                    }
                                    catch (Exception ex)
                                    {
                                        Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
                                    }
                                });
                            }
                            else
                            {
                                SetVisivility(VRecLastOperationResult.Disconnect);
                                Logger.Log(String.Format("VRec PlayCamera reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    SetVisivility(VRecLastOperationResult.Disconnect);
                    Logger.Log(String.Format("VRec PlayCamera reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("VRec PlayCamera exception {4}, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Information);
            }
        }

        private void InitializePanelVideo()
        {
            try
            {
                player = new PlayerControl4();

                player.Parent = this.PanelVideo;
                player.Dock = DockStyle.Fill;
                player.PlayerSelected += Player_PlayerSelected;
                player.ErrorHandler += VRecError;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        private void Player_PlayerSelected(object sender, EventArgs e)
        {
            try
            {
                CameraSelected?.Invoke(this, Camera);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
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

        private List<ButtonsContextBar> GetButtons()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>();
            commands.Add(ButtonsContextBar.Snapshot);
            commands.Add(ButtonsContextBar.Bookmark);
            commands.Add(ButtonsContextBar.Videoclip);
            commands.Add(ButtonsContextBar.Fullscreen);
            return commands;
        }

        private void ChangePlaySpeed(int value)
        {
            try
            {
                playbackRate += value;
                player.SetSkipRate((int)playbackRate);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        private void ShowPlaySpeed(int nMode)
        {
            try
            {
                if (nMode == 0)
                {
                    CurrentSpeed = PlaySpeed.NORMAL;
                }
                else if (nMode > 0) // Speed up
                {
                    if (CurrentSpeed >= PlaySpeed.MIN && CurrentSpeed < PlaySpeed.MAX)
                    {
                        CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
                    }
                }
                else if (nMode < 0) // Speed down
                {
                    if (CurrentSpeed > PlaySpeed.MIN && CurrentSpeed <= PlaySpeed.MAX)
                    {
                        CurrentSpeed = (PlaySpeed)((int)CurrentSpeed - 1);
                    }
                }

                UpdateControlsToShowSpeed();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        private void UpdateControlsToShowSpeed()
        {
            try
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
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        #endregion

        #region Implement Events

        private void VRecInstantPlaybackUserControl_Click(object sender, EventArgs e)
        {
            CameraSelected?.Invoke(this, Camera);
        }

        private void VRecInstantPlaybackUserControl_Load(object sender, EventArgs e)
        {
            DispatcherTimerInit();
        }

        private void VRecInstantPlaybackUserControl_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (this.painted)
                {
                    return;
                }

                this.painted = true;

                ButtonPlay.Image = FileResources.icon_play;
                ButtonPause.Image = FileResources.icon_pause;
                ButtonRewSecs.Image = FileResources.icon_replay_30;
                ButtonFwdSecs.Image = FileResources.icon_forward_30;
                ButtonBookmark.Image = FileResources.icon_bookmarks;
                ButtonFullScreen.Image = FileResources.icon_full_screen;
                ButtonSnapshot.Image = FileResources.icon_snapshot;
                ButtonFast.Image = FileResources.icon_fast;
                ButtonSlow.Image = FileResources.icon_slow;
                ButtonZoom.Image = FileResources.icon_digital_zoom_off;
                ButtonListen.Image = FileResources.icon_sound_off;
                LabelSpeed.Text = "1X";
                LabelSpeed.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                this.SizeChanged += VRecInstantPlaybackUserControl_SizeChanged;

                //PlayCamera();
                if (instantPlayback)
                {
                    slider.Value = (int)(InitDateTime.AddMinutes(MAX_MINUTES - 1) - InitDateTime).TotalSeconds;
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        private void VRecInstantPlaybackUserControl_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                //le resto -12 para que se ubique dentro del panel video
                this.slider.Location = new Point(this.PanelVideo.Location.X, this.PanelVideo.Location.Y + this.PanelVideo.Height);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        private void ButtonBookmark_Click(object sender, EventArgs e)
        {
            try
            {
                BookmarkState = !BookmarkState;
                //if (BookmarkState)
                //    ActualTime = StartTime.AddTicks((new DateTime((long)amc.CurrentPosition64 * 10000)).Ticks);

                OpenBookmark?.Invoke(this, BookmarkState);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                SliderTooltip.Text = this.InitDateTime.AddSeconds(slider.Value).ToString("MM/dd HH:mm:ss");

                SliderTooltip.Location = new Point(slider.Location.X, slider.Location.Y - 20);
                SliderTooltip.Visible = true;
                SliderTooltip.BringToFront();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        private void Slider_MouseUp(object sender, MouseEventArgs e)
        {
            Slider_ValueChanged(sender, e);
        }

        private void Slider_MouseDown(object sender, MouseEventArgs e)
        {
            Slider_ValueChanged(sender, e);
        }

        private void Slider_ValueChangeComplete(object sender, EventArgs e)
        {
            try
            {
                if (player.IsConnected())
                {
                    player.Seek(this.InitDateTime.AddSeconds(slider.Value).ToString("yyyyMMddHHmmss"));
                }
                else
                {
                    this.StartTime = this.InitDateTime.AddSeconds(slider.Value);
                    PlayCamera();
                }

                SliderTooltip.Visible = false;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            try
            {
                Play();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        private void ButtonFast_Click(object sender, EventArgs e)
        {
            Fast();
        }

        private void ButtonSlow_Click(object sender, EventArgs e)
        {
            Slow();
        }

        private void ButtonRewSecs_Click(object sender, EventArgs e)
        {
            try
            {
                Jump(30, false);
                slider.Value -= 30;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        private void ButtonFwdSecs_Click(object sender, EventArgs e)
        {
            try
            {
                Jump(30, true);
                slider.Value += 30;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        private void ButtonSnapshot_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        private void DispatcherTimerInit()
        {
            try
            {
                m_DispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
                m_DispatcherTimer.Tick += DispatcherTimer_Tick;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (player.IsConnected() && player.GetVideoDateTime() != "00010101000000")
                {

                    if (endTimeVault != null && player.GetTimestamp() >= endTimeVault)
                    {
                        player.Stop();
                    }

                    this.ActualTime = player.GetTimestamp();
                    OnTimeChanged?.Invoke(this.ActualTime, this);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        private void ButtonListen_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonListen.Image = _listenStatus ? FileResources.icon_sound_off : FileResources.icon_sound_on;
                ToggleListen();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
        }

        #endregion

        #region Public Methods

        public new void Dispose()
        {
            if (!player.IsDisposed)
            {
                player.Dispose();
                player.PlayerSelected -= Player_PlayerSelected;
                m_DispatcherTimer.Tick -= DispatcherTimer_Tick;
                m_DispatcherTimer.Stop();
            }
        }

        public bool Snapshot(string path)
        {
            try
            {
                player.Snapshot(path);
                notification.Show(Resources.SnapshotSaved, () => Process.Start(Common.Properties.Settings.Default["DefaultLocation"].ToString() + "\\Snapshot"));
                return true;
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
                //amc.FullScreen = !amc.FullScreen;

            }
            catch (Exception ex)
            {
                notification.Show(ex.Data.ToString(), null);
            }
        }

        public bool VideoClipStart(string path)
        {
            try
            {
                path = path.Replace(".mp4", ".asf");
                ClipStatus = true;
                return true;
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Data.ToString()), null);
                ClipStatus = false;
                return false;
            }
        }

        public bool VideoClipStop()
        {
            try
            {
                //amc.StopRecordMedia();
                ClipStatus = false;
                return true;
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Data.ToString()), null);
                ClipStatus = false;
                return false;
            }
        }

        public bool ToggleListen()
        {
            try
            {
                if (!_listenStatus)
                {
                    player.OpenSound();
                }
                else
                {
                    player.CloseSound();
                }

                _listenStatus = !_listenStatus;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }


            return true;
        }

        public bool Volume(int value)
        {
            //throw new NotImplementedException();
            return true;
        }


        public bool Play()
        {
            try
            {
                if (player.IsConnected())
                {
                    player.Start();
                    this.offLine = false;
                }
                else
                {

                    PlayCamera();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            return true;
            //bool bresult = true;
            //if (player.isConnected())
            //{
            //    player.start();
            //    this.offLine = false;
            //}
            //else
            //{
            //    String dateTime = player.getVideoDateTime();
            //    int offset = player.getTimeOffset();

            //    bool ok = player.open(player.getCameraId(), dateTime);

            //    if (ok)
            //    {
            //        player.start();
            //        player.setTimeOffset(offset);
            //        this.offLine = false;
            //        setVisivility(player.LastError);
            //        Logger.Log(String.Format(" Play conected {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, tryCount, tryLimit), LogPriority.Information);
            //    }
            //    else
            //    {
            //        notification.Show(player.getErrorMsg(), null);
            //        this.offLine = true;
            //        Logger.Log(String.Format("Error al realizar el Login Camera:  {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, player.getErrorMsg()), LogPriority.Fatal);

            //        if (player.LastError == VRecLastOperationResult.VRecMaximumClientNumbers || player.LastError == VRecLastOperationResult.VRecErrorInexperado)
            //        {
            //            setVisivility(player.LastError);
            //            if (tryCount < tryLimit)
            //            {
            //                tryCount++;
            //                int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
            //                Logger.Log(String.Format("Vrec Play failed, current count {4} of {5}   {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, tryCount, tryLimit), LogPriority.Information);
            //                Task.Delay(r).ContinueWith(t => bresult = Play());
            //            }
            //            else
            //            {
            //                setVisivility(VRecLastOperationResult.Disconnect); 
            //                Logger.Log(String.Format("VRec reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
            //                return false;
            //            }
            //        }
            //    }
            //}
            //return bresult;
        }

        public bool Stop()
        {
            player.Stop();

            return true;
        }

        public bool Pause()
        {
            player.Pause();

            return true;
        }

        public bool SetStartDateTime(DateTime dateTime, bool changeSlider = true, bool isVault = false)
        {
            try
            {
                StartTime = dateTime;
                InitDateTime = dateTime;

                player.Seek(dateTime.ToString("yyyyMMddHHmmss"));

                ButtonPlay.Image = FileResources.icon_play;
                ButtonPause.Image = FileResources.icon_pause;

                if (state == PlaybackState.Paused)
                {
                    state = PlaybackState.Playing;
                }

                //PlayCamera();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }


            return true;
        }

        public bool Rewind()
        {
            player.SetTimeOffset(player.GetTimeOffset() - 60);

            return true;
        }

        public bool Slow()
        {
            ChangePlaySpeed(-1);
            ShowPlaySpeed(-1);

            return true;
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

                if (masterSpeed < CurrentSpeed)
                {
                    while (CurrentSpeed != masterSpeed)
                    {
                        ChangePlaySpeed(-1);
                        CurrentSpeed = (PlaySpeed)((int)CurrentSpeed - 1);
                    }
                }
                else
                {
                    while (CurrentSpeed != masterSpeed)
                    {
                        ChangePlaySpeed(1);
                        CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
                    }

                }
                if (updateLabelSpeed == true)
                {//actualizo el label de velocidad
                    UpdateControlsToShowSpeed();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            return result;
        }

        public bool Fast()
        {
            try
            {
                ChangePlaySpeed(1);
                ShowPlaySpeed(1);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

            return true;
        }

        public bool SetStartUpSpeed(int speed)
        {
            try
            {
                Pause();
                CapacityNotAvailable(true);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

            return false;
        }

        public bool CapacityNotAvailable(bool show)
        {
            try
            {
                this.LabelSpeed.Text = Resources.CapacityNotAvailable;
                this.LabelSpeed.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
                this.LabelSpeed.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                this.LabelSpeed.Left = (this.LabelSpeed.Parent.Width - this.LabelSpeed.Width) / 2;
                this.LabelSpeed.Top = (this.LabelSpeed.Parent.Height - this.LabelSpeed.Height) / 2;

                this.LabelSpeed.Visible = show;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

            return true;
        }

        public bool Jump(int sec, bool asc)
        {
            try
            {
                player.SetTimeOffset(player.GetTimeOffset() + (asc ? sec : (-1) * sec));

                var currentDateTime = StartTime.AddMilliseconds(player.GetTimeOffset());
                OnTimeChanged?.Invoke(currentDateTime, this);
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }


            return true;
        }

        public bool ToogleDigitalZoom()
        {
            try
            {
                ButtonZoom.Image = FileResources.icon_digital_zoom_on;

                if (ZoomStatus)
                {
                    PanelVideo.Size = PanelVideo.Parent.Size;
                    PanelVideo.Location = new Point(0, 0);
                    PanelVideo.Visible = true;
                    ButtonZoom.Image = FileResources.icon_digital_zoom_off;
                    this.Cursor = Cursors.Default;
                    player.ZoomMode = ZoomModeEnum.Automatic;
                }
                else
                {
                    this.Cursor = Cursors.Cross;
                }

                ZoomStatus = !ZoomStatus;
                player.ZoomStatus = ZoomStatus;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }
            return ZoomStatus;
        }

        public PlaySpeed GetCurrentSpeed()
        {
            return this.CurrentSpeed;
        }

        public List<ButtonsPlayBackBar> ButtonsNotAllowed()
        {
            return new List<ButtonsPlayBackBar>();
        }

        public int Hash()
        {
            return string.Format("{0}-{1}-{2}", Camera.Id, Recorder.RecorderType, Recorder.Id).GetHashCode();
        }
        #endregion

        public bool PlayVideo()
        {
            return Play();
        }

        public bool PlayNoAsync()
        {
            return Play();
        }

        private void ButtonZoom_Click(object sender, EventArgs e)
        {
            ToogleDigitalZoom();
        }

        private int _actualSize = 0;
        private int _zoomLimit = int.Parse(Settings.Default["ZoomLimit"].ToString());
        private void Picture_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (!ZoomStatus)
                {
                    return;
                }

                Rectangle main = Screen.PrimaryScreen.Bounds;

                var direction = e.Delta > 0;
                Size picSize;
                if (direction)
                {
                    picSize = new Size((int)(PanelVideo.Width * 1.1), (int)(PanelVideo.Height * 1.1));
                }
                else
                {
                    picSize = new Size((int)(PanelVideo.Width * 0.9), (int)(PanelVideo.Height * 0.9));
                    if (picSize.Width < PanelVideo.Parent.Size.Width)
                    {
                        picSize = PanelVideo.Parent.Size;
                    }
                }

                var mp = new Point(100 * e.X / this.Width, 100 * e.Y / this.Height);
                var p = new Point(picSize.Width * mp.X / 100, picSize.Height * mp.Y / 100);
                var picPosition = new Point(e.X - p.X, e.Y - p.Y);
                if (picSize == PanelVideo.Parent.Size)
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
                    //Image = PanelVideo.Image,
                    Size = PanelVideo.Size,
                    Location = PanelVideo.Location,
                    Visible = true
                };

                this.Controls.Add(temp);
                //temp.BringToFront();
                //picture.Visible = false;

                this._actualSize = direction ? this._actualSize + 1 : this._actualSize - 1;

                PanelVideo.Size = picSize;
                PanelVideo.Location = picPosition;
                PanelVideo.Visible = true;
                this.Controls.Remove(temp);
                this.PanelControls.BringToFront();
                this.slider.BringToFront();
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
            }

        }

        public void SelectSpeed(PlaySpeed speed)
        {
            try
            {
                PlaySpeed temp = CurrentSpeed;
                while (temp != speed)
                {
                    if (temp >= PlaySpeed.MIN && temp < PlaySpeed.NORMAL)
                    {
                        ChangePlaySpeed(1);
                        ShowPlaySpeed(1);
                        temp = (PlaySpeed)((int)temp + 1);
                    }
                    else
                    {
                        ChangePlaySpeed(-1);
                        ShowPlaySpeed(-1);
                        temp = (PlaySpeed)((int)temp - 1);

                    }
                }
                CurrentSpeed = speed;
            }
            catch (Exception ex)
            {
                Logger.Log($"Message {ex.Message} StackTrace {ex.StackTrace}");
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