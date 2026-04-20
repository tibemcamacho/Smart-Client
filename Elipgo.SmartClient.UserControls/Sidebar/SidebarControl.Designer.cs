using Elipgo.SmartClient.Common;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Sidebar
{
    partial class SidebarControl
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
            this.CustonDispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SidebarControl));
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties21 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties22 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties23 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties24 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            this.ScrollBar = new Bunifu.UI.WinForms.BunifuVScrollBar();
            this.PanSidebar = new System.Windows.Forms.FlowLayoutPanel();
            this.FilterTextbox = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.FindButton = new System.Windows.Forms.Panel();
            this.clearTextImage = new Bunifu.Framework.UI.BunifuImageButton();
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.panCountSearch = new System.Windows.Forms.Panel();
            this.lblSitios = new System.Windows.Forms.Label();
            this.picBoxSitios = new System.Windows.Forms.PictureBox();
            this.picBoxCameras = new System.Windows.Forms.PictureBox();
            this.lblCameras = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.clearTextImage)).BeginInit();
            this.panCountSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSitios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCameras)).BeginInit();
            this.SuspendLayout();
            // 
            // ScrollBar
            // 
            this.ScrollBar.AllowCursorChanges = true;
            this.ScrollBar.AllowHomeEndKeysDetection = false;
            this.ScrollBar.AllowIncrementalClickMoves = true;
            this.ScrollBar.AllowMouseDownEffects = true;
            this.ScrollBar.AllowMouseHoverEffects = true;
            this.ScrollBar.AllowScrollingAnimations = true;
            this.ScrollBar.AllowScrollKeysDetection = true;
            this.ScrollBar.AllowScrollOptionsMenu = true;
            this.ScrollBar.AllowShrinkingOnFocusLost = false;
            this.ScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ScrollBar.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ScrollBar.BackgroundImage")));
            this.ScrollBar.BindingContainer = this.PanSidebar;
            this.ScrollBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.BorderRadius = 0;
            this.ScrollBar.BorderThickness = 1;
            this.ScrollBar.DurationBeforeShrink = 2000;
            this.ScrollBar.LargeChange = 10;
            this.ScrollBar.Location = new System.Drawing.Point(290, 45);
            this.ScrollBar.Maximum = 100;
            this.ScrollBar.Minimum = 0;
            this.ScrollBar.MinimumThumbLength = 18;
            this.ScrollBar.Name = "ScrollBar";
            this.ScrollBar.OnDisable.ScrollBarBorderColor = System.Drawing.Color.Silver;
            this.ScrollBar.OnDisable.ScrollBarColor = System.Drawing.Color.Transparent;
            this.ScrollBar.OnDisable.ThumbColor = System.Drawing.Color.Silver;
            this.ScrollBar.ScrollBarBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.ScrollBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ScrollBar.ShrinkSizeLimit = 3;
            this.ScrollBar.Size = new System.Drawing.Size(15, 638);
            this.ScrollBar.SmallChange = 1;
            this.ScrollBar.TabIndex = 7;
            this.ScrollBar.ThumbColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(98)))), ((int)(((byte)(98)))));
            this.ScrollBar.ThumbLength = 62;
            this.ScrollBar.ThumbMargin = 1;
            this.ScrollBar.ThumbStyle = Bunifu.UI.WinForms.BunifuVScrollBar.ThumbStyles.Inset;
            this.bunifuToolTip1.SetToolTip(this.ScrollBar, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ScrollBar, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ScrollBar, "");
            this.ScrollBar.Value = 0;
            this.ScrollBar.Visible = false;
            // 
            // PanSidebar
            // 
            this.PanSidebar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanSidebar.AutoScroll = true;
            this.PanSidebar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.PanSidebar.Location = new System.Drawing.Point(0, 45);
            this.PanSidebar.Margin = new System.Windows.Forms.Padding(0);
            this.PanSidebar.Name = "PanSidebar";
            this.PanSidebar.Size = new System.Drawing.Size(304, 638);
            this.PanSidebar.TabIndex = 6;
            this.bunifuToolTip1.SetToolTip(this.PanSidebar, "");
            this.bunifuToolTip1.SetToolTipIcon(this.PanSidebar, null);
            this.bunifuToolTip1.SetToolTipTitle(this.PanSidebar, "");
            // 
            // FilterTextbox
            // 
            this.FilterTextbox.AcceptsReturn = false;
            this.FilterTextbox.AcceptsTab = false;
            this.FilterTextbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterTextbox.AnimationSpeed = 200;
            this.FilterTextbox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.FilterTextbox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.FilterTextbox.BackColor = System.Drawing.Color.Transparent;
            this.FilterTextbox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("FilterTextbox.BackgroundImage")));
            this.FilterTextbox.BorderColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FilterTextbox.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.FilterTextbox.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FilterTextbox.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FilterTextbox.BorderRadius = 20;
            this.FilterTextbox.BorderThickness = 1;
            this.FilterTextbox.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.FilterTextbox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.FilterTextbox.DefaultFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FilterTextbox.DefaultText = "";
            this.FilterTextbox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FilterTextbox.ForeColor = System.Drawing.Color.White;
            this.FilterTextbox.HideSelection = true;
            this.FilterTextbox.IconLeft = null;
            this.FilterTextbox.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.FilterTextbox.IconPadding = 10;
            this.FilterTextbox.IconRight = null;
            this.FilterTextbox.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.FilterTextbox.Lines = new string[0];
            if (Screen.PrimaryScreen.Bounds.Width < 1400)
                this.FilterTextbox.Location = new System.Drawing.Point(20, (5+15));
            else
                this.FilterTextbox.Location = new System.Drawing.Point(20, 5);
            this.FilterTextbox.Margin = new System.Windows.Forms.Padding(0);
            this.FilterTextbox.MaxLength = 32767;
            this.FilterTextbox.MinimumSize = new System.Drawing.Size(1, 1);
            this.FilterTextbox.Modified = false;
            this.FilterTextbox.Multiline = false;
            this.FilterTextbox.Name = "FilterTextbox";
            stateProperties21.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            stateProperties21.FillColor = System.Drawing.Color.Empty;
            stateProperties21.ForeColor = System.Drawing.Color.Empty;
            stateProperties21.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.FilterTextbox.OnActiveState = stateProperties21;
            stateProperties22.BorderColor = System.Drawing.Color.Empty;
            stateProperties22.FillColor = System.Drawing.Color.White;
            stateProperties22.ForeColor = System.Drawing.Color.Empty;
            stateProperties22.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.FilterTextbox.OnDisabledState = stateProperties22;
            stateProperties23.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            stateProperties23.FillColor = System.Drawing.Color.Empty;
            stateProperties23.ForeColor = System.Drawing.Color.Empty;
            stateProperties23.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.FilterTextbox.OnHoverState = stateProperties23;
            stateProperties24.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            stateProperties24.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            stateProperties24.ForeColor = System.Drawing.Color.White;
            stateProperties24.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.FilterTextbox.OnIdleState = stateProperties24;
            this.FilterTextbox.PasswordChar = '\0';
            this.FilterTextbox.PlaceholderForeColor = System.Drawing.Color.White;
            this.FilterTextbox.PlaceholderText = "";
            this.FilterTextbox.ReadOnly = false;
            this.FilterTextbox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.FilterTextbox.SelectedText = "";
            this.FilterTextbox.SelectionLength = 0;
            this.FilterTextbox.SelectionStart = 0;
            this.FilterTextbox.ShortcutsEnabled = true;
            this.FilterTextbox.Size = new System.Drawing.Size(260, 32);
            this.FilterTextbox.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Bunifu;
            this.FilterTextbox.TabIndex = 0;
            this.FilterTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.FilterTextbox.TextMarginBottom = 0;
            this.FilterTextbox.TextMarginLeft = 5;
            this.FilterTextbox.TextMarginTop = 0;
            this.FilterTextbox.TextPlaceholder = "";
            this.bunifuToolTip1.SetToolTip(this.FilterTextbox, "");
            this.bunifuToolTip1.SetToolTipIcon(this.FilterTextbox, null);
            this.bunifuToolTip1.SetToolTipTitle(this.FilterTextbox, "");
            this.FilterTextbox.UseSystemPasswordChar = false;
            this.FilterTextbox.WordWrap = true;
            this.FilterTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.FilterTextbox_KeyPress);
            // 
            // FindButton
            // 
            this.FindButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FindButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.FindButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.FindButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.FindButton.Location = new System.Drawing.Point(247, 8);
            this.FindButton.Margin = new System.Windows.Forms.Padding(0);
            this.FindButton.Name = "FindButton";
            this.FindButton.Size = new System.Drawing.Size(24, 24);
            this.FindButton.TabIndex = 8;
            this.bunifuToolTip1.SetToolTip(this.FindButton, "");
            this.bunifuToolTip1.SetToolTipIcon(this.FindButton, null);
            this.bunifuToolTip1.SetToolTipTitle(this.FindButton, "");
            // 
            // clearTextImage
            // 
            this.clearTextImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.clearTextImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.clearTextImage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.clearTextImage.Image = ((System.Drawing.Image)(resources.GetObject("clearTextImage.Image")));
            this.clearTextImage.ImageActive = null;
            this.clearTextImage.Location = new System.Drawing.Point(224, 10);
            this.clearTextImage.Name = "clearTextImage";
            this.clearTextImage.Size = new System.Drawing.Size(20, 19);
            this.clearTextImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.clearTextImage.TabIndex = 20;
            this.clearTextImage.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.clearTextImage, "");
            this.bunifuToolTip1.SetToolTipIcon(this.clearTextImage, null);
            this.bunifuToolTip1.SetToolTipTitle(this.clearTextImage, "");
            this.clearTextImage.Visible = false;
            this.clearTextImage.Zoom = 10;
            this.clearTextImage.Click += new System.EventHandler(this.ClearTextImage_Click);
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
            // panCountSearch
            // 
            this.panCountSearch.Controls.Add(this.lblCameras);
            this.panCountSearch.Controls.Add(this.picBoxCameras);
            this.panCountSearch.Controls.Add(this.lblSitios);
            this.panCountSearch.Controls.Add(this.picBoxSitios);
            this.panCountSearch.Location = new System.Drawing.Point(20, 41);
            this.panCountSearch.Name = "panCountSearch";
            this.panCountSearch.Size = new System.Drawing.Size(260, 24);
            this.panCountSearch.TabIndex = 0;
            this.bunifuToolTip1.SetToolTip(this.panCountSearch, "");
            this.bunifuToolTip1.SetToolTipIcon(this.panCountSearch, null);
            this.bunifuToolTip1.SetToolTipTitle(this.panCountSearch, "");
            // 
            // lblSitios
            // 
            this.lblSitios.AutoSize = true;
            this.lblSitios.ForeColor = System.Drawing.Color.White;
            this.lblSitios.Location = new System.Drawing.Point(24, 8);
            this.lblSitios.Name = "lblSitios";
            this.lblSitios.Size = new System.Drawing.Size(13, 13);
            this.lblSitios.TabIndex = 24;
            this.lblSitios.Text = "_";
            this.bunifuToolTip1.SetToolTip(this.lblSitios, "");
            this.bunifuToolTip1.SetToolTipIcon(this.lblSitios, null);
            this.bunifuToolTip1.SetToolTipTitle(this.lblSitios, "");
            // 
            // picBoxSitios
            // 
            this.picBoxSitios.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picBoxSitios.Location = new System.Drawing.Point(5, 2);
            this.picBoxSitios.Name = "picBoxSitios";
            this.picBoxSitios.Size = new System.Drawing.Size(14, 20);
            this.picBoxSitios.TabIndex = 23;
            this.picBoxSitios.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.picBoxSitios, "");
            this.bunifuToolTip1.SetToolTipIcon(this.picBoxSitios, null);
            this.bunifuToolTip1.SetToolTipTitle(this.picBoxSitios, "");
            // 
            // picBoxCameras
            // 
            this.picBoxCameras.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.picBoxCameras.Location = new System.Drawing.Point(150, 2);
            this.picBoxCameras.Name = "picBoxCameras";
            this.picBoxCameras.Size = new System.Drawing.Size(18, 20);
            this.picBoxCameras.TabIndex = 25;
            this.picBoxCameras.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.picBoxCameras, "");
            this.bunifuToolTip1.SetToolTipIcon(this.picBoxCameras, null);
            this.bunifuToolTip1.SetToolTipTitle(this.picBoxCameras, "");
            // 
            // lblCameras
            // 
            this.lblCameras.AutoSize = true;
            this.lblCameras.ForeColor = System.Drawing.Color.White;
            this.lblCameras.Location = new System.Drawing.Point(171, 6);
            this.lblCameras.Name = "lblCameras";
            this.lblCameras.Size = new System.Drawing.Size(13, 13);
            this.lblCameras.TabIndex = 26;
            this.lblCameras.Text = "_";
            this.bunifuToolTip1.SetToolTip(this.lblCameras, "");
            this.bunifuToolTip1.SetToolTipIcon(this.lblCameras, null);
            this.bunifuToolTip1.SetToolTipTitle(this.lblCameras, "");
            // 
            // SidebarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.Controls.Add(this.panCountSearch);
            this.Controls.Add(this.clearTextImage);
            this.Controls.Add(this.FindButton);
            this.Controls.Add(this.FilterTextbox);
            this.Controls.Add(this.ScrollBar);
            this.Controls.Add(this.PanSidebar);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SidebarControl";
            this.Size = new System.Drawing.Size(304, 683);
            this.bunifuToolTip1.SetToolTip(this, "");
            this.bunifuToolTip1.SetToolTipIcon(this, null);
            this.bunifuToolTip1.SetToolTipTitle(this, "");
            ((System.ComponentModel.ISupportInitialize)(this.clearTextImage)).EndInit();
            this.panCountSearch.ResumeLayout(false);
            this.panCountSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxSitios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxCameras)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.UI.WinForms.BunifuVScrollBar ScrollBar;
        private FlowLayoutPanel PanSidebar;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox FilterTextbox;
        private System.Windows.Forms.Panel FindButton;
        private Bunifu.Framework.UI.BunifuImageButton clearTextImage;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
        private Panel panCountSearch;
        private Label lblSitios;
        private PictureBox picBoxSitios;
        private Label lblCameras;
        private PictureBox picBoxCameras;
    }
}
