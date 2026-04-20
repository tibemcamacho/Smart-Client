namespace Elipgo.SmartClient.UserControls.Sidebar
{
    partial class SidebarDdlFilterControl
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
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.filterButton = new System.Windows.Forms.Panel();
            this.PanelContainer = new System.Windows.Forms.Panel();
            this.Dropdown = new Elipgo.SmartClient.UserControls.Shared.Dropdown();
            this.PanelContainer.SuspendLayout();
            this.SuspendLayout();
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
            // filterButton
            // 
            this.filterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.filterButton.BackColor = System.Drawing.Color.Transparent;
            this.filterButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.filterButton.Location = new System.Drawing.Point(108, 6);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(26, 26);
            this.filterButton.TabIndex = 9;
            this.bunifuToolTip1.SetToolTip(this.filterButton, "");
            this.bunifuToolTip1.SetToolTipIcon(this.filterButton, null);
            this.bunifuToolTip1.SetToolTipTitle(this.filterButton, "");
            this.filterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // PanelContainer
            // 
            this.PanelContainer.BackColor = System.Drawing.Color.Transparent;
            this.PanelContainer.Controls.Add(this.Dropdown);
            this.PanelContainer.Controls.Add(this.filterButton);
            this.PanelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelContainer.ForeColor = System.Drawing.Color.White;
            this.PanelContainer.Location = new System.Drawing.Point(0, 0);
            this.PanelContainer.Margin = new System.Windows.Forms.Padding(0);
            this.PanelContainer.Name = "PanelContainer";
            this.PanelContainer.Size = new System.Drawing.Size(143, 40);
            this.PanelContainer.TabIndex = 0;
            this.bunifuToolTip1.SetToolTip(this.PanelContainer, "");
            this.bunifuToolTip1.SetToolTipIcon(this.PanelContainer, null);
            this.bunifuToolTip1.SetToolTipTitle(this.PanelContainer, "");
            // 
            // Dropdown
            // 
            this.Dropdown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Dropdown.BackColor = System.Drawing.Color.Transparent;
            this.Dropdown.ForeColor = System.Drawing.Color.White;
            this.Dropdown.Location = new System.Drawing.Point(0, 0);
            this.Dropdown.Margin = new System.Windows.Forms.Padding(0);
            this.Dropdown.MaximumSize = new System.Drawing.Size(150, 38);
            this.Dropdown.Name = "Dropdown";
            this.Dropdown.SelectedIndex = 0;
            this.Dropdown.Size = new System.Drawing.Size(103, 38);
            this.Dropdown.TabIndex = 2;
            this.bunifuToolTip1.SetToolTip(this.Dropdown, "");
            this.bunifuToolTip1.SetToolTipIcon(this.Dropdown, null);
            this.bunifuToolTip1.SetToolTipTitle(this.Dropdown, "");
            this.Dropdown.Click += new System.EventHandler(this.DropdownButton_Click);
            // 
            // SidebarDdlFilterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.PanelContainer);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "SidebarDdlFilterControl";
            this.Size = new System.Drawing.Size(143, 40);
            this.bunifuToolTip1.SetToolTip(this, "");
            this.bunifuToolTip1.SetToolTipIcon(this, null);
            this.bunifuToolTip1.SetToolTipTitle(this, "");
            this.PanelContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
        private Shared.Dropdown Dropdown;
        private System.Windows.Forms.Panel filterButton;
        private System.Windows.Forms.Panel PanelContainer;
    }
}
