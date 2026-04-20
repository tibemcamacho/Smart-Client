using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.FaceObject
{
    public partial class FaceElement : UserControl
    {
        public FaceAlarmsDTO faceAlarm = new FaceAlarmsDTO();

        public event EventHandler<FaceAlarmsDTO> Selected;

        private bool _loaded = false;

        public FaceElement(FaceAlarmsDTO face, int FaceWidth, int FaceHeight)
        {
            faceAlarm = face;
            InitializeComponent();

            this.Paint += FaceElement_Paint;
            this.Click += FaceElement_Click;
            this.PanelData.Click += FaceElement_Click;
            this.PanelIdentity.Click += FaceElement_Click;
            this.PanelPercent.Click += FaceElement_Click;
            this.PanelRecognition.Click += FaceElement_Click;

            this.PanelData.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            this.PanelPercent.BackColor = Color.Transparent;
            this.LabelPercent.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);

            this.Size = new Size(FaceWidth, FaceHeight);
            this.PanelRecognition.Paint += PanelRecognition_Paint;
            this.PanelIdentity.Paint += PanelIdentity_Paint;
            this.Resize += FaceElement_Resize;

        }

        private void FaceElement_Resize(object sender, EventArgs e)
        {
            SetStyles();
            //LoadData();
        }

        private void PanelIdentity_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU));
            e.Graphics.FillEllipse(b, (PanelPercent.Width / 2) * -1, PanelPercent.Top, PanelPercent.Width - 1, PanelPercent.Height - 1);
            base.OnPaint(e);
            PanelPercent.Visible = false;
        }

        private void PanelRecognition_Paint(object sender, PaintEventArgs e)
        {
            SolidBrush b = new SolidBrush(ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU));
            e.Graphics.FillEllipse(b, PanelPercent.Left, PanelPercent.Top, PanelPercent.Width - 1, PanelPercent.Height - 1);
            base.OnPaint(e);
            PanelPercent.Visible = false;
        }

        private void FaceElement_Click(object sender, EventArgs e)
        {
            Selected(this, faceAlarm);
        }

        public void LoadData()
        {
            if (faceAlarm.SubjectFaceImage != null)
            {
                using (var ms = new MemoryStream(faceAlarm.SubjectFaceImage, 0, faceAlarm.SubjectFaceImage.Length))
                {
                    var image = Image.FromStream(ms, true);
                    PanelRecognition.BackgroundImage = image;
                    PanelRecognition.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }

            if (faceAlarm.SubjectModelImage != null)
            {
                using (var ms = new MemoryStream(faceAlarm.SubjectModelImage, 0, faceAlarm.SubjectModelImage.Length))
                {
                    var image = Image.FromStream(ms, true);
                    PanelIdentity.BackgroundImage = image;
                    PanelIdentity.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }

            string name = "NO IDENTIFICADO";

            if (faceAlarm.SubjectLastName != null && faceAlarm.SubjectName != null)
            {
                name = faceAlarm.SubjectLastName + ", " + faceAlarm.SubjectName;
            }
            else if (faceAlarm.SubjectLastName != null && faceAlarm.SubjectName == null)
            {
                name = faceAlarm.SubjectLastName;
            }

            LabelName.Text = name;
            LabelPercent.Text = faceAlarm.Score != 0 ? faceAlarm.Score + "%" : "SIN DATOS";
            LabelDate.Text = faceAlarm.TimeStamp.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            LabelList.Text = faceAlarm.SubjectGroup;
        }

        private void SetStyles()
        {
            this.PanelPercent.Left = (int)(this.Width / 2) - 18;
            this.PanelIdentity.Width = this.Width / 2;
            this.PanelRecognition.Width = this.Width / 2;
            this.PanelIdentity.Left = this.PanelRecognition.Width;

            this.LabelName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_BOLD, FontStyle.Bold);
            this.LabelName.Location = new Point(5, 12);
            this.LabelDate.Font = FontHelper.Get(FontSizes.Medium_0, FontName.ROBOTO_REGULAR);
            this.LabelDate.Location = new Point(5, 27);
            this.LabelPercent.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_BOLD, FontStyle.Bold);
            this.LabelPercent.Location = new Point(((this.Width / 2) - (LabelPercent.Width / 2)) + 1, (PanelPercent.Top + (PanelPercent.Height / 2)) - (LabelPercent.Height / 2));
            this.LabelPercent.BringToFront();
            this.LabelList.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
            //this.LabelList.Location = new Point(this.PanelIdentity.Left + ((PanelIdentity.Width * 15) / 100), (PanelData.Height / 2) - (LabelList.Height / 2));
            this.LabelList.Location = new Point(this.PanelIdentity.Left, LabelDate.Top);
        }

        private void FaceElement_Paint(object sender, PaintEventArgs e)
        {
            if (_loaded)
            {
                return;
            }

            SetStyles();
            LoadData();
            _loaded = true;
        }
    }
}
