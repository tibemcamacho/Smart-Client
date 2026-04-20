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
    public partial class SidebarFilterControl : UserControl
    {
        public string path = AppDomain.CurrentDomain.BaseDirectory;
        private bool _resizeLoad = false;
        public event EventHandler<object> ItemSelectedClicked;
        public event EventHandler<object> ButtonFilterClicked;

        public string GroupName = "";
        public bool Opened = true;

        public SidebarFilterControl()
        {
            InitializeComponent();
            _resizeLoad = true;
            if (!DesignMode)
            {
                filterButton.BackgroundImage = FileResources.icon_filter;
                filterButton.BackgroundImageLayout = ImageLayout.Stretch;
                comboBoxFilter.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
                bunifuToolTip1.SetToolTip(filterButton, Resources.ApplyFilter);
            }
            this.Resize += SidebarFilterControl_Resize;
        }

        private void SidebarFilterControl_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        public void SetOptions(List<OptionObjectDTO> options)
        {
            comboBoxFilter.DataSource = new BindingSource(options, null);
            comboBoxFilter.DisplayMember = "Name";
            comboBoxFilter.ValueMember = "Key";

            comboBoxFilter.SelectedIndex = 0;
            ResizeForm();
        }

        private void ComboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
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

        private void ResizeForm()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                if (main.Width >= 2000)
                {
                    //comboBoxFilterSize = comboBoxFilterSize.Width > 0 ? comboBoxFilterSize : comboBoxFilter.Size;
                    //var width = (comboBoxFilterSize.Width * main.Width) / 1920;
                    //var height = (comboBoxFilterSize.Height * main.Height) / 1080;
                    //comboBoxFilter.Size = new Size(width - 30, height);
                    //comboBoxFilter.Font = FontHelper.GetRobotoRegular(FontSizes.Large_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    //comboBoxFilter.ItemHeight = 56;
                }
                else if (main.Width > 1400 && main.Width < 2000)
                {
                    comboBoxFilter.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_LIGHT);
                }
                else if (main.Width > 1366 && main.Width <= 1400)
                {
                }
                else if (main.Width <= 1366)
                {
                    comboBoxFilter.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_LIGHT);
                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    comboBoxFilter.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_LIGHT);
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    comboBoxFilter.Font = FontHelper.Get(FontSizes.Medium_4, FontName.ROBOTO_LIGHT);
                }

                var comboBoxFilterWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0855M), 2));
                var comboBoxFilterHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.035M), 2));
                comboBoxFilter.Size = new Size(comboBoxFilterWidth, comboBoxFilterHeight);

                this.comboBoxFilter.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                var filterButtonWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.117M), 2));
                var filterButtonHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.016M), 2));
                filterButton.Location = new Point(filterButtonWidth, filterButtonHeight);

                var filterButtonX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0124M), 2));
                var filterButtonY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                filterButton.Size = new Size(filterButtonX, filterButtonY);
                _resizeLoad = false;

            }
        }
    }
}
