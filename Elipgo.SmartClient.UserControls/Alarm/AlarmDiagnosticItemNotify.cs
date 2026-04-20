using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public delegate void ItemChangeEventHandler(CheckElementDTO control);

    public partial class AlarmDiagnosticItemNotify : UserControl
    {
        public event ItemChangeEventHandler ItemChangeState;

        public CheckElementDTO Item => new CheckElementDTO()
        {
            Key = this.Name,
            Name = this.LabelName.Text,
            State = CheckBox.Checked
        };

        public AlarmDiagnosticItemNotify(CheckElementDTO item)
        {
            InitializeComponent();
            this.LabelName.Anchor = AnchorStyles.Left;
            this.CheckBox.Anchor = AnchorStyles.Right;
            this.CheckBox.CheckedChanged += CheckBox_CheckedChanged;

            this.SetStyle();

            this.SetOption(item);
        }

        private void SetStyle()
        {
            // D3D3D3 
            this.LabelName.Font = FontHelper.Get(FontSizes.Medium_2, FontName.ROBOTO_REGULAR);
            this.LabelName.ForeColor = ColorTranslator.FromHtml(VariableResources.COLOR_TITLE_FONT);
        }

        private void CheckBox_CheckedChanged(object sender, Bunifu.UI.WinForms.BunifuCheckBox.CheckedChangedEventArgs e)
        {
            ItemChangeState?.Invoke(Item);
        }

        private void SetOption(CheckElementDTO item)
        {
            LabelName.Text = item.Name;
            this.Name = item.Key;
            CheckBox.Checked = item.State;
        }
    }
}
