using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    public partial class GenericFormNoData : UserControl
    {
        public GenericFormNoData(string ControlName)
        {
            InitializeComponent();
            Label1.Text = "Sin " + ControlName.Replace("Control", "");
            Label2.Text = "No tienes " + ControlName.Replace("Control", "") + " hasta el momento. Haz click en Nuevo para crear uno.";
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }
    }
}
