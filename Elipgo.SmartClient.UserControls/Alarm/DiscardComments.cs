using Elipgo.SmartClient.Common.Properties;
using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public partial class DiscardComments : UserControl
    {
        public bool Aceptar;
        public string Comments;
        public DiscardComments()
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

        public void SetMessage(string message)
        {
            LabelTitle.Text = message;
            LabelTitle.AutoSize = true;
            txtComments.KeyUp += txtCommentsKeyPress;
            ButtonOK.Visible = false;
        }

        public void SetLabelDiscard(string message)
        {
            LabelTitle.Text = message;
            LabelTitle.AutoSize = true;
        }

        private void txtCommentsKeyPress(object sender, KeyEventArgs e)
        {
            ButtonOK.Visible = txtComments.Text.Length > 0;
        }
    }
}
