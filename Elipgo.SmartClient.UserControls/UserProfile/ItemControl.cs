using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.UserControls.Sidebar;
using System.Drawing;
using System.Windows.Forms;
using Resources = Elipgo.SmartClient.Common.Properties.Resources;

namespace Elipgo.SmartClient.UserControls.UserProfile
{
    public partial class ItemControl : UserControl
    {

        public event ObjectSelectEventHandler ItemSelectedClicked;
        public string Label { get => _labelName.Text; set => _labelName.Text = value; }
        public Image Icon { get => _panelIcon.BackgroundImage; set => _panelIcon.BackgroundImage = value; }
        public CheckElementDTO Item { get; set; }
        public ItemControl()
        {
            InitializeComponent();
        }

        private void LabelName_Click(object sender, System.EventArgs e)
        {
            if (Item.Key == "DeviceStatus")
            {
                if (MessageBox.Show(Resources.DeviceStatusConfirm, Resources.DeviceStatus, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    ItemSelectedClicked?.Invoke(Item.Key, true);
                    _bunifuToggleSwitch.Checked = !_bunifuToggleSwitch.Checked;
                }
            }
            else
            {
                ItemSelectedClicked?.Invoke(Item.Key, true);
            }
        }

        public void VisibleSwichEvent(bool visible)
        {
            _bunifuToggleSwitch.Visible = visible;

        }

        public void SwichCheckedEvent(bool check)
        {
            _bunifuToggleSwitch.Checked = check;

        }

        public void IconSizeEvent(int width, int height)
        {
            this._panelIcon.Size = new System.Drawing.Size(width, height);
        }

        private void BunifuToggleSwitch_Click(object sender, System.EventArgs e)
        {
            if (Item.Key == "DeviceStatus")
            {
                if (MessageBox.Show(Resources.DeviceStatusConfirm, Resources.DeviceStatus, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    ItemSelectedClicked?.Invoke(Item.Key, true);
                }
            }
            else if (Item.Key == "BitRate")
            {
                ItemSelectedClicked?.Invoke(Item.Key, true);
            }
            else
            {
                _bunifuToggleSwitch.Checked = !_bunifuToggleSwitch.Checked;
            }
        }
    }
}
