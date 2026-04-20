using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Splat;

namespace Elipgo.SmartClient.Drivers.Rtsp
{
    public partial class RtspLiveUserControl : UserControl, IDriverLive, IDisposable
    {
        public event CameraSelectedEventHandler CameraSelected;
        private RtspPlayerControl.RtspPlayerControl rtspPlayer;
        public event OnVideoEventHandler OnVideo;

        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();

        public RtspLiveUserControl(CameraDTO camera, Profile profile)
        {
            Camera = camera;
            Profile = profile;

            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.Click += RtspPlayer_Click;

            try
            {
                rtspPlayer = new RtspPlayerControl.RtspPlayerControl
                {
                    Dock = System.Windows.Forms.DockStyle.Fill
                };
                rtspPlayer.Click += RtspPlayer_Click;
                rtspPlayer.OnClick += RtspPlayer_OnClick;
                this.Controls.Add(rtspPlayer);
            }
            catch (Exception e)
            {
                notification.Show(e.Message, null);
            }
            ListenStatus = false;
            ClipStatus = false;
            TalkStatus = false;
            PtzStatus = false;
            ZoomStatus = false;
        }

        private void RtspPlayer_OnClick(int x, int y)
        {
            CameraSelected(this, Camera);
        }

        public new void Dispose()
        {
            Stop();
        }

        public new void DisposeDragged()
        {
            Stop();
        }

        private void RtspPlayer_Click(object sender, EventArgs e)
        {
            CameraSelected(this, Camera);
        }

        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }
        public List<ButtonsContextBar> Commands { get => GetButtons(); }

        private List<ButtonsContextBar> GetButtons()
        {
            List<ButtonsContextBar> commands = new List<ButtonsContextBar>();

            if (Camera.AudioEnabled)
                commands.Add(ButtonsContextBar.Listen);
            //if (Camera.PtzEnabled)
            //    commands.Add(ButtonsContextBar.Ptz);

            commands.AddRange(new List<ButtonsContextBar>
            {
                ButtonsContextBar.Fullscreen,
                ButtonsContextBar.Snapshot,
                ButtonsContextBar.Videoclip,
            });

            return commands;
        }

        public bool CallGuard(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool CallPreset(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public bool ChangeProfile(Profile profile)
        {
            Stop();
            Profile = profile;
            Connect();
            return true;
        }

        public GuardDTO[] ListGuards()
        {
            throw new NotImplementedException();
        }

        public PresetDTO[] ListPresets()
        {
            throw new NotImplementedException();
        }

        private string MediaURL
        {
            get
            {
                Common.Reflections.IManufactureUri manufactureUri = Common.Reflections.ManufactureUriFactory.Instance(Camera, Profile.None);
                Uri uri = new Uri(manufactureUri.StreamLiveUri());

                if (uri.Scheme != "rtsp")
                {
                    var uriBuilder = new UriBuilder(uri)
                    {
                        Scheme = "rtsp",
                        Port = Camera.RtspPort
                    };
                    uri = uriBuilder.Uri;
                }
                return uri.ToString();
            }
        }

        public bool ListenStatus { get; set; }
        public bool ClipStatus { get; set; }
        public bool TalkStatus { get; set; }
        public bool PtzStatus { get; set; }
        public bool ZoomStatus { get; set; }
        public bool InstantPlaybackStatus { get; set; }
        public bool IsPlaying { get; set; }

        private bool Connect()
        {
            try
            {
                rtspPlayer.MediaURL = MediaURL;
                rtspPlayer.MediaUsername = Camera.User;
                rtspPlayer.MediaPassword = Camera.Password;

                rtspPlayer.Play();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }
        public bool Play()
        {
            if (IsPlaying)
                return true;

            try
            {
                IsPlaying = Connect();
            }
            catch (Exception)
            {
                IsPlaying = false;
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

        public bool SaveGuard(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool SavePreset(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public bool Snapshot(string path)
        {
            try
            {
                return rtspPlayer.SaveCurrentImage(0, path);
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
            if (!IsPlaying)
                return true;

            try
            {
                rtspPlayer.Stop();
                IsPlaying = false;
            }
            catch (Exception)
            {
                IsPlaying = true;
            }
            return !IsPlaying;
        }

        public void ToggleFullScreen()
        {
            rtspPlayer.FullScreen();
        }

        public bool ToggleListen()
        {
            rtspPlayer.Mute = !rtspPlayer.Mute;
            return true;
        }

        public bool ToggleTalk()
        {
            throw new NotImplementedException();
        }

        public bool ToggleInstantPlayback()
        {
            throw new NotImplementedException();
        }

        public bool VideoClipStart(string path)
        {
            try
            {
                rtspPlayer.StartRecordMedia(path);
                ClipStatus = true;
                return true;
            }
            catch (Exception)
            {
                ClipStatus = false;
                return false;
            }
        }

        public bool VideoClipStop()
        {
            try
            {
                rtspPlayer.StopRecordMedia();
                ClipStatus = false;
                return true;
            }
            catch (Exception)
            {
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
            throw new NotImplementedException();
        }

        public bool TooglePtz()
        {
            throw new NotImplementedException();
        }

        public bool StopGuard(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public bool SaveGuard(GuardForCreationDTO guard)
        {
            throw new NotImplementedException();
        }

        public GuardForCreationDTO GetGuard(int guardId)
        {
            throw new NotImplementedException();
        }
    }
}
