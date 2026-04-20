using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar
{
    public delegate void ObjectStateEventHandler(object sender, bool state);
    public delegate void ObjectDoubleGroupClickEventHandler(object sender, List<SidebarElementDTO> Elements);

    public partial class SidebarGroupObjectControl : UserControl
    {
        public event ObjectStateEventHandler ObjectClicked;
        public event ObjectDoubleGroupClickEventHandler ObjectGroupDoubleClick;

        public string GroupName = "";

        public bool Opened = true;
        public int totalDevices = 0;
        public int totalDevicesOnLine = 0;

        public int ElementId { get; set; }
        public List<SidebarElementDTO> Elements { get; set; }

        public SidebarGroupObjectControl()
        {
            InitializeComponent();

            //PanelButton.Click += PanelButton_Click;
            this.Click += PanelButton_Click;
            LabelName.Click += PanelButton_Click;
            PanelAction.Click += PanelButton_Click;
            PanelAction.DoubleClick += PanelButton_DoubleClick;
            LabelName.DoubleClick += PanelButton_DoubleClick;

            this.MouseLeave += SidebarGroupObjectControl_MouseLeave;
            //PanelButton.MouseLeave += SidebarGroupObjectControl_MouseLeave;
            LabelName.MouseLeave += SidebarGroupObjectControl_MouseLeave;
            PanelAction.MouseLeave += SidebarGroupObjectControl_MouseLeave;

            this.BackColorChanged += SidebarGroupObjectControl_BackColorChanged;

            //this.MouseHover += SidebarGroupObjectControl_MouseHover;
            //PanelButton.MouseHover += SidebarGroupObjectControl_MouseHover;
            //LabelName.MouseHover += SidebarGroupObjectControl_MouseHover;
            //PanelAction.MouseHover += SidebarGroupObjectControl_MouseHover;

            //LabelName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
            PanelAction.BackgroundImage = FileResources.icon_arrow_dropdown_down;
            PanelAction.BackgroundImageLayout = ImageLayout.Stretch;
            PanelAction.Visible = false;


            //PanelButton.BringToFront();
            this.Resize += SidebarGroupObjectControl_Resize;
        }

        private void SidebarGroupObjectControl_Resize(object sender, EventArgs e)
        {
            ResizeForm();
        }

        internal void SetValues(string name)
        {
            LabelName.Text = name;
            if (LabelName.Text.Trim().Length > 0)
            {
                PanelAction.Visible = true;
            }
        }
        internal void ChangeBackgroundImage()
        {
            if (Opened)
            {
                PanelAction.BackgroundImage = FileResources.icon_arrow_dropdown_up;
                LabelOpenedCount.Visible = true;
            }
            else
            {
                PanelAction.BackgroundImage = FileResources.icon_arrow_dropdown_down;
                LabelOpenedCount.Visible = false;
            }
        }

        private void SidebarGroupObjectControl_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
        }

        private void SidebarGroupObjectControl_BackColorChanged(object sender, EventArgs e)
        {
            this.BackColor = this.BackColor;
            //PanelButton.BackColor = this.BackColor;
        }

        private void SidebarGroupObjectControl_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
        }
        private void PanelButton_DoubleClick(object sender, EventArgs e)
        {
            if (!Opened)
            {
                Opened = true;
                ObjectClicked(this, true);
                PanelAction.BackgroundImage = FileResources.icon_arrow_dropdown_up;
            }
            ObjectGroupDoubleClick(this, Elements);

        }

        private void PanelButton_Click(object sender, EventArgs e)
        {
            Opened = !Opened;
            totalDevices = 0;
            totalDevicesOnLine = 0;
            ObjectClicked(this, Opened);
            if (Opened)
            {
                PanelAction.BackgroundImage = FileResources.icon_arrow_dropdown_up;
                LabelOpenedCount.Visible = true;
            }
            else
            {
                PanelAction.BackgroundImage = FileResources.icon_arrow_dropdown_down;
                LabelOpenedCount.Visible = false;
            }
        }

        private void ResizeForm()
        {
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                Rectangle main = Screen.GetWorkingArea(this);

                if (main.Width > 1400 && main.Width < 2000)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width < 1400)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width > 2000)
                {
                    LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Large_2, FontStyle.Regular, GraphicsUnit.Pixel);
                }
            }
        }

        public void PanelButton()
        {
            Opened = !Opened;
            ObjectClicked(this, Opened);
            if (Opened)
            {
                PanelAction.BackgroundImage = FileResources.icon_arrow_dropdown_up;

            }
            else
            {
                PanelAction.BackgroundImage = FileResources.icon_arrow_dropdown_down;
            }
        }

        public void SetTotalOpened_TotalCount()
        {
            LabelOpenedCount.Text = totalDevicesOnLine.ToString() + "/" + totalDevices.ToString();
        }
    }
}
