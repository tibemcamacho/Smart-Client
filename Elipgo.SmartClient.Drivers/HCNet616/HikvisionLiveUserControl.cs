using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Reflections;
using Elipgo.SmartClient.Common.Reflections.Manufactures;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Elipgo.SmartClient.Drivers.HCNet616
{
    public partial class HikvisionLiveUserControl : UserControl, IDriverLive, IDisposable, IConectionNotification
    {
        public ILoginControl loginControl = Locator.Current.GetService<ILoginControl>();
        public event OnDriverDispose OnDispose;
        private Int32 _realHandle = -1;
        private int _voiceComHandle = -1;
        public CHCNetSDK.NET_DVR_STREAM_MODE _strutStreamMode;
        private bool _painted = false;
        public bool IsSequencingEnabled => this.Camera.Sequencing != null;
        private Int32 DeviceLogin { get; set; }
        private ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        private delegate void SafeCallDelegate(PlaybackConnectionState connectionState);
        private PtzUserControl _ptzUserControl = null;
        private SequencingUserControl _sequencingUserControl;
        private CHCNetSDK.REALDATACALLBACK _realPlayDisConnectCallBack;
        private CHCNetSDK.EXCEPYIONCALLBACK _hikVisionExceptionCallBack;
        private CHCNetSDK.VOICEDATACALLBACKV30 _voiceDataCallback;
        private int _retryCount = 0;
        private readonly int _retryLimit;
        private int _tryCount = 0;
        private readonly int _tryLimit;
        private int _retryCallbackCount = 0;
        private readonly int _retryCallbackLimit;

        private readonly Random _random = new Random();
        private double _timeReConnectionCheck;
        private int _actualSize = 0;
        private int _zoomLimit = int.Parse(Common.Properties.Settings.Default["ZoomLimit"].ToString());
        private Dictionary<int, string> m_SaveDataHandleDict = new Dictionary<int, string>();
        private bool _isCallback = false;
        private int _startDigitalChannel = 0;
        private ManufactureUriAbstract _manufactureUri;

        private StreamSwitcher _streamSwitcher;
        private int _configuredFps = 0;
        private int _framesCount = 0;
        private volatile bool _disposed = false;

        private System.Timers.Timer _timerCheckFps = new System.Timers.Timer();

        public HikvisionLiveUserControl(CameraDTO camera, Profile profile, bool initAudio)
        {
            try
            {
                InitializeComponent();

                Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

                _retryLimit = int.Parse(config.AppSettings.Settings["retryLimit"].Value);
                _tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
                _retryCallbackLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);

                Camera = camera;
                Profile = profile;
                IsInitAudio = initAudio;
                ChangeProfile(Profile);

                this.Load += HikvisionLiveUserControl_Load;

                this.Click += HikvisionLiveUserControl_Click;
                this._picture.Click += HikvisionLiveUserControl_Click;
                this._panelFondoLogo.Click += HikvisionLiveUserControl_Click;

                this.DoubleClick += HikvisionLiveUserControl_DoubleClick; ;
                this._picture.DoubleClick += HikvisionLiveUserControl_DoubleClick;
                this._panelFondoLogo.DoubleClick += HikvisionLiveUserControl_DoubleClick;

                this.Paint += HikvisionLiveUserControl_Paint;
                this.Resize += HikvisionLiveUserControl_Resize;
                ListenStatus = false;
                ClipStatus = false;
                TalkStatus = false;
                PtzStatus = false;
                SequencingStatus = false;
                DigitalZoomStatus = false;
                InstantPlaybackStatus = false;

                _buttonZoomIn.MouseDown += ButtonZoomIn_MouseDown;
                _buttonZoomOut.MouseDown += ButtonZoomOut_MouseDown;

                _buttonZoomIn.MouseUp += ButtonZoomIn_MouseUp;
                _buttonZoomOut.MouseUp += ButtonZoomOut_MouseUp;
                this.MouseWheel += Picture_MouseWheel;
                this._timeReConnectionCheck = 5;
                _manufactureUri = new HikvisionUri(camera, Profile.None);

                bool res = CHCNetSDK.NET_DVR_Init();
                if (!res)
                {
                    _notification.Show("NetClient init failed!", null);
                }
                CHCNetSDK.NET_DVR_SetReconnect(20000, 1);

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
                _buttonZoomIn.SendToBack();
                _buttonZoomOut.SendToBack();
                _realPlayDisConnectCallBack = new CHCNetSDK.REALDATACALLBACK(HikVisionRealDataCallBack);
                _timerCheckFps.Interval = 60000; // 1 minuto
                _timerCheckFps.Elapsed += TimerCheckFps_Elapsed;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("HikvisionLiveUserControl Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
            }
        }

        private void TimerCheckFps_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckAndSwitchStream();
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
                    PTZControl(CHCNetSDK.ZOOM_IN, (uint)Camera.ZoomSensitivity, 1);
                }
                //Ctl+-
                if (keyData == (Keys.Control | Keys.OemMinus))
                {
                    PTZControl(CHCNetSDK.ZOOM_OUT, (uint)Camera.ZoomSensitivity, 1);

                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HikvisionLiveUserControl_DoubleClick(object sender, EventArgs e)
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
                Logger.Log(String.Format("HikvisionLiveUserControl_Click Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
            }
        }

        private void HikvisionLiveUserControl_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (_painted)
                {
                    Reconnecting.DisplayLogo(_picture.Width, _picture.Height, ref _panelConnection, ref _panelFondoLogo);
                    return;
                }

                _buttonZoomIn.Image = Common.Properties.FileResources.ptz_zoom_in;
                _buttonZoomOut.Image = Common.Properties.FileResources.ptz_zoom_out;
                _buttonZoomIn.Location = new Point(this.Width - 34, this.Height - 64);
                _buttonZoomOut.Location = new Point(this.Width - 34, this.Height - 34);
                _buttonTalk.Image = Common.Properties.FileResources.icon_micr_on;
                _buttonTalk.Location = new Point(this.Width - 50, this.Height - 40);
                _buttonTalk.Left = 15;
                _buttonTalk.Visible = false;

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
                _painted = true;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("HikvisionLiveUserControl_Paint Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
            }
        }

        private void SequencingUserControl_ButtonClick(object sender, int e)
        {
            OnSequencing?.Invoke(e);
        }

        private void HikvisionLiveUserControl_Resize(object sender, EventArgs e)
        {
            try
            {
                Reconnecting.DisplayLogo(_picture.Width, _picture.Height, ref _panelConnection, ref _panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("HikvisionLiveUserControl_Resize Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
            }
        }

        private void HikvisionLiveUserControl_Click(object sender, EventArgs e)
        {
            try
            {
                CameraSelected?.Invoke(this, Camera);

                if (DigitalZoomStatus)
                {
                    if (_picture.Location.X < 0)
                    {
                        var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);
                        var mp = new Point((100 * mouse.X) / this.Width, (100 * mouse.Y) / this.Height);
                        var p = new Point((_picture.Width * mp.X) / 100, (_picture.Height * mp.Y) / 100);
                        var picPosition = new Point((mouse.X - p.X), (mouse.Y - p.Y));
                        _picture.Location = picPosition;
                    }

                    this.Focus();
                    this.BringToFront();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("HikvisionLiveUserControl_Click Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
            }
        }

        private void HikvisionLiveUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                //CHCNetSDK.NET_DVR_Init();
                //CHCNetSDK.NET_DVR_SetReconnect(20000, 1);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("HikvisionLiveUserControl_Load Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
            }
        }

        private bool OpenCamera()
        {
            bool ret = RealStart(DeviceLogin, Camera.Channel, GetHandle());
            if (!ret)
            {
                Logger.Log(String.Format("HikVision OpenCamera Exception: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
            }
            return ret;
        }

        private bool Login()
        {
            try
            {
                IntPtr ptr;
                if (loginControl.GetDeviceLogin(Camera, out ptr, Common.Enum.Drivers.HCNetSDK_616, this))
                {
                    DeviceLogin = (Int32)ptr;
                    loginControl.AddChannel(Camera, this, Common.Enum.Drivers.HCNetSDK_616);
                    Logger.Log(String.Format("Reusing a Hikivion Login to Camara {0}", Camera.Name), LogPriority.Information);
                    return true;
                }

                CHCNetSDK.NET_DVR_DEVICEINFO_V30 deviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();
                //call login function.
                ushort dev_port = 0;
                try
                {
                    dev_port = Convert.ToUInt16(Camera.VideoPort);
                }
                catch (Exception ex)
                {
                    _notification.Show("ID_PORTERROR", null);
                    Logger.Log("ID_PORTERROR port: " + Camera.VideoPort + " Exception: " + ex.Message, LogPriority.Fatal);
                    return false;
                }
                DeviceLogin = CHCNetSDK.NET_DVR_Login_V30(Camera.Host, dev_port, Camera.User, Camera.Password, ref deviceInfo);
                if (DeviceLogin != -1)
                {
                    Logger.Log(String.Format("New Hikivion Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                    loginControl.AddDevice(Camera, (IntPtr)DeviceLogin, this, Common.Enum.Drivers.HCNetSDK_616);
                    this._startDigitalChannel = deviceInfo.byStartDChan;
                }
                else
                {
                    Logger.Log("Device Login is Zero " + CHCNetSDK.NET_DVR_GetCardLastError_Card(), LogPriority.Information);
                }
                return DeviceLogin != -1;
            }
            catch (Exception ex)
            {
                _notification.Show(ex.Message, null);
                Logger.Log(String.Format("Login Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
                return false;
            }

        }

        private bool RealStart(Int32 loginID, int channelNum, IntPtr handle)
        {
            try
            {
                bool response = false;
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = handle;
                lpPreviewInfo.lChannel = HikvisionHelper.GetChannelNumber(channelNum, this._startDigitalChannel);
                lpPreviewInfo.dwStreamType = (uint)Profile;//streamType; // Stream type:0-main stream, 1-sub stream, 2-stream 3, 3- virtual stream, and so on 
                lpPreviewInfo.dwLinkMode = 0; // Link mode: 0- TCP, 1- UDP, 2- multicast, 3- RTP, 4-RTP/RTSP, 5-RSTP/HTTP, 6- HRUDP 
                lpPreviewInfo.bBlocked = true; //0 - non - blocking stream getting, 1 - blocking stream getting. if it is block, there will be 5s timeout return when connect failed in the SDK, it is not suitable for polling stream gettitng.
                lpPreviewInfo.dwDisplayBufNum = 15; // The max buffer frames of player SDK,value range: 1, 6(default, self - adaptive mode), 15.It is 1 when setting to 0.
                lpPreviewInfo.byProtoType = 0;
                lpPreviewInfo.byPreviewMode = 0;
                IntPtr pUser = new IntPtr();
                _hikVisionExceptionCallBack = new CHCNetSDK.EXCEPYIONCALLBACK(HikVisionExceptionCallBack);
                _realHandle = CHCNetSDK.NET_DVR_RealPlay_V40(loginID, ref lpPreviewInfo, _realPlayDisConnectCallBack, pUser);
                if (_realHandle != -1)
                {
                    CHCNetSDK.NET_DVR_SetExceptionCallBack_V30(0, IntPtr.Zero, _hikVisionExceptionCallBack, pUser);
                    Task.Delay(2000).ContinueWith(t => CheckCallback());
                    response = true;
                }

                return response;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("RealStart Exception {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
                return false;
            }
        }

        private void HikVisionExceptionCallBack(uint dwType, int lUserID, int lHandle, IntPtr pUser)
        {
            switch (dwType)
            {
                case CHCNetSDK.EXCEPTION_RECONNECT:
                    _tryCount++;
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    Logger.Log(String.Format("HikVsion HikVisionExceptionCallBack Reconnect Event received :  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    break;
                //estos valores de eventos no figuran en la api, pero de manera empirica pude validar que se reciben cuando conecta la camara
                case 32789:
                case 32791:
                    _tryCount = 0;
                    SetVisivility(PlaybackConnectionState.Connected);
                    Logger.Log(String.Format("HikVsion HikVisionExceptionCallBack Reconnection Event received :  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    break;
                default:
                    break;

            }
            if (_tryCount >= _tryLimit)
            {
                SetVisivility(PlaybackConnectionState.Disconnected);
                Logger.Log(String.Format("HikVsion HikVisionExceptionCallBack alcanzo el maximo de reintento, estado desconectado:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
            }
        }
        private void HikVisionMessageCallback(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {

        }

        private void CheckCallback()
        {
            try
            {
                if (_isCallback || IsPlaying)
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
                        IsPlaying = OpenCamera();
                        if (!IsPlaying)
                        {
                            _tryCount = 0;
                            TryToReConnect();
                        }
                        Task.Delay(2000).ContinueWith(t => CheckCallback());
                    }
                    else
                    {
                        Logger.Log($"CheckCallback failed without video  set state as disconnected {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} ", LogPriority.Information);
                        SetVisivility(PlaybackConnectionState.Disconnected);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"CheckCallback Exception {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex} ", LogPriority.Fatal);
            }
        }

        private void HikVisionRealDataCallBack(int lRealHandle, uint dwDataType, IntPtr pBuffer, uint dwBufSize, IntPtr pUser)
        {
            try
            {
                Interlocked.Increment(ref _framesCount);

                if (_isCallback == true)
                {
                    return;
                }

                _isCallback = true;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("HikVisionRealDataCallBack Exception {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
            }
        }

        private IntPtr GetHandle()
        {
            try
            {
                if (_disposed || _picture == null || _picture.IsDisposed)
                {
                    return IntPtr.Zero;
                }
                return _picture.Handle;
            }
            catch (ObjectDisposedException)
            {
                // Control was disposed, return zero without logging as fatal
                return IntPtr.Zero;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("GetHandle Exception {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex), LogPriority.Fatal);
                return IntPtr.Zero;
            }
        }

        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }
        public List<Profile> Profiles => new List<Profile> { Profile.MainStream, Profile.SubStream };
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

        public bool ListenStatus { get; set; }
        public bool ClipStatus { get; set; }
        public bool TalkStatus { get; set; }
        public bool PtzStatus { get; set; }
        public bool SequencingStatus { get; set; }
        public bool DigitalZoomStatus { get; set; }
        public bool InstantPlaybackStatus { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsInitAudio { get; set; }

        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnVideoEventHandler OnVideo;
        public event ButtonPressedEventHandler PressedButtons;
        public event OnSequecingClick OnSequencing;
        public event OnInitializeAudioEventHandler OnInitializeAudio;
        public event OnAddExtraProfilesEventHandler OnAddExtraProfiles;

        public bool CallGuard(ActivateGuardDTO guard)
        {
            return false;
        }

        public bool CallPreset(PresetDTO preset)
        {
            try
            {
                var url = _manufactureUri.CallPresetUri(preset);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Put, "");
                string strResult = result.ToString().Replace("\r\n", "").Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Replace("version=\"2.0\" xmlns=\"http://www.hikvision.com/ver20/XMLSchema\"", "").Trim();
                var xdoc = XDocument.Parse(strResult);
                return (string)xdoc.Root.Elements("subStatusCode").FirstOrDefault() == "ok";
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeProfile(Profile profile, bool autoSwitching = false)
        {
            if (Profile != profile && _realHandle != -1)
            {
                Stop();
                Profile = profile;
                Play();
                _isCallback = true;
                return true;
            }
            return true;
        }

        public void Connect(IntPtr HandledBrocaster)
        {
            int r = ((int)(((_random.NextDouble() * _timeReConnectionCheck) + 1) * 1000));
            Task.Delay(r).ContinueWith(t => TryToReConnect());
        }

        public void Disconect(IntPtr HandledBrocaster)
        {
            Logger.Log(String.Format(" Disconect hik disconnected {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
            SetVisivility(PlaybackConnectionState.Reconnecting);
            CHCNetSDK.NET_DVR_StopRealPlay(_realHandle);
        }
        private void TryToReConnect()
        {
            try
            {
                if (_tryCount >= _tryLimit)
                {
                    SetVisivility(PlaybackConnectionState.Disconnected);
                    Logger.Log(String.Format("HikVsion TryToReConnect reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    return;
                }

                SetVisivility(PlaybackConnectionState.Reconnecting);
                _tryCount++;
                if (RealStart(DeviceLogin, Camera.Channel, GetHandle()) == false)
                {
                    int r = ((int)(((_random.NextDouble() * _timeReConnectionCheck) + 1) * 1000));
                    Logger.Log(String.Format(" TryToReConnect RealStar failed  {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    Task.Delay(r).ContinueWith(t => TryToReConnect());
                }
                else
                {
                    Logger.Log(String.Format(" TryToReConnect RealStar Connected  {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Connected);
                    IsPlaying = true;
                    _tryCount = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format(" TryToReConnect Exception: {4}  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, ex.Message), LogPriority.Fatal);
            }
        }

        public new void Dispose()
        {
            if (_disposed) return;
            _disposed = true;

            try
            {
                Logger.Log(String.Format(" Dispose Hik {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                if (loginControl.RemoveChannelAndCanLogout(Camera, this, Common.Enum.Drivers.HCNetSDK_616))
                {
                    Logger.Log(String.Format(" Dispose Logout Hik {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    CHCNetSDK.NET_DVR_Logout(DeviceLogin);
                }
                _realPlayDisConnectCallBack = null;
                CHCNetSDK.NET_DVR_CloseSound();
                CHCNetSDK.NET_DVR_StopSaveRealData(_realHandle);
                CHCNetSDK.NET_DVR_StopRealPlay(_realHandle);
                CHCNetSDK.NET_DVR_Cleanup();
                this.Controls.Clear();
                this.MouseWheel -= Picture_MouseWheel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void DisposeDragged()
        {
            CHCNetSDK.NET_DVR_CloseSound();
            CHCNetSDK.NET_DVR_StopSaveRealData(_realHandle);
            CHCNetSDK.NET_DVR_StopRealPlay(_realHandle);
        }

        public GuardForCreationDTO GetGuard(int guardId)
        {
            throw new NotImplementedException();
        }

        public GuardDTO[] ListGuards()
        {
            throw new NotImplementedException();
        }

        public PresetDTO[] ListPresets()
        {
            var response = new List<PresetDTO>();
            try
            {
                string url = _manufactureUri.PresetListUri();
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get, "");
                string strResult = result.ToString().Replace("\r\n", "").Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Replace("version=\"1.0\" xmlns=\"http://www.hikvision.com/ver20/XMLSchema\"", "").Replace("version=\"2.0\" xmlns=\"http://www.hikvision.com/ver20/XMLSchema\"", "").Trim();
                var xdoc = XDocument.Parse(strResult);
                foreach (var f in xdoc.Root.Elements("PTZPreset"))
                {
                    //if (Boolean.Parse((string)f.Element("enabled")))
                    //{
                    response.Add(new PresetDTO()
                    {
                        //var enabled = Boolean.Parse((string)f.Element("enabled"));
                        Name = (string)f.Element("presetName"),
                        Id = int.Parse((string)f.Element("id")),
                        Enabled = bool.Parse((string)f.Element("enabled"))
                    });
                    //}
                }
            }
            catch (Exception)
            {
                return new PresetDTO[0];
            }
            return response.ToArray();
        }


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
                    Logger.Log(String.Format(" Play Hik entered {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Connecting);
                    if (Login())
                    {
                        IsPlaying = OpenCamera();
                        if (!IsPlaying)
                        {
                            _tryCount = 0;
                            TryToReConnect();
                        }
                        else
                        {
                            Logger.Log(String.Format(" Play Hik Connected {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                            SetVisivility(PlaybackConnectionState.Connected);
                        }
                    }
                    else
                    {
                        IsPlaying = false;
                        _retryCount++;
                        if (_retryCount <= _retryLimit)
                        {
                            SetVisivility(PlaybackConnectionState.Reconnecting);
                            Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * _retryCount) }, () => Play());
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
                catch (Exception ex)
                {
                    Logger.Log(String.Format("Play Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                    _notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                    IsPlaying = false;
                }
            }).ContinueWith(x => { x.Dispose(); });
            return IsPlaying;
        }

        public bool RemoveGuard(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool RemovePreset(PresetDTO preset)
        {
            try
            {
                var url = _manufactureUri.DeletePresetUri(preset);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Delete, string.Format(@"<PTZPreset><id>{0}</id><enabled>false</enabled><presetName>{1}</presetName></PTZPreset>", preset.Id, preset.Name));
                string strResult = result.ToString().Replace("\r\n", "").Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Replace("version=\"2.0\" xmlns=\"http://www.hikvision.com/ver20/XMLSchema\"", "").Trim();
                var xdoc = XDocument.Parse(strResult);
                return (string)xdoc.Root.Elements("subStatusCode").FirstOrDefault() == "ok";
            }
            catch
            {
                return false;

            }

        }

        public bool SaveGuard(GuardForCreationDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool SavePreset(PresetDTO preset)
        {
            try
            {
                var url = _manufactureUri.SavePresetUri(preset);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Put, string.Format(@"<PTZPreset><id>{0}</id><enabled>true</enabled><presetName>{1}</presetName></PTZPreset>", preset.Id, preset.Name));
                string strResult = result.ToString().Replace("\r\n", "").Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "").Replace("version=\"2.0\" xmlns=\"http://www.hikvision.com/ver20/XMLSchema\"", "").Trim();
                var xdoc = XDocument.Parse(strResult);
                return (string)xdoc.Root.Elements("subStatusCode").FirstOrDefault() == "ok";
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Snapshot(string path, string token, int id)
        {
            try
            {
                CHCNetSDK.NET_DVR_JPEGPARA lpJpegPara = new CHCNetSDK.NET_DVR_JPEGPARA
                {
                    wPicQuality = 0,
                    wPicSize = 0xff
                };

                bool ret = CHCNetSDK.NET_DVR_CaptureJPEGPicture(DeviceLogin, HikvisionHelper.GetChannelNumber(Camera.Channel, this._startDigitalChannel), ref lpJpegPara, path);
                return ret;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool StateGuard(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool Stop()
        {
            _tryCount = _tryLimit;
            if (!IsPlaying)
            {
                return true;
            }

            ListenStatus = false;
            CHCNetSDK.NET_DVR_CloseSound();
            CHCNetSDK.NET_DVR_StopSaveRealData(_realHandle);
            CHCNetSDK.NET_DVR_StopRealPlay(_realHandle);

            if (!CHCNetSDK.NET_DVR_StopRealPlay(_realHandle))
            {
                var iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                Logger.Log($"NET_DVR_StopRealPlay failed, error code: {iLastErr}", LogPriority.Fatal);
            }
            else
            {
                Logger.Log($"NET_DVR_StopRealPlay succeeded!", LogPriority.Information);
            }
            _realHandle = -1;
            IsPlaying = false;
            return true;
        }

        public bool StopGuard(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public void SubcribePTZEvent()
        {
            _unsubcribedPTZEvent = false;
            _ptzUserControl.PtzJoystickStateEvent += PtzJoystickStateEvent;
            _ptzUserControl.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
        }

        public void ToggleFullScreen()
        {
            throw new NotImplementedException();
        }

        public bool ToggleInstantPlayback()
        {
            InstantPlaybackStatus = !InstantPlaybackStatus;

            return InstantPlaybackStatus;
        }

        public bool ToggleListen(bool Listen)
        {
            try
            {
                //ListenStatus = !ListenStatus;

                if (Listen)
                {
                    CHCNetSDK.NET_DVR_OpenSound(_realHandle);
                }
                else
                {
                    CHCNetSDK.NET_DVR_CloseSound();
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
                if (TalkStatus == false)
                {
                    _voiceDataCallback = new CHCNetSDK.VOICEDATACALLBACKV30(VoiceDataCallBack);

                    _voiceComHandle = CHCNetSDK.NET_DVR_StartVoiceCom_V30(DeviceLogin, 1, true, _voiceDataCallback, IntPtr.Zero);
                    if (_voiceComHandle < 0)
                    {
                        return false;
                    }
                }
                else
                {
                    if (!CHCNetSDK.NET_DVR_StopVoiceCom(_voiceComHandle))
                    {
                        return false;
                    }
                }
                TalkStatus = !TalkStatus;
                _buttonTalk.Visible = TalkStatus;
                return true;
            }
            catch (Exception)
            {
                TalkStatus = false;
                return false;
            }
        }

        public void VoiceDataCallBack(int lVoiceComHandle, IntPtr pRecvDataBuffer, uint dwBufSize, byte byAudioFlag, System.IntPtr pUser)
        {
        }

        public bool ToogleDigitalZoom()
        {
            if (DigitalZoomStatus)
            {
                _picture.Size = _picture.Parent.Size;
                _picture.Location = new Point(0, 0);
                _picture.Visible = true;
                this.Cursor = Cursors.Default;
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
            Size picSize;
            if (direction)
            {
                picSize = new Size((int)(_picture.Width * 1.1), (int)(_picture.Height * 1.1));
            }
            else
            {
                picSize = new Size((int)(_picture.Width * 0.9), (int)(_picture.Height * 0.9));
                if (picSize.Width < _picture.Parent.Size.Width)
                {
                    picSize = _picture.Parent.Size;
                }
            }

            var mp = new Point((100 * eventPositionX) / this.Width, (100 * eventPositionY) / this.Height);
            var p = new Point((picSize.Width * mp.X) / 100, (picSize.Height * mp.Y) / 100);
            var picPosition = new Point((eventPositionX - p.X), (eventPositionY - p.Y));
            if (picSize == _picture.Parent.Size)
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
                Image = _picture.Image,
                Size = _picture.Size,
                Location = _picture.Location,
                Visible = true
            };

            this.Controls.Add(temp);
            temp.BringToFront();
            //picture.Visible = false;

            this._actualSize = direction ? this._actualSize + 1 : this._actualSize - 1;

            _picture.Size = picSize;
            _picture.Location = picPosition;
            _picture.Visible = true;
            this.Controls.Remove(temp);
        }

        public bool TooglePtz()
        {
            if (_ptzUserControl.InvokeRequired)
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
                    _buttonZoomIn.BringToFront();
                    _buttonZoomOut.BringToFront();
                    _ptzUserControl.BringToFront();
                    _ptzUserControl.StartJoystick();
                    _ptzUserControl.PtzJoystickStateEvent += PtzJoystickStateEvent;
                    _ptzUserControl.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
                }
                else
                {
                    _buttonZoomIn.SendToBack();
                    _buttonZoomOut.SendToBack();
                    _ptzUserControl.SendToBack();
                    _ptzUserControl.StopJoystick();
                    _ptzUserControl.PtzJoystickStateEvent -= PtzJoystickStateEvent;
                    _ptzUserControl.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
                }
            }
            _buttonZoomIn.Visible = PtzStatus;
            _buttonZoomOut.Visible = PtzStatus;
            return PtzStatus;
        }

        private bool _unsubcribedPTZEvent = false;
        public void UnsubcribePTZEvent()
        {
            _ptzUserControl.PtzJoystickStateEvent -= PtzJoystickStateEvent;
            _ptzUserControl.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
            //cuando se utiliza el joytick y la camara ptz se comienza a mover, si se selecciona otra camara se ejecuta este metodo
            //realizando la desubcripcion a los eventos del joystick, pero ocurre un problema si el usuario mantiene el joystick en un estado de desplazamiento 
            //y se selecciona una nueva camara, esta camara nunca recibe el evento que pare y continua loca en forma indefinida hasta se vuelva a selecciona y ejecutar un 
            //nuevo comando con su stop correspondiente es por esto que 
            //envio un comando cualquiera para detener el movimiento de la camara cuando se descelecciona la camara forzando a parar este o no en ejecuccion
            PTZControl(CHCNetSDK.TILT_UP, (uint)Camera.MovementSensitivity, 1);
            _unsubcribedPTZEvent = true;
        }

        public bool VideoClipStart(string path)
        {
            try
            {
                path = path.Replace(".mp4", "._mp4");
                CHCNetSDK.NET_DVR_MakeKeyFrame(DeviceLogin, HikvisionHelper.GetChannelNumber(Camera.Channel, this._startDigitalChannel));
                bool ret = CHCNetSDK.NET_DVR_SaveRealData(_realHandle, path);
                if (ret)
                {
                    m_SaveDataHandleDict.Add(_realHandle, path);
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
                bool ret = CHCNetSDK.NET_DVR_StopSaveRealData(_realHandle);
                if (ret)
                {
                    if (m_SaveDataHandleDict.ContainsKey(_realHandle))
                    {
                        var path = m_SaveDataHandleDict[_realHandle];
                        m_SaveDataHandleDict.Remove(_realHandle);
                        VideoConversorServices.Instance.ConvertFileMediaToMp4(path);
                    }
                }
                ClipStatus = false;
                return ret;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, null);
                ClipStatus = false;
                return false;
            }
        }

        public bool Volume(int value)
        {
            throw new NotImplementedException();
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
                        _panelConnection.BringToFront();
                        _panelConnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.disconnected_es) : new Bitmap(Properties.Resources.disconnected_en);
                        break;
                    case PlaybackConnectionState.Connected:
                        _panelConnection.SendToBack();
                        _picture.BringToFront();
                        if (_ptzUserControl != null && _ptzUserControl.Visible)
                        {
                            _ptzUserControl.BringToFront();
                            _buttonZoomIn.BringToFront();
                            _buttonZoomOut.BringToFront();
                        }
                        break;
                    case PlaybackConnectionState.NoRecording:
                        _panelConnection.BringToFront();
                        _panelConnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.norecording_es) : new Bitmap(Properties.Resources.norecording_en);
                        break;
                    case PlaybackConnectionState.Reconnecting:
                        _panelConnection.BringToFront();
                        _panelConnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.reconnecting_es) : new Bitmap(Properties.Resources.reconnecting_en);
                        break;
                    case PlaybackConnectionState.Connecting:
                        _panelConnection.BringToFront();
                        _panelConnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.connecting_es) : new Bitmap(Properties.Resources.connecting_en);
                        break;
                }
                Reconnecting.DisplayLogo(_picture.Width, _picture.Height, ref _panelConnection, ref _panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"SetVisivility Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        private void PTZControl(uint ptzCommand, uint speed, uint stop)
        {
            try
            {
                CHCNetSDK.NET_DVR_PTZControlWithSpeed(_realHandle, ptzCommand, stop, speed);
            }
            catch (Exception)
            {
            }
        }

        /*
         * PUT /PTZ/channels/1/PTZControl?command=XXX&mode=YY&speed=N
        command, mode and speed are three primary parameters. 
        The common options of command are as follows:
         PAN_LEFT : 
         PAN_RIGHT: 
         TILT_UP: 
         TILT_DOWN: 
         ZOOM_IN: 
         ZOOM_OUT: 
         UP_LEFT: 
         UP_RIGHT: 
         DOWN_LEFT: 
         DOWN_RIGHT: 
         PAN_AUTO: 
        mode includes two options: start and stop
        speed stands for the speed that PTZ rotates. The value ranges from 1 to 7, the greater the */

        private void PtzUserControl_ButtonMouseUp(object sender, PtzMovement e)
        {
            switch (e)
            {
                case PtzMovement.Up:
                    PTZControl(CHCNetSDK.TILT_UP, (uint)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.Down:
                    PTZControl(CHCNetSDK.TILT_DOWN, (uint)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.Left:
                    PTZControl(CHCNetSDK.PAN_LEFT, (uint)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.Right:
                    PTZControl(CHCNetSDK.PAN_RIGHT, (uint)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.Center:
                    break;
                case PtzMovement.UpLeft:
                    PTZControl(CHCNetSDK.UP_LEFT, (uint)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.DownLeft:
                    PTZControl(CHCNetSDK.DOWN_LEFT, (uint)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.UpRight:
                    PTZControl(CHCNetSDK.UP_RIGHT, (uint)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.DownRight:
                    PTZControl(CHCNetSDK.DOWN_RIGHT, (uint)Camera.MovementSensitivity, 1);
                    break;
            }
        }
        private void PtzUserControl_ButtonMouseDown(object sender, PtzMovement e)
        {
            switch (e)
            {
                case PtzMovement.Up:
                    PTZControl(CHCNetSDK.TILT_UP, (uint)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.Down:
                    PTZControl(CHCNetSDK.TILT_DOWN, (uint)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.Left:
                    PTZControl(CHCNetSDK.PAN_LEFT, (uint)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.Right:
                    PTZControl(CHCNetSDK.PAN_RIGHT, (uint)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.Center:
                    break;
                case PtzMovement.UpLeft:
                    PTZControl(CHCNetSDK.UP_LEFT, (uint)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.DownLeft:
                    PTZControl(CHCNetSDK.DOWN_LEFT, (uint)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.UpRight:
                    PTZControl(CHCNetSDK.UP_RIGHT, (uint)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.DownRight:
                    PTZControl(CHCNetSDK.DOWN_RIGHT, (uint)Camera.MovementSensitivity, 0);
                    break;
            }
        }
        private void ButtonZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
            PTZControl(CHCNetSDK.ZOOM_OUT, (uint)Camera.ZoomSensitivity, 1);
        }

        private void ButtonZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            PTZControl(CHCNetSDK.ZOOM_IN, (uint)Camera.ZoomSensitivity, 1);
        }

        private void ButtonZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            PTZControl(CHCNetSDK.ZOOM_OUT, (uint)Camera.ZoomSensitivity, 0);
        }

        private void ButtonZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            PTZControl(CHCNetSDK.ZOOM_IN, (uint)Camera.ZoomSensitivity, 0);
        }

        private List<ActionCommand> ExecuteZoomCommand(List<ActionCommand> pressedButtons)
        {
            foreach (ActionCommand act in pressedButtons.Where(x => (x.command == PtzCommand.ZOOM_ADD_CONTROL || x.command == PtzCommand.ZOOM_DEC_CONTROL)).ToList())
            {
                PTZControl(ParseToHikCommand((ButtonOrAxis)System.Enum.Parse(typeof(ButtonOrAxis), act.command.ToString())), (uint)(act.Parameter * Camera.ZoomSensitivity), !act.isInvoked == true ? (uint)1 : (uint)0);
            }
            return pressedButtons.Where(x => !(x.command == PtzCommand.ZOOM_ADD_CONTROL || x.command == PtzCommand.ZOOM_DEC_CONTROL)).ToList();
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
                            _notification.Show(string.Format(Common.Properties.Resources.PresetStarted, id.ToString()), null);
                        }
                        else
                        {
                            _notification.Show(Common.Properties.Resources.PresetErrorActivate, null);
                        }
                    }
                    else
                    {
                        var message = string.Format("{0}", this.Camera.Name);
                        _notification.Show(string.Format(Common.Properties.Resources.NoPresetAvailable, message), null);
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
                PTZControl(ParseToHikCommand(act.buttonOrAxis), (uint)(act.Parameter * Camera.MovementSensitivity), !act.isInvoked == true ? (uint)1 : (uint)0);
            }
        }

        private uint ParseToHikCommand(ButtonOrAxis command)
        {
            uint result = CHCNetSDK.NOACTION;
            switch (command)
            {
                case ButtonOrAxis.UP_CONTROL:
                    result = CHCNetSDK.TILT_UP;
                    break;
                case ButtonOrAxis.DOWN_CONTROL:
                    result = CHCNetSDK.TILT_DOWN;
                    break;
                case ButtonOrAxis.LEFT_CONTROL:
                    result = CHCNetSDK.PAN_LEFT;
                    break;
                case ButtonOrAxis.RIGHT_CONTROL:
                    result = CHCNetSDK.PAN_RIGHT;
                    break;
                case ButtonOrAxis.LEFTTOP:
                    result = CHCNetSDK.UP_LEFT;
                    break;
                case ButtonOrAxis.LEFTDOWN:
                    result = CHCNetSDK.DOWN_LEFT;
                    break;
                case ButtonOrAxis.RIGHTTOP:
                    result = CHCNetSDK.UP_RIGHT;
                    break;
                case ButtonOrAxis.RIGHTDOWN:
                    result = CHCNetSDK.DOWN_RIGHT;
                    break;
                case ButtonOrAxis.ZOOM_ADD_CONTROL:
                    result = CHCNetSDK.ZOOM_IN;
                    break;
                case ButtonOrAxis.ZOOM_DEC_CONTROL:
                    result = CHCNetSDK.ZOOM_OUT;
                    break;
            }
            return result;
        }

        public bool ToggleTalk(bool talkStatus)
        {
            try
            {
                if (talkStatus)
                {
                    if (!TalkStatus)
                    {
                        CHCNetSDK.VOICEDATACALLBACKV30 VoiceData = new CHCNetSDK.VOICEDATACALLBACKV30(VoiceDataCallBack);//Ô¤ÀÀÊµÊ±Á÷»Øµ÷º¯Êý

                        _voiceComHandle = CHCNetSDK.NET_DVR_StartVoiceCom_V30(DeviceLogin, 1, true, VoiceData, IntPtr.Zero);
                        if (_voiceComHandle < 0)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (TalkStatus)
                    {
                        if (!CHCNetSDK.NET_DVR_StopVoiceCom(_voiceComHandle))
                        {
                            return false;
                        }
                    }
                }
                TalkStatus = talkStatus;
                _buttonTalk.Visible = TalkStatus;
                return true;
            }
            catch (Exception)
            {
                TalkStatus = false;
                return false;
            }
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

        private void GetConfiguredFps()
        {
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
    }
}
