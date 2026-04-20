using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Common.Reflections;
using Elipgo.SmartClient.Common.Reflections.Manufactures;
using Elipgo.SmartClient.Drivers.Dahua351.NetSDKCS;
using Elipgo.SmartClient.Services;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using EnumDrivers = Elipgo.SmartClient.Common.Enum.Drivers;

namespace Elipgo.SmartClient.Drivers.Dahua351
{

    public partial class DahuaLiveUserControl : UserControl, IDriverLive, IDisposable, IConectionNotification
    {
        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnVideoEventHandler OnVideo;
        public event ButtonPressedEventHandler PressedButtons;
        public event OnDriverDispose OnDispose;
        public event OnSequecingClick OnSequencing;
        public event OnInitializeAudioEventHandler OnInitializeAudio;
        public event OnAddExtraProfilesEventHandler OnAddExtraProfiles;

        public ILoginControl _loginControl = Locator.Current.GetService<ILoginControl>();
        private PtzUserControl _ptzUserControl;
        private SequencingUserControl _sequencingUserControl;
        private IntPtr _talkHandle = IntPtr.Zero;
        private const int SAMPLE_RATE = 8000;
        private const int AUDIO_BIT = 16;
        private const int PACKET_PERIOD = 25;
        private const int SEND_AUDIO = 0;
        private const int REVICE_AUDIO = 1;
        private const uint TIME_OUT = 3000;
        private int _retryCount = 0;
        private readonly int _retryLimit;
        private int _tryCount = 0;
        private readonly int _tryLimit;
        private int _retryCallbackCount = 0;
        private readonly int _retryCallbackLimit;
        private bool _showTextConnectionState = true;
        private uint _snapshotSerialNum = 1000;
        private bool _isSetSnapRevCallBack = false;
        private string _snapshotPath;
        private uint _bitRate = 0;
        private System.Timers.Timer _timerBitRate = new System.Timers.Timer();
        private System.Timers.Timer _timerCheckFps = new System.Timers.Timer();

        private bool _loaded = false;
        public bool IsSequencingEnabled => this.Camera.Sequencing != null;
        public Profile Profile { get; set; }
        public List<Profile> Profiles => _profilesList;
        private ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();

        private EM_RealPlayType DahuaProfile { get; set; }
        private delegate void SafeCallDelegate(PlaybackConnectionState connectionState);
        private ManufactureUriAbstract _manufactureUri;
        private string _nameTab = string.Empty;
        private EnumDrivers _driver;
        private bool _bitRateStatus = false;

        private bool _updateProfileStream = true;
        private List<Profile> _profilesList = new List<Profile>() { Profile.MainStream, Profile.SubStream };
        private StreamSwitcher _streamSwitcher;
        private int _configuredFps = 0;
        private int _framesCount = 0;
        private bool _autoSwitching = false;

        public DahuaLiveUserControl(CameraDTO camera, Profile profile, bool initAudio, string nameTab, EnumDrivers driver)
        {
            InitializeComponent();
            this._driver = driver;
            Camera = camera;
            _disConnectCallback = new fDisConnectCallBack(DisconnectCallBack); //set disconnect callback.
            NETClient.SetDriver(_driver);
            bool initNETClient = NETClient.Init(_disConnectCallback, IntPtr.Zero, null);
            if (!initNETClient)
            {
                _notification.Show("NetClient init failed!", null);
            }

            this._nameTab = nameTab;

            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            _retryLimit = int.Parse(config.AppSettings.Settings["retryLimit"].Value);
            _tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
            _retryCallbackLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);

            IsInitAudio = initAudio;
            Profile = profile;
            ChangeProfile(Profile);

            CheckForIllegalCrossThreadCalls = false;
            this.Load += DahuaLiveUserControl_Load;
            this.Click += DahuaLiveUserControl_Click;
            this.picture.Click += DahuaLiveUserControl_Click;
            this.panelFondoLogo.Click += DahuaLiveUserControl_Click;

            this.DoubleClick += DahuaLiveUserControl_DoubleClick;
            this.picture.DoubleClick += DahuaLiveUserControl_DoubleClick;
            this.panelFondoLogo.DoubleClick += DahuaLiveUserControl_DoubleClick;

            this.Paint += DahuaLiveUserControl_Paint;
            this.Resize += DahuaLiveUserControl_Resize;

            ListenStatus = false;
            ClipStatus = false;
            TalkStatus = false;
            PtzStatus = false;
            SequencingStatus = false;
            DigitalZoomStatus = false;
            InstantPlaybackStatus = false;

            ButtonZoomIn.MouseDown += ButtonZoomIn_MouseDown;
            ButtonZoomOut.MouseDown += ButtonZoomOut_MouseDown;
            ButtonZoomIn.MouseUp += ButtonZoomIn_MouseUp;
            ButtonZoomOut.MouseUp += ButtonZoomOut_MouseUp;
            this.MouseWheel += Picture_MouseWheel;

            _timeReConnectionCheck = 5;
            _manufactureUri = new DahuaUri(camera, Profile.None);

            _realPlayDisConnectCallBack = new fRealPlayDisConnectCallBack(RealPlayDisConnectCallBack); //instance realplay disconnect callback.
            _haveReConnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack); //instance reconnect callback.
            _snapRevCallBack = new fSnapRevCallBack(SnapRevCallBack); // instance capture callback.
            _audioDataCallBack = new fAudioDataCallBack(AudioDataCallBack);
            NETClient.SetAutoReconnect(_haveReConnectCallBack, IntPtr.Zero); //set reconnect callback.

