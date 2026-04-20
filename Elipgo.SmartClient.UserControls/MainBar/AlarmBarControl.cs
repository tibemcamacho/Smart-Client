using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.MainBar
{
    public enum ContentAlarmGrid
    {
        Alarms,
        HistoryAlarms
    }

    public class ToggleAlarmEventArgs : EventArgs
    {
        public ContentAlarmGrid CurrentContentGrid { get; set; }

        public ToggleAlarmEventArgs(ContentAlarmGrid contentGrid)
        {
            CurrentContentGrid = contentGrid;
        }
    }

    public delegate void ObjectSelectedEventHandler(object sender, LiveBarItemDTO element);
    public delegate void ButtonToggleAlarmsClick(object sender, ToggleAlarmEventArgs e);

    public partial class AlarmBarControl : MainBarBaseControl
    {
        public event ButtonToggleAlarmsClick ButtonToggleAlarmsClick;
        private IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private bool _resizeLoad = false;

        public AlarmBarControl()
        {
            InitializeComponent();
            _resizeLoad = true;
            BuildBar();
            this.Resize += AlarmBarControl_Resize;
            this.ButtonToggleAlarm.Click += ButtonToggleAlarm_Click;
        }

        private void AlarmBarControlResize()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                if (workingArea.Width > 1400 && workingArea.Width < 2000)
                {
                    ButtonToggleAlarm.Font = FontHelper.GetRobotoRegular(FontSizes.Small_6, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonToggleAlarm.IdleBorderRadius = 30;
                    ButtonToggleAlarm.OnIdleState.BorderRadius = 30;
                }
                else if (workingArea.Width > 1366 && workingArea.Width <= 1400)
                {
                }
                else if (workingArea.Width <= 1366)
                {
                    ButtonToggleAlarm.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonToggleAlarm.IdleBorderRadius = 20;
                    ButtonToggleAlarm.OnIdleState.BorderRadius = 20;
                }
                else if (workingArea.Width >= 2000 && workingArea.Width < 2560)
                {
                    ButtonToggleAlarm.Font = FontHelper.GetRobotoRegular(FontSizes.Small_8, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonToggleAlarm.IdleBorderRadius = 30;
                    ButtonToggleAlarm.OnIdleState.BorderRadius = 30;
                }
                else if (workingArea.Width >= 2560 && workingArea.Width <= 3440)
                {
                    ButtonToggleAlarm.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonToggleAlarm.IdleBorderRadius = 30;
                    ButtonToggleAlarm.OnIdleState.BorderRadius = 30;
                }

                var vaultBarControl = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.725M), 2));
                this.Width = vaultBarControl;

                var buttonToggleAlarmWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.100M), 2));
                var buttonToggleAlarmHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.034M), 2));
                ButtonToggleAlarm.Size = new Size(buttonToggleAlarmWidth, buttonToggleAlarmHeight);

                var buttonToggleAlarmX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.610M), 2));
                var buttonToggleAlarmY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.016M), 2));
                ButtonToggleAlarm.Location = new Point(buttonToggleAlarmX, buttonToggleAlarmY);

                if (workingArea.Width == 1024 && workingArea.Height == 768)
                {
                    ButtonToggleAlarm.Size = new Size(buttonToggleAlarmWidth + 30, buttonToggleAlarmHeight);
                    ButtonToggleAlarm.Location = new Point(buttonToggleAlarmX - 65, buttonToggleAlarmY);
                }

                _resizeLoad = false;
            }
        }

        private void AlarmBarControl_Resize(object sender, EventArgs e)
        {
            ResizeControls();
            AlarmBarControlResize();
        }

        private void ButtonRemoveAlarms_Click(object sender, EventArgs e)
        {
            base.OnObjectSelectedChanged(sender, new ObjectSelectedArgs(new LiveBarItemDTO(LiveBarButtom.removeAllAlarms)));

        }

        public override void LoadButtons()
        {
            this.Buttons.Add(buttonClearAlarm);
        }

        public override void SetImageButtons()
        {
            //  this.buttonClearAlarm.Image = global::Elipgo.SmartClient.Common.Properties.FileResources.icons_remover_alarmas;
        }

        public override void ShowButtons()
        {
            buttonClearAlarm.Visible = false;
        }

        public override void SetTooltips()
        {
            CultureInfo ci = CultureInfo.InstalledUICulture;
            bunifuToolTip1.SetToolTip(buttonClearAlarm, ci.Name.Contains("es") ? ButtonsContextBar.DiscardAllAlarms.GetDescription() : ButtonsContextBar.DiscardAllAlarms.GetAttribute<DescriptionEN>().Descripcion);
        }

        private void ButtonToggleAlarm_Click(object sender, EventArgs e)
        {
            Bunifu.UI.WinForms.BunifuButton.BunifuButton btnToggleBookmarks = (Bunifu.UI.WinForms.BunifuButton.BunifuButton)sender;
            ToggleAlarmEventArgs toggleEventArgs;

            if (this.ButtonToggleAlarm.Text.ToLower() == Resources.AlarmHistory.ToLower())
            {
                btnToggleBookmarks.Text = Resources.Alarms;
                toggleEventArgs = new ToggleAlarmEventArgs(ContentAlarmGrid.HistoryAlarms);
                ButtonToggleAlarmsClick(sender, toggleEventArgs);
            }
            else
            {
                btnToggleBookmarks.Text = Resources.AlarmHistory;
                toggleEventArgs = new ToggleAlarmEventArgs(ContentAlarmGrid.Alarms);
                ButtonToggleAlarmsClick(sender, toggleEventArgs);
            }
        }
    }
}
