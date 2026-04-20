using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Shared
{
    public partial class CapacityNotAvailableControl : UserControl
    {
        public CapacityNotAvailableControl()
        {
            InitializeComponent();

            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            this.label1.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
            this.label1.Text = Resources.CapacityNotAvailable;
        }

    }
}
