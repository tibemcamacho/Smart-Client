using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Outputs
{
    public delegate void ItemChangeEventHandler(int OutputId, IOPortState state);
    public partial class OutputItems : UserControl
    {
        public event ItemChangeEventHandler ItemChangeState;

        public int OutputId { get; set; }
        private IOPortState OutputState { get; set; }

        public OutputItems(int id, string label)
        {
            InitializeComponent();

            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            this.Label.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
            this.Label.ForeColor = ColorTranslator.FromHtml(VariableResources.COLOR_TITLE_FONT);
            this.Label.Text = label;
            this.Switch.OffColor = ColorTranslator.FromHtml(VariableResources.COLOR_SWITCH_OFF);
            this.Switch.OnColor = ColorTranslator.FromHtml(VariableResources.COLOR_SWITCH_ON);
            this.Switch.Value = false;
            this.Switch.Enabled = false;

            OutputId = id;
            OutputState = IOPortState.Offline;
        }

        private void Switch_Click(object sender, EventArgs e)
        {
            OutputState = this.Switch.Value ? IOPortState.Active : IOPortState.Inactive;
            ItemChangeState?.Invoke(OutputId, OutputState);
        }

        public void SetState(IOPortState state)
        {
            this.Switch.Enabled = true;
            OutputState = state;
            if (state != IOPortState.Offline)
            {
                this.Switch.Value = state == IOPortState.Active;
            }
        }

        private void Label_Click(object sender, EventArgs e)
        {
            if (!this.Switch.Enabled)
            {
                return;
            }

            this.Switch.Value = !this.Switch.Value;
            OutputState = this.Switch.Value ? IOPortState.Active : IOPortState.Inactive;
            ItemChangeState?.Invoke(OutputId, OutputState);
        }
    }
}
