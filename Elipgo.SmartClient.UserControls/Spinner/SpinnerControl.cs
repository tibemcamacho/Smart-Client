using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Spinner
{
    public partial class SpinnerControl : UserControl
    {
        public SpinnerControl()
        {
            InitializeComponent();
            LoadImages();
        }

        private void LoadImages()
        {
            if (!DesignMode)
            {
                this.image.Image = Image.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, VariableResources.PATH_IMAGE_SPINNER));
                this.AutoSize = true;
                this.Dock = DockStyle.Fill;
            }
        }

        public void ShowSpinner(bool show)
        {
            this.Visible = false;
        }
    }
}
