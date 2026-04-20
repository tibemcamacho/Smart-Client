using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.Cameras
{
    partial class GuardControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GuardControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            this.bunifuToolTip1 = new Bunifu.UI.WinForms.BunifuToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelPag = new System.Windows.Forms.Panel();
            this.bunifuSeparator2 = new Bunifu.Framework.UI.BunifuSeparator();
            this.tilteForm = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.lblPreset = new System.Windows.Forms.Label();
            this.presetObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.elementsAvailable = new System.Windows.Forms.Label();
            this.elementosSeleccionados = new System.Windows.Forms.Label();
            this.bunifuDataObject = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GuardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SpeedColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.UnitTimeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.deleteDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.order = new System.Windows.Forms.DataGridViewImageColumn();
            this.bunifuSeparator3 = new Bunifu.Framework.UI.BunifuSeparator();
            this.bunifuSeparator1 = new Bunifu.Framework.UI.BunifuSeparator();
            this.labelTime = new System.Windows.Forms.Label();
            this.ButtonAdd = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.unitObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.timeObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.labelName = new System.Windows.Forms.Label();
            this.txtName = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.errorManager = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuDataObject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).BeginInit();
            this.SuspendLayout();
            // 
            // bunifuToolTip1
            // 
            this.bunifuToolTip1.Active = true;
            this.bunifuToolTip1.AlignTextWithTitle = false;
            this.bunifuToolTip1.AllowAutoClose = true;
            this.bunifuToolTip1.AllowFading = true;
            this.bunifuToolTip1.AutoCloseDuration = 1000;
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
            // panel1
            // 
            this.panel1.Controls.Add(this.panelPag);
            this.panel1.Controls.Add(this.bunifuSeparator2);
            this.panel1.Controls.Add(this.tilteForm);
            this.panel1.Controls.Add(this.lblPreset);
            this.panel1.Controls.Add(this.presetObject);
            this.panel1.Controls.Add(this.elementsAvailable);
            this.panel1.Controls.Add(this.elementosSeleccionados);
            this.panel1.Controls.Add(this.bunifuDataObject);
            this.panel1.Controls.Add(this.bunifuSeparator3);
            this.panel1.Controls.Add(this.bunifuSeparator1);
            this.panel1.Controls.Add(this.labelTime);
            this.panel1.Controls.Add(this.ButtonAdd);
            this.panel1.Controls.Add(this.unitObject);
            this.panel1.Controls.Add(this.timeObject);
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(745, 495);
            this.panel1.TabIndex = 3;
            this.bunifuToolTip1.SetToolTip(this.panel1, "");
            this.bunifuToolTip1.SetToolTipIcon(this.panel1, null);
            this.bunifuToolTip1.SetToolTipTitle(this.panel1, "");
            // 
            // panelPag
            // 
            this.panelPag.Location = new System.Drawing.Point(277, 460);
            this.panelPag.Name = "panelPag";
            this.panelPag.Size = new System.Drawing.Size(200, 25);
            this.panelPag.TabIndex = 34;
            this.bunifuToolTip1.SetToolTip(this.panelPag, "");
            this.bunifuToolTip1.SetToolTipIcon(this.panelPag, null);
            this.bunifuToolTip1.SetToolTipTitle(this.panelPag, "");
            // 
            // bunifuSeparator2
            // 
            this.bunifuSeparator2.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator2.LineThickness = 1;
            this.bunifuSeparator2.Location = new System.Drawing.Point(7, 252);
            this.bunifuSeparator2.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator2.Name = "bunifuSeparator2";
            this.bunifuSeparator2.Size = new System.Drawing.Size(460, 8);
            this.bunifuSeparator2.TabIndex = 33;
            this.bunifuToolTip1.SetToolTip(this.bunifuSeparator2, "");
            this.bunifuToolTip1.SetToolTipIcon(this.bunifuSeparator2, null);
            this.bunifuToolTip1.SetToolTipTitle(this.bunifuSeparator2, "");
            this.bunifuSeparator2.Transparency = 255;
            this.bunifuSeparator2.Vertical = false;
            // 
            // tilteForm
            // 
            this.tilteForm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tilteForm.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.tilteForm.ForeColor = System.Drawing.Color.Silver;
            this.tilteForm.Location = new System.Drawing.Point(34, 11);
            this.tilteForm.Name = "tilteForm";
            this.tilteForm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tilteForm.Size = new System.Drawing.Size(136, 27);
            this.tilteForm.TabIndex = 13;
            this.tilteForm.Text = "Nueva  Guardia";
            this.tilteForm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bunifuToolTip1.SetToolTip(this.tilteForm, "");
            this.bunifuToolTip1.SetToolTipIcon(this.tilteForm, null);
            this.bunifuToolTip1.SetToolTipTitle(this.tilteForm, "");
            // 
            // lblPreset
            // 
            this.lblPreset.AutoSize = true;
            this.lblPreset.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.lblPreset.ForeColor = System.Drawing.Color.Silver;
            this.lblPreset.Location = new System.Drawing.Point(6, 205);
            this.lblPreset.Name = "lblPreset";
            this.lblPreset.Size = new System.Drawing.Size(31, 12);
            this.lblPreset.TabIndex = 32;
            this.lblPreset.Text = "Preset";
            this.bunifuToolTip1.SetToolTip(this.lblPreset, "");
            this.bunifuToolTip1.SetToolTipIcon(this.lblPreset, null);
            this.bunifuToolTip1.SetToolTipTitle(this.lblPreset, "");
            // 
            // presetObject
            // 
            this.presetObject.BackColor = System.Drawing.Color.Transparent;
            this.presetObject.BorderRadius = 1;
            this.presetObject.Color = System.Drawing.Color.Transparent;
            this.presetObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.presetObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.presetObject.DisabledColor = System.Drawing.Color.Gray;
            this.presetObject.DisplayMember = "Name";
            this.presetObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.presetObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.presetObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presetObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.presetObject.FillDropDown = false;
            this.presetObject.FillIndicator = false;
            this.presetObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.presetObject.ForeColor = System.Drawing.Color.White;
            this.presetObject.FormattingEnabled = true;
            this.presetObject.Icon = null;
            this.presetObject.IndicatorColor = System.Drawing.Color.White;
            this.presetObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.presetObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.presetObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.presetObject.ItemForeColor = System.Drawing.Color.White;
            this.presetObject.ItemHeight = 26;
            this.presetObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.presetObject.Location = new System.Drawing.Point(4, 224);
            this.presetObject.Margin = new System.Windows.Forms.Padding(2);
            this.presetObject.Name = "presetObject";
            this.presetObject.Size = new System.Drawing.Size(480, 32);
            this.presetObject.TabIndex = 31;
            this.presetObject.Text = null;
            this.bunifuToolTip1.SetToolTip(this.presetObject, "");
            this.bunifuToolTip1.SetToolTipIcon(this.presetObject, null);
            this.bunifuToolTip1.SetToolTipTitle(this.presetObject, "");
            this.presetObject.ValueMember = "Id";
            // 
            // elementsAvailable
            // 
            this.elementsAvailable.AutoSize = true;
            this.elementsAvailable.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.elementsAvailable.ForeColor = System.Drawing.Color.Silver;
            this.elementsAvailable.Location = new System.Drawing.Point(5, 163);
            this.elementsAvailable.Name = "elementsAvailable";
            this.elementsAvailable.Size = new System.Drawing.Size(136, 20);
            this.elementsAvailable.TabIndex = 30;
            this.elementsAvailable.Text = "Preset disponibles";
            this.bunifuToolTip1.SetToolTip(this.elementsAvailable, "");
            this.bunifuToolTip1.SetToolTipIcon(this.elementsAvailable, null);
            this.bunifuToolTip1.SetToolTipTitle(this.elementsAvailable, "");
            // 
            // elementosSeleccionados
            // 
            this.elementosSeleccionados.AutoSize = true;
            this.elementosSeleccionados.Location = new System.Drawing.Point(555, 264);
            this.elementosSeleccionados.Name = "elementosSeleccionados";
            this.elementosSeleccionados.Size = new System.Drawing.Size(127, 13);
            this.elementosSeleccionados.TabIndex = 29;
            this.elementosSeleccionados.Text = "Elementos seleccionados";
            this.bunifuToolTip1.SetToolTip(this.elementosSeleccionados, "");
            this.bunifuToolTip1.SetToolTipIcon(this.elementosSeleccionados, null);
            this.bunifuToolTip1.SetToolTipTitle(this.elementosSeleccionados, "");
            // 
            // bunifuDataObject
            // 
            this.bunifuDataObject.AllowCustomTheming = false;
            this.bunifuDataObject.AllowDrop = true;
            this.bunifuDataObject.AllowUserToAddRows = false;
            this.bunifuDataObject.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(48)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.bunifuDataObject.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.bunifuDataObject.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bunifuDataObject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bunifuDataObject.CausesValidation = false;
            this.bunifuDataObject.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.bunifuDataObject.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.bunifuDataObject.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.bunifuDataObject.ColumnHeadersHeight = 40;
            this.bunifuDataObject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.GuardName,
            this.SpeedColumn,
            this.TimeColumn,
            this.UnitTimeColumn,
            this.deleteDataGridViewCheckBoxColumn,
            this.order});
            this.bunifuDataObject.CurrentTheme.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(48)))), ((int)(((byte)(52)))));
            this.bunifuDataObject.CurrentTheme.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.bunifuDataObject.CurrentTheme.AlternatingRowsStyle.ForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.CurrentTheme.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(117)))), ((int)(((byte)(119)))));
            this.bunifuDataObject.CurrentTheme.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            this.bunifuDataObject.CurrentTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            this.bunifuDataObject.CurrentTheme.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(56)))), ((int)(((byte)(62)))));
            this.bunifuDataObject.CurrentTheme.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            this.bunifuDataObject.CurrentTheme.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            this.bunifuDataObject.CurrentTheme.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.CurrentTheme.HeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.bunifuDataObject.CurrentTheme.HeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.CurrentTheme.Name = null;
            this.bunifuDataObject.CurrentTheme.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.bunifuDataObject.CurrentTheme.RowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.bunifuDataObject.CurrentTheme.RowsStyle.ForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.CurrentTheme.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(117)))), ((int)(((byte)(119)))));
            this.bunifuDataObject.CurrentTheme.RowsStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(117)))), ((int)(((byte)(119)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.bunifuDataObject.DefaultCellStyle = dataGridViewCellStyle3;
            this.bunifuDataObject.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.bunifuDataObject.EnableHeadersVisualStyles = false;
            this.bunifuDataObject.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(56)))), ((int)(((byte)(62)))));
            this.bunifuDataObject.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            //this.bunifuDataObject.HeaderBgColor = System.Drawing.Color.Empty;
            this.bunifuDataObject.HeaderForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.Location = new System.Drawing.Point(0, 283);
            this.bunifuDataObject.Name = "bunifuDataObject";
            this.bunifuDataObject.RowHeadersVisible = false;
            this.bunifuDataObject.RowHeadersWidth = 51;
            this.bunifuDataObject.RowTemplate.Height = 40;
            this.bunifuDataObject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.bunifuDataObject.ShowCellErrors = false;
            this.bunifuDataObject.ShowCellToolTips = false;
            this.bunifuDataObject.ShowEditingIcon = false;
            this.bunifuDataObject.ShowRowErrors = false;
            this.bunifuDataObject.Size = new System.Drawing.Size(720, 170);
            this.bunifuDataObject.TabIndex = 27;
            this.bunifuDataObject.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.Dark;
            this.bunifuToolTip1.SetToolTip(this.bunifuDataObject, "");
            this.bunifuToolTip1.SetToolTipIcon(this.bunifuDataObject, null);
            this.bunifuToolTip1.SetToolTipTitle(this.bunifuDataObject, "");
            this.bunifuDataObject.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BunifuDataObject_CellMouseDown);
            this.bunifuDataObject.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.BunifuDataObject_CellMouseEnter);
            this.bunifuDataObject.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.bunifuDataObject_CellMouseLeave);
            this.bunifuDataObject.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.BunifuDataObject_CellPainting);
            this.bunifuDataObject.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BunifuDataObject_ColumnHeaderMouseClick);
            this.bunifuDataObject.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.bunifuDataObject_DataError);
            this.bunifuDataObject.DragDrop += new System.Windows.Forms.DragEventHandler(this.BunifuDataObject_DragDrop);
            this.bunifuDataObject.DragOver += new System.Windows.Forms.DragEventHandler(this.BunifuDataObject_DragOver);
            this.bunifuDataObject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BunifuDataObject_MouseDown);
            this.bunifuDataObject.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BunifuDataObject_MouseMove);
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.MinimumWidth = 6;
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // GuardName
            // 
            this.GuardName.DataPropertyName = "Name";
            this.GuardName.FillWeight = 300F;
            this.GuardName.HeaderText = "Fuente";
            this.GuardName.MinimumWidth = 6;
            this.GuardName.Name = "GuardName";
            this.GuardName.ReadOnly = true;
            this.GuardName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // SpeedColumn
            // 
            this.SpeedColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SpeedColumn.DataPropertyName = "Speed";
            this.SpeedColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.SpeedColumn.FillWeight = 300F;
            this.SpeedColumn.HeaderText = "Velocidad";
            this.SpeedColumn.MinimumWidth = 6;
            this.SpeedColumn.Name = "SpeedColumn";
            this.SpeedColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // TimeColumn
            // 
            this.TimeColumn.DataPropertyName = "Time";
            this.TimeColumn.HeaderText = "Duración";
            this.TimeColumn.MinimumWidth = 6;
            this.TimeColumn.Name = "TimeColumn";
            // 
            // UnitTimeColumn
            // 
            this.UnitTimeColumn.DataPropertyName = "UnitTime";
            this.UnitTimeColumn.HeaderText = "";
            this.UnitTimeColumn.MinimumWidth = 6;
            this.UnitTimeColumn.Name = "UnitTimeColumn";
            // 
            // deleteDataGridViewCheckBoxColumn
            // 
            this.deleteDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.deleteDataGridViewCheckBoxColumn.DataPropertyName = "IsDeleted";
            this.deleteDataGridViewCheckBoxColumn.HeaderText = "Delete";
            this.deleteDataGridViewCheckBoxColumn.MinimumWidth = 6;
            this.deleteDataGridViewCheckBoxColumn.Name = "deleteDataGridViewCheckBoxColumn";
            // 
            // order
            // 
            this.order.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.order.HeaderText = "order";
            this.order.Image = ((System.Drawing.Image)(resources.GetObject("order.Image")));
            this.order.MinimumWidth = 6;
            this.order.Name = "order";
            // 
            // bunifuSeparator3
            // 
            this.bunifuSeparator3.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator3.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator3.LineThickness = 1;
            this.bunifuSeparator3.Location = new System.Drawing.Point(405, 144);
            this.bunifuSeparator3.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator3.Name = "bunifuSeparator3";
            this.bunifuSeparator3.Size = new System.Drawing.Size(311, 8);
            this.bunifuSeparator3.TabIndex = 25;
            this.bunifuToolTip1.SetToolTip(this.bunifuSeparator3, "");
            this.bunifuToolTip1.SetToolTipIcon(this.bunifuSeparator3, null);
            this.bunifuToolTip1.SetToolTipTitle(this.bunifuSeparator3, "");
            this.bunifuSeparator3.Transparency = 255;
            this.bunifuSeparator3.Vertical = false;
            // 
            // bunifuSeparator1
            // 
            this.bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator1.LineThickness = 1;
            this.bunifuSeparator1.Location = new System.Drawing.Point(9, 141);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Size = new System.Drawing.Size(311, 8);
            this.bunifuSeparator1.TabIndex = 20;
            this.bunifuToolTip1.SetToolTip(this.bunifuSeparator1, "");
            this.bunifuToolTip1.SetToolTipIcon(this.bunifuSeparator1, null);
            this.bunifuToolTip1.SetToolTipTitle(this.bunifuSeparator1, "");
            this.bunifuSeparator1.Transparency = 255;
            this.bunifuSeparator1.Vertical = false;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelTime.ForeColor = System.Drawing.Color.Silver;
            this.labelTime.Location = new System.Drawing.Point(6, 100);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(99, 12);
            this.labelTime.TabIndex = 15;
            this.labelTime.Text = "Tiempo de Secuencia";
            this.bunifuToolTip1.SetToolTip(this.labelTime, "");
            this.bunifuToolTip1.SetToolTipIcon(this.labelTime, null);
            this.bunifuToolTip1.SetToolTipTitle(this.labelTime, "");
            // 
            // ButtonAdd
            // 
            this.ButtonAdd.AllowToggling = false;
            this.ButtonAdd.AnimationSpeed = 200;
            this.ButtonAdd.AutoGenerateColors = false;
            this.ButtonAdd.BackColor = System.Drawing.Color.Transparent;
            this.ButtonAdd.BackColor1 = System.Drawing.Color.Transparent;
            this.ButtonAdd.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ButtonAdd.BackgroundImage")));
            this.ButtonAdd.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonAdd.ButtonText = "Agregar a la lista";
            this.ButtonAdd.ButtonTextMarginLeft = 0;
            this.ButtonAdd.ColorContrastOnClick = 45;
            this.ButtonAdd.ColorContrastOnHover = 45;
            this.ButtonAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.ButtonAdd.CustomizableEdges = borderEdges1;
            this.ButtonAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.ButtonAdd.DisabledBorderColor = System.Drawing.Color.Empty;
            this.ButtonAdd.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.ButtonAdd.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));
            this.ButtonAdd.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;
            this.ButtonAdd.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.ButtonAdd.ForeColor = System.Drawing.Color.White;
            this.ButtonAdd.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonAdd.IconMarginLeft = 11;
            this.ButtonAdd.IconPadding = 10;
            this.ButtonAdd.IconRightCursor = System.Windows.Forms.Cursors.Hand;
            this.ButtonAdd.IdleBorderColor = System.Drawing.Color.DimGray;
            this.ButtonAdd.IdleBorderRadius = 30;
            this.ButtonAdd.IdleBorderThickness = 1;
            this.ButtonAdd.IdleFillColor = System.Drawing.Color.Transparent;
            this.ButtonAdd.IdleIconLeftImage = null;
            this.ButtonAdd.IdleIconRightImage = null;
            this.ButtonAdd.IndicateFocus = false;
            this.ButtonAdd.Location = new System.Drawing.Point(599, 224);
            this.ButtonAdd.Name = "ButtonAdd";
            this.ButtonAdd.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonAdd.onHoverState.BorderRadius = 30;
            this.ButtonAdd.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonAdd.onHoverState.BorderThickness = 1;
            this.ButtonAdd.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.ButtonAdd.onHoverState.ForeColor = System.Drawing.Color.White;
            this.ButtonAdd.onHoverState.IconLeftImage = null;
            this.ButtonAdd.onHoverState.IconRightImage = null;
            this.ButtonAdd.OnIdleState.BorderColor = System.Drawing.Color.DimGray;
            this.ButtonAdd.OnIdleState.BorderRadius = 30;
            this.ButtonAdd.OnIdleState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonAdd.OnIdleState.BorderThickness = 1;
            this.ButtonAdd.OnIdleState.FillColor = System.Drawing.Color.Transparent;
            this.ButtonAdd.OnIdleState.ForeColor = System.Drawing.Color.White;
            this.ButtonAdd.OnIdleState.IconLeftImage = null;
            this.ButtonAdd.OnIdleState.IconRightImage = null;
            this.ButtonAdd.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonAdd.OnPressedState.BorderRadius = 30;
            this.ButtonAdd.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.ButtonAdd.OnPressedState.BorderThickness = 1;
            this.ButtonAdd.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.ButtonAdd.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.ButtonAdd.OnPressedState.IconLeftImage = null;
            this.ButtonAdd.OnPressedState.IconRightImage = null;
            this.ButtonAdd.Size = new System.Drawing.Size(121, 37);
            this.ButtonAdd.TabIndex = 12;
            this.ButtonAdd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ButtonAdd.TextMarginLeft = 0;
            this.bunifuToolTip1.SetToolTip(this.ButtonAdd, "");
            this.bunifuToolTip1.SetToolTipIcon(this.ButtonAdd, null);
            this.bunifuToolTip1.SetToolTipTitle(this.ButtonAdd, "");
            this.ButtonAdd.UseDefaultRadiusAndThickness = true;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // unitObject
            // 
            this.unitObject.BackColor = System.Drawing.Color.Transparent;
            this.unitObject.BorderRadius = 1;
            this.unitObject.Color = System.Drawing.Color.Transparent;
            this.unitObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.unitObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.unitObject.DisabledColor = System.Drawing.Color.Gray;
            this.unitObject.DisplayMember = "Name";
            this.unitObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.unitObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.unitObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.unitObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.unitObject.FillDropDown = false;
            this.unitObject.FillIndicator = false;
            this.unitObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.unitObject.ForeColor = System.Drawing.Color.White;
            this.unitObject.FormattingEnabled = true;
            this.unitObject.Icon = null;
            this.unitObject.IndicatorColor = System.Drawing.Color.White;
            this.unitObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.unitObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.unitObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.unitObject.ItemForeColor = System.Drawing.Color.White;
            this.unitObject.ItemHeight = 26;
            this.unitObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.unitObject.Location = new System.Drawing.Point(402, 117);
            this.unitObject.Margin = new System.Windows.Forms.Padding(2);
            this.unitObject.Name = "unitObject";
            this.unitObject.Size = new System.Drawing.Size(328, 32);
            this.unitObject.TabIndex = 11;
            this.unitObject.Text = null;
            this.bunifuToolTip1.SetToolTip(this.unitObject, "");
            this.bunifuToolTip1.SetToolTipIcon(this.unitObject, null);
            this.bunifuToolTip1.SetToolTipTitle(this.unitObject, "");
            this.unitObject.ValueMember = "Key";
            // 
            // timeObject
            // 
            this.timeObject.BackColor = System.Drawing.Color.Transparent;
            this.timeObject.BorderRadius = 1;
            this.timeObject.Color = System.Drawing.Color.Transparent;
            this.timeObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.timeObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.timeObject.DisabledColor = System.Drawing.Color.Gray;
            this.timeObject.DisplayMember = "Name";
            this.timeObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.timeObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.timeObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timeObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.timeObject.FillDropDown = false;
            this.timeObject.FillIndicator = false;
            this.timeObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.timeObject.ForeColor = System.Drawing.Color.White;
            this.timeObject.FormattingEnabled = true;
            this.timeObject.Icon = null;
            this.timeObject.IndicatorColor = System.Drawing.Color.White;
            this.timeObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.timeObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.timeObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.timeObject.ItemForeColor = System.Drawing.Color.White;
            this.timeObject.ItemHeight = 26;
            this.timeObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.timeObject.Location = new System.Drawing.Point(7, 114);
            this.timeObject.Margin = new System.Windows.Forms.Padding(2);
            this.timeObject.Name = "timeObject";
            this.timeObject.Size = new System.Drawing.Size(328, 32);
            this.timeObject.TabIndex = 9;
            this.timeObject.Text = null;
            this.bunifuToolTip1.SetToolTip(this.timeObject, "");
            this.bunifuToolTip1.SetToolTipIcon(this.timeObject, null);
            this.bunifuToolTip1.SetToolTipTitle(this.timeObject, "");
            this.timeObject.ValueMember = "Key";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelName.ForeColor = System.Drawing.Color.Silver;
            this.labelName.Location = new System.Drawing.Point(5, 49);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 12);
            this.labelName.TabIndex = 7;
            this.labelName.Text = Resources.Name;// "Nombre";
            this.bunifuToolTip1.SetToolTip(this.labelName, "");
            this.bunifuToolTip1.SetToolTipIcon(this.labelName, null);
            this.bunifuToolTip1.SetToolTipTitle(this.labelName, "");
            // 
            // txtName
            // 
            this.txtName.AcceptsReturn = false;
            this.txtName.AcceptsTab = false;
            this.txtName.AnimationSpeed = 200;
            this.txtName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.txtName.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtName.BackgroundImage")));
            this.txtName.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this.txtName.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.txtName.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.txtName.BorderColorIdle = System.Drawing.Color.DimGray;
            this.txtName.BorderRadius = 1;
            this.txtName.BorderThickness = 1;
            this.txtName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.DefaultFont = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.txtName.DefaultText = "";
            this.txtName.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.txtName.ForeColor = System.Drawing.Color.White;
            this.txtName.HideSelection = true;
            this.txtName.IconLeft = null;
            this.txtName.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.IconPadding = 0;
            this.txtName.IconRight = null;
            this.txtName.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.Lines = new string[0];
            this.txtName.Location = new System.Drawing.Point(7, 68);
            this.txtName.MaxLength = 32767;
            this.txtName.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtName.Modified = false;
            this.txtName.Multiline = false;
            this.txtName.Name = "txtName";
            stateProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtName.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.Empty;
            stateProperties2.FillColor = System.Drawing.Color.White;
            stateProperties2.ForeColor = System.Drawing.Color.Empty;
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtName.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtName.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.DimGray;
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            stateProperties4.ForeColor = System.Drawing.Color.White;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtName.OnIdleState = stateProperties4;
            this.txtName.PasswordChar = '\0';
            this.txtName.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.txtName.PlaceholderText = "";
            this.txtName.ReadOnly = false;
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtName.SelectedText = "";
            this.txtName.SelectionLength = 0;
            this.txtName.SelectionStart = 0;
            this.txtName.ShortcutsEnabled = true;
            this.txtName.Size = new System.Drawing.Size(709, 20);
            this.txtName.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Material;
            this.txtName.TabIndex = 6;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtName.TextMarginBottom = 0;
            this.txtName.TextMarginLeft = 5;
            this.txtName.TextMarginTop = 0;
            this.txtName.TextPlaceholder = "";
            this.bunifuToolTip1.SetToolTip(this.txtName, "");
            this.bunifuToolTip1.SetToolTipIcon(this.txtName, null);
            this.bunifuToolTip1.SetToolTipTitle(this.txtName, "");
            this.txtName.UseSystemPasswordChar = false;
            this.txtName.WordWrap = true;
            // 
            // errorManager
            // 
            this.errorManager.ContainerControl = this;
            // 
            // GuardControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GuardControl";
            this.Size = new System.Drawing.Size(744, 495);
            this.bunifuToolTip1.SetToolTip(this, "");
            this.bunifuToolTip1.SetToolTipIcon(this, null);
            this.bunifuToolTip1.SetToolTipTitle(this, "");
            this.Load += new System.EventHandler(this.GuardControl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuDataObject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label elementosSeleccionados;
        private Bunifu.UI.WinForms.BunifuDataGridView bunifuDataObject;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator3;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator1;
        private System.Windows.Forms.Label labelTime;
        private Bunifu.Framework.UI.BunifuCustomLabel tilteForm;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonAdd;
        private Bunifu.UI.WinForms.BunifuDropdown unitObject;
        private Bunifu.UI.WinForms.BunifuDropdown timeObject;
        private System.Windows.Forms.Label labelName;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txtName;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator2;
        private System.Windows.Forms.Label lblPreset;
        private Bunifu.UI.WinForms.BunifuDropdown presetObject;
        private System.Windows.Forms.Label elementsAvailable;
        private System.Windows.Forms.ErrorProvider errorManager;
        private Bunifu.UI.WinForms.BunifuToolTip bunifuToolTip1;
        private System.Windows.Forms.Panel panelPag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn GuardName;
        private System.Windows.Forms.DataGridViewComboBoxColumn SpeedColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn TimeColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn UnitTimeColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn deleteDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn order;
    }
}
