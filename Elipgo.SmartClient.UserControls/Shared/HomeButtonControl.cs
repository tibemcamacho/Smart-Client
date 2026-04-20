using Elipgo.SmartClient.Common.Properties;
using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Shared
{
    public partial class HomeButtonControl : UserControl
    {
        public event EventHandler<bool> Clicked;
        public bool state = false;

        public HomeButtonControl()
        {
            InitializeComponent();

            this.Cursor = Cursors.Hand;
            this.HomeButton.Cursor = Cursors.Hand;

            this.Click += HomeButtonControl_Click;
            this.HomeButton.Click += HomeButtonControl_Click;
        }

        private void HomeButtonControl_Click(object sender, EventArgs e)
        {
            state = !state;
            Clicked?.Invoke(sender, state);
        }

        public void SetImage()
        {
            this.HomeButton.Image = FileResources.contextBarMain_home;
        }
    }
}
