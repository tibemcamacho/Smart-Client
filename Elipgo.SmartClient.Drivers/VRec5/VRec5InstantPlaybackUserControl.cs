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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;

namespace Elipgo.SmartClient.Drivers.VRec5
{
    public partial class VRec5InstantPlaybackUserControl : UserControl, IDriverInstantPlayback, IDisposable, IConectionNotification
    {
        private PlaybackState state = PlaybackState.Stopped;
        private PlaySpeed currentSpeed = PlaySpeed.NORMAL;
        private PlaySpeed _targetSpeed = PlaySpeed.NORMAL;
        private System.Windows.Forms.Timer _speedDebounceTimer;
        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        private RecorderDTOSmall Recorder;
        private const int MAX_HOUR_END = 12;
        private int MAX_MINUTES = 360;
        private static int secondsBefore = 30;
        private bool isDiagnostic = false;
        private bool _painted = false;
        private DateTime _selectedDateTime;
        private long duration;
        private int moveTo = 0;
        private bool instantPlayBack = true;
        private bool _paused = false;
        private float _currentPosition = 0;
        private DateTime initialTime = DateTime.Now.AddMinutes(-30);
        private delegate void SafeCallDelegate(PlaybackConnectionState connectionState);
        private DateTime? _endDate = null;

        private bool _secondScale;
        private PlayScaleTimeLine _currentScale;
        private int _previewSliderValue;
        private int _sliderCurrentValue;
        private bool _scaleActive;
        private int _currentBlock;
        private int _additionalSeconds;

        public bool IsPlaying { get; set; }

        public VRec5InstantPlaybackUserControl(CameraDTO camera, Profile profile, bool hideControls, DateTime selectedDateTime, RecorderDTOSmall recorder, DateTime? selectedEndDateTime = null)
        {
            InitializeComponent();
            _speedDebounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _speedDebounceTimer.Tick += SpeedDebounceTimer_Tick;
            this.Load += VRec5PlaybackUserControl_Load;
            this.Paint += VRec5PlaybackUserControl_Paint;
            this.Resize += VRec5InstantPlaybackUserControl_Resize;
            this.Click += VRec5InstantPlaybackUserControl_Click;
            this.DoubleClick += VRec5InstantPlaybackUserControl_DoubleClick;
            this.MouseWheel += VRec5InstantPlaybackUserControl_MouseWheel;
            this.vlcControl.Click += VRec5InstantPlaybackUserControl_Click;
            this.vlcControl.DoubleClick += VRec5InstantPlaybackUserControl_DoubleClick;
            this.vlcControl.EndReached += VlcControl_EndReached;
            this.vlcControl.Paused += VlcControl_Paused;
            this.vlcControl.Playing += VlcControl_Playing;
            this.vlcControl.Stopped += VlcControl_Stopped;
            this.vlcControl.EncounteredError += VlcControl_EncounteredError;
            this.vlcControl.PositionChanged += VlcControl_PositionChanged;
            this.vlcControl.Log += VlcControl_Log;
            this.vlcControl.TimeChanged += VlcControl_TimeChanged;
            this.vlcControl.Video.IsMouseInputEnabled = false;
            this.vlcControl.Video.IsKeyInputEnabled = false;
            this.vlcControl.MouseClick += new System.Windows.Forms.MouseEventHandler(Vrec5InstantPlaybackUserControl_Click);
            this.slider.ValueChanged += Slider_ValueChanged;
            this.slider.ValueChangeComplete += Slider_ValueChangeComplete;
            ButtonBookmark.Click += ButtonBookmark_Click;
            BookmarkState = false;

            Recorder = recorder;
            Camera = camera;
            Profile = profile;

            if (hideControls)
            {
                this.PanelControls.Hide();
                this.slider.Hide();
                this.PanelVideo.Dock = DockStyle.Fill;
                this.vlcControl.Size = this.PanelVideo.Size;
                instantPlayBack = false;
            }

            var offset = DateTimeOffset.Now.Offset.TotalMinutes * -1;
            if (!instantPlayBack)
            {
                selectedDateTime = selectedDateTime.AddMinutes(Camera.Gmt);
            }

            this._selectedDateTime = selectedDateTime.AddMinutes(-6);

            if (selectedEndDateTime != null)
            {
                StartTime = selectedDateTime;
                initialTime = StartTime;
                EndTime = (DateTime)selectedEndDateTime;
                this._endDate = (DateTime)selectedEndDateTime;
                instantPlayBack = false;
            }
            else
            {
                StartTime = isDiagnostic ? this._selectedDateTime.AddMinutes(-1 * MAX_MINUTES).AddSeconds(-secondsBefore) : this._selectedDateTime.AddMinutes(-1 * MAX_MINUTES);
                initialTime = StartTime;
                EndTime = this._selectedDateTime;
            }

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
        }