            if (Camera.PtzEnabled)
            {
                _ptzUserControl = new PtzUserControl
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(263, 9),
                    Margin = new Padding(0),
                    Name = "ptzUserControl1",
                    Size = new Size(72, 72),
                    Visible = false,
                    TabIndex = 1
                };
                this.Controls.Add(_ptzUserControl);
                _ptzUserControl.Location = new Point(this.Width - 82, 34);
                _ptzUserControl.ButtonMouseDown += PtzUserControl_ButtonMouseDown;
                _ptzUserControl.ButtonMouseUp += PtzUserControl_ButtonMouseUp;
                _ptzUserControl.BringToFront();
            }
            _bitRateStatus = bool.TryParse(WorkFolderUtils.GetUserSettings("BitRate", true), out bool preResult) && preResult;

            if (_bitRateStatus)
            {
                _timerBitRate.Interval = 500;
                _timerBitRate.Elapsed += TimerBitRate_Elapsed;
                ButtonZoomIn.SendToBack();
                ButtonZoomOut.SendToBack();
                labelBitRate.BringToFront();
                _timerBitRate.Start();
            }
            else
            {
                labelBitRate.Visible = false;
            }

            _timerCheckFps.Interval = 60000; // 1 minuto
            _timerCheckFps.Elapsed += TimerCheckFps_Elapsed;
        }

        private void TimerCheckFps_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckAndSwitchStream();
        }

        private void TimerBitRate_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (this.InvokeRequired)
                {

                    this.Invoke((MethodInvoker)delegate
                    {
                        TimerBitRate_Elapsed(sender, e);
                    });
                    return;
                }
                labelBitRate.BringToFront();
                //this.labelBitRate.Text = (_bitRate / 1024).ToString() + " kbps";
                _bitRate = 0;

            }
            catch (Exception)
            {

            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (DigitalZoomStatus)
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
            else
            {
                //Ctl++
                if (keyData == (Keys.Control | Keys.Oemplus))
                {
                    PTZControl(EM_EXTPTZ_ControlType.ZOOM_ADD_CONTROL, 0, (int)Camera.ZoomSensitivity, true);
                }
                //Ctl+-
                if (keyData == (Keys.Control | Keys.OemMinus))
                {
                    PTZControl(EM_EXTPTZ_ControlType.ZOOM_DEC_CONTROL, 0, (int)Camera.ZoomSensitivity, true);
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DahuaLiveUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (_loaded)
            {
                Reconnecting.DisplayLogo(picture.Width, picture.Height, ref panelconnection, ref panelFondoLogo);
                return;
            }

            ButtonZoomIn.Image = Common.Properties.FileResources.ptz_zoom_in;
            ButtonZoomOut.Image = Common.Properties.FileResources.ptz_zoom_out;
            ButtonZoomIn.Location = new Point(this.Width - 34, this.Height - 64);
            ButtonZoomOut.Location = new Point(this.Width - 34, this.Height - 34);
            ButtonTalk.Image = Common.Properties.FileResources.icon_micr_on;
            ButtonTalk.Location = new Point(this.Width - 50, this.Height - 40);
            ButtonTalk.Left = 15;
            ButtonTalk.Visible = false;

            //ButtonZoomIn.Visible = false;
            //ButtonZoomOut.Visible = false;

            if (Camera.Sequencing != null)
            {
                _sequencingUserControl = new SequencingUserControl(Camera.Sequencing)
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(263, 9),
                    Margin = new Padding(0),
                    Name = "SequencingUserControl",
                    Size = new Size(72, 72),
                    Visible = SequencingStatus,
                    TabIndex = 1
                };
                this.Controls.Add(_sequencingUserControl);
                _sequencingUserControl.Location = new Point(this.Width - 82, 34);
                _sequencingUserControl.ButtonMouseUp += SequencingUserControl_ButtonClick;
                _sequencingUserControl.BringToFront();
            }
            _loaded = true;
        }

        private void SequencingUserControl_ButtonClick(object sender, int e)
        {
            OnSequencing?.Invoke(e);
        }

        private void DahuaLiveUserControl_Resize(object sender, EventArgs e)
        {
            Reconnecting.DisplayLogo(picture.Width, picture.Height, ref panelconnection, ref panelFondoLogo);
        }

        private void PtzUserControl_ButtonMouseUp(object sender, PtzMovement e)
        {
            switch (e)
            {
                case PtzMovement.Up:
                    PTZControl(EM_EXTPTZ_ControlType.UP_CONTROL, 0, (int)Camera.MovementSensitivity, true);
                    break;
                case PtzMovement.Down:
                    PTZControl(EM_EXTPTZ_ControlType.DOWN_CONTROL, 0, (int)Camera.MovementSensitivity, true);
                    break;
                case PtzMovement.Left:
                    PTZControl(EM_EXTPTZ_ControlType.LEFT_CONTROL, 0, (int)Camera.MovementSensitivity, true);
                    break;
                case PtzMovement.Right:
                    PTZControl(EM_EXTPTZ_ControlType.RIGHT_CONTROL, 0, (int)Camera.MovementSensitivity, true);
                    break;
                case PtzMovement.Center:
                    break;
                case PtzMovement.UpLeft:
                    PTZControl(EM_EXTPTZ_ControlType.LEFTTOP, (int)Camera.MovementSensitivity, (int)Camera.MovementSensitivity, true);
                    break;
                case PtzMovement.DownLeft:
                    PTZControl(EM_EXTPTZ_ControlType.LEFTDOWN, (int)Camera.MovementSensitivity, (int)Camera.MovementSensitivity, true);
                    break;
                case PtzMovement.UpRight:
                    PTZControl(EM_EXTPTZ_ControlType.RIGHTTOP, (int)Camera.MovementSensitivity, (int)Camera.MovementSensitivity, true);
                    break;
                case PtzMovement.DownRight:
                    PTZControl(EM_EXTPTZ_ControlType.RIGHTDOWN, (int)Camera.MovementSensitivity, (int)Camera.MovementSensitivity, true);
                    break;
            }
        }

        private void PtzUserControl_ButtonMouseDown(object sender, PtzMovement e)
        {
            NETClient.SetDriver(_driver);
            switch (e)
            {
                case PtzMovement.Up:
                    PTZControl(EM_EXTPTZ_ControlType.UP_CONTROL, 0, (int)Camera.MovementSensitivity, false);
                    break;
                case PtzMovement.Down:
                    PTZControl(EM_EXTPTZ_ControlType.DOWN_CONTROL, 0, (int)Camera.MovementSensitivity, false);
                    break;
                case PtzMovement.Left:
                    PTZControl(EM_EXTPTZ_ControlType.LEFT_CONTROL, 0, (int)Camera.MovementSensitivity, false);
                    break;
                case PtzMovement.Right:
                    PTZControl(EM_EXTPTZ_ControlType.RIGHT_CONTROL, 0, (int)Camera.MovementSensitivity, false);
                    break;
                case PtzMovement.Center:
                    break;
                case PtzMovement.UpLeft:
                    PTZControl(EM_EXTPTZ_ControlType.LEFTTOP, (int)Camera.MovementSensitivity, (int)Camera.MovementSensitivity, false);
                    break;
                case PtzMovement.DownLeft:
                    PTZControl(EM_EXTPTZ_ControlType.LEFTDOWN, (int)Camera.MovementSensitivity, (int)Camera.MovementSensitivity, false);
                    break;
                case PtzMovement.UpRight:
                    PTZControl(EM_EXTPTZ_ControlType.RIGHTTOP, (int)Camera.MovementSensitivity, (int)Camera.MovementSensitivity, false);
                    break;
                case PtzMovement.DownRight:
                    PTZControl(EM_EXTPTZ_ControlType.RIGHTDOWN, (int)Camera.MovementSensitivity, (int)Camera.MovementSensitivity, false);
                    break;
            }
        }

        private void PTZControl(EM_EXTPTZ_ControlType type, int param1, int param2, bool isStop)
        {
            try
            {
                NETClient.PTZControl(DeviceLogin, Camera.Channel - 1, type, param1, param2, 0, isStop, IntPtr.Zero);
            }
            catch (Exception ex)
            {
                if (ex is Exception)
                {
                    //MessageBox.Show(this, (ex as NETClientExcetion).Message);
                }
            }
        }

        private void CustomDispose(bool removeControls = true)
        {
            try
            {
                if (!string.IsNullOrEmpty(_nameTab))
                {
                    NETClient.SetDriver(_driver);
                    if (_loginControl.RemoveChannelAndCanLogout(Camera, this, _driver))
                    {
                        
                        Logger.Log($"Dispose NETSDK_351  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User.ToLower()} {_nameTab}", LogPriority.Information);
                        if (IntPtr.Zero != _talkHandle)
                        {
                            TalkOff();
                        }
                        if (DeviceLogin != IntPtr.Zero)
                        {
                            //if (loginControl.RemoveChannelAndCanLogout(Camera, this, Common.Enum.Drivers.NETSDK_351))
                            //{
                            NETClient.Logout(DeviceLogin);
                            DeviceLogin = IntPtr.Zero;
                            _realHandle = IntPtr.Zero;
                            NETClient.Cleanup();
                            //}
                        }
                    }
                    Logger.Log($"Dispose Logout NETSDK_351 {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {_nameTab}", LogPriority.Information);

                    //_disConnectCallback = null;
                    // m_RealDataCallBack = null;
                    _realPlayDisConnectCallBack = null;//instance realplay disconnect callback.
                    _haveReConnectCallBack = null; //instance reconnect callback.
                    _snapRevCallBack = null;// instance capture callback.
                    _audioDataCallBack = null;
                    _realDataCallBackEx = null;
                    if (removeControls)
                    {
                        this.Controls.Clear();
                        this.MouseWheel -= Picture_MouseWheel;
                    }
                    UnsubcribePTZEvent();
                    this.OnDispose?.Invoke(_driver, _nameTab, this);
                    _timerBitRate.Elapsed -= new ElapsedEventHandler(TimerBitRate_Elapsed);
                    _timerBitRate.Stop();
                    _timerCheckFps.Elapsed -= new ElapsedEventHandler(TimerCheckFps_Elapsed);
                    _timerCheckFps.Stop();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error al ejecutar el driver NETSDK_351 {ex.StackTrace}");
            }
        }

        public new void Dispose()
        {
            this.CustomDispose();
        }

        public void DisposeDragged()
        {
            NETClient.CloseSound();
            NETClient.StopSaveRealData(_realHandle);
            NETClient.StopRealPlay(_realHandle);
        }

        private void DahuaLiveUserControl_Click(object sender, EventArgs e)
        {
            CameraSelected?.Invoke(this, Camera);

            if (DigitalZoomStatus)
            {
                if (picture.Location.X < 0)
                {
                    Point mousePosition = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);

                    // Find what content is under the cursor
                    int contentX = mousePosition.X - picture.Location.X;
                    int contentY = mousePosition.Y - picture.Location.Y;

                    // Center that content in the visible area
                    int newX = (this.Width / 2) - contentX;
                    int newY = (this.Height / 2) - contentY;

                    // Clamp so we don't show empty space beyond the picture edges
                    newX = Math.Min(0, Math.Max(this.Width - picture.Width, newX));
                    newY = Math.Min(0, Math.Max(this.Height - picture.Height, newY));

                    picture.Location = new Point(newX, newY);
                }

                this.Focus();
                this.BringToFront();
            }
        }

        private void DahuaLiveUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick(this);

                if (DigitalZoomStatus)
                {
                    if (picture.Location.X < 0)
                    {
                        Point mousePosition = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);

                        int contentX = mousePosition.X - picture.Location.X;
                        int contentY = mousePosition.Y - picture.Location.Y;

                        int newX = Math.Min(0, Math.Max(this.Width - picture.Width, (this.Width / 2) - contentX));
                        int newY = Math.Min(0, Math.Max(this.Height - picture.Height, (this.Height / 2) - contentY));

                        picture.Location = new Point(newX, newY);
                    }
                }
            }
        }

        public CameraDTO Camera { get; set; }

        public List<ButtonsContextBar> Commands => GetButtons();

        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();

        private List<ButtonsContextBar> GetButtons()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>();

            commands.AddRange(new List<ButtonsContextBar>
            {
                ButtonsContextBar.Fullscreen,
                ButtonsContextBar.Snapshot,
                ButtonsContextBar.Videoclip
            });

            return commands;
        }

        private int _actualSize = 0;
        private int _zoomLimit = int.Parse(Settings.Default["ZoomLimit"].ToString());

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

            if (Camera.PtzEnabled)
            {
                commands.Add(ButtonsContextBar.Ptz);
                commands.Add(ButtonsContextBar.Presets);
                commands.Add(ButtonsContextBar.Guards);
                commands.Add(ButtonsContextBar.CreatePreset);
            }
            return commands;
        }

        public bool CallGuard(ActivateGuardDTO guard)
        {
            try
            {
                if (guard == null)
                {
                    return false;
                }

                var url = _manufactureUri.CallGuardUri(guard);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
                if (result == "OK" + Environment.NewLine)
                {
                    _notification.Show($"{Common.Properties.Resources.GuardTourStart}: {guard.Id}", null);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CallPreset(PresetDTO preset)
        {
            try
            {
                var url = _manufactureUri.CallPresetUri(preset);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public GuardDTO[] ListGuards()
        {
            var response = new List<GuardDTO>();
            try
            {
                var protocolCaps = GetCurrentProtocolCaps();
                string tourMin = ParseKeyProtocolCaps(protocolCaps, "TourMin");
                string tourMax = ParseKeyProtocolCaps(protocolCaps, "TourMax");
                if (!string.IsNullOrEmpty(tourMin) && !string.IsNullOrEmpty(tourMax))
                {
                    int min = int.Parse(tourMin);
                    int max = int.Parse(tourMax);
                    int j = min == 0 ? 1 : min;

                    for (int i = min; i < max; i++, j++)
                    {
                        response.Add(new GuardDTO() { Id = i, Name = "Tour " + j });
                    }
                }
            }
            catch (Exception) { }
            return response.ToArray();
        }

        public PresetDTO[] ListPresets()
        {
            var response = new List<PresetDTO>();
            try
            {
                var protocolCaps = GetCurrentProtocolCaps();
                string presetMin = ParseKeyProtocolCaps(protocolCaps, "PresetMin");
                string presetMax = ParseKeyProtocolCaps(protocolCaps, "PresetMax");
                if (!string.IsNullOrEmpty(presetMin) && !string.IsNullOrEmpty(presetMax))
                {
                    int min = int.Parse(presetMin);
                    int max = int.Parse(presetMax);
                    int j = min == 0 ? 1 : min;

                    for (int i = min; i < max; i++, j++)
                    {
                        response.Add(new PresetDTO() { Id = j, Name = "Preset " + j });
                    }
                }
            }
            catch (Exception) { }
            return response.ToArray();
        }

        public bool Play()
        {
            if (IsPlaying)
            {
                return true;
            }

            try
            {
                var t = Task.Factory.StartNew(() =>
                {
                    IsPlay();
                }).ContinueWith(x => { x.Dispose(); });
            }
            catch (Exception ex)
            {
                Logger.Log($"Play Exception {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} - {ex.Message}", LogPriority.Fatal);
                _notification.Show($"{Camera.Name} - {ex.Message}", null);
                IsPlaying = false;
            }
            return IsPlaying;
        }

        private void IsPlay()
        {
            try
            {
                _isRealDataCallBackEx = false;
                Logger.Log($"Play Dahua entered  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                SetVisivility(PlaybackConnectionState.Connecting);
                NETClient.SetDriver(_driver);
                if (Login())
                {
                    _retryCallbackCount = 0;
                    IsPlaying = OpenCamera();
                    if (!IsPlaying)
                    {
                        //_tryCount = 0;
                        TryToReConnect();
                        SetVisivility(PlaybackConnectionState.Connected);
                    }
                    else
                    {
                        Logger.Log($"Play Dahua Connected  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                        SetVisivility(PlaybackConnectionState.Connected);
                    }
                }
                else
                {
                    if (!this.IsDisposed)
                    {
                        IsPlaying = false;
                        if (_retryCount <= _retryLimit)
                        {
                            SetVisivility(PlaybackConnectionState.Reconnecting);
                            Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(4000 * _retryCount) }, () => IsPlay());
                            Logger.Log($"Dahua Play() Error to Camera login current {_retryCount} of {_retryLimit}:  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                            _retryCount++;
                        }
                        else
                        {
                            SetVisivility(PlaybackConnectionState.Disconnected);
                            Logger.Log($"Dahua Play() reached max retry number, then it is  disconnected: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Play Exception {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} - {ex.Message}", LogPriority.Fatal);
                _notification.Show($"{Camera.Name} - {ex.Message}", null);
                IsPlaying = false;
            }
        }

        public bool RemoveGuard(GuardDTO guard)
        {
            try
            {
                if (guard == null)
                {
                    return false;
                }

                var url = _manufactureUri.RemoveGuardTourUri(guard);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
                if (result == "OK" + Environment.NewLine)
                {
                    _notification.Show($"{Common.Properties.Resources.GuardTourDeleted}: {guard.Name}", null);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemovePreset(PresetDTO preset)
        {
            try
            {
                var url = _manufactureUri.DeletePresetUri(preset);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveGuard(GuardDTO guard)
        {
            return false;
        }

        public bool SavePreset(PresetDTO preset)
        {
            try
            {
                var url = _manufactureUri.SavePresetUri(preset);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Snapshot(string path, string token, int id)
        {
            _snapshotPath = path;
            NETClient.SetDriver(_driver);
            try
            {
                if (!_isSetSnapRevCallBack)
                {
                    NETClient.SetSnapRevCallBack(_snapRevCallBack, IntPtr.Zero);
                    _isSetSnapRevCallBack = true;
                }
                NET_SNAP_PARAMS snapshotParams = new NET_SNAP_PARAMS
                {
                    Channel = (uint)Camera.Channel - 1,
                    Quality = 6,
                    ImageSize = 2,
                    mode = 0,
                    InterSnap = 0,
                    CmdSerial = _snapshotSerialNum
                };
                bool ret = NETClient.SnapPictureEx(DeviceLogin, snapshotParams, IntPtr.Zero); //call capture function.
                if (ret)
                {
                    if (!File.Exists(_snapshotPath))
                    {
                        var imagen64 = await Vmon5Service.Snapshot(id, token);
                        if (!string.IsNullOrEmpty(imagen64))
                        {
                            using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(imagen64)))
                            {
                                using (Bitmap bm2 = new Bitmap(ms))
                                {
                                    bm2.Save(_snapshotPath);
                                }
                            }
                            ret = true;
                        }
                        else
                            ret = false;
                    }
                    //_snapshotSerialNum++;
                }
                return ret;// ret;
            }
            catch (Exception ex)
            {
                if (ex is NETClientExcetion)
                {
                    _notification.Show((ex as NETClientExcetion).Message, null);
                }
                return false;
            }
        }

        public bool StateGuard(GuardDTO guard)
        {
            return false;
        }

        public bool Stop()
        {
            _tryCount = _tryLimit;
            if (!IsPlaying)
            {
                return true;
            }
            NETClient.SetDriver(_driver);
            ListenStatus = false;
            NETClient.CloseSound();
            //NETClient.StopSaveRealData(realHandle);
            NETClient.StopRealPlay(_realHandle);
            IsPlaying = false;
            return true;
        }

        public void ToggleFullScreen()
        {
            var tempProfile = this.Profile;
            DahuaFullscreen fullscreen = new DahuaFullscreen();
            Task.Run(() =>
            {
                Stop();
                Profile = Profile.MainStream;
                DahuaProfile = (EM_RealPlayType)2;
                RealStart(fullscreen.pHandle);
            });
            fullscreen.ShowDialog();

            Profile = tempProfile;
            switch (tempProfile)
            {
                case Profile.SubStream3:
                    DahuaProfile = (EM_RealPlayType)5;
                    break;
                case Profile.SubStream2:
                    DahuaProfile = (EM_RealPlayType)4;
                    break;
                case Profile.SubStream:
                    DahuaProfile = (EM_RealPlayType)3;
                    break;
                case Profile.MainStream:
                    DahuaProfile = (EM_RealPlayType)2;
                    break;
                default:
                    DahuaProfile = (EM_RealPlayType)3;
                    break;
            }
            RealStart(GetHandle());
        }

        public bool ToggleListen(bool Listen)
        {
            try
            {
                //ListenStatus = !ListenStatus;
                NETClient.SetDriver(_driver);
                if (Listen)
                {
                    NETClient.OpenSound(_realHandle); //call opensound function.
                }
                else
                {
                    NETClient.CloseSound();
                }

                return Listen;
            }
            catch (Exception ex)
            {
                _notification.Show(ex.Message, null);
                ListenStatus = false;
                return false;
            }
        }

        public bool ToggleTalk()
        {
            try
            {
                StartTalk();
                TalkStatus = !TalkStatus;
                ButtonTalk.Visible = TalkStatus;
                return true;
            }
            catch (Exception)
            {
                TalkStatus = false;
                return false;
                throw;
            }
        }

        private void StartTalk()
        {
            if (IntPtr.Zero == _talkHandle && !TalkStatus)
            {
                TalkOn();
            }
            else
            {
                TalkOff();
            }
        }

        private void AudioDataCallBack(IntPtr lTalkHandle, IntPtr pDataBuf, uint dwBufSize, byte byAudioFlag, IntPtr dwUser)
        {
            if (lTalkHandle == _talkHandle)
            {
                if (SEND_AUDIO == byAudioFlag)
                {
                    //send talk data 
                    NETClient.TalkSendData(lTalkHandle, pDataBuf, dwBufSize);
                }
                else if (REVICE_AUDIO == byAudioFlag)
                {
                    //here call netsdk decode audio, or can send data to other user.
                    try
                    {
                        NETClient.AudioDec(pDataBuf, dwBufSize);
                    }
                    catch (Exception ex)
                    {
                        _notification.Show(ex.Message, null);
                    }
                }
            }
        }

        public bool VideoClipStart(string path)
        {
            try
            {
                path = path.Replace(".mp4", ".dav");
                bool ret = ExecuteWithContextRetry(() => NETClient.SaveRealData(_realHandle, path));
                if (ret)
                {
                    _saveRealDataDictionary[_realHandle] = path;
                }

                ClipStatus = ret;
                return ret;
            }
            catch (Exception ex)
            {
                _notification.Show(ex.Message, null);
                ClipStatus = false;
                return false;
            }
        }

        public bool VideoClipStop()
        {
            try
            {
                bool ret = ExecuteWithContextRetry(() => NETClient.StopSaveRealData(_realHandle));
                if (ret)
                {
                    if (_saveRealDataDictionary.ContainsKey(_realHandle))
                    {
                        var path = _saveRealDataDictionary[_realHandle];
                        _saveRealDataDictionary.Remove(_realHandle);
                        VideoConversorServices.Instance.ConvertFileMediaToMp4(path);
                    }
                }
                ClipStatus = false;
                return ret;
            }
            catch (Exception ex)
            {
                _notification.Show(ex.Message, null);
                ClipStatus = false;
                return false;
            }
        }

        public bool Volume(int value)
        {
            throw new NotImplementedException();
        }

        public bool ChangeProfile(Profile profile, bool autoSwitching = false)
        {
            if (profile == Profile.Auto && autoSwitching && _configuredFps > 0)
            {
                _streamSwitcher = new StreamSwitcher(_configuredFps);
                _timerCheckFps.Start();
                _autoSwitching = true;
                return true;
            }
            else if (_autoSwitching && !autoSwitching)
            {
                _timerCheckFps.Stop();
                _autoSwitching = false;
            }

            _showTextConnectionState = false;
            switch (profile)
            {
                case Profile.SubStream:
                    DahuaProfile = (EM_RealPlayType)3;
                    break;
                case Profile.MainStream:
                    DahuaProfile = (EM_RealPlayType)2;
                    break;
                default:
                    DahuaProfile = (EM_RealPlayType)3;
                    break;
            }
            if (Profile != profile && _realHandle != IntPtr.Zero)
            {
                Stop();
                Profile = profile;
                IsPlay();
                _isRealDataCallBackEx = true;
                return true;
                //return Play();
            }
            return true;
        }


        ///// ActiveX Functionality
        ///

        //manage savedata handel.
        private Dictionary<IntPtr, string> _saveRealDataDictionary = new Dictionary<IntPtr, string>();

        //call back for receive real data .
        //private fRealDataCallBackEx m_RealDataCallBack;
        private fRealDataCallBackEx2 _realDataCallBackEx;
        //call back for realplay disconnect.
        private fRealPlayDisConnectCallBack _realPlayDisConnectCallBack;
        //call back for reconnect to device.
        private fHaveReConnectCallBack _haveReConnectCallBack;
        //call back for capture image and save image.
        private fSnapRevCallBack _snapRevCallBack;
        private fAudioDataCallBack _audioDataCallBack;
        private readonly Random _random = new Random();
        private bool _isRealDataCallBackEx = false;
        //private const int SpeedValue = 1;
        private double _timeReConnectionCheck;

        private IntPtr DeviceLogin;
        public bool ListenStatus { get; set; }
        public bool ClipStatus { get; set; }
        public bool TalkStatus { get; set; }
        public bool PtzStatus { get; set; }
        public bool SequencingStatus { get; set; }
        public bool DigitalZoomStatus { get; set; }
        public bool InstantPlaybackStatus { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsInitAudio { get; set; }

        private fDisConnectCallBack _disConnectCallback; //call back for device disconnect.
        private IntPtr _realHandle = IntPtr.Zero;

        private void DisconnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        DisconnectCallBack(lLoginID, pchDVRIP, nDVRPort, dwUser);
                    });
                    return;
                }
                _loginControl.Disconect(lLoginID, this._realHandle);
                NETClient.StartListen(lLoginID);
                UpdateDisConnectUI();
            }
            catch (Exception ex)
            {
                Logger.Log($"DisconnectCallBack Disconnect Exception {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        private void UpdateDisConnectUI()
        {
            try
            {
                if (_retryCallbackCount < _retryCallbackLimit)
                {
                    _retryCallbackCount++;
                    Logger.Log($"DisconnectCallBack current {_retryCallbackCount} of {_retryCallbackLimit}  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                }
                else
                {
                    Logger.Log($"DisconnectCallBack reached maximum retry value {_retryCallbackLimit} , {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Disconnected);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Disconnect Exception {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} - {ex.Message}", LogPriority.Fatal);
                _notification.Show($"{Camera.Name} - {ex.Message}", null);
            }
        }

        private void ReConnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        ReConnectCallBack(lLoginID, pchDVRIP, nDVRPort, dwUser);
                    });
                    return;
                }

                /*dahua use a static class, then  only the last object receives events, becuse of that I need to brocast for the other dahua objecs*/
                Logger.Log($" ----> ReConnectCallBack Live {Camera.Name} ", LogPriority.Information);
                _retryCallbackCount = 0;
                _loginControl.Connect(lLoginID, this._realHandle);
                NETClient.StartListen(lLoginID);
                UpdateReConnectUI();
            }
            catch (Exception ex)
            {
                Logger.Log($"ReConnectCallBack Exception {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} - {ex.Message}", LogPriority.Fatal);
                _notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
        }

        private void UpdateReConnectUI()
        {
            SetVisivility(PlaybackConnectionState.Connected);
        }

        private void RealPlayDisConnectCallBack(IntPtr lRealHandle, EM_REALPLAY_DISCONNECT_EVENT_TYPE dwEventType, IntPtr param, IntPtr dwUser)
        {

        }

        public void Disconect(IntPtr HandledBrocaster)
        {
            Logger.Log($"Disconect Dahua {_driver} disconnected {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
            SetVisivility(PlaybackConnectionState.Reconnecting);
            NETClient.StopRealPlay(_realHandle);
        }

        public void Connect(IntPtr HandledBrocaster)
        {/*to avoid all camara connect all together  set a random time sleep*/
            //tryCount = 0;
            int r = ((int)(((_random.NextDouble() * _timeReConnectionCheck) + 1) * 1000));
            Task.Delay(r).ContinueWith(t => TryToReConnect());
        }

        private void TryToReConnect()
        {
            try
            {
                if (!this.IsDisposed)
                {
                    if (_tryCount >= _tryLimit)
                    {
                        SetVisivility(PlaybackConnectionState.Disconnected);
                        Logger.Log($"Dahua TryToReConnect reached max retry number, then it is  disconnected:  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                        return;
                    }

                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    _tryCount++;
                    if (RealStart(GetHandle()) == false)
                    {
                        int r = ((int)(((_random.NextDouble() * _timeReConnectionCheck) + 1) * 3000));
                        Logger.Log($"Dahua TryToReConnect RealStar failed  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                        Task.Delay(r).ContinueWith(t => TryToReConnect());
                    }
                    else
                    {
                        Logger.Log($"Dahua TryToReConnect RealStar Connected  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}", LogPriority.Information);
                        SetVisivility(PlaybackConnectionState.Connected);
                        IsPlaying = true;
                        _tryCount = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Dahua TryToReConnect Exception: {ex.Message}  {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Fatal);
            }
        }


        private void SetVisivility(PlaybackConnectionState connectionState)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        SetVisivility(connectionState);
                    });
                    return;
                }

                if (!_showTextConnectionState)
                {
                    return;
                }

                switch (connectionState)
                {
                    case PlaybackConnectionState.Disconnected:
                        this.panelconnection.BringToFront();
                        this.panelconnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.disconnected_es) : new Bitmap(Properties.Resources.disconnected_en);
                        Logger.Log($"SetVisivility Disconnected: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.Disconnected} ", LogPriority.Information);
                        break;
                    case PlaybackConnectionState.Connected:
                        this.panelconnection.SendToBack();
                        picture.BringToFront();
                        if (_ptzUserControl != null && _ptzUserControl.Visible)
                        {
                            _ptzUserControl.BringToFront();
                            ButtonZoomIn.BringToFront();
                            ButtonZoomOut.BringToFront();
                        }
                        Logger.Log($"SetVisivility Connected: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.Connected} ", LogPriority.Information);
                        break;
                    case PlaybackConnectionState.NoRecording:
                        this.panelconnection.BringToFront();
                        this.panelconnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.norecording_es) : new Bitmap(Properties.Resources.norecording_en);
                        Logger.Log($"SetVisivility NoRecording: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.NoRecording} ", LogPriority.Information);
                        break;
                    case PlaybackConnectionState.Reconnecting:
                        this.panelconnection.BringToFront();
                        this.panelconnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.reconnecting_es) : new Bitmap(Properties.Resources.reconnecting_en);
                        Logger.Log($"setVisivility Reconnecting: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.Reconnecting} ", LogPriority.Information);
                        break;
                    case PlaybackConnectionState.Connecting:
                        this.panelconnection.BringToFront();
                        this.panelconnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.connecting_es) : new Bitmap(Properties.Resources.connecting_en);
                        Logger.Log($"SetVisivility Connecting: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {PlaybackConnectionState.Connecting} ", LogPriority.Information);
                        break;
                }
                Reconnecting.DisplayLogo(picture.Width, picture.Height, ref panelconnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"SetVisivility Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        private void RealDataCallBack(IntPtr lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, int param, IntPtr dwUser)
        {

        }

        private void DahuaLiveUserControl_Load(object sender, EventArgs e)
        {
            //_disConnectCallback = new fDisConnectCallBack(DisconnectCallBack); //set disconnect callback.
            //NETClient.SetDriver(_driver);
            //bool initNETClient = NETClient.Init(_disConnectCallback, IntPtr.Zero, null);
            //if (!initNETClient)
            //{
            //    _notification.Show("NetClient init failed!", null);
            //}
            //_realDataCallBackEx = new fRealDataCallBackEx2(RealDataCallBackEx2);
            //m_RealDataCallBack = new fRealDataCallBackEx(RealDataCallBack); //instance realdata callback.
            //m_RealPlayDisConnectCallBack = new fRealPlayDisConnectCallBack(RealPlayDisConnectCallBack); //instance realplay disconnect callback.
            //m_ReConnectCallBack = new fHaveReConnectCallBack(ReConnectCallBack); //instance reconnect callback.
            //m_SnapRevCallBack = new fSnapRevCallBack(SnapRevCallBack); // instance capture callback.
            //m_AudioDataCallBack = new fAudioDataCallBack(AudioDataCallBack);
            //NETClient.SetAutoReconnect(m_ReConnectCallBack, IntPtr.Zero); //set reconnect callback.
        }

        private void RealDataCallBackEx2(IntPtr lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr param, IntPtr dwUser)
        {
            _isRealDataCallBackEx = true;
            if (_bitRateStatus)
            {
                _bitRate += dwBufSize;
            }

            // Cuenta frames al recibir datos de video
            //if (dwDataType == NetSDKCS.DATA_TYPE_VIDEO)
            {
                Interlocked.Increment(ref _framesCount);
            }
        }

        private void SnapRevCallBack(IntPtr lLoginID, IntPtr pBuf, uint RevLen, uint EncodeType, uint CmdSerial, IntPtr dwUser)
        {
            if (EncodeType == 10) //.jpg
            {
                byte[] data = new byte[RevLen];
                Marshal.Copy(pBuf, data, 0, (int)RevLen);
                using (FileStream stream = new FileStream(_snapshotPath, FileMode.OpenOrCreate))
                {
                    stream.Write(data, 0, (int)RevLen);
                    stream.Flush();
                    stream.Dispose();
                }
            }
        }

        private bool OpenCamera()
        {
            bool ret = RealStart(GetHandle());
            if (!ret)
            {
                Logger.Log($"OpenCamera Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
            }
            return ret;
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private bool Login()
        {
            if (_loginControl.GetDeviceLogin(Camera, out DeviceLogin, _driver, this))
            {
                _loginControl.AddChannel(Camera, this,_driver);
                Logger.Log(String.Format("Reusing a Dahua Login to Camara {0}", Camera.Name), LogPriority.Information);
                return true;
            }

            NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();
            try
            {
                //call login function.
                ushort devicePort = 0;
                try
                {
                    devicePort = (ushort)Camera.VideoPort;
                }
                catch (Exception ex)
                {
                    Logger.Log($"ID_PORTERROR port: {Camera.VideoPort} - Exception: {ex.Message}", LogPriority.Fatal);
                    return false;
                }

                if (DeviceLogin == IntPtr.Zero)
                {
                    DeviceLogin = NETClient.Login(Camera.Host, devicePort, Camera.User, Camera.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);
                }

                if (DeviceLogin != IntPtr.Zero)
                {
                    if (!this.IsDisposed)
                    {
                        _loginControl.AddDevice(Camera, DeviceLogin, this, _driver);
                        Logger.Log($"New Dahua Login Sucessed Camara {Camera.Name}", LogPriority.Information);
                    }
                }
                else
                {
                    Logger.Log($"Device Login {Camera.Host} {Camera.VideoPort} is Zero {NETClient.GetLastError()}", LogPriority.Information);
                }

                return DeviceLogin != IntPtr.Zero;
            }
            catch (AccessViolationException ex)
            {
                MessageBox.Show($"Se capturó error crítico, por favor informe  a su area de sistemas");
                Logger.Log($"Critical error: Message: {ex.Message} StackTrace: {ex.StackTrace}", LogPriority.Fatal);
                return false;
            }
            catch (Exception ex)
            {
                if (ex is NETClientExcetion)
                {
                    _notification.Show((ex as NETClientExcetion).Message, null);
                }
                else
                {
                    _notification.Show(ex.Message, null);
                }
                Logger.Log($"Login Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} - {ex.Message}", LogPriority.Fatal);
                return false;
            }
        }

        [HandleProcessCorruptedStateExceptions]
        [SecurityCritical]
        private bool RealStart(IntPtr handle)
        {
            try
            {
                if (handle == IntPtr.Zero)
                {
                    return false;
                }

                bool response = false;
                _realHandle = IntPtr.Zero;
                NETClient.SetDriver(_driver);
                //realHandle = NETClient.StartRealPlay(loginID, channelNum - 1, handle, dahuaProfile, m_RealDataCallBack, m_RealPlayDisConnectCallBack, IntPtr.Zero, TimeOut);
                _realHandle = NETClient.RealPlay(DeviceLogin, Camera.Channel - 1, handle, DahuaProfile);
                if (_realHandle != IntPtr.Zero)
                {
                    if (_realDataCallBackEx == null)
                    {
                        //_realDataCallBackEx = new fRealDataCallBackEx2(RealDataCallBackEx2);
                        NETClient.SetRealDataCallBack(this._realHandle, _realDataCallBackEx, IntPtr.Zero, EM_REALDATA_FLAG.RAW_DATA | EM_REALDATA_FLAG.DATA_WITH_FRAME_INFO | EM_REALDATA_FLAG.YUV_DATA | EM_REALDATA_FLAG.PCM_AUDIO_DATA);
                        Task.Delay(10000).ContinueWith(t => CheckCallback());
                        //OpenCameraAudio();
                        //Se comenta temporal mente el tema de los fps ya que no funciona correctamente
                        //GetMaxExtraStream();
                        //GetConfiguredFps();
                        _framesCount = 0;
                    }
                    response = true;
                }
                else
                {
                    Logger.Log($"RealStart Login {Camera.Host} {Camera.VideoPort} is Zero {NETClient.GetLastError()} ", LogPriority.Information);
                }
                return response;
            }
            catch (AccessViolationException ex)
            {
                MessageBox.Show($"Se capturó error crítico, por favor informe  a su area de sistemas");
                Logger.Log($"Critical error: Message: {ex.Message} StackTrace: {ex.StackTrace}", LogPriority.Fatal);
                return false;
            }
            catch (Exception ex)
            {
                if (ex is NETClientExcetion)
                {
                    _notification.Show((ex as NETClientExcetion).Message, null);
                }
                Logger.Log($"RealStart Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} - {ex.Message}", LogPriority.Fatal);
                return false;
            }
        }

        private void CheckCallback()
        {
            if (_isRealDataCallBackEx)
            {
                Logger.Log($"CheckCallback is Receiving from camera {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                SetVisivility(PlaybackConnectionState.Connected);
                _retryCallbackCount = 0;
            }
            else
            {
                _retryCallbackCount++;
                Logger.Log($"CheckCallback Connected without video   {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                if (_retryCallbackCount < _retryCallbackLimit)
                {
                    Logger.Log($"CheckCallback retry to connect without video   {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                    NETClient.SetDriver(_driver);
                    //IsPlaying = OpenCamera();
                    if (_realHandle == IntPtr.Zero)
                    {
                        //_tryCount = 0;
                        TryToReConnect();
                    }
                    //Task.Delay(10000).ContinueWith(t => checkCallback());
                    SetVisivility(PlaybackConnectionState.Connected);
                }
                else
                {
                    Logger.Log($"CheckCallback failed without video  set state as disconnected {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Disconnected);
                }
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
                _tryCount = _tryLimit;
                return IntPtr.Zero;
            }
        }

        public bool ToogleDigitalZoom()
        {
            if (DigitalZoomStatus)
            {
                picture.Size = picture.Parent.Size;
                picture.Location = new Point(0, 0);
                picture.Visible = true;
                this.Cursor = Cursors.Default;
                this._actualSize = 0;
            }
            else
            {
                this.Cursor = Cursors.Cross;
                this.BringToFront();
                this.Focus();
            }

            DigitalZoomStatus = !DigitalZoomStatus;
            return DigitalZoomStatus;
        }

        private void Picture_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!DigitalZoomStatus)
            {
                return;
            }

            var direction = e.Delta > 0;
            ZoomPicture(direction, e.X, e.Y);
        }

        private void ZoomPicture(bool direction, int eventPositionX, int eventPositionY)
        {
            Rectangle main = Screen.PrimaryScreen.Bounds;
            Size pictureSize;
            if (direction)
            {
                pictureSize = new Size((int)(picture.Width * 1.1), (int)(picture.Height * 1.1));
            }
            else
            {
                pictureSize = new Size((int)(picture.Width * 0.9), (int)(picture.Height * 0.9));
                if (pictureSize.Width < picture.Parent.Size.Width)
                {
                    pictureSize = picture.Parent.Size;
                }
            }

            // Convert picture-relative coords to parent-relative coords
            int parentX = eventPositionX + picture.Location.X;
            int parentY = eventPositionY + picture.Location.Y;

            // Calculate content fraction under cursor using current picture dimensions
            double fracX = (double)eventPositionX / picture.Width;
            double fracY = (double)eventPositionY / picture.Height;

            // Where that content point will be in the new size
            int newContentX = (int)(fracX * pictureSize.Width);
            int newContentY = (int)(fracY * pictureSize.Height);

            // Position picture so content stays under cursor
            int newX = parentX - newContentX;
            int newY = parentY - newContentY;

            // Clamp position so the picture always covers the visible area
            newX = Math.Min(0, Math.Max(this.Width - pictureSize.Width, newX));
            newY = Math.Min(0, Math.Max(this.Height - pictureSize.Height, newY));

            Point pictureLocation = new Point(newX, newY);
            if (pictureSize == picture.Parent.Size)
            {
                pictureLocation = new Point(0, 0);
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

            picture.Size = pictureSize;
            picture.Location = pictureLocation;
            picture.Visible = true;
            this.Controls.Remove(temp);
        }

        private bool _unsubcribedPTZEvent = false;
        public void UnsubcribePTZEvent()
        {
            if (_ptzUserControl != null)
            {
                _ptzUserControl.PtzJoystickStateEvent -= PtzJoystickStateEvent;
                _ptzUserControl.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
                //cuando se utiliza el joytick y la camara ptz se comienza a mover, si se selecciona otra camara se ejecuta este metodo
                //realizando la desubcripcion a los eventos del joystick, pero ocurre un problema si el usuario mantiene el joystick en un estado de desplazamiento 
                //y se selecciona una nueva camara, esta camara nunca recibe el evento que pare y continua loca en forma indefinida hasta se vuelva a selecciona y ejecutar un 
                //nuevo comando con su stop correspondiente es por esto que 
                //envio un comando cualquiera para detener el movimiento de la camara cuando se descelecciona la camara forzando a parar este o no en ejecuccion
                PTZControl(EM_EXTPTZ_ControlType.ZOOM_ADD_CONTROL, 0, (int)Camera.ZoomSensitivity, true);
                _unsubcribedPTZEvent = true;
            }
        }

        public void SubcribePTZEvent()
        {
            _unsubcribedPTZEvent = false;
            if (_ptzUserControl != null)
            {
                _ptzUserControl.PtzJoystickStateEvent += PtzJoystickStateEvent;
                _ptzUserControl.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
            }
        }

        public bool TooglePtz()
        {
            if (_ptzUserControl.InvokeRequired || ButtonZoomIn.InvokeRequired || ButtonZoomOut.InvokeRequired)
            {
                _ptzUserControl.Invoke((MethodInvoker)delegate
                {
                    TooglePtz();
                });
                return false;
            }

            PtzStatus = !PtzStatus;
            if (_ptzUserControl != null)
            {
                _ptzUserControl.Visible = PtzStatus;
                if (PtzStatus)
                {
                    ButtonZoomIn.BringToFront();
                    ButtonZoomOut.BringToFront();
                    _ptzUserControl.BringToFront();
                    _ptzUserControl.StartJoystick();
                    _ptzUserControl.PtzJoystickStateEvent += PtzJoystickStateEvent;
                    _ptzUserControl.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
                }
                else
                {
                    ButtonZoomIn.SendToBack();
                    ButtonZoomOut.SendToBack();
                    _ptzUserControl.SendToBack();
                    _ptzUserControl.StopJoystick();
                    _ptzUserControl.PtzJoystickStateEvent -= PtzJoystickStateEvent;
                    _ptzUserControl.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
                }
            }
            ButtonZoomIn.Visible = PtzStatus;
            ButtonZoomOut.Visible = PtzStatus;
            return PtzStatus;
        }

        private List<ActionCommand> ExecuteZoomCommand(List<ActionCommand> pressedButtons)
        {
            foreach (ActionCommand act in pressedButtons.Where(x => x.command == PtzCommand.ZOOM_ADD_CONTROL || x.command == PtzCommand.ZOOM_DEC_CONTROL).ToList())
            {
                PTZControl((EM_EXTPTZ_ControlType)System.Enum.Parse(typeof(EM_EXTPTZ_ControlType), act.command.ToString()),
                    0,
                    (int)(Math.Abs(act.Parameter) * Camera.MovementSensitivity * 10), !act.isInvoked);
            }
            return pressedButtons.Where(x => !(x.command == PtzCommand.ZOOM_ADD_CONTROL || x.command == PtzCommand.ZOOM_DEC_CONTROL) && x.isInvoked == true).ToList();
        }

        private List<ActionCommand> ExecuteCallPresetCommand(List<ActionCommand> pressedButtons)
        {
            List<int> IdsPresets = pressedButtons.Where(x => (x.command == PtzCommand.CallPreset) && x.isInvoked == true).Select(x => (int)(x.Parameter)).ToList();
            if (IdsPresets.Count > 0)
            {
                List<PresetDTO> presets = ListPresets().OrderBy(x => x.Id).ToList();

                foreach (int id in IdsPresets)
                {
                    if (id <= presets.Count())
                    {
                        var preset = presets[id - 1];
                        if (CallPreset(preset))
                        {
                            _notification.Show(string.Format(Resources.PresetStarted, id.ToString()), null);
                        }
                        else
                        {
                            _notification.Show(Resources.PresetErrorActivate, null);
                        }
                    }
                    else
                    {
                        var message = string.Format("{0}", this.Camera.Name);
                        _notification.Show(string.Format(Resources.NoPresetAvailable, message), null);
                    }
                }
            }
            return pressedButtons.Where(x => !(x.command == PtzCommand.CallPreset)).ToList();
        }

        private void PtzJoystickButtonEvent(List<ActionCommand> pressedButtons)
        {
            if (_unsubcribedPTZEvent)
            {
                return;
            }

            pressedButtons = ExecuteZoomCommand(pressedButtons);
            pressedButtons = ExecuteCallPresetCommand(pressedButtons);
            if (pressedButtons.Count > 0)
            {
                PressedButtons?.Invoke(pressedButtons);
            }
        }

        private void PtzJoystickStateEvent(List<ActionCommand> actionCommands)
        {
            if (_unsubcribedPTZEvent)
            {
                return;
            }

            foreach (ActionCommand act in actionCommands)
            {
                PTZControl((EM_EXTPTZ_ControlType)System.Enum.Parse(typeof(EM_EXTPTZ_ControlType), act.buttonOrAxis.ToString()), (int)(Math.Abs(act.Parameter) * Camera.MovementSensitivity), (int)(Math.Abs(act.Parameter2) * Camera.MovementSensitivity), !act.isInvoked);
            }
        }

        public bool ToggleInstantPlayback()
        {
            InstantPlaybackStatus = !InstantPlaybackStatus;

            return InstantPlaybackStatus;
        }

        private void ButtonZoomIn_Click(object sender, EventArgs e)
        {

        }

        private void ButtonZoomOut_Click(object sender, EventArgs e)
        {

        }

        private void ButtonZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
            PTZControl(EM_EXTPTZ_ControlType.ZOOM_DEC_CONTROL, 0, (int)Camera.ZoomSensitivity, true);
        }

        private void ButtonZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            PTZControl(EM_EXTPTZ_ControlType.ZOOM_ADD_CONTROL, 0, (int)Camera.ZoomSensitivity, true);
        }

        private void ButtonZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            PTZControl(EM_EXTPTZ_ControlType.ZOOM_DEC_CONTROL, 0, (int)Camera.ZoomSensitivity, false);
        }

        private void ButtonZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            PTZControl(EM_EXTPTZ_ControlType.ZOOM_ADD_CONTROL, 0, (int)Camera.ZoomSensitivity, false);
        }

        public bool StopGuard(ActivateGuardDTO guard)
        {
            try
            {
                if (guard == null)
                {
                    return false;
                }

                var url = _manufactureUri.StopGuardUri(guard);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
                if (result == "OK" + Environment.NewLine)
                {
                    _notification.Show($"{Common.Properties.Resources.GuardTourStop}: {guard.Id}", null);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SaveGuard(GuardForCreationDTO guard)
        {
            try
            {
                if (guard != null)
                {
                    ClearTour(guard.Id);

                    if (guard.GuardTours != null)
                    {
                        foreach (GuardTourForCreationDTO tour in guard.GuardTours)
                        {
                            AddTour(guard.Id, tour.PresetId);
                        }
                    }
                    _notification.Show($"{Common.Properties.Resources.GuardTourSaved}: {guard.Name}", null);
                }
            }
            catch (Exception) { }
            return true;
        }

        public GuardForCreationDTO GetGuard(int guardId)
        {
            var guardTourList = new List<GuardTourForCreationDTO>();
            return new GuardForCreationDTO()
            {
                Id = guardId,
                Name = "Tour" + guardId,
                isActivated = false,
                GuardTours = guardTourList.ToArray()
            };
        }

        private string GetCurrentProtocolCaps()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "getCurrentProtocolCaps";
            query["channel"] = Camera.Channel.ToString();

            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
            return _manufactureUri.SendRequest(url, HttpMethod.Get);
        }

        private string ParseKeyProtocolCaps(string input, string key)
        {
            Regex regex = new Regex("caps." + key + "=(\\w+)", RegexOptions.Compiled);
            Match match = regex.Match(input);
            return match.Success ? match.Groups[1].Value : "";
        }

        private void ClearTour(int guardId)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "start";
            query["channel"] = Camera.Channel.ToString();
            query["code"] = "ClearTour";
            query["arg1"] = guardId.ToString();
            query["arg2"] = "0";
            query["arg3"] = "0";

            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
            _manufactureUri.SendRequest(url, HttpMethod.Get);
        }

        private void AddTour(int guardId, int presetId)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "start";
            query["channel"] = Camera.Channel.ToString();
            query["code"] = "AddTour";
            query["arg1"] = guardId.ToString();
            query["arg2"] = presetId.ToString();
            query["arg3"] = "0";

            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/cgi-bin/ptz.cgi",
                Query = query.ToString()
            }.ToString();
            _manufactureUri.SendRequest(url, HttpMethod.Get);
        }

        public bool ToggleTalk(bool talkStatus)
        {
            if (IntPtr.Zero == _talkHandle && talkStatus)
            {
                TalkOn();
            }
            else if (IntPtr.Zero != _talkHandle && !talkStatus)
            {
                TalkOff();
            }

            TalkStatus = talkStatus;
            ButtonTalk.Visible = TalkStatus;
            return true;
        }

        private void TalkOn()
        {
            NETClient.SetDriver(_driver);
            IntPtr talkEncodePointer = IntPtr.Zero;
            IntPtr talkSpeakPointer = IntPtr.Zero;
            IntPtr talkTransferPointer = IntPtr.Zero;
            IntPtr channelPointer = IntPtr.Zero;

            NET_DEV_TALKDECODE_INFO talkCodeInfo = new NET_DEV_TALKDECODE_INFO();
            talkCodeInfo.encodeType = EM_TALK_CODING_TYPE.PCM;
            talkCodeInfo.dwSampleRate = SAMPLE_RATE;
            talkCodeInfo.nAudioBit = AUDIO_BIT;
            talkCodeInfo.nPacketPeriod = PACKET_PERIOD;
            talkCodeInfo.reserved = new byte[60];

            talkEncodePointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_DEV_TALKDECODE_INFO)));
            Marshal.StructureToPtr(talkCodeInfo, talkEncodePointer, true);
            // set talk encode type
            NETClient.SetDeviceMode(DeviceLogin, EM_USEDEV_MODE.TALK_ENCODE_TYPE, talkEncodePointer);

            NET_SPEAK_PARAM speak = new NET_SPEAK_PARAM();
            speak.dwSize = (uint)Marshal.SizeOf(typeof(NET_SPEAK_PARAM));
            speak.nMode = 0;
            speak.bEnableWait = false;
            speak.nSpeakerChannel = 0;
            talkSpeakPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_SPEAK_PARAM)));
            Marshal.StructureToPtr(speak, talkSpeakPointer, true);
            //set talk speak mode
            NETClient.SetDeviceMode(DeviceLogin, EM_USEDEV_MODE.TALK_SPEAK_PARAM, talkSpeakPointer);

            NET_TALK_TRANSFER_PARAM transfer = new NET_TALK_TRANSFER_PARAM();
            transfer.dwSize = (uint)Marshal.SizeOf(typeof(NET_TALK_TRANSFER_PARAM));

            transfer.bTransfer = true;
            channelPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            Marshal.WriteInt32(channelPointer, (Camera.Channel - 1));
            NETClient.SetDeviceMode(DeviceLogin, EM_USEDEV_MODE.TALK_TALK_CHANNEL, channelPointer);

            talkTransferPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(NET_TALK_TRANSFER_PARAM)));
            Marshal.StructureToPtr(transfer, talkTransferPointer, true);
            //set talk transfer mode
            NETClient.SetDeviceMode(DeviceLogin, EM_USEDEV_MODE.TALK_TRANSFER_MODE, talkTransferPointer);

            _talkHandle = NETClient.StartTalk(DeviceLogin, _audioDataCallBack, talkTransferPointer);
            Marshal.FreeHGlobal(talkEncodePointer);
            Marshal.FreeHGlobal(talkSpeakPointer);
            Marshal.FreeHGlobal(talkTransferPointer);
            Marshal.FreeHGlobal(channelPointer);
            if (IntPtr.Zero == _talkHandle)
            {
                return;
            }
            bool ret = NETClient.RecordStart(DeviceLogin);
            if (!ret)
            {
                NETClient.StopTalk(_talkHandle);
                _talkHandle = IntPtr.Zero;
                return;
            }
        }

        private void TalkOff()
        {
            NETClient.SetDriver(_driver);
            NETClient.RecordStop(DeviceLogin);
            NETClient.StopTalk(_talkHandle);
            _talkHandle = IntPtr.Zero;
        }

        public IOPortState InputPortState()
        {
            return this._manufactureUri.InputPortState();
        }

        public IOPortState OuputPortState()
        {
            return this._manufactureUri.OuputPortState();
        }

        public void OuputPortChangeState(IOPortState state)
        {
            this._manufactureUri.OuputPortChangeState(state);
        }

        public bool ToogleSequencing(bool value)
        {
            SequencingStatus = value;
            if (this._sequencingUserControl != null)
            {
                this._sequencingUserControl.Visible = SequencingStatus;
            }

            return SequencingStatus;
        }

        private void OpenCameraAudio()
        {
            if (IsInitAudio && (Camera.AudioEnabled || Camera.TalkEnabled))
            {
                OnInitializeAudio?.Invoke(this, true);
            }
            IsInitAudio = false;
        }

        private void GetMaxExtraStream()
        {
            try
            {
                if (!_updateProfileStream)
                {
                    return;
                }

                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                query["action"] = "getProductDefinition";
                query["name"] = "MaxExtraStream";

                var url = new UriBuilder()
                {
                    Scheme = Camera.Protocol,
                    Host = Camera.Host,
                    Port = Camera.HttpPort,
                    Path = "/cgi-bin/magicBox.cgi",
                    Query = query.ToString()
                }.ToString();
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
                if (result.Contains("table.MaxExtraStream"))
                {
                    var maxExtraStream = int.Parse(result.Replace("table.MaxExtraStream=", "").Replace(Environment.NewLine, ""));
                    for (var i = 2; i <= maxExtraStream; i++)
                    {
                        if (i == 2)
                        {
                            _profilesList.Add(Profile.SubStream2);
                            OnAddExtraProfiles?.Invoke(this, Profile.SubStream2);
                        }
                        else if (i == 3)
                        {
                            _profilesList.Add(Profile.SubStream3);
                            OnAddExtraProfiles?.Invoke(this, Profile.SubStream3);
                        }
                    }
                    _updateProfileStream = false;
                }
            }
            catch (Exception)
            {

            }
        }

        private void GetConfiguredFps()
        {
            try
            {
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                query["action"] = "getConfig";
                query["name"] = "Encode";

                var url = new UriBuilder()
                {
                    Scheme = Camera.Protocol,
                    Host = Camera.Host,
                    Port = Camera.HttpPort,
                    Path = "/cgi-bin/configManager.cgi",
                    Query = query.ToString()
                }.ToString();
                var response = _manufactureUri.SendRequest(url, HttpMethod.Get);
                using (System.IO.StringReader reader = new System.IO.StringReader(response))
                {
                    string line = "";
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains($"Encode[{Camera.Channel - 1}].MainFormat[0].Video.FPS"))
                        {
                            var match = Regex.Match(line, @".Video.FPS=(\d+)");
                            if (match.Success && int.TryParse(match.Groups[1].Value, out int fps))
                            {
                                _configuredFps = fps;
                            }
                        }
                    }
                }

            }
            catch
            {
            }
        }

        private int GetFramesCountAndReset()
        {
            return Interlocked.Exchange(ref _framesCount, 0);
        }

        private void CheckAndSwitchStream()
        {
            int frames = GetFramesCountAndReset();
            _streamSwitcher.AddFrameCount(frames);

            double avgPercent = _streamSwitcher.GetAveragePercentage();

            Logger.Log($"CheckAndSwitchStream: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}. FPS: {_configuredFps} | Frames/min: {frames} | Avg %: {avgPercent:F1}%", LogPriority.Information);

            if (_streamSwitcher.ShouldSwitchToSubstream(Profile.MainStream == Profile))
            {
                ChangeProfile(Profile.SubStream, true);
                Logger.Log($"CheckAndSwitchStream: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}. 📉 Low FPS. Switching to substream.", LogPriority.Information);
            }
            else if (_streamSwitcher.ShouldSwitchToMainstream(!(Profile.MainStream == Profile)))
            {
                ChangeProfile(Profile.MainStream, true);
                Logger.Log($"CheckAndSwitchStream: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User}. 📈 FPS is good. Switching to main stream.", LogPriority.Information);
            }
        }

        private bool ExecuteWithContextRetry(Func<bool> action)
        {
            try
            {
                // Primary attempt to execute the SDK operation
                if (action()) return true;

                // If failed, force driver and try second time
                NETClient.SetDriver(_driver);
                var secondTry = action();
                if (!secondTry) {
                    var netClietError = NETClient.GetLastError();
                    Logger.Log($"ExecuteWithContextRetry driver {_driver} {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User.ToLower()} {_nameTab} {netClietError}", LogPriority.Information);
                };

                return secondTry;
            }
            catch (Exception ex)
            {
                Logger.Log($"Error ExecuteWithContextRetry driver {_driver} {ex.StackTrace}", LogPriority.Information);
                return false;
            }
            
        }
    }
}
