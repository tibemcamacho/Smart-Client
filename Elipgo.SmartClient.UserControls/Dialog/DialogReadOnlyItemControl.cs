using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Dialog
{
    public delegate void OnItemSelected(object sender, DialogReadOnlyItemDTO grid);

    public partial class DialogReadOnlyItemControl : UserControl
    {
        public event OnItemSelected ItemSelected;
        public event OnItemSelected ItemDoubleClick;
        private bool _resizeLoad = false;
        private DialogReadOnlyItemDTO Item { get; set; }

        public DialogReadOnlyItemControl(DialogReadOnlyItemDTO item)
        {
            Item = item;

            InitializeComponent();
            _resizeLoad = true;
            Anchor = AnchorStyles.Top;
            BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            Selected = Item.IsSelected;

            Icon.BackgroundImage = Image.FromFile(item.Icon);
            Icon.Width = 72;
            Icon.Height = 72;

            LabelName.ForeColor = ColorTranslator.FromHtml(VariableResources.COLOR_TITLE_FONT);
            LabelName.Text = Item.Name;
            LabelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);

            this.DoubleClick += DialogReadOnlyItemControl_DoubleClick;
            this.Icon.DoubleClick += DialogReadOnlyItemControl_DoubleClick;
            this.LabelName.DoubleClick += DialogReadOnlyItemControl_DoubleClick;
            DialogReadOnlyItemControlResize();
            this.LocationChanged += DialogReadOnlyItemControl_LocationChanged;
        }

        private void DialogReadOnlyItemControl_LocationChanged(object sender, EventArgs e)
        {
            DialogReadOnlyItemControlResize();
        }

        private void DialogReadOnlyItemControl_DoubleClick(object sender, EventArgs e)
        {
            ItemDoubleClick(this, Item);
        }

        private void DialogReadOnlyItemControl_Click(object sender, EventArgs e)
        {
            if (!Selected)
            {
                var handle = ItemSelected;
                if (handle != null)
                {
                    ItemSelected(this, Item);
                }
            }
        }

        private bool _selected;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                Item.IsSelected = value;
                this.BackColor = value ? ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_BACKGROUND) : ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            }
        }

        private void DialogReadOnlyItemControlResize()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                var iconWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0375M), 2));
                var iconHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.067M), 2));
                Icon.Size = new Size(iconWidth, iconHeight);

                var iconX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0290M), 2));
                var iconY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.050M), 2));
                Icon.Location = new Point(iconX, iconY);


                var thisWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0940M), 2));
                var thisHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.204M), 2));
                this.Size = new Size(thisWidth, thisHeight);
                this.Refresh();
                Console.Write("this.Size --> " + this.Size);


                var labelNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0940M), 2));
                var labelNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.055M), 2));
                LabelName.Size = new Size(labelNameWidth, labelNameHeight);

                var labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.146M), 2));
                LabelName.Location = new Point(0, labelNameY);
                _resizeLoad = false;
            }
        }
    }
}
