using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Shared
{
    public partial class CalendarControl : PopedCotainer
    {
        private DateTime _date = DateTime.Now;
        private DateTime _time = DateTime.Now;
        private bool _hideProgressRecords = false;
        private bool _isVisualSearch = false;
        private bool _isAlarms = false;

        public event EventHandler DateTimeClick;
        public event EventHandler DateTimeChange;
        public event EventHandler DateTimeClose;

        public CalendarControl(bool timePickerVisible = true)
        {
            InitializeComponent();

            _isVisualSearch = timePickerVisible;
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_HEADER_BACKGROUND);
            this._monthCalendar.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
            this._timePicker.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);

            this._monthCalendar.DateSelected += MonthCalendar_DateSelected;
            this._timePicker.ValueChanged += TimePicker_ValueChanged;
            this._buttonOK.Click += ButtonOK_Click;
            this._monthCalendar.MaxDate = DateTime.UtcNow.AddDays(1);
            this._timePicker.Visible = timePickerVisible;
            this.Size = timePickerVisible == true ? new Size(250, 280) : new Size(250, 280);
            this.Location = new Point(136, 80);
            this._listBoxRecords.Items.Clear();
            if (!_isVisualSearch) Resize();
        }

        public DateTime Date
        {
            get => this._date;
            set
            {
                this._date = value;
                this._monthCalendar.SetDate(this._date);
            }
        }

        public DateTime Hour
        {
            get => this._time;
            set
            {
                this._time = value;
                this._timePicker.Value = this._time.Year == 1 ? DateTime.Now : this._time;
            }
        }

        private void TimePicker_ValueChanged(object sender, EventArgs e)
        {
            _time = _timePicker.Value;
            _date = _monthCalendar.SelectionRange.Start;
        }

        private void MonthCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            _date = _monthCalendar.SelectionRange.Start;
            DateTimeChange?.Invoke(sender, e);
            this._monthCalendar.SetDate(this._date);
            _listBoxRecords.Items.Clear();

            if (_isAlarms)
            {
                _progressBar.Visible = false;
            }
            else if (_isVisualSearch)
            {
                ShowProgressBar();
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            _time = _timePicker.Value;
            _date = _monthCalendar.SelectionRange.Start;
            DateTimeClick.Invoke(sender, e);
            ButtonClose_Click(sender, e);
        }

        public void SetListRecording(List<TimelineDTO> list)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    SetListRecording(list);
                });
                return;
            }

            this.CloseProgressBar();
            Logger.Log("calendarControl SetListRecording: ", LogPriority.Information);
            if (list != null && list.Count > 0)
            {
                var dateTimeStart = DateTimeOffset.Parse(list[0].StartTime, null);
                DateTime dateStart = dateTimeStart.DateTime;
                this._labelListado.Text = Resources.CalendarRecordingList + "   " + _monthCalendar.SelectionRange.Start.ToString("yyyy-MM-dd");
                foreach (var timeLine in list)
                {

                    var dateTimeOffsetStart = DateTimeOffset.Parse(timeLine.StartTime, null);
                    DateTime start = dateTimeOffsetStart.DateTime;
                    var dateTimeOffsetEnd = DateTimeOffset.Parse(timeLine.EndTime, null);
                    DateTime end = dateTimeOffsetEnd.DateTime;
                    var item = start.ToString("hh:mm:ss") + (start.Hour < 12 ? " AM" : " PM") +
                               " - " +
                               end.ToString("hh:mm:ss") + (end.Hour < 12 ? " AM" : " PM");

                    this._listBoxRecords.Items.Add(item);
                }

                this._monthCalendar.Location = new Point(9, 242);
                this._timePicker.Location = new Point(136, 413);
                this._panelClear.Location = new Point(41, 382);
                this._buttonOK.Location = new Point(80, -1);
                this._listBoxRecords.Size = new Size(227, 199);
                this._labelInicio.Location = new Point(10, 418);
                this._labelInicio.Text = Resources.CalendarStartLabel;
                this.Size = new Size(250, 450);
            }
            else
            {
                if (!_isVisualSearch)
                {
                    Resize();
                }
                if (_hideProgressRecords)
                {
                    Resize_HideProgressRecords();
                }
                else
                {
                    this._monthCalendar.Location = new Point(9, 71);
                    this._timePicker.Location = new Point(136, 241);
                    this._panelClear.Location = new Point(41, 212);
                    this._buttonOK.Location = new Point(80, -1);
                    this._listBoxRecords.Size = new Size(227, 30);
                    this._labelInicio.Location = new Point(10, 246);
                    this._listBoxRecords.Items.Add("No hay grabaciones");
                    this.Size = new Size(250, 280);
                }

            }
        }

        private void ListBoxRecords_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_listBoxRecords.SelectedIndex == -1) return;

            var selectedItem = _listBoxRecords.Items[_listBoxRecords.SelectedIndex]?.ToString();
            if (string.IsNullOrEmpty(selectedItem)) return;

            selectedItem = selectedItem.Replace(" - ", "_");
            var dates = selectedItem.Split('_');

            if (dates.Length < 1) return;

            if (DateTimeOffset.TryParse(dates[0], null, System.Globalization.DateTimeStyles.None, out var dateTimeOffsetStart))
            {
                _time = dateTimeOffsetStart.DateTime;
                //_date = dateTimeOffsetStart.DateTime;
                DateTimeClick?.Invoke(sender, e);
            }
        }

        public void SetListRecordingLoading()
        {
            if (!_isAlarms)
            {
                if (_isVisualSearch)
                {
                    this._listBoxRecords.Items.Clear();
                    this.ShowProgressBar();

                }
                else
                {
                    _listBoxRecords.Visible = false;
                    _labelListado.Visible = false;
                    this.Controls.Remove(_listBoxRecords);

                }
            }
            else
            {
                this._progressBar.Visible = false;
            }
        }

        private new void Resize()
        {
            this._monthCalendar.Location = new Point(0, 8);
            this._panelClear.Location = new Point(35, 147);
            this._timePicker.Location = new Point(133, 165);
            this._labelInicio.Visible = false;
            this._labelInicio.Location = new Point(3, 171);
            this.Size = new Size(249, 179);
        }

        private void ProgressBarTimer_Tick()
        {
            this._progressBar.Value += 10;

            if (this._progressBar.Value == 100)
            {
                this._progressBar.Value = 10;
            }
        }

        private void ShowProgressBar()
        {
            this._listBoxRecords.Visible = false;
            this._progressBar.Visible = true;

            this._progressBar.Value = 0;
            var timer = new System.Threading.Timer(
            e => ProgressBarTimer_Tick(),
            null,
            TimeSpan.Zero,
            TimeSpan.FromSeconds(1));
        }

        private void CloseProgressBar()
        {
            this._progressBar.Value = 100;
            this._listBoxRecords.Items.Clear();
            this._listBoxRecords.Visible = true;
            this._progressBar.Visible = false;
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            DateTimeClose.Invoke(sender, e);
        }

        public void ListRecordingOff()
        {
            this._monthCalendar.Location = new Point(9, 10);
            this._timePicker.Location = new Point(136, 183);
            this._panelClear.Location = new Point(41, 151);
            this._buttonOK.Location = new Point(80, -1);
            this._listBoxRecords.Size = new Size(227, 30);
            this._labelInicio.Location = new Point(10, 186);
            this.Size = new Size(250, 220);
            this._isAlarms = true;
        }

        public void HideProgressBar()
        {
            this._listBoxRecords.Visible = false;
            this._progressBar.Visible = false;
            this._progressBar.Value = 0;
            this.Controls.Remove(this._listBoxRecords);
            this.Controls.Remove(this._progressBar);
            this.Controls.Remove(this._labelListado);
            this._hideProgressRecords = true;
            Resize_HideProgressRecords();
        }
        
        private void Resize_HideProgressRecords()
        {
            this._monthCalendar.Location = new Point(0, 8);
            this._panelClear.Location = new Point(35, 147);
            this._timePicker.Location = new Point(133, 175);
            this._labelInicio.Location = new Point(3, 178);
            this.Size = new Size(250, 205);
        }

        public void ToggleListRecording(bool enabled)
        {
            _listBoxRecords.Enabled = enabled;
            _buttonOK.Enabled = enabled;
        }
    }
}
