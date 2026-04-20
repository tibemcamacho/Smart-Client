using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar
{
    public partial class SidebarDdlFilterControl : UserControl
    {
        public event EventHandler<object> ItemSelectedClicked;
        public event EventHandler<object> ButtonFilterClicked;
        public event EventHandler<object> ButtonDropdownClicked;

        private bool _resizeLoad = false;

        public string GroupName = "";
        public bool Opened = true;

        public SidebarDdlFilterControl()
        {
            InitializeComponent();
            _resizeLoad = true;
            if (!DesignMode)
            {
                filterButton.BackgroundImage = FileResources.icon_filter;
                filterButton.BackgroundImageLayout = ImageLayout.Stretch;
                Dropdown.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                bunifuToolTip1.SetToolTip(filterButton, Resources.ApplyFilter);
            }

            this.Resize += SidebarFilterControl_Resize;
            this.Dropdown.ShowDDLBtn += ShowDDLContainer;
            this.Dropdown.OptionSelected += DDLFilter_SelectedOptionChanged;
            this.Disposed += SidebarDdlFilterControl_Disposed;
        }

        private void SidebarDdlFilterControl_Disposed(object sender, EventArgs e)
        {
            this.Dropdown.ShowDDLBtn -= ShowDDLContainer;
            this.Dropdown.OptionSelected -= DDLFilter_SelectedOptionChanged;
            this.Dropdown.Click -= DropdownButton_Click;
            this.filterButton.Click -= FilterButton_Click;
            this.Resize -= SidebarFilterControl_Resize;
            this.Disposed -= SidebarDdlFilterControl_Disposed;
        }

        private void SidebarFilterControl_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        public void SetOptions(List<OptionObjectDTO> options)
        {
            this.Dropdown.SetOptions(options);
            Dropdown.SelectedIndex = 0;
            ResizeForm();
        }

        public void RemoveOptions(List<OptionObjectDTO> options)
        {
            this.Dropdown.RemoveOptions(options);
            ResizeForm();
        }

        private void DDLFilter_SelectedOptionChanged(object sender, OptionObjectDTO e)
        {
            ItemSelectedClicked?.Invoke(sender, e);
        }

        public void HideButton()
        {
            filterButton.Visible = false;
        }

        public void ShowButton()
        {
            filterButton.Visible = true;
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            ButtonFilterClicked?.Invoke(sender, e);
        }

        private void DropdownButton_Click(object sender, EventArgs e)
        {
            ButtonDropdownClicked?.Invoke(sender, e);
        }

        private void ShowDDLContainer(bool state)
        {
            if (state)
            {
                this.Height = this.Dropdown.MinimumSize.Height;
            }
            else
            {
                this.Height = this.Dropdown.MaximumSize.Height;
            }
        }

        private void ResizeForm()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                if (main.Width > 1400 && main.Width < 2000)
                {
                    Dropdown.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_LIGHT);
                }
                else if (main.Width > 1366 && main.Width <= 1400)
                {
                }
                else if (main.Width <= 1366)
                {
                    Dropdown.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_LIGHT);
                }
                else if (main.Width >= 2000 && main.Width <= 2560)
                {
                    Dropdown.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_LIGHT);
                }
                else if (main.Width > 2560 && main.Width <= 3440)
                {
                    Dropdown.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_LIGHT);
                }

                var ddlFilterWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0855M), 2));
                var ddlFilterHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.045M), 2));
                this.MinimumSize = new Size(ddlFilterWidth, ddlFilterHeight);
                this.Dropdown.MinimumSize = new Size(this.Dropdown.Width, this.MinimumSize.Height);
                var maxHeigthDDL = this.Dropdown.GetMaxHeigth();
                this.Dropdown.MaximumSize = new Size(this.Dropdown.Width, maxHeigthDDL);

                var filterButtonWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0124M), 2));
                var filterButtonHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                filterButton.Size = new Size(filterButtonWidth, filterButtonHeight);

                var filterButtonX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.117M), 2));
                var filterButtonY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.010M), 2));
                filterButton.Location = new Point(filterButtonX, filterButtonY);
                _resizeLoad = false;
            }
        }
    }
}
