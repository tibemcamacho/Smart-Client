using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.UserControls.Shared;
using Splat;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using static Elipgo.SmartClient.UserControls.PlaybackControls.TimeLineControl;

namespace Elipgo.SmartClient.UserControls.Bookmark
{
    public partial class AddBookmarkControl : UserControl
    {
        public DateTime TimeStart = DateTime.Now;
        public DateTime TimeOrigin = DateTime.Now;
        public DateTime TimeEnd = DateTime.MinValue;
        public DateTime Duration = DateTime.MinValue;
        public string BookmarkName = "";
        public int _widthPanel = 0;
        public int _heightPanel = 0;

        public event EventHandler ButtonCancelClicked;
        public event EventHandler ButtonOkClicked;
        private readonly ISmartNotification _notification = Locator.Current.GetService<ISmartNotification>();
        private AppName _appActive = AppName.Playback;
        private Point _panelBookmarkLocation;
        public bool _isInstantPlayBack = false;
        private FormWindowState _windowState;
        private MouseHook _mouseHook;

        public AddBookmarkControl(DateTime timeStart, Point location = default)
        {
            InitializeComponent();
            _panelBookmarkLocation = location;
            _labelName.Text = Resources.Name;
            _labelStart.Text = Resources.Start;
            _labelEnd.Text = Resources.BookmarkHoraFin;
            _labelBookmark.Text = Resources.GenerateBookmark;
            _textBookmarkName.TextChanged += TextBookmarkName_TextChanged;
            _buttonOK.Text = Resources.ButtonOK;
            _buttonOK.MinimumSize = new Size(1, 1);
            _buttonCancel.Text = Resources.ButtonCancel;
            _buttonCancel.MinimumSize = new Size(1, 1);
            _labelError.Text = "";
            _mouseHook = new MouseHook();
            _mouseHook.MouseButtonPressed += MouseHook_MouseButtonPressed;
            this.Resize += AddBookmarkControl_Resize;
            this.Click += AddBookmarkControl_Click;
        }

        private void MouseHook_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            // Obtener la posición del mouse en coordenadas de pantalla
            Point mouseScreenPosition = new Point(e.X, e.Y);

            // Verificar si el mouse está dentro del control principal o sus subcontroles
            bool isMouseInsideControl = IsMouseInsideControlOrChildren(this, mouseScreenPosition);

            // Verificar si el mouse está dentro de algún panel de calendario abierto
            bool isMouseInsideCalendar = false;
            if (this.Parent?.Parent != null)
            {
                var openCalendarPanels = this.Parent.Parent.Controls.OfType<Panel>()
                    .Where(p => p.Name == "panelSelectDate" && p.Visible)
                    .ToList();

                foreach (var calendarPanel in openCalendarPanels)
                {
                    if (IsMouseInsideControlOrChildren(calendarPanel, mouseScreenPosition))
                    {
                        isMouseInsideCalendar = true;
                        break;
                    }
                }
            }

            // Si el click está fuera tanto del control como del calendario, cerrar los calendarios
            if (!isMouseInsideControl && !isMouseInsideCalendar)
            {
                Console.WriteLine("Click fuera del control - cerrando calendarios");
                System.Threading.Thread.Sleep(200); // Pequeña pausa para evitar conflictos de enfoque
                CloseCalendarsOpen();
            }
            else
            {
                Console.WriteLine("Click dentro del control o calendario - no cerrar");
            }
        }

        private bool IsMouseInsideControlOrChildren(Control control, Point mouseScreenPosition)
        {
            if (control == null || !control.Visible)
                return false;

            try
            {
                // Convertir la posición del control a coordenadas de pantalla
                Point controlScreenPosition = control.PointToScreen(Point.Empty);
                Rectangle controlBounds = new Rectangle(controlScreenPosition, control.Size);

                // Si el mouse está dentro del control actual
                if (controlBounds.Contains(mouseScreenPosition))
                    return true;

                // Verificar recursivamente en todos los controles hijos
                foreach (Control childControl in control.Controls)
                {
                    if (IsMouseInsideControlOrChildren(childControl, mouseScreenPosition))
                        return true;
                }
            }
            catch
            {
                // Ignorar excepciones si el control está siendo dispuesto
                return false;
            }

            return false;
        }



