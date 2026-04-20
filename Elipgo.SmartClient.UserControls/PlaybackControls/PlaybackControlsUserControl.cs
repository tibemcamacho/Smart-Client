using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.PlaybackControls
{
    public delegate void CalendarChangedEventHandler(DateTime datetime);
    public delegate void ButtonClicked(ButtonsPlayBackBar button);
    public delegate void PlaySpeedChangeEventHandler(double speed);
    public delegate void ButtonScaleTimeLineClicked(PlayScaleTimeLine scale);


    public partial class PlaybackControlsUserControl : UserControl
    {
        private bool _painted = false;

        public event CalendarChangedEventHandler OnCalendarChanged;
        public event PlaySpeedChangeEventHandler OnPlaySpeedChanged;
        public event ButtonClicked OnButtonClicked;
        public event CalendarChangedEventHandler OnDateChanged;
        public event ButtonScaleTimeLineClicked OnButtonScaleTimeLineClicked;

        private double currentSpeed = 1;
        private bool _resizeLoad = false;
        private static readonly IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        public PlaybackControlsUserControl()
        {
            InitializeComponent();
            this.Paint += PlaybackControlsUserControl_Paint;
            _resizeLoad = true;
            _buttonCalendar.DateTimeClick += ButtonCalendar_DateTimeSelected;
            _zoomTimeLinePlaybackControl.ItemSelectedClicked += ButtonScaleTimeline_Click;
            this.Resize += PlaybackControlsUserControl_Resize;
            _buttonCalendar.DateTimeChange += ButtonCalendar_DateTimeChange;
            ShowButtons();
            ShowPlaySpeed();
            _labelTime.Visible = false;
        }

        private void ButtonCalendar_DateTimeSelected(object sender, Dictionary<string, DateTime> e)
        {
            e.TryGetValue("date", out DateTime date);
            e.TryGetValue("time", out DateTime time);
            if (date != null && time != null)
            {
                var hours = TimeSpan.Parse(time.ToString("HH:mm:ss", CultureInfo.InvariantCulture));

                _labelDate.Text = date.ToString("dd/MM/yyyy");
                _labelTime.Text = time.ToString("HH:mm:ss");
                OnCalendarChanged?.Invoke(DateTime.ParseExact(_labelDate.Text + "_" + _labelTime.Text, "dd/MM/yyyy_HH:mm:ss", CultureInfo.InvariantCulture));
            }
        }

        private void PlaybackControlsUserControl_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
            {
                return;
            }

            this._painted = true;
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            this._buttonBookmark.Image = FileResources.icon_bookmarks;
            this._buttonForward.Image = FileResources.icon_forward;
            this._buttonRew30Secs.Image = FileResources.icon_replay_30;
            this._buttonPlay.Image = FileResources.icon_play;
            this._buttonPause.Image = FileResources.icon_pause;
            this._buttonSlow.Image = FileResources.icon_slow;
            this._buttonFast.Image = FileResources.icon_fast;
            this._buttonFw30Secs.Image = FileResources.icon_forward_30;
            this._labelScale.Image = FileResources.icon_group_scale_time;

            CultureInfo ci = CultureInfo.InstalledUICulture;
            bunifuToolTip1.SetToolTip(this._buttonBookmark, ci.Name.Contains("es") ? ButtonsContextBar.Bookmark.GetDescription() : ButtonsContextBar.Bookmark.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this._buttonForward, ci.Name.Contains("es") ? ButtonsContextBar.Play.GetDescription() : ButtonsContextBar.Play.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this._buttonPlay, ci.Name.Contains("es") ? ButtonsContextBar.Play.GetDescription() : ButtonsContextBar.Play.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this._buttonRew30Secs, ci.Name.Contains("es") ? ButtonsContextBar.RewSecs.GetDescription() : ButtonsContextBar.RewSecs.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this._buttonFw30Secs, ci.Name.Contains("es") ? ButtonsContextBar.FwdSecs.GetDescription() : ButtonsContextBar.FwdSecs.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this._buttonPause, ci.Name.Contains("es") ? ButtonsContextBar.Pause.GetDescription() : ButtonsContextBar.Pause.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this._buttonFast, ci.Name.Contains("es") ? ButtonsContextBar.Fast.GetDescription() : ButtonsContextBar.Fast.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this._buttonSlow, ci.Name.Contains("es") ? ButtonsContextBar.Slow.GetDescription() : ButtonsContextBar.Slow.GetAttribute<DescriptionEN>().Descripcion);
            bunifuToolTip1.SetToolTip(this._labelSpeed, ci.Name.Contains("es") ? ButtonsContextBar.Speed.GetDescription() : ButtonsContextBar.Speed.GetAttribute<DescriptionEN>().Descripcion);
        }
        public void SetDateTime(DateTime datetime)
        {
            _labelDate.Text = datetime.ToString("dd/MM/yyyy");
            _labelTime.Text = datetime.ToString("HH:mm:ss");
            this._buttonCalendar.Date = this._buttonCalendar.Hour = datetime;
        }

        public void UpdateSpeed(double speed)
        {
            this.currentSpeed = speed;
            ShowPlaySpeed();
        }
        private void ButtonPause_Click(object sender, EventArgs e)
        {
            OnButtonClicked?.Invoke(ButtonsPlayBackBar.Pause);
        }

        private void ButtonForward_Click(object sender, EventArgs e)
        {
            OnButtonClicked?.Invoke(ButtonsPlayBackBar.Play);
        }

        private void ButtonBookmark_Click(object sender, EventArgs e)
        {
            OnButtonClicked?.Invoke(ButtonsPlayBackBar.Bookmark);
        }

        private void ButtonRew30Secs_Click(object sender, EventArgs e)
        {
            OnButtonClicked?.Invoke(ButtonsPlayBackBar.RewSecs);
        }

        private void ButtonFw30Secs_Click(object sender, EventArgs e)
        {
            OnButtonClicked?.Invoke(ButtonsPlayBackBar.FwdSecs);
        }

        private void ButtonSlow_Click(object sender, EventArgs e)
        {
            if (currentSpeed > 0.0625)
            {
                currentSpeed /= 2;
                ShowPlaySpeed();
                OnPlaySpeedChanged?.Invoke(currentSpeed);
                OnButtonClicked?.Invoke(ButtonsPlayBackBar.Slow);
            }
        }


        private void ButtonFast_Click(object sender, EventArgs e)
        {
            if (currentSpeed < 16)
            {
                currentSpeed *= 2;

                ShowPlaySpeed();
                OnPlaySpeedChanged?.Invoke(currentSpeed);
                OnButtonClicked?.Invoke(ButtonsPlayBackBar.Fast);
            }
        }
        public void SaveButtonState(bool disableButtons)
        {
            _buttonSlow.Enabled = !disableButtons;
            _buttonFast.Enabled = !disableButtons;
            _labelSpeed.Visible = !disableButtons;
        }

        private void ShowPlaySpeed()
        {

            if (currentSpeed == 0.125)
            {
                this._labelSpeed.Text = "1/8 X";
            }
            else if (currentSpeed == 0.25)
            {
                this._labelSpeed.Text = "1/4 X";
            }
            else if (currentSpeed == 0.5)
            {
                this._labelSpeed.Text = "1/2 X";
            }
            else if (currentSpeed == 1)
            {
                this._labelSpeed.Text = "1 X";
            }
            else if (currentSpeed == 2)
            {
                this._labelSpeed.Text = "2 X";
            }
            else if (currentSpeed == 4)
            {
                this._labelSpeed.Text = "4 X";
            }
            else if (currentSpeed == 8)
            {
                this._labelSpeed.Text = "8 X";
            }
            else if (currentSpeed == 16)
            {
                this._labelSpeed.Text = "16 X";
            }
        }


        public int GetCurrentSpeedInNumberOfJumps()
        {
            if (currentSpeed < 0)
            {
                return (int)(Math.Log(currentSpeed) / Math.Log(2)) * -1;
            }
            return (int)(Math.Log(currentSpeed) / Math.Log(2));
        }

        private void ShowButtons()
        {
            _buttonBookmark.Visible = appAuthorization.Exist(ButtonsContextBar.Bookmark.GetAttribute<PermissionPlayback>().PermissionKey);
            _buttonCalendar.Visible = appAuthorization.Exist(ButtonsContextBar.Calendar.GetAttribute<PermissionPlayback>().PermissionKey);
            _buttonFast.Visible = appAuthorization.Exist(ButtonsContextBar.Fast.GetAttribute<PermissionPlayback>().PermissionKey);
            _buttonForward.Visible = appAuthorization.Exist(ButtonsContextBar.FwdSecs.GetAttribute<PermissionPlayback>().PermissionKey);
            _buttonFw30Secs.Visible = appAuthorization.Exist(ButtonsContextBar.FwdSecs.GetAttribute<PermissionPlayback>().PermissionKey);
            _buttonPause.Visible = appAuthorization.Exist(ButtonsContextBar.Pause.GetAttribute<PermissionPlayback>().PermissionKey);
            _buttonPlay.Visible = appAuthorization.Exist(ButtonsContextBar.Play.GetAttribute<PermissionPlayback>().PermissionKey);
            _buttonRew30Secs.Visible = appAuthorization.Exist(ButtonsContextBar.Rew.GetAttribute<PermissionPlayback>().PermissionKey);
            // ButtonRewind.Visible = appAuthorization.Exist(ButtonsContextBar.Rew.GetAttribute<PermissionPlayback>().PermissionKey);
            _buttonSlow.Visible = appAuthorization.Exist(ButtonsContextBar.Slow.GetAttribute<PermissionPlayback>().PermissionKey);
            _labelSpeed.Visible = _buttonSlow.Visible && _buttonFast.Visible ? true : false;
        }

        public void HideButtons(List<ButtonsPlayBackBar> buttonsToHide)
        {
            /*
            Bookmark,Rewind,Play,Pause,Stop,Slow,Fast,RewSecs,FwdSecs*/
            _buttonPause.Visible = _buttonRew30Secs.Visible = _buttonPlay.Visible = _buttonBookmark.Visible = _buttonFast.Visible = _buttonSlow.Visible = _labelSpeed.Visible = true;
            if (buttonsToHide.Count == 0)
            {
                return;
            }
            if (buttonsToHide.Contains(ButtonsPlayBackBar.Pause))
            {
                _buttonPause.Visible = false;
            }
            if (buttonsToHide.Contains(ButtonsPlayBackBar.RewSecs))
            {
                _buttonRew30Secs.Visible = false;
            }
            if (buttonsToHide.Contains(ButtonsPlayBackBar.Play))
            {
                _buttonPlay.Visible = false;
            }
            if (buttonsToHide.Contains(ButtonsPlayBackBar.Bookmark))
            {
                _buttonBookmark.Visible = false;
            }
            if (buttonsToHide.Contains(ButtonsPlayBackBar.Fast))
            {
                _buttonFast.Visible = false;
            }
            if (buttonsToHide.Contains(ButtonsPlayBackBar.Slow))
            {
                _buttonSlow.Visible = false;
            }
            if (!_buttonSlow.Visible && !_buttonFast.Visible)
            {
                _labelSpeed.Visible = false;
            }
        }

        private void PlaybackControlsUserControl_Resize(object sender, EventArgs e)
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                var btnWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0127M), 2));
                var btnHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.022M), 2));

                foreach (Control c in this.Controls)
                {
                    if (c is Bunifu.Framework.UI.BunifuImageButton)
                    {
                        c.Size = new Size(btnWidth, btnHeight);
                    }
                }

                var buttonCalendarWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0185M), 2));
                var buttonCalendarHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.033M), 2));
                this._buttonCalendar.Size = new Size(buttonCalendarWidth, buttonCalendarHeight);

                var labelScaletimeHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0194M), 2));
                if (this.Width > 1400 && this.Width < 2000)
                {
                    var labelScaletimeWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.04479M), 2));
                    this._labelScaletime.Size = new Size(labelScaletimeWidth, labelScaletimeHeight);

                    var labelScaleX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.5468M), 2));
                    var labelScaleY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0138M), 2));
                    this._labelScale.Location = new Point(labelScaleX, labelScaleY);

                    var labelScaletimeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.5625M), 2));
                    var labelScaletimeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0138M), 2));
                    this._labelScaletime.Location = new Point(labelScaletimeX, labelScaletimeY);

                    var zmTimePybckCtrX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.65104M), 2));
                    var zmTimePybckCtrY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.01204M), 2));
                    _zoomTimeLinePlaybackControl.Location = new Point(zmTimePybckCtrX, zmTimePybckCtrY);

                    var btnCldrX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.41276M), 2));
                    var btnCldrY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.01019M), 2));
                    _buttonCalendar.Location = new Point(btnCldrX, btnCldrY);

                    var lblDteX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.41276M), 2)) + 40;
                    var lblDteY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.01389M), 2));
                    _labelDate.Location = new Point(lblDteX, lblDteY);
                }
                else if (workingArea.Width == 1024 && workingArea.Height == 768)
                {
                    var sizeIcon = new Size(20, 20);
                    int pointY = 10;
                    _buttonPlay.Location = new Point(20, pointY);
                    _buttonPlay.Size = sizeIcon;
                    _buttonPause.Location = new Point(45, pointY);
                    _buttonPause.Size = sizeIcon;
                    _buttonForward.Location = new Point(50, pointY);
                    _buttonForward.Size = sizeIcon;
                    _buttonForward.Visible = false;

                    _buttonSlow.Location = new Point(100, pointY);
                    _buttonSlow.Size = sizeIcon;
                    _labelSpeed.Location = new Point(114, pointY + 2);
                    //_labelSpeed.Size = sizeIcon;
                    _buttonFast.Location = new Point(141, pointY);
                    _buttonFast.Size = sizeIcon;
                    _buttonFast.Visible = true;

                    _buttonRew30Secs.Location = new Point(190, pointY);
                    _buttonRew30Secs.Size = sizeIcon;
                    _buttonFw30Secs.Location = new Point(215, pointY);
                    _buttonFw30Secs.Size = sizeIcon;

                    _buttonCalendar.Location = new Point(280, pointY - 5);
                    _labelDate.Location = new Point(300, pointY);
                    _labelDate.TextAlign = ContentAlignment.MiddleLeft;

                    _labelScale.Location = new Point(410, pointY);
                    _labelScaletime.Location = new Point(440, pointY);
                    _labelScaletime.TextAlign = ContentAlignment.MiddleLeft;

                    _zoomTimeLinePlaybackControl.Location = new Point(520, pointY - 5);

                    _buttonBookmark.Location = new Point(735, pointY);
                    _buttonBookmark.Size = sizeIcon;
                }
                else if (this.Width < 1400)
                {
                    var labelScaletimeWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.05124M), 2));
                    this._labelScaletime.Size = new Size(labelScaletimeWidth, labelScaletimeHeight);
                    var labelScaleX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.5248M), 2));
                    var labelScaleY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0138M), 2));
                    this._labelScale.Location = new Point(labelScaleX, labelScaleY);
                    var labelScaletimeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.5505M), 2));
                    var labelScaletimeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0138M), 2));
                    this._labelScaletime.Location = new Point(labelScaletimeX, labelScaletimeY);
                    var buttonCalendarX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.4099M), 2));
                    var buttonCalendarY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0109M), 2));
                    this._buttonCalendar.Location = new Point(buttonCalendarX, buttonCalendarY);
                    var LabelDateX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.4319M), 2));
                    var LabelDateY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0109M), 2));
                    this._labelDate.Location = new Point(LabelDateX, LabelDateY);
                    var zmTimePybckCtrX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.62252M), 2));
                    var zmTimePybckCtrY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.00651M), 2));
                    _zoomTimeLinePlaybackControl.Location = new Point(zmTimePybckCtrX, zmTimePybckCtrY);
                }
                else if (this.Width > 2000)
                {
                    var lblScaleX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.54688M), 2));
                    var lblScaleY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.01389M), 2));
                    _labelScale.Location = new Point(lblScaleX, lblScaleY);
                    var lblScaleTimeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.5625M), 2));
                    var lblScaleTimeY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.01389M), 2));
                    _labelScaletime.Location = new Point(lblScaleTimeX, lblScaleTimeY);
                    var zmTimePybckCtrX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.65104M), 2));
                    var zmTimePybckCtrY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.01204M), 2));
                    _zoomTimeLinePlaybackControl.Location = new Point(zmTimePybckCtrX, zmTimePybckCtrY);
                    var btnCldrX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.41276M), 2));
                    var btnCldrY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.01019M), 2));
                    _buttonCalendar.Location = new Point(btnCldrX, btnCldrY);
                    var lblDteX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.41276M), 2)) + 40;
                    var lblDteY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.01389M), 2));
                    _labelDate.Location = new Point(lblDteX, lblDteY);
                }

                _resizeLoad = true;
            }
        }
        public void SetCalendarListRecording(List<TimelineDTO> list)
        {
            this._buttonCalendar.SetCalendarListRecording(list);
        }

        private void ButtonCalendar_DateTimeChange(object sender, Dictionary<string, DateTime> e)
        {
            e.TryGetValue("date", out DateTime date);
            e.TryGetValue("time", out DateTime time);
            if (date != null && time != null)
            {
                var hours = TimeSpan.Parse(time.ToString("HH:mm:ss", CultureInfo.InvariantCulture));

                _labelDate.Text = date.ToString("dd/MM/yyyy");
                _labelTime.Text = time.ToString("HH:mm:ss");
                OnDateChanged?.Invoke(DateTime.ParseExact(_labelDate.Text + "_" + _labelTime.Text, "dd/MM/yyyy_HH:mm:ss", CultureInfo.InvariantCulture));
            }
        }

        private void ButtonScaleTimeline_Click(string name, bool state)
        {
            var scale = PlayScaleTimeLine.Normal;
            switch (name)
            {
                case Constants.ZOOM_TIMELINE_NORMAL:
                    scale = PlayScaleTimeLine.Normal;
                    break;
                case Constants.ZOOM_TIMELINE_15:
                    scale = PlayScaleTimeLine.m15;
                    break;
                case Constants.ZOOM_TIMELINE_10:
                    scale = PlayScaleTimeLine.m10;
                    break;
                case Constants.ZOOM_TIMELINE_5:
                    scale = PlayScaleTimeLine.m5;
                    break;
                default:
                    break;
            }

            OnButtonScaleTimeLineClicked?.Invoke(scale);
        }

        public void SelectItemScale(string name)
        {
            _zoomTimeLinePlaybackControl.SelecteItem(name);
        }

        public void EnabledTimelineScale(bool enabled)
        {
            _zoomTimeLinePlaybackControl.EnabledTimelineScale(enabled);
        }

        public void VisibleTimelineScale(bool visible)
        {
            _zoomTimeLinePlaybackControl.VisibleTimelineScale(visible);
            _labelScale.Visible = visible;
            _labelScaletime.Visible = visible;
        }
        public void speedChanged(double speed)
        {
            currentSpeed = speed;
            OnPlaySpeedChanged?.Invoke(speed);
            ShowPlaySpeed();
        }

        public void ToggleListRecording(bool enabled)
        {
            _buttonCalendar.ToggleListRecording(enabled);
        }
    }
}
