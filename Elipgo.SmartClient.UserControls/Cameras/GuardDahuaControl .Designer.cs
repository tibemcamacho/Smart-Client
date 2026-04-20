using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.Cameras
{
    partial class GuardDahuaControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GuardDahuaControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LabelDahuaGuardNote = new System.Windows.Forms.Label();
            this.bunifuSeparator1 = new Bunifu.Framework.UI.BunifuSeparator();
            this.bunifuSeparator2 = new Bunifu.Framework.UI.BunifuSeparator();
            this.LabelPreset = new System.Windows.Forms.Label();
            this.DropdownPresets = new Bunifu.UI.WinForms.BunifuDropdown();
            this.elementsAvailable = new System.Windows.Forms.Label();
            this.LabelSelectedItems = new System.Windows.Forms.Label();
            this.DataGridViewItems = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GuardName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.deleteDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.order = new System.Windows.Forms.DataGridViewImageColumn();
            this.LabelTitle = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.ButtonAdd = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.labelName = new System.Windows.Forms.Label();
            this.LabelGuardName = new System.Windows.Forms.Label();
            this.errorManager = new System.Windows.Forms.ErrorProvider(this.components);
            this.panelPag = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewItems)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelPag);
            this.panel1.Controls.Add(this.LabelDahuaGuardNote);
            this.panel1.Controls.Add(this.bunifuSeparator1);
            this.panel1.Controls.Add(this.bunifuSeparator2);
            this.panel1.Controls.Add(this.LabelPreset);
            this.panel1.Controls.Add(this.DropdownPresets);
            this.panel1.Controls.Add(this.elementsAvailable);
            this.panel1.Controls.Add(this.LabelSelectedItems);
            this.panel1.Controls.Add(this.DataGridViewItems);
            this.panel1.Controls.Add(this.LabelTitle);
            this.panel1.Controls.Add(this.ButtonAdd);
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.LabelGuardName);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(1, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(745, 495);
            this.panel1.TabIndex = 3;
            // 
            // LabelDahuaGuardNote
            // 
            this.LabelDahuaGuardNote.Location = new System.Drawing.Point(10, 44);
            this.LabelDahuaGuardNote.Name = "LabelDahuaGuardNote";
            this.LabelDahuaGuardNote.Size = new System.Drawing.Size(908, 36);
            this.LabelDahuaGuardNote.TabIndex = 36;
            this.LabelDahuaGuardNote.Text = "label1";
            // 
            // bunifuSeparator1
            // 
            this.bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator1.LineThickness = 1;
            this.bunifuSeparator1.Location = new System.Drawing.Point(7, 128);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Size = new System.Drawing.Size(460, 8);
            this.bunifuSeparator1.TabIndex = 35;
            this.bunifuSeparator1.Transparency = 255;
            this.bunifuSeparator1.Vertical = false;
            // 
            // bunifuSeparator2
            // 
            this.bunifuSeparator2.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator2.LineThickness = 1;
            this.bunifuSeparator2.Location = new System.Drawing.Point(7, 244);
            this.bunifuSeparator2.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator2.Name = "bunifuSeparator2";
            this.bunifuSeparator2.Size = new System.Drawing.Size(460, 8);
            this.bunifuSeparator2.TabIndex = 33;
            this.bunifuSeparator2.Transparency = 255;
            this.bunifuSeparator2.Vertical = false;
            // 
            // LabelPreset
            // 
            this.LabelPreset.AutoSize = true;
            this.LabelPreset.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.LabelPreset.ForeColor = System.Drawing.Color.Silver;
            this.LabelPreset.Location = new System.Drawing.Point(6, 197);
            this.LabelPreset.Name = "LabelPreset";
            this.LabelPreset.Size = new System.Drawing.Size(31, 12);
            this.LabelPreset.TabIndex = 32;
            this.LabelPreset.Text = "Preset";
            // 
            // DropdownPresets
            // 
            this.DropdownPresets.BackColor = System.Drawing.Color.Transparent;
            this.DropdownPresets.BorderRadius = 1;
            this.DropdownPresets.Color = System.Drawing.Color.Transparent;
            this.DropdownPresets.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DropdownPresets.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.DropdownPresets.DisabledColor = System.Drawing.Color.Gray;
            this.DropdownPresets.DisplayMember = "Name";
            this.DropdownPresets.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DropdownPresets.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.DropdownPresets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DropdownPresets.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.DropdownPresets.FillDropDown = false;
            this.DropdownPresets.FillIndicator = false;
            this.DropdownPresets.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DropdownPresets.ForeColor = System.Drawing.Color.White;
            this.DropdownPresets.FormattingEnabled = true;
            this.DropdownPresets.Icon = null;
            this.DropdownPresets.IndicatorColor = System.Drawing.Color.White;
            this.DropdownPresets.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.DropdownPresets.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DropdownPresets.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.DropdownPresets.ItemForeColor = System.Drawing.Color.White;
            this.DropdownPresets.ItemHeight = 26;
            this.DropdownPresets.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.DropdownPresets.Location = new System.Drawing.Point(4, 216);
            this.DropdownPresets.Margin = new System.Windows.Forms.Padding(2);
            this.DropdownPresets.Name = "DropdownPresets";
            this.DropdownPresets.Size = new System.Drawing.Size(480, 32);
            this.DropdownPresets.TabIndex = 31;
            this.DropdownPresets.Text = null;
            this.DropdownPresets.ValueMember = "Id";
            // 
            // elementsAvailable
            // 
            this.elementsAvailable.AutoSize = true;
            this.elementsAvailable.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.elementsAvailable.ForeColor = System.Drawing.Color.Silver;
            this.elementsAvailable.Location = new System.Drawing.Point(4, 169);
            this.elementsAvailable.Name = "elementsAvailable";
            this.elementsAvailable.Size = new System.Drawing.Size(136, 20);
            this.elementsAvailable.TabIndex = 30;
            this.elementsAvailable.Text = "Preset disponibles";
            // 
            // LabelSelectedItems
            // 
            this.LabelSelectedItems.AutoSize = true;
            this.LabelSelectedItems.Location = new System.Drawing.Point(458, 271);
            this.LabelSelectedItems.Name = "LabelSelectedItems";
            this.LabelSelectedItems.Size = new System.Drawing.Size(127, 13);
            this.LabelSelectedItems.TabIndex = 29;
            this.LabelSelectedItems.Text = "Elementos seleccionados";
            // 
            // DataGridViewItems
            // 
            this.DataGridViewItems.AllowCustomTheming = false;
            this.DataGridViewItems.AllowDrop = true;
            this.DataGridViewItems.AllowUserToAddRows = false;
            this.DataGridViewItems.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(48)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.DataGridViewItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DataGridViewItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DataGridViewItems.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DataGridViewItems.CausesValidation = false;
            this.DataGridViewItems.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.DataGridViewItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DataGridViewItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DataGridViewItems.ColumnHeadersHeight = 40;
            this.DataGridViewItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.GuardName,
            this.deleteDataGridViewCheckBoxColumn,
            this.order});
            this.DataGridViewItems.CurrentTheme.AlternatingRowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(48)))), ((int)(((byte)(52)))));
            this.DataGridViewItems.CurrentTheme.AlternatingRowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.DataGridViewItems.CurrentTheme.AlternatingRowsStyle.ForeColor = System.Drawing.Color.White;
            this.DataGridViewItems.CurrentTheme.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(117)))), ((int)(((byte)(119)))));
            this.DataGridViewItems.CurrentTheme.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            this.DataGridViewItems.CurrentTheme.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            this.DataGridViewItems.CurrentTheme.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(56)))), ((int)(((byte)(62)))));
            this.DataGridViewItems.CurrentTheme.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            this.DataGridViewItems.CurrentTheme.HeaderStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            this.DataGridViewItems.CurrentTheme.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.DataGridViewItems.CurrentTheme.HeaderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            this.DataGridViewItems.CurrentTheme.HeaderStyle.SelectionForeColor = System.Drawing.Color.White;
            this.DataGridViewItems.CurrentTheme.Name = null;
            this.DataGridViewItems.CurrentTheme.RowsStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this.DataGridViewItems.CurrentTheme.RowsStyle.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.DataGridViewItems.CurrentTheme.RowsStyle.ForeColor = System.Drawing.Color.White;
            this.DataGridViewItems.CurrentTheme.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(117)))), ((int)(((byte)(119)))));
            this.DataGridViewItems.CurrentTheme.RowsStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(117)))), ((int)(((byte)(119)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DataGridViewItems.DefaultCellStyle = dataGridViewCellStyle3;
            this.DataGridViewItems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.DataGridViewItems.EnableHeadersVisualStyles = false;
            this.DataGridViewItems.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(56)))), ((int)(((byte)(62)))));
            this.DataGridViewItems.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            //this.DataGridViewItems.HeaderBgColor = System.Drawing.Color.Empty;
            this.DataGridViewItems.HeaderForeColor = System.Drawing.Color.White;
            this.DataGridViewItems.Location = new System.Drawing.Point(0, 287);
            this.DataGridViewItems.Name = "DataGridViewItems";
            this.DataGridViewItems.RowHeadersVisible = false;
            this.DataGridViewItems.RowHeadersWidth = 51;
            this.DataGridViewItems.RowTemplate.Height = 40;
            this.DataGridViewItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DataGridViewItems.ShowCellErrors = false;
            this.DataGridViewItems.ShowCellToolTips = false;
            this.DataGridViewItems.ShowEditingIcon = false;
            this.DataGridViewItems.ShowRowErrors = false;
            this.DataGridViewItems.Size = new System.Drawing.Size(720, 175);
            this.DataGridViewItems.TabIndex = 27;
            this.DataGridViewItems.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.Dark;
            this.DataGridViewItems.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewItems_CellMouseDown);
            this.DataGridViewItems.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewItems_CellMouseEnter);
            this.DataGridViewItems.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DataGridViewItems_CellPainting);
            this.DataGridViewItems.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridViewItems_ColumnHeaderMouseClick);
            this.DataGridViewItems.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DataGridViewItems_DataError);
            this.DataGridViewItems.DragDrop += new System.Windows.Forms.DragEventHandler(this.DataGridViewItems_DragDrop);
            this.DataGridViewItems.DragOver += new System.Windows.Forms.DragEventHandler(this.DataGridViewItems_DragOver);
            this.DataGridViewItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DataGridViewItems_MouseDown);
            this.DataGridViewItems.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DataGridViewItems_MouseMove);
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
            // deleteDataGridViewCheckBoxColumn
            // 
            this.deleteDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.deleteDataGridViewCheckBoxColumn.DataPropertyName = "IsDeleted";
            this.deleteDataGridViewCheckBoxColumn.HeaderText = "Delete";
            this.deleteDataGridViewCheckBoxColumn.MinimumWidth = 6;
            this.deleteDataGridViewCheckBoxColumn.Name = "deleteDataGridViewCheckBoxColumn";
            this.deleteDataGridViewCheckBoxColumn.Width = 125;
            // 
            // order
            // 
            this.order.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.order.HeaderText = "order";
            this.order.Image = ((System.Drawing.Image)(resources.GetObject("order.Image")));
            this.order.MinimumWidth = 6;
            this.order.Name = "order";
            this.order.Width = 125;
            // 
            // LabelTitle
            // 
            this.LabelTitle.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LabelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.LabelTitle.ForeColor = System.Drawing.Color.Silver;
            this.LabelTitle.Location = new System.Drawing.Point(4, 3);
            this.LabelTitle.Name = "LabelTitle";
            this.LabelTitle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LabelTitle.Size = new System.Drawing.Size(131, 27);
            this.LabelTitle.TabIndex = 13;
            this.LabelTitle.Text = "Editar Guardia";
            this.LabelTitle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            this.ButtonAdd.Location = new System.Drawing.Point(599, 216);
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
            this.ButtonAdd.UseDefaultRadiusAndThickness = true;
            this.ButtonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelName.ForeColor = System.Drawing.Color.Silver;
            this.labelName.Location = new System.Drawing.Point(5, 84);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 12);
            this.labelName.TabIndex = 7;
            this.labelName.Text = Resources.Name;//"Nombre";
            // 
            // LabelGuardName
            // 
            this.LabelGuardName.BackColor = System.Drawing.Color.Transparent;
            this.LabelGuardName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.LabelGuardName.Enabled = false;
            this.LabelGuardName.ForeColor = System.Drawing.Color.White;
            this.LabelGuardName.Location = new System.Drawing.Point(7, 101);
            this.LabelGuardName.MinimumSize = new System.Drawing.Size(1, 1);
            this.LabelGuardName.Name = "LabelGuardName";
            this.LabelGuardName.Size = new System.Drawing.Size(460, 23);
            this.LabelGuardName.TabIndex = 34;
            this.LabelGuardName.Text = "Guard Name";
            // 
            // errorManager
            // 
            this.errorManager.ContainerControl = this;
            // 
            // panelPag
            // 
            this.panelPag.Location = new System.Drawing.Point(247, 467);
            this.panelPag.Name = "panelPag";
            this.panelPag.Size = new System.Drawing.Size(200, 25);
            this.panelPag.TabIndex = 37;
            // 
            // GuardDahuaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "GuardDahuaControl";
            this.Size = new System.Drawing.Size(745, 495);
            this.Load += new System.EventHandler(this.GuardControl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DataGridViewItems)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LabelSelectedItems;
        private Bunifu.UI.WinForms.BunifuDataGridView DataGridViewItems;
        private Bunifu.Framework.UI.BunifuCustomLabel LabelTitle;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonAdd;
        private System.Windows.Forms.Label labelName;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator2;
        private System.Windows.Forms.Label LabelPreset;
        private Bunifu.UI.WinForms.BunifuDropdown DropdownPresets;
        private System.Windows.Forms.Label elementsAvailable;
        private System.Windows.Forms.ErrorProvider errorManager;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn GuardName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn deleteDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn order;
        private System.Windows.Forms.Label LabelGuardName;
        private System.Windows.Forms.Label LabelDahuaGuardNote;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator1;
        private System.Windows.Forms.Panel panelPag;
    }
}
