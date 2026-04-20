using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.MainBar
{

    public partial class VisualSearchBarControl : MainBarBaseControl
    {
        public event EventHandler<Dictionary<string, DateTime>> DateTimeSelected;
        private bool _resizeLoad = false;
        public VisualSearchBarControl()
        {
            this.ButtonCalendar = new Elipgo.SmartClient.UserControls.Shared.ButtonCalendarControl(false);
            InitializeComponent();
            _resizeLoad = true;
            //ButtonCalendar.DateTimeSelected += ButtonCalendar_DateTimeSelected;
            ButtonCalendar.DateTimeClick += ButtonCalendar_DateTimeSelected;
            LabelDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            this.Resize += VisualSearchBarControl_Resize;
            this.Disposed += VisualSearchBarControl_Disposed;
        }

        private void VisualSearchBarControl_Disposed(object sender, EventArgs e)
        {
            ButtonCalendar.DateTimeClick -= ButtonCalendar_DateTimeSelected;
            this.Resize -= VisualSearchBarControl_Resize;
            this.Disposed -= VisualSearchBarControl_Disposed;
        }

        private void VisualSearchBarControl_Resize(object sender, EventArgs e)
        {
            ResizeControls();
            SetSizes();
        }

        private void ButtonCalendar_DateTimeSelected(object sender, Dictionary<string, DateTime> e)
        {
            DateTimeSelected?.Invoke(sender, e);
            LabelDate.Text = e["date"].ToString("dd/MM/yyyy");
        }

        public override void LoadButtons()
        {
        }

        public override void SetImageButtons()
        {

        }

        public override void ShowButtons()
        {

        }

        public override void SetTooltips()
        {

        }
        private void SetSizes()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                if (this.ParentForm == null || this.ParentForm.ParentForm.WindowState == FormWindowState.Maximized)
                {
                    var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                    if (main.Width >= 2050)
                    {
                        LabelDate.Font = FontHelper.GetRobotoRegular(FontSizes.Large_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    }
                    //2049 x 1280  125%
                    else if (main.Width == 2048 && main.Height == 1280)
                    {
                        LabelDate.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    }
                    else if (main.Width > 1400 && main.Width < 2000)
                    {
                        LabelDate.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    }
                    else if (main.Width <= 1366)
                    {
                        LabelDate.Font = FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    }
                    else if (main.Width >= 2000 && main.Width < 2560)
                    {
                        LabelDate.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    }
                    else if (main.Width >= 2560 && main.Width <= 3440)
                    {
                        LabelDate.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    }

                    var containerW = this.Width;
                    var containerH = this.Height;

                    var buttonCalendarX = Convert.ToInt32(containerW - ButtonCalendar.Width - 10);
                    var buttonCalendarY = Convert.ToInt32((containerH * .5) - (ButtonCalendar.Height * .5));
                    this.ButtonCalendar.Location = new Point(buttonCalendarX, buttonCalendarY);

                    var labelDateX = buttonCalendarX - LabelDate.Width;
                    var labelDateY = Convert.ToInt32((containerH * .5) - (LabelDate.Height * .5));
                    this.LabelDate.Location = new Point(labelDateX, labelDateY);
                    this.LabelDate.Visible = true;

                    var ButtonCalendarWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0187M), 2));
                    var ButtonCalendarHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0333M), 2));
                    this.ButtonCalendar.Size = new Size(ButtonCalendarWidth, ButtonCalendarHeight);
                    _resizeLoad = false;
                }
            }
        }

    }
}
