using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Drivers.Dahua351.NetSDKCS;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace Elipgo.SmartClient.Drivers.ONVIF
{
    public partial class OnvifLiveUserControl : UserControl, IDriverLive, IDisposable
    {
        AbsOnvif absOnvif;
        private PtzUserControl ptzUserControl1 = null;
        private SequencingUserControl SequencingUserControl;
        private bool Unsubcribed = false;
        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private string filePathSnap;
        public event OnDriverDispose OnDispose;
        private int retryCount = 0;
        private readonly int retryLimit;
        private delegate void SafeCallDelegate(PlaybackConnectionState connectionState);
        private readonly Configuration _config;
        public bool IsSequencingEnabled => this.Camera.Sequencing != null;
        private int _zoomLimit = int.Parse(Common.Properties.Settings.Default["ZoomLimit"].ToString());
        private int _actualSize = 0;

        public OnvifLiveUserControl(CameraDTO camera, Profile profile, bool initAudio, bool isfullscreen = false)
        {
            InitializeComponent();

            _config = SmartClientEnvironmentUtils.GetConfiguration();

            Camera = camera;
            IsInitAudio = initAudio;
            switch (camera.Driver)
            {
                case Common.Enum.Drivers.ONVIFV1:
                    absOnvif = new Onvifv1(Camera.User, Camera.Password, Camera.Host, Camera.HttpPort, Camera.RtspPort);
                    break;
                case Common.Enum.Drivers.ONVIFV2:
                    absOnvif = new Onvifv2(Camera.User, Camera.Password, Camera.Host, Camera.HttpPort, Camera.RtspPort);
                    break;
                case Common.Enum.Drivers.ONVIFV3:
                    absOnvif = new Onvifv3(Camera.User, Camera.Password, Camera.Host, Camera.HttpPort, Camera.RtspPort);
                    break;
            }
            this.Paint += OnvifLiveUserControl_Paint;
            ButtonZoomIn.MouseDown += ButtonZoomIn_MouseDown;
            ButtonZoomOut.MouseDown += ButtonZoomOut_MouseDown;

            ButtonZoomIn.MouseUp += ButtonZoomIn_MouseUp;
            ButtonZoomOut.MouseUp += ButtonZoomOut_MouseUp;
            this.MouseWheel += Picture_MouseWheel;
            this.Resize += OnvifLiveUserControl_Resize;
            retryLimit = int.Parse(_config.AppSettings.Settings["retryLimit"].Value);
            this.vlcControl.Video.IsMouseInputEnabled = false;
            this.vlcControl.Video.IsKeyInputEnabled = false;
            this.vlcControl.MouseClick += new System.Windows.Forms.MouseEventHandler(OnvifLiveUserControl_Click);
            this.Click += OnvifLiveUserControl_Click;
            this.panelNoConnection.Click += OnvifLiveUserControl_Click;

            this.DoubleClick += OnvifLiveUserControl_DoubleClick; ;
            this.panelNoConnection.DoubleClick += OnvifLiveUserControl_DoubleClick;

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
                ptzUserControl1.ButtonMouseDown += PtzUserControl1_ButtonMouseDown;
                ptzUserControl1.ButtonMouseUp += PtzUserControl1_ButtonMouseUp;
                ptzUserControl1.BringToFront();
                ButtonZoomIn.Image = Common.Properties.FileResources.ptz_zoom_in;
                ButtonZoomOut.Image = Common.Properties.FileResources.ptz_zoom_out;
                ButtonZoomIn.Location = new Point(this.Width - 34, this.Height - 64);
                ButtonZoomOut.Location = new Point(this.Width - 34, this.Height - 34);
                ButtonZoomIn.Visible = false;
                ButtonZoomOut.Visible = false;
            }
            ButtonZoomIn.SendToBack();
            ButtonZoomOut.SendToBack();
            this.PanelVideo.Dock = DockStyle.Fill;
            this.vlcControl.Size = this.PanelVideo.Size;
        }

        private void OnvifLiveUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick?.Invoke(this);
            }
        }

        private void ButtonZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void ButtonZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void ButtonZoomOut_MouseDown(object sender, MouseEventArgs e)
        {//-
            PTZControl(PtzMovement.ZoomOut, 0, (int)Camera.ZoomSensitivity, false);
        }

        private void ButtonZoomIn_MouseDown(object sender, MouseEventArgs e)
        {// +             
            PTZControl(PtzMovement.ZoomIn, 0, (int)Camera.ZoomSensitivity, false);
        }

        private void OnvifLiveUserControl_Paint(object sender, PaintEventArgs e)
        {

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
        }

        private void SequencingUserControl_ButtonClick(object sender, int e)
        {
            OnSequencing?.Invoke(e);
        }

        private void PtzUserControl1_ButtonMouseUp(object sender, PtzMovement e)
        {
            switch (e)
            {
                case PtzMovement.Up:
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.Down:
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.Left:
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.Right:
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.Center:
                    break;
                case PtzMovement.UpLeft:
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.DownLeft:
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.UpRight:
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                    break;
                case PtzMovement.DownRight:
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                    break;
            }
        }

        private void PtzUserControl1_ButtonMouseDown(object sender, PtzMovement e)
        {
            switch (e)
            {
                case PtzMovement.Up:
                    absOnvif.PTZControl(PtzMovement.Up, (int)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.Down:
                    absOnvif.PTZControl(PtzMovement.Down, (int)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.Left:
                    absOnvif.PTZControl(PtzMovement.Left, (int)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.Right:
                    absOnvif.PTZControl(PtzMovement.Right, (int)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.Center:
                    break;
                case PtzMovement.UpLeft:
                    absOnvif.PTZControl(PtzMovement.UpLeft, (int)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.DownLeft:
                    absOnvif.PTZControl(PtzMovement.DownLeft, (int)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.UpRight:
                    absOnvif.PTZControl(PtzMovement.UpRight, (int)Camera.MovementSensitivity, 0);
                    break;
                case PtzMovement.DownRight:
                    absOnvif.PTZControl(PtzMovement.DownRight, (int)Camera.MovementSensitivity, 0);
                    break;
            }
        }

        private void PTZControl(PtzMovement type, int param1, int param2, bool isStop)
        {
            try
            {
                absOnvif.PTZControlSZoom(type, param1, param2);
            }
            catch (Exception ex)
            {
                if (ex is Exception)
                {
                    //MessageBox.Show(this, (ex as NETClientExcetion).Message);
                }
            }
        }

        private void OnVlcControlNeedLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "Vlc"));
        }


        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }
        public List<Profile> Profiles
        {
            get => new List<Profile> { Profile.MainStream, Profile.SubStream };
        }
        public List<ButtonsContextBar> Commands => GetButtons();

        private List<ButtonsContextBar> GetButtons()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>();

            //if (Camera.AudioEnabled)
            //{
            //commands.Add(ButtonsContextBar.Listen);
            //commands.Add(ButtonsContextBar.Talk);
            //}


            commands.AddRange(new List<ButtonsContextBar>
            {
                ButtonsContextBar.Fullscreen,
                ButtonsContextBar.Snapshot,
                ButtonsContextBar.Videoclip
            });

            //if (Camera.PtzEnabled)
            //{
            //commands.Add(ButtonsContextBar.Ptz);
            //commands.Add(ButtonsContextBar.Presets);
            //commands.Add(ButtonsContextBar.Guards);
            //commands.Add(ButtonsContextBar.CreatePreset);

            //}

            return commands;
        }

        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();

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
        public bool InstantPlaybackStatus { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
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
            throw new NotImplementedException();
        }

        public bool CallPreset(PresetDTO preset)
        {
            try
            {
                absOnvif.callPreset(preset);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeProfile(Profile profile, bool autoSwitching = false)
        {
            return true;
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
            throw new NotImplementedException();
        }

        public PresetDTO[] ListPresets()
        {
            var response = new List<PresetDTO>();
            try
            {
                response = absOnvif.GetPresetList().Result;

            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" GetPresetList excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }
            return response.ToArray();
        }

        public bool Play()
        {

            try
            {
                Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(100) }, () => SetVisivility(PlaybackConnectionState.Connecting));
                Task<string> data = absOnvif.getStreamUri();
                string result = data.Result.ToString();
                string url = result;
                this.vlcControl.Play(url);
                IsPlaying = true;
                Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(1000 * 6) }, () => SetVisivility(PlaybackConnectionState.Connected));
                return true;
            }
            catch (Exception e)
            {
                if (retryCount <= retryLimit)
                {
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * retryCount) }, () => Play());
                    Logger.Log(String.Format("Onvif Play() Error to Camera login current {4} of {5}:  {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User, retryCount, retryLimit), LogPriority.Information);
                    retryCount++;
                }
                else
                {
                    Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(1000 * 3) }, () => SetVisivility(PlaybackConnectionState.Disconnected));
                    notification.Show(string.Format("{0} - {1}", Camera.Name, e.Message), null);
                    Logger.Log(String.Format("Onvif Play() reached max retry number, then it is  disconnected: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                }

                return false;
            }
        }

        public bool RemoveGuard(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool RemovePreset(PresetDTO preset)
        {
            try
            {
                absOnvif.RemovePreset(preset);
                //this.ListPresets();
                return true;
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" RemovePreset excepcion {0}", e), LogPriority.Fatal);
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
                absOnvif.SavePreset(preset);
                return true;
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" SavePreset excepcion {0}", e), LogPriority.Fatal);
                return false;
            }
        }

        public async Task<bool> Snapshot(string path, string token, int id)
        {
            filePathSnap = path;
            try
            {

                absOnvif.downloadSnapShot(path);
                return true;
            }
            catch (Exception ex)
            {
                if (ex is NETClientExcetion)
                {
                    notification.Show((ex as NETClientExcetion).Message, null);
                }
                return false;
            }
        }

        public bool StateGuard(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool Stop()
        {
            if (!IsPlaying)
            {
                return true;
            }

            try
            {
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
            throw new NotImplementedException();
        }

        public void ToggleFullScreen()
        {
            //this._tempProfile = this.Profile;
            if (this.Profile == Profile.SubStream)
            {
                this.Profile = Profile.MainStream;
                Stop();
                Play();
            }
        }

        public bool ToggleInstantPlayback()
        {
            throw new NotImplementedException();
        }

        public bool ToggleListen(bool Listen)
        {
            throw new NotImplementedException();
        }

        public bool ToggleTalk()
        {
            throw new NotImplementedException();
        }

        public bool ToogleDigitalZoom()
        {
            if (DigitalZoomStatus)
            {
                this.vlcControl.Size = this.vlcControl.Parent.Size;
                this.vlcControl.Location = new Point(0, 0);
                this.vlcControl.Visible = true;
                picture.Size = picture.Parent.Size;
                picture.Location = new Point(0, 0);
                picture.Visible = true;
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
                if (ptzUserControl1 != null)
                {
                    ptzUserControl1.Visible = PtzStatus;
                    if (PtzStatus)
                    {
                        ButtonZoomIn.BringToFront();
                        ButtonZoomOut.BringToFront();
                        ptzUserControl1.BringToFront();
                        ptzUserControl1.StartJoystick();
                        ptzUserControl1.PtzJoystickStateEvent += PtzjoystickStateEvent;
                        ptzUserControl1.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
                    }
                    else
                    {
                        ButtonZoomIn.SendToBack();
                        ButtonZoomOut.SendToBack();
                        ptzUserControl1.SendToBack();
                        ptzUserControl1.StopJoystick();
                        ptzUserControl1.PtzJoystickStateEvent -= PtzjoystickStateEvent;
                        ptzUserControl1.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
                    }
                }
                ButtonZoomIn.Visible = PtzStatus;
                ButtonZoomOut.Visible = PtzStatus;
                return PtzStatus;
            }
        }

        private void Picture_MouseWheel(object sender, MouseEventArgs e)
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
                    absOnvif.PTZControl(ParseToONVIFCommand(act.buttonOrAxis), (int)(Math.Abs(act.Parameter) * Camera.ZoomSensitivity),
                            !act.isInvoked == true ? 1 : 0);
                }
                else
                {
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                }
            }
            return pressedButtons.Where(x => !(x.command == PtzCommand.ZOOM_ADD_CONTROL || x.command == PtzCommand.ZOOM_DEC_CONTROL)).ToList();
        }

        private List<ActionCommand> ExecuteCallPresetCommand(List<ActionCommand> pressedButtons)
        {//TODO para se implementado cuando en onvif preset
            return pressedButtons;
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
                    absOnvif.PTZControl(ParseToONVIFCommand(act.buttonOrAxis), (int)(Math.Abs(act.Parameter) * Camera.MovementSensitivity),
                        !act.isInvoked == true ? 1 : 0);
                }
                else
                {
                    absOnvif.PTZControlStop(PtzMovement.Stop, (int)Camera.MovementSensitivity, 1);
                }
            }
        }

        private PtzMovement ParseToONVIFCommand(ButtonOrAxis command)
        {

            PtzMovement result = PtzMovement.Stop;
            switch (command)
            {
                case ButtonOrAxis.UP_CONTROL:
                    result = PtzMovement.Up;
                    break;
                case ButtonOrAxis.DOWN_CONTROL:
                    result = PtzMovement.Down;
                    break;
                case ButtonOrAxis.LEFT_CONTROL:
                    result = PtzMovement.Left;
                    break;
                case ButtonOrAxis.RIGHT_CONTROL:
                    result = PtzMovement.Right;
                    break;
                case ButtonOrAxis.LEFTTOP:
                    result = PtzMovement.UpLeft;
                    break;
                case ButtonOrAxis.LEFTDOWN:
                    result = PtzMovement.DownLeft;
                    break;
                case ButtonOrAxis.RIGHTTOP:
                    result = PtzMovement.UpRight;
                    break;
                case ButtonOrAxis.RIGHTDOWN:
                    result = PtzMovement.DownRight;
                    break;
                case ButtonOrAxis.ZOOM_ADD_CONTROL:
                    result = PtzMovement.ZoomIn;
                    break;
                case ButtonOrAxis.ZOOM_DEC_CONTROL:
                    result = PtzMovement.ZoomOut;
                    break;
            }
            return result;
        }

        public void UnsubcribePTZEvent()
        {
            ptzUserControl1.PtzJoystickStateEvent -= PtzjoystickStateEvent;
            ptzUserControl1.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
            //cuando se utiliza el joytick y la camara ptz se comienza a mover, si se selecciona otra camara se ejecuta este metodo
            //realizando la desubcripcion a los eventos del joystick, pero ocurre un problema si el usuario mantiene el joystick en un estado de desplazamiento 
            //y se selecciona una nueva camara, esta camara nunca recibe el evento que pare y continua loca en forma indefinida hasta se vuelva a selecciona y ejecutar un 
            //nuevo comando con su stop correspondiente es por esto que 
            //envio un comando cualquiera para detener el movimiento de la camara cuando se descelecciona la camara forzando a parar este o no en ejecuccion
            Unsubcribed = true;
        }

        public void SubcribePTZEvent()
        {
            Unsubcribed = false;
            ptzUserControl1.PtzJoystickStateEvent += PtzjoystickStateEvent;
            ptzUserControl1.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
        }

        public bool VideoClipStart(string path)
        {
            throw new NotImplementedException();
        }

        public bool VideoClipStop()
        {
            throw new NotImplementedException();
        }

        public bool Volume(int value)
        {
            throw new NotImplementedException();
        }


        private void OnvifLiveUserControl_Load(object sender, EventArgs e)
        {
            this.vlcControl.Video.AspectRatio = $"{this.vlcControl.Width}:{this.vlcControl.Height}";
            this.vlcControl.EndReached += VlcControl_EndReached;
            this.vlcControl.Playing += VlcControl_Playing;
            this.vlcControl.Stopped += VlcControl_Stopped;
        }

        private void VlcControl_Stopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            Console.WriteLine("@Stopped");
        }

        private void VlcControl_Playing(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            Console.WriteLine("@Playing");
        }

        private void VlcControl_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
        }

        public bool ToggleTalk(bool talkStatus)
        {
            return false;
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
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_es);
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
                Reconnecting.DisplayLogo(vlcControl.Width, vlcControl.Height, ref panelNoConnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("setVisivility Exception: {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
            }
        }

        public IOPortState InputPortState()
        {
            throw new NotImplementedException();
        }

        public IOPortState OuputPortState()
        {
            throw new NotImplementedException();
        }

        public void OuputPortChangeState(IOPortState state)
        {
            throw new NotImplementedException();
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

        private void OnvifLiveUserControl_Click(object sender, EventArgs e)
        {
            CameraSelected(this, Camera);
        }

        private void OnvifLiveUserControl_Resize(object sender, EventArgs e)
        {
            //this.vlcControl.Video.AspectRatio = $"{this.vlcControl.Width}:{this.vlcControl.Height}";
            Reconnecting.DisplayLogo(this.vlcControl.Width, this.vlcControl.Height, ref panelNoConnection, ref panelFondoLogo);
            this.vlcControl.Size = this.Size;
        }
    }

}
