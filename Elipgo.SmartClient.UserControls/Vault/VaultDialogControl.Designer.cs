using Elipgo.SmartClient.Common.Properties;
using System.Drawing;

namespace Elipgo.SmartClient.UserControls.Vault
{
    partial class VaultDialogControl
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
            this.components = new System.ComponentModel.Container();

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VaultDialogControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this._panelHeader = new System.Windows.Forms.Panel();
            this._buttonClose = new Bunifu.Framework.UI.BunifuImageButton();
            this._dataGridView = new Bunifu.UI.WinForms.BunifuDataGridView();
            this._dataGridColumnSite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._dataGridColumnDevice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._dataGridColumnRecorder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._dataGridColumnPlay = new System.Windows.Forms.DataGridViewImageColumn();
            this._labelTo = new System.Windows.Forms.Label();
            this._labelFrom = new System.Windows.Forms.Label();
            this._labelTitleForm = new Bunifu.Framework.UI.BunifuCustomLabel();
            this._labelName = new System.Windows.Forms.Label();
            this._labelFromData = new System.Windows.Forms.Label();
            this._labelToData = new System.Windows.Forms.Label();
            this._labelNameData = new System.Windows.Forms.Label();
            this._dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this._bunifuImageButtonFolder = new Bunifu.Framework.UI.BunifuImageButton();
            this._buttonOK = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this._textBoxName = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this._buttonCancel = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this._toolTipFolder = new System.Windows.Forms.ToolTip();
            this._panelHeader.SuspendLayout();
            this._errorProvider = new System.Windows.Forms.ErrorProvider();
            ((System.ComponentModel.ISupportInitialize)(this._buttonClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._bunifuImageButtonFolder)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelHeader
            // 
            this._panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(58)))), ((int)(((byte)(58)))));
            this._panelHeader.Controls.Add(this._buttonClose);
            this._panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelHeader.Location = new System.Drawing.Point(0, 0);
            this._panelHeader.Name = "PanelHeader";
            this._panelHeader.Size = new System.Drawing.Size(500, 26);
            this._panelHeader.TabIndex = 9;
            // 
            // ButtonClose
            // 
            this._buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this._buttonClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this._buttonClose.Image = ((System.Drawing.Image)(resources.GetObject("ButtonClose.Image")));
            this._buttonClose.ImageActive = null;
            this._buttonClose.Location = new System.Drawing.Point(470, 3);
            this._buttonClose.Name = "ButtonClose";
            this._buttonClose.Size = new System.Drawing.Size(22, 20);
            this._buttonClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._buttonClose.TabIndex = 0;
            this._buttonClose.TabStop = false;
            this._buttonClose.Zoom = 10;
            this._buttonClose.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // DataGridView
            // 
            this._dataGridView.AllowCustomTheming = true;
            this._dataGridView.AllowDrop = true;
            this._dataGridView.AllowUserToAddRows = false;
            this._dataGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            this._dataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this._dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._dataGridView.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this._dataGridView.CausesValidation = false;
            this._dataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this._dataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this._dataGridView.ColumnHeadersHeight = 40;
            this._dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._dataGridColumnSite,
            this._dataGridColumnDevice,
            this._dataGridColumnRecorder,
            this._dataGridColumnPlay});
            this._dataGridView.CurrentTheme.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._dataGridView.CurrentTheme.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this._dataGridView.CurrentTheme.AlternatingRowsStyle.ForeColor = System.Drawing.Color.White;
            this._dataGridView.CurrentTheme.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this._dataGridView.CurrentTheme.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            this._dataGridView.CurrentTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.CurrentTheme.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.CurrentTheme.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.CurrentTheme.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            this._dataGridView.CurrentTheme.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this._dataGridView.CurrentTheme.HeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.CurrentTheme.HeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            this._dataGridView.CurrentTheme.Name = null;
            this._dataGridView.CurrentTheme.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.CurrentTheme.RowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this._dataGridView.CurrentTheme.RowsStyle.ForeColor = System.Drawing.Color.White;
            this._dataGridView.CurrentTheme.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.CurrentTheme.RowsStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this._dataGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this._dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this._dataGridView.EnableHeadersVisualStyles = false;
            this._dataGridView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