        private int _actualSize = 0;
        private int _zoomLimit = int.Parse(Settings.Default["ZoomLimit"].ToString());
        private void VRec5InstantPlaybackUserControl_MouseWheel(object sender, MouseEventArgs e)
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
            this.PanelControls.BringToFront();
            this.slider.BringToFront();
        }

        private void VRec5InstantPlaybackUserControl_Click(object sender, EventArgs e)
        {
            if (CameraSelected != null)
            {
                CameraSelected(this, Camera);
            }
        }

        private void VRec5InstantPlaybackUserControl_DoubleClick(object sender, EventArgs e)
        {
            if (CameraSelectedDoubleClick != null)
            {
                CameraSelectedDoubleClick(this);

                if (ZoomStatus)
                {
                    if (this.vlcControl.Location.X < 0)
                    {
                        var mouse = new Point(Cursor.Position.X - this.PointToScreen(Point.Empty).X, Cursor.Position.Y - this.PointToScreen(Point.Empty).Y);
                        var mp = new Point(100 * mouse.X / this.Width, 100 * mouse.Y / this.Height);
                        var p = new Point(this.vlcControl.Width * mp.X / 100, this.vlcControl.Height * mp.Y / 100);
                        var picPosition = new Point(mouse.X - p.X, mouse.Y - p.Y);
                        this.vlcControl.Location = picPosition;
                    }
                }
            }
        }

        private void VRec5PlaybackUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (_painted)
            {
                return;
            }

            _painted = true;

