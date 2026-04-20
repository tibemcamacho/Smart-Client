using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.UserControls.PlaybackControls;
using Elipgo.SmartClient.ViewModels;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Elipgo.SmartClient.UserControls.PlaybackControls.TimeLineControl;

namespace Elipgo.SmartClient.UserControls.InstantPlayer
{
    public partial class InstantPlayerControl : UserControl, IDisposable
    {
        private int _recorderSelected = 0;
        private CameraDTO _camera = new CameraDTO();
        private List<RecorderDTO> _recorders = new List<RecorderDTO>();
        private DateTime _timeAction = DateTime.Now;
        private DateTime? _endTimeAction = null;
        private bool _painted;
        private bool _autoCloseForm = false;
        private readonly IDriverFactory _driverFactory = Locator.Current.GetService<IDriverFactory>();
        private BookmarkViewModel _bookmarkViewModel = Locator.Current.GetService<BookmarkViewModel>();
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        public event CalendarChangedEventHandler OnDateChanged;
        private SidebarElementDTO _selectSidebarElementDTO;

        public event ButtonScaleTimeLineClicked OnButtonScaleTimeLineClicked;
        private PlaybackViewModel _viewModel;
        private Control _driver = new Control();
        private DateTime _startDateTimeLine;
        private DateTime _endDateTimeLine;
        private bool _secondScale = false;
        private DateTime _temporalStart;
        private DateTime _temporalEnd;
        private TimeSpan _durationInVault;
        private bool _isVault = false;
        private double _totalDurationInMinutes;
        private double _halfDuration;
        private double _thirdDuration;
        private double _sixthDuration;
        private int _currentSliderValue;
        private int _sliderMaxValue;
        private IDriverInstantPlayback _currentdriverControl;
        private const int _sliderMaxMinutes = 360;
        private double _sliderMaxSeconds = 21600;
        private bool _isButtonClicked = false;
        private double _previewMaxSliderValue = 21600;
        private PlayScaleTimeLine _previewScale = PlayScaleTimeLine.Normal;
        private DateTime _selectedDateTime;
        private double _currentDurationMinutes = 360;
        private FormWindowState _previewWindowsState;

        public bool IsPlaying { get; set; }
        private readonly IAlarmService _alarmService = Locator.Current.GetService<IAlarmService>();
        public PlaybackViewModel ViewModel
        {
            get => this._viewModel;
            set
            {
                this._viewModel = value;
                //this._bookmarkViewModel.Catalog = _viewModel.Catalog;
                this._bookmarkViewModel.Token = _viewModel.MainView.UserToken;
            }
        }

        public event EventHandler OnFormClosed;

        public InstantPlayerControl()
        {
            InitializeComponent();

            if (Screen.AllScreens.Length >= 1 && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                if (main.Width <= 1366)
                {
                    _labelElementName.Font = FontHelper.Get(FontSizes.Small_4, FontName.ROBOTO_REGULAR);
                    _labelGroupName.Font = FontHelper.Get(FontSizes.Small_2, FontName.ROBOTO_REGULAR);
                    _dropDownSelectRecorder.Font = FontHelper.Get(FontSizes.Small_4, FontName.ROBOTO_REGULAR);
                    _dropDownSelectRecorder.Location = new Point(450, _dropDownSelectRecorder.Location.Y);
                    _bunifuSeparator.Location = new Point(600, _bunifuSeparator.Location.Y);

                    int widthtext = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.120M), 2));
                    this._labelElementName.Location = new Point(73, 15);
                    this._labelElementName.Size = new Size(widthtext, 30);
                    this._labelElementName.TextAlign = ContentAlignment.BottomLeft;
                    this._labelGroupName.Width = widthtext;
                }
                else
                {
                    _labelElementName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                    _labelGroupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                    _dropDownSelectRecorder.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                }

