using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Reflections;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Elipgo.SmartClient.Drivers.UNVNet231
{
    public partial class UniviewLiveUserControl : UserControl, IDriverLive, IDisposable, IConectionNotification
    {
        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        public ILoginControl loginControl = Locator.Current.GetService<ILoginControl>();
        private readonly Random _random = new Random();
        private bool _isFullScreen;
        private PtzUserControl ptzUserControl1 = null;
        private SequencingUserControl SequencingUserControl;
        private bool IsCallback = false;
        private int retryCallbackCount = 0;
        //private StreamSwitcher _streamSwitcher;
        //private int _configuredFps = 0;
        //private int _framesCount = 0;
        //private bool _autoSwitching = false;

        public bool IsSequencingEnabled => this.Camera.Sequencing != null;
        private readonly int retryCallbackLimit;
        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }
        public List<Profile> Profiles
        {
            get => new List<Profile> { Profile.MainStream, Profile.SubStream };
        }
        NETDEVSDK.NETDEV_ExceptionCallBack_PF excepCB = null;
        delegate void RealDataCallBack(IntPtr lpPlayHandle, byte[] pucBuffer, Int32 dwBufSize, Int32 dwMediaDataType, IntPtr lpUserParam);
        RealDataCallBack cb = null;

        public List<ButtonsContextBar> Commands => GetButtons();
        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();
        public event OnDriverDispose OnDispose;

        public bool ListenStatus { get; set; }
        public bool ClipStatus { get; set; }
        public bool TalkStatus { get; set; }
        public bool PtzStatus { get; set; }
        public bool SequencingStatus { get; set; }
        public bool DigitalZoomStatus { get; set; }
        public bool InstantPlaybackStatus { get; set; }
        public bool IsPlaying { get; set; }

        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnVideoEventHandler OnVideo;
        public event ButtonPressedEventHandler PressedButtons;
        public event OnSequecingClick OnSequencing;
        public event OnInitializeAudioEventHandler OnInitializeAudio;
        public event OnAddExtraProfilesEventHandler OnAddExtraProfiles;

        private int retryCount = 0;
        private readonly int retryLimit;
        private IntPtr DeviceLogin;
        public bool _painted { get; set; } = false;
        public bool IsInitAudio { get; set; }

        private IntPtr realHandle = IntPtr.Zero;
        private delegate void SafeCallDelegate(PlaybackConnectionState connectionState);
        private int _actualSize = 0;
        private int tryCount = 0;
        private int _zoomLimit = int.Parse(Common.Properties.Settings.Default["ZoomLimit"].ToString());
        private readonly int tryLimit;
        private double TimeReConnectionCheck;
        private DispatcherTimer timerCheckConecction = null;
        private DateTime dtFechaUltimoBuffer;
        NETDEV_REV_TIMEOUT_S stRevTimeout;
        private ManufactureUriAbstract manufactureUri;

        public UniviewLiveUserControl(CameraDTO camera, Profile profile, bool initAudio, bool isfullscreen = false)
        {
            InitializeComponent();
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
            tryLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);
            retryLimit = int.Parse(config.AppSettings.Settings["retryLimit"].Value);
            retryCallbackLimit = int.Parse(config.AppSettings.Settings["tryLimit"].Value);

            int iRet = NETDEVSDK.NETDEV_Init();

            if (NETDEVSDK.TRUE != iRet)
            {
                Logger.Log("it is not a admin oper", LogPriority.Fatal);
                return;
            }
            else
            {
                Logger.Log("init is correct", LogPriority.Information);
            }

            Camera = camera;
            Profile = profile;
            IsInitAudio = initAudio;
            _isFullScreen = isfullscreen;

            this.Load += UniviewLiveUserControl_Load;

            this.Click += UniviewLiveUserControl_Click;
            this.picture.Click += UniviewLiveUserControl_Click;
            this.panelFondoLogo.Click += UniviewLiveUserControl_Click;

            this.DoubleClick += UniviewLiveUserControl_DoubleClick;
            this.picture.DoubleClick += UniviewLiveUserControl_DoubleClick;
            this.panelFondoLogo.DoubleClick += UniviewLiveUserControl_DoubleClick;

            this.Paint += UniviewLiveUserControl_Paint;
            this.Resize += UniviewLiveUserControl_Resize;
            this.MouseWheel += Picture_MouseWheel;
            this.TimeReConnectionCheck = 5;
            excepCB = new NETDEVSDK.NETDEV_ExceptionCallBack_PF(NETDEV_ExceptionCallBack_PF);
            iRet = NETDEVSDK.NETDEV_SetExceptionCallBack(excepCB, IntPtr.Zero);
            if (NETDEVSDK.FALSE == iRet)
            {
                Logger.Log(string.Format("failed to set exception callback {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Information);
            }
            if (Camera.PtzEnabled)
            {
                ptzUserControl1 = new PtzUserControl
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(263, 9),
                    Margin = new Padding(0),
                    Name = "ptzUserControl1",
                    Size = new Size(72, 72),
                    Visible = false,
                    TabIndex = 1
                };
                this.Controls.Add(ptzUserControl1);
                ptzUserControl1.Location = new Point(this.Width - 82, 34);
                //ptzUserControl1.ButtonMouseDown += PtzUserControl1_ButtonMouseDown;
                //ptzUserControl1.ButtonMouseUp += PtzUserControl1_ButtonMouseUp;
                ptzUserControl1.BringToFront();
            }
            ButtonZoomIn.SendToBack();
            ButtonZoomOut.SendToBack();
        }

        private void UniviewLiveUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick?.Invoke(this);
            }
        }

        private void UniviewLiveUserControl_Resize(object sender, EventArgs e)
        {
            Reconnecting.DisplayLogo(picture.Width, picture.Height, ref panelconnection, ref panelFondoLogo);
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

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void UniviewLiveUserControl_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (_painted)
                {
                    Reconnecting.DisplayLogo(picture.Width, picture.Height, ref panelconnection, ref panelFondoLogo);
                    return;
                }

                ButtonZoomIn.Image = Common.Properties.FileResources.ptz_zoom_in;
                ButtonZoomOut.Image = Common.Properties.FileResources.ptz_zoom_out;
                ButtonZoomIn.Location = new Point(this.Width - 34, this.Height - 64);
                ButtonZoomOut.Location = new Point(this.Width - 34, this.Height - 34);
                //ButtonZoomIn.Visible = false;
                //ButtonZoomOut.Visible = false;

                _painted = true;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("UniviewLiveUserControl_Paint Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User, ex), LogPriority.Fatal);
            }
        }

        private void SequencingUserControl_ButtonClick(object sender, int e)
        {
            OnSequencing?.Invoke(e);
        }

        private void UniviewLiveUserControl_Click(object sender, EventArgs e)
        {
            CameraSelected?.Invoke(this, Camera);

            if (DigitalZoomStatus)
            {
                if (picture.Location.X < 0)
                {
                    var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);
                    var mp = new Point((100 * mouse.X) / this.Width, (100 * mouse.Y) / this.Height);
                    var p = new Point((picture.Width * mp.X) / 100, (picture.Height * mp.Y) / 100);
                    var picPosition = new Point((mouse.X - p.X), (mouse.Y - p.Y));
                    picture.Location = picPosition;
                }

                this.Focus();
                this.BringToFront();
            }
        }

        private void UniviewLiveUserControl_Load(object sender, EventArgs e)
        {
            //int iRet = NETDEVSDK.NETDEV_Init();

            //if (NETDEVSDK.TRUE != iRet)
            //{
            //    Logger.Log("it is not a admin oper", LogPriority.Fatal);
            //}
        }

        public bool CallGuard(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool CallPreset(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public bool ChangeProfile(Profile profile, bool autoSwitching = false)
        {
            if (Profile != profile && realHandle != IntPtr.Zero)
            {
                Stop();
                Profile = profile;
                return Play();
            }
            return true;
        }

        public void Connect(IntPtr HandledBrocaster)
        {
            int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
            Task.Delay(r).ContinueWith(t => TryToReConnect());
        }

        public void Disconect(IntPtr HandledBrocaster)
        {
            SetVisivility(PlaybackConnectionState.Reconnecting);
            if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_StopRealPlay(realHandle))
            {

                Logger.Log(String.Format(" Error disconnected {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
            }
            Logger.Log(String.Format(" Disconect uniview disconnected {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
        }

        public void DisposeDragged()
        {
            throw new NotImplementedException();
        }

        public GuardForCreationDTO GetGuard(int guardId)
        {
            throw new NotImplementedException();
        }

        public GuardDTO[] ListGuards()
        {
            //throw new NotImplementedException();
            return new GuardDTO[] { };
        }

        public PresetDTO[] ListPresets()
        {
            return new PresetDTO[] { };
            //throw new NotImplementedException();
        }

        public bool Play()
        {
            if (IsPlaying)
            {
                return true;
            }

            try
            {
                Logger.Log(String.Format(" Play uniview entered {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                SetVisivility(PlaybackConnectionState.Connecting);
                if (Login())
                {
                    IsPlaying = OpenCamera();
                    if (!IsPlaying)
                    {
                        tryCount = 0;
                        TryToReConnect();
                    }
                    else
                    {
                        Logger.Log(String.Format(" Play Uniview Connected {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                        SetVisivility(PlaybackConnectionState.Connected);
                    }
                }
                else
                {
                    IsPlaying = false;
                    retryCount++;
                    if (retryCount <= retryLimit)
                    {
                        SetVisivility(PlaybackConnectionState.Reconnecting);
                        Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * retryCount) }, () => Play());
                        Logger.Log(String.Format("Error al realizar el Login Camera:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                        retryCount++;
                    }
                    else
                    {
                        SetVisivility(PlaybackConnectionState.Disconnected);
                        Logger.Log(String.Format("Uniview Error al realizar el Login Camera alcanzo el maximo de reintentos, estado desconectado:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Play Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                return false;
            }
            return IsPlaying;
        }

        public bool RemoveGuard(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool RemovePreset(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public bool SaveGuard(GuardForCreationDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool SavePreset(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Snapshot(string path, string token, int id)
        {
            byte[] picSavePath;
            GetUTF8Buffer(path, NETDEVSDK.NETDEV_LEN_260, out picSavePath);
            int iRet = NETDEVSDK.NETDEV_CapturePicture(realHandle, picSavePath, (int)NETDEV_PICTURE_FORMAT_E.NETDEV_PICTURE_BMP);
            if (NETDEVSDK.FALSE == iRet)
            {
                Logger.Log(string.Format("Error to Snapshot {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
                return false;
            }
            return true;
        }

        public bool StateGuard(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool Stop()
        {
            timerCheckConecction.Stop();
            ListenStatus = false;
            NETDEVSDK.NETDEV_CloseSound(realHandle);
            NETDEVSDK.NETDEV_StopSaveRealData(realHandle);
            if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_StopRealPlay(realHandle))
            {
                Logger.Log(string.Format("Error to Stop {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Fatal);
            }
            IsPlaying = false;
            realHandle = IntPtr.Zero;
            return true;
        }

        public bool StopGuard(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public void SubcribePTZEvent()
        {
            throw new NotImplementedException();
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
                    RealStart(fullscreen.pHandle);
                });
                fullscreen.ShowDialog();
                Profile = tempProfile;
                RealStart(GetHandle());
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("ToggleFullScreen Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
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
                    NETDEVSDK.NETDEV_OpenSound(realHandle); //call opensound function.
                }
                else
                {
                    NETDEVSDK.NETDEV_CloseSound(realHandle);
                }

                return Listen;
            }
            catch (Exception ex)
            {
                notification.Show(ex.Message, null);
                ListenStatus = false;
                return false;
            }
        }

        public bool ToggleTalk()
        {
            throw new NotImplementedException();
        }

        public bool ToogleDigitalZoom()
        {
            if (DigitalZoomStatus)
            {
                picture.Size = picture.Parent.Size;
                picture.Location = new Point(0, 0);
                picture.Visible = true;
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

        public bool TooglePtz()
        {
            return true;
            //throw new NotImplementedException();
        }

        public void UnsubcribePTZEvent()
        {
            //throw new NotImplementedException();
        }

        public bool VideoClipStart(string path)
        {
            byte[] localRecordPath;
            GetUTF8Buffer(path, NETDEVSDK.NETDEV_LEN_260, out localRecordPath);
            int iRet = NETDEVSDK.NETDEV_SaveRealData(realHandle, localRecordPath, (int)NETDEV_MEDIA_FILE_FORMAT_E.NETDEV_MEDIA_FILE_MP4);
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
            int iRet = NETDEVSDK.NETDEV_StopSaveRealData(realHandle);
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

        private bool Login()
        {
            try
            {
                if (loginControl.GetDeviceLogin(Camera, out DeviceLogin, Common.Enum.Drivers.UNVNetSDK_231, this))
                {
                    loginControl.AddChannel(Camera, this, Common.Enum.Drivers.UNVNetSDK_231);
                    Logger.Log(String.Format("Reusing a Uniview Login to Camara {0}", Camera.Name), LogPriority.Information);
                    return true;
                }

                NETDEV_DEVICE_LOGIN_INFO_S pstDevLoginInfo = new NETDEV_DEVICE_LOGIN_INFO_S();
                NETDEV_SELOG_INFO_S pstSELogInfo = new NETDEV_SELOG_INFO_S();
                pstSELogInfo.dwSELogCount = 2;
                pstSELogInfo.dwSELogTime = 50;
                pstDevLoginInfo.szIPAddr = Camera.Host;
                try
                {
                    pstDevLoginInfo.dwPort = Camera.HttpPort;
                }
                catch (Exception ex)
                {
                    notification.Show("ID_PORTERROR", null);
                    Logger.Log("ID_PORTERROR port: " + Camera.HttpPort + " Exception: " + ex.Message, LogPriority.Fatal);
                    return false;
                }

                pstDevLoginInfo.szUserName = Camera.User;
                pstDevLoginInfo.szPassword = Camera.Password;
                pstDevLoginInfo.dwLoginProto = (int)NETDEV_LOGIN_PROTO_E.NETDEV_LOGIN_PROTO_ONVIF;

                DeviceLogin = NETDEVSDK.NETDEV_Login_V30(ref pstDevLoginInfo, ref pstSELogInfo);

                if (DeviceLogin == IntPtr.Zero)
                {
                    Logger.Log("Device Login is Zero " + NETDEVSDK.NETDEV_GetLastError(), LogPriority.Fatal);
                    return false;
                }
                else
                {
                    int pdwChlCount = 256;
                    IntPtr pstVideoChlList = new IntPtr();
                    pstVideoChlList = Marshal.AllocHGlobal(256 * Marshal.SizeOf(typeof(NETDEV_VIDEO_CHL_DETAIL_INFO_S)));
                    var iRet2 = NETDEVSDK.NETDEV_QueryVideoChlDetailList(DeviceLogin, ref pdwChlCount, pstVideoChlList);

                    Logger.Log(String.Format("New Uniview Login Sucessed Camara {0}", Camera.Name), LogPriority.Information);
                    loginControl.AddDevice(Camera, (IntPtr)DeviceLogin, this, Common.Enum.Drivers.UNVNetSDK_260);
                    //set keep alive
                    return true;
                }

            }
            catch (Exception ex)
            {
                notification.Show(ex.Message, null);
                Logger.Log(String.Format("Login Exception: {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User, ex), LogPriority.Fatal);
                return false;
            }

        }

        private bool OpenCamera()
        {
            bool ret = RealStart(GetHandle());
            if (!ret)
            {
                Logger.Log(String.Format("Uniview OpenCamera Exception: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
            }
            return ret;
        }

        private IntPtr GetHandle()
        {
            IntPtr handle = IntPtr.Zero;
            if (this.picture.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    handle = GetHandle();
                });
                return handle;
            }

            try
            {

                handle = picture.Handle;
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("GetHandle Exception {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User, ex), LogPriority.Fatal);
            }
            return handle;
        }

        IntPtr ptrCallBack;
        private bool RealStart(IntPtr handle)
        {
            try
            {
                int iRet;

                int streamType = 0;
                switch (Profile)
                {
                    case Profile.SubStream:
                        streamType = (int)NETDEV_LIVE_STREAM_INDEX_E.NETDEV_LIVE_STREAM_INDEX_AUX;
                        break;
                    default:
                        streamType = (int)NETDEV_LIVE_STREAM_INDEX_E.NETDEV_LIVE_STREAM_INDEX_MAIN;
                        break;
                }



                NETDEV_PREVIEWINFO_S stPreviewInfo = new NETDEV_PREVIEWINFO_S();
                stPreviewInfo.dwChannelID = Camera.Channel;
                stPreviewInfo.dwLinkMode = (int)NETDEV_PROTOCAL_E.NETDEV_TRANSPROTOCAL_RTPTCP;
                stPreviewInfo.dwStreamType = streamType;
                stPreviewInfo.hPlayWnd = picture.Handle;

                if (ptrCallBack == IntPtr.Zero)
                {
                    cb = RealDataCB;
                    ptrCallBack = Marshal.GetFunctionPointerForDelegate(cb);
                }

                realHandle = NETDEVSDK.NETDEV_RealPlay(DeviceLogin, ref stPreviewInfo, ptrCallBack, IntPtr.Zero);
                if (realHandle != IntPtr.Zero)
                {
                    iRet = NETDEVSDK.NETDEV_SetIVAEnable(realHandle, 1);
                    iRet = NETDEVSDK.NETDEV_SetIVAShowParam(7);
                    iRet = NETDEVSDK.NETDEV_SetConnectTime(10, 2);
                    timerCheckConecction = new DispatcherTimer();
                    timerCheckConecction.Tick += TimerCheckConecction_Tick;
                    timerCheckConecction.Interval = TimeSpan.FromMilliseconds(5000);
                    timerCheckConecction.Start();
                    if (NETDEVSDK.FALSE == iRet)
                    {
                        Logger.Log(string.Format("NETDEV_SetConnectTime failed {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Information);
                    }
                    stRevTimeout = new NETDEV_REV_TIMEOUT_S();
                    stRevTimeout.dwRevTimeOut = 30;
                    stRevTimeout.dwFileReportTimeOut = 90;
                    iRet = NETDEVSDK.NETDEV_SetRevTimeOut(ref stRevTimeout);
                    if (NETDEVSDK.TRUE != iRet)
                    {
                        Logger.Log(string.Format("NETDEV_SetRevTimeOut failed {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Information);
                    }
                    Task.Delay(2000).ContinueWith(t => CheckCallback());
                    return true;
                }
                else
                {
                    Logger.Log(string.Format(" NETDEV_RealPlay failed Last Error {0}", NETDEVSDK.NETDEV_GetLastError()), LogPriority.Information);
                    // iRet = NETDEVSDK.NETDEV_Logout(realHandle);
                    //iRet = NETDEVSDK.NETDEV_StopRealPlay(realHandle);
                    return false;
                }

            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("RealStart Exception {0} {1} {2} {3} {4} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User, ex), LogPriority.Fatal);
                return false;
            }
        }

        private void TimerCheckConecction_Tick(object sender, EventArgs e)
        {
            TimeSpan dp = DateTime.Now.Subtract(dtFechaUltimoBuffer);
            if (dp.TotalSeconds > 3)
            {
                if (!panelconnection.Visible)
                {
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                }

                Logger.Log(String.Format("Uniview automatic reconnection live view:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
            }
            else
            {
                if (panelconnection.Visible)
                {
                    SetVisivility(PlaybackConnectionState.Connected);
                }
            }
        }

        private void SetVisivility(PlaybackConnectionState connectionState)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (this.panelconnection.InvokeRequired)
            {
                panelconnection.Invoke((MethodInvoker)delegate
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
                        panelconnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelconnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_es);
                        }
                        else
                        {
                            this.panelconnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_en);
                        }
                        break;
                    case PlaybackConnectionState.Connected:
                        panelconnection.Visible = false;
                        break;
                    case PlaybackConnectionState.NoRecording:
                        panelconnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelconnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_es);
                        }
                        else
                        {
                            this.panelconnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_en);
                        }
                        break;
                    case PlaybackConnectionState.Reconnecting:
                        panelconnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelconnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_es);
                        }
                        else
                        {
                            this.panelconnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_en);
                        }
                        break;
                    case PlaybackConnectionState.Connecting:
                        panelconnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelconnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_es);
                        }
                        else
                        {
                            this.panelconnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_en);
                        }
                        break;
                }
                Reconnecting.DisplayLogo(picture.Width, picture.Height, ref panelconnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"setVisivility Exception: {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        public new void Dispose()
        {
            if (!_isFullScreen && Controls.Count > 0)
            {
                Logger.Log(String.Format(" Dispose uniview {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                if (loginControl.RemoveChannelAndCanLogout(Camera, this, Common.Enum.Drivers.UNVNetSDK_231))
                {
                    Logger.Log(String.Format(" Dispose Logout uniview {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                    var iResult = NETDEVSDK.NETDEV_Logout(DeviceLogin);
                    if (iResult == NETDEVSDK.TRUE)
                    {
                        Logger.Log(String.Format("Successfully logged out uniview {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                    }
                    else
                    {
                        Logger.Log(String.Format("Error logged out uniview {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                        return;
                    }

                }
                NETDEVSDK.NETDEV_CloseSound(realHandle);
                NETDEVSDK.NETDEV_StopSaveRealData(realHandle);
                NETDEVSDK.NETDEV_StopRealPlay(realHandle);
                this.Controls.Clear();
                this.MouseWheel -= Picture_MouseWheel;
                UnsubcribePTZEvent();
                realHandle = IntPtr.Zero;
            }
        }

        public void GetUTF8Buffer(string inputString, int bufferLen, out byte[] utf8Buffer)
        {
            utf8Buffer = new byte[bufferLen];
            byte[] tempBuffer = System.Text.Encoding.UTF8.GetBytes(inputString);
            for (int i = 0; i < tempBuffer.Length; ++i)
            {
                utf8Buffer[i] = tempBuffer[i];
            }
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


        private void TryToReConnect()
        {
            try
            {


                SetVisivility(PlaybackConnectionState.Reconnecting);
                tryCount++;

                NETDEVSDK.NETDEV_StopSaveRealData(realHandle);
                NETDEVSDK.NETDEV_StopRealPlay(realHandle);
                Logger.Log(String.Format(" Dispose Logout uniview {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                if (RealStart(GetHandle()) == false)
                {
                    timerCheckConecction.Stop();
                    int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                    Logger.Log(String.Format(" TryToReConnect RealStar failed  {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                    Task.Delay(r).ContinueWith(t => TryToReConnect());
                }
                else
                {
                    timerCheckConecction.Start();
                    Logger.Log(String.Format(" TryToReConnect RealStar Connected  {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Connected);
                    IsPlaying = true;
                    tryCount = 0;

                    if (tryCount >= tryLimit)
                    {

                        {
                            timerCheckConecction.Stop();
                            SetVisivility(PlaybackConnectionState.Disconnected);
                            Logger.Log(String.Format("Uniview TryToReConnect reached max retry number, then it is  disconnected:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format(" TryToReConnect Exception:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
            }
        }

        private void NETDEV_ExceptionCallBack_PF(IntPtr lpUserID, Int32 dwType, IntPtr lpExpHandle, IntPtr lpUserData)
        {
            switch (dwType)
            {
                case 32768:
                    timerCheckConecction.Stop();
                    Logger.Log(String.Format("Uniview UniviewExceptionCallBack  Event received :  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                    tryCount = 0;
                    int r = ((int)(((_random.NextDouble() * TimeReConnectionCheck) + 1) * 1000));
                    Task.Delay(r).ContinueWith(t => TryToReConnect());
                    break;
                default:
                    break;

            }
        }

        private void RealDataCB(IntPtr lpPlayHandle, byte[] pucBuffer, Int32 dwBufSize, Int32 dwMediaDataType, IntPtr lpUserParam)
        {
            dtFechaUltimoBuffer = DateTime.Now;
            IsCallback = true;
        }

        private void CheckCallback()
        {
            if (IsCallback)
            {
                Logger.Log(String.Format("checkCallback is Receiving from camera {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                SetVisivility(PlaybackConnectionState.Connected);
                retryCallbackCount = 0;
            }
            else
            {
                retryCallbackCount++;
                Logger.Log(String.Format(" checkCallback Connected without video   {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                if (retryCallbackCount < retryCallbackLimit)
                {
                    Logger.Log(String.Format(" checkCallback retry to connect without video   {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                    IsPlaying = OpenCamera();
                    if (!IsPlaying)
                    {
                        tryCount = 0;
                        TryToReConnect();
                    }
                    Task.Delay(2000).ContinueWith(t => CheckCallback());
                }
                else
                {
                    Logger.Log(String.Format(" checkCallback failed without video  set state as disconnected{0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User), LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Disconnected);
                }
            }
        }

        public bool ToggleTalk(bool talkStatus)
        {
            return false;
        }

        public IOPortState InputPortState()
        {
            return this.manufactureUri.InputPortState();
        }

        public IOPortState OuputPortState()
        {
            return this.manufactureUri.OuputPortState();
        }

        public void OuputPortChangeState(IOPortState state)
        {
            //this.manufactureUri.OuputPortChangeState(state);
        }

        public bool ToogleSequencing(bool value)
        {
            SequencingStatus = value;
            if (this.SequencingUserControl != null)
            {
                this.SequencingUserControl.Visible = SequencingStatus;
            }

            return SequencingStatus;
        }

        private void GetConfiguredFps() 
        {
            NETDEV_VIDEO_STREAM_INFO_S videoConfig = new NETDEV_VIDEO_STREAM_INFO_S();
            int structSize = Marshal.SizeOf(videoConfig);
            IntPtr pOutBuffer = Marshal.AllocHGlobal(structSize);
            try
            {
            }
            finally
            {
                Marshal.FreeHGlobal(pOutBuffer);
            }
        }
    }
}
