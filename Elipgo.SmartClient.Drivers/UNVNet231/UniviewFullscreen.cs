using Elipgo.SmartClient.Common.Properties;
using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.Drivers.UNVNet231
{
    public partial class UniviewFullscreen : Form
    {
        public IntPtr pHandle = IntPtr.Zero;
        public UniviewFullscreen()
        {
            InitializeComponent();
            pHandle = PictureFullscreen.Handle;

            this.ButtonClose.Image = FileResources.icon_close;
            this.CancelButton = bClose;
            this.ButtonClose.Click += ButtonClose_Click;
            this.bClose.Click += ButtonClose_Click;
            this.PictureFullscreen.DoubleClick += ButtonClose_Click;
            this.DoubleClick += ButtonClose_Click;

            this.Paint += DahuaFullscreen_Paint;
        }

        private void DahuaFullscreen_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
