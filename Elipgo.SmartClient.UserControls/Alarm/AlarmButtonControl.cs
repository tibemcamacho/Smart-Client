using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public partial class AlarmButtonControl : UserControl
    {
        public event EventHandler<bool> Clicked;
        public bool state = false;
        private int _lastValue = -1; // Cache para evitar actualizaciones innecesarias
        private string _lastTooltip = null; // Cache para evitar parpadeo del tooltip

        public AlarmButtonControl()
        {
            InitializeComponent();
            this._labelNumber.Text = "";
            this._labelNumber.BackColor = Color.Transparent;

            this.Cursor = Cursors.Hand;
            this._labelNumber.Cursor = Cursors.Hand;
            this._buttonAlarm.Cursor = Cursors.Hand;

            this.Click += AlarmButtonControl_Click;
            this._buttonAlarm.Click += AlarmButtonControl_Click;
            this._labelNumber.Click += AlarmButtonControl_Click;

            _labelNumber.Visible = false;
            //PanelCircle.Visible = false;
            _labelNumber.Font = FontHelper.Get(FontSizes.Medium_0, FontName.ROBOTO_REGULAR);
            this.Resize += AlarmButtonControl_Resize;
        }

        private void AlarmButtonControl_Resize(object sender, EventArgs e)
        {
            ResizeView();
        }

        private void AlarmButtonControl_Click(object sender, EventArgs e)
        {
            state = !state;
            Clicked?.Invoke(sender, state);
        }

        public void SetValue(int n)
        {
            try
            {
                if (_labelNumber.InvokeRequired)
                {
                    _labelNumber.Invoke((MethodInvoker)delegate
                       {
                           SetValue(n);
                       });
                    return;
                }

                // Evitar actualizaciones innecesarias
                if (n == _lastValue)
                    return;

                _lastValue = n;

                if (n <= 0)
                {
                    _labelNumber.Visible = false;
                    _buttonAlarm.BackgroundImage = null;
                    // Limpiar tooltip si estaba activo
                    if (_lastTooltip != null)
                    {
                        _lastTooltip = null;
                        this._bunifuToolTip.SetToolTip(this._labelNumber, string.Empty);
                    }
                    return;
                }

                // n > 0
                _labelNumber.Visible = true;
                _buttonAlarm.BackgroundImage = Elipgo.SmartClient.UserControls.Properties.Resources.alarm_background;
                _buttonAlarm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

                // Determinar texto a mostrar
                string newText = n > 99 ? "..." : n.ToString();

                // Tooltip solo cuando mostramos "..." (más de 99 alarmas)
                string tooltipText = n > 99 ? n.ToString() : null;

                if (_labelNumber.Text != newText)
                {
                    _labelNumber.Text = newText;
                }

                // Solo actualizar tooltip si cambió
                if (_lastTooltip != tooltipText)
                {
                    _lastTooltip = tooltipText;
                    this._bunifuToolTip.SetToolTip(this._labelNumber, tooltipText ?? string.Empty);
                }

                _labelNumber.BackColor = Color.Transparent;
            }
            catch (Exception ex)
            {
                Logger.Log("Bell SetValue Exception " + ex.Message, LogPriority.Fatal);
            }
        }

        public void SetImageMute()
        {
            this._buttonAlarm.Image = FileResources.icon_notifications_off;
        }

        public void SetImageSound()
        {
            this._buttonAlarm.Image = FileResources.icon_alarms_remove;
        }

        private void ResizeView()
        {
            if (Screen.AllScreens.Length >= 1 && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                var settings = GetLabelSettings(main.Width, main.Height);

                _labelNumber.Font = settings.Font;
                _labelNumber.Size = new Size(settings.Width, settings.Height);
                _labelNumber.Location = new Point(this.Width - settings.Width, 0);

                if (main.Width == 1024 && main.Height == 768)
                {
                    _buttonAlarm.Size = new Size(25, 25);
                }
            }
        }

        private (Font Font, int Width, int Height) GetLabelSettings(int width, int height)
        {
            if (width == 1366 && height == 768)
                return (FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel),
                        Convert.ToInt32(Math.Round(width * 0.0095M, 2)),
                        Convert.ToInt32(Math.Round(height * 0.0175M, 2)));

            if (width > 1400 && width < 2000)
                return (FontHelper.GetRobotoRegular(FontSizes.Small_3, FontStyle.Regular, GraphicsUnit.Pixel),
                        Convert.ToInt32(Math.Round(width * 0.0083M, 2)),
                        Convert.ToInt32(Math.Round(height * 0.0148M, 2)));

            if (width >= 1366 && width < 1400)
                return (FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel),
                        Convert.ToInt32(Math.Round(width * 0.0088M, 2)),
                        Convert.ToInt32(Math.Round(height * 0.0157M, 2)));

            if (width >= 2000 && width <= 2560)
                return (FontHelper.GetRobotoRegular(FontSizes.Small_5, FontStyle.Regular, GraphicsUnit.Pixel),
                        Convert.ToInt32(Math.Round(width * 0.0078M, 2)),
                        Convert.ToInt32(Math.Round(height * 0.0185M, 2)));

            if (width > 2560 && width <= 3440)
                return (FontHelper.GetRobotoRegular(FontSizes.Small_7, FontStyle.Regular, GraphicsUnit.Pixel),
                        Convert.ToInt32(Math.Round(width * 0.0093M, 2)),
                        Convert.ToInt32(Math.Round(height * 0.0166M, 2)));

            // Default
            return (FontHelper.GetRobotoRegular(FontSizes.Small_0, FontStyle.Regular, GraphicsUnit.Pixel),
                    Convert.ToInt32(Math.Round(width * 0.0088M, 2)),
                    Convert.ToInt32(Math.Round(height * 0.0157M, 2)));
        }
    }
}
