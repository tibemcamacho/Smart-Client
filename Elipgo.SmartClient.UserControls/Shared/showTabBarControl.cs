using Elipgo.SmartClient.Common.Properties;
using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Shared
{
    public partial class ShowTabBarControl : UserControl
    {
        public event EventHandler ButtonClick;

        public ShowTabBarControl()
        {
            InitializeComponent();
            pictureBoxTopbarOpen.Image = FileResources.icon_topbar_open;
        }

        private void PictureBoxTopbarOpen_Click(object sender, EventArgs e)
        {
            ButtonClick?.Invoke(sender, e);
        }
    }
}
