using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.FaceObject
{
    public partial class FaceElementEmpty : UserControl
    {
        public FaceElementEmpty(int faceWidth, int faceHeight)
        {
            InitializeComponent();

            this.label1.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_BOLD, FontStyle.Bold);
            this.label1.Text = "SIN DATOS";
            this.label1.ForeColor = Color.White;

            this.label2.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_BOLD, FontStyle.Bold);
            this.label2.Text = "SIN DATOS";
            this.label2.ForeColor = Color.White;

            this.panelIdentity.BorderStyle = BorderStyle.FixedSingle;
            this.panel1.BorderStyle = BorderStyle.FixedSingle;
            ControlResize(faceWidth, faceHeight);
        }

        private void ControlResize(int faceWidth, int faceHeight)
        {
            var size = new FontSizes();
            if (faceWidth > 200)
                size = FontSizes.Medium_3;
            else if (faceWidth > 100 && faceWidth < 200)
                size = FontSizes.Medium_0;
            else if (faceWidth > 60 && faceWidth < 100)
                size = FontSizes.Small_1;
            else if (faceWidth > 41 && faceWidth < 60)
                size = FontSizes.Small_0;
            else if (this.Size.Width < 41)
                size = FontSizes.Small_0;

            label1.Font = FontHelper.Get(size, FontName.ROBOTO_BOLD);
            label2.Font = FontHelper.Get(size, FontName.ROBOTO_BOLD);

            this.Size = new Size(faceWidth, faceHeight);
            if (Screen.AllScreens.Any(m => m.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                //117, 142
                var panelIdentityWith = Convert.ToInt32(Math.Round(Convert.ToDecimal(faceWidth * 0.5M), 2));
                var panelIdentityHeight = faceHeight;//Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1314M), 2));
                panelIdentity.Size = new Size(panelIdentityWith, panelIdentityHeight);
                panel1.Size = new Size(panelIdentityWith, panelIdentityHeight);

                var panel1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(faceWidth * 0.5M), 2));
                panel1.Location = new Point(panel1X, panel1.Location.Y);

            }


            var labelPlateX = Convert.ToInt32(Math.Round(Convert.ToDecimal(faceWidth * 0.0512M), 2));
            var labelPlateY = Convert.ToInt32(Math.Round(Convert.ToDecimal(faceHeight * 0.5352M), 2));
            label1.Location = new Point(labelPlateX, labelPlateY);//LabelPlate.Location.X
            label2.Location = new Point(labelPlateX, labelPlateY);//LabelPlate.Location.X

        }
    }
}
