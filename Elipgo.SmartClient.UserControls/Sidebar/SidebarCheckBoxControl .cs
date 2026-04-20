using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar
{
    public delegate void ObjectSelectEventHandler(string name, bool state);

    public delegate void ItemChangeEventHandler(CheckElementDTO control);

    public partial class SidebarCheckBoxControl : UserControl
    {
        public string path = AppDomain.CurrentDomain.BaseDirectory;

        public event ObjectSelectEventHandler CheckFilterSelectedClicked;

        public event ItemChangeEventHandler ItemChangeState;
        private bool _resizeLoad = false;
        public CheckElementDTO Item => new CheckElementDTO()
        {
            Key = this.Name,
            Name = this.LabelName.Text,
            State = CheckBox.Checked
        };

        public bool Selected { get; set; }

        public bool ShowCheckBox
        {
            set => this.CheckBox.Visible = value;
        }

        public bool CheckedItem
        {
            set => this.CheckBox.Checked = value;
        }

        public event ObjectSelectEventHandler ItemSelectedClicked;

        public string GroupName = "";

        public bool Opened = true;

        public SidebarCheckBoxControl()
        {
            InitializeComponent();
            _resizeLoad = true;
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            this.Resize += SidebarCheckBoxControl_Resize;
            this.Disposed += SidebarCheckBoxControl_Disposed;
        }

        private void SidebarCheckBoxControl_Resize(object sender, EventArgs e)
        {
            ResizeControl();
        }

        private void PanelButton_Paint(object sender, PaintEventArgs e)
        {

        }
        public void SetOption(string name, string key, bool state)
        {

            LabelName.Text = name;
            this.Name = key;
            CheckBox.Checked = state;
            ResizeControl();
        }

        private void BunifuCheckBox1_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            Selected = CheckBox.Checked;
            CheckFilterSelectedClicked?.Invoke(this.Name, e.Checked);
            ItemChangeState?.Invoke(Item);
        }

        private void LabelName_Click(object sender, EventArgs e)
        {
            ItemSelectedClicked?.Invoke(this.Name, true);
        }

        private void ResizeControl()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                if (main.Width > 1400 && main.Width < 2000)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    CheckBox.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    panel1.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width > 1366 && main.Width <= 1400)
                {
                }
                else if (main.Width <= 1366)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    CheckBox.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    panel1.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    CheckBox.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    panel1.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    CheckBox.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    panel1.Font = FontHelper.GetRobotoRegular(FontSizes.Large_2, FontStyle.Regular, GraphicsUnit.Pixel);
                }

                var LabelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.074M), 2));
                var LabelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.033M), 2));
                LabelName.Size = new Size(LabelNameX, LabelNameY);

                var CheckX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.011M), 2));
                var CheckXY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0196M), 2));
                CheckBox.Size = new Size(CheckX, CheckXY);

                var ChecklX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0856M), 2));
                var ChecklY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0114M), 2)) + 1;
                CheckBox.Location = new Point(ChecklX, ChecklY);

                var PanelIconX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.105M), 2)) + 1;
                var PanelIconY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0448M), 2));

                panel1.Size = new Size(PanelIconX, PanelIconY);
                _resizeLoad = false;
            }
        }

        private void SidebarCheckBoxControl_Disposed(object sender, EventArgs e)
        {
            this.Resize -= SidebarCheckBoxControl_Resize;
            this.panel1.Click -= this.LabelName_Click;
            this.LabelName.Click -= this.LabelName_Click;
            this.Disposed -= SidebarCheckBoxControl_Disposed;
        }
    }
}
