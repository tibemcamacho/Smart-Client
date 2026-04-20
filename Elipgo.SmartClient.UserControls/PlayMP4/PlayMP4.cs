using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.Services.Services.Implement;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;

namespace Elipgo.SmartClient.UserControls.PlayMP4
{
    public partial class PlayMP4 : UserControl
    {
        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private delegate void SafeCallDelegate(PlaybackConnectionState connectionState);

        private bool _painted = false;
        private string _filePath;
        private bool _paused = false;
        private float _currentPosition = 0;
        private DateTime _selectedDateTime;
        private DateTime initialTime = DateTime.Now.AddMinutes(-30);
        private DateTime? _endDate = null;
        private int moveTo = 0;
        private PlaySpeed currentSpeed = PlaySpeed.NORMAL;
        private PlaybackState state = PlaybackState.Stopped;
        private long duration;

        private int _actualSize = 0;
        private int _zoomLimit = int.Parse(Settings.Default["ZoomLimit"].ToString());
        private int _additionalSeconds = 0;

        public bool IsPlaying { get; set; }
        public DateTime ActualTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public CameraDTO Camera { get; set; }
        public bool ZoomStatus { get; set; }

        public PlayMP4(string pathFileMP4, CameraDTO camera, DateTime selectedDateTime, DateTime? selectedEndDateTime = null)
        {
            InitializeComponent();
            _filePath = pathFileMP4;

            Camera = camera;
            StartTime = selectedDateTime;
            _selectedDateTime = selectedDateTime;
            initialTime = StartTime;
            EndTime = (DateTime)selectedEndDateTime;
            this._endDate = (DateTime)selectedEndDateTime;

            this.Load += PlayMP4_Load;
            this.Paint += PlayMP4_Paint;
            this.Resize += PlayMP4_Resize;
            this.MouseWheel += VRec5InstantPlaybackUserControl_MouseWheel;

            this.vlcControl.EndReached += VlcControl_EndReached;
            this.vlcControl.Paused += VlcControl_Paused;
            this.vlcControl.Playing += VlcControl_Playing;
            this.vlcControl.TimeChanged += VlcControl_TimeChanged;
            this.vlcControl.Video.IsMouseInputEnabled = false;
            this.vlcControl.Video.IsKeyInputEnabled = false;

            this.slider.ValueChanged += Slider_ValueChanged;
            this.slider.ValueChangeComplete += Slider_ValueChangeComplete;

            CultureInfo ci = CultureInfo.InstalledUICulture;
            bunifuToolTip1.SetToolTip(this.ButtonSnapshot, ci.Name.Contains("es") ? ButtonsContextBar.Snapshot.GetDescription() : ButtonsContextBar.Snapshot.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonPlay, ci.Name.Contains("es") ? ButtonsContextBar.Play.GetDescription() : ButtonsContextBar.Play.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonFwdSecs, ci.Name.Contains("es") ? ButtonsContextBar.FwdSecs.GetDescription() : ButtonsContextBar.FwdSecs.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonRewSecs, ci.Name.Contains("es") ? ButtonsContextBar.RewSecs.GetDescription() : ButtonsContextBar.RewSecs.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonPause, ci.Name.Contains("es") ? ButtonsContextBar.Pause.GetDescription() : ButtonsContextBar.Pause.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonSlow, ci.Name.Contains("es") ? ButtonsContextBar.Slow.GetDescription() : ButtonsContextBar.Slow.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this.ButtonFast, ci.Name.Contains("es") ? ButtonsContextBar.Fast.GetDescription() : ButtonsContextBar.Fast.GetAttribute<DescriptionEN>().Descripcion);
            ShowButtons();
        }

        private void PlayMP4_Paint(object sender, PaintEventArgs e)
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
            ButtonSlow.Image = FileResources.icon_slow;
            ButtonFast.Image = FileResources.icon_fast;
            LabelSpeed.Text = "1X";
            LabelSpeed.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
            ButtonSnapshot.Image = FileResources.icon_snapshot;
            ButtonZoom.Image = FileResources.icon_digital_zoom_off;

            var tSecs = (EndTime - StartTime).TotalSeconds;
            slider.MaximumValue = (int)tSecs;
            slider.Enabled = true;
        }

