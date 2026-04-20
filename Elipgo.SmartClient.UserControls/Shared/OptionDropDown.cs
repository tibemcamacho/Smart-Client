using Elipgo.SmartClient.Common.DTOs;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Shared
{
    public partial class OptionDropDown : UserControl
    {
        public event EventHandler<OptionObjectDTO> OptionButtonClicked;

        public OptionDropDown()
        {
            InitializeComponent();
            this.Disposed += OptionDropDown_Disposed;
        }

        private void OptionDropDown_Disposed(object sender, EventArgs e)
        {
            this.OptionBtn.Click -= OptionButton_Click;
            this.Disposed -= OptionDropDown_Disposed;
        }

        public void SetOption(string text)
        {
            this.OptionBtn.Text = "".PadLeft(6) + text;
            this.OptionBtn.Click += OptionButton_Click;
        }

        public void SetIconOption(string pathImage)
        {
            switch (pathImage)
            {
                case "Locations":
                    this.OptionBtn.Image = ((System.Drawing.Image)(Elipgo.SmartClient.UserControls.Properties.Resources.icon_location));
                    break;
                case "Devices":
                    this.OptionBtn.Image = ((System.Drawing.Image)(Elipgo.SmartClient.UserControls.Properties.Resources.icon_videocam));
                    break;
                case "Analytics":
                    this.OptionBtn.Image = ((System.Drawing.Image)(Elipgo.SmartClient.UserControls.Properties.Resources.icon_analitics_up));
                    break;
                case "Carousels":
                    this.OptionBtn.Image = ((System.Drawing.Image)(Elipgo.SmartClient.UserControls.Properties.Resources.icon_carruseles));
                    break;
                case "AlarmsMap":
                    this.OptionBtn.Image = ((System.Drawing.Image)(Elipgo.SmartClient.UserControls.Properties.Resources.icon_location));
                    break;
                default:
                    break;
            }
        }

        public void SetIconOption(Image image)
        {
            this.OptionBtn.Image = image;
        }

        private void OptionButton_Click(object sender, EventArgs e)
        {
            var dataOption = new OptionObjectDTO()
            {
                Name = this.OptionBtn.Text.Trim(),
                Key = this.Name,
                Item = this.OptionBtn.Image

            };
            OptionButtonClicked?.Invoke(this, dataOption);
        }
    }
}
