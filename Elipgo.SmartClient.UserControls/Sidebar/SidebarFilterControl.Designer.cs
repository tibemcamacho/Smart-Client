namespace Elipgo.SmartClient.UserControls.Sidebar
{
    partial class SidebarFilterControl
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
            this.components = new System.ComponentModel.Container();
            this.PanelButton = new System.Windows.Forms.Panel();
            this.filterButton = new System.Windows.Forms.Panel();
            this.comboBoxFilter = new Bunifu.UI.WinForms.BunifuDropdown();
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.PanelButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // PanelButton
            // 
            this.PanelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.PanelButton.BackColor = System.Drawing.Color.Transparent;
            this.PanelButton.Controls.Add(this.filterButton);
            this.PanelButton.Controls.Add(this.comboBoxFilter);
            this.PanelButton.Location = new System.Drawing.Point(0, 0);
            this.PanelButton.Margin = new System.Windows.Forms.Padding(0);
            this.PanelButton.Name = "PanelButton";
            this.PanelButton.Size = new System.Drawing.Size(266, 48);
            this.PanelButton.TabIndex = 0;
            this.bunifuToolTip1.SetToolTip(this.PanelButton, "");
            this.bunifuToolTip1.SetToolTipIcon(this.PanelButton, null);
            this.bunifuToolTip1.SetToolTipTitle(this.PanelButton, "");
            // 
            // filterButton
            // 
            this.filterButton.BackColor = System.Drawing.Color.Transparent;
            this.filterButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.filterButton.Location = new System.Drawing.Point(229, 14);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(24, 24);
            this.filterButton.TabIndex = 9;
            this.bunifuToolTip1.SetToolTip(this.filterButton, "");
            this.bunifuToolTip1.SetToolTipIcon(this.filterButton, null);
            this.bunifuToolTip1.SetToolTipTitle(this.filterButton, "");
            this.filterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // comboBoxFilter
            // 
            this.comboBoxFilter.BackColor = System.Drawing.Color.Transparent;
            this.comboBoxFilter.BorderRadius = 0;
            this.comboBoxFilter.Color = System.Drawing.Color.Transparent;
            this.comboBoxFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBoxFilter.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.comboBoxFilter.DisabledColor = System.Drawing.Color.Gray;
            this.comboBoxFilter.DisplayMember = "Name";
            this.comboBoxFilter.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxFilter.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thick;
            this.comboBoxFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilter.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.comboBoxFilter.FillDropDown = false;
            this.comboBoxFilter.FillIndicator = false;
            this.comboBoxFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.comboBoxFilter.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.comboBoxFilter.ForeColor = System.Drawing.Color.White;
            this.comboBoxFilter.FormattingEnabled = true;
            this.comboBoxFilter.Icon = null;
            this.comboBoxFilter.IndicatorColor = System.Drawing.Color.White;
            this.comboBoxFilter.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.comboBoxFilter.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.comboBoxFilter.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.comboBoxFilter.ItemForeColor = System.Drawing.Color.White;
            this.comboBoxFilter.ItemHeight = 32;
            this.comboBoxFilter.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.comboBoxFilter.Location = new System.Drawing.Point(0, 5);
            this.comboBoxFilter.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.comboBoxFilter.Name = "comboBoxFilter";
            this.comboBoxFilter.Size = new System.Drawing.Size(164, 38);
            this.comboBoxFilter.TabIndex = 2;
            this.comboBoxFilter.Text = null;
            this.bunifuToolTip1.SetToolTip(this.comboBoxFilter, "");
            this.bunifuToolTip1.SetToolTipIcon(this.comboBoxFilter, null);
            this.bunifuToolTip1.SetToolTipTitle(this.comboBoxFilter, "");
            this.comboBoxFilter.ValueMember = "Key";
            this.comboBoxFilter.SelectedIndexChanged += new System.EventHandler(this.ComboBoxFilter_SelectedIndexChanged);
            // 
            // bunifuToolTip1
            // 
            this.bunifuToolTip1.Active = true;
            this.bunifuToolTip1.AlignTextWithTitle = false;
            this.bunifuToolTip1.AllowAutoClose = true;
            this.bunifuToolTip1.AllowFading = true;
            this.bunifuToolTip1.AutoCloseDuration = 2000;
            this.bunifuToolTip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.bunifuToolTip1.BorderColor = System.Drawing.Color.Black;
            this.bunifuToolTip1.ClickToShowDisplayControl = false;
            this.bunifuToolTip1.ConvertNewlinesToBreakTags = true;
            this.bunifuToolTip1.DisplayControl = null;
            this.bunifuToolTip1.EntryAnimationSpeed = 350;
            this.bunifuToolTip1.ExitAnimationSpeed = 200;
            this.bunifuToolTip1.GenerateAutoCloseDuration = false;
            this.bunifuToolTip1.IconMargin = 6;
            this.bunifuToolTip1.InitialDelay = 0;
            this.bunifuToolTip1.Name = "bunifuToolTip1";
            this.bunifuToolTip1.Opacity = 1D;
            this.bunifuToolTip1.OverrideToolTipTitles = false;
            this.bunifuToolTip1.Padding = new System.Windows.Forms.Padding(10);
            this.bunifuToolTip1.ReshowDelay = 100;
            this.bunifuToolTip1.ShowAlways = true;
            this.bunifuToolTip1.ShowBorders = false;
            this.bunifuToolTip1.ShowIcons = true;
            this.bunifuToolTip1.ShowShadows = true;
            this.bunifuToolTip1.Tag = null;
            this.bunifuToolTip1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this.bunifuToolTip1.TextForeColor = System.Drawing.Color.White;
            this.bunifuToolTip1.TextMargin = 2;
            this.bunifuToolTip1.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bunifuToolTip1.TitleForeColor = System.Drawing.Color.Black;
            this.bunifuToolTip1.ToolTipPosition = new System.Drawing.Point(0, 0);
            this.bunifuToolTip1.ToolTipTitle = null;
            // 
            // SidebarFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.PanelButton);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "SidebarFilterControl";
            this.Size = new System.Drawing.Size(267, 48);
            this.bunifuToolTip1.SetToolTip(this, "");
            this.bunifuToolTip1.SetToolTipIcon(this, null);
            this.bunifuToolTip1.SetToolTipTitle(this, "");
            this.PanelButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelButton;
        private Bunifu.UI.WinForms.BunifuDropdown comboBoxFilter;
        private System.Windows.Forms.Panel filterButton;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
    }
}
