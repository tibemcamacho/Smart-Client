using Bunifu.Framework.UI;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.UserControls.Outputs;
using Elipgo.SmartClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ContextBar
{
    public delegate void ButtonClicked(ButtonsContextBar button);
    public delegate void SelectedValueChanged(object sender, EventArgs e);
    public delegate void RecorderSelectedValueChanged(object sender, RecorderDTOSmall recorder);
    public delegate void MouseHover(object sender, EventArgs e);

    public partial class ContextBarControl : UserControl
    {
        public event ButtonClicked ButtonClicked;
        public event SelectedValueChanged DropDownValueChange;
        public event RecorderSelectedValueChanged RecorderDropDownValueChange;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public event MouseHover MouseHover;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        public event EventHandler<Profile> ProfileStreamDropDownValueChange;

        private readonly List<Control> _buttons = new List<Control>();
        private List<OptionObjectDTO> _profilesStreamList = new List<OptionObjectDTO>();
        private Profile _profileSelected = Profile.None;
        private RecorderDTOSmall _recorderSelected = new RecorderDTOSmall();
        private Bunifu.UI.WinForms.BunifuDropdown _dropdownProfileStream = null;
        private int _nextButtonPosition;
        private bool _painted = false;
        private bool _isFullScreen = false;
        private bool _resizeLoad = false;

        public ContextBarControl()
        {
            InitializeComponent();
            _resizeLoad = true;
            _removeButton.Image = FileResources.icon_close;
            _removeButton.Click += ImageButton_Click;

            _elementName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
            _groupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
            _removeButton.Cursor = Cursors.Hand;
            _bunifuToolTip.AllowAutoClose = true;
            // Units for time are in milliseconds.
            _bunifuToolTip.AutoCloseDuration = 2000;

            //bunifuToolTip1.Closed += BunifuToolTip1_Closed;
            this._nextButtonPosition = _removeButton.Left;

            CultureInfo ci = CultureInfo.InstalledUICulture;

            _bunifuToolTip.SetToolTip(this._removeButton, ci.Name.Contains("es") ? ButtonsContextBar.Remove.GetDescription() : ButtonsContextBar.Remove.GetAttribute<DescriptionEN>().Descripcion);

            this.Resize += ContextBarControl_Resize;
            this.Paint += ContextBarControl_Paint;
        }

        private void ButtonClose_MouseHover(object sender, EventArgs e)
        {
            MouseHover?.Invoke(sender, e);
        }

        private void ButtonClose_MouseLeave(object sender, EventArgs e)
        {
            MouseHover?.Invoke(sender, e);
        }

        private void ContextBarControl_Paint(object sender, PaintEventArgs e)
        {
            if (_painted)
            {
                return;
            }

            _painted = true;
            ContextBarControl_Resize();
            Screen primaryScreen = Screen.PrimaryScreen;
            if (!Screen.AllScreens.Any(s => s.WorkingArea.Contains(Cursor.Position)))
            {
                return;
            }

            Rectangle workingArea = Screen.GetWorkingArea(this);
            if (workingArea.Width > 2000 && !_isFullScreen)
            {
                _elementName.Left = _iconElement.Left + _iconElement.Width + 40;
                _groupName.Left = _iconElement.Left + _iconElement.Width + 40;
                _elementNvrName.Left = _iconElement.Left + _iconElement.Width + 40;

                if (primaryScreen != null && primaryScreen.Primary && primaryScreen.DeviceName != Screen.AllScreens[0].DeviceName)
                {
                    if (primaryScreen.WorkingArea.Width >= 1900 && primaryScreen.WorkingArea.Width <= 2000)
                    {
                        _elementName.Top = 20;
                        _groupName.Top = 5;
                        _elementNvrName.Top = 5;
                    }

                }
                //2049 x 1280  125%
                else if (workingArea.Width == 2048 && workingArea.Height == 1232)
                {
                    _elementName.Top = 5;
                    _groupName.Top = 40;
                }
                else
                {
                    _elementName.Top = 5;
                    _groupName.Top = 40;
                }

                _elementNvrName.Top = 100;
            }

        }

        private void ContextBarControl_Resize(object sender, EventArgs e)
        {
            ContextBarControl_Resize();
        }

        public ContextBarControl(bool contextBarDefault = false) : this()
        {
            if (!contextBarDefault)
            {
                _elementName.Visible = false;
                _groupName.Visible = false;
            }
        }

        public string GetElementName()
        {
            return _elementName.Text;
        }

        private void BunifuToolTip1_Closed(object sender, EventArgs e)
        {

        }

        public void AddButton(ButtonsContextBar button, Control control, int positionY = 12)
        {
            int currentPosition = this._nextButtonPosition - _removeButton.Width;
            positionY = _removeButton.Top;

            control.Name = button.ToString();
            if (!_buttons.Exists(ctl => ctl.Name == control.Name))
            {
                currentPosition -= control.Width;
                control.Location = new Point(currentPosition, positionY);
                this._nextButtonPosition = currentPosition;
                control.Visible = true;

                control.Click += ImageButton_Click;
                control.Cursor = Cursors.Hand;

                this.Controls.Add(control);
                _buttons.Add(control);

                CultureInfo ci = CultureInfo.InstalledUICulture;

                if (ci.Name.Contains("es"))
                {
                    _bunifuToolTip.SetToolTip(control, button.GetDescription());
                    _bunifuToolTip.SetToolTipIcon(control, null);
                    this._bunifuToolTip.SetToolTipTitle(control, "");
                }
                else
                {
                    _bunifuToolTip.SetToolTip(control, button.GetAttribute<DescriptionEN>().Descripcion);
                    _bunifuToolTip.SetToolTipIcon(control, null);
                    this._bunifuToolTip.SetToolTipTitle(control, "");
                }
            }
            ContextBarControl_Resize();
        }

        public void AddButton(ButtonsContextBar button, Image image = null, string type = "imageButton")
        {
            ContextBarControl_Resize();
            int currentPosition = this._nextButtonPosition - _removeButton.Width;

            Control control;
            switch (type)
            {
                case "switchButton":
                    control = new BunifuiOSSwitch()
                    {
                        OnColor = ColorTranslator.FromHtml(VariableResources.COLOR_CONTEXT),
                        OffColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND)
                    };
                    break;
                default:
                    control = new BunifuImageButton()
                    {
                        Image = image,
                        Zoom = 10
                    };
                    break;
            }

            control.Name = button.ToString();
            if (!_buttons.Exists(ctl => ctl.Name == control.Name))
            {
                control.Size = _removeButton.Size;
                currentPosition -= _removeButton.Width;
                control.Location = new Point(currentPosition, _removeButton.Top);
                this._nextButtonPosition = currentPosition;
                control.Visible = true;

                control.Click += ImageButton_Click;
                control.Cursor = Cursors.Hand;
                control.Anchor = AnchorStyles.Right;
                this.Controls.Add(control);
                _buttons.Add(control);

                CultureInfo ci = CultureInfo.InstalledUICulture;

                if (ci.Name.Contains("es"))
                {
                    _bunifuToolTip.SetToolTip(control, button.GetDescription());
                    _bunifuToolTip.SetToolTipIcon(control, null);
                    this._bunifuToolTip.SetToolTipTitle(control, "");
                }
                else
                {
                    _bunifuToolTip.SetToolTip(control, button.GetAttribute<DescriptionEN>().Descripcion);
                    _bunifuToolTip.SetToolTipIcon(control, null);
                    this._bunifuToolTip.SetToolTipTitle(control, "");
                }
            }
        }

        public void AddOutputToggleButton(LiveViewModel viewModel, int siteId, int cameraId)
        {
            var b = this.Controls.Find("OutputToggleButton", false);
            if (b.Length > 0)
            {
                this.Controls.Remove(b[0]);
            }

            List<CatalogIot> catalog = viewModel.GetCatalogIOTs(siteId, cameraId);
            if (catalog.Count == 0)
            {
                return;
            }

            int currenPosition = this._nextButtonPosition - _removeButton.Width;

            var control = new OutputToggleButton(catalog, viewModel)
            {
                Name = "OutputToggleButton"
            };
            control.Button.Image = FileResources.output;
            control.Size = _removeButton.Size;
            control.Button.Size = _removeButton.Size;
            currenPosition -= _removeButton.Width;
            control.Location = new Point(currenPosition, _removeButton.Top);
            this._nextButtonPosition = currenPosition;

            control.Cursor = Cursors.Hand;
            _buttons.Add(control);
            this.Controls.Add(control);

            control.Button.MouseEnter += OutputToggleButton_MouseEnter;
            control.Button.MouseLeave += OutputToggleButton_MouseLeave;
        }

        public void RemoveOutputToggleButton()
        {
            var b = this.Controls.Find("OutputToggleButton", false);
            if (b.Length > 0)
            {
                OutputToggleButton control = (b[0] as OutputToggleButton);
                control.Button.MouseEnter -= OutputToggleButton_MouseEnter;
                control.Button.MouseLeave -= OutputToggleButton_MouseLeave;
                control.Dispose();
                this.Controls.Remove(control);
            }
        }

        private void OutputToggleButton_MouseLeave(object sender, EventArgs e)
        {
            _bunifuToolTip.Hide();
        }

        private void OutputToggleButton_MouseEnter(object sender, EventArgs e)
        {
            var b = this.Controls.Find("OutputToggleButton", false);
            if (b.Length > 0)
            {
                OutputToggleButton control = (b[0] as OutputToggleButton);
                CultureInfo ci = CultureInfo.InstalledUICulture;
                _bunifuToolTip.Show(control, ci.Name.Contains("es") ? ButtonsContextBar.IoDescription.GetDescription() : ButtonsContextBar.IoDescription.GetAttribute<DescriptionEN>().Descripcion, "", null, new Point(1300, 138));
            }
        }

        public void AddProfileStreamDropdown(ButtonsContextBar button, Profile profile, List<Profile> profileList)
        {
            var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
            int width = 217;
            if (workingArea.Width == 1024 && workingArea.Height == 768)
            {
                width = 117;
            }
            _dropdownProfileStream = new Bunifu.UI.WinForms.BunifuDropdown
            {
                BackColor = SystemColors.Control,
                BorderRadius = 1,
                Color = Color.Transparent,
                Cursor = Cursors.Hand,
                Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down,
                DisabledColor = Color.Gray,
                DisplayMember = "Name",
                DrawMode = DrawMode.OwnerDrawFixed,
                DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left,
                FillDropDown = false,
                FillIndicator = false,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                FormattingEnabled = true,
                Icon = null,
                IndicatorColor = Color.White,
                IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right,
                ItemBackColor = Color.DimGray,
                ItemBorderColor = Color.DimGray,
                ItemForeColor = Color.White,
                ItemHeight = 26,
                ItemHighLightColor = Color.Gray,
                Location = new Point(24, 24),
                Margin = new Padding(2),
                Name = "dropdownStream",
                Size = new Size(width, _removeButton.Height - 2),
                TabIndex = 6,
                Text = null,
                ValueMember = "Key"
            };

            if (!_buttons.Exists(ctl => ctl.Name == "dropdownStream"))
            {
                int currentPosition = _nextButtonPosition - _removeButton.Width;
                List<OptionObjectDTO> listStream = new List<OptionObjectDTO>();
                listStream.Add(new OptionObjectDTO
                {
                    Key = "-2",
                    Name = "Automatico"
                });
                foreach (Profile p in profileList)
                {
                    listStream.Add(new OptionObjectDTO
                    {
                        Key = p.GetDescription(),
                        Name = Resources.ResourceManager.GetString(p.ToString())
                    });
                }
                _profilesStreamList = listStream;
                _dropdownProfileStream.DataSource = new BindingSource(_profilesStreamList, null);
                _dropdownProfileStream.DisplayMember = "Name";
                _dropdownProfileStream.ValueMember = "Key";
                _dropdownProfileStream.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                currentPosition -= width;
                _dropdownProfileStream.Location = new Point(currentPosition, _removeButton.Top);
                _dropdownProfileStream.Anchor = AnchorStyles.Right;
                _nextButtonPosition = currentPosition;
                _dropdownProfileStream.Visible = true;
                _profileSelected = profile;
                Controls.Add(_dropdownProfileStream);
                _buttons.Add(_dropdownProfileStream);
                _dropdownProfileStream.SelectedValue = profile;
                _dropdownProfileStream.Text = Resources.ResourceManager.GetString(profile.ToString());
                _dropdownProfileStream.SelectedValueChanged += ProfileStreamDropdown_SelectedValueChanged;
            }
        }

        public void ToggleProfileDropdown(bool enabled)
        {
            if (_dropdownProfileStream != null)
            {
                _dropdownProfileStream.Enabled = enabled;
            }
        }

        private void ProfileStreamDropdown_SelectedValueChanged(object sender, EventArgs e)
        {
            //if ((string)_dropdownProfileStream.SelectedValue == "-2")
            //{
            //    return;
            //}
            _profileSelected = (Profile)Enum.Parse(typeof(Profile), (string)_dropdownProfileStream.SelectedValue);
            ProfileStreamDropDownValueChange?.Invoke(sender, _profileSelected);
        }

        public void AddDropDownControl(ButtonsContextBar button, List<OptionObjectDTO> listStream, int width, int selectedIndex)
        {
            if (_buttons.Exists(ctl => ctl.Name == button.ToString()))
            {
                return;
            }

            Bunifu.UI.WinForms.BunifuDropdown dropdown = new Bunifu.UI.WinForms.BunifuDropdown
            {
                BackColor = SystemColors.Control,
                BorderRadius = 1,
                Color = Color.Transparent,
                Cursor = Cursors.Hand,
                Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down,
                DisabledColor = Color.Gray,
                DisplayMember = "Name",
                DrawMode = DrawMode.OwnerDrawFixed,
                DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin,
                DropDownStyle = ComboBoxStyle.DropDownList,
                DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left,
                FillDropDown = false,
                FillIndicator = false,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                FormattingEnabled = true,
                Icon = null,
                IndicatorColor = Color.White,
                IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right,
                ItemBackColor = Color.DimGray,
                ItemBorderColor = Color.DimGray,
                ItemForeColor = Color.White,
                ItemHeight = 26,
                ItemHighLightColor = Color.Gray,
                Location = new Point(24, 24),
                Margin = new Padding(2),
                Name = button.ToString(),
                Size = new Size(217, _removeButton.Height - 2),
                TabIndex = 6,
                Text = null,
                ValueMember = "Key"
            };
            CultureInfo ci = CultureInfo.InstalledUICulture;
            int currenPosition = this._nextButtonPosition - _removeButton.Width;

            dropdown.DataSource = new BindingSource(listStream, null);
            dropdown.DisplayMember = "Name";
            dropdown.ValueMember = "Key";
            dropdown.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
            dropdown.Visible = true;
            dropdown.Size = new Size(width, _removeButton.Height - 2);
            currenPosition -= width;
            dropdown.Location = new Point(currenPosition, _removeButton.Top);
            this._nextButtonPosition -= currenPosition;
            dropdown.Visible = true;
            this.Controls.Add(dropdown);
            _buttons.Add(dropdown);
            _bunifuToolTip.SetToolTip(dropdown, ci.Name.Contains("es") ? button.GetDescription() : button.GetAttribute<DescriptionEN>().Descripcion);
            dropdown.SelectedValueChanged += Dropdown_SelectedValueChanged;
            if (listStream.Count > 0)
            {
                dropdown.Text = listStream[selectedIndex].Name;
            }
            else
            {
                dropdown.Text = ci.Name.Contains("es") ? button.GetDescription() : button.GetAttribute<DescriptionEN>().Descripcion;
            }
        }

        private void Dropdown_SelectedValueChanged(object sender, EventArgs e)
        {
            if (sender is Bunifu.UI.WinForms.BunifuDropdown)
            {
                if ((sender as Bunifu.UI.WinForms.BunifuDropdown).Name == "dropdownStream")
                {
                    _profileSelected = (Profile)Enum.Parse(typeof(Profile), (string)this._dropdownProfileStream.SelectedValue);
                    ProfileStreamDropDownValueChange?.Invoke(sender, _profileSelected);
                    return;
                }
            }
            DropDownValueChange?.Invoke(sender, e);
        }

        public void AddRecorderDropDown(ButtonsContextBar button, int cameraId, List<RecorderDTOSmall> recorders, RecorderDTOSmall currentRecorder, bool verifyStatus)
        {
            if (_buttons.Exists(ctl => ctl.Name == button.ToString()))
            {
                return;
            }
            RecorderDropDown.RecorderDropDown dropdown = new RecorderDropDown.RecorderDropDown(cameraId, recorders, currentRecorder, verifyStatus);

            int currentPosition = this._nextButtonPosition - _removeButton.Width;
            currentPosition -= dropdown.Width;

            dropdown.Location = new Point(currentPosition, _removeButton.Top);
            dropdown.Visible = true;
            dropdown.SelectedValueChanged += RecorderDropdown_SelectedValueChanged;

            this._nextButtonPosition -= currentPosition;

            this.Controls.Add(dropdown);
            _buttons.Add(dropdown);
        }

        private void RecorderDropdown_SelectedValueChanged(object sender, RecorderDTOSmall recorder)
        {
            _recorderSelected = recorder;
            RecorderDropDownValueChange?.Invoke(sender, _recorderSelected);
        }

        public void ChangeImageButton(ButtonsContextBar button, Image image)
        {
            var b = this.Controls.Find(button.ToString(), false);
            if (b.Length == 0)
            {
                return;
            }

            var but = b[0] as BunifuImageButton;
            but.Image = image;
        }

        private void ImageButton_Click(object sender, EventArgs e)
        {
            _bunifuToolTip.Hide();
            Enum.TryParse((sender as Control).Name, out ButtonsContextBar button);
            if (button != ButtonsContextBar.None)
            {
                ButtonClicked(button);
            }
        }

        public void ClearButtons()
        {
            try
            {
                _bunifuToolTip.RemoveAll();
                _bunifuToolTip.Dispose();
                foreach (var b in _buttons)
                {
                    switch (b)
                    {
                        case OutputToggleButton control:
                            control.Button.MouseEnter -= OutputToggleButton_MouseEnter;
                            control.Button.MouseLeave -= OutputToggleButton_MouseLeave;
                            break;
                        case Bunifu.UI.WinForms.BunifuDropdown dropdown:
                            dropdown.SelectedValueChanged -= ProfileStreamDropdown_SelectedValueChanged;
                            break;
                        case BunifuiOSSwitch outputToggleSwitch:
                            outputToggleSwitch.Click -= OutputToggleSwitch_Click;
                            outputToggleSwitch.Click -= ImageButton_Click;
                            break;
                        case BunifuImageButton button:
                            if ((b as BunifuImageButton).Image != null)
                            {
                                (b as BunifuImageButton).Image.Dispose();
                            }
                            button.Click -= ImageButton_Click;
                            break;
                    }

                    b.Dispose();
                    this.Controls.Remove(b);
                }
                _buttons.Clear();
                this._nextButtonPosition = _removeButton.Left;
                this._painted = false;
            }
            catch (Exception)
            {

            }
        }

        public void SetValues(SidebarElementDTO element, Image iconElement, int startPosition = 0, bool isFullScreem = false)
        {
            this._isFullScreen = isFullScreem;
            _iconElement.Image = iconElement;

            if (element.ShowDvfId)
            {
                _elementName.Text = $"{element.Name.ToUpper()} - ID: {element.ElementId}";
            }
            else
            {
                _elementName.Text = element.Name.ToUpper();
            }

            if (string.IsNullOrEmpty(element.RecorderName))
            {
                //this.ElementName.Location = new System.Drawing.Point(77, 14);
                //this.GroupName.Location = new System.Drawing.Point(77, 30);
                this._groupName.Text = element.GroupName?.ToUpper();
                this._elementNvrName.Visible = false;
            }
            else
            {
                this._elementNvrName.Visible = false;
                switch (element.DeviceType)
                {
                    case ElementType.AlarmsMap:
                        this._groupName.Text = $"{Resources.AlarmsGeo.ToUpper()} - {element.RecorderName.ToUpper()}";
                        break;
                    case ElementType.Geolocation_Alarm:
                        this._groupName.Text = element.RecorderName.ToUpper();
                        break;
                    default:
                        this._groupName.Text = $"{Resources.DeviceName.ToUpper()} - {element.RecorderName.ToUpper()}";
                        break;
                }
            }

            if (startPosition > 0)
            {
                this._nextButtonPosition = startPosition;
            }
        }

        public void AddOutputToggleSwitch(string labelState, IOPortState state)
        {
            var outputToggleSwitch = new BunifuiOSSwitch
            {
                BackColor = Color.Transparent,
                BackgroundImageLayout = ImageLayout.Stretch,
                Cursor = Cursors.Hand,
                Location = new Point(_removeButton.Left - 48, _removeButton.Top),
                Name = "outputToggleSwitch",
                OffColor = ColorTranslator.FromHtml(VariableResources.COLOR_SWITCH_OFF),
                OnColor = ColorTranslator.FromHtml(VariableResources.COLOR_SWITCH_ON),
                Size = new Size(48, _removeButton.Height),
                Value = state == IOPortState.Active,
            };
            outputToggleSwitch.Click += OutputToggleSwitch_Click;

            this.Controls.Add(outputToggleSwitch);

            var outputLabel = new Label
            {
                Name = "outputLabel",
                Text = labelState,
                Size = new Size(100, _removeButton.Height),
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(_removeButton.Left - 48 - 110, _removeButton.Top),
                Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Bold, GraphicsUnit.Pixel),
                ForeColor = Color.White
            };
            this.Controls.Add(outputLabel);

            _buttons.Add(outputLabel);
            _buttons.Add(outputToggleSwitch);
        }

        private void OutputToggleSwitch_Click(object sender, EventArgs e)
        {
            ButtonClicked(ButtonsContextBar.OutputToggleSwitch);
        }

        public void AddInputState(string labelState, Image iconState)
        {
            var inputLabel = new Label
            {
                Name = "inputLabel",
                Text = labelState,
                Size = new Size(100, _removeButton.Height),
                TextAlign = ContentAlignment.MiddleRight,
                Location = new Point(_removeButton.Left - 24 - 110, _removeButton.Top),
                Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Bold, GraphicsUnit.Pixel),
                ForeColor = Color.White
            };
            this.Controls.Add(inputLabel);

            var inputStatePictureBox = new PictureBox
            {
                Size = _removeButton.Size,
                Location = new Point(_removeButton.Left - 24, _removeButton.Top),
                Name = "inputStatePictureBox",
                BackgroundImage = iconState
            };

            this.Controls.Add(inputStatePictureBox);

            _buttons.Add(inputLabel);
            _buttons.Add(inputStatePictureBox);
        }

        public void SetLevelText(string textLevel, string rangeText, bool isVisible = true)
        {
            this._levelContext.Text = textLevel;
            this._levelContext.Visible = isVisible;
            this._rangeContext.Text = rangeText;
            this._rangeContext.Visible = isVisible;
        }

        public void ContextBarControl_Resize()
        {
            if (!Screen.AllScreens.Any(s => s.WorkingArea.Contains(Cursor.Position)))
            {
                return;
            }

            Screen primaryScreen = Screen.PrimaryScreen;
            if (primaryScreen != null && primaryScreen.Primary && primaryScreen.DeviceName != Screen.AllScreens[0].DeviceName)
            {
                // Esta sección debe ejecutarse cuando la pantalla principal seleccionada es diferente a la pantalla del PC/Laptop
                // es decir, cuando hay una pantalla extra conectada y es seleccionada como pantalla principal.
                if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
                {
                    if (primaryScreen.WorkingArea.Width >= 1900 && primaryScreen.WorkingArea.Width <= 2000)
                    {
                        var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                        int height = 35;
                        int top = 20;
                        int radius = 30;

                        _elementName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                        _groupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                        _levelContext.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);

                        _removeButton.Size = new Size(height, height);
                        _removeButton.Top = top;
                        var RemoteWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.987M), 2));
                        if (main.Width > 1700)
                        {
                            _removeButton.Left = Convert.ToInt32(this.Width * (0.97));
                            var iconElementWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * (0.0409M)), 2));
                            _iconElement.Location = new Point(iconElementWidth, _removeButton.Top);
                        }
                        else if (main.Width > 1400 && main.Width <= 1600)
                        {
                            _removeButton.Left = Convert.ToInt32(main.Width * (0.8243));
                            var iconElementWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * (0.0156M)), 2));
                            _iconElement.Location = new Point(iconElementWidth, _removeButton.Top);
                        }
                        else if (main.Width <= 1400)
                        {
                            _removeButton.Left = Convert.ToInt32(this.Width * (0.95));
                            var iconElementWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * (0.0156M)), 2));
                            _iconElement.Location = new Point(iconElementWidth, _removeButton.Top);
                        }
                        else
                        {
                            _removeButton.Left = Convert.ToInt32(this.Width * (0.92));
                            var iconElementWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * (0.0156M)), 2));
                            _iconElement.Location = new Point(iconElementWidth, _removeButton.Top);
                        }

                        //_removeButton.Left = Convert.ToInt32(this.Width * (main.Width > 1700 ? 0.97 : (main.Width < 1400 ? 0.77 : 0.98)));

                        int separator = 0;
                        _iconElement.Size = _removeButton.Size;
                        //_iconElement.Top = _removeButton.Top;

                        var elementNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * (main.Width > 1700 ? 0.061M : 0.0343M)), 2));
                        var elementNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.001M), 2));
                        _elementName.Location = new Point(elementNameX, elementNameY);

                        var groupNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * (main.Width > 1700 ? 0.061M : 0.0343M)), 2));
                        var groupNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));
                        _groupName.Location = new Point(groupNameX, groupNameY);

                        var levelContextX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.091M), 2));
                        var levelContextY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.001M), 2));
                        _levelContext.Location = new Point(levelContextX, _removeButton.Top);

                        var nameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.028M), 2));

                        _elementName.Height = nameHeight;
                        _groupName.Height = nameHeight;
                        _levelContext.Height = nameHeight;

                        var panelMenuWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.020M), 2));
                        var panelMenuHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.037M), 2));
                        //panelMenu.Size = new Size(panelMenuWidth, panelMenuHeight);

                        var panelMenuX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.005M), 2));
                        //panelMenu.Location = new Point(0, panelMenuX);

                        _buttons.ForEach(button =>
                        {
                            switch (button)
                            {
                                case OutputToggleButton control:
                                    control.Size = _removeButton.Size;
                                    control.Top = _removeButton.Top;
                                    break;
                                case Bunifu.UI.WinForms.BunifuDropdown control:
                                    control.Height = _removeButton.Height;
                                    control.Top = _removeButton.Top;
                                    break;
                                case BunifuiOSSwitch control:
                                    control.Height = _removeButton.Height;
                                    control.Top = _removeButton.Top;
                                    break;
                                case BunifuImageButton control:
                                    control.Size = _removeButton.Size;
                                    control.Top = _removeButton.Top;
                                    break;
                                case Bunifu.UI.WinForms.BunifuButton.BunifuButton control:
                                    control.Height = _removeButton.Height + 5;
                                    int currenPosition = _removeButton.Location.X - (control.Width + height);
                                    int Yposition = _removeButton.Top;
                                    control.Location = new Point(currenPosition, Yposition);
                                    control.IdleBorderRadius = radius;
                                    control.OnIdleState.BorderRadius = radius;
                                    break;
                            }
                            separator += height * 2;
                        });
                    }
                }
            }
            else
            {
                if ((_resizeLoad || Screen.AllScreens.Length >= 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
                {
                    var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                    int height = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.023M), 2));
                    var widthContextBar = this.Width;
                    int top = 0;
                    int radius = 30;
                    if (workingArea.Width > 1400 && workingArea.Width < 2000)
                    {
                        _elementName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                        _groupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                        _levelContext.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                        _rangeContext.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                        top = 20;
                    }
                    else if (workingArea.Width >= 1366 && workingArea.Width < 1400)
                    {
                        _elementName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                        _groupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                        _levelContext.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                        _rangeContext.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                        top = 14;
                        radius = 20;
                        widthContextBar = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.770M), 2));
                        //this.Size = new Size(733, 60);
                    }
                    else if (workingArea.Width == 2048 && workingArea.Height == 1280)
                    {
                        _elementName.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_REGULAR);
                        _groupName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                        _levelContext.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_REGULAR);
                        _levelContext.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                        top = 25;
                    }
                    else if (workingArea.Width == 1024 && workingArea.Height == 768)
                    {
                        _elementName.Font = FontHelper.Get(FontSizes.Small_5, FontName.ROBOTO_REGULAR);
                        _groupName.Font = FontHelper.Get(FontSizes.Small_1, FontName.ROBOTO_REGULAR);

                        _levelContext.Font = FontHelper.Get(FontSizes.Small_5, FontName.ROBOTO_REGULAR);
                        _rangeContext.Font = FontHelper.Get(FontSizes.Small_1, FontName.ROBOTO_REGULAR);

                        _groupName.Size = new Size(150, 22);
                        _elementName.Size = new Size(150, 22);
                        top = 14;
                        widthContextBar = this.Width;
                    }
                    else if (workingArea.Width < 1366)
                    {
                        _elementName.Font = FontHelper.Get(FontSizes.Small_6, FontName.ROBOTO_REGULAR);
                        _groupName.Font = FontHelper.Get(FontSizes.Small_1, FontName.ROBOTO_REGULAR);
                        _levelContext.Font = FontHelper.Get(FontSizes.Small_6, FontName.ROBOTO_REGULAR);
                        _rangeContext.Font = FontHelper.Get(FontSizes.Small_1, FontName.ROBOTO_REGULAR);
                        top = 14;
                    }
                    else if (workingArea.Width >= 2000 && workingArea.Width < 2560)
                    {
                        _elementName.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_REGULAR);
                        _groupName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                        _levelContext.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_REGULAR);
                        _rangeContext.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                        top = (_isFullScreen ? 20 : 50);
                    }
                    else if (workingArea.Width >= 2560 && workingArea.Width <= 3440)
                    {
                        _elementName.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_REGULAR);
                        _groupName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                        _levelContext.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_REGULAR);
                        _levelContext.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                        top = (_isFullScreen ? 25 : 50);
                    }
                    else if (workingArea.Width > 2000)
                    {
                        _elementName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                        _groupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR, FontStyle.Bold);
                        _levelContext.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                        _rangeContext.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR, FontStyle.Bold);
                        top = (_isFullScreen ? 20 : 50);
                    }

                    _removeButton.Size = new Size(height, height);
                    _removeButton.Top = top;
                    var RemoteWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.987M), 2));
                    _removeButton.Left = (_isFullScreen ? RemoteWidth : widthContextBar) - (_removeButton.Width * 1);

                    int separator = 0;

                    var iconElementX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0409M), 2));
                    var iconElementY = _removeButton.Top;
                    _iconElement.Size = _removeButton.Size;
                    _iconElement.Top = _removeButton.Top;

                    var iconElementWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0409M), 2));

                    var elementNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.061M), 2));
                    var elementNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.001M), 2));

                    var GroupNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.061M), 2));
                    var GroupNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.030M), 2));
                    var nameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.028M), 2));

                    var levelContextX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.280M), 2));
                    var levelContextY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.001M), 2));
                    _levelContext.Location = new Point(levelContextX, levelContextY);

                    var rangeContextX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.280M), 2));
                    var rangeContextY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.030M), 2));
                    _rangeContext.Location = new Point(levelContextX, rangeContextY);

                    _elementName.Height = nameHeight;
                    _groupName.Height = nameHeight;
                    _levelContext.Height = nameHeight;
                    _rangeContext.Height = nameHeight;

                    if (workingArea.Width == 1024 && workingArea.Height == 768)
                    {
                        iconElementX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.061M), 2)) - 50;
                        elementNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.061M), 2)) - 30;
                        GroupNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.061M), 2)) - 29;
                    }

                    _iconElement.Location = new Point(iconElementX, iconElementY);
                    _elementName.Location = new Point(elementNameX, elementNameY);
                    _groupName.Location = new Point(GroupNameX, GroupNameY);

                    _buttons.ForEach(button =>
                    {
                        switch (button)
                        {
                            case OutputToggleButton control:
                                control.Size = _removeButton.Size;
                                control.Top = _removeButton.Top;
                                break;
                            case Bunifu.UI.WinForms.BunifuDropdown control:
                                control.Height = _removeButton.Height;
                                control.Top = _removeButton.Top;
                                break;
                            case BunifuiOSSwitch control:
                                control.Height = _removeButton.Height;
                                control.Top = _removeButton.Top;
                                break;
                            case BunifuImageButton control:
                                control.Size = _removeButton.Size;
                                control.Top = _removeButton.Top;
                                break;
                            case Bunifu.UI.WinForms.BunifuButton.BunifuButton control:
                                control.Height = _removeButton.Height + 5;
                                //int currenPosition = Remove.Location.X - (control.Width + height);
                                int Yposition = _removeButton.Top;
                                control.Location = new Point(control.Location.X, Yposition);
                                control.IdleBorderRadius = radius;
                                control.OnIdleState.BorderRadius = radius;

                                break;

                        }
                        separator += height * 2;
                    });
                }
            }
        }

        public void ResizeControl()
        {
            //ClearButtons();
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                int top = 0;
                if (workingArea.Width > 1400 && workingArea.Width < 2000)
                {
                    _elementName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                    _groupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                    _levelContext.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                    top = 20;
                }
                else if (workingArea.Width >= 1366 && workingArea.Width < 1400)
                {
                    _elementName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                    _groupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                    _levelContext.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                    top = 14;
                }
                else if (workingArea.Width < 1366)
                {
                    _elementName.Font = FontHelper.Get(FontSizes.Small_2, FontName.ROBOTO_REGULAR);
                    _groupName.Font = FontHelper.Get(FontSizes.Small_1, FontName.ROBOTO_REGULAR);
                    _levelContext.Font = FontHelper.Get(FontSizes.Small_1, FontName.ROBOTO_REGULAR);
                    top = 14;
                }
                else if (workingArea.Width >= 2000 && workingArea.Width < 2560)
                {
                    _elementName.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_REGULAR);
                    _groupName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                    _levelContext.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                    top = (_isFullScreen ? 20 : 50);
                }
                else if (workingArea.Width >= 2560 && workingArea.Width <= 3440)
                {
                    _elementName.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_REGULAR);
                    _groupName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                    _levelContext.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                    top = (_isFullScreen ? 25 : 50);
                }
                else if (workingArea.Width > 2000)
                {
                    _elementName.Font = FontHelper.Get(FontSizes.Large_2, FontName.ROBOTO_REGULAR);
                    _groupName.Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_REGULAR);
                    _levelContext.Font = FontHelper.Get(FontSizes.Large_1, FontName.ROBOTO_REGULAR);
                    top = (_isFullScreen ? 20 : 50);
                }

                int height = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.023M), 2));
                int sizeX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.013M), 2));
                int removeWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.987M), 2));

                _removeButton.Top = top;
                _removeButton.Width = sizeX;
                _removeButton.Left = (_isFullScreen ? this.Width : removeWidth);// - (Remove.Width * 1);
                _nextButtonPosition = _removeButton.Left;

                foreach (var b in _buttons)
                {
                    int currentPosition = this._nextButtonPosition - _removeButton.Width;
                    currentPosition -= _removeButton.Width;
                    _nextButtonPosition = currentPosition;
                    var c = this.Controls.Find(b.Name, false);
                    BunifuImageButton control = (c[0] as BunifuImageButton);

                    control.Location = new Point(currentPosition, _removeButton.Top);
                    var controlSizeX = sizeX;
                    var controlSizeY = height;
                    control.Size = new Size(controlSizeX, controlSizeY);
                    control.Anchor = AnchorStyles.Right;
                }
                _resizeLoad = false;
            }
        }

        public void UpdateSelectStream(Profile profile)
        {
            try
            {
                if (_dropdownProfileStream != null)
                {
                    _dropdownProfileStream.SelectedValueChanged -= ProfileStreamDropdown_SelectedValueChanged;

                    _profilesStreamList.Add(new OptionObjectDTO
                    {
                        Key = profile.GetDescription(),
                        Name = Resources.ResourceManager.GetString(profile.ToString())
                    });

                    var dataSource = new BindingSource(_profilesStreamList, null);
                    _dropdownProfileStream.DataSource = dataSource;
                    _dropdownProfileStream.SelectedValue = _profileSelected;
                    _dropdownProfileStream.Text = Resources.ResourceManager.GetString(_profileSelected.ToString());
                    _dropdownProfileStream.SelectedValueChanged += ProfileStreamDropdown_SelectedValueChanged;
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
