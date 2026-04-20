using CefSharp;
using CefSharp.WinForms;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.UserControls.ElementContainer;
using Elipgo.SmartClient.UserControls.InstantPlayer;
using Elipgo.SmartClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public delegate void AlarmDiagnosticButtonOkClickEventHandler(object sender, AlarmDTO alarm);
    public delegate void AlarmDiagnosticButtonCancelClickEventHandler(object sender, AlarmDTO alarm);

    public partial class AlarmDiagnosticBase : UserControl
    {
        public event AlarmDiagnosticButtonOkClickEventHandler ButtonOkClick;
        public event AlarmDiagnosticButtonCancelClickEventHandler ButtonCancelClick;

        protected AlarmViewModel _viewModel;

        protected AlarmDTO _alarmDTO;

        // Task para que las clases derivadas puedan esperar a que _alarmDTO esté inicializado
        protected Task AlarmDTOInitializationTask { get; private set; }

        private ChromiumWebBrowser browser = null;

        private MapConfigDTO _mapConfig;
        private bool _toggleOn;
        CardDto m_card;

        public AlarmDiagnosticBase(AlarmViewModel viewModel, CardDto card)
        {
            m_card = card;
            InitializeComponent();

            this.SetDefaultSizeAndLocation();

            this._viewModel = viewModel;
            this._alarmIcon.Image = FileResources.icon_alarm_card;
            _contentStep.Scroll += ContentList_Scroll;

            // Sincronizar _toggleOn con el valor inicial del toggle
            _toggleOn = this._alarmStateToggleSwitch.Value;

            this._alarmStateToggleSwitch.Click += (s, e) =>
            {
                _toggleOn = this._alarmStateToggleSwitch.Value;
            };

            //this._alarmDTO = this._viewModel.GetAlarm(m_card.IdAlarm);
            AlarmDTOInitializationTask = InitializeAlarmDTOAsync(m_card.IdAlarm);
            this.SetSizes();
            this.SetButtonEvent();
            this.SetData();
        }

        private async Task InitializeAlarmDTOAsync(int IdAlarm)
        {
            this._alarmDTO = await this._viewModel.GetAlarm(IdAlarm);
        }

        private void AlarmDiagnosticBase_Resize(object sender, EventArgs e)
        {
            SetSizes();
        }

        private void SetData()
        {
            string message = null;
            _procedure.Text = Resources.Procedure;
            _addNote.Text = Resources.note;
            _alarmState.Text = Resources.AlarmState;
            _alarmConfirm.Text = Resources.AlarmConfirmed;
            _deviceName.Text = m_card.DeviceName;
            if (Enum.TryParse<AlarmType>(m_card.Type, out var enumValue))
            {
                _alarmType.Text = EnumExtension.GetTranslation(enumValue);
            }
            _alarmLocation.Text = m_card.Site;
            _alarmDateTime.Text = m_card.Time.ToString() + (!string.IsNullOrEmpty(m_card.Cad_Invoice) ? (" Folio " + m_card.Cad_Invoice) : "");
            if ("LPR".Equals(m_card.Type))
            {
                string[] tempMessage = m_card.Message != null ? m_card.Message.Split(',') : m_card.Personalized_Message.Split('|');
                string[] tempPlate = tempMessage[0].Split(':');
                if (tempMessage[1] != " ")
                {
                    string[] tempList = tempMessage[1].Split(':');
                    string list = tempList[1].Replace('[', ' ');
                    list = list.Replace(']', ' ');
                    list = list.Replace('.', ' ');
                    message = tempPlate[0] + " : " + tempPlate[1] + " - " + tempList[0] + " : " + list;
                }
                if (tempMessage == null && !string.IsNullOrEmpty(m_card.Personalized_Message))
                {
                    _alarmMessage.Text = m_card.Personalized_Message.Replace("|", " - ");
                }
            }
            else
            {
                _alarmMessage.Text = m_card.Message;
            }
        }

        private async void CreateConfigMaps(string type)
        {
            // Esperar a que _alarmDTO esté inicializado
            await AlarmDTOInitializationTask;

            if (_alarmDTO == null)
            {
                Logger.Log("CreateConfigMaps: _alarmDTO es null después de esperar inicialización", LogPriority.Warning);
                return;
            }

            double LocationLatitude = 0;
            double LocationLongitude = 0;
            if (!string.IsNullOrEmpty(this._alarmDTO.Latitude) && !string.IsNullOrEmpty(this._alarmDTO.Longitude))
            {
                LocationLatitude = Convert.ToDouble(this._alarmDTO.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                LocationLongitude = Convert.ToDouble(this._alarmDTO.Longitude, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                var locationChannel = await this._viewModel.GetDeviceFeature(this._alarmDTO.ObjectId);
                if (!string.IsNullOrEmpty(locationChannel?.Latitude) && !string.IsNullOrEmpty(locationChannel?.Longitude))
                {
                    LocationLatitude = Convert.ToDouble(locationChannel.Latitude, System.Globalization.CultureInfo.InvariantCulture);
                    LocationLongitude = Convert.ToDouble(locationChannel.Longitude, System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    if (_alarmDTO.SiteId != 0)
                    {
                        this._viewModel._dvfid = this._alarmDTO.ObjectId;
                        var _catalog = await this._viewModel.GetCatalogAsync();
                        var sites = _catalog.Sites;

                        var location = sites.FirstOrDefault(x => x.Id.Equals(this._alarmDTO.SiteId));
                        LocationLatitude = location.LocationLatitude;
                        LocationLongitude = location.LocationLongitude;
                    }
                }
            }

            this._mapConfig = new MapConfigDTO()
            {

                Type = type,
                Name = "Alert",
                LocationLatitude = LocationLatitude,
                LocationLongitude = LocationLongitude
            };
        }

        private void SetButtonEvent()
        {
            this._buttonOK.Click += ButtonOK_Click;
            this._buttonCancel.Click += ButtonCancel_Click;
        }

        public virtual void ButtonCancel_Click(object sender, EventArgs e)
        {
            ButtonCancelClick?.Invoke(sender, _alarmDTO);
        }

        public virtual void ButtonOK_Click(object sender, EventArgs e)
        {
            if (_alarmDTO == null)
            {
                Logger.Log("ButtonOK_Click: _alarmDTO es null, no se puede diagnosticar", LogPriority.Warning);
                return;
            }

            this._alarmDTO.Comments = this._txtNote.Text;
            this._alarmDTO.Level = _toggleOn ? AlarmLevels.CriticalChecked : AlarmLevels.CriticalAttendedToggle;
            ButtonOkClick?.Invoke(sender, _alarmDTO);
        }


        private void SetDefaultSizeAndLocation()
        {
            Location = new Point(13, 100);
            Size = new Size(this.Width - 40, this.Height - 50);
            TabIndex = 0;
            Margin = new Padding(10, 24, 0, 0);
        }

        private void ContentList_Scroll(object sender, ScrollEventArgs e)
        {
            System.Console.WriteLine($"Scroll{e.OldValue}");
        }

        private void BunifuVScrollBar1_Scroll(object sender, Bunifu.UI.WinForms.BunifuVScrollBar.ScrollEventArgs e)
        {
            _contentStep.AutoScrollPosition = new Point(_contentStep.AutoScrollPosition.X, e.Value);
            Console.WriteLine(e.Value);
            _contentStep.Refresh();
        }
        protected async void LoadStep()
        {
            this._contentStep.Hide();

            // Esperar a que _alarmDTO esté inicializado
            await AlarmDTOInitializationTask;

            if (_alarmDTO == null)
            {
                Logger.Log("LoadStep: _alarmDTO es null después de esperar inicialización", LogPriority.Warning);
                return;
            }

            var workFLow = await this._viewModel.GetAlarmWorkflow(_alarmDTO.AlarmWorkFlowId);

            if (workFLow != null)
            {
                if (workFLow.Actions != null)
                {
                    char[] stringSeparators = new char[] { '\n' };
                    foreach (var item in workFLow.Actions.Split(stringSeparators))
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            var lbl = new Label() { Text = item, Width = this._contentStep.Width - 30, AutoSize = true, Padding = new Padding(4) };
                            if (Screen.PrimaryScreen.Bounds.Width >= 2000)
                            {
                                lbl.Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_REGULAR);
                            }

                            this._contentStep.Controls.Add(lbl);
                        }
                    }
                }

                this._contentStep.Show();
                _scrollBar.Show();
                _scrollBar.Refresh();
                if (this._contentStep.Controls.Count > 6)
                {
                    _scrollBar.Maximum = (120 * this._contentStep.Controls.Count) - 247;
                    _contentStep.VerticalScroll.Visible = true;
                    this._scrollBar.Visible = true;
                }
                this._scrollBar.Scroll += BunifuVScrollBar1_Scroll;
                _scrollBar.BindingContainer = _contentStep;
                _scrollBar.BackgroundColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            }
        }

        public void SetSizes()
        {
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                var panelCameraWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.725M), 2));
                var panelCameraHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.668M), 2));
                _panelCamera.Size = new Size(panelCameraWidth, panelCameraHeight);

                var PanelCameraX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.00259M), 2));
                var PanelCameraY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.146M), 2));
                _panelCamera.Location = new Point(PanelCameraX, PanelCameraY);

                var ProcedureX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.761M), 2));
                var ProcedureY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.046M), 2));
                _procedure.Location = new Point(ProcedureX, ProcedureY);

                var ContentBodyX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.761M), 2));
                var ContentBodyY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.084M), 2));
                _contentBody.Location = new Point(ContentBodyX, ContentBodyY);

                var ContentBodyWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.219M), 2));
                var ContentBodyHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.130M), 2));
                _contentBody.Size = new Size(ContentBodyWidth, ContentBodyHeight);

                var ContentStepX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.761M), 2));
                var ContentStepY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.084M), 2));
                _contentStep.Location = new Point(ContentStepX, ContentStepY);

                var ContentStepWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.202M), 2));
                var ContentStepHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.130M), 2));
                _contentStep.Size = new Size(ContentStepWidth, ContentStepHeight);

                var AddNoteX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.761M), 2));
                var AddNoteY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.229M), 2));
                _addNote.Location = new Point(AddNoteX, AddNoteY);

                var txtNoteWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.217M), 2));
                var txtNoteHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.363M), 2));
                _txtNote.Size = new Size(txtNoteWidth, txtNoteHeight);

                var txtNoteX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.761M), 2));
                var txtNoteY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.261M), 2));
                _txtNote.Location = new Point(txtNoteX, txtNoteY);

                var bunifuSeparator1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.217M), 2));
                var bunifuSeparator1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.032M), 2));
                _bunifuSeparator1.Size = new Size(bunifuSeparator1Width, bunifuSeparator1Height);

                var bunifuSeparator1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.761M), 2));
                var bunifuSeparator1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.645M), 2));
                _bunifuSeparator1.Location = new Point(bunifuSeparator1X, bunifuSeparator1Y);

                var AlarmStateWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.104M), 2));
                var AlarmStateHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                _alarmState.Size = new Size(AlarmStateWidth, AlarmStateHeight);

                var AlarmStateX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.761M), 2));
                var AlarmStateY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.676M), 2));
                _alarmState.Location = new Point(AlarmStateX, AlarmStateY);

                var AlarmConfirmWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.104M), 2));
                var AlarmConfirmHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                _alarmConfirm.Size = new Size(AlarmConfirmWidth, AlarmConfirmHeight);

                var AlarmConfirmX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.761M), 2));
                var AlarmConfirmY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.713M), 2));
                _alarmConfirm.Location = new Point(AlarmConfirmX, AlarmConfirmY);

                var AlarmStateToggleSwitchX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.958M), 2));
                var AlarmStateToggleSwitchY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.718M), 2));
                _alarmStateToggleSwitch.Location = new Point(AlarmStateToggleSwitchX, AlarmStateToggleSwitchY);

                var btnWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.048M), 2));
                var btnHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.034M), 2));

                var ButtonOKX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.912M), 2));
                var ButtonOKY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.792M), 2));
                _buttonOK.Location = new Point(ButtonOKX, ButtonOKY);
                _buttonOK.Size = new Size(btnWidth, btnHeight);

                var ButtonCancelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.855M), 2));
                var ButtonCancelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.792M), 2));
                _buttonCancel.Location = new Point(ButtonCancelX, ButtonCancelY);
                _buttonCancel.Size = new Size(btnWidth, btnHeight);

                var inconWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.017M), 2));
                var inconHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.017M), 2));

                this._alarmDateTime.ForeColor = ColorTranslator.FromHtml("#D3D3D3");
                this._deviceName.ForeColor = ColorTranslator.FromHtml("#FFFFFF");

                if (main.Width >= 2000 && main.Width < 2560)
                {
                    _deviceName.AutoSize = true;
                    _alarmDateTime.AutoSize = true;
                    _procedure.AutoSize = true;
                    _addNote.AutoSize = true;
                    _alarmState.AutoSize = true;
                    _alarmType.AutoSize = true;
                    _alarmLocation.AutoSize = true;
                    //_alarmMessage.AutoSize = true;

                    _deviceName.Font = FontHelper.Get(FontSizes.Large_2, FontName.ROBOTO_REGULAR);
                    _alarmDateTime.Font = FontHelper.Get(FontSizes.Large_2, FontName.ROBOTO_REGULAR);
                    _procedure.Font = FontHelper.Get(FontSizes.Large_2, FontName.ROBOTO_REGULAR);
                    _addNote.Font = FontHelper.Get(FontSizes.Large_2, FontName.ROBOTO_REGULAR);
                    _alarmState.Font = FontHelper.Get(FontSizes.Large_2, FontName.ROBOTO_REGULAR);
                    _alarmConfirm.Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_REGULAR);
                    _alarmType.Font = FontHelper.Get(FontSizes.Large_2, FontName.ROBOTO_REGULAR);
                    _alarmLocation.Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_REGULAR);
                    _txtNote.Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_REGULAR);
                    _alarmIcon.Size = new Size(inconWidth, inconHeight);
                    foreach (Control ctl in _contentStep.Controls)
                    {
                        (ctl as Label).Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_REGULAR);
                    }
                    return;
                }
                else if (main.Width == 1024 && main.Height == 768)
                {
                    _procedure.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                    foreach (Control ctl in _contentStep.Controls)
                    {
                        (ctl as Label).Font = FontHelper.Get(FontSizes.Small_3, FontName.ROBOTO_REGULAR);
                    }

                    this._buttonCancel.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_MEDIUM);
                    this._buttonOK.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_MEDIUM);
                    _buttonOK.IdleBorderRadius = 20;
                    _buttonCancel.IdleBorderRadius = 20;
                }
                else if (main.Width <= 1400)
                {
                    this._procedure.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_MEDIUM);
                    this._addNote.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                    this._alarmState.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_MEDIUM);
                    this._alarmConfirm.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);

                    this._buttonCancel.Font = FontHelper.Get(FontSizes.Medium_0, FontName.ROBOTO_MEDIUM);
                    this._buttonOK.Font = FontHelper.Get(FontSizes.Medium_0, FontName.ROBOTO_MEDIUM);
                    _buttonOK.IdleBorderRadius = 20;
                    _buttonCancel.IdleBorderRadius = 20;
                }
                else if (main.Width >= 2560 && main.Width < 3440)
                {
                    _addNote.AutoSize = true;
                    this._procedure.Font = FontHelper.Get(FontSizes.Large_0, FontName.ROBOTO_MEDIUM);
                    this._addNote.Font = FontHelper.Get(FontSizes.Large_0, FontName.ROBOTO_REGULAR);
                    this._alarmState.Font = FontHelper.Get(FontSizes.Large_0, FontName.ROBOTO_MEDIUM);
                    this._alarmConfirm.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_REGULAR);
                    this._buttonCancel.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_MEDIUM);
                    this._buttonOK.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_MEDIUM);
                    _buttonOK.IdleBorderRadius = 30;
                    _buttonCancel.IdleBorderRadius = 30;
                }
                else if (main.Width <= 3440)
                {
                    _addNote.AutoSize = true;
                    this._procedure.Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_MEDIUM);
                    this._addNote.Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_REGULAR);
                    this._alarmState.Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_MEDIUM);
                    this._alarmConfirm.Font = FontHelper.Get(FontSizes.Large_0, FontName.ROBOTO_REGULAR);
                    this._buttonCancel.Font = FontHelper.Get(FontSizes.Large_0, FontName.ROBOTO_MEDIUM);
                    this._buttonOK.Font = FontHelper.Get(FontSizes.Large_0, FontName.ROBOTO_MEDIUM);
                    _buttonOK.IdleBorderRadius = 40;
                    _buttonCancel.IdleBorderRadius = 40;
                }
                else
                {
                    _addNote.AutoSize = true;
                    this._procedure.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_MEDIUM);
                    this._addNote.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_REGULAR);
                    this._alarmState.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_MEDIUM);
                    this._alarmConfirm.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                    this._buttonCancel.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_MEDIUM);
                    this._buttonOK.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_MEDIUM);
                    _buttonOK.IdleBorderRadius = 30;
                    _buttonCancel.IdleBorderRadius = 30;
                }

                this._alarmMessage.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_MEDIUM);

                this._alarmDateTime.ForeColor = ColorTranslator.FromHtml("#D3D3D3");
                this._deviceName.ForeColor = ColorTranslator.FromHtml("#FFFFFF");
            }

        }

        protected virtual void CreatePanel(string type)
        {
            CreateConfigMaps(type);
            this.browser = CreateBrowser();
            AddElementToPanel(this.browser);

        }

        protected virtual ChromiumWebBrowser CreateBrowser()
        {
            CefSettings settings = new CefSettings()
            {

            };
            ChromiumWebBrowser chromiumWebBrowser;

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);
            }
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            string page = string.Format(@"{0}\Resources\Html\gmaps.html", Application.StartupPath);
            chromiumWebBrowser = new ChromiumWebBrowser(page)
            {
                RequestContext = new RequestContext(),
                BrowserSettings = new BrowserSettings
                {
                    FileAccessFromFileUrls = CefState.Enabled,
                    UniversalAccessFromFileUrls = CefState.Enabled,
                    BackgroundColor = Cef.ColorSetARGB(255, 34, 34, 34)
                },
                MenuHandler = new CustomCefMenuHandler()
            };
            var bound = new BoundObject();
            chromiumWebBrowser.JavascriptObjectRepository.Register("bound", bound, true);
            chromiumWebBrowser.LoadingStateChanged += Browser_LoadingStateChanged;
            return chromiumWebBrowser;
        }

        /// <summary>
        /// Llega con la instancia del Chromium
        /// </summary>
        /// <param name="control"></param>
        protected virtual void AddElementToPanel(Control control)
        {
            foreach (var it in this._panelCamera.Controls)
            {
                if (it is ElementSnapshotControl)
                {
                    (it as ElementSnapshotControl).Dispose();
                }
                else if (it is ElementCameraControl)
                {
                    (it as ElementCameraControl).Dispose();
                }
                else if (it is Label)
                {
                    (it as Label).Dispose();
                }
                else if (it is CefSharp.WinForms.ChromiumWebBrowser)
                {
                    (it as ChromiumWebBrowser).LoadingStateChanged -= Browser_LoadingStateChanged;
                }
                else if (it is InstantPlayerControl)
                {
                    (it as InstantPlayerControl).Dispose();
                }
            }

            this._panelCamera.Controls.Clear();
            if (control != null)
            {
                this._panelCamera.Controls.Add(control);
            }
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new List<MapConfigDTO>() { _mapConfig });
                browser.GetMainFrame().ExecuteJavaScriptAsync(string.Format("addMarkers('{0}')", json));
            }
        }
    }
}
