using Elipgo.SmartClient.Common.Helpers;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.ElementContainer
{
    public partial class ElementSnapshotControl : UserControl
    {
        public ElementSnapshotControl(string imageBase64)
        {
            InitializeComponent();

            if (imageBase64 != null)
            {
                this.pictureBox.Image = ImageHelper.Base64ToImage(imageBase64);
                this.pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

            }
        }
    }
}