                var SelectRecorderWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1078M), 2));
                var SelectRecorderHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0231M), 2));
                _dropDownSelectRecorder.Size = new Size(SelectRecorderWidth, SelectRecorderHeight);
            }

            _pictureBoxIconElement.Image = FileResources.siderbar_icon_camara;
            this.Paint += InstantPlayerControl_Paint;
            _labelDate.Click += LabelDate_Click;

            _addBookmarkControl._isInstantPlayBack = true;
            _addBookmarkControl._widthPanel = _panelVideo.Width;
            _addBookmarkControl._heightPanel = _panelVideo.Height;
            _addBookmarkControl.ButtonCancelClicked += AddBookmarkControl_ButtonCancelClicked;
            _addBookmarkControl.ButtonOkClicked += AddBookmarkControl_ButtonOkClicked;
            _zoomTimeLinePlaybackControl.ItemSelectedClicked += ButtonScaleTimeline_click;
            OnButtonScaleTimeLineClicked += ScaleTimeLine_OnSetScale;
            this.Resize += InstantPlayerControl_Resize;
            _addBookmarkControl.Hide();

        }

        private void ButtonScaleTimeline_click(string name, bool state)
        {

            if (_isButtonClicked)
            {
                return;
            }

            _isButtonClicked = true;

            try
            {
                if (!_isVault)
                {
                    PlayScaleTimeLine scale = PlayScaleTimeLine.Normal;
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
                else
                {
                    PlayScaleTimeLine scale = PlayScaleTimeLine.x1;
                    switch (name)
                    {
                        case "x1":
                            scale = PlayScaleTimeLine.x1;
                            break;
                        case "x1/2":
                            scale = PlayScaleTimeLine.x1_2;
                            break;
                        case "x1/3":
                            scale = PlayScaleTimeLine.x1_3;
                            break;
                        case "x1/6":
                            scale = PlayScaleTimeLine.x1_6;
                            break;
                        default:
                            break;
                    }

                    OnButtonScaleTimeLineClicked?.Invoke(scale);
                }
            }
            finally
            {
                Task.Delay(1000).ContinueWith(_ => _isButtonClicked = false);//Buscar Alternativa?
            }
        }

        private async void InstantPlayerControl_Resize(object sender, EventArgs e)
        {
            CloseCalendarsOpen();

            if (_addBookmarkControl.Visible)
            {
                _addBookmarkControl.Width = _panelVideo.Width;
                _addBookmarkControl._widthPanel = _panelVideo.Width;
                _addBookmarkControl._heightPanel = _panelVideo.Height;
                if (this.ParentForm != null)
                {
                    _addBookmarkControl.ResizeBookMarkControls(true, this.ParentForm.WindowState);
                }
                return;
            }

            var screen = Screen.AllScreens.FirstOrDefault(s => s.WorkingArea.Contains(Cursor.Position));
            if (screen == null) return;
            var main = screen.Bounds;
            var panelTmlY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.7731M), 2));
            if (main.Width >= 1400 && main.Width < 1700)
            {
                panelTmlY = panelTmlY - 25;
            }
            else if (main.Width >= 1300 && main.Width < 1400)
            {
                panelTmlY = panelTmlY - 45;
            }

            this._panelTimeLine.Location = new Point(0, panelTmlY);

            var panelTmlWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.9720M), 2));
            var panelTmlHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0286M), 2));
            this._panelTimeLine.Size = new Size(panelTmlWidth, panelTmlHeight);

            if (this.ParentForm != null && this.ParentForm.WindowState == FormWindowState.Maximized)
            {
                if (main.Width >= 1366 && main.Width < 1400)
                {
                    panelTmlY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.7431M), 2));
                    this._panelTimeLine.Location = new Point(0, panelTmlY);
                }
                this._panelTimeLine.Controls.Add(_timeLineControl);
                this._panelVideo.Controls.Add(_panelTimeLine);
                this.Controls.Add(_labelScale);

                var labelScaleWidt = 30;
                var labelScaleHeight = 15;

                var labelScaleX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.28658M), 2));
                var labelScaleY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.5145M), 2));

                var bookmarkControlx = 0;
                var bookmarkControlY = 780;

                var panelVideoWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.9793M), 2));
                var panelVideoHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.4869M), 2));
                if (main.Width == 1366 && main.Height == 768)
                {
                    panelVideoWidth = panelVideoWidth - 20;
                    panelVideoHeight = panelVideoHeight - 50;
                }
                else if (main.Width == 1024 && main.Height == 768)
                {

                    panelVideoWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.9480M), 2));
                    panelVideoHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.5969M), 2)) + 36;


                    this._panelTimeLine.Controls.Clear();
                    this._panelTimeLine.Location = new Point(1, panelTmlY - 25);
                    this._panelTimeLine.Height = 38;
                    this._panelTimeLine.Width = 975;

                    Console.WriteLine($"this._timeLineControl.Width {this._timeLineControl.Width}");
                    this._timeLineControl.Width = 970;
                    this._panelTimeLine.Controls.Add(this._timeLineControl);
                    Console.WriteLine($"this._timeLineControl.Width add {this._timeLineControl.Width}");

                    bookmarkControlY = 500;
                }
                else if (main.Width >= 1600)
                {
                    this._panelTimeLine.Location = new Point(5, panelTmlY + 7);
                }

                this._panelVideo.Size = new Size(panelVideoWidth, panelVideoHeight);
                this._labelScale.Size = new Size(labelScaleWidt, labelScaleHeight);
                this._labelScale.Location = new Point(labelScaleX, labelScaleY);
                this._panelTimeLine.BringToFront();
                this._labelScale.Visible = true;
                this._timeLineControl.BringToFront();
                _addBookmarkControl.Location = new Point(bookmarkControlx, bookmarkControlY);
                _addBookmarkControl.BringToFront();

                if (_isVault)
                {
                    _startDateTimeLine = _timeAction;
                    _endDateTimeLine = (DateTime)_endTimeAction;
                    _durationInVault = _endDateTimeLine - _startDateTimeLine;
                    //double durationInMinutes = _durationInVault.TotalMinutes;
                }
                else
                {
                    _startDateTimeLine = _timeAction.AddMinutes(-1 * _sliderMaxMinutes);
                    _endDateTimeLine = _timeAction;
                }

                TimeSpan duration = _endDateTimeLine - _startDateTimeLine;

                double totalDurationInMinutes = duration.TotalMinutes;
                double intervalMinutes = totalDurationInMinutes / 25;
                _totalDurationInMinutes = totalDurationInMinutes;
                this._zoomTimeLinePlaybackControl.Visible = duration.TotalHours >= 1;
                this._labelScale.Visible = duration.TotalHours >= 1;


                _halfDuration = totalDurationInMinutes / 2;
                _thirdDuration = totalDurationInMinutes / 3;
                _sixthDuration = totalDurationInMinutes / 6;

                string FormatInterval(double interval)
                {
                    int minutes = (int)Math.Floor(interval);
                    int seconds = (int)Math.Round((interval - minutes) * 60);
                    return $"{minutes}:{seconds:D2}";
                }

                var intervalMinutes1 = totalDurationInMinutes / 24;
                var intervalMinutes1_2 = _halfDuration / 24;
                var intervalMinutes1_3 = _thirdDuration / 24;
                var intervalMinutes1_6 = _sixthDuration / 24;

                string result1 = FormatInterval(intervalMinutes1);
                string result1_2 = FormatInterval(intervalMinutes1_2);
                string result1_3 = FormatInterval(intervalMinutes1_3);
                string result1_6 = FormatInterval(intervalMinutes1_6);

                _zoomTimeLinePlaybackControl.UpdateZoomOptionsBasedOnDuration(_isVault, result1, result1_2, result1_3, result1_6);

                double intervalToPass = _isVault ? intervalMinutes : 30;
                GenerateTimeline(_isVault ? AppName.InstantVault : AppName.InstantPlayback, 24, intervalToPass);
                await SetTimeLineAlarms(_isVault ? AppName.InstantVault : AppName.InstantPlayback, null, 24, intervalToPass);
            }
            else
            {
                var anchoAjustado = this.Width - (this.Width * 0.0716);
                var altoAjustado = this.Height - (this.Height * 0.2019);

                this._panelVideo.Size = new Size((int)anchoAjustado, (int)altoAjustado);
                //this._panelVideo.Size = new Size((int)560, (int)altoAjustado);

                this._panelVideo.Controls.Remove(this._panelTimeLine);
                this._labelScale.Visible = false;
                this._zoomTimeLinePlaybackControl.Visible = false;
                _addBookmarkControl.Location = new Point(0, 260);
            }

            if (this.ParentForm != null && this.ParentForm.WindowState == FormWindowState.Minimized)
            {
                _previewWindowsState = this.ParentForm.WindowState;
            }

            if (_previewWindowsState == FormWindowState.Minimized)
            {
                panelTmlY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.7731M), 2));
                this._panelTimeLine.Location = new Point(0, panelTmlY);

                panelTmlWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.9792M), 2));
                panelTmlHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0286M), 2));
                this._panelTimeLine.Size = new Size(panelTmlWidth, panelTmlHeight);
                this._panelTimeLine.BringToFront();
            }

            if ((main.Width == 1024 && main.Height == 768) && _previewWindowsState == FormWindowState.Minimized)
            {
                panelTmlWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.9792M), 2));
                panelTmlHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0286M), 2));
                this._panelTimeLine.Size = new Size(panelTmlWidth, 40);
                this._panelTimeLine.Location = new Point(0, 572);
            }

            _addBookmarkControl.Width = _panelVideo.Width;
            _addBookmarkControl._widthPanel = _panelVideo.Width;
            _addBookmarkControl._heightPanel = _panelVideo.Height;

            if (this.ParentForm != null)
            {
                _addBookmarkControl.ResizeBookMarkControls(true, this.ParentForm.WindowState);
            }
        }

        private async void AddBookmarkControl_ButtonOkClicked(object sender, EventArgs e)
        {
#pragma warning disable CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception
            string timeStart = _addBookmarkControl.TimeStart.ToString("yyyy/MM/dd HH:mm:ss");
            string timeEnd = _addBookmarkControl.TimeEnd.ToString("yyyy/MM/dd HH:mm:ss");
#pragma warning restore CS1690 // Accessing a member on a field of a marshal-by-reference class may cause a runtime exception

            BookmarkGroupDTO bookmarkGroup = new BookmarkGroupDTO
            {
                UserId = (int)_viewModel.MainView.User.UserId,
                FileName = _addBookmarkControl.BookmarkName,
                Description = _addBookmarkControl.BookmarkName,
                IsDeleted = false,
                Elements = new List<BookmarkGroupElementDTO>
                {
                    new BookmarkGroupElementDTO
                    {
                        DeviceFeatureId = _camera.Id,
                        IsDeleted = false,
                        TimeStart = timeStart,
                        TimeEnd = timeEnd,
                        NvrId = _dropDownSelectRecorder.SelectedValue != null ? (int?)_dropDownSelectRecorder.SelectedValue : null,
                        TypeRecorder = _recorders.Where(r=>r.Id == (int)_dropDownSelectRecorder.SelectedValue).FirstOrDefault()?.RecorderType.ToString()
                    }
                }
            };

            bool res = await _bookmarkViewModel.AddBookmark(bookmarkGroup);
            if (res)
            {
                _notification.Show(Resources.BookmarkCreated, null);
            }

            IDriverInstantPlayback control = this._panelVideo.Controls.OfType<IDriverInstantPlayback>().ToList()[0];
            control.BookmarkState = false;
            _addBookmarkControl.Hide();
            if (_autoCloseForm)
            {
                OnFormClosed?.Invoke(sender, e);
            }
        }

        private void AddBookmarkControl_ButtonCancelClicked(object sender, EventArgs e)
        {
            CancelBookMark();
            if (_autoCloseForm)
            {
                OnFormClosed?.Invoke(sender, e);
            }
        }

        private void CancelBookMark()
        {
            if (_driver is IDriverInstantPlayback instantDriver)
            {
                instantDriver.BookmarkState = false;
            }
            _addBookmarkControl.Hide();
        }

        private void LabelDate_Click(object sender, EventArgs e)
        {
            _buttonCalendar.OpenCalendar();
        }

        public new void Dispose()
        {
            if (this._panelVideo.Controls.Count > 0)
            {
                try
                {

                    if (this._panelVideo.Controls.OfType<IDriverInstantPlayback>().ToList().Count() > 0)
                    {
                        IDriverInstantPlayback control = this._panelVideo.Controls.OfType<IDriverInstantPlayback>().ToList()[0];
                        control.Dispose();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void InstantPlayerControl_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
            {
                return;
            }

            this._labelScale.Image = FileResources.icon_group_scale_time;
            this._painted = true;
        }

        private void LoadDriver(bool isDiagnostic, bool ddlIndexChanged = false)
        {
            Control control = _driverFactory.GetDriverInstantPlayback(_camera, Profile.MainStream, this.GetSelectedRecorderDriver(ddlIndexChanged), this._timeAction, Guid.NewGuid().ToString(), false, isDiagnostic, this._endTimeAction) as Control;
            if (control == null)
            {
                return;
            }

            control.Name = Guid.NewGuid().ToString();
            control.Height = this.Height - 2;
            control.Width = this.Width - 2;
            control.Location = new Point(1, 1);
            control.Visible = true;
            control.Dock = DockStyle.Fill;
            Rectangle main = Screen.PrimaryScreen.Bounds;
            _driver = control;

            _labelDate.Font = main.Width <= 1366 ? FontHelper.Get(FontSizes.Small_4, FontName.ROBOTO_REGULAR) : FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
            if (_isVault)
            {
                var bookmarkBtn = _driver.Controls.Find("ButtonBookmark", true).FirstOrDefault();
                if (bookmarkBtn != null)
                {
                    bookmarkBtn.Visible = false;
                }
            }
            var driverPlayback = _driver as IDriverInstantPlayback;
            driverPlayback.OpenBookmark += InstantPlayerControl_OpenBookmark;
            this._panelVideo.Controls.Add(_driver);
            _buttonCalendar.Date = this._timeAction.Date;
            _buttonCalendar.Hour = this._timeAction;
            _buttonCalendar.DateTimeClick += ButtonCalendar_DateTimeSelected;
            _buttonCalendar.DateTimeChange += ButtonCalendar_DateTimeChange;
            Task.Run(() => { driverPlayback.Play(); });
            this.OnDateChanged += OnGetRecording;
        }

        private void InstantPlayerControl_OpenBookmark(object sender, bool e)
        {
            if (e)
            {
                if (this._panelVideo.Controls.Count > 0)
                {
                    OpenBookMark();
                }
            }
            else
            {
                _addBookmarkControl.Hide();
            }
        }

        private void OpenBookMark()
        {
            _addBookmarkControl.Show();
            _addBookmarkControl.Width = _panelVideo.Width;
            _addBookmarkControl._widthPanel = _panelVideo.Width;
            _addBookmarkControl._heightPanel = _panelVideo.Height;
            _addBookmarkControl.Anchor = ((AnchorStyles.Bottom) | AnchorStyles.Left) | AnchorStyles.Right;
            _addBookmarkControl.ResizeBookMarkControls(true, this.ParentForm.WindowState);
            IDriverInstantPlayback control = this._panelVideo.Controls.OfType<IDriverInstantPlayback>().ToList()[0];
            if (control.ActualTime == DateTime.MinValue)
            {
                control.ActualTime = _timeAction.AddMinutes(_camera.Gmt);
            }
            if (!_isVault)
            {
                _addBookmarkControl.SetTime(_timeAction, _endDateTimeLine, AppName.InstantPlayback);

                if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
                {
                    var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                    if (main.Width > 1600)
                    {
                        _addBookmarkControl.Location = new Point(0, 350);
                    }
                    else if (main.Width >= 1400 && main.Width <= 1600)
                    {
                        _addBookmarkControl.Location = new Point(0, 250);
                    }
                    else
                    {
                        if (this.ParentForm.WindowState != FormWindowState.Maximized)
                        {
                            _addBookmarkControl.Location = new Point(0, 190);
                            _addBookmarkControl.BringToFront();
                        }

                    }
                    if (this.ParentForm.WindowState == FormWindowState.Maximized)
                    {
                        Control bookmarControlContainer = _addBookmarkControl.Parent;

                        if (bookmarControlContainer != null)
                        {
                            int x = 0;
                            int y = bookmarControlContainer.ClientSize.Height - _addBookmarkControl.Height;
                            _addBookmarkControl.Location = new Point(x, y);
                        }

                        _addBookmarkControl.BringToFront();
                    }

                    _currentdriverControl = this._panelVideo.Controls
                           .OfType<IDriverInstantPlayback>()
                           .FirstOrDefault();

                }
            }
            _addBookmarkControl.BringToFront();
        }

        private async void ButtonCalendar_DateTimeSelected(object sender, Dictionary<string, DateTime> e)
        {
            _buttonCalendar.ToggleListRecording(false);
            e.TryGetValue("date", out DateTime date);
            e.TryGetValue("time", out DateTime time);
            if (date != null && time != null)
            {
                _labelDate.Text = date.ToString("dd/MM/yyyy");
                var hours = time.ToString("HH:mm:ss");
                _timeAction = DateTime.ParseExact(_labelDate.Text + "_" + hours, "dd/MM/yyyy_HH:mm:ss", CultureInfo.InvariantCulture);
                var rec = _recorders[_recorderSelected];
                int recorderId = 0;
                if (_camera.Recorders.Exists(r => r.Name == rec.Name))
                {
                    recorderId = rec.Id;
                    _camera.RecorderId = rec.Id;
                }
                await Task.Run(async () =>
                {
                    try
                    {
                        IDriverInstantPlayback control = this._panelVideo.Controls.OfType<IDriverInstantPlayback>().ToList()[0];

                        try
                        {
                            control.SetStartDateTime(_timeAction, false);
                            control.PlayNoAsync();
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("InstantPlayerControl Exception: " + ex.Message, LogPriority.Fatal);
                        }
                        if (_addBookmarkControl.Visible == true)
                        {
                            _addBookmarkControl.SetTime(_timeAction, control.EndTime);
                        }
                        _startDateTimeLine = _timeAction;
                        _endDateTimeLine = _timeAction.AddMinutes(_sliderMaxMinutes);
                        GenerateTimeline(AppName.InstantPlayback, 24, 30);
                        await SetTimeLineAlarms(AppName.InstantPlayback, null, 24, 30);

                    }
                    catch (Exception ex)
                    {
                        Logger.Log("Timeline fetch error: " + ex.ToString(), LogPriority.Fatal);
                    }
                });
                _buttonCalendar.ToggleListRecording(true);
            }
        }

        public void SetCamera(CameraDTO camera, DateTime timeAction, SidebarElementDTO element, bool openBM, List<RecorderDTO> recorders, bool isDiagnostic = false, DateTime? endtimeAction = null)
        {
            _camera = camera;
            _selectSidebarElementDTO = element;
            _timeAction = timeAction;
            if (endtimeAction != null)
            {
                _isVault = true;
                _endTimeAction = endtimeAction;
            }

            this._labelElementName.Text = camera.Name;
            this._labelGroupName.Text = element.GroupName;
            this._labelDate.Text = _timeAction.ToString("dd/MM/yyyy");
            SetRecorders(recorders);
            LoadDriver(isDiagnostic);
            if (openBM)
            {
                _autoCloseForm = true;
                this.OpenBookMark();
            }
            _panelVideo.BringToFront();
        }

        public void SetRecorders(List<RecorderDTO> recorders)
        {
            if (recorders != null && recorders.Count > 0)
            {
                _recorders = recorders;
                _dropDownSelectRecorder.DataSource = new BindingSource(recorders, null);
                _dropDownSelectRecorder.DisplayMember = "Name";
                _dropDownSelectRecorder.ValueMember = "Id";
                if (_camera.RecorderId != 0)
                {
                    _selectSidebarElementDTO.RecorderId = _camera.RecorderId;
                }
                if (!_camera.EdgeEnabled)
                {
                    if (_camera.Recorders != null && _camera.Recorders.Count > 0)
                    {
                        _camera.RecorderId = _camera.Recorders[0].Id;
                    }
                }
                var recorder = _recorders.Where(r => r.Id == (_camera.RecorderId != 0 ? _camera.RecorderId : _camera.Id)).FirstOrDefault();
                if (recorder != null)
                {
                    _recorderSelected = _recorders.IndexOf(recorder);
                }
                _dropDownSelectRecorder.SelectedValue = recorders[_recorderSelected].Id;
                _dropDownSelectRecorder.Visible = true;
                _recorderSelected = 0;
            }

            if (recorders == null || recorders.Count <= 1)
            {
                _dropDownSelectRecorder.Visible = false;
            }
            else
            {
                _dropDownSelectRecorder.SelectedIndexChanged -= SelectRecorder_SelectedIndexChanged;
                _dropDownSelectRecorder.SelectedIndexChanged += SelectRecorder_SelectedIndexChanged;
            }
        }

        private void SelectRecorder_SelectedIndexChanged(object sender, EventArgs e)
        {

            _recorderSelected = this._dropDownSelectRecorder.SelectedIndex;
            var previousdriver = this._panelVideo.Controls.OfType<IDriverInstantPlayback>().FirstOrDefault();
            if (previousdriver != null)
            {
                this._panelVideo.Controls.Remove(previousdriver as Control);
                previousdriver.Dispose();
                _buttonCalendar.DateTimeClick -= ButtonCalendar_DateTimeSelected;
                _buttonCalendar.DateTimeChange -= ButtonCalendar_DateTimeChange;
                this.OnDateChanged -= OnGetRecording;
                var rec = _recorders[_recorderSelected];
                if (_camera.Recorders.Exists(r => r.Name == rec.Name))
                {
                    _camera.RecorderId = rec.Id;
                }

                LoadDriver(false, true);
            }
        }

        private RecorderDTOSmall GetSelectedRecorderDriver(bool ddlIndexChanged)
        {
            if (_recorders.Count == 0)
            {
                return new RecorderDTOSmall();
            }

            var recorder = _recorders[_recorderSelected];
            if (_selectSidebarElementDTO != null && _camera.Recorders.Count > 0 && !ddlIndexChanged)
            {
                _selectSidebarElementDTO.RecorderId = _camera.Recorders[0].Id;
                var record = _camera.Recorders.Where(x => x.Id == _selectSidebarElementDTO.RecorderId).FirstOrDefault();
                if (record != null && recorder.Id == record.Id)
                {
                    return new RecorderDTOSmall()
                    {
                        Driver = record.Driver,
                        Id = record.Id,
                        Name = record.Name,
                        RecorderType = record.RecorderType,
                        Host = record.Host,
                        VideoPort = record.VideoPort,
                        Channel = record.Channel,
                        Username = record.Username,
                        Password = record.Password
                    };
                }
            }


            return new RecorderDTOSmall()
            {
                Driver = recorder.Driver,
                Id = recorder.Id,
                Name = recorder.Name,
                RecorderType = recorder.RecorderType,
                Host = recorder.Host,
                VideoPort = recorder.VideoPort,
                Channel = recorder.Channel,
                Username = recorder.Username,
                Password = recorder.Password

            };
        }

        public void SetInstantPlaybackVault()
        {
            _dropDownSelectRecorder.Visible = false;
            _bunifuSeparator.Visible = false;
            _labelDate.Visible = false;
            _buttonCalendar.Visible = false;
            (_driver as IDriverInstantPlayback).SetStartDateTime(this._timeAction, false, true);
            (_driver as IDriverInstantPlayback).Play();
        }

        public void SetinstantPlaybackVaultMP4(string path, CameraDTO camera, SidebarElementDTO element, DateTime timeAction, DateTime? endtimeAction = null)
        {
            _camera = camera;
            _selectSidebarElementDTO = element;
            _timeAction = timeAction;
            if (endtimeAction != null)
            {
                _isVault = true;
                _endTimeAction = endtimeAction;
            }

            _dropDownSelectRecorder.Visible = false;
            _bunifuSeparator.Visible = false;
            _labelDate.Visible = false;
            _buttonCalendar.Visible = false;
            this._labelElementName.Text = camera.Name;
            this._labelGroupName.Text = element.GroupName;
            this._labelDate.Text = _timeAction.ToString("dd/MM/yyyy");
            var controlMP4 = new PlayMP4.PlayMP4(path, camera, timeAction, endtimeAction);
            controlMP4.Name = Guid.NewGuid().ToString();
            controlMP4.Height = this.Height - 2;
            controlMP4.Width = this.Width - 2;
            controlMP4.Location = new Point(1, 1);
            controlMP4.Visible = true;
            controlMP4.Dock = DockStyle.Fill;

            Rectangle main = Screen.PrimaryScreen.Bounds;
            _labelDate.Font = main.Width <= 1366 ? FontHelper.Get(FontSizes.Small_4, FontName.ROBOTO_REGULAR) : FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);

            this._panelVideo.Controls.Add(controlMP4);

            _buttonCalendar.Date = this._timeAction.Date;
            _buttonCalendar.Hour = this._timeAction;
            _buttonCalendar.DateTimeClick += ButtonCalendar_DateTimeSelected;
            _buttonCalendar.DateTimeChange += ButtonCalendar_DateTimeChange;
            this.OnDateChanged += OnGetRecording;
        }

        private void ButtonCalendar_DateTimeChange(object sender, Dictionary<string, DateTime> e)
        {
            e.TryGetValue("date", out DateTime date);
            e.TryGetValue("time", out DateTime time);
            if (date != null && time != null)
            {
                var hours = TimeSpan.Parse(time.ToString("HH:mm:ss", CultureInfo.InvariantCulture));

                _labelDate.Text = date.ToString("dd/MM/yyyy");
                var hoursString = time.ToString("HH:mm:ss");
                OnDateChanged?.Invoke(DateTime.ParseExact(_labelDate.Text + "_" + hoursString, "dd/MM/yyyy_HH:mm:ss", CultureInfo.InvariantCulture));
            }
        }

        private void OnGetRecording(DateTime dateTime)
        {
            try
            {
                Task.Run(() =>
                {
                    List<TimelineDTO> timeLine = null;
                    if (dateTime != null)
                    {
                        var rec = _recorders[_recorderSelected];
                        int recorderId = 0;
                        if (_camera.Recorders.Exists(r => r.Name == rec.Name))
                        {
                            recorderId = rec.Id;
                            _camera.RecorderId = rec.Id;
                        }

                        try
                        {
                            var startTime = dateTime.ToString("yyyy-MM-ddT00:00:00Z", System.Globalization.CultureInfo.InvariantCulture);
                            var endTime = dateTime.ToString("yyyy-MM-ddT23:59:59Z", System.Globalization.CultureInfo.InvariantCulture);
                            timeLine = ViewModel.GetTimeLineSC(_camera, startTime, endTime, true, recorderId); //selectSidebarElementDTO.RecorderId);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("GetTimeLineSC error: " + ex.Message, LogPriority.Fatal);
                        }
                    }

                    this.SetCalendarListRecording(timeLine);
                });
            }
            catch (Exception ex)
            {
                Logger.Log("OnGetRecording Exception: " + ex.Message, LogPriority.Fatal);
            }
        }


        public void SetCalendarListRecording(List<TimelineDTO> list)
        {
            this._buttonCalendar.SetCalendarListRecording(list);
        }

        public void ListRecordingOff()
        {
            this._buttonCalendar.ListRecordingOff();
        }

        private void GenerateTimeline(AppName appName, int hourLine, double minutesLineInterval, List<AlarmDTO> alarmsTimeLine = null)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    GenerateTimeline(appName, hourLine, minutesLineInterval, alarmsTimeLine);
                });
                return;
            }
            this._timeLineControl.Controls.Clear();
            this._timeLineControl.LoadTimeLineHTML(appName, _startDateTimeLine, _endDateTimeLine, hourLine, minutesLineInterval, false, _totalDurationInMinutes, alarmsTimeLine);
            this._timeLineControl.BringToFront();
        }

        private async void ScaleTimeLine_OnSetScale(PlayScaleTimeLine scale)
        {
            _currentdriverControl = this._panelVideo.Controls
                                   .OfType<IDriverInstantPlayback>()
                                   .FirstOrDefault();

            _currentSliderValue = _currentdriverControl.GetCurrentSliderValue();
            _sliderMaxValue = _currentdriverControl.GetMaxSliderValue();
            _selectedDateTime = _currentdriverControl.GetDateSelected();

            double maxMinutes = _sliderMaxMinutes;
            var sliderMaxSeconds = _sliderMaxSeconds;

            if (scale == 0)
            {
                scale = PlayScaleTimeLine.Normal;
            }


            if (!_isVault)
            {
                _startDateTimeLine = _timeAction.AddMinutes(-1 * maxMinutes);
                _endDateTimeLine = _timeAction;

                switch (scale)
                {
                    case PlayScaleTimeLine.Normal:
                        await UpdateTimelineNormal();
                        break;

                    case PlayScaleTimeLine.m15:
                        await UpdateTimelineM15();
                        break;

                    case PlayScaleTimeLine.m10:

                        if (_previewScale == PlayScaleTimeLine.Normal)
                        {
                            await UpdateTimelineForPlayScaleM10();
                        }
                        else
                        {
                            int totalMinutes = (int)_currentDurationMinutes;

                            int blockDurationMinutes = (int)(_temporalEnd - _temporalStart).TotalMinutes / 3;
                            _currentDurationMinutes /= 3;
                            _sliderMaxSeconds /= 3;
                            DateTime firstBlockEnd = _temporalStart.AddMinutes(_currentDurationMinutes);
                            DateTime secondBlockEnd = _temporalStart.AddMinutes(2 * _currentDurationMinutes);

                            if (_selectedDateTime < firstBlockEnd)
                            {
                                _startDateTimeLine = firstBlockEnd.AddMinutes(-1 * totalMinutes);
                                _endDateTimeLine = firstBlockEnd;
                                ScaleSliderValue(_sliderMaxSeconds, PlayScaleTimeLine.m10, 1, 3, _secondScale);
                            }
                            else if (_selectedDateTime < secondBlockEnd)
                            {
                                _startDateTimeLine = firstBlockEnd;
                                _endDateTimeLine = secondBlockEnd;
                                ScaleSliderValue(_sliderMaxSeconds, PlayScaleTimeLine.m10, 2, 3, _secondScale);
                            }
                            else
                            {
                                _startDateTimeLine = secondBlockEnd;
                                ScaleSliderValue(_sliderMaxSeconds, PlayScaleTimeLine.m10, 3, 3, _secondScale);
                            }

                            _temporalStart = _startDateTimeLine;
                            _temporalEnd = _endDateTimeLine;
                            GenerateTimeline(AppName.InstantPlayback, 6, 10);
                            await SetTimeLineAlarms(AppName.InstantPlayback, null, 24, 10);
                        }

                        _previewScale = PlayScaleTimeLine.m10;
                        break;

                    case PlayScaleTimeLine.m5:

                        if (_previewScale == PlayScaleTimeLine.Normal)
                        {
                            await UpdateTimelineForPlayScaleM5();
                        }
                        else
                        {
                            _startDateTimeLine = _temporalStart;
                            _endDateTimeLine = _temporalEnd;
                            GenerateTimeline(AppName.InstantPlayback, 12, 5);
                            await SetTimeLineAlarms(AppName.InstantPlayback, null, 24, 5);
                            _previewScale = PlayScaleTimeLine.m5;
                        }

                        break;

                    default:
                        break;
                }


                _previewScale = scale;
                _previewMaxSliderValue = sliderMaxSeconds;
            }
            else
            {
                _isVault = true;
                double totalDuracionInVault = _totalDurationInMinutes;
                double durationInSeconds = _durationInVault.TotalSeconds;
                double intervalMinutes;
                double totalDurationInMinutes = (_endDateTimeLine - _startDateTimeLine).TotalMinutes;

                switch (scale)
                {
                    case PlayScaleTimeLine.x1:
                        _startDateTimeLine = _timeAction;
                        _endDateTimeLine = _startDateTimeLine.AddMinutes(_totalDurationInMinutes);
                        intervalMinutes = totalDurationInMinutes / 25;
                        GenerateTimeline(AppName.InstantVault, 24, intervalMinutes);
                        ScaleSliderValue(durationInSeconds, PlayScaleTimeLine.Normal, 1, 1);
                        await SetTimeLineAlarms(AppName.InstantVault, null, 24, intervalMinutes);
                        break;

                    case PlayScaleTimeLine.x1_2:
                        totalDuracionInVault = _totalDurationInMinutes / 2;
                        DateTime halfDurationTime = _startDateTimeLine.AddMinutes(totalDuracionInVault);
                        if (_selectedDateTime < halfDurationTime)
                        {
                            _endDateTimeLine = _endDateTimeLine.AddMinutes(-totalDuracionInVault);
                            ScaleSliderValue(durationInSeconds / 2, PlayScaleTimeLine.m15, 1, 2);
                        }
                        else
                        {
                            _startDateTimeLine = _endDateTimeLine.AddMinutes(-totalDuracionInVault);
                            ScaleSliderValue(durationInSeconds / 2, PlayScaleTimeLine.m15, 2, 2);
                        }

                        intervalMinutes = (totalDurationInMinutes / 2) / 25;
                        GenerateTimeline(AppName.InstantVault, 24, intervalMinutes);
                        await SetTimeLineAlarms(AppName.InstantVault, null, 24, intervalMinutes);
                        break;

                    case PlayScaleTimeLine.x1_3:
                        totalDuracionInVault = _totalDurationInMinutes / 3;
                        DateTime firstThirdTime = _startDateTimeLine.AddMinutes(totalDuracionInVault);
                        DateTime secondThirdTime = _startDateTimeLine.AddMinutes(2 * totalDuracionInVault);

                        if (_selectedDateTime < firstThirdTime)
                        {
                            _endDateTimeLine = _endDateTimeLine.AddMinutes(-2 * totalDuracionInVault);
                            ScaleSliderValue(durationInSeconds / 3, PlayScaleTimeLine.m10, 1, 1);
                        }
                        else if (_selectedDateTime < secondThirdTime)
                        {
                            _endDateTimeLine = _endDateTimeLine.AddMinutes(-1 * totalDuracionInVault);
                            _startDateTimeLine = _endDateTimeLine.AddMinutes(-1 * totalDuracionInVault);

                            ScaleSliderValue(durationInSeconds / 3, PlayScaleTimeLine.m10, 2, 1);
                        }
                        else
                        {
                            _startDateTimeLine = _endDateTimeLine.AddMinutes(-totalDuracionInVault);
                            ScaleSliderValue(durationInSeconds / 3, PlayScaleTimeLine.m10, 3, 1);
                        }

                        intervalMinutes = (totalDurationInMinutes / 3) / 25;
                        GenerateTimeline(AppName.InstantVault, 24, intervalMinutes);
                        await SetTimeLineAlarms(AppName.InstantVault, null, 24, intervalMinutes);
                        break;

                    case PlayScaleTimeLine.x1_6:
                        totalDuracionInVault = _totalDurationInMinutes / 6;
                        DateTime[] timeParts = new DateTime[6];
                        for (int i = 0; i < 6; i++)
                        {
                            timeParts[i] = _startDateTimeLine.AddMinutes((i + 1) * totalDuracionInVault);
                        }

                        for (int i = 0; i < 6; i++)
                        {
                            if (_selectedDateTime < timeParts[i])
                            {
                                _startDateTimeLine = _endDateTimeLine.AddMinutes(-(totalDuracionInVault * (6 - i)));
                                _endDateTimeLine = _endDateTimeLine.AddMinutes(-(totalDuracionInVault * (5 - i)));
                                ScaleSliderValue(durationInSeconds / 6, PlayScaleTimeLine.m5, i + 1, 1);
                                break;
                            }
                        }

                        intervalMinutes = (totalDuracionInVault / 6) / 25;
                        GenerateTimeline(AppName.InstantVault, 24, intervalMinutes);
                        await SetTimeLineAlarms(AppName.InstantVault, null, 24, intervalMinutes);
                        break;

                    default:
                        break;
                }
            }
            _previewScale = scale;
            _previewMaxSliderValue = sliderMaxSeconds;
        }

        private void ScaleSliderValue(double maxMinutes, PlayScaleTimeLine scale, int numberBlock, int totalBlocksNumber, bool secondScale = false)
        {

            if (_currentdriverControl == null)
            {
                return;
            }
            _currentdriverControl.UpdateSlider(maxMinutes, scale, secondScale, _startDateTimeLine, _endDateTimeLine, numberBlock, totalBlocksNumber);
        }

        public async Task UpdateTimelineNormal()
        {
            _currentDurationMinutes = _currentDurationMinutes != 360 ? 360 : _currentDurationMinutes;
            _sliderMaxSeconds = 21600;
            _endDateTimeLine = _timeAction;
            _startDateTimeLine = _timeAction.AddMinutes(-_currentDurationMinutes);
            ScaleSliderValue(_sliderMaxSeconds, PlayScaleTimeLine.Normal, 1, 1);
            GenerateTimeline(AppName.InstantPlayback, 12, 30);
            await SetTimeLineAlarms(AppName.InstantPlayback, null, 24, 30);
            _previewScale = PlayScaleTimeLine.Normal;
        }

        public async Task UpdateTimelineM15()
        {
            _currentDurationMinutes /= 2;
            _sliderMaxSeconds /= 2;

            DateTime halfDurationTime = _startDateTimeLine.AddMinutes(_currentDurationMinutes);
            _secondScale = !(_selectedDateTime < halfDurationTime);
            _startDateTimeLine = _secondScale ? _timeAction.AddMinutes(-_currentDurationMinutes) : _startDateTimeLine;
            _endDateTimeLine = _secondScale ? _endDateTimeLine : _timeAction.AddMinutes(-_currentDurationMinutes);
            ScaleSliderValue(_sliderMaxSeconds, PlayScaleTimeLine.m15, _secondScale ? 2 : 1, 2);

            _temporalStart = _startDateTimeLine;
            _temporalEnd = _endDateTimeLine;
            GenerateTimeline(AppName.InstantPlayback, 12, 15);
            await SetTimeLineAlarms(AppName.InstantPlayback, null, 24, 15);
            _previewScale = PlayScaleTimeLine.m15;
        }

        private async Task UpdateTimelineForPlayScaleM10()
        {
            _currentDurationMinutes /= 3;
            _sliderMaxSeconds /= 3;

            DateTime firstThirdTime = _startDateTimeLine.AddMinutes(_currentDurationMinutes);
            DateTime secondThirdTime = _startDateTimeLine.AddMinutes(2 * _currentDurationMinutes);

            if (_selectedDateTime < firstThirdTime)
            {
                _endDateTimeLine = _timeAction.AddMinutes(-_currentDurationMinutes);
                ScaleSliderValue(_sliderMaxSeconds, PlayScaleTimeLine.m10, 1, 3);
            }
            else if (_selectedDateTime < secondThirdTime)
            {
                _startDateTimeLine = _timeAction.AddMinutes(-2 * _currentDurationMinutes);
                _endDateTimeLine = _timeAction.AddMinutes(-_currentDurationMinutes);
                ScaleSliderValue(_sliderMaxSeconds, PlayScaleTimeLine.m10, 2, 3);
            }
            else
            {
                _startDateTimeLine = _timeAction.AddMinutes(-_currentDurationMinutes);
                ScaleSliderValue(_sliderMaxSeconds, PlayScaleTimeLine.m10, 3, 3);
            }

            GenerateTimeline(AppName.InstantPlayback, 12, 10);
            await SetTimeLineAlarms(AppName.InstantPlayback, null, 24, 10);
        }

        private async Task UpdateTimelineForPlayScaleM5()
        {
            _currentDurationMinutes /= 6;
            _sliderMaxSeconds /= 6;

            DateTime[] timeParts = new DateTime[6];
            for (int i = 0; i < 6; i++)
            {
                timeParts[i] = _startDateTimeLine.AddMinutes((i + 1) * _currentDurationMinutes);
                if (_selectedDateTime < timeParts[i])
                {
                    _startDateTimeLine = _timeAction.AddMinutes(-(_currentDurationMinutes * (6 - i)));
                    _endDateTimeLine = _timeAction.AddMinutes(-(_currentDurationMinutes * (5 - i)));
                    ScaleSliderValue(_sliderMaxSeconds, PlayScaleTimeLine.m5, i + 1, 1);
                    break;
                }
            }

            GenerateTimeline(AppName.InstantPlayback, 12, 5);
            await SetTimeLineAlarms(AppName.InstantPlayback, null, 24, 5);
            _previewScale = PlayScaleTimeLine.m5;
        }
        private async Task SetTimeLineAlarms(AppName appName, DateTime? dateSelected = null, int hours = 24, double minutes = 30)
        {
            var driver = (_driver as IDriverInstantPlayback);
            if (driver == null)
            {
                return;
            }

            if (!dateSelected.HasValue)
            {
                dateSelected = driver.ActualTime != default ? driver.ActualTime : driver.GetDateSelected().ToLocalTime();
            }

            DateTime startDate = dateSelected.Value.Date;
            DateTime endDate = dateSelected.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            var alarmsReport = await _alarmService.GetFiltered(driver.Camera.Id, "TODAS", 0, startDate, endDate, 100, 0, "0", ViewModel.MainView.UserToken);
            var reports = alarmsReport?.Report != null ? alarmsReport.Report.ToList() : new List<AlarmDTO> { };
            GenerateTimeline(appName, hours, minutes, reports);
        }

        private void CloseCalendarsOpen()
        {
            var calendarPanels = this.Controls.Find("panelSelectDate", true);
            foreach (Control panel in calendarPanels)
            {
                if (panel.Parent != null)
                {
                    panel.Parent.Controls.Remove(panel);
                }
                panel.Dispose();
            }
        }
    }
}
