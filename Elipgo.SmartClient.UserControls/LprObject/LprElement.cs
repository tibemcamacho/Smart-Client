using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.LprObject
{
    public partial class LprElement : UserControl
    {
        public LprAlarmDTO _lprAlarm = new LprAlarmDTO();

        public event EventHandler<LprAlarmDTO> Selected;

        private bool _sinContenido = false;

        private bool _loaded = false;
        private int _lprWidth;
        private int _lprHeight;

        public LprElement(LprAlarmDTO lprAlarm, int LprWidth, int LprHeight)
        {
            _lprAlarm = lprAlarm;
            _lprWidth = LprWidth;
            _lprHeight = LprHeight;
            InitializeComponent();
            this.Resize += LprElement_Resize;
        }

        private void LprElement_Resize(object sender, EventArgs e)
        {

            this._sinContenido = _lprAlarm.Number.Equals("SIN DATOS");

            _labelDate.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);

            var lblPlateFontSize = FontSizes.Medium_4;
            var lblDateFontSize = FontSizes.Medium_0;
            var lblListFontSize = FontSizes.Medium_0;

            if (!this._sinContenido)
            {
                if (_lprWidth > 200 && Height < 100)
                {
                    lblPlateFontSize = FontSizes.Small_5;
                    lblDateFontSize = FontSizes.Small_5;
                    lblListFontSize = FontSizes.Small_5;
                    Console.WriteLine($"LabelLocation 1");
                    if (_lprWidth < 300)
                    {
                        lblPlateFontSize = FontSizes.Medium_3;
                        lblDateFontSize = FontSizes.Small_4;
                        lblListFontSize = FontSizes.Small_4;
                        Console.WriteLine($"LabelLocation 2");
                    }
                }
                else if (_lprWidth > 100)
                {
                    lblPlateFontSize = FontSizes.Small_3;
                    lblDateFontSize = FontSizes.Small_3;
                    lblListFontSize = FontSizes.Small_3;
                    Console.WriteLine($"LabelLocation 3");
                }
                else if (_lprWidth > 60)
                {
                    lblPlateFontSize = FontSizes.Small_4;
                    Console.WriteLine($"LabelLocation 4");
                }
                else if (_lprWidth > 50)
                {
                    lblPlateFontSize = FontSizes.Small_2;
                    _labelPlate.Location = new Point(0, _labelPlate.Location.Y);
                    _labelDate.Location = new Point(0, _labelDate.Location.Y);
                    _labelList.Location = new Point(0, _labelList.Location.Y);
                    Console.WriteLine($"LabelLocation 5");
                }
                else
                {
                    lblPlateFontSize = FontSizes.Small_0;
                    lblListFontSize = FontSizes.Small_0;
                    lblDateFontSize = FontSizes.Small_0;
                    Console.WriteLine($"LabelLocation 6");
                }
            }

            _labelPlate.Font = FontHelper.Get(lblPlateFontSize, FontName.ROBOTO_BOLD);
            _labelList.Font = FontHelper.Get(lblListFontSize, FontName.ROBOTO_BOLD);
            _labelDate.Font = FontHelper.Get(lblDateFontSize, FontName.ROBOTO_BOLD);

            this.Size = new Size(_lprWidth, this.Size.Height);
            this.Paint += LprElement_Paint;

            if (!this._sinContenido)
            {
                this.Click += LprElement_Click;
                this._labelDate.Click += LprElement_Click;
                this._labelList.Click += LprElement_Click;
                this._labelPlate.Click += LprElement_Click;
            }
            ResizeControl(_lprWidth, _lprHeight);

        }

        private void LprElement_Click(object sender, EventArgs e)
        {
            Selected(this, _lprAlarm);
        }

        private void LprElement_Paint(object sender, PaintEventArgs e)
        {
            if (_loaded)
            {
                return;
            }

            if (!string.IsNullOrEmpty(this._lprAlarm.TimeAction))
            {
                DateTime date = DateTime.ParseExact(_lprAlarm.TimeActionUtc, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                _labelDate.Text = date.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            }

            if (_lprAlarm.LprLists != null)
            {
                _labelList.Text = string.Join(",", _lprAlarm.LprLists);
            }
            else
            {
                _labelList.Text = "";
            }

            _labelPlate.Text = _lprAlarm.Number;
            _loaded = true;
        }

        public void SetSelected()
        {
            if (!this._sinContenido)
            {
                _labelPlate.BringToFront();
                this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
            }

        }

        public void SetUnselected()
        {

            if (!this._sinContenido)
            {
                _labelPlate.BringToFront();
                this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            }

        }

        public void ResizeControl(int LprWidth, int LprHeight)
        {
            int labelListHeight = Convert.ToInt32(Math.Round(LprHeight * 0.16M, 2));
            int labelDateHeight = Convert.ToInt32(Math.Round(LprHeight * 0.34M, 2));
            int labelPlateHeight = Convert.ToInt32(Math.Round(LprHeight * 0.35M, 2));

            if (LprWidth > 500)
            {
                labelListHeight = Convert.ToInt32(Math.Round(LprHeight * 0.25M, 2));
                labelDateHeight = Convert.ToInt32(Math.Round(LprHeight * 0.25M, 2));
                labelPlateHeight = Convert.ToInt32(Math.Round(LprHeight * 0.25M, 2));
            }

            _labelList.Size = new Size(LprWidth, labelListHeight);
            _labelDate.Size = new Size(LprWidth, labelDateHeight);
            _labelPlate.Size = new Size(LprWidth, labelPlateHeight);

            int labelListY = Convert.ToInt32(Math.Round(LprHeight * 0.05M, 2));
            int labelDateY = Convert.ToInt32(Math.Round(LprHeight * 0.55M, 2));

            int labelPlateY = labelListY + labelListHeight;

            _labelList.Location = new Point(0, labelListY);
            _labelDate.Location = new Point(0, labelDateY);
            _labelPlate.Location = new Point(0, labelPlateY);
        }

    }
}
