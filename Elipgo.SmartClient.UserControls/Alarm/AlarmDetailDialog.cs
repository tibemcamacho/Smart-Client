using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public partial class AlarmDetailDialog : UserControl
    {
        public bool Aceptar;
        public string Comments;
        public AlarmDetailDialog()
        {
            InitializeComponent();
            ButtonOK.Text = Resources.ButtonOK;
            ButtonCancel.Text = Resources.cancel;
            Aceptar = false;
            Comments = "";

        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Comments = txtComments.Text;
            Aceptar = true;
            ((Form)this.TopLevelControl).Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            ((Form)this.TopLevelControl).Close();
        }

        public void setTitle(string message)
        {
            LabelTitle.Text = message;
            LabelTitle.AutoSize = false;
        }

        private void Title_TextChanged(object sender, EventArgs e)
        {
            SetTextFont((Label)sender);
        }

        public void setComments(string comments)
        {
            txtComments.Text = comments;
            txtComments.AutoSize = true;
            txtComments.Enabled = false;
        }

        public void setAttendUser(string text)
        {
            lblAttendByUser.Text = text;
            lblAttendByUser.AutoSize = true;
        }

        public void setAttendDate(string text)
        {
            lblAttendDate.Text = text;
            lblAttendDate.AutoSize = true;
        }

        public void HideCancelButtton()
        {
            this.ButtonCancel.Visible = false;
        }

        public void EnableOkButton()
        {
            this.ButtonOK.Enabled = true;
        }

        public void setAlertMessage(string text)
        {
            this.AlertMessage.Text = text;
            this.AlertMessage.AutoSize = true;
            this.AlertMessage.Visible = true;
        }

        private void SetTextFont(Label label)
        {
            if (string.IsNullOrEmpty(label.Text) || label.Width == 0 || label.Height == 0)
                return;

            float tamañoMaximo = 15F;
            float tamañoMinimo = 4f;

            Font fuenteActual = label.Font;
            float tamañoPrueba = tamañoMaximo;

            using (Graphics g = label.CreateGraphics())
            {
                while (tamañoPrueba >= tamañoMinimo)
                {
                    Font fuentePrueba = new Font(fuenteActual.FontFamily, tamañoPrueba, fuenteActual.Style);
                    SizeF tamañoTexto = g.MeasureString(label.Text, fuentePrueba);

                    if (tamañoTexto.Width <= label.Width && tamañoTexto.Height <= label.Height)
                    {
                        label.Font = fuentePrueba;
                        return;
                    }

                    tamañoPrueba -= 0.5f;
                }

                label.Font = new Font(fuenteActual.FontFamily, tamañoMinimo, fuenteActual.Style);
            }
        }
    }
}
