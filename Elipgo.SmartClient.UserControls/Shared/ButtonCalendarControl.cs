using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Shared
{
    public partial class ButtonCalendarControl : UserControl
    {
        private PopedCotainer Cotainer;
        private PoperContainer Poper;
        private CalendarControl calendar;
        private MouseHook mouseHook;
        //public event EventHandler<Dictionary<string, DateTime>> DateTimeSelected;
        public event EventHandler<Dictionary<string, DateTime>> DateTimeClick;
        public event EventHandler<Dictionary<string, DateTime>> DateTimeLoad;
        public event EventHandler<Dictionary<string, DateTime>> DateTimeChange;
        private bool _resizeLoad = false;
        public ButtonCalendarControl(bool timepickerVisible = true)
        {
            calendar = new CalendarControl(timepickerVisible);
            calendar.DateTimeClick += ButtonOK_Click;
            calendar.DateTimeChange += DateChangeCalendar;
            calendar.DateTimeClose += ButtonClose_Click;
            InitializeComponent();
            this.ButtonCalendar.Image = FileResources.icon_calendar;
            _resizeLoad = true;
            Cotainer = calendar;
            Poper = new PoperContainer(Cotainer);
            Poper.Closing += Poper_Closing;

            this.ButtonCalendar.Click += ButtonCalendar_Click;
            this.Load += ButtonCalendar_Load;
            this.Resize += ButtonCalendarControl_Resize;
            this.Cursor = Cursors.Hand;
            calendar.Hour = ((DateTimePicker)calendar.Controls[1]).Value;
            CultureInfo ci = CultureInfo.InstalledUICulture;
            bunifuToolTip1.SetToolTip(this.ButtonCalendar, ci.Name.Contains("es") ? ButtonsContextBar.Calendar.GetDescription() : ButtonsContextBar.Calendar.GetAttribute<DescriptionEN>().Descripcion);

            mouseHook = new MouseHook();
        }

        private void MouseHook_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Point screenPoint = new Point(e.X, e.Y);
                Point clientPoint = this.PointToClient(screenPoint);
                bool isDentro = false;

                if (this.ClientRectangle.Contains(clientPoint))
                {
                    isDentro = true;
                }
                else if (Poper != null && Poper.Visible)
                {
                    Rectangle poperBounds = new Rectangle(Poper.Location, Poper.Size);
                    if (poperBounds.Contains(screenPoint))
                    {
                        isDentro = true;
                    }
                }
                if (!isDentro)
                {
                    System.Threading.Thread.Sleep(200); // Pequeña pausa para evitar conflictos
                    Poper.Hide();
                }
            }
            finally { }
            


 
        }

        public DateTime Date
        {
            set => this.calendar.Date = value;
        }

        public DateTime Hour
        {
            set => this.calendar.Hour = value;
        }

        private void Poper_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            this.calendar.SetListRecording(null);
            mouseHook.MouseButtonPressed -= MouseHook_MouseButtonPressed;
        }

        private void ButtonCalendar_Click(object sender, EventArgs e)
        {
            this.Focus();
            mouseHook.MouseButtonPressed += MouseHook_MouseButtonPressed;
            Poper.Show(this);
            DateChangeCalendar(sender, e);
        }

        public void SetValues(int minutes)
        {
            if (minutes != 0)
            {
                ((DateTimePicker)calendar.Controls[1]).Value = calendar.Hour.AddMinutes(minutes);
            }
        }

        internal void OpenCalendar()
        {
            this.Focus();
            Poper.Show(this);
            DateChangeCalendar(null, null);
        }

        private void ButtonCalendar_Load(object sender, EventArgs e)
        {
            var date = calendar.Date;
            var time = calendar.Hour;

            DateTimeLoad?.Invoke(this, new Dictionary<string, DateTime>
            {
                {"date", date},
                {"time", time }
            });
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            var date = calendar.Date;
            var time = calendar.Hour;

            DateTimeClick?.Invoke(this, new Dictionary<string, DateTime>
            {
                {"date", date},
                {"time", time }
            });
        }

        private void DateChangeCalendar(object sender, EventArgs e)
        {
            var date = calendar.Date;
            var time = calendar.Hour;
            this.calendar.SetListRecordingLoading();
            AsyncCallback callbackProgress = new AsyncCallback(CallbackProgress);

            if (ButtonCalendar.InvokeRequired)
            {
                ButtonCalendar.Invoke((MethodInvoker)delegate
                {
                    DateChangeCalendar(sender, e);

                });
                return;
            }

            //Obtenemos la lista de suscriptores
            var delegates = DateTimeChange?.GetInvocationList();

            if (delegates != null)
            {
                var eventData = new Dictionary<string, DateTime>
                {
                    {"date", date},
                    {"time", time }
                };

                foreach (var handler in delegates)
                {
                    // Disparamos cada suscriptor de forma individual y asíncrona
                    ((EventHandler<Dictionary<string, DateTime>>)handler).BeginInvoke(
                        this,
                        eventData,
                        callbackProgress,
                        null);
                }
            }

        }
        private void CallbackProgress(IAsyncResult result)
        {
            //calendar.DateTimeChange -= DateChangeCalendar;

        }
        public void SetCalendarListRecording(List<TimelineDTO> list)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => SetCalendarListRecording(list)));
                return;
            }

            if (this.IsDisposed) return;

            try
            {
                if (!this.Visible || this.TopLevelControl == null || !this.TopLevelControl.ContainsFocus)
                {
                    this.calendar.SetListRecording(list);
                    return;
                }

                this.calendar.SetListRecording(list);

                if (list == null || list.Count == 0)
                {
                    Poper.Show(this);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Visual Error on SetCalendarListRecording: " + ex.Message, LogPriority.Information);
            }
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            Poper.Hide();
        }

        private void ButtonCalendarControl_Resize(object sender, EventArgs e)
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                var buttonCalendarWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0127M), 2));
                var buttonCalendarHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.022M), 2));
                this.ButtonCalendar.Size = new Size(buttonCalendarWidth, buttonCalendarHeight);
                _resizeLoad = false;
            }
        }

        public void ListRecordingOff()
        {
            this.calendar.ListRecordingOff();
        }
        
        public void HideProgressBar()
        {
            this.calendar.HideProgressBar();
        }

        public void ToggleListRecording(bool enabled)
        {
            calendar.ToggleListRecording(enabled);
        }
    }
}
