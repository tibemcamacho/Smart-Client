using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Dialog
{
    public partial class DialogReadOnlyControl : UserControl
    {
        public DialogReadOnlyItemDTO ItemSelected { get; set; }
        private bool _resizeLoad = false;
        private List<DialogReadOnlyItemDTO> Elements = new List<DialogReadOnlyItemDTO>();
        private bool _painted = false;
        private readonly int MAX_ITEMS = 15;

        public DialogReadOnlyControl(string title, List<DialogReadOnlyItemDTO> elements, string buttonOKText, string buttonCancelText)
        {
            InitializeComponent();
            DialogReadOnlyControlResize();
            ScrollBar.Maximum = TableLayoutPanel.VerticalScroll.Maximum;
            ScrollBar.ThumbLength = 220;
            ScrollBar.BindingContainer = TableLayoutPanel;
            _resizeLoad = true;
            LabelTitle.Text = title;
            ButtonOK.Text = buttonOKText;
            ButtonCancel.Text = buttonCancelText;

            ScrollBar.BackgroundColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            LabelTitle.ForeColor = ColorTranslator.FromHtml(VariableResources.COLOR_TITLE_FONT);
            BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_GRAY_CONTEXT_MENU);
            PanelHeader.BackColor = ColorTranslator.FromHtml(VariableResources.COLOR_HEADER_BACKGROUND);

            Elements = elements;
            this.Paint += DialogReadOnlyControl_Paint;
        }
        int scrollPosition = 0;
        public void DialogReadOnlyControlResize()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var workingArea = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                if (workingArea.Width > 1400 && workingArea.Width < 2000)
                {
                    LabelTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_5, FontStyle.Bold, GraphicsUnit.Pixel);
                    this.ButtonOK.IdleBorderRadius = 30;
                    this.ButtonOK.OnIdleState.BorderRadius = 30;

                    this.ButtonCancel.IdleBorderRadius = 30;
                    this.ButtonCancel.OnIdleState.BorderRadius = 30;
                    ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (workingArea.Width > 1366 && workingArea.Width <= 1400)
                {
                }
                else if (workingArea.Width <= 1366)
                {
                    LabelTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_3, FontStyle.Bold, GraphicsUnit.Pixel);
                    this.ButtonOK.IdleBorderRadius = 20;
                    this.ButtonOK.OnIdleState.BorderRadius = 20;

                    this.ButtonCancel.IdleBorderRadius = 20;
                    this.ButtonCancel.OnIdleState.BorderRadius = 20;

                    ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (workingArea.Width >= 2000 && workingArea.Width < 2560)
                {
                    LabelTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_5, FontStyle.Bold, GraphicsUnit.Pixel);
                    this.ButtonOK.IdleBorderRadius = 30;
                    this.ButtonOK.OnIdleState.BorderRadius = 30;

                    this.ButtonCancel.IdleBorderRadius = 30;
                    this.ButtonCancel.OnIdleState.BorderRadius = 30;
                    ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (workingArea.Width >= 2560 && workingArea.Width <= 3440)
                {
                    LabelTitle.Font = FontHelper.GetRobotoRegular(FontSizes.Large_5, FontStyle.Bold, GraphicsUnit.Pixel);
                    this.ButtonOK.IdleBorderRadius = 30;
                    this.ButtonOK.OnIdleState.BorderRadius = 30;

                    this.ButtonCancel.IdleBorderRadius = 30;
                    this.ButtonCancel.OnIdleState.BorderRadius = 30;
                    ButtonOK.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    ButtonCancel.Font = FontHelper.GetRobotoRegular(FontSizes.Large_1, FontStyle.Regular, GraphicsUnit.Pixel);
                }

                var labelTitleHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.034M), 2));
                LabelTitle.Size = new Size(LabelTitle.Width, labelTitleHeight);
                var thisWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0895M), 2));
                var thisHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.042M), 2));

                var tableLayoutPanelWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.469M), 2));
                var tableLayoutPanelHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.656M), 2));
                TableLayoutPanel.Size = new Size(tableLayoutPanelWidth, tableLayoutPanelHeight);

                var tableLayoutPanelColumnX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0112M), 2));
                var tableLayoutPanelColumnY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.0840M), 2));
                TableLayoutPanel.Location = new Point(tableLayoutPanelColumnX, tableLayoutPanelColumnY);


                var tableLayoutPanelColumnWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0940M), 2));
                for (int col = 0; col < TableLayoutPanel.ColumnCount; col++)
                {
                    TableLayoutPanel.ColumnStyles[col].SizeType = SizeType.Absolute;
                    TableLayoutPanel.ColumnStyles[col].Width = tableLayoutPanelColumnWidth;
                }
                if (scrollPosition == 1)
                {
                    scrollPosition = 0;
                }
                else
                {
                    scrollPosition = 1;
                }

                TableLayoutPanel.VerticalScroll.Value = scrollPosition;

                var scrollBarX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.482M), 2));
                var scrollBarY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.084M), 2));
                ScrollBar.Location = new Point(scrollBarX, scrollBarY);

                var scrollBarWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0085M), 2));
                var scrollBarHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.656M), 2));
                ScrollBar.Size = new Size(scrollBarWidth, scrollBarHeight);

                var buttonCloseX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.482M), 2));
                var buttonCloseY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.004M), 2));
                ButtonClose.Location = new Point(buttonCloseX, buttonCloseY);

                var buttonCloseWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.0125M), 2));
                var buttonCloseHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.022M), 2));
                ButtonClose.Size = new Size(buttonCloseWidth, buttonCloseHeight);


                var buttonCancelX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.392M), 2));
                var buttonCancelY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.765M), 2));
                ButtonCancel.Location = new Point(buttonCancelX, buttonCancelY);

                var buttonCancelWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.048M), 2));
                var buttonCancelHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.034M), 2));
                ButtonCancel.Size = new Size(buttonCancelWidth, buttonCancelHeight);


                var buttonOKX = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.441M), 2));
                var buttonOKY = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.765M), 2));
                ButtonOK.Location = new Point(buttonOKX, buttonOKY);

                var buttonOKWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Width * 0.048M), 2));
                var buttonOKHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(workingArea.Height * 0.034M), 2));
                ButtonOK.Size = new Size(buttonOKWidth, buttonOKHeight);
                _resizeLoad = false;
            }
        }

        private void DialogReadOnlyControl_Paint(object sender, PaintEventArgs e)
        {
            if (this._painted)
            {
                return;
            }

            this.TableLayoutPanel.Hide();
            foreach (DialogReadOnlyItemDTO element in Elements)
            {
                var control = new DialogReadOnlyItemControl(element);
                control.ItemSelected += Control_ItemSelected;
                control.ItemDoubleClick += Control_ItemDoubleClick;
                this.TableLayoutPanel.Controls.Add(control);
            }
            this.TableLayoutPanel.Show();
            ScrollVisibility();
            this._painted = true;
        }

        private void Control_ItemDoubleClick(object sender, DialogReadOnlyItemDTO grid)
        {
            Control_ItemSelected(sender, grid);
            ButtonOK.DialogResult = DialogResult.None;
            if (ItemSelected != null && ItemSelected.Id != null)
            {
                this.ParentForm.DialogResult = DialogResult.OK;
            }
        }

        private void Control_ItemSelected(object sender, DialogReadOnlyItemDTO item)
        {
            var control = (DialogReadOnlyItemControl)sender;
            foreach (DialogReadOnlyItemControl c in this.TableLayoutPanel.Controls.OfType<DialogReadOnlyItemControl>())
            {
                if (c != control)
                {
                    c.Selected = false;
                }
                else
                {
                    c.Selected = true;
                    ItemSelected = item;
                }
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            ButtonOK.DialogResult = DialogResult.None;
            if (ItemSelected != null && ItemSelected.Id != null)
            {
                this.ParentForm.DialogResult = DialogResult.OK;
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.ParentForm.DialogResult = DialogResult.Cancel;
        }

        private void ScrollVisibility()
        {
            if (Elements.Count > MAX_ITEMS)
            {
                ScrollBar.Show();
            }
            else
            {
                ScrollBar.Hide();
            }
        }
    }
}
