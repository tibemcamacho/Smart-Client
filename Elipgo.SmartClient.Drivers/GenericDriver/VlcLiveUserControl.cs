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
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace Elipgo.SmartClient.Drivers.GenericDriver
{
    public partial class VlcLiveUserControl : UserControl, IDriverLive, IDisposable
    {
        private bool offLine = true;
        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private readonly int retryLimit;
        private PtzUserControl ptzUserControl;
        private SequencingUserControl SequencingUserControl;
        private bool Unsubcribed = false;
        private bool _isReconnecting = false;
        private int _currentRetryCount = 0;
        private readonly int[] _staggeredDelaysSeconds = { 0, 60, 60, 60, 300, 600, 1800, 3600 };
        private CancellationTokenSource _reconnectCts;

        //private StreamSwitcher _streamSwitcher;
        //private int _configuredFps = 0;
        //private int _framesCount = 0;
        //private bool _autoSwitching = false;

        public VlcLiveUserControl(CameraDTO camera, Profile profile, bool initAudio)
        {
            InitializeComponent();

            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

            retryLimit = int.Parse(config.AppSettings.Settings["retryLimit"].Value);

            Camera = camera;
            Profile = profile;
            IsInitAudio = initAudio;

            this.Resize += VlcLiveUserControl_Resize;

            ListenStatus = false;
            ClipStatus = false;
            TalkStatus = false;
            PtzStatus = false;
            SequencingStatus = false;
            DigitalZoomStatus = false;
            InstantPlaybackStatus = false;
            panelNoConnection.Visible = this.offLine;
            if (Camera.PtzEnabled)
            {
                //ptzUserControl = new PtzUserControl
                //{
                //    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                //    Location = new Point(263, 9),
                //    Margin = new Padding(0),
                //    Name = "ptzUserControl",
                //    Size = new Size(72, 72),
                //    Visible = false,
                //    TabIndex = 1
                //};
                //this.Controls.Add(ptzUserControl);
                //ptzUserControl.Location = new Point(this.Width - 82, 34);
                //ptzUserControl.ButtonMouseDown += PtzUserControl_ButtonMouseDown;
                //ptzUserControl.ButtonMouseUp += PtzUserControl_ButtonMouseUp;
                //ptzUserControl.BringToFront();
            }

            ButtonZoomIn.Image = Common.Properties.FileResources.ptz_zoom_in;
            ButtonZoomOut.Image = Common.Properties.FileResources.ptz_zoom_out;
            ButtonZoomIn.Location = new Point(this.Width - 34, this.Height - 64);
            ButtonZoomOut.Location = new Point(this.Width - 34, this.Height - 34);

            if (Camera.Sequencing != null)
            {
                SequencingUserControl = new SequencingUserControl(Camera.Sequencing)
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(263, 9),
                    Margin = new Padding(0),
                    Name = "SequencingUserControl",
                    Size = new Size(72, 72),
                    Visible = SequencingStatus,
                    TabIndex = 1
                };
                this.Controls.Add(SequencingUserControl);
                SequencingUserControl.Location = new Point(this.Width - 82, 34);
                SequencingUserControl.ButtonMouseUp += SequencingUserControl_ButtonClick;
                SequencingUserControl.BringToFront();
            }

            ButtonZoomIn.Visible = false;
            ButtonZoomOut.Visible = false;

            this.Click += VlcLiveUserControl_Click;
            this.DoubleClick += VlcLiveUserControl_DoubleClick;
            this.MouseWheel += VlcLiveUserControl_MouseWheel;
            this.vlcControl.Click += VlcLiveUserControl_Click;
            this.vlcControl.DoubleClick += VlcLiveUserControl_DoubleClick;
            this.vlcControl.EndReached += VlcControl_EndReached;
            this.vlcControl.Paused += VlcControl_Paused;
            this.vlcControl.Playing += VlcControl_Playing;
            this.vlcControl.Stopped += VlcControl_Stopped;
            this.vlcControl.EncounteredError += VlcControl_EncounteredError;
            this.vlcControl.Log += VlcControl_Log;
            this.vlcControl.Video.IsMouseInputEnabled = false;
            this.vlcControl.Video.IsKeyInputEnabled = false;
            this.vlcControl.MouseClick += new System.Windows.Forms.MouseEventHandler(VlcLiveUserControl_Click);
            this.vlcControl.Video.AspectRatio = $"{this.vlcControl.Width}:{this.vlcControl.Height}";
        }

        private int _actualSize = 0;
        private int _zoomLimit = int.Parse(Settings.Default["ZoomLimit"].ToString());
        private void VlcLiveUserControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!DigitalZoomStatus)
            {
                return;
            }

            Rectangle main = Screen.PrimaryScreen.Bounds;

            var direction = e.Delta > 0;
            Size picSize;
            if (direction)
            {
                picSize = new Size((int)(this.vlcControl.Width * 1.1), (int)(this.vlcControl.Height * 1.1));
            }
            else
            {
                picSize = new Size((int)(this.vlcControl.Width * 0.9), (int)(this.vlcControl.Height * 0.9));
                if (picSize.Width < this.vlcControl.Parent.Size.Width)
                {
                    picSize = this.vlcControl.Parent.Size;
                }
            }

            var mp = new Point((100 * e.X) / this.Width, (100 * e.Y) / this.Height);
            var p = new Point((picSize.Width * mp.X) / 100, (picSize.Height * mp.Y) / 100);
            var picPosition = new Point((e.X - p.X), (e.Y - p.Y));
            if (picSize == this.vlcControl.Parent.Size)
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
                Size = this.vlcControl.Size,
                Location = this.vlcControl.Location,
                Visible = true
            };

            this.Controls.Add(temp);

            this._actualSize = direction ? this._actualSize + 1 : this._actualSize - 1;

            this.vlcControl.Size = picSize;
            this.vlcControl.Location = picPosition;
            this.vlcControl.Visible = true;
            this.Controls.Remove(temp);
        }

        private void VlcLiveUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick(this);

                if (DigitalZoomStatus)
                {
                    if (this.vlcControl.Location.X < 0)
                    {
                        var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);
                        var mp = new Point((100 * mouse.X) / this.Width, (100 * mouse.Y) / this.Height);
                        var p = new Point((this.vlcControl.Width * mp.X) / 100, (this.vlcControl.Height * mp.Y) / 100);
                        var picPosition = new Point((mouse.X - p.X), (mouse.Y - p.Y));
                        this.vlcControl.Location = picPosition;
                    }
                }
            }
        }

        private void VlcLiveUserControl_Click(object sender, EventArgs e)
        {
            if (CameraSelected != null)
            {
                CameraSelected(this, Camera);
            }
        }

        private void VlcControl_Log(object sender, Vlc.DotNet.Core.VlcMediaPlayerLogEventArgs e)
        {
        }

        private void VlcControl_EncounteredError(object sender, Vlc.DotNet.Core.VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            SetVisivility(PlaybackConnectionState.Disconnected);
        }

        private void VlcControl_Stopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            //SetVisivility(PlaybackConnectionState.Disconnected);
        }

        private void VlcControl_Playing(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            _currentRetryCount = 0;
            this._isReconnecting = false;

            this.IsPlaying = true;

            SetVisivility(PlaybackConnectionState.Connected);
        }

        private void VlcControl_Paused(object sender, Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs e)
        {
        }

        private void VlcControl_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            Task.Run(async () => await TriggerReconnectAsync());
        }

        public new void Dispose()
        {
            _reconnectCts?.Cancel();
            _reconnectCts?.Dispose();

            if (this.vlcControl != null)
            {
                this.vlcControl.EndReached -= VlcControl_EndReached;
                this.vlcControl.Paused -= VlcControl_Paused;
                this.vlcControl.Playing -= VlcControl_Playing;
                this.vlcControl.Stopped -= VlcControl_Stopped;
                this.vlcControl.EncounteredError -= VlcControl_EncounteredError;
                this.vlcControl.Log -= VlcControl_Log;
                this.vlcControl.Dispose();
            }
        }

        private void VlcLiveUserControl_Resize(object sender, EventArgs e)
        {
            this.vlcControl.Video.AspectRatio = $"{this.vlcControl.Width}:{this.vlcControl.Height}";
            Reconnecting.DisplayLogo(this.Width, this.Height, ref panelNoConnection, ref panelFondoLogo);
        }

        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }
        public List<Profile> Profiles
        {
            get => new List<Profile> { Profile.MainStream, Profile.SubStream };
        }
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

        public bool IsSequencingEnabled => this.Camera.Sequencing != null;
        public bool IsInitAudio { get; set; }

        private List<ButtonsContextBar> GetButtons()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>();

            //if (Camera.AudioEnabled)
            //{
            //    commands.Add(ButtonsContextBar.Listen);
            //    commands.Add(ButtonsContextBar.Talk);
            //}
            //if (Camera.PtzEnabled)
            //{
            //    commands.Add(ButtonsContextBar.Ptz);
            //    commands.Add(ButtonsContextBar.Presets);
            //    commands.Add(ButtonsContextBar.CreatePreset);
            //    commands.Add(ButtonsContextBar.Guards);
            //}

            commands.AddRange(new List<ButtonsContextBar>
            {
                ButtonsContextBar.Fullscreen,
                ButtonsContextBar.Snapshot,
            });

            return commands;
        }

        private List<ButtonsContextBar> GetButtonsAudioPtz()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>();

            //if (Camera.AudioEnabled)
            //{
            //    commands.Add(ButtonsContextBar.Listen);
            //}
            //if (Camera.TalkEnabled)
            //{
            //    commands.Add(ButtonsContextBar.Talk);
            //}

            //if (Camera.PtzEnabled)
            //{
            //    commands.Add(ButtonsContextBar.Ptz);
            //    commands.Add(ButtonsContextBar.Presets);
            //    commands.Add(ButtonsContextBar.Guards);
            //    commands.Add(ButtonsContextBar.CreatePreset);
            //}
            return commands;
        }

        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnVideoEventHandler OnVideo;
        public event ButtonPressedEventHandler PressedButtons;
        public event OnDriverDispose OnDispose;
        public event OnSequecingClick OnSequencing;
        public event OnInitializeAudioEventHandler OnInitializeAudio;
        public event OnAddExtraProfilesEventHandler OnAddExtraProfiles;

        private delegate void SafeCallDelegate(PlaybackConnectionState connectionState);

        public bool CallGuard(ActivateGuardDTO guard)
        {
            return false;
        }

        public bool CallPreset(PresetDTO preset)
        {
            return false;
        }

        public bool ChangeProfile(Profile profile, bool autoSwitching = false)
        {
            Profile = profile;
            return false;
        }

        public void DisposeDragged()
        {
            throw new NotImplementedException();
        }

        public GuardForCreationDTO GetGuard(int guardId)
        {
            throw new NotImplementedException();
        }

        public IOPortState InputPortState()
        {
            throw new NotImplementedException();
        }

        public GuardDTO[] ListGuards()
        {
            throw new NotImplementedException();
        }

        public PresetDTO[] ListPresets()
        {
            throw new NotImplementedException();
        }

        public void OuputPortChangeState(IOPortState state)
        {
            throw new NotImplementedException();
        }

        public IOPortState OuputPortState()
        {
            throw new NotImplementedException();
        }

        public bool Play()
        {
            if (IsPlaying)
            {
                return true;
            }

            try
            {
                Logger.Log(String.Format(" Play Generic entered {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                IManufactureUri manufactureUri = ManufactureUriFactory.Instance(this.Camera, this.Profile);

                SetVisivility(PlaybackConnectionState.Connecting);

                this.vlcControl.Play(manufactureUri.StreamLiveUri());

                Logger.Log(String.Format(" Play Generic  Connected {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                return true;
            }
            catch (Exception ex)
            {
                IsPlaying = false;
                SetVisivility(PlaybackConnectionState.Reconnecting);
                Logger.Log(String.Format("Play Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                return false;
            }
        }

        public bool RemoveGuard(GuardDTO guard)
        {
            return false;
        }

        public bool RemovePreset(PresetDTO preset)
        {
            return false;
        }

        public bool SaveGuard(GuardForCreationDTO guard)
        {
            return false;
        }

        public bool SavePreset(PresetDTO preset)
        {
            return false;
        }

        public async Task<bool> Snapshot(string path, string token, int id)
        {
            try
            {
                this.vlcControl.TakeSnapshot(path);
                return true;
            }
            catch (Exception ex)
            {
                notification.Show(ex.Data.ToString(), null);
                return false;
            }
        }

        public bool StateGuard(GuardDTO guard)
        {
            return false;
        }

        public bool Stop()
        {
            if (!IsPlaying) return true;

            try
            {
                this._isReconnecting = false;
                this._currentRetryCount = 0;

                _reconnectCts?.Cancel();

                //this.vlcControl.Stop();
                IsPlaying = false;
            }
            catch (Exception ex)
            {
                notification.Show("Camera : " + Camera.Name + "Error code: " + ex.Data.ToString(), null);
                IsPlaying = true;
            }
            return !IsPlaying;
        }

        public bool StopGuard(ActivateGuardDTO guard)
        {
            return false;
        }

        public void SubcribePTZEvent()
        {
            Unsubcribed = false;
            ptzUserControl.PtzJoystickStateEvent += PtzjoystickStateEvent;
            ptzUserControl.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
        }

        public void UnsubcribePTZEvent()
        {
            ptzUserControl.PtzJoystickStateEvent -= PtzjoystickStateEvent;
            ptzUserControl.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
            Unsubcribed = true;
        }

        public void ToggleFullScreen()
        {
        }

        public bool ToggleInstantPlayback()
        {
            InstantPlaybackStatus = !InstantPlaybackStatus;

            return InstantPlaybackStatus;
        }

        public bool ToggleListen(bool Listen)
        {
            return false;
        }

        public bool ToggleTalk()
        {
            return false;
        }

        public bool ToggleTalk(bool talkStatus)
        {
            return false;
        }

        public bool ToogleDigitalZoom()
        {
            if (DigitalZoomStatus)
            {
                this.vlcControl.Size = this.vlcControl.Parent.Size;
                this.vlcControl.Location = new Point(0, 0);
                this.vlcControl.Visible = true;
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.Cross;
            }

            DigitalZoomStatus = !DigitalZoomStatus;
            return DigitalZoomStatus;
        }

        public bool TooglePtz()
        {
            {
                PtzStatus = !PtzStatus;
                if (ptzUserControl != null)
                {
                    ptzUserControl.Visible = PtzStatus;
                    if (PtzStatus)
                    {
                        ButtonZoomIn.BringToFront();
                        ButtonZoomOut.BringToFront();
                        ptzUserControl.BringToFront();
                        ptzUserControl.StartJoystick();
                        ptzUserControl.PtzJoystickStateEvent += PtzjoystickStateEvent;
                        ptzUserControl.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
                    }
                    else
                    {
                        ButtonZoomIn.SendToBack();
                        ButtonZoomOut.SendToBack();
                        ptzUserControl.SendToBack();
                        ptzUserControl.StopJoystick();
                        ptzUserControl.PtzJoystickStateEvent -= PtzjoystickStateEvent;
                        ptzUserControl.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
                    }
                }
                ButtonZoomIn.Visible = PtzStatus;
                ButtonZoomOut.Visible = PtzStatus;
                return PtzStatus;
            }
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

        private void OnVlcControlNeedLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "Vlc"));
        }

        private void ButtonZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void ButtonZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void ButtonZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void ButtonZoomIn_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void SetVisivility(PlaybackConnectionState connectionState)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (this.vlcControl.InvokeRequired)
            {
                var d = new SafeCallDelegate(SetVisivility);
                vlcControl.Invoke(d, new object[] { connectionState });
            }
            try
            {
                switch (connectionState)
                {
                    case PlaybackConnectionState.Disconnected:
                        panelNoConnection.Visible = true;
                        panelFondoLogo.Visible = true;
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
                        panelNoConnection.Visible = false;
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
                        panelFondoLogo.Visible = true;
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
                        panelNoConnection.BringToFront();
                        panelFondoLogo.Visible = true;
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
                Reconnecting.DisplayLogo(this.Width, this.Height, ref panelNoConnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"setVisivility Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        private void PtzUserControl_ButtonMouseUp(object sender, PtzMovement e)
        {
        }

        private void PtzUserControl_ButtonMouseDown(object sender, PtzMovement e)
        {
        }

        private void SequencingUserControl_ButtonClick(object sender, int e)
        {
            OnSequencing?.Invoke(e);
        }

        private void PtzjoystickStateEvent(List<ActionCommand> actionCommands)
        {
            if (Unsubcribed)
            {
                return;
            }

            foreach (ActionCommand act in actionCommands)
            {
                if (act.isInvoked)
                {
                }
                else
                {
                }
            }
        }

        private void PtzJoystickButtonEvent(List<ActionCommand> pressedButtons)
        {
            if (Unsubcribed)
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

        private List<ActionCommand> ExecuteZoomCommand(List<ActionCommand> pressedButtons)
        {
            foreach (ActionCommand act in pressedButtons.Where(x => (x.command == PtzCommand.ZOOM_ADD_CONTROL || x.command == PtzCommand.ZOOM_DEC_CONTROL) && x.isInvoked).ToList())
            {
                if (act.isInvoked)
                {
                }
                else
                {
                }
            }
            return pressedButtons.Where(x => !(x.command == PtzCommand.ZOOM_ADD_CONTROL || x.command == PtzCommand.ZOOM_DEC_CONTROL)).ToList();
        }

        private List<ActionCommand> ExecuteCallPresetCommand(List<ActionCommand> pressedButtons)
        {
            return pressedButtons;
        }

        private async Task TriggerReconnectAsync()
        {
            if (this._isReconnecting) return;

            this._isReconnecting = true;
            SetVisivility(PlaybackConnectionState.Reconnecting);

            _reconnectCts?.Cancel();
            _reconnectCts?.Dispose();
            _reconnectCts = new CancellationTokenSource();
            CancellationToken token = _reconnectCts.Token;

            bool reconnected = false;

            try
            {
                await Task.Delay(1000, token);

                // Tiempos de espera escalonados
                for (int stage = 0; stage < _staggeredDelaysSeconds.Length; stage++)
                {
                    int waitTime = _staggeredDelaysSeconds[stage];

                    if (waitTime > 0)
                    {
                        Logger.Log($"Wait stage {stage}: Waiting {waitTime} seconds before next retry batch...", LogPriority.Information);
                        await Task.Delay(waitTime * 1000, token);
                    }

                    _currentRetryCount = 0;

                    // Reintentos por iteración
                    while (_currentRetryCount < this.retryLimit)
                    {
                        token.ThrowIfCancellationRequested();

                        _currentRetryCount++;
                        Logger.Log($"Reconnecting attempt {_currentRetryCount}/{retryLimit} (Stage {stage + 1}/{_staggeredDelaysSeconds.Length}) for {Camera.Name}", LogPriority.Information);

                        if (_currentRetryCount > 1)
                        {
                            await Task.Delay(10000, token);
                        }

                        this.IsPlaying = false;

                        await Task.Run(() => Play(), token);

                        int timeoutMs = 15000;
                        int elapsedMs = 0;
                        int checkInterval = 250;

                        while (!this.IsPlaying && elapsedMs < timeoutMs)
                        {
                            token.ThrowIfCancellationRequested();
                            await Task.Delay(checkInterval, token);
                            elapsedMs += checkInterval;
                        }

                        if (this.IsPlaying)
                        {
                            reconnected = true;
                            break;
                        }
                        else
                        {
                            await Task.Run(() => this.vlcControl.Stop(), token);
                        }
                    }

                    // Si logramos conectar salimos del ciclo exterior escalonado
                    if (reconnected) break;
                }
            }
            catch (TaskCanceledException)
            {
                // Ciclo de reconexión cancelado porque el usuario llamó a Stop() o cerró la UI
                Logger.Log($"Reconnect cancelled for {Camera.Name}", LogPriority.Information);
                return;
            }

            // Desconexion definitiva si se agotan las etapas de intento
            if (!reconnected)
            {
                SetVisivility(PlaybackConnectionState.Disconnected);
                this._isReconnecting = false;
            }
        }
    }
}
