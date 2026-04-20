using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    public partial class GenericFormItemCard : UserControl
    {
        public ContentFormDTO Item { get; set; }

        private Color _backColor;

        public Action<ContentFormDTO, GenericFormItemCard> OnSelect;

        public Action<bool, ContentFormDTO> OnSwitchChange;

        public new Action<ContentFormDTO, GenericFormItemCard> OnDoubleClick;
        public string Label1Text { get => Label1.Text; set => Label1.Text = value; }
        public string Label2Text { get => label2.Text; set => label2.Text = value; }

        public Action<ContentFormDTO, GenericFormItemCard, Point> OnClickIconContextMenu;

        public GenericFormItemCard(ContentFormDTO item, ConfigGenericForm config)
        {
            InitializeComponent();
            this.Controls.Remove(PrivateIconPanel);
            this.Controls.Remove(bunifuToggleSwitch);

            if (item.IsPrivate == true && config.CanPrivate)
            {
                this.Controls.Add(PrivateIconPanel);
            }

            if (config.ShowSwitch == true)
            {
                this.Controls.Add(bunifuToggleSwitch);
                Label1.Location = new Point(Label1.Location.X, 148);
                label2.Location = new Point(label2.Location.X, 176);
                bunifuToggleSwitch.Value = item.Switch ?? false;
            }

            PrivateIconPanel.TabIndex = 15;

            EntityIconPanel.BackgroundImage = item.EntityIcon;

            Label1.Text = item.Label1;
            label2.Text = item.Label2;

            this.Name = item.Id.ToString();

            Item = item;

            this.Controls.Add(EntityIconPanel);
            _backColor = this.BackColor;

            PrivateIconPanel.Visible = item.IsPrivate ?? false;
            ResizeControls();
        }

        private void GenericFormItemCard_Click(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
            OnSelect?.Invoke(Item, this);
        }

        public void Uncheck()
        {
            this.BackColor = _backColor;
        }

        public void UpdateItem(ContentFormDTO item)
        {
            Label1.Text = item.Label1;
            label2.Text = item.Label2;
            bunifuToggleSwitch.Value = item.Switch ?? false;
            this.Name = item.Id.ToString();
            Item = item;
        }
        private void GenericFormItemCard_DoubleClick(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);

            OnDoubleClick?.Invoke(Item, this);
        }

        private void ButtonContexMenu_Click(object sender, EventArgs e)
        {
            this.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND);
            OnClickIconContextMenu?.Invoke(Item, this, Cursor.Position);
        }

        private void bunifuToggleSwitch_OnValuechange(object sender, EventArgs e)
        {
            if (Item != null)
            {
                Item.Switch = bunifuToggleSwitch.Value;
                OnSwitchChange?.Invoke(bunifuToggleSwitch.Value, this.Item);
            }
        }

        public void ResizeControls()
        {
            if (Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                if (main.Width > 1400 && main.Width < 2000)
                {
                    //lblTitle.Font = FontHelper.Get(FontSizes.Medium_3, FontName.ROBOTO_REGULAR);
                    //lblTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Bold, GraphicsUnit.Pixel);
                    //IndexEntity.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    //GroupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                    Label1.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                    label2.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 1366 && main.Width < 1400)
                {
                    Label1.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    label2.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    //GroupName.Font = FontHelper.Get(FontSizes.Medium_1, FontName.ROBOTO_REGULAR);
                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    Label1.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    label2.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    Label1.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    label2.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Regular, GraphicsUnit.Pixel);
                }

                //170, 212
                var GenericFormItemCardWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0885M), 2));
                var GenericFormItemCardHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1962M), 2));
                
                this.Size = new Size(GenericFormItemCardWidth, GenericFormItemCardHeight);
                this.MinimumSize = new Size(GenericFormItemCardWidth, GenericFormItemCardHeight);

                //24, 24
                var buttonContexMenuWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0125M), 2));
                var buttonContexMenuHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.022M), 2));
                buttonContexMenu.Size = new Size(buttonContexMenuWidth, buttonContexMenuHeight);
                //143, 9
                var buttonContexMenuX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.07447M), 2));
                var buttonContexMenuY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0084M), 2));
                buttonContexMenu.Location = new Point(buttonContexMenuX, buttonContexMenuY);

                //24, 23
                var PrivateIconPanelWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0125M), 2));
                var PrivateIconPanelHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0216M), 2));
                PrivateIconPanel.Size = new Size(PrivateIconPanelWidth, PrivateIconPanelHeight);
                //110, 25
                var PrivateIconPanelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0572M), 2));
                var PrivateIconPanelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.02314M), 2));
                PrivateIconPanel.Location = new Point(PrivateIconPanelX, PrivateIconPanelY);

                //65,65
                var EntityIconPanelWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0338M), 2));
                var EntityIconPanelHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0601M), 2));
                EntityIconPanel.Size = new Size(EntityIconPanelWidth, EntityIconPanelHeight);
                //58, 39
                var EntityIconPanelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.03020M), 2));
                var EntityIconPanelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0361M), 2));
                EntityIconPanel.Location = new Point(EntityIconPanelY, EntityIconPanelX);

                //42, 15
                var bunifuToggleSwitchWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0218M), 2));
                var bunifuToggleSwitchHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138M), 2));
                bunifuToggleSwitch.Size = new Size(bunifuToggleSwitchWidth, bunifuToggleSwitchHeight);
                //67, 120
                var bunifuToggleSwitchX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0348M), 2));
                var bunifuToggleSwitchY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1111M), 2));
                bunifuToggleSwitch.Location = new Point(bunifuToggleSwitchX, bunifuToggleSwitchY);

                //172, 45
                var Label1Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.090M), 2));
                var Label1Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.042M), 2));
                Label1.Size = new Size(Label1Width, Label1Height);
                //3, 139
                var Label1X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0016M), 2));
                var Label1Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.135M), 2));
                Label1.Location = new Point(Label1X, Label1Y);

                //172, 35
                var label2Width = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.090M), 2));
                var label2Height = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0324M), 2));
                label2.Size = new Size(label2Width, label2Height);
                //3, 176
                var label2X = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0016M), 2));
                var label2Y = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.1629M), 2));
                label2.Location = new Point(PrivateIconPanelX, PrivateIconPanelY);
            }
        }
    }
}