        private void AddBookmarkControl_Click(object sender, EventArgs e)
        {
            CloseCalendarsOpen();
        }

        private void AddBookmarkControl_Resize(object sender, EventArgs e)
        {
            var currentState = this.FindForm()?.WindowState ?? _windowState;
            ResizeBookMarkControls(_isInstantPlayBack, currentState);
        }

        public void ResizeBookMarkControls(bool isInstantPlayBack, FormWindowState formWindowState)
        {
            _isInstantPlayBack = isInstantPlayBack;
            if (formWindowState == FormWindowState.Minimized) return;
            _windowState = formWindowState;
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {

                _labelError.Location = new Point(_textBookmarkName.Width, _textBookmarkName.Location.Y);
                _labelError.Font = FontHelper.Get(FontSizes.Small_6, FontName.ROBOTO_BOLD, FontStyle.Bold);
                _labelError.ForeColor = Color.Red;

                if (_isInstantPlayBack)
                {
                    ResizeBookMarkInstantPlayback(this._windowState);
                }
                else
                {
                    var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                    if (!(main.Width == 1024 && main.Height == 768))
                    {
                        if (_widthPanel != 0 && _heightPanel != 0)
                        {
                            main.Width = 1400;// _widthPanel;
                            main.Height = 900;// _heightPanel;
                        }
                    }

                    int btnWidth = Convert.ToInt32(Math.Round(main.Width * 0.068M, 2));
                    int btnHeight = Convert.ToInt32(Math.Round(main.Height * 0.034M, 2));

                    _buttonOK.Size = _buttonCancel.Size = new Size(btnWidth, btnHeight);

                    _textBookmarkName.Width = Convert.ToInt32(Math.Round(this.Width * 0.250M, 2));
                    _separatorBookMarkName.Width = _textBookmarkName.Width;

                    _panelButtonsBookMark.Location = new Point(Convert.ToInt32(Math.Round(this.Width * 0.802M, 2)), _panelButtonsBookMark.Location.Y);
                    _panelButtonsBookMark.Width = btnWidth * 3;

                    int separation = (int)(_panelButtonsBookMark.Width * 0.18);
                    _buttonCancel.Location = new Point(0, _buttonCancel.Location.Y);
                    _buttonOK.Location = new Point(_buttonCancel.Location.X + _buttonCancel.Width + 10, _buttonOK.Location.Y);


                    FontSizes fontSize;
                    FontName fontName = FontName.ROBOTO_BOLD;
                    int buttonRadius = main.Width <= 1400 ? 20 :
                                      (main.Width > 2000 && main.Width <= 2560 ? 30 :
                                      (main.Width > 2560 && main.Width <= 3440 ? 40 : 30));

                    fontSize = main.Width <= 1400 ? FontSizes.Small_6 :
                               (main.Width > 2000 && main.Width <= 2560 ? FontSizes.Medium_3 :
                               (main.Width > 2560 && main.Width <= 3440 ? FontSizes.Large_0 : FontSizes.Medium_1));

                    SetFontForElements(fontSize, fontName);
                    _buttonOK.IdleBorderRadius = buttonRadius;
                    _buttonCancel.IdleBorderRadius = buttonRadius;
                }
            }
        }

