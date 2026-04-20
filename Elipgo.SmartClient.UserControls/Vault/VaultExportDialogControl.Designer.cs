namespace Elipgo.SmartClient.UserControls.Vault
{
    partial class VaultExportDialogControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VaultExportDialogControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.PanelHeader = new System.Windows.Forms.Panel();
            this.ButtonClose = new Bunifu.Framework.UI.BunifuImageButton();
            this.TitleForm = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.LabelBookmarkName = new System.Windows.Forms.Label();
            this.ProgressBookmark = new Bunifu.Framework.UI.BunifuProgressBar();
            this.LabelDateTime = new System.Windows.Forms.Label();
            this.ButtonCancelExport = new Bunifu.Framework.UI.BunifuImageButton();
            this.ButtonOK = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.ButtonCancel = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.DataGridView = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.PanelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonCancelExport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this.PanelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this.PanelHeader.Controls.Add(this.ButtonClose);
            this.PanelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelHeader.Location = new System.Drawing.Point(0, 0);
            this.PanelHeader.Name = "PanelHeader";
            this.PanelHeader.Size = new System.Drawing.Size(1466, 26);
            this.PanelHeader.TabIndex = 10;
            // 
            // ButtonClose
            // 
            this.ButtonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ButtonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("ButtonClose.Image")));
            this.ButtonClose.ImageActive = null;
            this.ButtonClose.Location = new System.Drawing.Point(1436, 3);
            this.ButtonClose.Name = "ButtonClose";
            this.ButtonClose.Size = new System.Drawing.Size(22, 20);
            this.ButtonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonClose.TabIndex = 0;
            this.ButtonClose.TabStop = false;
            this.ButtonClose.Zoom = 10;
            this.ButtonClose.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // TitleForm
            // 
            this.TitleForm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.TitleForm.AutoEllipsis = false;
            this.TitleForm.Cursor = null;
            this.TitleForm.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.TitleForm.ForeColor = System.Drawing.Color.Silver;
            this.TitleForm.Location = new System.Drawing.Point(24, 32);
            this.TitleForm.Name = "TitleForm";
            this.TitleForm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TitleForm.Size = new System.Drawing.Size(96, 27);
            this.TitleForm.TabIndex = 14;
            this.TitleForm.Text = "Bookmark";
            this.TitleForm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            //this.TitleForm.TextFormat = Bunifu.UI.WinForms.BunifuLabel.TextFormattingOptions.Default;
            // 
            // LabelBookmarkName
            // 
            this.LabelBookmarkName.AutoSize = true;
            this.LabelBookmarkName.Font = new System.Drawing.Font("Segoe UI", 7.25F, System.Drawing.FontStyle.Bold);
            this.LabelBookmarkName.ForeColor = System.Drawing.Color.Silver;
            this.LabelBookmarkName.Location = new System.Drawing.Point(23, 82);
            this.LabelBookmarkName.Name = "LabelBookmarkName";
            this.LabelBookmarkName.Size = new System.Drawing.Size(88, 12);
            this.LabelBookmarkName.TabIndex = 15;
            this.LabelBookmarkName.Text = "_BookmarkName_";
            // 
            // ProgressBookmark
            // 
            this.ProgressBookmark.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ProgressBookmark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ProgressBookmark.BorderRadius = 0;
            this.ProgressBookmark.Location = new System.Drawing.Point(25, 106);
            this.ProgressBookmark.MaximumValue = 100;
            this.ProgressBookmark.Name = "ProgressBookmark";
            this.ProgressBookmark.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(133)))), ((int)(((byte)(244)))));
            this.ProgressBookmark.Size = new System.Drawing.Size(1325, 8);
            this.ProgressBookmark.TabIndex = 16;
            this.ProgressBookmark.Value = 30;
            // 
            // LabelDateTime
            // 
            this.LabelDateTime.AutoSize = true;
            this.LabelDateTime.Font = new System.Drawing.Font("Segoe UI", 7.25F, System.Drawing.FontStyle.Bold);
            this.LabelDateTime.ForeColor = System.Drawing.Color.Silver;
            this.LabelDateTime.Location = new System.Drawing.Point(23, 126);
            this.LabelDateTime.Name = "LabelDateTime";
            this.LabelDateTime.Size = new System.Drawing.Size(105, 12);
            this.LabelDateTime.TabIndex = 17;
            this.LabelDateTime.Text = "_BookmarkDateTime_";
            // 
            // ButtonCancelExport
            // 
            this.ButtonCancelExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonCancelExport.Image = ((System.Drawing.Image)(resources.GetObject("ButtonCancelExport.Image")));
            this.ButtonCancelExport.ImageActive = null;
            this.ButtonCancelExport.Location = new System.Drawing.Point(1356, 85);
            this.ButtonCancelExport.Name = "ButtonCancelExport";
            this.ButtonCancelExport.Size = new System.Drawing.Size(36, 36);
            this.ButtonCancelExport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ButtonCancelExport.TabIndex = 18;
            this.ButtonCancelExport.TabStop = false;
            this.ButtonCancelExport.Zoom = 10;
            // 
            // ButtonOK
            // 
            this.ButtonOK.AllowToggling = false;
            this.ButtonOK.AnimationSpeed = 200;
            this.ButtonOK.AutoGenerateColors = false;
            this.ButtonOK.BackColor = System.Drawing.Color.Transparent;
            this.ButtonOK.BackColor1 = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonOK.BackgroundImage")));
            this.ButtonOK.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.ButtonText = "_OK_";
            this.ButtonOK.ButtonTextMarginLeft = 0;
            this.ButtonOK.ColorContrastOnClick = 45;
            this.ButtonOK.ColorContrastOnHover = 45;
            this.ButtonOK.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.ButtonOK.CustomizableEdges = borderEdges1;
            this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this.ButtonOK.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonOK.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.ButtonOK.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.ButtonOK.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.ButtonOK.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.ButtonOK.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonOK.IconMarginLeft = 11;
            this.ButtonOK.IconPadding = 10;
            this.ButtonOK.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonOK.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.IdleBorderRadius = 30;
            this.ButtonOK.IdleBorderThickness = 1;
            this.ButtonOK.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.IdleIconLeftImage = null;
            this.ButtonOK.IdleIconRightImage = null;
            this.ButtonOK.IndicateFocus = false;
            this.ButtonOK.Location = new System.Drawing.Point(1347, 865);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonOK.onHoverState.BorderRadius = 30;
            this.ButtonOK.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.onHoverState.BorderThickness = 1;
            this.ButtonOK.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonOK.onHoverState.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.onHoverState.IconLeftImage = null;
            this.ButtonOK.onHoverState.IconRightImage = null;
            this.ButtonOK.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.OnIdleState.BorderRadius = 30;
            this.ButtonOK.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.OnIdleState.BorderThickness = 1;
            this.ButtonOK.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this.ButtonOK.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.OnIdleState.IconLeftImage = null;
            this.ButtonOK.OnIdleState.IconRightImage = null;
            this.ButtonOK.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonOK.OnPressedState.BorderRadius = 30;
            this.ButtonOK.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonOK.OnPressedState.BorderThickness = 1;
            this.ButtonOK.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonOK.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.ButtonOK.OnPressedState.IconLeftImage = null;
            this.ButtonOK.OnPressedState.IconRightImage = null;
            this.ButtonOK.Size = new System.Drawing.Size(92, 37);
            this.ButtonOK.TabIndex = 29;
            this.ButtonOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonOK.TextMarginLeft = 0;
            this.ButtonOK.UseDefaultRadiusAndThickness = true;
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.AllowToggling = false;
            this.ButtonCancel.AnimationSpeed = 200;
            this.ButtonCancel.AutoGenerateColors = false;
            this.ButtonCancel.BackColor = System.Drawing.Color.Transparent;
            this.ButtonCancel.BackColor1 = System.Drawing.Color.Transparent;
            this.ButtonCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonCancel.BackgroundImage")));
            this.ButtonCancel.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.ButtonText = "_CANCEL_";
            this.ButtonCancel.ButtonTextMarginLeft = 0;
            this.ButtonCancel.ColorContrastOnClick = 45;
            this.ButtonCancel.ColorContrastOnHover = 45;
            this.ButtonCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.ButtonCancel.CustomizableEdges = borderEdges2;
            this.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonCancel.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonCancel.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.ButtonCancel.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.ButtonCancel.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.ButtonCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.ButtonCancel.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonCancel.IconMarginLeft = 11;
            this.ButtonCancel.IconPadding = 10;
            this.ButtonCancel.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonCancel.IdleBorderColor = System.Drawing.Color.DimGray;
            this.ButtonCancel.IdleBorderRadius = 30;
            this.ButtonCancel.IdleBorderThickness = 1;
            this.ButtonCancel.IdleFillColor = System.Drawing.Color.Transparent;
            this.ButtonCancel.IdleIconLeftImage = null;
            this.ButtonCancel.IdleIconRightImage = null;
            this.ButtonCancel.IndicateFocus = false;
            this.ButtonCancel.Location = new System.Drawing.Point(1233, 865);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonCancel.onHoverState.BorderRadius = 30;
            this.ButtonCancel.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.onHoverState.BorderThickness = 1;
            this.ButtonCancel.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonCancel.onHoverState.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.onHoverState.IconLeftImage = null;
            this.ButtonCancel.onHoverState.IconRightImage = null;
            this.ButtonCancel.OnIdleState.BorderColor = System.Drawing.Color.DimGray;
            this.ButtonCancel.OnIdleState.BorderRadius = 30;
            this.ButtonCancel.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.OnIdleState.BorderThickness = 1;
            this.ButtonCancel.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this.ButtonCancel.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.OnIdleState.IconLeftImage = null;
            this.ButtonCancel.OnIdleState.IconRightImage = null;
            this.ButtonCancel.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonCancel.OnPressedState.BorderRadius = 30;
            this.ButtonCancel.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonCancel.OnPressedState.BorderThickness = 1;
            this.ButtonCancel.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonCancel.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.ButtonCancel.OnPressedState.IconLeftImage = null;
            this.ButtonCancel.OnPressedState.IconRightImage = null;
            this.ButtonCancel.Size = new System.Drawing.Size(92, 37);
            this.ButtonCancel.TabIndex = 30;
            this.ButtonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonCancel.TextMarginLeft = 0;
            this.ButtonCancel.UseDefaultRadiusAndThickness = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // DataGridView
            // 
            this.DataGridView.AllowCustomTheming = true;
            this.DataGridView.AllowDrop = true;
            this.DataGridView.AllowUserToAddRows = false;
            this.DataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this.DataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataGridView.CausesValidation = false;
            this.DataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DataGridView.ColumnHeadersHeight = 40;
            this.DataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewImageColumn1});
            this.DataGridView.CurrentTheme.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.CurrentTheme.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.DataGridView.CurrentTheme.AlternatingRowsStyle.ForeColor = System.Drawing.Color.White;
            this.DataGridView.CurrentTheme.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.DataGridView.CurrentTheme.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridView.CurrentTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.CurrentTheme.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.CurrentTheme.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.CurrentTheme.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            this.DataGridView.CurrentTheme.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.DataGridView.CurrentTheme.HeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.CurrentTheme.HeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.DataGridView.CurrentTheme.Name = null;
            this.DataGridView.CurrentTheme.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.CurrentTheme.RowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.DataGridView.CurrentTheme.RowsStyle.ForeColor = System.Drawing.Color.White;
            this.DataGridView.CurrentTheme.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.CurrentTheme.RowsStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridView.DefaultCellStyle = dataGridViewCellStyle9;
            this.DataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DataGridView.EnableHeadersVisualStyles = false;
            this.DataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
