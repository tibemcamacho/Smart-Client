using Bunifu.Framework.UI;
using Elipgo.SmartClient.Common.Properties;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.VaultBar
{
    partial class VaultBarControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LiveBar.LiveBarControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.LabelVaultBarTitle = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.buttonRefresh = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonToggleBookmarkGrid = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.TextboxFilter = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.ButtonClearTextFilter = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonFind = new Bunifu.Framework.UI.BunifuImageButton();
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.bunifuToolTip2 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            //this.ddlDevices = new Bunifu.UI.WinForms.BunifuDropdown();
            this.ucddlDevices = new Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown();
            this.ButtonStartDatetime = new Elipgo.SmartClient.UserControls.Shared.ButtonCalendarControl();
            this.LabelStartDateTime = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.ButtonEndDatetime = new Elipgo.SmartClient.UserControls.Shared.ButtonCalendarControl();
            this.LabelEndDateTime = new Bunifu.Framework.UI.BunifuCustomLabel();
            ((System.ComponentModel.ISupportInitialize)(this.buttonRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClearTextFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonFind)).BeginInit();
            this.SuspendLayout();

            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties5 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties6 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties7 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties8 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            // 
            // LabelVaultBarTitle
            // 
            this.LabelVaultBarTitle.AutoEllipsis = false;
            this.LabelVaultBarTitle.AutoSize = false;
            this.LabelVaultBarTitle.CausesValidation = false;
            this.LabelVaultBarTitle.Cursor = null;
            this.LabelVaultBarTitle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.LabelVaultBarTitle.ForeColor = System.Drawing.Color.White;
            this.LabelVaultBarTitle.Location = new System.Drawing.Point(3, 15);
            this.LabelVaultBarTitle.Name = "LabelGridTitle";
            this.LabelVaultBarTitle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelVaultBarTitle.Size = new System.Drawing.Size(152, 45);
            this.LabelVaultBarTitle.TabIndex = 3;
            this.LabelVaultBarTitle.Text = Resources.BookmarkSearchVaultBar;
            this.LabelVaultBarTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //this.LabelVaultBarTitle.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRefresh.Image = ((System.Drawing.Image)(resources.GetObject("buttonRefresh.Image")));
            this.buttonRefresh.ImageActive = null;
            this.buttonRefresh.Location = new System.Drawing.Point(871, 24);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(24, 24);
            this.buttonRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.buttonRefresh.TabIndex = 8;
            this.buttonRefresh.TabStop = false;
            this.bunifuToolTip1.SetToolTip(this.buttonRefresh, "");
            this.bunifuToolTip1.SetToolTipIcon(this.buttonRefresh, null);
            this.bunifuToolTip1.SetToolTipTitle(this.buttonRefresh, "");
            this.buttonRefresh.Visible = false;
            this.buttonRefresh.Zoom = 10;
            this.buttonRefresh.Click += new System.EventHandler(this.ButtonRefresh_Click);

            //
            // Border Edges
            //
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;

            // 
            // ButtonToogleBookmars
            // 

            this.ButtonToggleBookmarkGrid.AllowToggling = false;
            this.ButtonToggleBookmarkGrid.AnimationSpeed = 200;
            this.ButtonToggleBookmarkGrid.AutoGenerateColors = false;
            this.ButtonToggleBookmarkGrid.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonOK.BackgroundImage")));
            this.ButtonToggleBookmarkGrid.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonToggleBookmarkGrid.ButtonText = Resources.ExportedBookmarkGridTitle;
            this.ButtonToggleBookmarkGrid.ButtonTextMarginLeft = 1;
            this.ButtonToggleBookmarkGrid.ColorContrastOnClick = 45;
            this.ButtonToggleBookmarkGrid.ColorContrastOnHover = 45;
            this.ButtonToggleBookmarkGrid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonToggleBookmarkGrid.CustomizableEdges = borderEdges1;
            this.ButtonToggleBookmarkGrid.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ButtonToggleBookmarkGrid.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonToggleBookmarkGrid.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            this.ButtonToggleBookmarkGrid.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            this.ButtonToggleBookmarkGrid.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.ButtonToggleBookmarkGrid.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.ButtonToggleBookmarkGrid.ForeColor = System.Drawing.Color.White;
            this.ButtonToggleBookmarkGrid.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonToggleBookmarkGrid.IconMarginLeft = 11;
            this.ButtonToggleBookmarkGrid.IconPadding = 10;
            this.ButtonToggleBookmarkGrid.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonToggleBookmarkGrid.IdleBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.ButtonToggleBookmarkGrid.IdleBorderRadius = 30;
            this.ButtonToggleBookmarkGrid.IdleBorderThickness = 1;
            this.ButtonToggleBookmarkGrid.IdleIconLeftImage = ((System.Drawing.Image)(resources.GetObject("ButtonOK.BackgroundImage")));
            this.ButtonToggleBookmarkGrid.IdleIconRightImage = null;
            this.ButtonToggleBookmarkGrid.IndicateFocus = false;
            this.ButtonToggleBookmarkGrid.Location = new System.Drawing.Point(700, 17); // Point(1155, 17) // ddvl/31-Mzo-2021/vmon-3980/ relacionado a defecto de UX
            this.ButtonToggleBookmarkGrid.Name = "ButtonToggleBookmarkGrid";
            this.ButtonToggleBookmarkGrid.Size = new System.Drawing.Size(240, 37);
            this.ButtonToggleBookmarkGrid.TabIndex = 21;
            this.ButtonToggleBookmarkGrid.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonToggleBookmarkGrid.TextMarginLeft = 1;
            this.ButtonToggleBookmarkGrid.UseDefaultRadiusAndThickness = true;
            this.ButtonToggleBookmarkGrid.Visible = true;
            this.ButtonToggleBookmarkGrid.Click += new System.EventHandler(this.ButtonToggleBookmarkGrid_Click);

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
            // bunifuToolTip2
            // 
            this.bunifuToolTip2.Active = true;
            this.bunifuToolTip2.AlignTextWithTitle = false;
            this.bunifuToolTip2.AllowAutoClose = true;
            this.bunifuToolTip2.AllowFading = true;
            this.bunifuToolTip2.AutoCloseDuration = 2000;
            this.bunifuToolTip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
            this.bunifuToolTip2.BorderColor = System.Drawing.Color.Black;
            this.bunifuToolTip2.ClickToShowDisplayControl = false;
            this.bunifuToolTip2.ConvertNewlinesToBreakTags = true;
            this.bunifuToolTip2.DisplayControl = null;
            this.bunifuToolTip2.EntryAnimationSpeed = 350;
            this.bunifuToolTip2.ExitAnimationSpeed = 200;
            this.bunifuToolTip2.GenerateAutoCloseDuration = false;
            this.bunifuToolTip2.IconMargin = 6;
            this.bunifuToolTip2.InitialDelay = 0;
            this.bunifuToolTip2.Name = "bunifuToolTip2";
            this.bunifuToolTip2.Opacity = 1D;
            this.bunifuToolTip2.OverrideToolTipTitles = false;
            this.bunifuToolTip2.Padding = new System.Windows.Forms.Padding(10);
            this.bunifuToolTip2.ReshowDelay = 100;
            this.bunifuToolTip2.ShowAlways = true;
            this.bunifuToolTip2.ShowBorders = false;
            this.bunifuToolTip2.ShowIcons = true;
            this.bunifuToolTip2.ShowShadows = true;
            this.bunifuToolTip2.Tag = null;
            this.bunifuToolTip2.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            this.bunifuToolTip2.TextForeColor = System.Drawing.Color.White;
            this.bunifuToolTip2.TextMargin = 2;
            this.bunifuToolTip2.TitleFont = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.bunifuToolTip2.TitleForeColor = System.Drawing.Color.Black;
            this.bunifuToolTip2.ToolTipPosition = new System.Drawing.Point(0, 0);
            this.bunifuToolTip2.ToolTipTitle = null;
            // 
            // ButtonClearTextFilter
            // 
            this.ButtonClearTextFilter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.ButtonClearTextFilter.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonClearTextFilter.Image = ((System.Drawing.Image)(FileResources.icon_close));
            this.ButtonClearTextFilter.ImageActive = null;
            this.ButtonClearTextFilter.Location = new System.Drawing.Point(353, 29);
            this.ButtonClearTextFilter.Name = "ButtonClearTextFilter";
            this.ButtonClearTextFilter.Size = new System.Drawing.Size(20, 19);
            this.ButtonClearTextFilter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonClearTextFilter.TabIndex = 26;
            this.ButtonClearTextFilter.TabStop = false;
            this.ButtonClearTextFilter.Visible = false;
            this.ButtonClearTextFilter.Zoom = 10;
            this.ButtonClearTextFilter.Click += new System.EventHandler(this.ButtonClearText_Click);
            // 
            // ButtonFind
            // 
            //this.ButtonFind.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.ButtonFind.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonFind.Image = ((System.Drawing.Image)(Elipgo.SmartClient.Common.Properties.FileResources.icon_search_input));
            this.ButtonFind.ImageActive = null;
            this.ButtonFind.Location = new System.Drawing.Point(376, 27);
            this.ButtonFind.Name = "ButtonFind";
            this.ButtonFind.Size = new System.Drawing.Size(24, 24);
            this.ButtonFind.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonFind.TabIndex = 25;
            this.ButtonFind.TabStop = false;
            this.ButtonFind.Zoom = 10;
            this.ButtonFind.BringToFront();
            this.ButtonFind.Click += new System.EventHandler(this.ButtonFind_Click);
            this.bunifuToolTip2.SetToolTip(this.ButtonFind, "");
            this.bunifuToolTip2.SetToolTipIcon(this.ButtonFind, null);
            this.bunifuToolTip2.SetToolTipTitle(this.ButtonFind, "");
            // 
            // ddlDevices
            // 
            this.ucddlDevices.BackColor = System.Drawing.Color.Transparent;
            this.ucddlDevices.ForeColor = System.Drawing.Color.White;
            this.ucddlDevices.Location = new System.Drawing.Point(24, 4);
            this.ucddlDevices.Name = "ddlDevices";
            this.ucddlDevices.Size = new System.Drawing.Size(127, 37);
            this.ucddlDevices.TabIndex = 14;
            this.ucddlDevices.SelectedIndexChanged += new System.EventHandler(this.ucddlDevices_SelectedIndexChanged);
            this.ucddlDevices.Visible = false;
            //this.ddlDevices.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            //this.ddlDevices.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            //this.ddlDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.ddlDevices.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            //this.ddlDevices.FillDropDown = false;
            //this.ddlDevices.FillIndicator = false;
            //this.ddlDevices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.ddlDevices.ForeColor = System.Drawing.Color.White;
            //this.ddlDevices.FormattingEnabled = true;
            //this.ddlDevices.Icon = null;
            //this.ddlDevices.IndicatorColor = System.Drawing.Color.White;
            //this.ddlDevices.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            //this.ddlDevices.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.ddlDevices.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.ddlDevices.ItemForeColor = System.Drawing.Color.White;
            //this.ddlDevices.ItemHeight = 26;
            //this.ddlDevices.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            //this.ddlDevices.Location = new System.Drawing.Point(24, 4);
            //this.ddlDevices.Margin = new System.Windows.Forms.Padding(0);
            //this.ddlDevices.Name = "ddlDevices";
            //this.ddlDevices.Size = new System.Drawing.Size(127, 37);
            //this.ddlDevices.TabIndex = 14;
            //this.ddlDevices.Text = null;
            //this.ddlDevices.ValueMember = "Key";
            //this.ddlDevices.BackColor = System.Drawing.Color.DarkCyan;
            //this.ddlDevices.BorderRadius = 1;
            //this.ddlDevices.Color = System.Drawing.Color.Transparent;
            //this.ddlDevices.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.ddlDevices.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            //this.ddlDevices.DisabledColor = System.Drawing.Color.Gray;
            //this.ddlDevices.DisplayMember = "Name";
            // 
            // ButtonStartDatetime
            // 
            this.ButtonStartDatetime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonStartDatetime.Location = new System.Drawing.Point(327, 23);
            this.ButtonStartDatetime.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonStartDatetime.Name = "ButtonStartDatetime";
            this.ButtonStartDatetime.Size = new System.Drawing.Size(36, 36);
            this.ButtonStartDatetime.TabIndex = 14;
            this.ButtonStartDatetime.TabStop = false;
            this.ButtonStartDatetime.Visible = false;
            // 
            // LabelStartDateTime
            // 
            this.LabelStartDateTime.AutoEllipsis = false;
            this.LabelStartDateTime.AutoSize = false;
            this.LabelStartDateTime.CausesValidation = false;
            this.LabelStartDateTime.Cursor = null;
            this.LabelStartDateTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.LabelStartDateTime.ForeColor = System.Drawing.Color.White;
            this.LabelStartDateTime.Location = new System.Drawing.Point(3, 15);
            this.LabelStartDateTime.Name = "LabelStartDateTime";
            this.LabelStartDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelStartDateTime.Size = new System.Drawing.Size(152, 45);
            this.LabelStartDateTime.TabIndex = 3;
            this.LabelStartDateTime.Text = "0000/00/00 00:00:00";
            this.LabelStartDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelStartDateTime.Visible = false;
            // 
            // ButtonEndDatetime
            // 
            this.ButtonEndDatetime.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonEndDatetime.Location = new System.Drawing.Point(327, 23);
            this.ButtonEndDatetime.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonEndDatetime.Name = "ButtonEndDatetime";
            this.ButtonEndDatetime.Size = new System.Drawing.Size(36, 36);
            this.ButtonEndDatetime.TabIndex = 14;
            this.ButtonEndDatetime.TabStop = false;
            this.ButtonEndDatetime.Visible = false;
            // 
            // LabelEndDateTime
            // 
            this.LabelEndDateTime.AutoEllipsis = false;
            this.LabelEndDateTime.AutoSize = false;
            this.LabelEndDateTime.CausesValidation = false;
            this.LabelEndDateTime.Cursor = null;
            this.LabelEndDateTime.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.LabelEndDateTime.ForeColor = System.Drawing.Color.White;
            this.LabelEndDateTime.Location = new System.Drawing.Point(3, 15);
            this.LabelEndDateTime.Name = "LabelEndDateTime";
            this.LabelEndDateTime.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelEndDateTime.Size = new System.Drawing.Size(152, 45);
            this.LabelEndDateTime.TabIndex = 3;
            this.LabelEndDateTime.Text = "0000/00/00 00:00:00";
            this.LabelEndDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.LabelEndDateTime.Visible = false;
            // 
            // TextboxFilter
            // 
            this.TextboxFilter.AcceptsReturn = false;
            this.TextboxFilter.AcceptsTab = false;
            this.TextboxFilter.AnimationSpeed = 200;
            this.TextboxFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.TextboxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.TextboxFilter.BackColor = System.Drawing.Color.Transparent;
            this.TextboxFilter.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TextboxFilter.BackgroundImage")));
            this.TextboxFilter.BorderColorActive = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.TextboxFilter.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.TextboxFilter.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.TextboxFilter.BorderColorIdle = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.TextboxFilter.BorderRadius = 25;
            this.TextboxFilter.BorderThickness = 1;
            this.TextboxFilter.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.TextboxFilter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TextboxFilter.DefaultFont = new System.Drawing.Font("Segoe UI", 10F);
            this.TextboxFilter.DefaultText = "";
            this.TextboxFilter.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.TextboxFilter.ForeColor = System.Drawing.Color.White;
            this.TextboxFilter.HideSelection = true;
            this.TextboxFilter.IconLeft = null;
            this.TextboxFilter.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.TextboxFilter.IconPadding = 10;
            this.TextboxFilter.IconRight = null;
            this.TextboxFilter.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.TextboxFilter.Lines = new string[0];
            this.TextboxFilter.Location = new System.Drawing.Point(156, 24);
            this.TextboxFilter.MaxLength = 32767;
            //this.TextboxFilter.MinimumSize = new System.Drawing.Size(1, 1);
            this.TextboxFilter.Modified = false;
            this.TextboxFilter.Multiline = false;
            this.TextboxFilter.Name = "TextboxFilter";
            stateProperties5.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            stateProperties5.FillColor = System.Drawing.Color.Empty;
            stateProperties5.ForeColor = System.Drawing.Color.Empty;
            stateProperties5.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.TextboxFilter.OnActiveState = stateProperties5;
            stateProperties6.BorderColor = System.Drawing.Color.Empty;
            stateProperties6.FillColor = System.Drawing.Color.White;
            stateProperties6.ForeColor = System.Drawing.Color.Empty;
            stateProperties6.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.TextboxFilter.OnDisabledState = stateProperties6;
            stateProperties7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            stateProperties7.FillColor = System.Drawing.Color.Empty;
            stateProperties7.ForeColor = System.Drawing.Color.Empty;
            stateProperties7.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.TextboxFilter.OnHoverState = stateProperties7;
            stateProperties8.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            stateProperties8.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            stateProperties8.ForeColor = System.Drawing.Color.White;
            stateProperties8.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.TextboxFilter.OnIdleState = stateProperties8;
            this.TextboxFilter.PasswordChar = '\0';
            this.TextboxFilter.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.TextboxFilter.PlaceholderText = Resources.search;
            this.TextboxFilter.ReadOnly = false;
            this.TextboxFilter.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TextboxFilter.SelectedText = "";
            this.TextboxFilter.SelectionLength = 0;
            this.TextboxFilter.SelectionStart = 0;
            this.TextboxFilter.ShortcutsEnabled = true;
            this.TextboxFilter.Size = new System.Drawing.Size(259, 29);
            this.TextboxFilter.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Bunifu;
            this.TextboxFilter.TabIndex = 24;
            this.TextboxFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.TextboxFilter.TextMarginBottom = 0;
            this.TextboxFilter.TextMarginLeft = 5;
            this.TextboxFilter.TextMarginTop = 0;
            this.TextboxFilter.TextPlaceholder = Resources.search;
            this.TextboxFilter.UseSystemPasswordChar = false;
            this.TextboxFilter.WordWrap = true;
            this.TextboxFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextboxFilter_KeyUp);
            // 
            // VaultBarControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LabelVaultBarTitle);
            this.Controls.Add(this.ButtonToggleBookmarkGrid);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.TextboxFilter);
            this.Controls.Add(this.ButtonFind);
            this.Controls.Add(this.ButtonClearTextFilter);
            this.Controls.Add(this.ucddlDevices);
            this.Controls.Add(this.ButtonStartDatetime);
            this.Controls.Add(this.LabelStartDateTime);
            this.Controls.Add(this.ButtonEndDatetime);
            this.Controls.Add(this.LabelEndDateTime);
            this.Name = "LiveBarControl";
            this.Size = new System.Drawing.Size(940, 72); //Size(1410, 72); // ddvl/31-Mzo-2021/vmon-3980/ relacionado a defecto de UX
            this.bunifuToolTip1.SetToolTip(this, "");
            this.bunifuToolTip1.SetToolTipIcon(this, null);
            this.bunifuToolTip1.SetToolTipTitle(this, "");
            this.bunifuToolTip2.SetToolTip(this, "");
            this.bunifuToolTip2.SetToolTipIcon(this, null);
            this.bunifuToolTip2.SetToolTipTitle(this, "");
            ((System.ComponentModel.ISupportInitialize)(this.buttonRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClearTextFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonFind)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Bunifu.Framework.UI.BunifuCustomLabel LabelVaultBarTitle;
        private Bunifu.Framework.UI.BunifuImageButton buttonRefresh;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonToggleBookmarkGrid;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip2;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox TextboxFilter;
        private Bunifu.Framework.UI.BunifuImageButton ButtonClearTextFilter;
        private Bunifu.Framework.UI.BunifuImageButton ButtonFind;
        private Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown ucddlDevices;
        private Elipgo.SmartClient.UserControls.Shared.ButtonCalendarControl ButtonStartDatetime;
        private BunifuCustomLabel LabelStartDateTime;
        private Elipgo.SmartClient.UserControls.Shared.ButtonCalendarControl ButtonEndDatetime;
        private BunifuCustomLabel LabelEndDateTime;

    }
}
