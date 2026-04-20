using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    public partial class DropdownItems : UserControl
    {
        //private const int MarginWidth = 4;
        //private const int MarginHeight = 4;
        //private int optionIndex = -1;
        private bool _resizeLoad = false;

        public List<ContentFormDTO> Items { get; set; }
        public ContentFormDTO Item { get; set; }

        public Action<ContentFormDTO, Point> OnClickIconDropdownItems;

        /// <summary>
        /// Propiedas que se encarga de obtener y establecer el texto del combo.
        /// </summary>
        public string SelectItem { get => optionItems.SelectedText; set => optionItems.SelectedText = value; }
        /// <summary>
        /// Propiedas que se encarga de obtener y establecer el select value de combo de opciones
        /// </summary>
        public string SelectValue { get => (optionState ? comboBoxItem.SelectedValue.ToString() : optionItems.SelectedValue.ToString()); }// set => ((optionState == true) ? comboBoxItem.SelectedValue : optionItems.SelectedValue) = value; }
        /// <summary>
        /// Propiedas que se encarga de obtener el estatus de visible de txt de busqueda
        /// </summary>
        public bool txtBusquedaVisible { get => txtFilter.Visible; set => txtFilter.Visible = value; }

        public int optionCount { get => optionItems.Items.Count; }

        public int comboBoxCount { get => comboBoxItem.Items.Count; }
        public bool optionState { get; set; }

        public DropdownItems()//List<ContentFormDTO> items
        {
            InitializeComponent();
            // Make the ComboBox owner-drawn.
            optionItems.DrawMode = DrawMode.OwnerDrawFixed;
            optionItems.Items.Clear();
            optionItems.DropDownStyle = ComboBoxStyle.DropDownList;
            txtFilter.LostFocus += txtFilter_Lostfocus;
            _resizeLoad = true;
            comboBoxItem.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxItem.DropDownStyle = ComboBoxStyle.DropDownList;

            resize();
        }

        private void txtFilter_Lostfocus(object Sender, EventArgs e)
        {
            if (this.txtFilter.Visible)
            {
                this.FindButton.Visible = false;
                this.clearTextImage.Visible = false;
                this.txtFilter.Visible = false;
                this.optionItems.DroppedDown = false;
                this.comboBoxItem.DroppedDown = false;
            }

        }
        private void optionItems_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        public void DataSourceItems(List<ContentFormDTO> items, bool bUpdatelist = true)
        {
            if (bUpdatelist)
                Items = items;
            var IdSelect = 0;
            var list = new List<OptionItemDTO<int>>();
            var list1 = new List<OptionItemDTO>();
            foreach (var item in items)
            {
                if (item.Switch == true)
                    IdSelect = item.Id;

                list.Add(new OptionItemDTO<int> { Name = item.Label1, Key = item.Id, Item = item.EntityIcon, Tag = string.Empty });
                list1.Add(new OptionItemDTO { Name = item.Label1, Key = item.Id });
            }

            if (optionState)
            {
                comboBoxItem.DataSource = list1;
                comboBoxItem.DisplayMember = "Name";
                comboBoxItem.ValueMember = "Key";
                optionItems.Visible = false;
                comboBoxItem.Visible = true;
            }
            else
            {
                optionItems.DataSource = list;
                optionItems.Visible = true;
                comboBoxItem.Visible = false;
            }

        }

        private void buttonContexMenu_Click(object sender, EventArgs e)
        {
            if (!optionState && !this.txtFilter.Visible && optionItems.Items.Count > 0)
                OnClickIconDropdownItems?.Invoke(Item, Cursor.Position);
            else if (optionState && !this.txtFilter.Visible && comboBoxItem.Items.Count > 0)
                OnClickIconDropdownItems?.Invoke(Item, Cursor.Position);
        }

        private void optionItems_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.txtFilter.Visible = true;
            this.clearTextImage.Visible = true;
            this.txtFilter.Focus();
            this.txtFilter.Text = e.KeyChar.ToString();
            this.FindButton.Visible = true;
            this.FindButton.BringToFront();
            Filtertxt();
        }

        private void optionItems_Click(object sender, EventArgs e)
        {
            this.txtFilter.Visible = false;
            this.txtFilter.Text = String.Empty;
            this.FindButton.Visible = false;
            this.clearTextImage.Visible = false;

            bool bcount = (optionState ? this.comboBoxItem.Items.Count > 0 : this.optionItems.Items.Count > 0);

            if (!bcount)
            {
                this.optionItems.DroppedDown = false;
                this.comboBoxItem.DroppedDown = false;
                if (Items != null)
                    DataSourceItems(Items, false);

            }

            this.optionItems.Focus();
        }


        private void FindButton_Click(object sender, EventArgs e)
        {
            Filtertxt();
        }

        private void Filtertxt()
        {
            if (optionState)
            {
                comboBoxItem.DataSource = new List<OptionItemDTO>(); ;
                comboBoxItem.SelectedIndex = -1;
                comboBoxItem.Text = String.Empty;
            }
            else
            {
                optionItems.DataSource = new List<OptionItemDTO>();
                optionItems.SelectedIndex = -1;
                optionItems.Text = String.Empty;
            }
            var filter = txtFilter.Text.Trim().ToLower();

            if (filter == string.Empty)
                DataSourceItems(Items, false);

            var filterData = Items.Where(p => (p.Label1?.ToLower().Contains(filter) == true)).ToList();


            DataSourceItems(filterData, false);

            this.FindButton.Visible = false;
            this.clearTextImage.Visible = false;
            if (filterData.Count > 0)
                if (optionState)
                    comboBoxItem.DroppedDown = true;
                else
                {
                    optionItems.DroppedDown = true;
                }
        }

        private void clearTextImage_Click(object sender, EventArgs e)
        {
            filer_clear();
        }

        private void filer_clear()
        {
            txtFilter.Text = string.Empty;

            if (optionItems.Items.Count != Items.Count)
                DataSourceItems(Items, false);
        }

        public void resize()
        {
            if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
            {
                var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;

                //
                if (main.Width > 1400 && main.Width < 2000)
                {
                    optionItems.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    comboBoxItem.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    txtFilter.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_1, FontStyle.Regular, GraphicsUnit.Pixel);
                    labelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 1366 && main.Width < 1400)
                {
                    optionItems.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    comboBoxItem.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    txtFilter.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_0, FontStyle.Regular, GraphicsUnit.Pixel);
                    labelName.Font = FontHelper.GetRobotoRegular(FontSizes.Small_4, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 2000 && main.Width < 2560)
                {
                    optionItems.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    comboBoxItem.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    txtFilter.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_2, FontStyle.Regular, GraphicsUnit.Pixel);
                    labelName.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                }
                else if (main.Width >= 2560 && main.Width <= 3440)
                {
                    optionItems.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    comboBoxItem.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    txtFilter.Font = FontHelper.GetRobotoRegular(FontSizes.Medium_4, FontStyle.Regular, GraphicsUnit.Pixel);
                    labelName.Font = FontHelper.GetRobotoRegular(FontSizes.Large_0, FontStyle.Regular, GraphicsUnit.Pixel);
                }

                //51, 15
                var labelNameWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0265M), 2));
                var labelNameHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138M), 2));
                labelName.Size = new Size(labelNameWidth, labelNameHeight);

                //18, 20
                var labelNameX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0093M), 2));
                var labelNameY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0185M), 2));
                labelName.Location = new Point(labelNameX, labelNameY);

                //251,16 20
                var txtFilterWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1307M), 2));
                var txtFilterHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0148M), 2));
                txtFilter.Size = new Size(txtFilterWidth, txtFilterHeight);

                //21, 47
                var txtFilterX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0109M), 2));
                var txtFilterY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0435M), 2));
                txtFilter.Location = new Point(txtFilterX, txtFilterY);

                //292, 32
                var optionItemsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1520M), 2));
                var optionItemsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0296M), 2));
                optionItems.Size = new Size(optionItemsWidth, optionItemsHeight);

                //21, 37
                var optionItemsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0109M), 2));
                var optionItemsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0342M), 2));
                optionItems.Location = new Point(optionItemsX, optionItemsY);
                this.optionItems.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //comboBoxItem
                //282, 21
                var comboBoxItemWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1468M), 2));
                var comboBoxItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0194M), 2));
                comboBoxItem.Size = new Size(comboBoxItemWidth, comboBoxItemHeight);

                //21, 42
                var comboBoxItemX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0109M), 2));
                var comboBoxItemY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0388M), 2));
                comboBoxItem.Location = new Point(comboBoxItemX, comboBoxItemY);
                //this.comboBoxItem.ItemHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.030M), 2));

                //282, 6
                var bunifuSeparatorItemsWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1468M), 2));
                var bunifuSeparatorItemsHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0037M), 2));
                bunifuSeparatorItems.Size = new Size(bunifuSeparatorItemsWidth, bunifuSeparatorItemsHeight);
                //21, 63
                var bunifuSeparatorItemsX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0109M), 2));
                var bunifuSeparatorItemsY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0601M), 2));
                bunifuSeparatorItems.Location = new Point(bunifuSeparatorItemsX, bunifuSeparatorItemsY);
                bunifuSeparatorItems.BringToFront();

                //310, 43
                var buttonContexMenuX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1614M), 2));
                var buttonContexMenuY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0398M), 2));
                buttonContexMenu.Location = new Point(buttonContexMenuX, buttonContexMenuY);

                //24, 24
                var buttonContexMenuWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0125M), 2));
                var buttonContexMenuHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0222M), 2));
                buttonContexMenu.Size = new Size(buttonContexMenuWidth, buttonContexMenuHeight);

                //15, 15
                var clearTextImageWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0078M), 2));
                var clearTextImageHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0138M), 2));
                clearTextImage.Size = new Size(clearTextImageWidth, clearTextImageHeight);

                //237, 44
                var clearTextImageX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1234M), 2));
                var clearTextImageY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0407M), 2));
                clearTextImage.Location = new Point(clearTextImageX, clearTextImageY);

                //18, 18
                var FindButtonWidth = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.0093M), 2));
                var FindButtonHeight = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0166M), 2));
                FindButton.Size = new Size(FindButtonWidth, FindButtonHeight);
                //253, 45
                var FindButtonX = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Width * 0.1317M), 2));
                var FindButtonY = Convert.ToInt32(Math.Round(Convert.ToDecimal(main.Height * 0.0416M), 2));
                FindButton.Location = new Point(FindButtonX, FindButtonY);
                _resizeLoad = false;
            }
        }

        private void DropdownItems_Resize(object sender, EventArgs e)
        {
            _resizeLoad = true;
            resize();
        }

        private void comboBoxItem_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index >= 0)
            {
                if (e.Index < comboBoxItem.Items.Count)
                {
                    var ImgSize = new Size(15, 15);
                    if ((_resizeLoad || Screen.AllScreens.Length > 1) && Screen.AllScreens.Any(x => x.WorkingArea.Contains(Cursor.Position)))
                    {
                        var main = Screen.AllScreens.First(s => s.WorkingArea.Contains(Cursor.Position)).Bounds;
                        if (main.Width > 1400 && main.Width < 2000)
                            ImgSize = new Size(15, 15);
                        else if (main.Width >= 1366 && main.Width < 1400)
                            ImgSize = new Size(10, 10);

                    }

                    Image imgOn = new Bitmap(Elipgo.SmartClient.UserControls.Properties.Resources.icon_input_state_on, ImgSize);
                    Image imgOff = new Bitmap(Elipgo.SmartClient.UserControls.Properties.Resources.icon_input_state_off, ImgSize);

                    var item = (Elipgo.SmartClient.Common.DTOs.OptionItemDTO)comboBoxItem.Items[e.Index];
                    var disponible = Items.SingleOrDefault(t => t.Id == item.Key);

                    e.Graphics.DrawImage((disponible.Switch == true ? imgOn : imgOff), new PointF(e.Bounds.Left, e.Bounds.Top));



                    e.Graphics.DrawString(string.Format(((Elipgo.SmartClient.Common.DTOs.OptionItemDTO)comboBoxItem.Items[e.Index]).Name)
                    , e.Font, new SolidBrush(e.ForeColor)
                    , e.Bounds.Left + 16, e.Bounds.Top);
                }
            }
        }

        private void comboBoxItem_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.txtFilter.Visible = true;
            this.clearTextImage.Visible = true;
            this.txtFilter.Focus();
            this.txtFilter.Text = e.KeyChar.ToString();
            this.txtFilter.BringToFront();
            this.FindButton.Visible = true;
            this.FindButton.BringToFront();
            Filtertxt();
        }

        private void comboBoxItem_Click(object sender, EventArgs e)
        {

            this.txtFilter.Visible = false;
            this.txtFilter.Text = String.Empty;
            this.FindButton.Visible = false;
            this.clearTextImage.Visible = false;

            if (this.comboBoxItem.Items.Count == 0)
            {
                DataSourceItems(Items, false);
                this.comboBoxItem.DroppedDown = true;
            }

            this.comboBoxItem.Focus();
        }

        private void txtFilter_TextChange(object sender, EventArgs e)
        {
            Filtertxt();
        }
    }
}