            this.vlcControl.Video.AspectRatio = $"{this.vlcControl.Width}:{this.vlcControl.Height}";

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
            LabelSpeed.Text = "1X";
            LabelSpeed.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);

            var tSecs = (EndTime - StartTime).TotalSeconds;
            slider.MaximumValue = (int)tSecs;
            slider.Enabled = true;
        }

        private void VRec5InstantPlaybackUserControl_Resize(object sender, EventArgs e)
        {
            this.vlcControl.Video.AspectRatio = $"{this.vlcControl.Width}:{this.vlcControl.Height}";
            Reconnecting.DisplayLogo(this.Width, this.Height, ref panelNoConnection, ref panelFondoLogo);

            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                Form instantPlayerView = Application.OpenForms.OfType<Form>().FirstOrDefault(f => f.Name == "InstantPlayerView" && (string)f.Tag == Camera.IdGuid);
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                if (instantPlayerView != null && instantPlayerView.WindowState == FormWindowState.Maximized)
                {
                    this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.745M), 2));
                    if (main.Width >= 1366 && main.Width < 1400)
                    {
                        this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.705M), 2));
                    }
                    slider.Location = new Point(0, 800);

                }
                else if (instantPlayerView != null && instantPlayerView.WindowState == FormWindowState.Normal)
                {
                    var panelControlsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.371M), 2));
                    var panelControlsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.047M), 2));
                    this.PanelControls.Size = new Size(panelControlsWidth, panelControlsHeight);

                    var panelVideoWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.371M), 2));
                    var panelVideoHeigth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.325M), 2));
                    this.PanelVideo.Size = new Size(panelVideoWidth, panelVideoHeigth);
                    this.slider.Location = new System.Drawing.Point(0, 361);
                }

                var btn = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                var btnLocationX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.021M), 2));
                var btnLocation = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014M), 2));

                ButtonPlay.Size = new Size(btn, btn);
                ButtonPlay.Location = new Point(btnLocationX, btnLocation);

                var buttonPauseX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.0855), 2));
                var buttonPauseY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonPause.Size = new Size(btn, btn);
                ButtonPause.Location = new Point(buttonPauseX, buttonPauseY);

                var buttonSlowX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.2566), 2));
                var buttonSlowY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonSlow.Size = new Size(btn, btn);
                ButtonSlow.Location = new Point(buttonSlowX, buttonSlowY);

                var ButtonFastX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.3422), 2));
                var ButtonFastY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonFast.Size = new Size(btn, btn);
                ButtonFast.Location = new Point(ButtonFastX, ButtonFastY);

                var ButtonRewSecsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.4824), 2));
                var ButtonRewSecsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonRewSecs.Size = new Size(btn, btn);
                ButtonRewSecs.Location = new Point(ButtonRewSecsX, ButtonRewSecsY);

                var ButtonFwdSecsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.5441), 2));
                var ButtonFwdSecsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonFwdSecs.Size = new Size(btn, btn);
                ButtonFwdSecs.Location = new Point(ButtonFwdSecsX, ButtonFwdSecsY);

                var ButtonZoomX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.8523), 2));
                var ButtonZoomY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonZoom.Size = new Size(btn, btn);
                ButtonZoom.Location = new Point(ButtonZoomX, ButtonZoomY);

                var ButtonFullScreenX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.6998), 2));
                var ButtonFullScreenY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonFullScreen.Size = new Size(btn, btn);
                ButtonFullScreen.Location = new Point(ButtonFullScreenX, ButtonFullScreenY);


                var ButtonBookmarkX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.7517), 2));
                var ButtonBookmarkY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonBookmark.Size = new Size(btn, btn);
                ButtonBookmark.Location = new Point(ButtonBookmarkX, ButtonBookmarkY);

                var ButtonSnapshotX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.8036), 2));
                var ButtonSnapshotY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonSnapshot.Size = new Size(btn, btn);
                ButtonSnapshot.Location = new Point(ButtonSnapshotX, ButtonSnapshotY);


                var labelSpeedWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.016M), 2));
                var labelSpeedHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.017M), 2));

                var LabelSpeedX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.2945), 2));
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

        public new void Dispose()
        {
            if (_speedDebounceTimer != null)
            {
                _speedDebounceTimer.Stop();
                _speedDebounceTimer.Tick -= SpeedDebounceTimer_Tick;
                _speedDebounceTimer.Dispose();
            }
            if (this.vlcControl != null)
            {
                this.vlcControl.EndReached -= VlcControl_EndReached;
                this.vlcControl.Paused -= VlcControl_Paused;
                this.vlcControl.Playing -= VlcControl_Playing;
                this.vlcControl.Stopped -= VlcControl_Stopped;
                this.vlcControl.TimeChanged -= VlcControl_TimeChanged;
                this.vlcControl.PositionChanged -= VlcControl_PositionChanged;
                this.vlcControl.EncounteredError -= VlcControl_EncounteredError;
                this.vlcControl.Log -= VlcControl_Log;
                this.vlcControl.Dispose();
            }
        }

        private void VRec5PlaybackUserControl_Load(object sender, EventArgs e)
        {

        }

        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            var datetime = initialTime.AddSeconds(slider.Value);
            SliderTooltip.Text = this.initialTime.AddSeconds(slider.Value + _additionalSeconds).ToString("MM/dd HH:mm:ss");
            SliderTooltip.Location = new Point(slider.Location.X, slider.Location.Y - 20);
            SliderTooltip.Visible = true;
            SliderTooltip.BringToFront();
        }

        private void Slider_ValueChangeComplete(object sender, EventArgs e)
        {
            try
            {
                SliderTooltip.Visible = false;
                this._selectedDateTime = initialTime.AddSeconds(slider.Value);
                LabelSpeed.Text = "1X";
                currentSpeed = PlaySpeed.NORMAL;
                _targetSpeed = PlaySpeed.NORMAL;
                ButtonFast.Enabled = true;
                ButtonSlow.Enabled = true;
                IsPlaying = false;
                this.vlcControl.Stop();
                Task.Run(() => Play());
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Slider_ValueChangeComplete Exception {0} {1} {2} {3} ", Camera.Name, Camera.Host, Camera.VideoPort.ToString(), Camera.User) + ex.Message, LogPriority.Fatal);
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
        }

        private void VlcControl_Log(object sender, VlcMediaPlayerLogEventArgs e)
        {
            //Logger.Log(string.Format("libVlc : {0} {1} @ {2}", e.Level, e.Message, e.Module), LogPriority.Information);
        }

        private void VlcControl_EncounteredError(object sender, VlcMediaPlayerEncounteredErrorEventArgs e)
        {
            //Logger.Log(string.Format("libVlc : {0}", e.ToString()), LogPriority.Information);
            SetVisivility(PlaybackConnectionState.NoRecording);
        }

        private void VlcControl_Paused(object sender, VlcMediaPlayerPausedEventArgs e)
        {
            SetState(PlaybackState.Paused);
            _currentPosition = vlcControl.Position;
        }

        private void VlcControl_Stopped(object sender, VlcMediaPlayerStoppedEventArgs e)
        {
            SetState(PlaybackState.Stopped);
            SetVisivility(PlaybackConnectionState.Disconnected);
        }

        private void VlcControl_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            duration = this.vlcControl.Length;
            this.vlcControl.Position = (float)(new TimeSpan(0, 0, 0, moveTo).TotalMilliseconds / this.vlcControl.Length);
            this.vlcControl.Visible = false;

            SetVisivility(PlaybackConnectionState.Connecting);
            SetState(PlaybackState.Playing);
        }

        private void VlcControl_EndReached(object sender, VlcMediaPlayerEndReachedEventArgs e)
        {
            IsPlaying = false;
            var newSelectedDateTime = this._selectedDateTime.AddMilliseconds(duration - (moveTo * 1000) + 1000);
            double diff = this._selectedDateTime.Subtract(newSelectedDateTime).TotalSeconds;
            if (diff > 10 || diff < -1)
            {
                this._selectedDateTime = newSelectedDateTime;
                Task.Run(() => Play());
            }
            else
            {
                this._selectedDateTime = newSelectedDateTime.AddMilliseconds((moveTo * 1000) + 1000);
                if (_selectedDateTime > ActualTime)
                {

                    SetVisivility(PlaybackConnectionState.NoRecording);
                    SetState(PlaybackState.Stopped);
                    return;
                }
                else
                {
                    Task.Run(() => Play());
                }
            }
        }

        private void VlcControl_PositionChanged(object sender, VlcMediaPlayerPositionChangedEventArgs e)
        {
            if (!this.vlcControl.Visible)
            {
                SetVisivility(PlaybackConnectionState.Connected);
                this.vlcControl.Visible = true;
            }
        }

        private void VlcControl_TimeChanged(object sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
            var time = TimeSpan.FromMilliseconds(e.NewTime);
            ActualTime = this._selectedDateTime.AddMilliseconds(time.TotalMilliseconds).AddMilliseconds(moveTo * -1000);

            //if (ActualTime > EndTime) 
            //{
            //    //Stop();
            //    return;
            //}

            OnTimeChanged?.Invoke(ActualTime, this);
            OnVideo?.Invoke(true, this);

            var sliderValue = (int)(ActualTime - initialTime).TotalSeconds;
            if (sliderValue <= slider.MaximumValue)
            {
                slider.Value = sliderValue;
            }

            if (this._endDate.HasValue && ActualTime >= this._endDate)
            {
                IsPlaying = false;
                Pause();
            }
        }

        private void Vrec5InstantPlaybackUserControl_Click(object sender, EventArgs e)
        {
            CameraSelected?.Invoke(this, Camera);
        }

        private void ButtonBookmark_Click(object sender, EventArgs e)
        {
            BookmarkState = !BookmarkState;

            OpenBookmark?.Invoke(this, BookmarkState);
        }

        public CameraDTO Camera { get; set; }
        public Profile Profile { get; set; }
        public List<ButtonsContextBar> Commands => GetButtons();

        public List<ButtonsContextBar> CommandsAudioPtz => GetButtonsAudioPtz();

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

            return commands;
        }

        public bool ClipStatus { get; set; }
        public bool ZoomStatus { get; set; }
        public bool FullScreenStatus { get; set; } = false;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool BookmarkState { get; set; }
        public DateTime ActualTime { get; set; }

        public event OnVideoEventHandler OnVideo;
        public event EventHandler<bool> OpenBookmark;
        public event CameraSelectedEventHandler CameraSelected;
        public event OnDriverDispose OnDispose;
        public event OnTimeChangedEventHandler OnTimeChanged;
        public event OnStateChangedEventHandler OnStateChanged;
        public event CameraSelectedDoubleClickEventHandler CameraSelectedDoubleClick;

        public List<ButtonsPlayBackBar> ButtonsNotAllowed()
        {
            return new List<ButtonsPlayBackBar>();
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

        public void Connect(IntPtr HandledBrocaster)
        {

        }

        public void Disconect(IntPtr HandledBrocaster)
        {

        }
        private void ShowButtons()
        {
            this.ButtonSnapshot.Visible = appAuthorization.Exist(ButtonsContextBar.Snapshot.GetAttribute<PermissionPlayback>().PermissionKey);
            this.ButtonBookmark.Visible = appAuthorization.Exist(ButtonsContextBar.Bookmark.GetAttribute<PermissionPlayback>().PermissionKey);
        }

        private void ButtonPlay_Click(object sender, EventArgs e)
        {
            PlayVideo();
        }

        private void ButtonPause_Click(object sender, EventArgs e)
        {
            IsPlaying = false;
            Pause();
        }

        private void ButtonZoom_Click(object sender, EventArgs e)
        {
            ToogleDigitalZoom();
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

        private void ButtonRewSecs_Click(object sender, EventArgs e)
        {
            Jump(30, false);
        }

        private void ButtonFwdSecs_Click(object sender, EventArgs e)
        {
            Jump(30, true);
        }

        private void ButtonSlow_Click(object sender, EventArgs e)
        {
            Slow();
        }

        private void ButtonFast_Click(object sender, EventArgs e)
        {
            Fast();
        }

        public bool Fast()
        {
            if (_targetSpeed < PlaySpeed.MAX)
            {
                _targetSpeed = (PlaySpeed)((int)_targetSpeed + 1);
                ShowPlaySpeed(1);
                _speedDebounceTimer.Stop();
                _speedDebounceTimer.Start();
            }
            return true;
        }

        private void SpeedDebounceTimer_Tick(object sender, EventArgs e)
        {
            _speedDebounceTimer.Stop();
            ApplySpeedToVlc();
        }

        private void ApplySpeedToVlc()
        {
            try
            {
                float targetRate = GetRateForSpeed(_targetSpeed);
                this.vlcControl.Rate = targetRate;
            }
            catch (Exception ex)
            {
                Logger.Log($"ApplySpeedToVlc error: {ex.Message}", LogPriority.Fatal);
            }
        }

        private float GetRateForSpeed(PlaySpeed speed)
        {
            switch (speed)
            {
                case PlaySpeed.DOWN_16: return 1f / 16f;
                case PlaySpeed.DOWN_8:  return 1f / 8f;
                case PlaySpeed.DOWN_4:  return 1f / 4f;
                case PlaySpeed.DOWN_2:  return 1f / 2f;
                case PlaySpeed.NORMAL:  return 1f;
                case PlaySpeed.UP_2:    return 2f;
                case PlaySpeed.UP_4:    return 4f;
                case PlaySpeed.UP_8:    return 8f;
                case PlaySpeed.UP_16:   return 16f;
                default:                return 1f;
            }
        }

        public PlaySpeed GetCurrentSpeed()
        {
            return PlaySpeed.NORMAL;
        }

        private void ShowPlaySpeed(int nMode)
        {
            if (nMode == 0)
            {
                currentSpeed = PlaySpeed.NORMAL;
                _targetSpeed = PlaySpeed.NORMAL;
            }
            else if (nMode > 0)	// Speed up
            {
                if (currentSpeed >= PlaySpeed.MIN && currentSpeed < PlaySpeed.MAX)
                {
                    currentSpeed = (PlaySpeed)((int)currentSpeed + 1);
                }
            }
            else if (nMode < 0)	// Speed down
            {
                if (currentSpeed > PlaySpeed.MIN && currentSpeed <= PlaySpeed.MAX)
                {
                    currentSpeed = (PlaySpeed)((int)currentSpeed - 1);
                }
            }

            UpdateControlsToShowSpeed();
        }

        private void UpdateControlsToShowSpeed()
        {
            switch (currentSpeed)
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

            if (PlaySpeed.MAX == currentSpeed)
            {
                ButtonFast.Enabled = false;
                ButtonSlow.Enabled = true;
            }
            else if (PlaySpeed.MIN == currentSpeed)
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

        public int Hash()
        {
            return string.Format("{0}-{1}-{2}", Camera.Id, Recorder.RecorderType, Recorder.Id).GetHashCode();
        }

        public bool Jump(int sec, bool asc)
        {
            if (asc)
            {
                this.vlcControl.Time += sec * 1000;
            }
            else
            {
                this.vlcControl.Time -= sec * 1000;
            }
            return true;
        }

        public bool Pause()
        {
            if (!_paused)
            {
                CapacityNotAvailable(false);
                this._paused = !this._paused;
                this.vlcControl.Pause();
                if (_paused)
                {
                    ButtonPause.Image = FileResources.icon_pause;
                    ButtonPlay.Image = FileResources.icon_play;
                }
            }
            return this._paused;
        }

        public bool Play()
        {
            try
            {
                if (IsPlaying)
                {
                    return true;
                }

                if (this.InvokeRequired)
                {
                    bool bresult = true;
                    this.Invoke((MethodInvoker)delegate
                    {
                        bresult = Play();
                    });

                    IsPlaying = bresult;
                    return bresult;
                }

                this.vlcControl.Stop();

                LoginDevice();

                IsPlaying = true;
                return true;
            }
            catch (Exception)
            {
                IsPlaying = false;
                return false;
            }
        }

        private async void LoginDevice()
        {
            SetVisivility(PlaybackConnectionState.Connecting);

            DateTime selectedDateTime = this._selectedDateTime.AddMinutes(Camera.Gmt * -1);
            var r = Camera.Recorders.Where(x => x.Id == Recorder.Id).First();
            string url = string.Format("{0}://{1}:{2}/player/vidmeta?from={3}&cameraId={4}", r.HttpProtocol, r.Host, r.HttpPort, selectedDateTime.ToString("yyyyMMddHHmmss"), Camera.Id.ToString().PadLeft(8, '0'));
            Logger.Log(string.Format("Vrec5 LoginDevice {0} {1}", Camera.Name, url), LogPriority.Information);

            using (System.Net.WebClient webClient = new System.Net.WebClient())
            {
                try
                {
                    webClient.Credentials = new NetworkCredential(r.Username, r.Password);
                    string password = string.Empty;
                    try
                    {
                        password = Security.AESDecrypt(r.Password);
                    }
                    catch (Exception)
                    {
                        password = r.Password;
                    }
                    webClient.Headers.Add($"Authorization", $"Basic {Security.Base64Encode($"{r.Username}:{password}")}");
                    webClient.DownloadStringCompleted += WebClient_DownloadStringCompleted;
                    await webClient.DownloadStringTaskAsync(url);
                }
                catch (WebException ex)
                {
                    IsPlaying = false;
                    SetVisivility(PlaybackConnectionState.NoRecording);
                    if (ex.Response != null)
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(ex.Response.GetResponseStream()))
                        {
                            string exResponse = sr.ReadToEnd();
                            Logger.Log(string.Format("Vrec5 LoginDevice {0} ERROR: {1}", Camera.Name, exResponse), LogPriority.Information);
                        }
                    }
                    else
                    {
                        Logger.Log(string.Format("Vrec5 LoginDevice {0} ERROR: {1}", Camera.Name, ex.Message), LogPriority.Information);
                    }
                }
            }
        }


        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                if (e == null)
                {
                    Logger.Log(string.Format("Vrec5 {0} ERROR: DownloadStringCompletedEventArgs is cannot be null", Camera.Name), LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.NoRecording);
                    return;
                }

                if (e.Error != null || e.Result.Contains("error"))
                {
                    Logger.Log(string.Format("Vrec5 {0} ERROR: {1}", Camera.Name, e.Error.Message), LogPriority.Information);
                    SetVisivility(PlaybackConnectionState.NoRecording);
                    return;
                }

                SetVisivility(PlaybackConnectionState.Connected);

                var data = JsonConvert.DeserializeObject<VidMeta>(e.Result);

                var r = Camera.Recorders.Where(x => x.Id == Recorder.Id).First();
                DateTime selectedDateTime = this._selectedDateTime.AddMinutes(Camera.Gmt * -1);
                string password = string.Empty;
                try
                {
                    password = Security.AESDecrypt(r.Password);
                }
                catch (Exception)
                {
                    password = r.Password;//Camera.Password;
                }
                string url = string.Format("{0}://{1}:{2}@{3}:{4}/player/vid?from={5}&cameraId={6}", r.HttpProtocol, r.Username, password, r.Host, r.HttpPort, data.from, Camera.Id.ToString().PadLeft(8, '0'));
                Logger.Log(string.Format("Vrec5 Play {0} Startime: {1}", Camera.Name, this._selectedDateTime), LogPriority.Information);
                Logger.Log(string.Format("Vrec5 Play moveTo {0} from {1} to {2}", data.moveTo, data.from, data.to), LogPriority.Information);
                this.vlcControl.Play(url);
                moveTo = Convert.ToInt32(data.moveTo);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("Vrec5 {0} ERROR: {1}", Camera.Name, ex.Message), LogPriority.Information);
                SetVisivility(PlaybackConnectionState.NoRecording);
            }
        }

        private void SetVisivility(PlaybackConnectionState connectionState)
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            if (this.panelNoConnection.InvokeRequired)
            {
                var d = new SafeCallDelegate(SetVisivility);
                panelNoConnection.Invoke(d, new object[] { connectionState });
            }
            try
            {
                switch (connectionState)
                {
                    case PlaybackConnectionState.Disconnected:
                        panelNoConnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_es);
                        }
                        else
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.disconnected_en);
                        }
                        this.panelNoConnection.BackgroundImageLayout = ImageLayout.Zoom;
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
                        this.panelNoConnection.BackgroundImageLayout = ImageLayout.Zoom;
                        break;
                    case PlaybackConnectionState.Reconnecting:
                        panelNoConnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_es);
                        }
                        else
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.reconnecting_en);
                        }
                        this.panelNoConnection.BackgroundImageLayout = ImageLayout.Zoom;
                        break;
                    case PlaybackConnectionState.Connecting:
                        panelNoConnection.Visible = true;
                        if (ci.Name.Contains("es"))
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_es);
                        }
                        else
                        {
                            this.panelNoConnection.BackgroundImage = new Bitmap(Properties.Resources.connecting_en);
                        }

                        this.panelNoConnection.BackgroundImageLayout = ImageLayout.Zoom;
                        break;
                }
                Reconnecting.DisplayLogo(this.Width, this.Height, ref panelNoConnection, ref panelFondoLogo);
            }
            catch (Exception ex)
            {
                Logger.Log($"setVisivility Exception: {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
            }
        }

        public bool Rewind()
        {
            if (!_paused)
            {
                Pause();
            }

            CapacityNotAvailable(true);
            return true;
        }

        public bool SetStartUpSpeed(int speed)
        {
            bool result = false;
            if (speed == 0)
            {
                return true;
            }

            try
            {
                int variance = speed > 0 ? -1 : 1;
                while (speed != 0)
                {
                    result = true;
                    this.vlcControl.Rate *= (float)1.5;
                    speed += variance;
                    currentSpeed = (PlaySpeed)((int)currentSpeed + (variance * -1));
                }
                UpdateControlsToShowSpeed();
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
            }
            return result;
        }

        public bool Slow()
        {
            if (_targetSpeed > PlaySpeed.MIN)
            {
                _targetSpeed = (PlaySpeed)((int)_targetSpeed - 1);
                ShowPlaySpeed(-1);
                _speedDebounceTimer.Stop();
                _speedDebounceTimer.Start();
            }
            return true;
        }

        public bool Snapshot(string path)
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

        public bool Stop()
        {
            IsPlaying = false;
            this.vlcControl.Stop();
            return false;
        }

        public bool SyncSpeed(PlaySpeed masterSpeed, bool updateLabelSpeed)
        {
            bool result = true;
            try
            {
                if (this.currentSpeed == masterSpeed)
                {
                    return result;
                }
                if (masterSpeed < currentSpeed)
                {
                    while (currentSpeed != masterSpeed && result)
                    {
                        result = true;
                        this.vlcControl.Rate /= (float)1.5;
                        currentSpeed = (PlaySpeed)((int)currentSpeed - 1);
                    }
                }
                else
                {
                    while (currentSpeed != masterSpeed && result)
                    {
                        result = true;
                        this.vlcControl.Rate *= (float)1.5;
                        currentSpeed = (PlaySpeed)((int)currentSpeed + 1);
                    }

                }
            }
            catch (Exception ex)
            {
                notification.Show(string.Format("{0} - {1}", Camera.Name, ex.Message), null);
                Logger.Log(string.Format("SyncSpeed {0} - {1}", Camera.Name, ex.Message), LogPriority.Fatal);
            }
            if (updateLabelSpeed == true)
            {//actualizo el label de velocidad
                UpdateControlsToShowSpeed();
            }
            return result;
        }

        public void ToggleFullScreen()
        {
            try
            {
                FullScreenStatus = !FullScreenStatus;
                this.vlcControl.Video.FullScreen = FullScreenStatus;
            }
            catch (Exception ex)
            {
                notification.Show(ex.Data.ToString(), null);
            }
        }

        public bool ToggleListen()
        {
            return false;
        }

        public bool ToogleDigitalZoom()
        {
            ButtonZoom.Image = FileResources.icon_digital_zoom_on;

            if (ZoomStatus)
            {
                this.vlcControl.Size = this.vlcControl.Parent.Size;
                this.vlcControl.Location = new Point(0, 0);
                this.vlcControl.Visible = true;
                ButtonZoom.Image = FileResources.icon_digital_zoom_off;
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.Cross;
            }

            ZoomStatus = !ZoomStatus;
            return ZoomStatus;
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

        private void SetState(PlaybackState state)
        {
            this.state = state;
            OnStateChanged?.Invoke(state, this);
        }

        public bool SetStartDateTime(DateTime dateTime, bool changeSlider = true, bool isVault = false)
        {
            IsPlaying = false;
            this._selectedDateTime = dateTime;
            initialTime = dateTime;
            StartTime = dateTime;

            if (changeSlider)
            {
                EndTime = StartTime.AddDays(1);
                var tSecs = (EndTime - StartTime).TotalSeconds;
                slider.MaximumValue = (int)tSecs;
            }
            else
            {
                EndTime = Convert.ToDateTime(dateTime.ToString("yyyy-MM-dd 23:59:59"));
            }

            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            return true;
        }

        public bool PlayVideo()
        {
            if (_paused)
            {
                IsPlaying = true;
                this._paused = !this._paused;
                this.vlcControl.Play();
                Thread.Sleep(100);
                this.vlcControl.Position = _currentPosition;
                ButtonPlay.Image = FileResources.icon_play;
                ButtonPause.Image = FileResources.icon_pause;
            }
            return true;
        }

        public bool PlayNoAsync()
        {
            CapacityNotAvailable(false);

            ButtonPlay.Image = FileResources.icon_play;
            ButtonPause.Image = FileResources.icon_pause;
            if (_paused)
            {
                _paused = !_paused;
            }

            Play();
            return true;
        }

        public void SelectSpeed(PlaySpeed speed)
        {
            _targetSpeed = speed;
            currentSpeed = speed;
            ApplySpeedToVlc();
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