#pragma warning disable CS0618 // Type or member is obsolete
            this.DataGridView.HeaderBgColor = System.Drawing.Color.Empty;
#pragma warning restore CS0618 // Type or member is obsolete
            this.DataGridView.HeaderForeColor = System.Drawing.Color.White;
            this.DataGridView.Location = new System.Drawing.Point(27, 168);
            this.DataGridView.Name = "DataGridView";
            this.DataGridView.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.DataGridView.RowHeadersVisible = false;
            this.DataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle11.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.Color.White;
            this.DataGridView.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.DataGridView.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.DataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this.DataGridView.RowTemplate.Height = 40;
            this.DataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridView.ShowCellErrors = false;
            this.DataGridView.ShowCellToolTips = false;
            this.DataGridView.ShowEditingIcon = false;
            this.DataGridView.ShowRowErrors = false;
            this.DataGridView.Size = new System.Drawing.Size(1412, 576);
            this.DataGridView.TabIndex = 31;
            this.DataGridView.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.Dark;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsDeleted";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.NullValue = false;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridViewCheckBoxColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewCheckBoxColumn1.HeaderText = "Delete";
            this.dataGridViewCheckBoxColumn1.MinimumWidth = 6;
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn1.HeaderText = "Id";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "DeviceName";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn2.FillWeight = 300F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Device";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridViewTextBoxColumn3.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn3.HeaderText = "From";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn4.HeaderText = "To";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.NullValue = ((object)(resources.GetObject("dataGridViewCellStyle8.NullValue")));
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn1.Image")));
            this.dataGridViewImageColumn1.MinimumWidth = 6;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 125;
            // 
            // VaultExportDialogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(44)))), ((int)(((byte)(44)))));
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.DataGridView);
            this.Controls.Add(this.ButtonCancelExport);
            this.Controls.Add(this.LabelDateTime);
            this.Controls.Add(this.ProgressBookmark);
            this.Controls.Add(this.LabelBookmarkName);
            this.Controls.Add(this.TitleForm);
            this.Controls.Add(this.PanelHeader);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VaultExportDialogControl";
            this.Size = new System.Drawing.Size(1466, 930);
            this.PanelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ButtonClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ButtonCancelExport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PanelHeader;
        private Bunifu.Framework.UI.BunifuImageButton ButtonClose;
        private Bunifu.Framework.UI.BunifuCustomLabel TitleForm;
        private System.Windows.Forms.Label LabelBookmarkName;
        private Bunifu.Framework.UI.BunifuProgressBar ProgressBookmark;
        private System.Windows.Forms.Label LabelDateTime;
        private Bunifu.Framework.UI.BunifuImageButton ButtonCancelExport;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonOK;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonCancel;
        private Bunifu.UI.WinForms.BunifuDataGridView DataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
    }
}
