using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using Newtonsoft.Json;
using Splat;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;

namespace Elipgo.SmartClient.Drivers.SyncServer
{
    public partial class SyncServerInstantPlaybackUserControl : UserControl, IDriverInstantPlayback, IDisposable
    {
        private PlaybackState state = PlaybackState.Stopped;

        private readonly ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        public event OnVideoEventHandler OnVideo;
        public event OnTimeChangedEventHandler OnTimeChanged;
        public event OnStateChangedEventHandler OnStateChanged;
        public event EventHandler<bool> OpenBookmark;
        public event CameraSelectedEventHandler CameraSelected;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;
        public event OnDriverDispose OnDispose;

        private bool _painted = false;
        public bool BookmarkState { get; set; }

        public DateTime EndTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime ActualTime { get; set; }
        private DateTime InitDateTime { get; set; }
        private DateTime EndDatetime { get; set; }
        private DateTime CurrentDateTime { get; set; }
        private bool offLine = false;
        private ulong thirtySecond;

        private const int MAX_MINUTES = 360;
        private bool instantPlayBack = true;

        private List<Recordings> recordings = new List<Recordings>();
        private PlaySpeed CurrentSpeed = PlaySpeed.NORMAL;

        private readonly Configuration _config;

        private string URL_GET_RECORDINGS = "http://{0}:{1}/SyncServerWebApi/api/camera/recordings?EntityId={2}&CameraId={3}&StartDate={4}&EndDate={5}";
        private string URL_DOWNLOAD = "http://{0}:{1}/SyncServerWebApi/api/camera/download?EntityId={2}&CameraId={3}&StartDate={4}&EndDate={5}&ProtocolEncoded=H264_1&TypeRecorder=DAHUA_NVR";
        private SyncServerDTO SyncServer { get; set; }
        private int currentRecording = 0;

        private int retryCount = 0;
        private readonly int retryLimit = 0;
        private int tryCount = 0;
        private readonly int tryLimit = 0;

        private readonly int OffsetUtc = 0;
        private RecorderDTOSmall Recorder;

        public SyncServerInstantPlaybackUserControl(CameraDTO camera, Profile profile, bool hideControls, DateTime selectedDateTime, RecorderDTOSmall recorder)
        {
            InitializeComponent();

            _config = SmartClientEnvironmentUtils.GetConfiguration();

            retryLimit = int.Parse(_config.AppSettings.Settings["retryLimit"].Value);
            tryLimit = int.Parse(_config.AppSettings.Settings["tryLimit"].Value);

            Camera = camera;
            Profile = profile;
            Recorder = recorder;

            this.Paint += SyncServerInstantPlaybackUserControl_Paint;
            this.Resize += SyncServerInstantPlaybackUserControl_Resize;
            this.slider.MouseDown += Slider_MouseDown;
            this.slider.MouseUp += Slider_MouseUp;
            this.slider.ValueChanged += Slider_ValueChanged;
            this.slider.ValueChangeComplete += Slider_ValueChangeComplete;
            this.Click += SyncServerInstantPlaybackUserControl_Click;
            this.DoubleClick += SyncServerInstantPlaybackUserControl_DoubleClick;
            if (hideControls)
            {
                this.PanelControls.Hide();
                this.slider.Hide();
                this.vlcControl.Dock = DockStyle.Fill;
                instantPlayBack = false;
            }

            OffsetUtc = (int)DateTimeOffset.Now.Offset.TotalMinutes;
            OffsetUtc = Camera.Gmt;

            if (instantPlayBack)
            {
                StartTime = selectedDateTime.AddMinutes(MAX_MINUTES * -1);
                InitDateTime = StartTime;
                EndTime = selectedDateTime;
                EndDatetime = selectedDateTime;
                CurrentDateTime = selectedDateTime;

            }
            else
            {
                StartTime = selectedDateTime.AddMinutes(OffsetUtc).AddMinutes(MAX_MINUTES * -1);
                InitDateTime = StartTime;
                EndTime = selectedDateTime.AddMinutes(OffsetUtc);
                EndDatetime = selectedDateTime.AddMinutes(OffsetUtc);
                CurrentDateTime = selectedDateTime.AddMinutes(OffsetUtc);
            }

            ButtonBookmark.Click += ButtonBookmark_Click;
            BookmarkState = false;
            ClipStatus = false;
            thirtySecond = (ulong)new TimeSpan(0, 0, 30).Ticks / 10000;

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

            ShowButtons();

            SetVisivility(PlaybackConnectionState.Reconnecting);

            SyncServer = camera.SyncServers.Where(x => x.Id == Recorder.Id).FirstOrDefault();

        }

        private void SyncServerInstantPlaybackUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick?.Invoke(this);
            }
        }

        private void OnVlcControlNeedLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "Vlc"));
        }

        private void ShowButtons()
        {
            this.ButtonSnapshot.Visible = appAuthorization.Exist(ButtonsContextBar.Snapshot.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonBookmark.Visible = appAuthorization.Exist(ButtonsContextBar.Bookmark.GetAttribute<PermissionPlayback>().PermissionKey);
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
                        panelNoConnection.BringToFront();
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
                Reconnecting.DisplayLogo(panelNoConnection.Parent.Width, panelNoConnection.Parent.Height, ref panelNoConnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"setVisivility Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        private void SyncServerInstantPlaybackUserControl_Click(object sender, EventArgs e)
        {
            CameraSelected?.Invoke(this, Camera);
        }

        private void SyncServerInstantPlaybackUserControl_Resize(object sender, EventArgs e)
        {
            this.panelNoConnection.Left = (this.panelNoConnection.Parent.Width - this.panelNoConnection.Width) / 2;
            this.panelNoConnection.Top = (this.panelNoConnection.Parent.Height - this.panelNoConnection.Height) / 2;
        }

        private void ButtonBookmark_Click(object sender, EventArgs e)
        {
            BookmarkState = !BookmarkState;
        }

        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            SliderTooltip.Text = this.InitDateTime.AddSeconds(slider.Value).ToString("MM/dd HH:mm:ss");

            SliderTooltip.Location = new Point(slider.Location.X, slider.Location.Y - 13);
            SliderTooltip.Visible = true;
            SliderTooltip.BringToFront();
        }

        private void Slider_ValueChangeComplete(object sender, EventArgs e)
        {
            SliderTooltip.Visible = false;
            CurrentDateTime = InitDateTime.AddSeconds(slider.Value).AddMinutes(OffsetUtc * -1);
            currentRecording = FindRecording(CurrentDateTime);
            PlayCamera();
        }

        private void Slider_MouseUp(object sender, MouseEventArgs e)
        {
            Slider_ValueChanged(sender, e);
        }

        private void Slider_MouseDown(object sender, MouseEventArgs e)
        {
            Slider_ValueChanged(sender, e);
        }

        private void SyncServerInstantPlaybackUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
            {
                return;
            }

            this._painted = true;

            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            ButtonRewSecs.Image = FileResources.icon_replay_30;
            ButtonFwdSecs.Image = FileResources.icon_forward_30;
            ButtonBookmark.Image = FileResources.icon_bookmarks;
            ButtonFullScreen.Image = FileResources.icon_full_screen;
            ButtonSnapshot.Image = FileResources.icon_snapshot;
            ButtonFast.Image = FileResources.icon_fast;
            ButtonSlow.Image = FileResources.icon_slow;
            LabelSpeed.Text = "1X";
            LabelSpeed.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);

            this.vlcControl.Video.AspectRatio = $"{this.vlcControl.Width}:{this.vlcControl.Height}";
            this.vlcControl.EndReached += VlcControl_EndReached;
            this.vlcControl.PositionChanged += VlcControl_PositionChanged;
            this.vlcControl.Playing += VlcControl_Playing;
            this.vlcControl.Paused += VlcControl_Paused;
            this.vlcControl.Stopped += VlcControl_Stopped;
            this.vlcControl.Video.IsMouseInputEnabled = false;
            this.vlcControl.Video.IsKeyInputEnabled = false;
            this.vlcControl.MouseClick += new System.Windows.Forms.MouseEventHandler(SyncServerInstantPlaybackUserControl_Click);

            Task.Run(() => LoginPlayRecording());

            var tSecs = (EndTime - StartTime).TotalSeconds;
            slider.MaximumValue = (int)tSecs;
            if (instantPlayBack)
            {
                slider.Value = (int)(EndTime - StartTime).TotalSeconds;
            }

            this.SizeChanged += SyncServerInstantPlaybackUserControl_SizeChanged;
        }

        public new void Dispose()
        {
            if (this.vlcControl != null)
            {
                this.vlcControl.EndReached -= VlcControl_EndReached;
                this.vlcControl.PositionChanged -= VlcControl_PositionChanged;
                this.vlcControl.Playing -= VlcControl_Playing;
                this.vlcControl.Paused -= VlcControl_Paused;
                this.vlcControl.Stopped -= VlcControl_Stopped;
                this.vlcControl.Dispose();
            }
        }


        private void LoginPlayRecording()
        {
            Logger.Log(String.Format(" Play SyncBack entered  {0} {1} {2} {3} ", SyncServer.Host, SyncServer.Port, SyncServer.EntityId, Camera.Id), LogPriority.Information);
            SetVisivility(PlaybackConnectionState.Connecting);

            if (LoginDevice())
            {
                currentRecording = FindRecording(CurrentDateTime);
                PlayCamera();
            }
            else
            {
                if (retryCount <= retryLimit)
                {
                    SetVisivility(PlaybackConnectionState.Reconnecting);
                    Threads.RunInOtherThread(new Action[] { () => Thread.Sleep(2000 * retryCount) }, () => LoginPlayRecording());
                    Logger.Log(String.Format("SyncBack Error al realizar el Login: {0} {1} {2} {3} ", SyncServer.Host, SyncServer.Port, SyncServer.EntityId, Camera.Id), LogPriority.Information);
                    retryCount++;
                }
                else
                {
                    SetVisivility(PlaybackConnectionState.Disconnected);
                    Logger.Log(String.Format("SyncBack Error al realizar el Login alcanzo el maximo de reintentos, estado desconectado:  {0} {1} {2} {3} ", SyncServer.Host, SyncServer.Port, SyncServer.EntityId, Camera.Id), LogPriority.Information);
                    notification.Show(string.Format("{0}", Camera.Name), null);
                }
            }
        }

        private bool LoginDevice()
        {
            try
            {
                currentRecording = 0;
                WebClient webClient = new WebClient();
                var url = string.Format(URL_GET_RECORDINGS,
                    SyncServer.Host, SyncServer.Port, SyncServer.EntityId, Camera.Id, StartTime.AddHours(-1).ToString("yyyy-MM-ddTHH:mm:ssZ"), EndTime.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ssZ"));

                Logger.Log(string.Format("Connect to Syncroback {0}", url), LogPriority.Information);

                string response = webClient.DownloadString(url);
                webClient.Dispose();

                this.recordings = JsonConvert.DeserializeObject<List<Recordings>>(response);
                return true;
            }
            catch (WebException wex)
            {
                if (wex.Status == WebExceptionStatus.ProtocolError)
                {
                    if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)
                    {
                        this.recordings.Clear();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void PlayStream(string mediaPath)
        {
            if (this.vlcControl != null)
            {
                string[] mediaOptions = { };

                Uri mediaUri;
                try
                {
                    this.vlcControl.Stop();
                    mediaUri = new Uri(mediaPath);
                    this.vlcControl.Play(mediaUri, mediaOptions);
                }
                catch
                {
                    return;
                }
            }
        }

        private void PlayCamera()
        {
            vlcControl.Stop();
            if (recordings.Count > 0 && currentRecording >= 0)
            {
                SetVisivility(PlaybackConnectionState.Reconnecting);

                var recording = recordings[currentRecording];
                var url = string.Format(URL_DOWNLOAD, SyncServer.Host, SyncServer.Port, SyncServer.EntityId, Camera.Id, recording.StartDate.ToString("yyyy-MM-ddTHH:mm:ssZ"), recording.EndDate.AddSeconds(1).ToString("yyyy-MM-ddTHH:mm:ssZ"));

                Logger.Log(string.Format("Play recording {0}", url), LogPriority.Information);
                PlayStream(url);
            }
            else
            {
                var message = string.Format("{0} - {1}", Camera.Name, CurrentDateTime.AddMinutes(OffsetUtc).ToString("yyyy/MM/dd HH:mm:ss"));
                notification.Show(string.Format(Resources.NoRecordingAvailable, message), null);
            }
        }

        private void VlcControl_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {

                if (currentRecording < recordings.Count - 1)
                {
                    currentRecording++;
                    PlayCamera();
                }

            });
        }

        private void VlcControl_PositionChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerPositionChangedEventArgs e)
        {
            int time = (int)this.vlcControl.VlcMediaPlayer.Time / 1000;
            if (recordings != null && recordings.Count > 0 && currentRecording > -1)
            {
                var actualTime = recordings[currentRecording].StartDate.AddMinutes(OffsetUtc);
                actualTime.AddSeconds(time);
                OnTimeChanged?.Invoke(actualTime, this);
            }
        }

        private void VlcControl_Stopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            SetState(PlaybackState.Stopped);
        }

        private void VlcControl_Paused(object sender, Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs e)
        {
            SetState(PlaybackState.Paused);
        }

        private void VlcControl_Playing(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            SetState(PlaybackState.Playing);
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

        public bool Snapshot(string path)
        {
            try
            {
                vlcControl.TakeSnapshot(path);
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
                FullScreenStatus = !FullScreenStatus;
                vlcControl.Video.FullScreen = FullScreenStatus;
            }
            catch (Exception ex)
            {
                notification.Show(ex.Data.ToString(), null);
            }
        }

        public bool VideoClipStart(string path)
        {
            throw new NotImplementedException();
        }

        public bool VideoClipStop()
        {
            throw new NotImplementedException();
        }

        public bool ToggleListen()
        {
            throw new NotImplementedException();
        }

        public bool Volume(int value)
        {
            throw new NotImplementedException();
        }

        public bool Play()
        {
            CapacityNotAvailable(false);
            vlcControl.Play();
            return true;
        }

        public bool Stop()
        {
            CapacityNotAvailable(false);
            vlcControl.Stop();
            return true;
        }

        public bool Pause()
        {
            CapacityNotAvailable(false);
            vlcControl.Pause();
            return true;
        }

        public bool SetStartDateTime(DateTime dateTime, bool changeSlider = true, bool isVault = false)
        {
            dateTime = dateTime.AddMinutes(OffsetUtc * -1);
            StartTime = dateTime;
            InitDateTime = dateTime;
            var date = DateTime.UtcNow - dateTime;
            EndTime = date.TotalMinutes > 60 ? dateTime.AddMinutes(60) : DateTime.UtcNow.AddMinutes(-2);
            CurrentDateTime = dateTime;

            var tSecs = (EndTime - StartTime).TotalSeconds;
            slider.MaximumValue = (int)tSecs;

            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            if (state == PlaybackState.Paused)
            {
                state = PlaybackState.Playing;
            }

            LoginPlayRecording();
            return true;
        }

        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }

        public List<ButtonsContextBar> Commands => GetButtons();

        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();

        private List<ButtonsContextBar> GetButtonsAudioPtz()
        {
            //throw new NotImplementedException();
            return new List<ButtonsContextBar>();
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

        public bool ClipStatus { get; set; } = false;
        public bool ZoomStatus { get; set; }
        public bool FullScreenStatus { get; set; } = false;

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        public bool Rewind()
        {
            Pause();
            CapacityNotAvailable(true);
            return true;
        }

        public bool Slow()
        {
            CurrentSpeed = (PlaySpeed)((int)CurrentSpeed - 1);
            if (CurrentSpeed == PlaySpeed.NORMAL)
            {
                Play();
                CapacityNotAvailable(false);
                return true;
            }
            else
            {
                Pause();
                CapacityNotAvailable(true);
                return false;
            }

        }
        public bool SyncSpeed(PlaySpeed masterSpeed, bool updateLabelSpeed)
        {
            if (masterSpeed == PlaySpeed.NORMAL)
            {
                if (state != PlaybackState.Playing)
                {
                    Play();
                }
            }
            else
            {
                CurrentSpeed = masterSpeed;
                CapacityNotAvailable(true);
            }
            return true;
        }

        public bool Fast()
        {
            CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
            if (CurrentSpeed == PlaySpeed.NORMAL)
            {
                Play();
                CapacityNotAvailable(false);
                return true;
            }
            else
            {
                Pause();
                CapacityNotAvailable(true);
                return false;
            }
        }

        public bool SetStartUpSpeed(int speed)
        {
            Pause();
            CapacityNotAvailable(true);
            return false;
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

        private void SyncServerInstantPlaybackUserControl_SizeChanged(object sender, EventArgs e)
        {
            this.vlcControl.Video.AspectRatio = $"{this.vlcControl.Width}:{this.vlcControl.Height}";

            //le resto -12 para que se ubique dentro del panel video
            this.slider.Location = new Point(this.vlcControl.Location.X, this.vlcControl.Location.Y + this.vlcControl.Height - 12);
        }

        private void ButtonRewSecs_Click(object sender, EventArgs e)
        {
            Jump(30, false);
        }

        private void ButtonFwdSecs_Click(object sender, EventArgs e)
        {
            Jump(30, true);
        }

        public bool Jump(int sec, bool asc)
        {
            if (asc)
            {
                vlcControl.Time += sec * 1000;
            }
            else
            {
                vlcControl.Time -= sec * 1000;
            }
            return true;
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

        public bool ToogleDigitalZoom()
        {
            return false;
        }

        private void ButtonSlow_Click(object sender, EventArgs e)
        {
            vlcControl.Rate /= 2;
            UpdateControlsToShowSpeed();
        }

        private void ButtonFast_Click(object sender, EventArgs e)
        {
            vlcControl.Rate *= 2;
            UpdateControlsToShowSpeed();
        }

        private void UpdateControlsToShowSpeed()
        {
            switch (vlcControl.Rate)
            {
                case 1:
                    LabelSpeed.Text = "1X";
                    break;
                case 2:
                    LabelSpeed.Text = "2X";
                    break;
                case 4:
                    LabelSpeed.Text = "4X";
                    break;
                case 8:
                    LabelSpeed.Text = "8X";
                    break;
                case 16:
                    LabelSpeed.Text = "16X";
                    break;
                case 0.5f:
                    LabelSpeed.Text = "1/2X";
                    break;
                case 0.25f:
                    LabelSpeed.Text = "1/4X";
                    break;
                case 0.125f:
                    LabelSpeed.Text = "1/8X";
                    break;
                case 0.0625f:
                    LabelSpeed.Text = "1/16";
                    break;
            }

            if (vlcControl.Rate == 16)
            {
                ButtonFast.Enabled = false;
                ButtonSlow.Enabled = true;
            }
            else if (vlcControl.Rate == 0.0625f)
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
            return PlaySpeed.NORMAL;
        }

        public List<ButtonsPlayBackBar> ButtonsNotAllowed()
        {
            return new List<ButtonsPlayBackBar>() { };
        }

        private int FindRecording(DateTime dateTime)
        {
            bool found = false;
            var index = -1;
            while (index < this.recordings.Count - 1 && !found)
            {
                index++;
                Recordings r = this.recordings[index];
                if (dateTime >= r.StartDate && dateTime <= r.EndDate)
                {
                    found = true;
                }
            }
            return found ? index : -1;
        }

        public int Hash()
        {
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
        public void SelectSpeed(PlaySpeed speed)
        {
            PlaySpeed temp = CurrentSpeed;
            //La funcionalidad no esta implementado para axis es por ello cuando vuelvo a Normal speed hago play
            bool result = true;
            while (result && temp != speed)
            {
                if (CurrentSpeed >= PlaySpeed.MIN && CurrentSpeed < PlaySpeed.NORMAL)
                {
                    CurrentSpeed = (PlaySpeed)((int)CurrentSpeed + 1);
                    temp = (PlaySpeed)((int)temp + 1);
                }
                else
                {
                    CurrentSpeed = (PlaySpeed)((int)CurrentSpeed - 1);
                    temp = (PlaySpeed)((int)temp - 1);

                }
            }
            if (CurrentSpeed == speed)
            {
                Play();
            }
            else
            {
                Pause();
                CapacityNotAvailable(true);
            }
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
