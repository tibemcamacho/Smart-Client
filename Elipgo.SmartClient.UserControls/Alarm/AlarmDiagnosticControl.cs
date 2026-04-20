using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.UserControls.ContextBar;
using Elipgo.SmartClient.UserControls.ElementContainer;
using Elipgo.SmartClient.UserControls.InstantPlayer;
using Elipgo.SmartClient.ViewModels;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public delegate void AlarmDiagnosticContextBarButtonClickEventHandler(object sender, ButtonsContextBar button);
    public partial class AlarmDiagnosticControl : AlarmDiagnosticBase
    {
        public event AlarmDiagnosticContextBarButtonClickEventHandler ContextBarButtonClick;
        private static readonly IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private readonly IDriverFactory DriverFactory = Locator.Current.GetService<IDriverFactory>();
        private IDriverLive _instantPlaybackDriver;
        private CameraDTO _cameraDTO = new CameraDTO();
        private DateTime _timeAction;
        List<OptionObjectDTO> dvfs;
        string mode;
        object _lock = new object();
        public string Snapshot { get; set; }
        Label labelWithOutResult;
        private string nameTab = string.Empty;
        CardDto _card;
        public AlarmDiagnosticControl(AlarmViewModel viewModel, CardDto card) : base(viewModel, card)
        {
            InitializeComponent();
            this._card = card;
            _viewDropdown.Items.Clear();
            _timeAction = card.Time;
            _buttonIstanPlayback.Visible = false;
            this.nameTab = viewModel.MainView.ApplicationTitle;
            //this.Resize += new EventHandler(AlarmDiagnosticControl_Resize);
            InitializeAsync(card);

            this.Snapshot = card.Snapshot;
            //this.Snapshot =  _viewModel.GetSnapshot(card.IdAlarm);
            
        }

        public async void InitializeAsync(CardDto card)
        {
            // Esperar a que _alarmDTO esté inicializado desde la clase base
            await AlarmDTOInitializationTask;

            // Verificar que _alarmDTO se haya inicializado correctamente
            if (_alarmDTO == null)
            {
                Common.Logger.Log("AlarmDiagnosticControl.InitializeAsync: _alarmDTO es null después de esperar inicialización", Common.LogPriority.Warning);
                return;
            }

            _cameraDTO = await _viewModel.GetCamera(_alarmDTO.ObjectId);
            this.mode = card.DeviceType.ToUpper();
            List<OptionObjectDTO> options = new List<OptionObjectDTO>();

            if (card.DeviceType == "DI" || card.DeviceType.ToUpper() == "VCA")
            {
                if (card.dvfs.Length > 0)
                    options.Add(new OptionObjectDTO() { Key = "live", Name = "En vivo" });
            }
            else
            {
                if (_cameraDTO != null)
                    options.Add(new OptionObjectDTO() { Key = "live", Name = "En vivo" });
            }
            if (this._alarmDTO.HasSnapshot)
            {
                options.Add(new OptionObjectDTO() { Key = "snapshot", Name = "Snapshot" });
            }


            options.Add(new OptionObjectDTO() { Key = "geolocalizacion", Name = "Geolocalización" });

            if (appAuthorization.Exist(ButtonsContextBar.InstantPlayback.GetAttribute<PermissionLive>().PermissionKey))
            {
                if (_cameraDTO != null || card.DeviceType.ToUpper() == "VCA")
                {
                    if (card.DeviceType == "DI" || card.DeviceType.ToUpper() == "VCA")
                    {
                        if (card.dvfs.Length > 0)
                            options.Add(new OptionObjectDTO() { Key = "instantPlayback", Name = "InstantPlayback" });
                    }
                    else
                        options.Add(new OptionObjectDTO() { Key = "instantPlayback", Name = "InstantPlayback" });

                    //si la alarma tiene asociado algun otro device entonces muestro el combo
                    if (card.dvfs != null && card.dvfs.Length > 0)
                    {
                        _dvfsRelated.Visible = true;
                        _dvfsRelated.Enabled = false;
                        dvfs = card.dvfs.Select(x => new OptionObjectDTO()
                        {
                            Key = x.IdDvf.ToString(),
                            Name = x.Name
                        }).ToList();
                        //if (card.DeviceType.ToUpper() == "DI")
                        //{
                        //    dvfs.Insert(0, new OptionObjectDTO()
                        //    {
                        //        Key = cameraDTO.Id.ToString(),
                        //        Name = cameraDTO.Name
                        //    });
                        //}
                        _dvfsRelated.DataSource = new BindingSource(dvfs, null);
                        _dvfsRelated.DisplayMember = "Name";
                        _dvfsRelated.ValueMember = "Key";
                        _dvfsRelated.Location = _viewDropdown.Location;
                        _viewDropdown.Location = new Point(_viewDropdown.Location.X - _viewDropdown.Width - 20, _viewDropdown.Location.Y);
                    }
                }
                ResizeControl();
            }
            if (options.Count == 1)
            {
                _viewDropdown.Visible = false;
            }

            _viewDropdown.DataSource = new BindingSource(options, null);
            _viewDropdown.DisplayMember = "Name";
            _viewDropdown.ValueMember = "Key";

            _viewDropdown.SelectedValue = "instantPlayback";
        }

        public void ResizeControl()
        {
            if (this.IsDisposed || this.Disposing)
                return;

            lock (_lock)
            {
                if (this.IsDisposed || this.Disposing)
                    return;

                var screen = Screen.AllScreens.FirstOrDefault(x => x.WorkingArea.Contains(Cursor.Position));
                if (screen == null) return;

                var bounds = screen.Bounds;

                // Resolución de referencia (donde el diseño se ve correcto)
                const float RefWidth = 1920f;
                const float RefHeight = 1080f;

                // Factores de escala basados en la resolución actual
                float scaleX = bounds.Width / RefWidth;
                float scaleY = bounds.Height / RefHeight;

                // Factor de escala DPI
                float dpiScale = GetDpiScale();

                // Valores base calibrados para 1920x1080 al 100%
                const int BasePanelWidth = 1392;
                const int BasePanelHeight = 50;
                const int BasePanelX = 5;
                const int BasePanelY = 108;
                const int BaseDropdownWidth = 217;
                const int BaseDropdownHeight = 25;
                const int BaseDropdownY = 54;
                const int BaseViewDropdownX = 1177;
                const int BaseDvfsX = 941;

                // Panel context bar escalado proporcionalmente
                int panelWidth = Clamp((int)(BasePanelWidth * scaleX / dpiScale), 300, bounds.Width - 50);
                int panelHeight = Clamp((int)(BasePanelHeight * scaleY / dpiScale), 25, 80);
                int panelX = Math.Max(3, (int)(BasePanelX * scaleX));
                int panelY = (int)(BasePanelY * scaleY / dpiScale);

                _panelContextBar.Size = new Size(panelWidth, panelHeight);
                _panelContextBar.Location = new Point(panelX, panelY);
                ShowContextbar();

                // Dropdown escalado proporcionalmente
                int dropdownY = (int)(BaseDropdownY * scaleY / dpiScale);
                int dropdownWidth = Clamp((int)(BaseDropdownWidth * scaleX / dpiScale), 80, 300);
                int dropdownHeight = Clamp((int)(BaseDropdownHeight * scaleY / dpiScale), 18, 40);
                var dropdownSize = new Size(dropdownWidth, dropdownHeight);

                int viewDropdownX = (int)(BaseViewDropdownX * scaleX / dpiScale);
                _viewDropdown.Location = new Point(viewDropdownX, dropdownY);
                _viewDropdown.Size = dropdownSize;

                if (_card.dvfs != null && _card.dvfs.Length > 0)
                {
                    _dvfsRelated.Location = _viewDropdown.Location;
                    int dvfsX = (int)(BaseDvfsX * scaleX / dpiScale);
                    _viewDropdown.Location = new Point(dvfsX, dropdownY);
                    _dvfsRelated.Size = dropdownSize;
                }

                this.SetSizes();
            }
        }

        private float GetDpiScale()
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return 1.0f; // Valor por defecto si el control no está listo
            }

            try
            {
                using (var g = this.CreateGraphics())
                {
                    return g.DpiX / 96f;
                }
            }
            catch (ObjectDisposedException)
            {
                return 1.0f;
            }
        }

        private static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public void ShowSnapshot()
        {
            if (this.Snapshot != null)
            {
                var control = new ElementSnapshotControl(this.Snapshot);
                control.Dock = DockStyle.Fill;
                base.AddElementToPanel(control);
            }
        }

        public async void ShowLive()
        {
            if (mode == "VCA" && _cameraDTO == null && this.dvfs.Count > 0)
            {
                var dvfId = Convert.ToInt32(this.dvfs[0].Key);
                _cameraDTO = await _viewModel.GetCamera(dvfId);
            }

            if (mode == "DI" && this.dvfs.Count > 0)
            {
                var dvfId = Convert.ToInt32(this.dvfs[0].Key);
                _cameraDTO = await _viewModel.GetCamera(dvfId);
            }

            if (_cameraDTO == null)
            {
                NoRecordingMessage();
                return;
            }
            var controlCamera = new ElementCameraControl(null, _cameraDTO, Profile.MainStream, true, nameTab);
            controlCamera.Name = "elementCameraControl";
            controlCamera.Dock = DockStyle.Fill;
            base.AddElementToPanel(controlCamera);
            var contextBar = new ContextBarControl(false);
            contextBar.Visible = true;
            contextBar.Width = controlCamera.Width + 5;
            contextBar.ClearButtons();

            var driver = controlCamera.Controls[0] as IDriverLive;
            driver.OnInitializeAudio += Driver_OnInitializeAudio;

            var commands = new List<ButtonsContextBar>();
            commands = driver.Commands;
            commands.AddRange(driver.CommandsAudioPtz);
            foreach (var b in commands)
            {
                PermissionLive permissionLive = b.GetAttribute<PermissionLive>();
                if (permissionLive != null && appAuthorization.Exist(permissionLive.PermissionKey))
                    contextBar.AddButton(b, Icons.GetDefaultIconForButton(b));
            }

            contextBar.ButtonClicked += ContextBar_ButtonClicked;
            this._panelContextBar.Controls.Add(contextBar);
            this._panelContextBar.Visible = true;
            this._dvfsRelated.SelectedValueChanged += new System.EventHandler(this.DVFS_SelectedValueChanged);
        }

        private void Driver_OnInitializeAudio(object sender, bool audio)
        {
            if (this.IsDisposed || this.Disposing)
                return;

            var commands = (sender as IDriverLive).CommandsAudioPtz;
            foreach (var b in commands)
            {
                PermissionLive permissionLive = b.GetAttribute<PermissionLive>();
                if (permissionLive != null && appAuthorization.Exist(permissionLive.PermissionKey))
                {
                    if (b == ButtonsContextBar.Listen)
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                if (!this.IsDisposed)
                                    ContextBarButtonClick?.Invoke(this, ButtonsContextBar.Listen);
                            }));
                        }
                        else
                        {
                            ContextBarButtonClick?.Invoke(this, ButtonsContextBar.Listen);
                        }
                    }
                }
            }
        }

        private void ShowgMaps()
        {
            CreatePanel("KPI");
        }

        private void NoRecordingMessage()
        {
            labelWithOutResult = new Label()
            {
                Location = new Point(13, 100),
                Name = "lblWithResult",
                Size = new Size(this.Width - 40, this.Height - 150),
                Margin = new Padding(10, 24, 0, 0),
                Text = string.Format(Resources.NoRecordingAvailable, "Video"),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = FontHelper.GetRobotoRegular(FontSizes.Medium_5, FontStyle.Regular, GraphicsUnit.Pixel),
                ForeColor = Color.White,
                Visible = true
            };
            labelWithOutResult.Dock = DockStyle.Fill;
            base.AddElementToPanel(labelWithOutResult);
            return;
        }

        private async void ShowIstanPlayback()
        {
            if (mode == "VCA" && _cameraDTO == null)
            {
                var dvfId = Convert.ToInt32(this.dvfs[0].Key);
                _cameraDTO = await _viewModel.GetCamera(dvfId);
            }
            InstantPlayerControl instantPlayerControl = null;
            PlaybackViewModel playbackModel = null;
            MainViewModel main = null;
            if (_cameraDTO == null)
            {
                NoRecordingMessage();
                return;
            }
            _instantPlaybackDriver = DriverFactory.GetDriverLive(_cameraDTO, Profile.SubStream, false, nameTab);
            if (instantPlayerControl == null)
            {
                instantPlayerControl = new InstantPlayerControl();
                instantPlayerControl.Dock = DockStyle.Fill;
                main = Locator.Current.GetService<IScreen>() as MainViewModel;
                playbackModel = new PlaybackViewModel
                {
                    Parent = main.ID,
                    MainView = main
                };
            }
            if (_cameraDTO == null)
                return;

            _instantPlaybackDriver.ToggleInstantPlayback();
            var recorders = Locator.Current.GetService<ICatalogService>().GetRecorders(_cameraDTO);
            //instantPlayerControl.SetRecorders(recorders);

            var Element = new SidebarElementDTO()
            {
                ElementId = _cameraDTO.Id,
                Name = _cameraDTO.Name,
                Status = DeviceStatus.Online,
                //DeviceType = currentCamera.ChannelType,
                GroupName = _cameraDTO.SiteName,
                SiteId = _cameraDTO.SiteId,
                DeviceTypeStr = _cameraDTO.ChannelType.ToString(),
                Key = Guid.NewGuid(),
            };

            instantPlayerControl.SetCamera(_cameraDTO, _timeAction.AddMinutes(5), Element, false, recorders, true);
            instantPlayerControl.ViewModel = playbackModel;
            base.AddElementToPanel(instantPlayerControl);
            this._dvfsRelated.SelectedValueChanged += new System.EventHandler(this.DVFS_SelectedValueChanged);
            instantPlayerControl.ListRecordingOff();
        }

        private void ContextBar_ButtonClicked(ButtonsContextBar button)
        {
            ContextBarButtonClick?.Invoke(this, button);
        }

        private string currentState;
        private void ExecuteOptionSelected(OptionObjectDTO option)
        {
            if (option == null) return;

            base.AddElementToPanel(null);
            switch (option.Key)
            {
                case "snapshot":
                    _dvfsRelated.Enabled = false;
                    ShowSnapshot();
                    this._dvfsRelated.SelectedValueChanged -= new System.EventHandler(this.DVFS_SelectedValueChanged);
                    break;
                case "live":
                    this._dvfsRelated.Enabled = true;
                    ShowLive();
                    break;
                case "geolocalizacion":
                    _dvfsRelated.Enabled = false;
                    ShowgMaps();
                    this._dvfsRelated.SelectedValueChanged -= new System.EventHandler(this.DVFS_SelectedValueChanged);
                    break;
                case "instantPlayback":
                    this._dvfsRelated.Enabled = true;
                    ShowIstanPlayback();
                    break;
                default:
                    break;
            }
            currentState = option.Key;

        }
        private void ViewDropdown_SelectedValueChanged(object sender, EventArgs e)
        {
            OptionObjectDTO option = (OptionObjectDTO)this._viewDropdown.SelectedItem;
            ClearPanel();
            ExecuteOptionSelected(option);
        }

        private void ButtonIstanPlayback_Click(object sender, EventArgs e)
        {
            this._dvfsRelated.Enabled = true;
            ShowIstanPlayback();
        }

        private void ClearPanel()
        {
            foreach (var ctl in _panelContextBar.Controls)
            {
                if (ctl is ContextBarControl)
                    (ctl as ContextBarControl).Dispose();
            }
        }

        private async void DVFS_SelectedValueChanged(object sender, EventArgs e)
        {
            this._dvfsRelated.SelectedValueChanged -= new System.EventHandler(this.DVFS_SelectedValueChanged);
            OptionObjectDTO option = (OptionObjectDTO)this._dvfsRelated.SelectedItem;
            int dvfId = Convert.ToInt32(option.Key);
            _cameraDTO = await _viewModel.GetCamera(dvfId);
            mode = _cameraDTO.ChannelType.ToString();
            switch (currentState)
            {
                case "live":
                    ShowLive();
                    break;
                case "instantPlayback":
                    ShowIstanPlayback();
                    break;
            }
        }

        private void AlarmDiagnosticControl_Load(object sender, EventArgs e)
        {
            this.LoadStep();
            this._viewDropdown.SelectedValueChanged += new System.EventHandler(this.ViewDropdown_SelectedValueChanged);
            OptionObjectDTO option = (OptionObjectDTO)this._viewDropdown.SelectedItem;
            ExecuteOptionSelected(option);
        }

        private void ShowContextbar()
        {
            var panelCamera = base.Controls.Find("PanelCamera", true).FirstOrDefault() as Panel;
            ElementCameraControl controlCamera = panelCamera.Controls.Find("elementCameraControl", true).FirstOrDefault() as ElementCameraControl;
            if (controlCamera == null)
                return;
            var contextBar = this._panelContextBar.Controls.Find("ContextBarControl", true).FirstOrDefault() as ContextBarControl;
            contextBar.Visible = true;
            contextBar.Width = controlCamera.Width + 5;
            contextBar.ClearButtons();

            var driver = controlCamera.Controls[0] as IDriverLive;

            var commands = new List<ButtonsContextBar>();
            commands = driver.Commands;
            commands.AddRange(driver.CommandsAudioPtz);
            foreach (var b in commands)
            {
                PermissionLive permissionLive = b.GetAttribute<PermissionLive>();
                if (permissionLive != null && appAuthorization.Exist(permissionLive.PermissionKey))
                    contextBar.AddButton(b, Icons.GetDefaultIconForButton(b));
            }
        }
    }
}