        private void SetFontForElements(FontSizes fontSize, FontName fontName)
        {
            _labelBookmark.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_BOLD);
            _labelStart.Font = FontHelper.Get(fontSize, fontName);
            _labelEnd.Font = FontHelper.Get(fontSize, fontName);
            _labelName.Font = FontHelper.Get(fontSize, fontName);
        }

        public void SetTime(DateTime startTime, DateTime endTime, AppName appName = AppName.Playback)
        {
            _appActive = appName;

            if (appName == AppName.InstantPlayback)
            {
                TimeOrigin = endTime;
                TimeStart = endTime;

                SetTextValues(startTime, startTime);
            }
            else
            {
                TimeOrigin = startTime;
                TimeStart = startTime.Date;

                SetTextValues(TimeStart, startTime);
            }
        }

        public void ResizeBookMarkInstantPlayback(FormWindowState windowState)
        {
            if (this.Width <= 0 || this.Height <= 0) return;

            var screen = Screen.AllScreens.FirstOrDefault(s => s.WorkingArea.Contains(Cursor.Position));
            if (screen == null) return;
            var main = screen.Bounds;

            if (!(main.Width == 1024 && main.Height == 768))
            {
                if (_widthPanel != 0 && _heightPanel != 0)
                {
                    main.Width = 1400;// _widthPanel;
                    main.Height = 900;// _heightPanel;
                }
            }

            this.SuspendLayout();

            _windowState = windowState;
            _appActive = AppName.InstantPlayback;
            var sizeCalendar = new Size(24, 24);
            this._labelError.Text = "";

            if (windowState == FormWindowState.Normal)
            {
                this.Height = 100;
                Font commonFont = FontHelper.Get(FontSizes.Small_6, FontName.ROBOTO_BOLD, FontStyle.Bold);
                if (main.Width > 1600)
                {
                    commonFont = FontHelper.Get(FontSizes.Small_6, FontName.ROBOTO_BOLD, FontStyle.Bold);
                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    commonFont = FontHelper.Get(FontSizes.Small_2, FontName.ROBOTO_BOLD, FontStyle.Bold);
                    sizeCalendar = new Size(16, 16);
                }
                else if (main.Width >= 1400 && main.Width <= 1600)
                {
                    commonFont = FontHelper.Get(FontSizes.Small_4, FontName.ROBOTO_BOLD, FontStyle.Bold);
                }
                else
                {
                    commonFont = FontHelper.Get(FontSizes.Small_2, FontName.ROBOTO_BOLD, FontStyle.Bold);
                    sizeCalendar = new Size(16, 16);
                }
                int panelButtonsBookMarkX = Convert.ToInt32(Math.Round(this.Width * 0.785M, 2));
                int panelButtonsBookMarkY = Convert.ToInt32(Math.Round(this.Height * 0.250M, 2));
                _panelButtonsBookMark.Location = new Point(panelButtonsBookMarkX, panelButtonsBookMarkY);

                int panelButtonsBookMarkWidth = Convert.ToInt32(Math.Round(this.Width * 0.252M, 2));
                _panelButtonsBookMark.Width = panelButtonsBookMarkWidth;

                int labelBookmarkX = Convert.ToInt32(Math.Round(this.Width * 0.000M, 2));
                int labelBookmarkY = Convert.ToInt32(Math.Round(this.Height * 0.050M, 2));
                _labelBookmark.Location = new Point(labelBookmarkX, labelBookmarkY);
                _labelBookmark.Font = commonFont;

                int panelStartDateControlsX = Convert.ToInt32(Math.Round(this.Width * 0.000M, 2));
                int panelStartDateControlsY = Convert.ToInt32(Math.Round(this.Height * 0.200M, 2));
                _panelStartDateControls.Location = new Point(panelStartDateControlsX, panelStartDateControlsY);

                int panelEndDateControlsX = Convert.ToInt32(Math.Round(this.Width * 0.267M, 2));
                int panelEndDateControlsY = Convert.ToInt32(Math.Round(this.Height * 0.200M, 2));
                _panelEndDateControls.Location = new Point(panelEndDateControlsX, panelEndDateControlsY);

                int panelBookMarkNameX = Convert.ToInt32(Math.Round(this.Width * 0.519M, 2));
                int panelBookMarkNameY = Convert.ToInt32(Math.Round(this.Height * 0.200M, 2));
                _panelBookMarkName.Location = new Point(panelBookMarkNameX, panelBookMarkNameY);

                int buttonCalendarStartTimeX = Convert.ToInt32(Math.Round(this.Width * 0.004M, 2));
                _buttonCalendarStartTime.Location = new Point(buttonCalendarStartTimeX, _buttonCalendarStartTime.Location.Y);
                _buttonCalendarStartTime.Size = sizeCalendar;

                int labelStartDateX = Convert.ToInt32(Math.Round(this.Width * 0.038M, 2));
                _labelStartDate.Location = new Point(labelStartDateX, _labelStartDate.Location.Y);
                _labelStartDate.Font = commonFont;

                int pictureBoxStartTimeX = Convert.ToInt32(Math.Round(this.Width * 0.139M, 2));
                _pictureBoxStartTime.Location = new Point(pictureBoxStartTimeX, _pictureBoxStartTime.Location.Y);
                _pictureBoxStartTime.Size = sizeCalendar;

                int labelStartTimeX = Convert.ToInt32(Math.Round(this.Width * 0.171M, 2));
                _labelStartTime.Location = new Point(labelStartTimeX, _labelStartTime.Location.Y);
                _labelStartTime.Font = commonFont;

                int buttonCalendarEndTimeX = Convert.ToInt32(Math.Round(this.Width * 0.000M, 2));
                _buttonCalendarEndTime.Location = new Point(buttonCalendarEndTimeX, _buttonCalendarEndTime.Location.Y);
                _buttonCalendarEndTime.Size = sizeCalendar;

                int labelEndX = Convert.ToInt32(Math.Round(this.Width * 0.000M, 2));
                _labelEnd.Location = new Point(labelEndX, _labelEnd.Location.Y);

                int labelEndDateX = Convert.ToInt32(Math.Round(this.Width * 0.032M, 2));
                _labelEndDate.Location = new Point(labelEndDateX, _labelEndDate.Location.Y);
                _labelEndDate.Font = commonFont;

                int pictureBoxEndTimeX = Convert.ToInt32(Math.Round(this.Width * 0.128M, 2));
                _pictureBoxEndTime.Location = new Point(pictureBoxEndTimeX, _pictureBoxEndTime.Location.Y);
                _pictureBoxEndTime.Size = sizeCalendar;

                int labelEndTimeX = Convert.ToInt32(Math.Round(this.Width * 0.159M, 2));
                _labelEndTime.Location = new Point(labelEndTimeX, _labelEndTime.Location.Y);
                _labelEndTime.Font = commonFont;

                _labelName.Font = commonFont;
                _labelEnd.Font = commonFont;
                _labelStart.Font = commonFont;

                int buttonOKWidth = Convert.ToInt32(Math.Round(this.Width * 0.098M, 2));
                int buttonOKX = Convert.ToInt32(Math.Round(this.Width * 0.112M, 2));
                int buttonOKY = Convert.ToInt32(Math.Round(this.Height * 0.100M, 2));
                _buttonOK.Width = buttonOKWidth;
                _buttonOK.Location = new Point(buttonOKX, buttonOKY);
                _buttonOK.Font = commonFont;
                _buttonOK.TextAlign = ContentAlignment.MiddleCenter;

                int buttonCancelWidth = buttonOKWidth;
                int buttonCancelX = Convert.ToInt32(Math.Round(this.Width * 0.014M, 2));
                int buttonCancelY = buttonOKY;
                _buttonCancel.Width = buttonCancelWidth;
                _buttonCancel.Location = new Point(buttonCancelX, buttonCancelY);
                _buttonCancel.Font = commonFont;
                _buttonCancel.TextAlign = ContentAlignment.MiddleCenter;
            }
            else
            {
                this.Height = 180;
                Font commonFont = FontHelper.Get(FontSizes.Small_6, FontName.ROBOTO_BOLD, FontStyle.Bold);
                if (main.Width > 1600)
                {
                    commonFont = FontHelper.Get(FontSizes.Small_6, FontName.ROBOTO_BOLD, FontStyle.Bold);
                }
                else if (main.Width >= 1400 && main.Width <= 1600)
                {
                    commonFont = FontHelper.Get(FontSizes.Small_5, FontName.ROBOTO_BOLD, FontStyle.Bold);
                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    commonFont = FontHelper.Get(FontSizes.Small_3, FontName.ROBOTO_BOLD, FontStyle.Bold);
                }
                else
                {
                    commonFont = FontHelper.Get(FontSizes.Small_5, FontName.ROBOTO_BOLD, FontStyle.Bold);
                }

                _labelName.Font = commonFont;
                _labelEnd.Font = commonFont;
                _labelStart.Font = commonFont;
                _labelEndTime.Font = commonFont;
                _labelEndDate.Font = commonFont;
                _labelStartTime.Font = commonFont;
                _labelStartDate.Font = commonFont;

                int pictureBoxStartTimeX = Convert.ToInt32(Math.Round(this.Width * 0.075M, 2));
                int pictureBoxStartTimeY = Convert.ToInt32(Math.Round(this.Height * 0.250M, 2));
                _pictureBoxStartTime.Location = new Point(pictureBoxStartTimeX, _pictureBoxStartTime.Location.Y);

                int buttonCalendarStartTimeX = Convert.ToInt32(Math.Round(this.Width * 0.00319M, 2));
                _buttonCalendarStartTime.Location = new Point(buttonCalendarStartTimeX, _buttonCalendarStartTime.Location.Y);

                int labelStartDateX = Convert.ToInt32(Math.Round(this.Width * 0.01915M, 2));
                _labelStartDate.Location = new Point(labelStartDateX, _labelStartDate.Location.Y);

                int labelStartTimeX = Convert.ToInt32(Math.Round(this.Width * 0.09287M, 2));
                _labelStartTime.Location = new Point(labelStartTimeX, _labelStartTime.Location.Y);


                int buttonCalendarEndTimeX = Convert.ToInt32(Math.Round(this.Width * 0.004M, 2));
                _buttonCalendarEndTime.Location = new Point(buttonCalendarEndTimeX, _buttonCalendarEndTime.Location.Y);

                int labelEndX = Convert.ToInt32(Math.Round(this.Width * 0.002M, 2));
                _labelEnd.Location = new Point(labelEndX, _labelEnd.Location.Y);

                int labelEndDateX = Convert.ToInt32(Math.Round(this.Width * 0.019M, 2));
                _labelEndDate.Location = new Point(labelEndDateX, _labelEndDate.Location.Y);

                int pictureBoxEndTimeX = Convert.ToInt32(Math.Round(this.Width * 0.072M, 2));
                _pictureBoxEndTime.Location = new Point(pictureBoxEndTimeX, _pictureBoxEndTime.Location.Y);

                int labelEndTimeX = Convert.ToInt32(Math.Round(this.Width * 0.091M, 2));
                _labelEndTime.Location = new Point(labelEndTimeX, _labelEndTime.Location.Y);

                int buttonOKX = Convert.ToInt32(Math.Round(this.Width * 0.042M, 2));
                int buttonOKY = Convert.ToInt32(Math.Round(this.Height * 0.056M, 2));
                _buttonOK.Location = new Point(buttonOKX, buttonOKY);

                _buttonOK.Width = 70;
                _buttonCancel.Width = _buttonOK.Width;

                int buttonCancelX = Convert.ToInt32(Math.Round(this.Width * 0.042M, 2));
                int buttonCancelY = Convert.ToInt32(Math.Round(this.Height * 0.056M, 2));
                _buttonCancel.Location = new Point(buttonCancelX, buttonCancelY);

                _textBookmarkName.Width = Convert.ToInt32(Math.Round(this.Width * 0.250M, 2));
                _separatorBookMarkName.Width = _textBookmarkName.Width;

                int separation = (int)(_panelButtonsBookMark.Width * 0.15);
                _buttonCancel.Location = new Point(0, _buttonCancel.Location.Y);
                _buttonOK.Location = new Point(_buttonOK.Location.X + separation, _buttonOK.Location.Y);

                _labelBookmark.Location = new Point(0, 0);
                _buttonOK.Width = 70;
                _buttonCancel.Width = _buttonOK.Width;

                int btnWidth = Convert.ToInt32(Math.Round(main.Width * 0.068M, 2));
                int btnHeight = Convert.ToInt32(Math.Round(main.Height * 0.034M, 2));
                _buttonOK.Size = _buttonCancel.Size = new Size(btnWidth, btnHeight);

                _textBookmarkName.Width = Convert.ToInt32(Math.Round(this.Width * 0.250M, 2));
                _separatorBookMarkName.Width = _textBookmarkName.Width;

                int panelStartDateX = Convert.ToInt32(Math.Round(this.Width * 0.006M, 2));
                int panelStartDateY = Convert.ToInt32(Math.Round(this.Height * 0.270M, 2));
                _panelStartDateControls.Location = new Point(panelStartDateX, panelStartDateY);

                int panelEndDateX = Convert.ToInt32(Math.Round(this.Width * 0.162M, 2));
                _panelEndDateControls.Location = new Point(panelEndDateX, panelStartDateY);

                int panelBookMarkX = Convert.ToInt32(Math.Round(this.Width * 0.344M, 2));
                _panelBookMarkName.Location = new Point(panelBookMarkX, panelStartDateY);

                int panelButtonsX = Convert.ToInt32(Math.Round(this.Width * 0.826M, 2));
                _panelButtonsBookMark.Location = new Point(panelButtonsX, panelStartDateY);

                int separatorBookMarkNameWidth = Convert.ToInt32(Math.Round(this.Width * 0.107M, 2));
                _separatorBookMarkName.Width = separatorBookMarkNameWidth;

                int panelBookMarkNameWidth = Convert.ToInt32(Math.Round(this.Width * 0.320M, 2));
                _panelBookMarkName.Width = panelBookMarkNameWidth;

                int panelButtonsBookMarkWidth = Convert.ToInt32(Math.Round(this.Width * 0.212M, 2));
                _panelButtonsBookMark.Width = panelButtonsBookMarkWidth;

                _separatorBookMarkName.Width = _panelBookMarkName.Width;

                _buttonCancel.Location = new Point(0, _buttonCancel.Location.Y);
                _buttonOK.Location = new Point(_buttonCancel.Location.X + 150, _buttonOK.Location.Y);
                _labelError.Visible = false;

                if (main.Width == 1024 && main.Height == 768)
                {
                    _panelButtonsBookMark.Location = new Point(panelButtonsX - 30, panelStartDateY);
                    _buttonOK.Location = new Point(_buttonOK.Location.X - 50, _buttonOK.Location.Y);
                }
            }

            this.ResumeLayout(true);
        }

        private void SetTextValues(DateTime timeStart, DateTime startTime)
        {
            _labelStartDate.Text = timeStart.ToString("dd/MM/yyyy");
            _labelStartTime.Text = startTime.ToString("HH:mm:ss");

            DateTime endTime = startTime.AddMinutes(5);
            _labelEndDate.Text = timeStart.ToString("dd/MM/yyyy");
            _labelEndTime.Text = endTime.ToString("HH:mm:ss");
        }

        private void TextBookmarkName_TextChanged(object sender, EventArgs e)
        {
            CloseCalendarsOpen();
            if (_textBookmarkName.Text.Trim().Length > 0 && !string.IsNullOrWhiteSpace(_textBookmarkName.Text))
            {
                _buttonOK.Enabled = true;
            }
            else
            {
                _buttonOK.Enabled = false;
            }

            ValidateButtonOK();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            CloseCalendarsOpen();
            DateTime startDate = DateTime.Parse(_labelStartDate.Text);
            DateTime startTime = DateTime.Parse(_labelStartTime.Text);
            DateTime endDate = DateTime.Parse(_labelEndDate.Text);
            DateTime endTime = DateTime.Parse(_labelEndTime.Text);

            DateTime dStart = startDate.Add(startTime.TimeOfDay);
            DateTime dEnd = endDate.Add(endTime.TimeOfDay);

            if (dStart < dEnd)
            {
                TimeStart = dStart;
                TimeEnd = dEnd;
                BookmarkName = _textBookmarkName.Text;
                ButtonOkClicked?.Invoke(this, e);
            }
            else
            {
                _notification.Show(Resources.DateStartEndError, null);
            }

        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            CloseCalendarsOpen();
            ButtonCancelClicked?.Invoke(this, e);
        }

        private void CloseCalendarsOpen()
        {
            var parent = this.Parent?.Parent;
            if (parent == null) return;

            var openPanels = this.Parent.Parent.Controls.OfType<Panel>()
                                .Where(p => p.Name == "panelSelectDate")
                                .ToList();
            foreach (var panel in openPanels)
            {
                CloseCalendarPanel(panel);
            }
        }

        private void TextBookmarkName_KeyPress(object sender, KeyPressEventArgs e)
        {
            string invalid = new string(Path.GetInvalidFileNameChars());

            if (!(invalid.IndexOf(e.KeyChar.ToString()) > -1) || (e.KeyChar == (char)Keys.Back) || (e.KeyChar == (char)Keys.Space))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }

            ValidateButtonOK();
        }


        private void ButtonCalendarStartTime_Click(object sender, EventArgs e)
        {
            Panel panelStartDate = this.Parent.Parent.Controls.OfType<Panel>()
                .FirstOrDefault(p => p.Name == "panelSelectDate");

            if (panelStartDate != null)
            {
                CloseCalendarPanel(panelStartDate);
            }
            else
            {
                panelStartDate = CreateCalendarPanel("Start");

                if (_appActive == AppName.Playback)
                {
                    panelStartDate.Location = new Point(_panelBookmarkLocation.X + 10, (_panelBookmarkLocation.Y - this.Height) - 20);
                }
                else if (_appActive == AppName.InstantPlayback)
                {
                    if (_windowState == FormWindowState.Normal)
                    {
                        panelStartDate.Location = new Point(this.Location.X + (_buttonCalendarStartTime.Location.X * 4 + 10), this.Location.Y - (_buttonCalendarStartTime.Location.Y * 3));
                    }
                    else
                    {
                        Point buttonScreenPos = this.PointToScreen(Point.Empty);
                        Point containerRelativePos = this.Parent.Parent.PointToClient(buttonScreenPos);
                        panelStartDate.Location = new Point(
                            containerRelativePos.X + _buttonCalendarStartTime.Location.X + 10,
                            containerRelativePos.Y - panelStartDate.Height - 5
                        );
                    }

                }
                else
                {
                    Point buttonScreenPos = this.PointToScreen(Point.Empty);
                    Point containerRelativePos = this.Parent.Parent.PointToClient(buttonScreenPos);
                    panelStartDate.Location = new Point(
                        containerRelativePos.X + _buttonCalendarStartTime.Location.X + 10,
                        containerRelativePos.Y - panelStartDate.Height - 5
                    );
                }

                this.Parent.Parent.Controls.Add(panelStartDate);
                panelStartDate.BringToFront();
                panelStartDate.Focus();
            }
        }

        private void ButtonCalendarEndTime_Click(object sender, EventArgs e)
        {
            Panel panelEndDate = this.Parent.Parent.Controls.OfType<Panel>()
                .FirstOrDefault(p => p.Name == "panelSelectDate");

            if (panelEndDate != null)
            {
                CloseCalendarPanel(panelEndDate);
            }
            else
            {
                panelEndDate = CreateCalendarPanel("End");

                if (_appActive == AppName.Playback)
                {
                    panelEndDate.Location = new Point(_panelBookmarkLocation.X + (_panelStartDateControls.Width + 50), (_panelBookmarkLocation.Y - this.Height) - 20);
                }
                else if (_appActive == AppName.InstantPlayback)
                {
                    if (_windowState == FormWindowState.Normal)
                    {
                        panelEndDate.Location = new Point(this.Location.X + (_pictureBoxStartTime.Location.X * 2 + 10), this.Location.Y - (_buttonCalendarStartTime.Location.Y * 3));
                    }
                    else
                    {
                        Point buttonEndScreenPos = this.PointToScreen(Point.Empty);
                        Point relativeToGrandParent = this.Parent.Parent.PointToClient(buttonEndScreenPos);
                        panelEndDate.Location = new Point(
                            relativeToGrandParent.X + this._panelEndDateControls.Location.X + 10,
                            relativeToGrandParent.Y - panelEndDate.Height - 5
                        );
                    }

                }
                else
                {
                    Point buttonEndScreenPos = this.PointToScreen(Point.Empty);
                    Point relativeToGrandParent = this.Parent.Parent.PointToClient(buttonEndScreenPos);
                    panelEndDate.Location = new Point(
                        relativeToGrandParent.X + this._panelEndDateControls.Location.X + 10,
                        relativeToGrandParent.Y - panelEndDate.Height - 5
                    );
                }

                this.Parent.Parent.Controls.Add(panelEndDate);
                panelEndDate.BringToFront();
                panelEndDate.Focus();
            }
        }



        private Panel CreateCalendarPanel(string type)
        {
            this.Focus();
            Panel panelSelectDate = new Panel
            {
                Name = "panelSelectDate",
                Tag = type,
                BackColor = Color.White,
                Size = new Size(270, 210),
                TabIndex = 25
            };

            panelSelectDate.Leave += (s, e) =>
            {
                if (this == null || this.IsDisposed || this.Disposing)
                    return;

                this.BeginInvoke(new Action(() =>
                {
                    var form = panelSelectDate.FindForm();
                    if (form != null && !panelSelectDate.Contains(form.ActiveControl))
                    {
                        CloseCalendarsOpen();
                    }
                }));
            };

            Panel panelButtons = new Panel
            {
                Name = "panelButtons",
                BackColor = Color.White,
                Size = new Size(150, 20),
                Location = new Point(58, 150)
            };
            panelSelectDate.Controls.Add(panelButtons);

            MonthCalendar monthCalendar = new MonthCalendar
            {
                Location = new Point(10, 10),
                MaxSelectionCount = 1
            };
            monthCalendar.SelectionStart =
                type == "Start" ?
                DateTime.ParseExact(_labelStartDate.Text + " " + _labelStartTime.Text, "dd/MM/yyyy HH:mm:ss", null) : DateTime.ParseExact(_labelEndDate.Text + " " + _labelEndTime.Text, "dd/MM/yyyy HH:mm:ss", null);

            panelSelectDate.Controls.Add(monthCalendar);

            Button btnApply = new Button
            {
                Text = Resources.ApplyButton,
                Size = new Size(60, 30),
                Location = new Point(10, monthCalendar.Bottom + 10),
                BackColor = Color.Gainsboro
            };
            btnApply.Click += BtnApply_Click;
            panelSelectDate.Controls.Add(btnApply);

            Button btnClose = new Button
            {
                Text = Resources.ButtonClose,
                Size = new Size(60, 30),
                Location = new Point(btnApply.Right + 10, monthCalendar.Bottom + 10),
                BackColor = Color.Gainsboro
            };
            btnClose.Click += BtnClose_Click;
            panelSelectDate.Controls.Add(btnClose);

            DateTimePicker timePicker = new DateTimePicker
            {
                Format = DateTimePickerFormat.Time,
                ShowUpDown = true,
                Size = new Size(100, 30),
                Location = new Point(btnClose.Right + 15, monthCalendar.Bottom + 15)
            };
            timePicker.Value = monthCalendar.SelectionStart;
            panelSelectDate.Controls.Add(timePicker);
            return panelSelectDate;
        }

        private void CloseCalendarPanel(Panel panel)
        {
            if (panel != null)
            {
                this.Parent.Parent.Controls.Remove(panel);
                panel.Dispose();
            }

            ValidateButtonOK();
        }

        private void BtnApply_Click(object sender, EventArgs e)
        {
            var parent = this.Parent?.Parent;
            if (parent == null) return;

            Panel panel = (Panel)((Button)sender).Parent;
            MonthCalendar monthCalendar = panel.Controls.OfType<MonthCalendar>().FirstOrDefault();
            DateTimePicker timePicker = panel.Controls.OfType<DateTimePicker>().FirstOrDefault();

            if (monthCalendar != null && timePicker != null)
            {
                DateTime selectedDate = monthCalendar.SelectionStart;
                DateTime selectedTime = timePicker.Value;

                string dateText = selectedDate.ToString("dd/MM/yyyy");
                string timeText = selectedTime.ToString("HH:mm:ss");

                if (panel.Tag.ToString() == "Start")
                {
                    _labelStartDate.Text = dateText;
                    _labelStartTime.Text = timeText;
                }
                else
                {
                    _labelEndDate.Text = dateText;
                    _labelEndTime.Text = timeText;
                }

                CloseCalendarPanel(panel);
            }

            ValidateButtonOK();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            var parent = this.Parent?.Parent;
            if (parent == null) return;

            Panel panelSelectDate = this.Parent.Parent.Controls.OfType<Panel>()
                .FirstOrDefault(p => p.Name == "panelSelectDate");

            if (panelSelectDate != null)
            {
                CloseCalendarPanel(panelSelectDate);
            }
        }

        private void ValidateButtonOK()
        {
            DateTime startDateTime = DateTime.ParseExact(_labelStartDate.Text + " " + _labelStartTime.Text, "dd/MM/yyyy HH:mm:ss", null);
            DateTime endDateTime = DateTime.ParseExact(_labelEndDate.Text + " " + _labelEndTime.Text, "dd/MM/yyyy HH:mm:ss", null);
            TimeSpan duration = endDateTime - startDateTime;

            string bookmarkName = _textBookmarkName.Text;
            bool isValid = true;

            if (startDateTime > endDateTime || endDateTime < startDateTime || duration.TotalHours > 24 ||
                string.IsNullOrEmpty(bookmarkName) || string.IsNullOrWhiteSpace(bookmarkName) || HasSpecialCharacters(bookmarkName))
            {
                _labelError.Text = "*";
                _buttonOK.Enabled = false;
                isValid = false;
            }

            if (isValid)
            {
                _labelError.Text = "";
                _buttonOK.Enabled = true;
            }
        }

        private bool HasSpecialCharacters(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"[^a-zA-Z0-9\s]");
        }
    }
}
