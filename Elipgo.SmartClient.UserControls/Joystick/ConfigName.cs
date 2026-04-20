using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Joystick
{
    public partial class ConfigName : UserControl
    {
        public ConfigName()
        {
            InitializeComponent();
        }
        public string configName;
        public bool saved;
        private void button1_Click(object sender, EventArgs e)
        {
            saved = false;
            ((Form)this.TopLevelControl).Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            configName = txtConfig.Text;
            saved = true;
            ((Form)this.TopLevelControl).Close();
        }
    }
}