        private void PlayMP4_Resize(object sender, EventArgs e)
        {
            this.vlcControl.Video.AspectRatio = $"{this.vlcControl.Width}:{this.vlcControl.Height}";

            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                Form instantPlayerView = Application.OpenForms.OfType<Form>().FirstOrDefault(f => f.Name == "InstantPlayerView" && (string)f.Tag == Camera.IdGuid);
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                if (instantPlayerView != null && instantPlayerView.WindowState == FormWindowState.Maximized)
                {
                    if (main.Width > 1600)
                    {
                        this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.745M), 2));
                        slider.Location = new Point(0, 795);
                    }
                    else if (main.Width >= 1400 && main.Width <= 1600)
                    {
                        this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.7187M), 2));
                        slider.Location = new Point(0, 660);//810
                    }
                    else
                    {
                        this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.6666M), 2));
                        slider.Location = new Point(0, 515);
                    }
                    if (!IsPlaying)
                    {
                        Jump(1, false);
                    }

                }
                else if (instantPlayerView != null && instantPlayerView.WindowState == FormWindowState.Normal)
                {
                    ShowButtons();
                    var panelControlsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.371M), 2));
                    var panelControlsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.047M), 2));
                    this.PanelControls.Size = new Size(panelControlsWidth, panelControlsHeight);

                    var panelVideoWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.371M), 2));
                    var panelVideoHeigth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.325M), 2));
                    this.PanelVideo.Size = new Size(panelVideoWidth, panelVideoHeigth);
                    var sliderY = 357;
                    if (main.Width > 1700)
                    {
                        sliderY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3305M), 2));
                    }
                    else if (main.Width >= 1400 && main.Width <= 1600)
                    {
                        this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3133M), 2));
                        sliderY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.3166M), 2));
                    }
                    else
                    {
                        this.PanelVideo.Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2604M), 2));
                        sliderY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.2539M), 2));
                    }

                    slider.Location = new Point(0, sliderY);

                    slider.BringToFront();
                }

                var btn = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                var btnLocationX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.021M), 2));
                var btnLocation = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014M), 2));

                ButtonPlay.Size = new Size(btn, btn);
                ButtonPlay.Location = new Point(btnLocationX, btnLocation);

                var buttonPauseX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.0843), 2));
                var buttonPauseY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonPause.Size = new Size(btn, btn);
                ButtonPause.Location = new Point(buttonPauseX, buttonPauseY);

                var buttonSlowX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.3090), 2));
                var buttonSlowY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonSlow.Size = new Size(btn, btn);
                ButtonSlow.Location = new Point(buttonSlowX, buttonSlowY);

                var ButtonFastX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.3940), 2));
                var ButtonFastY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonFast.Size = new Size(btn, btn);
                ButtonFast.Location = new Point(ButtonFastX, ButtonFastY);

                var ButtonRewSecsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.4790), 2));
                var ButtonRewSecsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonRewSecs.Size = new Size(btn, btn);
                ButtonRewSecs.Location = new Point(ButtonRewSecsX, ButtonRewSecsY);

                var ButtonFwdSecsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.5399), 2));
                var ButtonFwdSecsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonFwdSecs.Size = new Size(btn, btn);
                ButtonFwdSecs.Location = new Point(ButtonFwdSecsX, ButtonFwdSecsY);

                var ButtonFullScreenX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.705), 2));
                var ButtonFullScreenY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                var ButtonZoomX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.803), 2));
                var ButtonZoomY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonZoom.Size = new Size(btn, btn);
                ButtonZoom.Location = new Point(ButtonZoomX, ButtonZoomY);

                var ButtonSnapshotX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.852), 2));
                var ButtonSnapshotY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                ButtonSnapshot.Size = new Size(btn, btn);
                ButtonSnapshot.Location = new Point(ButtonSnapshotX, ButtonSnapshotY);

                var ButtonVideoclipX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.901), 2));
                var ButtonVideoclipY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.014), 2));

                /*---------------------------*/

                var labelSpeedWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.016M), 2));
                var labelSpeedHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.017M), 2));

                var LabelSpeedX = Convert.ToInt32(Math.Round(Convert.ToDecimal(this.Width * 0.347), 2));
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

        private void OnVlcControlNeedLibDirectory(object sender, VlcLibDirectoryNeededEventArgs e)
        {
            e.VlcLibDirectory = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "Vlc"));
        }

        private void PlayMP4_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                this.vlcControl.Play(new FileInfo(_filePath));
            }
        }

        public new void Dispose()
        {
            if (this.vlcControl != null)
            {
                this.vlcControl.EndReached -= VlcControl_EndReached;
                this.vlcControl.Paused -= VlcControl_Paused;
                this.vlcControl.Playing -= VlcControl_Playing;
                this.vlcControl.TimeChanged -= VlcControl_TimeChanged;
                this.vlcControl.Dispose();
            }
        }

        private void SetState(PlaybackState state)
        {
            this.state = state;
        }

        private void VlcControl_Playing(object sender, VlcMediaPlayerPlayingEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    VlcControl_Playing(sender, e);
                });
                return;
            }

            if (vlcControl != null)
            {
                duration = this.vlcControl.Length;
                this.vlcControl.Position = (float)(new TimeSpan(0, 0, 0, moveTo).TotalMilliseconds / this.vlcControl.Length);
            }
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

                    vlcControl.Stop();
                    return;
                }
                else
                {
                    Task.Run(() => Play());
                }
            }
        }


        private void VlcControl_Paused(object sender, VlcMediaPlayerPausedEventArgs e)
        {
            _currentPosition = vlcControl.Position;
        }

        private void VlcControl_TimeChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    VlcControl_TimeChanged(sender, e);
                });
                return;
            }

            var time = TimeSpan.FromMilliseconds(e.NewTime);
            ActualTime = this._selectedDateTime.AddMilliseconds(time.TotalMilliseconds).AddMilliseconds(moveTo * -1000);

            var sliderValue = (int)(ActualTime - initialTime).TotalSeconds;
            if (sliderValue <= slider.MaximumValue)
            {
                slider.Value = sliderValue;
            }

            if (this._endDate.HasValue && ActualTime >= this._endDate)
            {
                IsPlaying = false;
                moveTo = 0;
                slider.Value = 0;
                _currentPosition = 0;
                this._selectedDateTime = initialTime;
                Pause();
            }
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

        private void ShowPlaySpeed(int nMode)
        {
            if (nMode == 0)
            {
                currentSpeed = PlaySpeed.NORMAL;
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

        private void ButtonSnapshot_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ButtonSnapshot_Click(sender, e);
                });
                return;
            }

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
                Snapshot(saveFileDialog.FileName);
                notification.Show(Resources.SnapshotSaved, () => Process.Start(saveFileDialog.FileName));
            }
        }

        private void ShowButtons()
        {
            this.ButtonSnapshot.Visible = appAuthorization.Exist(ButtonsContextBar.Snapshot.GetAttribute<PermissionPlayback>().PermissionKey);
        }

        private void ButtonZoom_Click(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ButtonZoom_Click(sender, e);
                });
                return;
            }
            ToogleDigitalZoom();
        }

        private void VRec5InstantPlaybackUserControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    VRec5InstantPlaybackUserControl_MouseWheel(sender, e);
                });
                return;
            }

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

        private void Slider_ValueChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Slider_ValueChanged(sender, e);
                });
                return;
            }

            var datetime = initialTime.AddSeconds(slider.Value);
            SliderTooltip.Text = this.initialTime.AddSeconds(slider.Value + _additionalSeconds).ToString("MM/dd HH:mm:ss");
            SliderTooltip.Location = new Point(slider.Location.X, slider.Location.Y - 20);
            SliderTooltip.Visible = true;
            SliderTooltip.BringToFront();
        }

        private void Slider_ValueChangeComplete(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    Slider_ValueChangeComplete(sender, e);
                });
                return;
            }

            try
            {
                SliderTooltip.Visible = false;

                this._selectedDateTime = initialTime.AddSeconds(slider.Value);

                LabelSpeed.Text = "1X";
                currentSpeed = PlaySpeed.NORMAL;
                ButtonFast.Enabled = true;
                ButtonSlow.Enabled = true;

                IsPlaying = false;
                this.vlcControl.Stop();
                moveTo = slider.Value;
                Task.Run(() => Play());
            }
            catch (Exception ex)
            {
                Logger.Log($"Slider_ValueChangeComplete Exception {Camera.Name} {Camera.Host} {Camera.VideoPort} {Camera.User} {ex.Message}", LogPriority.Fatal);
                notification.Show($"{Camera.Name} - {ex.Message}", null);
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

        public bool Pause()
        {
            if (!_paused)
            {
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

        public bool Slow()
        {
            vlcControl.Rate /= (float)1.5;
            ShowPlaySpeed(-1);
            return true;
        }

        public bool Fast()
        {
            this.vlcControl.Rate *= (float)1.5;
            ShowPlaySpeed(1);
            return true;
        }

        public bool Snapshot(string path)
        {
            if (this.InvokeRequired)
            {
                return (bool)this.Invoke(new Func<bool>(() => Snapshot(path)));
            }

            try
            {
                this.vlcControl.TakeSnapshot(path);
                return true;
            }
            catch (Exception ex)
            {
                notification.Show(ex.Message, null);
                return false;
            }
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

                this.vlcControl.Play();

                IsPlaying = true;
                return true;
            }
            catch (Exception)
            {
                IsPlaying = false;
                return false;
            }
        }

    }
}
