using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.RecorderDropDown
{
    public partial class DropDownOption : UserControl
    {
        public event EventHandler<OptionObjectDTO> ButtonOptionClicked;

        public string Key { get; set; }
        public RecorderDTOSmall Item { get; set; }

        private readonly string path = AppDomain.CurrentDomain.BaseDirectory;


        public DropDownOption()
        {
            InitializeComponent();
        }

        public void SetText(string text)
        {
            ButtonItem.Text = text;
        }

        public void ShowImage(bool showImage = false)
        {
            if (showImage)
            {
                ButtonItem.Image = FileResources.icon_device_undefined;
            }
            else
            {
                ButtonItem.Image = null;
            }
        }

        public void SetImage(bool state, bool showImage = false)
        {
            if (showImage)
            {
                if (state)
                {
                    ButtonItem.Image = FileResources.icon_device_online;
                }
                else
                {
                    ButtonItem.Image = FileResources.icon_device_offline;
                }
            }
            else
            {
                ButtonItem.Image = null;
            }
        }

        private void ButtonItem_Click(object sender, EventArgs e)
        {
            var dataOption = new OptionObjectDTO()
            {
                Name = this.ButtonItem.Text,
                Key = this.Key,
                Item = this.Item

            };
            ButtonOptionClicked?.Invoke(this, dataOption);
        }
    }
}
