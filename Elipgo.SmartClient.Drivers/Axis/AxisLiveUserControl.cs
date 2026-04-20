using AXISMEDIACONTROLLib;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Common.Reflections;
using Elipgo.SmartClient.Common.Reflections.Manufactures;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers.Axis
{
    public partial class AxisLiveUserControl : UserControl, IDriverLive, IDisposable
    {
        private AxAXISMEDIACONTROLLib.AxAxisMediaControl amc;

        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnVideoEventHandler OnVideo;
        public event ButtonPressedEventHandler PressedButtons;
        public event OnDriverDispose OnDispose;
        public event OnSequecingClick OnSequencing;
        public event OnInitializeAudioEventHandler OnInitializeAudio;
        public event OnAddExtraProfilesEventHandler OnAddExtraProfiles;

        private const int SPEED_VALUE = 4;
        private const int TIME_SLEEP_TIMEOUT = 800; // milliseconds

        private bool _videoState = false;
        private Profile _tempProfile = Profile.SubStream;
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        private bool _offLine = true;
        private bool _imageReceived = false;
        private int _maxTryReconnection;
        private int _currentTryReconnection;
        private readonly PtzUserControl _ptzUserControl;
        private readonly SequencingUserControl _sequencingUserControl;
        private readonly ManufactureUriAbstract _manufactureUri;
        private string _asfPath = "";

        private StreamSwitcher _streamSwitcher;
        private int _configuredFps = 0;
        private int _framesCount = 0;

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_MOUSEWHEEL = 0x020A;

        public AxisLiveUserControl(CameraDTO camera, Profile profile, bool initAudio)
        {
            InitializeComponent();
            CreateAndConfigurateAxisMediaControl();

            Camera = camera;
            Profile = profile;
            IsInitAudio = initAudio;

            ListenStatus = false;
            ClipStatus = false;
            TalkStatus = false;
            PtzStatus = false;
            SequencingStatus = false;
            DigitalZoomStatus = false;
            InstantPlaybackStatus = false;

            this.Click += AxisLiveUserControl_Click;
            this.DoubleClick += AxisLiveUserControl_DoubleClick;
            this.Resize += AxisLiveUserControl_Resize;
            _panelFondoLogo.Click += AxisLiveUserControl_Click;
            _panelFondoLogo.DoubleClick += AxisLiveUserControl_DoubleClick;

            _panelNoConnection.Visible = _offLine;
            _manufactureUri = new AxisUri(Camera, Profile.None);

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
                _buttonZoomIn.Image = Common.Properties.FileResources.ptz_zoom_in;
                _buttonZoomOut.Image = Common.Properties.FileResources.ptz_zoom_out;
                _buttonZoomIn.Location = new Point(this.Width - 34, this.Height - 64);
                _buttonZoomOut.Location = new Point(this.Width - 34, this.Height - 34);
                _buttonZoomIn.Visible = false;
                _buttonZoomOut.Visible = false;
            }

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
            _buttonZoomIn.SendToBack();
            _buttonZoomOut.SendToBack();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (DigitalZoomStatus)
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
            else
            {
                //Ctl++
                if (keyData == (Keys.Control | Keys.Oemplus))
                {
                    PTZControl(PtzMovement.ZoomIn, 0, (int)Camera.ZoomSensitivity * 50, true);
                }
                //Ctl+-
                if (keyData == (Keys.Control | Keys.OemMinus))
                {
                    PTZControl(PtzMovement.ZoomOut, 0, (int)Camera.ZoomSensitivity * 50, true);
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

        private void SequencingUserControl_ButtonClick(object sender, int e)
        {
            OnSequencing?.Invoke(e);
        }

        private void PtzUserControl_ButtonMouseUp(object sender, PtzMovement e)
        {
            PTZControl(e, 0, 0, true);
        }

        private void PtzUserControl_ButtonMouseDown(object sender, PtzMovement e)
        {
            PTZControl(e, 0, 0, false);
        }

        private void ButtonZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            PTZControl(PtzMovement.ZoomIn, 0, (int)Camera.ZoomSensitivity * 50, false);
        }

        private void ButtonZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            PTZControl(PtzMovement.ZoomIn, 0, (int)Camera.ZoomSensitivity * 50, true);
        }

        private void ButtonZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            PTZControl(PtzMovement.ZoomOut, 0, (int)Camera.ZoomSensitivity * -50, false);
        }

        private void ButtonZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
            PTZControl(PtzMovement.ZoomOut, 0, (int)Camera.ZoomSensitivity * 50, true);
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
                        _panelNoConnection.BringToFront();
                        _panelNoConnection.Visible = _offLine;
                        _panelNoConnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.disconnected_es) : new Bitmap(Properties.Resources.disconnected_en);
                        break;
                    case PlaybackConnectionState.NoRecording:
                        _panelNoConnection.BringToFront();
                        _panelNoConnection.Visible = _offLine;
                        _panelNoConnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.norecording_es) : new Bitmap(Properties.Resources.norecording_en);
                        break;
                    case PlaybackConnectionState.Reconnecting:
                        _panelNoConnection.Visible = _offLine;
                        _panelNoConnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.reconnecting_es) : new Bitmap(Properties.Resources.reconnecting_en);
                        break;
                    case PlaybackConnectionState.Connecting:
                        _panelNoConnection.BringToFront();
                        _panelNoConnection.Visible = _offLine;
                        _panelNoConnection.BackgroundImage = ci.Name.Contains("es") ? new Bitmap(Properties.Resources.connecting_es) : new Bitmap(Properties.Resources.connecting_en);
                        break;
                }
                Reconnecting.DisplayLogo(_panelFondoLogo.Width, _panelFondoLogo.Height, ref _panelNoConnection, ref _panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"SetVisivility Exception: {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} - {ex.Message}", LogPriority.Fatal);
            }
        }

        public new void Dispose()
        {
            if (this.amc == null)
            {
                return;
            }

            if (!this.amc.IsDisposed)
            {
                this.amc.Mute = true;
                if ((this.amc.Status & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0)
                {
                    this.amc.StopRecordMedia();
                }

                this.amc.Stop();
                this.amc.OnError -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.Amc_OnError);
                this.amc.OnMouseMove -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnMouseMoveEventHandler(this.Amc_OnMouseMove);
                this.amc.OnStatusChange -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnStatusChangeEventHandler(this.Amc_OnStatusChange);
                this.amc.OnNewVideoSize -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnNewVideoSizeEventHandler(this.Amc_OnNewVideoSize);
                this.amc.OnClick -= Amc_OnClick;
                this.amc.OnDoubleClick -= Amc_OnDoubleClick;
                this.amc.OnKeyUp -= Amc_OnKeyUp; ;
                this.amc.OnNewImage -= Amc_OnNewImage;
                this.amc.Dispose();
                this.amc = null;
                UnsubcribePTZEvent();

                this.Click -= AxisLiveUserControl_Click;
                this.DoubleClick -= AxisLiveUserControl_DoubleClick;

            }
        }

        public void DisposeDragged()
        {
            if (!this.amc.IsDisposed)
            {
                this.amc.Mute = true;
                if ((this.amc.Status & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0)
                {
                    this.amc.StopRecordMedia();
                }
                this.amc.Stop();
                this.amc.Dispose();
            }
        }

        private void AxisLiveUserControl_Load(object sender, EventArgs e)
        {
        }

        private void AxisLiveUserControl_DoubleClick(object sender, EventArgs e)
        {
            CameraSelectedDoubleClick(this);
        }

        private void AxisLiveUserControl_Click(object sender, EventArgs e)
        {
            CameraSelected(this, Camera);
        }

        private void AxisLiveUserControl_Resize(object sender, EventArgs e)
        {
            Reconnecting.DisplayLogo(this.Width, this.Height, ref _panelNoConnection, ref _panelFondoLogo);
        }

        private void CreateAndConfigurateAxisMediaControl()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AxisLiveUserControl));
            this.amc = new AxAXISMEDIACONTROLLib.AxAxisMediaControl();
            ((System.ComponentModel.ISupportInitialize)this.amc).BeginInit();
            this.SuspendLayout();

            this.amc.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.amc.Enabled = true;
            this.amc.Location = new System.Drawing.Point(0, 0);
            this.amc.Name = "amc";
            this.amc.OcxState = (AxHost.State)resources.GetObject("amc.OcxState");
            this.amc.Dock = DockStyle.Fill;
            this.amc.TabIndex = 0;
            this.amc.TabStop = false;
            this.amc.OnError += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.Amc_OnError);
            this.amc.OnMouseMove += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnMouseMoveEventHandler(this.Amc_OnMouseMove);
            this.amc.OnStatusChange += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnStatusChangeEventHandler(this.Amc_OnStatusChange);
            this.amc.OnNewVideoSize += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnNewVideoSizeEventHandler(this.Amc_OnNewVideoSize);
            this.Controls.Add(this.amc);
            ((System.ComponentModel.ISupportInitialize)this.amc).EndInit();
            this.ResumeLayout(false);

            this.amc.OnClick += Amc_OnClick;
            this.amc.OnDoubleClick += Amc_OnDoubleClick;
            this.amc.OnKeyUp += Amc_OnKeyUp; ;
            this.amc.OnNewImage += Amc_OnNewImage;

            this.amc.StretchToFit = true;
            this.amc.MaintainAspectRatio = false;
            this.amc.ShowStatusBar = false;
            this.amc.BackgroundColor = 0; // black
            this.amc.VideoRenderer = (int)AMC_VIDEO_RENDERER.AMC_VIDEO_RENDERER_EVR;
            this.amc.EnableOverlays = true;
            this.amc.EnableContextMenu = false;
            this.amc.ToolbarConfiguration = "-play,-fullscreen,-settings"; //"-pixcount" to remove pixel counter
            this.amc.Popups = 0;
            //this.amc.UIMode = Camera.PtzEnabled ? "ptz-relative-no-cross" : "none";
            this.amc.NetworkTimeout = 4000;
            this.amc.EnableReconnect = true;
            this.amc.SetReconnectionStrategy(60000, 10000, 300000, 30000, 300000, 60000, true);

            _currentTryReconnection = 0;
            _maxTryReconnection = (60000 / 10000) + (300000 / 30000) + (300000 / 60000);
        }

        private void Amc_OnKeyUp(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnKeyUpEvent e)
        {
            if (e.keyCode == 27)
            {
                if (_tempProfile != Profile)
                {
                    ChangeProfile(_tempProfile);
                }
            }
        }

        private void Amc_OnNewImage(object sender, EventArgs e)
        {
            if (_offLine == true)
            {
                _imageReceived = true;
                _currentTryReconnection = 0;
                _offLine = false;
                OpenCameraAudio();
                GetConfiguredFps();
                Logger.Log($"---------> Conexion Axis Live, Camara: {Camera.Name}", LogPriority.Information);
                SetVisivility(PlaybackConnectionState.Reconnecting);
                this.amc.OnError += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.Amc_OnError);
            }

            if (_videoState)
            {
                return;
            }

            if (OnVideo != null)
            {
                OnVideo(true, this);
                _videoState = true;
            }

        }

        private void Amc_OnClick(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnClickEvent e)
        {
            CameraSelected?.Invoke(this, Camera);
        }

        private void Amc_OnDoubleClick(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnDoubleClickEvent e)
        {
            CameraSelectedDoubleClick?.Invoke(this);
        }

        private void Amc_OnError(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEvent e)
        {
            if (_imageReceived == false)
            {// es la primera vez cuando offline esta en true entonces voy a desconetar directamente
                Logger.Log($"Camera: {Camera.Name} - Error code: {e.theErrorCode:X8} - {e.theErrorInfo}", LogPriority.Information);
                _notification.Show($"Camera: {Camera.Name} - Error code {e.theErrorCode:X8}", null);
                SetVisivility(PlaybackConnectionState.Disconnected);
                Logger.Log($"---------> Se ha desconectado Axis Live, Camara: {Camera.Name}", LogPriority.Information);
                this.amc.OnError -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.Amc_OnError);
            }
            else
            {
                _offLine = true;
                if (_currentTryReconnection <= _maxTryReconnection)
                {
                    _currentTryReconnection++;
                    Logger.Log($"Camera: {Camera.Name} - Error code: {e.theErrorCode:X8} - {e.theErrorInfo}", LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    Logger.Log($"---------> Intentando conectar Axis Live, Intento [{_currentTryReconnection}]. Camara: {Camera.Name}", LogPriority.Information);
                }
                else
                {
                    Logger.Log($"Camera: {Camera.Name} - Error code: {e.theErrorCode:X8} - Disconnected", LogPriority.Information);
                    _notification.Show($"Camera: {Camera.Name} - Error code: {e.theErrorCode:X8}", null);
                    SetVisivility(PlaybackConnectionState.Disconnected);
                    Logger.Log($"---------> Se ha desconectado Axis Live, Camara: {Camera.Name}", LogPriority.Information);
                    this.amc.OnError -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.Amc_OnError);
                }
            }
        }

        private void Amc_OnMouseMove(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnMouseMoveEvent e)
        {
            if (this.amc.UIMode == "digital-zoom")
            {
                if ((this.amc.Status & (int)AMC_STATUS.AMC_STATUS_PLAYING) > 0)
                {
                    // set focus to AMC in order to zoom using mouse wheel
                    this.amc.Focus();
                }
            }
        }

        void Amc_OnStatusChange(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnStatusChangeEvent e)
        {
            //int reconncting = ( e.theNewStatus & 32);
            if ((e.theOldStatus & (int)AMC_STATUS.AMC_STATUS_RECORDING) == 0 && // was not recording
                    (e.theNewStatus & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0) // is recording
            {
            }
            else
            {
            }
        }

        void Amc_OnNewVideoSize(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnNewVideoSizeEvent e)
        {
        }

        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }

        public List<ButtonsContextBar> Commands => GetButtons();
        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();

        public bool ListenStatus { get; set; }
        public bool ClipStatus { get; set; }
        public bool TalkStatus { get; set; }
        public bool PtzStatus { get; set; }
        public bool SequencingStatus { get; set; }
        public bool DigitalZoomStatus { get; set; }
        public bool InstantPlaybackStatus { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsSequencingEnabled => Camera.Sequencing != null;
        public bool IsInitAudio { get; set; }

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

        public bool ChangeProfile(Profile profile, bool autoSwitching = false)
        {
            Profile = profile;
            Stop();
            return Play();
        }

        public async Task<bool> Snapshot(string path, string token, int id)
        {
            try
            {
                this.amc.SaveCurrentImage(0, path);
                _notification.Show(Resources.SnapshotSaved, () => Process.Start(path));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Camera: {Camera.Name} - Error code: {ex.Data}", LogPriority.Warning);
                return false;
            }
        }

        public bool Play()
        {
            if (IsPlaying)
            {
                return true;
            }

            try
            {
                Logger.Log($"Play Axis entered {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User}", LogPriority.Information);
                IManufactureUri manufactureUri = ManufactureUriFactory.Instance(Camera, Profile);

                SetVisivility(PlaybackConnectionState.Connecting);

                //Stop possible streams
                this.amc.Stop();

                this.amc.Mute = true;
                this.amc.MediaUsername = Camera.User;
                this.amc.MediaPassword = Camera.Password;
                this.amc.MediaURL = manufactureUri.StreamLiveUri();

                if (!Camera.Recorders.Exists(x => x.ProxyEnabled))
                {
                    this.amc.AudioConfigURL = manufactureUri.AudioConfigUri();
                    this.amc.AudioTransmitURL = manufactureUri.AudioTrasmitUri();
                }

                if (!DigitalZoomStatus)
                {
                    this.amc.UIMode = Camera.PtzEnabled ? "ptz-relative-no-cross" : "none";
                }

                if (Camera.PtzEnabled)
                {
                    this.amc.PTZControlURL = manufactureUri.PtzControlUri();
                    this.amc.UIMode = "ptz-relative-no-cross";
                }

                // Start the streaming
                this.amc.Play();

                Logger.Log($"Play Axis Connected {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} ", LogPriority.Information);
                IsPlaying = true;
            }
            catch (Exception)
            {
                IsPlaying = false;
                SetVisivility(PlaybackConnectionState.Reconnecting);
                Logger.Log($"Error al realizar el Login Camera: {Camera.Name} {Camera.Host} {Camera.HttpPort} {Camera.User} ", LogPriority.Fatal);
            }
            return IsPlaying;
        }

        public bool Stop()
        {
            if (!IsPlaying)
            {
                return true;
            }

            try
            {
                this.amc.Stop();
                IsPlaying = false;
            }
            catch (Exception ex)
            {
                Logger.Log($"Camera: {Camera.Name} - Error code: {ex.Data}", LogPriority.Warning);
                IsPlaying = true;
            }
            return !IsPlaying;
        }

        public void ToggleFullScreen()
        {
            _tempProfile = Profile;
            if (Profile == Profile.SubStream)
            {
                Profile = Profile.MainStream;
                Stop();
                Play();
            }
            this.amc.FullScreen = true;
        }

        public bool ToggleListen(bool Listen)
        {
            try
            {
                this.amc.Mute = Listen; //!this.amc.Mute;
                ListenStatus = Listen; //!this.amc.Mute;
                return true;
            }
            catch (Exception)
            {
                ListenStatus = false;
                return false;
            }
        }

        public bool ToggleTalk()
        {
            try
            {
                if (TalkStatus)
                {
                    this.amc.AudioTransmitStop();
                }
                else
                {
                    this.amc.AudioTransmitStart();
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

        public bool VideoClipStart(string path)
        {
            try
            {
                path = path.Replace(".mp4", ".asf");
                _asfPath = path;
                if ((this.amc.Status & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0)
                {
                    this.amc.StopRecordMedia();
                }
                else
                {
                    // Start the recording (video and audio)
                    int recordingFlag = (int)AMC_RECORD_FLAG.AMC_RECORD_FLAG_AUDIO_VIDEO;
                    recordingFlag = (int)AMC_RECORD_FLAG.AMC_RECORD_FLAG_VIDEO;

                    this.amc.StartRecordMedia(path, recordingFlag, "");
                }
                ClipStatus = true;
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Camera: {Camera.Name} - Error code: {ex.Data}", LogPriority.Warning);
                ClipStatus = false;
                return false;
            }
        }

        public bool VideoClipStop()
        {
            try
            {
                this.amc.StopRecordMedia();
                if (!string.IsNullOrWhiteSpace(_asfPath))
                {
                    VideoConversorServices.Instance.ConvertFileMediaToMp4(_asfPath);
                }
                ClipStatus = false;
                _notification.Show(Common.Properties.Resources.VideoclipSaved, () => Process.Start(_asfPath));
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Camera: {Camera.Name} - Error code: {ex.Data}", LogPriority.Warning);
                ClipStatus = false;
                return false;
            }
        }

        public bool Volume(int value)
        {
            throw new NotImplementedException();
        }

        public bool ToogleDigitalZoom()
        {
            try
            {
                this.amc.UIMode = "digital-zoom";
                DigitalZoomStatus = this.amc.UIMode == "digital-zoom";
            }
            catch (Exception)
            {
                DigitalZoomStatus = false;
            }
            return DigitalZoomStatus;
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

        private bool SendRequestAndHandleError(string url, HttpMethod method)
        {
            try
            {
                SendRequest(url, method);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string SendRequest(string url, HttpMethod method, string data = "")
        {
            try
            {
                Uri uri = new Uri(url);
                // Crear el request
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);
                webRequest.Method = method.ToString();
                webRequest.Timeout = 60000;

                // Configuración de las credenciales
                var credentials = new NetworkCredential(Camera.User, Camera.Password);
                var credentialCache = new CredentialCache
                {
                    { uri, "Basic", credentials },
                    { uri, "Digest", credentials }
                };
                webRequest.Credentials = credentialCache;
                webRequest.PreAuthenticate = true;

                // Si no es un GET y se proporcionan datos, escribir el cuerpo
                if (method != HttpMethod.Get && !string.IsNullOrEmpty(data))
                {
                    byte[] requestData = Encoding.ASCII.GetBytes(data);
                    webRequest.ContentLength = requestData.Length;
                    using (var requestStream = webRequest.GetRequestStream())
                    {
                        requestStream.Write(requestData, 0, requestData.Length);
                    }
                }

                // Obtener respuesta
                using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
                using (Stream stream = webResponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Camera: {Camera.Name} - Error code: {ex.Message}", LogPriority.Warning);
                return string.Empty; // En caso de error, devolver cadena vacía
            }
        }

        private void PTZControlSingleShoot(PtzMovement movement, int param1, int param2)
        {
            Task.Run(() =>
            {
                try
                {
                    PTZControl(movement, param1, param2, false);
                }
                catch (Exception) { }
            });
        }

        private void PTZControl(PtzMovement movement, int param1, int param2, bool isStop)
        {
            var continuouspantiltmove = "0,0";

            switch (movement)
            {
                case PtzMovement.Up:
                    continuouspantiltmove = "0,1";
                    break;
                case PtzMovement.Down:
                    continuouspantiltmove = "0,-1";
                    break;
                case PtzMovement.Left:
                    continuouspantiltmove = "-1,0";
                    break;
                case PtzMovement.Right:
                    continuouspantiltmove = "1,0";
                    break;
                case PtzMovement.Center:
                    continuouspantiltmove = "";
                    break;
                case PtzMovement.UpLeft:
                    continuouspantiltmove = "-1,1";
                    break;
                case PtzMovement.DownLeft:
                    continuouspantiltmove = "-1,-1";
                    break;
                case PtzMovement.UpRight:
                    continuouspantiltmove = "1,1";
                    break;
                case PtzMovement.DownRight:
                    continuouspantiltmove = "1,-1";
                    break;
            }
            //var continuouszoommove = param2 < 0d ? "-1" : "1";
            var continuouszoommove = param2 < 0d ? "-100" : "100";

            if (isStop)
            {
                continuouspantiltmove = "0,0";
                continuouszoommove = "0";
            }

            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["camera"] = Camera.Channel.ToString();
            if (movement == PtzMovement.ZoomIn || movement == PtzMovement.ZoomOut)
            {
                //query["continuouszoommove"] = continuouszoommove;
                query["rzoom"] = continuouszoommove;
            }
            else
            {
                query["continuouspantiltmove"] = continuouspantiltmove;
            }
            var ptzUri = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-cgi/com/ptz.cgi",
                Query = query.ToString()
            }.ToString();
            SendRequestAndHandleError(ptzUri.ToString(), HttpMethod.Get);
        }

        public List<Profile> Profiles => new List<Profile> { Profile.MainStream, Profile.SubStream };

        public bool CallGuard(ActivateGuardDTO guard)
        {
            return SendRequestAndHandleError(_manufactureUri.CallGuardUri(guard), HttpMethod.Get);
        }

        public bool StopGuard(ActivateGuardDTO guard)
        {
            return SendRequestAndHandleError(_manufactureUri.StopGuardUri(guard), HttpMethod.Get);
        }

        public bool CallPreset(PresetDTO preset)
        {
            return SendRequestAndHandleError(_manufactureUri.CallPresetUri(preset), HttpMethod.Get);
        }

        public GuardDTO[] ListGuards()
        {
            var result = SendRequest(_manufactureUri.GuardTourUri(), HttpMethod.Get);

            if (string.IsNullOrEmpty(result))
            {
                return Array.Empty<GuardDTO>();
            }

            if (result.Contains("Error"))
            {
                Logger.Log($"Camera: {Camera.Name} Error code: {result}", LogPriority.Fatal);
                return Array.Empty<GuardDTO>();
            }

            var guards = new List<GuardDTO>();
            var guardChunks = SplitChunksByGuardName(result);
            foreach (var chunk in guardChunks)
            {
                var guard = ParseGuardChunk(chunk);
                guards.Add(guard);
            }
            return guards.ToArray();
        }

        private List<string> SplitChunksByGuardName(string result)
        {
            var guardChunks = new List<string>();
            var regex = new Regex(@"root\.GuardTour\.G\d+\.Name=.*?(?=\nroot\.GuardTour\.G\d+\.Name|$)", RegexOptions.Singleline);

            foreach (Match match in regex.Matches(result))
            {
                guardChunks.Add(match.Value);
            }

            return guardChunks;
        }

        private GuardDTO ParseGuardChunk(string chunk)
        {
            var guard = new GuardDTO();
            var guardTourList = new List<GuardTourDTO>();

            // Parsear nombre y estado del guardia
            guard.Id = int.Parse(ExtractId(chunk, @"root\.GuardTour\.G(\d+)\."));
            guard.Name = ExtractValue(chunk, @"root\.GuardTour\.G\d+\.Name=(\w+)");
            guard.isActivated = ExtractValue(chunk, @"root\.GuardTour\.G\d+\.Running=(yes|no)") == "yes";

            // Parsear los tours
            var tourChunks = SplitChunksByTour(chunk);
            foreach (var tourChunk in tourChunks)
            {
                var guardTour = ParseGuardTour(tourChunk);
                guardTourList.Add(guardTour);
            }

            guard.GuardTours = guardTourList.ToArray();
            return guard;
        }

        private string ExtractId(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private string ExtractValue(string input, string pattern)
        {
            var match = Regex.Match(input, pattern);
            return match.Success ? match.Groups[1].Value : string.Empty;
        }

        private List<string> SplitChunksByTour(string chunk)
        {
            var tourChunks = new List<string>();
            var regex = new Regex(@"root\.GuardTour\.G\d+\.Tour\.T\d+\.MoveSpeed=.*?(?=\nroot\.GuardTour\.G\d+\.Tour\.T\d+\.MoveSpeed|$)", RegexOptions.Singleline);

            foreach (Match match in regex.Matches(chunk))
            {
                tourChunks.Add(match.Value);
            }

            return tourChunks;
        }

        private GuardTourDTO ParseGuardTour(string tourChunk)
        {
            // int.Parse(ExtractId(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T(\d+)\."));
            var guardTour = new GuardTourDTO
            {
                Speed = int.Parse(ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.MoveSpeed=(\d+)")),
                PresetId = int.Parse(ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.PresetNbr=(\d+)")),
                Time = ParseWaitTime(ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.WaitTime=(\d+)"),
                                     ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.WaitTimeViewType=(\w+)"))
            };
            return guardTour;
        }

        private int ParseWaitTime(string time, string waitTimeViewType)
        {
            int parsedTime = int.TryParse(time, out int result) ? result : 0;
            if (!string.IsNullOrEmpty(waitTimeViewType))
            {
                switch (waitTimeViewType)
                {
                    case "Minutes":
                        parsedTime = parsedTime * 60;
                        break;
                    case "Hours":
                        parsedTime = parsedTime * 60 * 60;
                        break;
                }
            }
            return parsedTime;
        }

        public GuardForCreationDTO GetGuard(int guardId)
        {
            string url = _manufactureUri.GuardTourGetUri(guardId);
            var result = SendRequest(url, HttpMethod.Get);

            if (string.IsNullOrEmpty(result))
            {
                return null;
            }

            if (result.Contains("Error"))
            {
                Logger.Log($"Camera: {Camera.Name} Error code: {result}", LogPriority.Fatal);
                return null;
            }

            var response = new GuardForCreationDTO
            {
                Id = int.Parse(ExtractId(result, @"root\.GuardTour\.G(\d+)\.")),
                Name = ExtractValue(result, @"root\.GuardTour\.G\d+\.Name=(\w+)"),
                isActivated = ExtractValue(result, @"root\.GuardTour\.G\d+\.Running=(yes|no)") == "yes",
                TimeBetweenSequences = int.Parse(ExtractValue(result, @"root\.GuardTour\.G\d+\.TimeBetweenSequences=(\d+)")),
                GuardTours = SplitChunksByTour(result).Select(ParseGuardTourForCreationDTO).ToArray()
            };
            return response;
        }

        private GuardTourForCreationDTO ParseGuardTourForCreationDTO(string tourChunk)
        {
            var guardTour = new GuardTourForCreationDTO
            {
                PresetId = int.Parse(ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.PresetNbr=(\d+)")),
                Speed = int.Parse(ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.MoveSpeed=(\d+)")),
                Time = ParseWaitTime(ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.WaitTime=(\d+)"),
                                     ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.WaitTimeViewType=(\w+)")),
                WaitTimeViewType = Enum.TryParse<WaitTimeViewType>(ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.WaitTimeViewType=(\w+)"), out var waitTimeView) ? waitTimeView : WaitTimeViewType.Seconds,
                ViewOrder = int.Parse(ExtractValue(tourChunk, @"root\.GuardTour\.G\d+\.Tour\.T\d+\.ViewOrder=(\d+)"))
            };
            return guardTour;
        }

        public PresetDTO[] ListPresets()
        {
            var response = new List<PresetDTO>();
            try
            {
                string url = _manufactureUri.PresetListUri();
                string result = SendRequest(url, HttpMethod.Get);

                // Verificación de posibles errores en la respuesta
                if (result.Contains("Error"))
                {
                    Logger.Log($"Camera: {Camera.Name} Error code: {result}", LogPriority.Fatal);
                    return Array.Empty<PresetDTO>();
                }

                // Split the result by carriage return (CR)
                var items = result.Split(new[] { (char)13 }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var item in items)
                {
                    var currentItem = item.Replace("\n", "");
                    if (currentItem.Length > 10 && currentItem.StartsWith("presetposno"))
                    {
                        int equalIndex = currentItem.IndexOf('=');
                        if (equalIndex > -1)
                        {
                            // Parse the ID and Name of the preset
                            response.Add(new PresetDTO
                            {
                                Id = int.Parse(currentItem.Substring(11, equalIndex - 11)),
                                Name = currentItem.Substring(equalIndex + 1)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Camera: {Camera.Name} - Error: {ex.Message}", LogPriority.Warning);
            }
            return response.ToArray();
        }

        public bool RemoveGuard(GuardDTO guard)
        {
            return guard.Id <= -1 || SendRequestAndHandleError(_manufactureUri.RemoveGuardTourUri(guard), HttpMethod.Get);
        }

        public bool RemovePreset(PresetDTO preset)
        {
            return SendRequestAndHandleError(_manufactureUri.DeletePresetUri(preset), HttpMethod.Get);
        }

        public bool SaveGuard(GuardForCreationDTO guard)
        {
            if (guard == null)
            {
                return false;
            }

            if (guard.Id > -1)
            {
                RemoveGuard(new GuardDTO() { Id = guard.Id });
            }

            var queryAddGuardTour = System.Web.HttpUtility.ParseQueryString(string.Empty);
            queryAddGuardTour["action"] = "add";
            queryAddGuardTour["group"] = "GuardTour";
            queryAddGuardTour["template"] = "guardtour";
            queryAddGuardTour["GuardTour.G.Name"] = guard.Name;
            queryAddGuardTour["GuardTour.G.TimeBetweenSequences"] = guard.TimeBetweenSequences.ToString();
            queryAddGuardTour["GuardTour.G.Running"] = "no";
            queryAddGuardTour["GuardTour.G.RandomEnabled"] = "no";

            var createGuardUri = new UriBuilder
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "axis-cgi/param.cgi",
                Query = queryAddGuardTour.ToString()
            }.Uri;
            var responseAddGuardTour = SendRequest(createGuardUri.ToString(), HttpMethod.Get);

            Regex regex = new Regex(@"G(\d+) OK");
            Match match = regex.Match(responseAddGuardTour);
            if (!match.Success)
            {
                return false;
            }

            guard.Id = int.Parse(match.Groups[1].ToString());
            foreach (var guardItem in guard.GuardTours)
            {
                var queryAddTour = System.Web.HttpUtility.ParseQueryString(string.Empty);
                queryAddTour["action"] = "add";
                queryAddTour["group"] = $"GuardTour.G{guard.Id}.Tour";
                queryAddTour["template"] = "tour";
                queryAddTour[$"GuardTour.G{guard.Id}.Tour.T.PresetNbr"] = guardItem.PresetId.ToString();
                queryAddTour[$"GuardTour.G{guard.Id}.Tour.T.Position"] = guardItem.ViewOrder.ToString();
                queryAddTour[$"GuardTour.G{guard.Id}.Tour.T.MoveSpeed"] = guardItem.Speed.ToString();
                queryAddTour[$"GuardTour.G{guard.Id}.Tour.T.WaitTime"] = guardItem.Time.ToString();
                queryAddTour[$"GuardTour.G{guard.Id}.Tour.T.WaitTimeViewType"] = guardItem.WaitTimeViewType.ToString();

                var createTourUri = new UriBuilder
                {
                    Scheme = Camera.Protocol,
                    Host = Camera.Host,
                    Port = Camera.HttpPort,
                    Path = "axis-cgi/param.cgi",
                    Query = queryAddTour.ToString()
                }.Uri;

                SendRequest(createTourUri.ToString(), HttpMethod.Get);
            }

            return true;
        }

        public bool SavePreset(PresetDTO preset)
        {
            try
            {
                if (preset.OldName != null)
                {
                    var oldPreset = new PresetDTO() { Id = preset.Id, Name = preset.OldName, Enabled = preset.Enabled };
                    //axis could not have api to update preset then we recall to old prest,remove the old preset and finally create a new one at current position
                    SendRequest(_manufactureUri.CallPresetUri(oldPreset), HttpMethod.Get);
                    SendRequest(_manufactureUri.DeletePresetUri(oldPreset), HttpMethod.Get);
                }

                string result = SendRequest(_manufactureUri.SavePresetUri(preset), HttpMethod.Get);
                if (result.Contains("Error"))
                {
                    _notification.Show($"Camera: {Camera.Name} - Error code: {result}", null);
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log($"Camera: {Camera.Name} - Error code: {ex.Data}", LogPriority.Warning);
                return false;
            }
        }

        public bool StateGuard(GuardDTO guard)
        {
            return false;
        }

        private bool _unsubcribedPtzEvent = false;

        public void UnsubcribePTZEvent()
        {
            if (_ptzUserControl != null)
            {
                _ptzUserControl.PtzJoystickStateEvent -= PtzJoystickStateEvent;
                _ptzUserControl.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
            }
            //cuando se utiliza el joytick y la camara ptz se comienza a mover, si se selecciona otra camara se ejecuta este metodo
            //realizando la desubcripcion a los eventos del joystick, pero ocurre un problema si el usuario mantiene el joystick en un estado de desplazamiento 
            //y se selecciona una nueva camara, esta camara nunca recibe el evento que pare y continua loca en forma indefinida hasta se vuelva a selecciona y ejecutar un 
            //nuevo comando con su stop correspondiente es por esto que 
            //envio un comando cualquiera para detener el movimiento de la camara cuando se descelecciona la camara forzando a parar este o no en ejecuccion
            PTZControl(PtzMovement.Up, 0, 0, true);
            _unsubcribedPtzEvent = true;
        }

        public void SubcribePTZEvent()
        {
            _unsubcribedPtzEvent = false;
            _ptzUserControl.PtzJoystickStateEvent += PtzJoystickStateEvent;
            _ptzUserControl.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
        }

        private List<ActionCommand> ExecuteZoomCommand(List<ActionCommand> pressedButtons)
        {
            foreach (ActionCommand act in pressedButtons.Where(x => (x.command == PtzCommand.ZOOM_ADD_CONTROL || x.command == PtzCommand.ZOOM_DEC_CONTROL) && x.isInvoked).ToList())
            {
                PTZControlSingleShoot(ParseToAxisCommand((ButtonOrAxis)System.Enum.Parse(typeof(ButtonOrAxis), act.command.ToString())), 0, (int)(act.Parameter * (int)Camera.ZoomSensitivity * 50));
            }
            return pressedButtons.Where(x => !(x.command == PtzCommand.ZOOM_ADD_CONTROL || x.command == PtzCommand.ZOOM_DEC_CONTROL)).ToList();
        }

        private List<ActionCommand> ExecuteCallPresetCommand(List<ActionCommand> pressedButtons)
        {
            List<int> presetsIds = pressedButtons.Where(x => (x.command == PtzCommand.CallPreset) && x.isInvoked == true).Select(x => (int)x.Parameter).ToList();
            if (presetsIds.Count > 0)
            {
                List<PresetDTO> presets = ListPresets().OrderBy(x => x.Id).ToList();

                foreach (int id in presetsIds)
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
                        _notification.Show(string.Format(Common.Properties.Resources.NoPresetAvailable, Camera.Name), null);
                    }
                }
            }
            return pressedButtons.Where(x => !(x.command == PtzCommand.CallPreset)).ToList();
        }

        private void PtzJoystickButtonEvent(List<ActionCommand> pressedButtons)
        {
            if (_unsubcribedPtzEvent)
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
            if (_unsubcribedPtzEvent)
            {
                return;
            }

            foreach (ActionCommand act in actionCommands.Where(x => !(x.buttonOrAxis == ButtonOrAxis.ZOOM_ADD_CONTROL || x.buttonOrAxis == ButtonOrAxis.ZOOM_DEC_CONTROL)).ToList())
            {
                int movementSpeed1 = (int)(act.Parameter * (Camera.MovementSensitivity != 0 ? Camera.MovementSensitivity : SPEED_VALUE));
                int movementSpeed2 = (int)(act.Parameter2 * (Camera.MovementSensitivity != 0 ? Camera.MovementSensitivity : SPEED_VALUE));
                PTZControl(ParseToAxisCommand(act.buttonOrAxis), movementSpeed1, movementSpeed2, !act.isInvoked);
            }

            foreach (ActionCommand act in actionCommands.Where(x => x.buttonOrAxis == ButtonOrAxis.ZOOM_ADD_CONTROL || x.buttonOrAxis == ButtonOrAxis.ZOOM_DEC_CONTROL).ToList())
            {
                int zoomSpeed1 = (int)(act.Parameter * (int)Camera.ZoomSensitivity * 50);
                int zoomSpeed2 = (int)(act.Parameter2 * (int)Camera.ZoomSensitivity * 50);
                PTZControl(ParseToAxisCommand(act.buttonOrAxis), zoomSpeed1, zoomSpeed2, !act.isInvoked);
            }
        }

        private PtzMovement ParseToAxisCommand(ButtonOrAxis command)
        {
            switch (command)
            {
                case ButtonOrAxis.UP_CONTROL: return PtzMovement.Up;
                case ButtonOrAxis.DOWN_CONTROL: return PtzMovement.Down;
                case ButtonOrAxis.LEFT_CONTROL: return PtzMovement.Left;
                case ButtonOrAxis.RIGHT_CONTROL: return PtzMovement.Right;
                case ButtonOrAxis.LEFTTOP: return PtzMovement.UpLeft;
                case ButtonOrAxis.LEFTDOWN: return PtzMovement.DownLeft;
                case ButtonOrAxis.RIGHTTOP: return PtzMovement.UpRight;
                case ButtonOrAxis.RIGHTDOWN: return PtzMovement.DownRight;
                case ButtonOrAxis.ZOOM_ADD_CONTROL: return PtzMovement.ZoomIn;
                case ButtonOrAxis.ZOOM_DEC_CONTROL: return PtzMovement.ZoomOut;
                default: return PtzMovement.Stop;
            }
        }

        public bool ToggleInstantPlayback()
        {
            InstantPlaybackStatus = !InstantPlaybackStatus;
            return InstantPlaybackStatus;
        }

        public bool ToggleTalk(bool talkStatus)
        {
            try
            {
                if (talkStatus)
                {
                    if (!TalkStatus)
                    {
                        this.amc.AudioTransmitStart();
                    }
                }
                else
                {
                    if (TalkStatus)
                    {
                        this.amc.AudioTransmitStop();
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
            return _manufactureUri.InputPortState();
        }

        public IOPortState OuputPortState()
        {
            return _manufactureUri.OuputPortState();
        }

        public void OuputPortChangeState(IOPortState state)
        {
            _manufactureUri.OuputPortChangeState(state);
        }

        public bool ToogleSequencing(bool value)
        {
            SequencingStatus = value;
            if (_sequencingUserControl != null)
            {
                _sequencingUserControl.Visible = SequencingStatus;
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

        private void GetConfiguredFps()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "list";
            query["group"] = "root.Image.I0.Stream";

            var url = new UriBuilder()
            {
                Scheme = Camera.Protocol,
                Host = Camera.Host,
                Port = Camera.HttpPort,
                Path = "/axis-cgi/param.cgi",
                Query = query.ToString()
            }.ToString();
            var response = _manufactureUri.SendRequest(url, HttpMethod.Get);
            using (System.IO.StringReader reader = new System.IO.StringReader(response))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains($"root.Image.I0.Stream.FPS"))
                    {
                        var match = Regex.Match(line, @"root.Image.I0.Stream.FPS=(\d+)");
                        if (match.Success && int.TryParse(match.Groups[1].Value, out int fps))
                        {
                            _configuredFps = fps;
                        }
                    }
                }
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
    }
}