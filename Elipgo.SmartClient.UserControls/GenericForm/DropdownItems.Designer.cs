using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    partial class DropdownItems
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DropdownItems));
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            this.bunifuSeparatorItems = new Bunifu.Framework.UI.BunifuSeparator();
            this.optionItems = new Bunifu.UI.WinForms.BunifuDropdown();
            this.labelName = new System.Windows.Forms.Label();
            this.comboBoxItem = new System.Windows.Forms.ComboBox();
            this.clearTextImage = new Bunifu.Framework.UI.BunifuImageButton();
            this.FindButton = new Bunifu.Framework.UI.BunifuImageButton();
            this.txtFilter = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.buttonContexMenu = new Bunifu.Framework.UI.BunifuImageButton();
            ((System.ComponentModel.ISupportInitialize)(this.clearTextImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FindButton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonContexMenu)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuSeparatorItems
            // 
            this.bunifuSeparatorItems.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparatorItems.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparatorItems.LineThickness = 1;
            this.bunifuSeparatorItems.Location = new System.Drawing.Point(21, 64);
            this.bunifuSeparatorItems.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparatorItems.Name = "bunifuSeparatorItems";
            this.bunifuSeparatorItems.Size = new System.Drawing.Size(282, 4);
            this.bunifuSeparatorItems.TabIndex = 30;
            this.bunifuSeparatorItems.Transparency = 255;
            this.bunifuSeparatorItems.Vertical = false;
            // 
            // optionItems
            // 
            this.optionItems.BackColor = System.Drawing.Color.Transparent;
            this.optionItems.BorderRadius = 1;
            this.optionItems.Color = System.Drawing.Color.Transparent;
            this.optionItems.Cursor = System.Windows.Forms.Cursors.Hand;
            this.optionItems.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.optionItems.DisabledColor = System.Drawing.Color.Gray;
            this.optionItems.DisplayMember = "Name";
            this.optionItems.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.optionItems.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.optionItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.optionItems.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.optionItems.FillDropDown = true;
            this.optionItems.FillIndicator = false;
            this.optionItems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optionItems.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionItems.ForeColor = System.Drawing.Color.White;
            this.optionItems.FormattingEnabled = true;
            this.optionItems.Icon = null;
            this.optionItems.IndicatorColor = System.Drawing.Color.White;
            this.optionItems.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.optionItems.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionItems.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionItems.ItemForeColor = System.Drawing.Color.White;
            this.optionItems.ItemHeight = 26;
            this.optionItems.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.optionItems.Location = new System.Drawing.Point(13, 36);
            this.optionItems.Margin = new System.Windows.Forms.Padding(2);
            this.optionItems.Name = "optionItems";
            this.optionItems.Size = new System.Drawing.Size(292, 32);
            this.optionItems.TabIndex = 29;
            this.optionItems.Text = null;
            this.optionItems.ValueMember = "Key";
            this.optionItems.SelectedIndexChanged += new System.EventHandler(this.optionItems_SelectedIndexChanged);
            this.optionItems.Click += new System.EventHandler(this.optionItems_Click);
            this.optionItems.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.optionItems_KeyPress);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.ForeColor = System.Drawing.Color.Silver;
            this.labelName.Location = new System.Drawing.Point(18, 20);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(51, 15);
            this.labelName.TabIndex = 32;
            this.labelName.Text = Resources.Name;// "Nombre";
            // 
            // comboBoxItem
            // 
            this.comboBoxItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.comboBoxItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxItem.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboBoxItem.ForeColor = System.Drawing.Color.White;
            this.comboBoxItem.FormattingEnabled = true;
            this.comboBoxItem.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxItem.Location = new System.Drawing.Point(21, 42);
            this.comboBoxItem.Name = "comboBoxItem";
            this.comboBoxItem.Size = new System.Drawing.Size(282, 21);
            this.comboBoxItem.TabIndex = 37;
            this.comboBoxItem.Visible = false;
            this.comboBoxItem.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBoxItem_DrawItem);
            this.comboBoxItem.Click += new System.EventHandler(this.comboBoxItem_Click);
            this.comboBoxItem.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBoxItem_KeyPress);
            // 
            // clearTextImage
            // 
            this.clearTextImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.clearTextImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearTextImage.Image = ((System.Drawing.Image)(resources.GetObject("clearTextImage.Image")));
            this.clearTextImage.ImageActive = null;
            this.clearTextImage.Location = new System.Drawing.Point(237, 44);
            this.clearTextImage.Name = "clearTextImage";
            this.clearTextImage.Size = new System.Drawing.Size(15, 15);
            this.clearTextImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.clearTextImage.TabIndex = 35;
            this.clearTextImage.TabStop = false;
            this.clearTextImage.Visible = false;
            this.clearTextImage.Zoom = 10;
            this.clearTextImage.Click += new System.EventHandler(this.clearTextImage_Click);
            // 
            // FindButton
            // 
            this.FindButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.FindButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FindButton.Image = ((System.Drawing.Image)(resources.GetObject("FindButton.Image")));
            this.FindButton.ImageActive = null;
            this.FindButton.Location = new System.Drawing.Point(253, 45);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(18, 18);
            this.FindButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.FindButton.TabIndex = 34;
            this.FindButton.TabStop = false;
            this.FindButton.Visible = false;
            this.FindButton.Zoom = 10;
            this.FindButton.Click += new System.EventHandler(this.FindButton_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.AcceptsReturn = false;
            this.txtFilter.AcceptsTab = false;
            this.txtFilter.AnimationSpeed = 200;
            this.txtFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtFilter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtFilter.BackgroundImage")));
            this.txtFilter.BorderColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtFilter.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.txtFilter.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtFilter.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtFilter.BorderRadius = 1;
            this.txtFilter.BorderThickness = 1;
            this.txtFilter.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtFilter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFilter.DefaultFont = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.txtFilter.DefaultText = "";
            this.txtFilter.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.txtFilter.ForeColor = System.Drawing.Color.White;
            this.txtFilter.HideSelection = true;
            this.txtFilter.IconLeft = null;
            this.txtFilter.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFilter.IconPadding = 0;
            this.txtFilter.IconRight = null;
            this.txtFilter.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFilter.Lines = new string[0];
            this.txtFilter.Location = new System.Drawing.Point(21, 47);
            this.txtFilter.MaxLength = 32767;
            this.txtFilter.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtFilter.Modified = false;
            this.txtFilter.Multiline = false;
            this.txtFilter.Name = "txtFilter";
            stateProperties1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtFilter.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.Empty;
            stateProperties2.FillColor = System.Drawing.Color.White;
            stateProperties2.ForeColor = System.Drawing.Color.Empty;
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtFilter.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtFilter.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            stateProperties4.ForeColor = System.Drawing.Color.White;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtFilter.OnIdleState = stateProperties4;
            this.txtFilter.PasswordChar = '\0';
            this.txtFilter.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.txtFilter.PlaceholderText = "";
            this.txtFilter.ReadOnly = false;
            this.txtFilter.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtFilter.SelectedText = "";
            this.txtFilter.SelectionLength = 0;
            this.txtFilter.SelectionStart = 0;
            this.txtFilter.ShortcutsEnabled = true;
            this.txtFilter.Size = new System.Drawing.Size(251, 16);
            this.txtFilter.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Material;
            this.txtFilter.TabIndex = 33;
            this.txtFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtFilter.TextMarginBottom = 0;
            this.txtFilter.TextMarginLeft = 5;
            this.txtFilter.TextMarginTop = 0;
            this.txtFilter.TextPlaceholder = "";
            this.txtFilter.UseSystemPasswordChar = false;
            this.txtFilter.Visible = false;
            this.txtFilter.WordWrap = true;
            this.txtFilter.TextChange += new System.EventHandler(this.txtFilter_TextChange);
            // 
            // buttonContexMenu
            // 
            this.buttonContexMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonContexMenu.Image = ((System.Drawing.Image)(resources.GetObject("buttonContexMenu.Image")));
            this.buttonContexMenu.ImageActive = null;
            this.buttonContexMenu.Location = new System.Drawing.Point(310, 43);
            this.buttonContexMenu.Name = "buttonContexMenu";
            this.buttonContexMenu.Size = new System.Drawing.Size(24, 24);
            this.buttonContexMenu.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonContexMenu.TabIndex = 31;
            this.buttonContexMenu.TabStop = false;
            this.buttonContexMenu.Zoom = 10;
            this.buttonContexMenu.Click += new System.EventHandler(this.buttonContexMenu_Click);
            // 
            // DropdownItems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.comboBoxItem);
            this.Controls.Add(this.clearTextImage);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.bunifuSeparatorItems);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.buttonContexMenu);
            this.Controls.Add(this.optionItems);
            this.Name = "DropdownItems";
            this.Size = new System.Drawing.Size(338, 172);
            this.Resize += new System.EventHandler(this.DropdownItems_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.clearTextImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FindButton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.buttonContexMenu)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparatorItems;
        private Bunifu.UI.WinForms.BunifuDropdown optionItems;
        private Bunifu.Framework.UI.BunifuImageButton buttonContexMenu;
        private System.Windows.Forms.Label labelName;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txtFilter;
        private Bunifu.Framework.UI.BunifuImageButton FindButton;
        private Bunifu.Framework.UI.BunifuImageButton clearTextImage;
        private System.Windows.Forms.ComboBox comboBoxItem;
    }
}