#pragma warning disable CS0618 // Type or member is obsolete
            this._dataGridView.HeaderBgColor = System.Drawing.Color.Empty;
#pragma warning restore CS0618 // Type or member is obsolete
            this._dataGridView.HeaderForeColor = System.Drawing.Color.White;
            this._dataGridView.Location = new System.Drawing.Point(24, 224);
            this._dataGridView.Name = "DataGridView";
            this._dataGridView.ReadOnly = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this._dataGridView.RowHeadersVisible = false;
            this._dataGridView.RowHeadersWidth = 51;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.Color.White;
            this._dataGridView.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this._dataGridView.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this._dataGridView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._dataGridView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
            this._dataGridView.RowTemplate.Height = 40;
            this._dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._dataGridView.ShowCellErrors = false;
            this._dataGridView.ShowCellToolTips = true;
            this._dataGridView.ShowEditingIcon = false;
            this._dataGridView.ShowRowErrors = false;
            this._dataGridView.Size = new System.Drawing.Size(449, 197);
            this._dataGridView.TabIndex = 27;
            this._dataGridView.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.Dark;
            this._dataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView_CellMouseClick);
            this._dataGridView.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellMouseEnter);
            // 
            // Site
            // 
            this._dataGridColumnSite.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._dataGridColumnSite.DataPropertyName = "Site";
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this._dataGridColumnSite.DefaultCellStyle = dataGridViewCellStyle3;
            this._dataGridColumnSite.FillWeight = 150F;
            this._dataGridColumnSite.HeaderText = Resources.sitesText;
            this._dataGridColumnSite.MinimumWidth = 6;
            this._dataGridColumnSite.Name = "Site";
            this._dataGridColumnSite.ReadOnly = true;
            // 
            // Device
            // 
            this._dataGridColumnDevice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._dataGridColumnDevice.DataPropertyName = "DeviceName";
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this._dataGridColumnDevice.DefaultCellStyle = dataGridViewCellStyle4;
            this._dataGridColumnDevice.FillWeight = 250F;
            this._dataGridColumnDevice.HeaderText = Resources.Device;
            this._dataGridColumnDevice.MinimumWidth = 6;
            this._dataGridColumnDevice.Name = "Device";
            this._dataGridColumnDevice.ReadOnly = true;
            this._dataGridColumnDevice.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Recorder
            // 
            this._dataGridColumnRecorder.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this._dataGridColumnRecorder.DataPropertyName = "RecorderName";
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.White;
            this._dataGridColumnRecorder.DefaultCellStyle = dataGridViewCellStyle5;
            this._dataGridColumnRecorder.FillWeight = 150F;
            this._dataGridColumnRecorder.HeaderText = Resources.RecorderName;
            this._dataGridColumnRecorder.MinimumWidth = 6;
            this._dataGridColumnRecorder.Name = "Recorder";
            this._dataGridColumnRecorder.ReadOnly = true;
            this._dataGridColumnRecorder.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Play
            // 
            this._dataGridColumnPlay.HeaderText = "";
            this._dataGridColumnPlay.Image = global::Elipgo.SmartClient.UserControls.Properties.Resources.icon_play;
            this._dataGridColumnPlay.Name = "Play";
            this._dataGridColumnPlay.ReadOnly = true;
            this._dataGridColumnPlay.ToolTipText = Resources.Play;
            // 
            // labelHasta
            // 
            this._labelTo.AutoSize = true;
            this._labelTo.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            this._labelTo.ForeColor = System.Drawing.Color.White;
            this._labelTo.Location = new System.Drawing.Point(254, 108);
            this._labelTo.Name = "labelHasta";
            this._labelTo.Size = new System.Drawing.Size(29, 12);
            this._labelTo.TabIndex = 17;
            this._labelTo.Text = Resources.To;
            // 
            // labelDesde
            // 
            this._labelFrom.AutoSize = true;
            this._labelFrom.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            this._labelFrom.ForeColor = System.Drawing.Color.White;
            this._labelFrom.Location = new System.Drawing.Point(26, 109);
            this._labelFrom.Name = "labelDesde";
            this._labelFrom.Size = new System.Drawing.Size(32, 12);
            this._labelFrom.TabIndex = 15;
            this._labelFrom.Text = Resources.From;
            // 
            // LabelTitleForm
            // 
            this._labelTitleForm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._labelTitleForm.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this._labelTitleForm.ForeColor = System.Drawing.Color.White;
            this._labelTitleForm.Location = new System.Drawing.Point(4, -195);
            this._labelTitleForm.Name = "LabelTitleForm";
            this._labelTitleForm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._labelTitleForm.Size = new System.Drawing.Size(67, 27);
            this._labelTitleForm.TabIndex = 13;
            this._labelTitleForm.Text = "_TITLE_";
            this._labelTitleForm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelNombre
            // 
            this._labelName.AutoSize = true;
            this._labelName.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            this._labelName.ForeColor = System.Drawing.Color.White;
            this._labelName.Location = new System.Drawing.Point(26, 43);
            this._labelName.Name = "labelNombre";
            this._labelName.Size = new System.Drawing.Size(41, 12);
            this._labelName.TabIndex = 7;
            this._labelName.Text = Resources.Name;
            // 
            // lblDesde
            // 
            this._labelFromData.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._labelFromData.ForeColor = System.Drawing.Color.White;
            this._labelFromData.Location = new System.Drawing.Point(25, 132);
            this._labelFromData.Name = "lblDesde";
            this._labelFromData.Size = new System.Drawing.Size(220, 35);
            this._labelFromData.TabIndex = 36;
            this._labelFromData.Text = Resources.From;
            // 
            // lblHasta
            // 
            this._labelToData.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._labelToData.ForeColor = System.Drawing.Color.White;
            this._labelToData.Location = new System.Drawing.Point(253, 132);
            this._labelToData.Name = "lblHasta";
            this._labelToData.Size = new System.Drawing.Size(220, 35);
            this._labelToData.TabIndex = 37;
            this._labelToData.Text = Resources.To;
            // 
            // lblNombre
            // 
            this._labelNameData.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._labelNameData.ForeColor = System.Drawing.Color.White;
            this._labelNameData.Location = new System.Drawing.Point(28, 60);
            this._labelNameData.Name = "lblNombre";
            this._labelNameData.Size = new System.Drawing.Size(426, 35);
            this._labelNameData.TabIndex = 38;
            this._labelNameData.Text = Resources.Name;
            // 
            // dataGridViewImageColumn1
            // 
            this._dataGridViewImageColumn1.HeaderText = "";
            this._dataGridViewImageColumn1.Image = global::Elipgo.SmartClient.UserControls.Properties.Resources.icon_play;
            this._dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this._dataGridViewImageColumn1.Width = 69;
            // 
            // bunifuImageButtonFolder
            // 
            this._bunifuImageButtonFolder.Image = global::Elipgo.SmartClient.UserControls.Properties.Resources.icon_folder;
            this._bunifuImageButtonFolder.ImageActive = null;
            this._bunifuImageButtonFolder.Location = new System.Drawing.Point(31, 182);
            this._bunifuImageButtonFolder.Name = "bunifuImageButtonFolder";
            this._bunifuImageButtonFolder.Size = new System.Drawing.Size(24, 24);
            this._bunifuImageButtonFolder.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._bunifuImageButtonFolder.TabIndex = 35;
            this._bunifuImageButtonFolder.TabStop = false;
            this._bunifuImageButtonFolder.Visible = false;
            this._bunifuImageButtonFolder.Zoom = 10;
            this._bunifuImageButtonFolder.Click += new System.EventHandler(this.bunifuImageButtonFolder_Click);
            this._bunifuImageButtonFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this._toolTipFolder.SetToolTip(_bunifuImageButtonFolder, Resources.ShowFolder);
            // 
            // ButtonOK
            // 
            this._buttonOK.AllowToggling = false;
            this._buttonOK.AnimationSpeed = 200;
            this._buttonOK.AutoGenerateColors = false;
            this._buttonOK.BackColor = System.Drawing.Color.Transparent;
            this._buttonOK.BackColor1 = System.Drawing.Color.DodgerBlue;
            this._buttonOK.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonOK.BackgroundImage")));
            this._buttonOK.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonOK.ButtonText = "_OK_";
            this._buttonOK.ButtonTextMarginLeft = 0;
            this._buttonOK.ColorContrastOnClick = 45;
            this._buttonOK.ColorContrastOnHover = 45;
            this._buttonOK.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this._buttonOK.CustomizableEdges = borderEdges1;
            this._buttonOK.DialogResult = System.Windows.Forms.DialogResult.None;
            this._buttonOK.DisabledBorderColor = System.Drawing.Color.Empty;
            this._buttonOK.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this._buttonOK.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this._buttonOK.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this._buttonOK.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._buttonOK.ForeColor = System.Drawing.Color.White;
            this._buttonOK.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonOK.IconMarginLeft = 11;
            this._buttonOK.IconPadding = 10;
            this._buttonOK.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonOK.IdleBorderColor = System.Drawing.Color.DodgerBlue;
            this._buttonOK.IdleBorderRadius = 30;
            this._buttonOK.IdleBorderThickness = 1;
            this._buttonOK.IdleFillColor = System.Drawing.Color.DodgerBlue;
            this._buttonOK.IdleIconLeftImage = null;
            this._buttonOK.IdleIconRightImage = null;
            this._buttonOK.IndicateFocus = false;
            this._buttonOK.Location = new System.Drawing.Point(362, 438);
            this._buttonOK.Name = "ButtonOK";
            this._buttonOK.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonOK.onHoverState.BorderRadius = 30;
            this._buttonOK.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonOK.onHoverState.BorderThickness = 1;
            this._buttonOK.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonOK.onHoverState.ForeColor = System.Drawing.Color.White;
            this._buttonOK.onHoverState.IconLeftImage = null;
            this._buttonOK.onHoverState.IconRightImage = null;
            this._buttonOK.OnIdleState.BorderColor = System.Drawing.Color.DodgerBlue;
            this._buttonOK.OnIdleState.BorderRadius = 30;
            this._buttonOK.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonOK.OnIdleState.BorderThickness = 1;
            this._buttonOK.OnIdleState.FillColor = System.Drawing.Color.DodgerBlue;
            this._buttonOK.OnIdleState.ForeColor = System.Drawing.Color.White;
            this._buttonOK.OnIdleState.IconLeftImage = null;
            this._buttonOK.OnIdleState.IconRightImage = null;
            this._buttonOK.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonOK.OnPressedState.BorderRadius = 30;
            this._buttonOK.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonOK.OnPressedState.BorderThickness = 1;
            this._buttonOK.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonOK.OnPressedState.ForeColor = System.Drawing.Color.White;
            this._buttonOK.OnPressedState.IconLeftImage = null;
            this._buttonOK.OnPressedState.IconRightImage = null;
            this._buttonOK.Size = new System.Drawing.Size(92, 37);
            this._buttonOK.TabIndex = 1;
            this._buttonOK.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._buttonOK.TextMarginLeft = 0;
            this._buttonOK.UseDefaultRadiusAndThickness = true;
            this._buttonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // TextBoxName
            // 
            this._textBoxName.AcceptsReturn = false;
            this._textBoxName.AcceptsTab = false;
            this._textBoxName.AnimationSpeed = 200;
            this._textBoxName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this._textBoxName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this._textBoxName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._textBoxName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("TextBoxName.BackgroundImage")));
            this._textBoxName.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this._textBoxName.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this._textBoxName.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._textBoxName.BorderColorIdle = System.Drawing.Color.DimGray;
            this._textBoxName.BorderRadius = 1;
            this._textBoxName.BorderThickness = 1;
            this._textBoxName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this._textBoxName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this._textBoxName.DefaultFont = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._textBoxName.DefaultText = "";
            this._textBoxName.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this._textBoxName.ForeColor = System.Drawing.Color.White;
            this._textBoxName.HideSelection = true;
            this._textBoxName.IconLeft = null;
            this._textBoxName.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this._textBoxName.IconPadding = 10;
            this._textBoxName.IconRight = null;
            this._textBoxName.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this._textBoxName.Lines = new string[0];
            this._textBoxName.Location = new System.Drawing.Point(28, 60);
            this._textBoxName.MaxLength = 32767;
            this._textBoxName.MinimumSize = new System.Drawing.Size(1, 1);
            this._textBoxName.Modified = false;
            this._textBoxName.Multiline = false;
            this._textBoxName.Name = "TextBoxName";
            stateProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this._textBoxName.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.Empty;
            stateProperties2.FillColor = System.Drawing.Color.White;
            stateProperties2.ForeColor = System.Drawing.Color.Empty;
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.Silver;
            this._textBoxName.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this._textBoxName.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.DimGray;
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            stateProperties4.ForeColor = System.Drawing.Color.White;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this._textBoxName.OnIdleState = stateProperties4;
            this._textBoxName.PasswordChar = '\0';
            this._textBoxName.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this._textBoxName.PlaceholderText = "";
            this._textBoxName.ReadOnly = false;
            this._textBoxName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._textBoxName.SelectedText = "";
            this._textBoxName.SelectionLength = 0;
            this._textBoxName.SelectionStart = 0;
            this._textBoxName.ShortcutsEnabled = true;
            this._textBoxName.Size = new System.Drawing.Size(426, 35);
            this._textBoxName.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Material;
            this._textBoxName.TabIndex = 6;
            this._textBoxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this._textBoxName.TextMarginBottom = 0;
            this._textBoxName.TextMarginLeft = 5;
            this._textBoxName.TextMarginTop = 0;
            this._textBoxName.TextPlaceholder = "";
            this._textBoxName.UseSystemPasswordChar = false;
            this._textBoxName.WordWrap = true;
            this._textBoxName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBookmarkName_KeyPress);
            // 
            // ButtonCancel
            // 
            this._buttonCancel.AllowToggling = false;
            this._buttonCancel.AnimationSpeed = 200;
            this._buttonCancel.AutoGenerateColors = false;
            this._buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this._buttonCancel.BackColor1 = System.Drawing.Color.Transparent;
            this._buttonCancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonCancel.BackgroundImage")));
            this._buttonCancel.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancel.ButtonText = "_CANCEL_";
            this._buttonCancel.ButtonTextMarginLeft = 0;
            this._buttonCancel.ColorContrastOnClick = 45;
            this._buttonCancel.ColorContrastOnHover = 45;
            this._buttonCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this._buttonCancel.CustomizableEdges = borderEdges2;
            this._buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._buttonCancel.DisabledBorderColor = System.Drawing.Color.Empty;
            this._buttonCancel.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this._buttonCancel.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this._buttonCancel.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this._buttonCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this._buttonCancel.ForeColor = System.Drawing.Color.White;
            this._buttonCancel.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonCancel.IconMarginLeft = 11;
            this._buttonCancel.IconPadding = 10;
            this._buttonCancel.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this._buttonCancel.IdleBorderColor = System.Drawing.Color.DimGray;
            this._buttonCancel.IdleBorderRadius = 30;
            this._buttonCancel.IdleBorderThickness = 1;
            this._buttonCancel.IdleFillColor = System.Drawing.Color.Transparent;
            this._buttonCancel.IdleIconLeftImage = null;
            this._buttonCancel.IdleIconRightImage = null;
            this._buttonCancel.IndicateFocus = false;
            this._buttonCancel.Location = new System.Drawing.Point(256, 438);
            this._buttonCancel.Name = "ButtonCancel";
            this._buttonCancel.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonCancel.onHoverState.BorderRadius = 30;
            this._buttonCancel.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancel.onHoverState.BorderThickness = 1;
            this._buttonCancel.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this._buttonCancel.onHoverState.ForeColor = System.Drawing.Color.White;
            this._buttonCancel.onHoverState.IconLeftImage = null;
            this._buttonCancel.onHoverState.IconRightImage = null;
            this._buttonCancel.OnIdleState.BorderColor = System.Drawing.Color.DimGray;
            this._buttonCancel.OnIdleState.BorderRadius = 30;
            this._buttonCancel.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancel.OnIdleState.BorderThickness = 1;
            this._buttonCancel.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this._buttonCancel.OnIdleState.ForeColor = System.Drawing.Color.White;
            this._buttonCancel.OnIdleState.IconLeftImage = null;
            this._buttonCancel.OnIdleState.IconRightImage = null;
            this._buttonCancel.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonCancel.OnPressedState.BorderRadius = 30;
            this._buttonCancel.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this._buttonCancel.OnPressedState.BorderThickness = 1;
            this._buttonCancel.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this._buttonCancel.OnPressedState.ForeColor = System.Drawing.Color.White;
            this._buttonCancel.OnPressedState.IconLeftImage = null;
            this._buttonCancel.OnPressedState.IconRightImage = null;
            this._buttonCancel.Size = new System.Drawing.Size(92, 37);
            this._buttonCancel.TabIndex = 2;
            this._buttonCancel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._buttonCancel.TextMarginLeft = 0;
            this._buttonCancel.UseDefaultRadiusAndThickness = true;
            this._buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // VaultDialogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this._labelNameData);
            this.Controls.Add(this._labelToData);
            this.Controls.Add(this._labelFromData);
            this.Controls.Add(this._bunifuImageButtonFolder);
            this.Controls.Add(this._buttonOK);
            this.Controls.Add(this._dataGridView);
            this.Controls.Add(this._labelTo);
            this.Controls.Add(this._labelFrom);
            this.Controls.Add(this._labelTitleForm);
            this.Controls.Add(this._labelName);
            this.Controls.Add(this._textBoxName);
            this.Controls.Add(this._panelHeader);
            this.Controls.Add(this._buttonCancel);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "VaultDialogControl";
            this.Size = new System.Drawing.Size(500, 500);
            this._panelHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._buttonClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._bunifuImageButtonFolder)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
            // 
            // ErrorProvider
            // 
            this._errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this._errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
        }

        #endregion
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton _buttonCancel;
        private System.Windows.Forms.Panel _panelHeader;
        private Bunifu.Framework.UI.BunifuImageButton _buttonClose;
        private Bunifu.UI.WinForms.BunifuDataGridView _dataGridView;
        private System.Windows.Forms.Label _labelTo;
        private System.Windows.Forms.Label _labelFrom;
        private Bunifu.Framework.UI.BunifuCustomLabel _labelTitleForm;
        private System.Windows.Forms.Label _labelName;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox _textBoxName;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton _buttonOK;
        private Bunifu.Framework.UI.BunifuImageButton _bunifuImageButtonFolder;
        private System.Windows.Forms.Label _labelFromData;
        private System.Windows.Forms.Label _labelToData;
        private System.Windows.Forms.Label _labelNameData;
        private System.Windows.Forms.DataGridViewTextBoxColumn _dataGridColumnSite;
        private System.Windows.Forms.DataGridViewTextBoxColumn _dataGridColumnDevice;
        private System.Windows.Forms.DataGridViewTextBoxColumn _dataGridColumnRecorder;
        private System.Windows.Forms.DataGridViewImageColumn _dataGridColumnPlay;
        private System.Windows.Forms.DataGridViewImageColumn _dataGridViewImageColumn1;
        private System.Windows.Forms.ToolTip _toolTipFolder;
        private System.Windows.Forms.ErrorProvider _errorProvider;
    }
}
