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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers.GenericDriver
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

        private bool videoState = false;
        private Profile _tempProfile = Profile.SubStream;
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        private bool offLine = true;
        private bool ImageReceived = false;
        private int maxTryReconnection;
        private int currentTryReconnection;
        private PtzUserControl _ptzUserControl;
        private SequencingUserControl _sequencingUserControl;
        private const int SpeedValue = 4;
        private ManufactureUriAbstract _manufactureUri;
        public bool IsSequencingEnabled => this.Camera.Sequencing != null;
        private StreamSwitcher _streamSwitcher;
        private int _framesCount = 0;

        public AxisLiveUserControl(CameraDTO camera, Profile profile, bool initAudio)
        {
            InitializeComponent();
            DrawAxisPlayer();
            ConfigureAmc();
            Camera = camera;
            Profile = profile;
            IsInitAudio = initAudio;

            this.Click += AxisLiveUserControl_Click;
            this.DoubleClick += AxisLiveUserControl_DoubleClick;
            this.panelFondoLogo.Click += AxisLiveUserControl_Click;
            this.panelFondoLogo.DoubleClick += AxisLiveUserControl_DoubleClick;
            this.Resize += AxisLiveUserControl_Resize;
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

            ButtonZoomIn.Image = Common.Properties.FileResources.ptz_zoom_in;
            ButtonZoomOut.Image = Common.Properties.FileResources.ptz_zoom_out;
            ButtonZoomIn.Location = new Point(this.Width - 34, this.Height - 64);
            ButtonZoomOut.Location = new Point(this.Width - 34, this.Height - 34);
            ButtonTalk.Image = Common.Properties.FileResources.icon_micr_on;
            ButtonTalk.Location = new Point(this.Width - 50, this.Height - 40);
            ButtonTalk.Left = 15;
            ButtonTalk.Visible = false;

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

            ButtonZoomIn.Visible = false;
            ButtonZoomOut.Visible = false;
            _manufactureUri = new AxisUri(camera, Profile.None);

        }

        private void SequencingUserControl_ButtonClick(object sender, int e)
        {
            OnSequencing?.Invoke(e);
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
                        panelNoConnection.BringToFront();
                        panelNoConnection.Visible = this.offLine;
                        panelNoConnection.BackgroundImage = null;
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_en);
                        }
                        break;
                    case PlaybackConnectionState.NoRecording:
                        panelNoConnection.BringToFront();
                        panelNoConnection.Visible = this.offLine;
                        panelNoConnection.BackgroundImage = null;
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.norecording_en);
                        }
                        break;
                    case PlaybackConnectionState.Reconnecting:
                        panelNoConnection.Visible = this.offLine;
                        panelNoConnection.BackgroundImage = null;
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_en);
                        }
                        break;
                    case PlaybackConnectionState.Connecting:
                        panelNoConnection.BringToFront();
                        panelNoConnection.Visible = this.offLine;
                        panelNoConnection.BackgroundImage = null;
                        if (ci.Name.Contains("es"))
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_es);
                        }
                        else
                        {
                            panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_en);
                        }
                        break;
                }
                Reconnecting.DisplayLogo(panelFondoLogo.Width, panelFondoLogo.Height, ref panelNoConnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"SetVisivility Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        public new void Dispose()
        {
            if (amc == null)
            {
                return;
            }

            if (!amc.IsDisposed)
            {
                amc.Mute = true;
                if ((amc.Status & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0)
                {
                    amc.StopRecordMedia();
                }
                this.amc.Stop();
                this.amc.OnError -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.Amc_OnError);
                this.amc.OnMouseMove -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnMouseMoveEventHandler(this.Amc_OnMouseMove);
                this.amc.OnStatusChange -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnStatusChangeEventHandler(this.Amc_OnStatusChange);
                this.amc.OnNewVideoSize -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnNewVideoSizeEventHandler(this.Amc_OnNewVideoSize);
                this.Click -= AxisLiveUserControl_Click;
                this.DoubleClick -= AxisLiveUserControl_DoubleClick;

                this.amc.OnClick -= Amc_OnClick;
                this.amc.OnDoubleClick -= Amc_OnDoubleClick;
                this.amc.OnKeyUp -= Amc_OnKeyUp; ;
                this.amc.OnNewImage -= Amc_OnNewImage;

                //this.amc.Dispose();
                this.amc = null;
            }
        }

        private void AxisLiveUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick(this);
            }
        }

        public void DisposeDragged()
        {
            if (!amc.IsDisposed)
            {
                amc.Mute = true;
                if ((amc.Status & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0)
                {
                    amc.StopRecordMedia();
                }
                amc.Stop();
                amc.Dispose();
            }
        }

        private void AxisLiveUserControl_Click(object sender, EventArgs e)
        {
            CameraSelected(this, Camera);
        }

        private void AxisLiveUserControl_Resize(object sender, EventArgs e)
        {
            Reconnecting.DisplayLogo(this.Width, this.Height, ref panelNoConnection, ref panelFondoLogo);
        }

        private void DrawAxisPlayer()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AxisLiveUserControl));
            this.amc = new AxAXISMEDIACONTROLLib.AxAxisMediaControl();
            ((System.ComponentModel.ISupportInitialize)(this.amc)).BeginInit();
            this.SuspendLayout();

            this.amc.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right;
            this.amc.Enabled = true;
            this.amc.Location = new System.Drawing.Point(0, 0);
            this.amc.Name = "amc";
            this.amc.OcxState = ((AxHost.State)(resources.GetObject("amc.OcxState")));
            this.amc.Dock = DockStyle.Fill;
            this.amc.TabIndex = 0;
            this.amc.TabStop = false;
            this.amc.OnError += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.Amc_OnError);
            this.amc.OnMouseMove += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnMouseMoveEventHandler(this.Amc_OnMouseMove);
            this.amc.OnStatusChange += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnStatusChangeEventHandler(this.Amc_OnStatusChange);
            this.amc.OnNewVideoSize += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnNewVideoSizeEventHandler(this.Amc_OnNewVideoSize);
            this.Controls.Add(this.amc);
            ((System.ComponentModel.ISupportInitialize)(this.amc)).EndInit();
            this.ResumeLayout(false);

            amc.OnClick += Amc_OnClick;
            amc.OnDoubleClick += Amc_OnDoubleClick;
            amc.OnKeyUp += Amc_OnKeyUp; ;
            amc.OnNewImage += Amc_OnNewImage;
        }

        private void Amc_OnKeyUp(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnKeyUpEvent e)
        {
            if (e.keyCode == 27)
            {
                if (this._tempProfile != this.Profile)
                {
                    ChangeProfile(this._tempProfile);
                }
            }
        }

        private void Amc_OnNewImage(object sender, EventArgs e)
        {
            if (offLine == true)
            {//if 
                ImageReceived = true;
                this.currentTryReconnection = 0;
                offLine = false;
                SetVisivility(PlaybackConnectionState.Reconnecting);
                this.amc.OnError += new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.Amc_OnError);
            }
            if (videoState)
            {
                return;
            }

            OnVideo(true, this);
            videoState = true;

        }

        private void Amc_OnClick(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnClickEvent e)
        {
            CameraSelected(this, Camera);
        }

        private void Amc_OnDoubleClick(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnDoubleClickEvent e)
        {
            CameraSelectedDoubleClick(this);
        }

        private void ConfigureAmc()
        {
            this.amc.StretchToFit = true;
            this.amc.MaintainAspectRatio = false;
            this.amc.ShowStatusBar = false;
            this.amc.BackgroundColor = 0; // black
            this.amc.VideoRenderer = (int)AMC_VIDEO_RENDERER.AMC_VIDEO_RENDERER_EVR;
            this.amc.EnableOverlays = true;

            // Configure context menu
            this.amc.EnableContextMenu = false;
            this.amc.ToolbarConfiguration = "-play,-fullscreen,-settings"; //"-pixcount" to remove pixel counter

            // AMC messaging setting
            this.amc.Popups = 0;
            //this.amc.Popups |= (int)AMC_POPUPS.AMC_POPUPS_LOGIN_DIALOG; // Allow login dialog
            //this.amc.Popups |= (int)AMC_POPUPS.AMC_POPUPS_NO_VIDEO; // "No Video" message when stopped
            //amc.Popups |= (int)AMC_POPUPS.AMC_POPUPS_MESSAGES; // Yellow-balloon notification
            this.amc.UIMode = "none";
            this.amc.NetworkTimeout = 4000;
            this.amc.EnableReconnect = true;
            this.amc.SetReconnectionStrategy(60000, 10000, 300000, 30000, 300000, 60000, true);
            this.currentTryReconnection = 0;
            this.maxTryReconnection = 60000 / 10000 + 300000 / 30000 + 300000 / 60000;
        }

        private void Amc_OnError(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEvent e)
        {
            if (ImageReceived == false)
            {// es la primera vez cuando offline esta en true entonces voy a desconetar directamente
                Logger.Log("Camera : " + Camera.Name + "Error code " + e.theErrorCode.ToString("X8") + " ", LogPriority.Information);
                _notification.Show("Camera : " + Camera.Name + "Error code " + e.theErrorCode.ToString("X8"), null);
                SetVisivility(PlaybackConnectionState.Disconnected);
                this.amc.OnError -= new AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnErrorEventHandler(this.Amc_OnError);
            }
            else
            {
                offLine = true;
                if (currentTryReconnection <= maxTryReconnection)
                {
                    currentTryReconnection++;
                    Logger.Log("Camera : " + Camera.Name + "Error code " + e.theErrorCode.ToString("X8") + " ", LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                }
                else
                {
                    Logger.Log("Camera : " + Camera.Name + "Error code " + e.theErrorCode.ToString("X8") + " Disconnected ", LogPriority.Information);
                    _notification.Show("Camera : " + Camera.Name + "Error code " + e.theErrorCode.ToString("X8"), null);
                    SetVisivility(PlaybackConnectionState.Disconnected);
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
            if ((e.theOldStatus & (int)AMC_STATUS.AMC_STATUS_RECORDING) == 0 && // was not recording
                    (e.theNewStatus & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0) // is recording
            {
                //myRecordButton.Text = "Stop Recording";
            }
            else
            {
                //myRecordButton.Text = "Record";
            }
        }

        void Amc_OnNewVideoSize(object sender, AxAXISMEDIACONTROLLib._IAxisMediaControlEvents_OnNewVideoSizeEvent e)
        {
            //if (e.theWidth >= 320 && e.theHeight >= 240)
            //{
            //    // Adapt window size to video size
            //    Width += e.theWidth - amc.Width;
            //    Height += e.theHeight - amc.Height;
            //
            //    if (amc.ShowStatusBar)
            //    {
            //        Height += 22;
            //    }
            //
            //    if (amc.ShowToolbar)
            //    {
            //        Height += 32;
            //    }
            //}
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

        public bool CallGuard(ActivateGuardDTO guard)
        {
            try
            {
                var url = _manufactureUri.CallGuardUri(guard);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
                return (result == "OK" + Environment.NewLine);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool StopGuard(ActivateGuardDTO guard)
        {
            try
            {
                var url = _manufactureUri.StopGuardUri(guard);
                var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
                return (result == "OK" + Environment.NewLine);
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
                return (result == "OK" + Environment.NewLine);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeProfile(Profile profile, bool autoSwitching = false)
        {
            this.Profile = profile;
            Stop();
            return Play();
        }

        public GuardDTO[] ListGuards()
        {
            var response = new List<GuardDTO>();
            try
            {
                switch (Camera.ManufactureCode)
                {
                    case Manufacturer.Dahua:
                        var protocolCaps = GetDahuaCurrentProtocolCaps();
                        string tourMin = ParseKeyDahuaProtocolCaps(protocolCaps, "TourMin");
                        string tourMax = ParseKeyDahuaProtocolCaps(protocolCaps, "TourMax");
                        if (!string.IsNullOrEmpty(tourMin) && !string.IsNullOrEmpty(tourMax))
                        {
                            int min = Int32.Parse(tourMin);
                            int max = Int32.Parse(tourMax);
                            int j = min == 0 ? 1 : min;

                            for (int i = min; i < max; i++, j++)
                            {
                                response.Add(new GuardDTO() { Id = i, Name = "Tour " + j });
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response.ToArray();
        }

        public GuardForCreationDTO GetGuard(int guardId)
        {
            var guardTourList = new List<GuardTourForCreationDTO>();
            return new GuardForCreationDTO()
            {
                Id = guardId,
                Name = "Tour " + guardId,
                isActivated = false,
                GuardTours = guardTourList.ToArray()
            };
        }


        public PresetDTO[] ListPresets()
        {
            var response = new List<PresetDTO>();
            try
            {
                switch (Camera.ManufactureCode)
                {
                    case Manufacturer.Dahua:
                        var protocolCaps = GetDahuaCurrentProtocolCaps();
                        string presetMin = ParseKeyDahuaProtocolCaps(protocolCaps, "PresetMin");
                        string presetMax = ParseKeyDahuaProtocolCaps(protocolCaps, "PresetMax");
                        if (!string.IsNullOrEmpty(presetMin) && !string.IsNullOrEmpty(presetMax))
                        {
                            int min = Int32.Parse(presetMin);
                            int max = Int32.Parse(presetMax);
                            int j = min == 0 ? 1 : min;

                            for (int i = min; i < max; i++, j++)
                            {
                                response.Add(new PresetDTO() { Id = j, Name = "Preset " + j });
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                Logger.Log(String.Format(" Play Generic entered {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                IManufactureUri manufactureUri = ManufactureUriFactory.Instance(this.Camera, this.Profile);

                SetVisivility(PlaybackConnectionState.Connecting);

                //Stop possible streams
                this.amc.Stop();

                // Set properties, deciding what url completion to use by MediaType.
                this.amc.MediaUsername = Camera.User;
                this.amc.MediaPassword = Camera.Password;

                var mediaUrl = manufactureUri.StreamLiveUri();

                if (string.IsNullOrWhiteSpace(mediaUrl))
                {
                    this.amc.Stop();
                    IsPlaying = false;
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    return false;
                }

                this.amc.MediaURL = mediaUrl;

                // IMPLEMENTAR URI PARA PTZ, AUDIO EN TODAS LAS MARCAS
                //
                //this.amc.PTZControlURL = manufactureUri.PtzControlUri();
                //this.amc.AudioConfigURL = manufactureUri.AudioConfigUri();
                //this.amc.AudioTransmitURL = manufactureUri.AudioTrasmitUri();
                this.amc.Mute = true;
                this.amc.UIMode = "none";


                //amc.MediaURL = CompleteURL(Camera., MediaType.h264);

                // Start the streaming
                this.amc.Play();

                Logger.Log(String.Format(" Play Generic  Connected {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User), LogPriority.Information);
                IsPlaying = true;
            }
            catch (Exception ex)
            {
                IsPlaying = false;
                SetVisivility(PlaybackConnectionState.Reconnecting);
                Logger.Log(String.Format("Play Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                this.amc.Stop();
                return false;
            }
            return IsPlaying;
        }

        public bool RemoveGuard(GuardDTO guard)
        {
            if (guard.Id > 0)
            {
                try
                {
                    var deleteUri = _manufactureUri.RemoveGuardTourUri(guard);
                    var result = _manufactureUri.SendRequest(deleteUri, HttpMethod.Get);
                    return (result == "OK" + Environment.NewLine);
                }
                catch (Exception) { }
            }
            return false;
        }

        public bool RemovePreset(PresetDTO preset)
        {
            var url = _manufactureUri.DeletePresetUri(preset);
            var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
            return result == "OK" + Environment.NewLine;
        }

        public bool SaveGuard(GuardForCreationDTO guard)
        {
            try
            {
                if (guard != null)
                {
                    switch (Camera.ManufactureCode)
                    {
                        case Manufacturer.Dahua:
                            bool result = DahuaTour("ClearTour", guard.Id);
                            if (!result)
                            {
                                return false;
                            }

                            if (guard.GuardTours != null)
                            {
                                foreach (GuardTourForCreationDTO tour in guard.GuardTours)
                                {
                                    result = DahuaTour("AddTour", guard.Id, tour.PresetId);
                                    if (!result)
                                    {
                                        return false;
                                    }
                                }
                                _notification.Show(Common.Properties.Resources.GuardTourSaved + ": " + guard.Name, null);
                            }
                            return result;
                    }
                }
            }
            catch (Exception) { }
            return false;
        }

        public bool SavePreset(PresetDTO preset)
        {
            var url = _manufactureUri.SavePresetUri(preset);
            var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
            return result == "OK" + Environment.NewLine;
        }

        public async Task<bool> Snapshot(string path, string token, int id)
        {
            try
            {
                amc.SaveCurrentImage(0, path);
                return true;
            }
            catch (Exception ex)
            {
                _notification.Show("Camera : " + Camera.Name + "Error code " + ex.Data.ToString(), null);
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
                amc.Stop();
                IsPlaying = false;
            }
            catch (Exception ex)
            {
                _notification.Show("Camera : " + Camera.Name + "Error code " + ex.Data.ToString(), null);
                IsPlaying = true;
            }
            return !IsPlaying;
        }

        public void ToggleFullScreen()
        {
            this._tempProfile = this.Profile;
            if (this.Profile == Profile.SubStream)
            {
                this.Profile = Profile.MainStream;
                Stop();
                Play();
            }
            amc.FullScreen = true;
        }

        public bool ToggleListen(bool Listen)
        {
            try
            {
                amc.Mute = Listen;// !amc.Mute;
                ListenStatus = Listen;//!amc.Mute;
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
                    amc.AudioTransmitStop();
                }
                else
                {
                    amc.AudioTransmitStart();
                }

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

        public bool VideoClipStart(string path)
        {
            try
            {
                path = path.Replace(".mp4", ".asf");
                if ((amc.Status & (int)AMC_STATUS.AMC_STATUS_RECORDING) > 0)
                {
                    amc.StopRecordMedia();
                }
                else
                {
                    // Start the recording (video and audio)
                    int recordingFlag = (int)AMC_RECORD_FLAG.AMC_RECORD_FLAG_AUDIO_VIDEO;
                    recordingFlag = (int)AMC_RECORD_FLAG.AMC_RECORD_FLAG_VIDEO;


                    amc.StartRecordMedia(path, recordingFlag, "");
                }
                ClipStatus = true;
                return true;
            }
            catch (Exception ex)
            {
                _notification.Show("Camera : " + Camera.Name + "Error code " + ex.Data.ToString(), null);
                ClipStatus = false;
                return false;
            }

        }

        public bool VideoClipStop()
        {
            try
            {
                amc.StopRecordMedia();
                ClipStatus = false;
                return true;
            }
            catch (Exception ex)
            {
                _notification.Show("Camera : " + Camera.Name + "Error code " + ex.Data.ToString(), null);
                ClipStatus = false;
                return false;
            }
        }

        public bool Volume(int value)
        {
            throw new NotImplementedException();
        }


        private void AxisLiveUserControl_Load(object sender, EventArgs e)
        {

        }

        public bool ToogleDigitalZoom()
        {
            try
            {
                amc.UIMode = (amc.UIMode == "none") ? "digital-zoom" : "none";
                DigitalZoomStatus = amc.UIMode == "digital-zoom";
            }
            catch (Exception)
            {
                DigitalZoomStatus = false;
            }

            return DigitalZoomStatus;
        }

        private bool Unsubcribed = false;
        public void UnsubcribePTZEvent()
        {
            _ptzUserControl.PtzJoystickStateEvent -= PtzjoystickStateEvent;
            _ptzUserControl.PtzJoystickButtonEvent -= PtzJoystickButtonEvent;
            //cuando se utiliza el joytick y la camara ptz se comienza a mover, si se selecciona otra camara se ejecuta este metodo
            //realizando la desubcripcion a los eventos del joystick, pero ocurre un problema si el usuario mantiene el joystick en un estado de desplazamiento 
            //y se selecciona una nueva camara, esta camara nunca recibe el evento que pare y continua loca en forma indefinida hasta se vuelva a selecciona y ejecutar un 
            //nuevo comando con su stop correspondiente es por esto que 
            //envio un comando cualquiera para detener el movimiento de la camara cuando se descelecciona la camara forzando a parar este o no en ejecuccion
            DahuaPTZControl("Up", 0, SpeedValue, true);
            Unsubcribed = true;
        }
        public void SubcribePTZEvent()
        {
            Unsubcribed = false;
            _ptzUserControl.PtzJoystickStateEvent += PtzjoystickStateEvent;
            _ptzUserControl.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
        }

        public bool TooglePtz()
        {
            if (amc.UIMode == "digital-zoom")
            {
                amc.UIMode = "none";
                DigitalZoomStatus = false;
            }

            PtzStatus = !PtzStatus;
            if (_ptzUserControl != null)
            {
                _ptzUserControl.Visible = PtzStatus;
                if (PtzStatus)
                {
                    _ptzUserControl.StartJoystick();
                    _ptzUserControl.PtzJoystickStateEvent += PtzjoystickStateEvent;
                    _ptzUserControl.PtzJoystickButtonEvent += PtzJoystickButtonEvent;
                }
                else
                {
                    _ptzUserControl.StopJoystick();
                    _ptzUserControl.PtzJoystickStateEvent -= PtzjoystickStateEvent;
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
                DahuaPTZControl(ParseToAxisCommand((ButtonOrAxis)System.Enum.Parse(typeof(ButtonOrAxis), act.command.ToString())),
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
            else
            {
                var message = string.Format("{0}", this.Camera.Name);
                _notification.Show(string.Format(Resources.NoPresetAvailable, message), null);
            }
            return pressedButtons.Where(x => !(x.command == PtzCommand.CallPreset)).ToList();
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

        private void PtzjoystickStateEvent(List<ActionCommand> actionCommands)
        {
            if (Unsubcribed)
            {
                return;
            }

            foreach (ActionCommand act in actionCommands)
            {
                DahuaPTZControl(ParseToAxisCommand(act.buttonOrAxis), (int)(Math.Abs(act.Parameter) * SpeedValue), (int)(Math.Abs(act.Parameter2) * SpeedValue), !act.isInvoked);
            }
        }
        private string ParseToAxisCommand(ButtonOrAxis command)
        {
            string result = "";
            switch (command)
            {
                case ButtonOrAxis.UP_CONTROL:
                    result = "Up";
                    break;
                case ButtonOrAxis.DOWN_CONTROL:
                    result = "Down";
                    break;
                case ButtonOrAxis.LEFT_CONTROL:
                    result = "Left";
                    break;
                case ButtonOrAxis.RIGHT_CONTROL:
                    result = "Right";
                    break;
                case ButtonOrAxis.LEFTTOP:
                    result = "LeftUp";
                    break;
                case ButtonOrAxis.LEFTDOWN:
                    result = "LeftDown";
                    break;
                case ButtonOrAxis.RIGHTTOP:
                    result = "RightUp";
                    break;
                case ButtonOrAxis.RIGHTDOWN:
                    result = "RightDown";
                    break;
                case ButtonOrAxis.ZOOM_ADD_CONTROL:
                    result = "ZoomTele";
                    break;
                case ButtonOrAxis.ZOOM_DEC_CONTROL:
                    result = "ZoomWide";
                    break;
            }
            return result;
        }

        public bool ToggleInstantPlayback()
        {
            InstantPlaybackStatus = !InstantPlaybackStatus;

            return InstantPlaybackStatus;
        }

        private void ButtonZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            DahuaPTZControl("ZoomTele", 0, 1, false);
        }

        private void ButtonZoomIn_MouseUp(object sender, MouseEventArgs e)
        {
            DahuaPTZControl("ZoomTele", 0, 1, true);
        }

        private void ButtonZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            DahuaPTZControl("ZoomWide", 0, 1, false);
        }

        private void ButtonZoomOut_MouseUp(object sender, MouseEventArgs e)
        {
            DahuaPTZControl("ZoomWide", 0, 1, true);
        }
        private void PtzUserControl_ButtonMouseUp(object sender, PtzMovement e)
        {
            switch (e)
            {
                case PtzMovement.Up:
                    DahuaPTZControl("Up", 0, SpeedValue, true);
                    break;
                case PtzMovement.Down:
                    DahuaPTZControl("Down", 0, SpeedValue, true);
                    break;
                case PtzMovement.Left:
                    DahuaPTZControl("Left", 0, SpeedValue, true);
                    break;
                case PtzMovement.Right:
                    DahuaPTZControl("Right", 0, SpeedValue, true);
                    break;
                case PtzMovement.Center:
                    break;
                case PtzMovement.UpLeft:
                    DahuaPTZControl("LeftUp", SpeedValue, SpeedValue, true);
                    break;
                case PtzMovement.DownLeft:
                    DahuaPTZControl("LeftDown", SpeedValue, SpeedValue, true);
                    break;
                case PtzMovement.UpRight:
                    DahuaPTZControl("RightUp", SpeedValue, SpeedValue, true);
                    break;
                case PtzMovement.DownRight:
                    DahuaPTZControl("RightDown", SpeedValue, SpeedValue, true);
                    break;
            }
        }

        private void PtzUserControl_ButtonMouseDown(object sender, PtzMovement e)
        {
            switch (e)
            {
                case PtzMovement.Up:
                    DahuaPTZControl("Up", 0, SpeedValue, false);
                    break;
                case PtzMovement.Down:
                    DahuaPTZControl("Down", 0, SpeedValue, false);
                    break;
                case PtzMovement.Left:
                    DahuaPTZControl("Left", 0, SpeedValue, false);
                    break;
                case PtzMovement.Right:
                    DahuaPTZControl("Right", 0, SpeedValue, false);
                    break;
                case PtzMovement.Center:
                    break;
                case PtzMovement.UpLeft:
                    DahuaPTZControl("LeftUp", SpeedValue, SpeedValue, false);
                    break;
                case PtzMovement.DownLeft:
                    DahuaPTZControl("LeftDown", SpeedValue, SpeedValue, false);
                    break;
                case PtzMovement.UpRight:
                    DahuaPTZControl("RightUp", SpeedValue, SpeedValue, false);
                    break;
                case PtzMovement.DownRight:
                    DahuaPTZControl("RightDown", SpeedValue, SpeedValue, false);
                    break;
            }
        }

        private void DahuaPTZControl(string type, int param1, int param2, bool isStop)
        {
            var thread = new Thread(() =>
            {
                try
                {
                    var uri = _manufactureUri.PtzControlUri(type, param1.ToString(), param2.ToString(), (isStop) ? "stop" : "start");
                    _manufactureUri.SendRequest(uri, HttpMethod.Get);
                }
                catch (Exception) { }
            });
            thread.Start();
        }

        private string GetDahuaCurrentProtocolCaps()
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

        private string ParseKeyDahuaProtocolCaps(string input, string key)
        {
            Regex regex = new Regex("caps." + key + "=(\\w+)", RegexOptions.Compiled);
            Match match = regex.Match(input);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return "";
        }
        private bool DahuaTour(string action, int guardId, int presetId = 0)
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["action"] = "start";
            query["channel"] = Camera.Channel.ToString();
            query["code"] = action;
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
            var result = _manufactureUri.SendRequest(url, HttpMethod.Get);
            return result == "OK" + Environment.NewLine;
        }

        public bool ToggleTalk(bool talkStatus)
        {
            try
            {
                if (talkStatus)
                {
                    if (!TalkStatus)
                    {
                        amc.AudioTransmitStart();
                    }
                }
                else
                {
                    if (TalkStatus)
                    {
                        amc.AudioTransmitStop();
                    }
                }
                TalkStatus = talkStatus;
                return true;
            }
            catch (Exception)
            {

                TalkStatus = false;
                return false;
                throw;
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

            if (_streamSwitcher.ShouldSwitchToSubstream(Profile.MainStream == Profile))
            {
                ChangeProfile(Profile.SubStream, true);
                Logger.Log($"CheckAndSwitchStream: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} Switching to SubStream", LogPriority.Information);
            }
            else if (_streamSwitcher.ShouldSwitchToMainstream(!(Profile.MainStream == Profile)))
            {
                ChangeProfile(Profile.MainStream, true);
                Logger.Log($"CheckAndSwitchStream: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} Switching to MainStream", LogPriority.Information);
            }
        }
    }
}