using Elipgo.SmartClient.Common.Properties;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Groups
{
    partial class FormGroups
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGroups));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelPag = new System.Windows.Forms.Panel();
            this.bunifuSeparator5 = new Bunifu.Framework.UI.BunifuSeparator();
            this.label1 = new System.Windows.Forms.Label();
            this.tipoObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.elementosSeleccionados = new System.Windows.Forms.Label();
            this.bunifuDataObject = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.idDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.objectTitleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.typeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameObjectDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StreamColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.deleteDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.order = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataViewGroupBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bunifuSeparator4 = new Bunifu.Framework.UI.BunifuSeparator();
            this.bunifuSeparator3 = new Bunifu.Framework.UI.BunifuSeparator();
            this.bunifuSeparator2 = new Bunifu.Framework.UI.BunifuSeparator();
            this.bunifuSeparatorType = new Bunifu.Framework.UI.BunifuSeparator();
            this.bunifuSeparator1 = new Bunifu.Framework.UI.BunifuSeparator();
            this.labelNameObject = new System.Windows.Forms.Label();
            this.labelTypeObject = new System.Windows.Forms.Label();
            this.labelSitesGroup = new System.Windows.Forms.Label();
            this.labelObject = new System.Windows.Forms.Label();
            this.tilteForm = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.ButtonAdd = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            // CONTROL ANTIGUO COMENTADO
            // this.optionObjectName = new Bunifu.UI.WinForms.BunifuDropdown();

            // NUEVO USER CONTROL AGREGADO AQUÍ
            this.ucOptionObjectName = new Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown();

            this.optionTypeObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.optionObject = new Bunifu.UI.WinForms.BunifuDropdown();
            //this.optionSitesName = new Bunifu.UI.WinForms.BunifuDropdown();
            this.ucOptionSitesName = new Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown();
            this.elementsAvailable = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.txtName = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.labelTypeGrid = new System.Windows.Forms.Label();
            this.optionGrid = new Bunifu.UI.WinForms.BunifuDropdown();
            this.bindingSourceGroup = new System.Windows.Forms.BindingSource(this.components);
            this.errorManager = new System.Windows.Forms.ErrorProvider(this.components);
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuDataObject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataViewGroupBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelPag);
            this.panel1.Controls.Add(this.bunifuSeparator5);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.tipoObject);
            this.panel1.Controls.Add(this.elementosSeleccionados);
            this.panel1.Controls.Add(this.bunifuDataObject);
            this.panel1.Controls.Add(this.bunifuSeparator4);
            this.panel1.Controls.Add(this.bunifuSeparator3);
            this.panel1.Controls.Add(this.bunifuSeparator2);
            this.panel1.Controls.Add(this.bunifuSeparatorType);
            this.panel1.Controls.Add(this.bunifuSeparator1);
            this.panel1.Controls.Add(this.labelNameObject);
            this.panel1.Controls.Add(this.labelTypeObject);
            this.panel1.Controls.Add(this.labelSitesGroup);
            this.panel1.Controls.Add(this.labelObject);
            this.panel1.Controls.Add(this.tilteForm);
            this.panel1.Controls.Add(this.ButtonAdd);

            // AGREGAMOS EL NUEVO CONTROL AL PANEL
            this.panel1.Controls.Add(this.ucOptionObjectName);
            // COMENTAMOS EL AGREGADO DEL VIEJO CONTROL
            // this.panel1.Controls.Add(this.optionObjectName);

            this.panel1.Controls.Add(this.optionTypeObject);
            this.panel1.Controls.Add(this.optionObject);
            this.panel1.Controls.Add(this.ucOptionSitesName);
            this.panel1.Controls.Add(this.elementsAvailable);
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.labelTypeGrid);
            this.panel1.Controls.Add(this.optionGrid);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(744, 535);
            this.panel1.TabIndex = 1;
            // 
            // panelPag
            // 
            this.panelPag.Location = new System.Drawing.Point(264, 463);
            this.panelPag.Name = "panelPag";
            this.panelPag.Size = new System.Drawing.Size(201, 48);
            this.panelPag.TabIndex = 43;
            // 
            // bunifuSeparator5
            // 
            this.bunifuSeparator5.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator5.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator5.LineThickness = 1;
            this.bunifuSeparator5.Location = new System.Drawing.Point(512, 104);
            this.bunifuSeparator5.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator5.Name = "bunifuSeparator5";
            this.bunifuSeparator5.Size = new System.Drawing.Size(205, 10);
            this.bunifuSeparator5.TabIndex = 42;
            this.bunifuSeparator5.Transparency = 255;
            this.bunifuSeparator5.Vertical = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(510, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 16);
            this.label1.TabIndex = 41;
            this.label1.Text = "Tipo";
            // 
            // tipoObject
            // 
            this.tipoObject.BackColor = System.Drawing.Color.Transparent;
            this.tipoObject.BorderRadius = 1;
            this.tipoObject.Color = System.Drawing.Color.Transparent;
            this.tipoObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tipoObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.tipoObject.DisabledColor = System.Drawing.Color.Gray;
            this.tipoObject.DisplayMember = "Name";
            this.tipoObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.tipoObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.tipoObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tipoObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.tipoObject.FillDropDown = false;
            this.tipoObject.FillIndicator = false;
            this.tipoObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tipoObject.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tipoObject.ForeColor = System.Drawing.Color.White;
            this.tipoObject.FormattingEnabled = true;
            this.tipoObject.Icon = null;
            this.tipoObject.IndicatorColor = System.Drawing.Color.White;
            this.tipoObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.tipoObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.tipoObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.tipoObject.ItemForeColor = System.Drawing.Color.White;
            this.tipoObject.ItemHeight = 26;
            this.tipoObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.tipoObject.Location = new System.Drawing.Point(512, 78);
            this.tipoObject.Margin = new System.Windows.Forms.Padding(2);
            this.tipoObject.Name = "tipoObject";
            this.tipoObject.Size = new System.Drawing.Size(218, 32);
            this.tipoObject.TabIndex = 40;
            this.tipoObject.Text = null;
            this.tipoObject.ValueMember = "IsPrivate";
            // 
            // elementosSeleccionados
            // 
            this.elementosSeleccionados.AutoSize = true;
            this.elementosSeleccionados.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.elementosSeleccionados.ForeColor = System.Drawing.Color.Silver;
            this.elementosSeleccionados.Location = new System.Drawing.Point(582, 270);
            this.elementosSeleccionados.Name = "elementosSeleccionados";
            this.elementosSeleccionados.Size = new System.Drawing.Size(135, 13);
            this.elementosSeleccionados.TabIndex = 28;
            this.elementosSeleccionados.Text = "Elementos seleccionados";
            // 
            // bunifuDataObject
            // 
            this.bunifuDataObject.AllowCustomTheming = false;
            this.bunifuDataObject.AllowDrop = true;
            this.bunifuDataObject.AllowUserToAddRows = false;
            this.bunifuDataObject.AllowUserToResizeColumns = false;
            this.bunifuDataObject.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(48)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.bunifuDataObject.AutoGenerateColumns = false;
            this.bunifuDataObject.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.bunifuDataObject.BackgroundColor = System.Drawing.Color.Black;
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
            this.idDataGridViewTextBoxColumn,
            this.objectTitleDataGridViewTextBoxColumn,
            this.typeDataGridViewTextBoxColumn,
            this.nameObjectDataGridViewTextBoxColumn,
            this.StreamColumn,
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
            this.bunifuDataObject.DataSource = this.dataViewGroupBindingSource;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(117)))), ((int)(((byte)(119)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.bunifuDataObject.DefaultCellStyle = dataGridViewCellStyle3;
            this.bunifuDataObject.EnableHeadersVisualStyles = false;
            this.bunifuDataObject.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(56)))), ((int)(((byte)(62)))));
            this.bunifuDataObject.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            //this.bunifuDataObject.HeaderBgColor = System.Drawing.Color.Empty;
            this.bunifuDataObject.HeaderForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.Location = new System.Drawing.Point(4, 286);
            this.bunifuDataObject.Name = "bunifuDataObject";
            this.bunifuDataObject.RowHeadersVisible = false;
            this.bunifuDataObject.RowHeadersWidth = 51;
            this.bunifuDataObject.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.bunifuDataObject.RowTemplate.Height = 40;
            this.bunifuDataObject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.bunifuDataObject.ShowCellErrors = false;
            this.bunifuDataObject.ShowCellToolTips = false;
            this.bunifuDataObject.ShowEditingIcon = false;
            this.bunifuDataObject.ShowRowErrors = false;
            this.bunifuDataObject.Size = new System.Drawing.Size(720, 170);
            this.bunifuDataObject.TabIndex = 27;
            this.bunifuDataObject.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.Dark;
            this.bunifuDataObject.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BunifuDataObject_CellMouseDown);
            this.bunifuDataObject.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.BunifuDataObject_CellMouseEnter);
            this.bunifuDataObject.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.BunifuDataObject_CellMouseLeave);
            this.bunifuDataObject.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.BunifuDataObject_CellPainting);
            this.bunifuDataObject.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BunifuDataObject_ColumnHeaderMouseClick);
            this.bunifuDataObject.DragDrop += new System.Windows.Forms.DragEventHandler(this.BunifuDataObject_DragDrop);
            this.bunifuDataObject.DragOver += new System.Windows.Forms.DragEventHandler(this.BunifuDataObject_DragOver);
            this.bunifuDataObject.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BunifuDataObject_MouseDown);
            this.bunifuDataObject.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BunifuDataObject_MouseMove);
            // 
            // idDataGridViewTextBoxColumn
            // 
            this.idDataGridViewTextBoxColumn.DataPropertyName = "Id";
            this.idDataGridViewTextBoxColumn.HeaderText = "Id";
            this.idDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.idDataGridViewTextBoxColumn.Name = "idDataGridViewTextBoxColumn";
            this.idDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.idDataGridViewTextBoxColumn.Visible = false;
            // 
            // objectTitleDataGridViewTextBoxColumn
            // 
            this.objectTitleDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.objectTitleDataGridViewTextBoxColumn.DataPropertyName = "objectTitle";
            this.objectTitleDataGridViewTextBoxColumn.FillWeight = 113.9843F;
            this.objectTitleDataGridViewTextBoxColumn.HeaderText = "Title";
            this.objectTitleDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.objectTitleDataGridViewTextBoxColumn.Name = "objectTitleDataGridViewTextBoxColumn";
            this.objectTitleDataGridViewTextBoxColumn.ReadOnly = true;
            this.objectTitleDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // typeDataGridViewTextBoxColumn
            // 
            this.typeDataGridViewTextBoxColumn.DataPropertyName = "type";
            this.typeDataGridViewTextBoxColumn.FillWeight = 113.9843F;
            this.typeDataGridViewTextBoxColumn.HeaderText = "type";
            this.typeDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.typeDataGridViewTextBoxColumn.Name = "typeDataGridViewTextBoxColumn";
            this.typeDataGridViewTextBoxColumn.ReadOnly = true;
            this.typeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // nameObjectDataGridViewTextBoxColumn
            // 
            this.nameObjectDataGridViewTextBoxColumn.DataPropertyName = "nameObject";
            this.nameObjectDataGridViewTextBoxColumn.FillWeight = 113.9843F;
            this.nameObjectDataGridViewTextBoxColumn.HeaderText = "nameObject";
            this.nameObjectDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.nameObjectDataGridViewTextBoxColumn.Name = "nameObjectDataGridViewTextBoxColumn";
            this.nameObjectDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameObjectDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // StreamColumn
            // 
            this.StreamColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.StreamColumn.DataPropertyName = "StreamProfile";
            this.StreamColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.StreamColumn.FillWeight = 150F;
            this.StreamColumn.HeaderText = "Stream";
            this.StreamColumn.MinimumWidth = 6;
            this.StreamColumn.Name = "StreamColumn";
            this.StreamColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.StreamColumn.Width = 100;
            // 
            // deleteDataGridViewCheckBoxColumn
            // 
            this.deleteDataGridViewCheckBoxColumn.DataPropertyName = "delete";
            this.deleteDataGridViewCheckBoxColumn.FillWeight = 81.9049F;
            this.deleteDataGridViewCheckBoxColumn.HeaderText = "delete";
            this.deleteDataGridViewCheckBoxColumn.MinimumWidth = 6;
            this.deleteDataGridViewCheckBoxColumn.Name = "deleteDataGridViewCheckBoxColumn";
            // 
            // order
            // 
            this.order.FillWeight = 76.14214F;
            this.order.HeaderText = "order";
            this.order.Image = ((System.Drawing.Image)(resources.GetObject("order.Image")));
            this.order.MinimumWidth = 6;
            this.order.Name = "order";
            // 
            // dataViewGroupBindingSource
            // 
            this.dataViewGroupBindingSource.DataSource = typeof(Elipgo.SmartClient.Common.DTOs.DataViewGroup);
            // 
            // bunifuSeparator4
            // 
            this.bunifuSeparator4.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator4.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator4.LineThickness = 1;
            this.bunifuSeparator4.Location = new System.Drawing.Point(264, 104);
            this.bunifuSeparator4.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator4.Name = "bunifuSeparator4";
            this.bunifuSeparator4.Size = new System.Drawing.Size(205, 10);
            this.bunifuSeparator4.TabIndex = 26;
            this.bunifuSeparator4.Transparency = 255;
            this.bunifuSeparator4.Vertical = false;
            // 
            // bunifuSeparator3
            // 
            this.bunifuSeparator3.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator3.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator3.LineThickness = 1;
            this.bunifuSeparator3.Location = new System.Drawing.Point(444, 244);
            this.bunifuSeparator3.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator3.Name = "bunifuSeparator3";
            this.bunifuSeparator3.Size = new System.Drawing.Size(137, 10);
            this.bunifuSeparator3.TabIndex = 25;
            this.bunifuSeparator3.Transparency = 255;
            this.bunifuSeparator3.Vertical = false;
            // 
            // bunifuSeparator2
            // 
            this.bunifuSeparator2.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator2.LineThickness = 1;
            this.bunifuSeparator2.Location = new System.Drawing.Point(292, 244);
            this.bunifuSeparator2.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator2.Name = "bunifuSeparator2";
            this.bunifuSeparator2.Size = new System.Drawing.Size(132, 10);
            this.bunifuSeparator2.TabIndex = 25;
            this.bunifuSeparator2.Transparency = 255;
            this.bunifuSeparator2.Vertical = false;
            // 
            // bunifuSeparatorType
            // 
            this.bunifuSeparatorType.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparatorType.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparatorType.LineThickness = 1;
            this.bunifuSeparatorType.Location = new System.Drawing.Point(151, 244);
            this.bunifuSeparatorType.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparatorType.Name = "bunifuSeparatorType";
            this.bunifuSeparatorType.Size = new System.Drawing.Size(117, 10);
            this.bunifuSeparatorType.TabIndex = 24;
            this.bunifuSeparatorType.Transparency = 255;
            this.bunifuSeparatorType.Vertical = false;
            // 
            // bunifuSeparator1
            // 
            this.bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator1.LineThickness = 1;
            this.bunifuSeparator1.Location = new System.Drawing.Point(7, 244);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Size = new System.Drawing.Size(117, 10);
            this.bunifuSeparator1.TabIndex = 20;
            this.bunifuSeparator1.Transparency = 255;
            this.bunifuSeparator1.Vertical = false;
            // 
            // labelNameObject
            // 
            this.labelNameObject.AutoSize = true;
            this.labelNameObject.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelNameObject.ForeColor = System.Drawing.Color.Silver;
            this.labelNameObject.Location = new System.Drawing.Point(449, 202);
            this.labelNameObject.Name = "labelNameObject";
            this.labelNameObject.Size = new System.Drawing.Size(88, 12);
            this.labelNameObject.TabIndex = 17;
            this.labelNameObject.Text = "Nombre del objeto";
            // 
            // labelTypeObject
            // 
            this.labelTypeObject.AutoSize = true;
            this.labelTypeObject.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelTypeObject.ForeColor = System.Drawing.Color.Silver;
            this.labelTypeObject.Location = new System.Drawing.Point(149, 202);
            this.labelTypeObject.Name = "labelTypeObject";
            this.labelTypeObject.Size = new System.Drawing.Size(69, 12);
            this.labelTypeObject.TabIndex = 16;
            this.labelTypeObject.Text = "Tipo de objeto";
            // 
            // labelSitesGroup
            // 
            this.labelSitesGroup.AutoSize = true;
            this.labelSitesGroup.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelSitesGroup.ForeColor = System.Drawing.Color.Silver;
            this.labelSitesGroup.Location = new System.Drawing.Point(290, 202);
            this.labelSitesGroup.Name = "labelSitesGroup";
            this.labelSitesGroup.Size = new System.Drawing.Size(27, 12);
            this.labelSitesGroup.TabIndex = 32;
            this.labelSitesGroup.Text = "Sitios";
            // 
            // labelObject
            // 
            this.labelObject.AutoSize = true;
            this.labelObject.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelObject.ForeColor = System.Drawing.Color.Silver;
            this.labelObject.Location = new System.Drawing.Point(6, 197);
            this.labelObject.Name = "labelObject";
            this.labelObject.Size = new System.Drawing.Size(35, 12);
            this.labelObject.TabIndex = 15;
            this.labelObject.Text = "Objeto";
            // 
            // tilteForm
            // 
            this.tilteForm.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tilteForm.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.tilteForm.ForeColor = System.Drawing.Color.Silver;
            this.tilteForm.Location = new System.Drawing.Point(15, 11);
            this.tilteForm.Name = "tilteForm";
            this.tilteForm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tilteForm.Size = new System.Drawing.Size(123, 27);
            this.tilteForm.TabIndex = 13;
            this.tilteForm.Text = "Nuevo grupo";
            this.tilteForm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
            // AJUSTE: MOVEMOS EL BOTON A LA DERECHA (X=845) PARA QUE NO SE ENCIME CON EL NUEVO CONTROL
            this.ButtonAdd.Location = new System.Drawing.Point(796, 214);
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
            // ucOptionObjectName (NUEVO CONTROL)
            // 
            this.ucOptionObjectName.BackColor = System.Drawing.Color.Transparent;
            this.ucOptionObjectName.ForeColor = System.Drawing.Color.White;
            this.ucOptionObjectName.Location = new System.Drawing.Point(548, 216);
            this.ucOptionObjectName.Name = "ucOptionObjectName";
            this.ucOptionObjectName.Size = new System.Drawing.Size(220, 32);
            this.ucOptionObjectName.TabIndex = 11;
            // ASIGNAMOS EL EVENTO ORIGINAL A LA PROPIEDAD DEL NUEVO CONTROL
            this.ucOptionObjectName.SelectedIndexChanged += new System.EventHandler(this.OptionObjectName_SelectedIndexChanged);

            // 
            // optionObjectName (CÓDIGO VIEJO COMENTADO)
            // 
            /*
            this.optionObjectName.BackColor = System.Drawing.Color.Transparent;
            this.optionObjectName.BorderRadius = 1;
            this.optionObjectName.Color = System.Drawing.Color.Transparent;
            this.optionObjectName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.optionObjectName.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.optionObjectName.DisabledColor = System.Drawing.Color.Gray;
            this.optionObjectName.DisplayMember = "Name";
            this.optionObjectName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.optionObjectName.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.optionObjectName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.optionObjectName.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.optionObjectName.FillDropDown = false;
            this.optionObjectName.FillIndicator = false;
            this.optionObjectName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optionObjectName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionObjectName.ForeColor = System.Drawing.Color.White;
            this.optionObjectName.FormattingEnabled = true;
            this.optionObjectName.Icon = null;
            this.optionObjectName.IndicatorColor = System.Drawing.Color.White;
            this.optionObjectName.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.optionObjectName.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionObjectName.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionObjectName.ItemForeColor = System.Drawing.Color.White;
            this.optionObjectName.ItemHeight = 26;
            this.optionObjectName.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.optionObjectName.Location = new System.Drawing.Point(440, 216);
            this.optionObjectName.Margin = new System.Windows.Forms.Padding(2);
            this.optionObjectName.Name = "optionObjectName";
            this.optionObjectName.Size = new System.Drawing.Size(154, 32);
            this.optionObjectName.TabIndex = 11;
            this.optionObjectName.Text = null;
            this.optionObjectName.ValueMember = "Key";
            this.optionObjectName.SelectedIndexChanged += new System.EventHandler(this.OptionObjectName_SelectedIndexChanged);
            */

            // 
            // optionTypeObject
            // 
            this.optionTypeObject.BackColor = System.Drawing.Color.Transparent;
            this.optionTypeObject.BorderRadius = 1;
            this.optionTypeObject.Color = System.Drawing.Color.Transparent;
            this.optionTypeObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.optionTypeObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.optionTypeObject.DisabledColor = System.Drawing.Color.Gray;
            this.optionTypeObject.DisplayMember = "Name";
            this.optionTypeObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.optionTypeObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.optionTypeObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.optionTypeObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.optionTypeObject.FillDropDown = false;
            this.optionTypeObject.FillIndicator = false;
            this.optionTypeObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optionTypeObject.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionTypeObject.ForeColor = System.Drawing.Color.White;
            this.optionTypeObject.FormattingEnabled = true;
            this.optionTypeObject.Icon = null;
            this.optionTypeObject.IndicatorColor = System.Drawing.Color.White;
            this.optionTypeObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.optionTypeObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionTypeObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionTypeObject.ItemForeColor = System.Drawing.Color.White;
            this.optionTypeObject.ItemHeight = 26;
            this.optionTypeObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.optionTypeObject.Location = new System.Drawing.Point(178, 216);
            this.optionTypeObject.Margin = new System.Windows.Forms.Padding(2);
            this.optionTypeObject.Name = "optionTypeObject";
            this.optionTypeObject.Size = new System.Drawing.Size(150, 32);
            this.optionTypeObject.TabIndex = 10;
            this.optionTypeObject.Text = null;
            this.optionTypeObject.ValueMember = "Key";
            this.optionTypeObject.SelectedIndexChanged += new System.EventHandler(this.OptionTypeObject_SelectedIndexChanged);
            // 
            // optionObject
            // 
            this.optionObject.BackColor = System.Drawing.Color.Transparent;
            this.optionObject.BorderRadius = 1;
            this.optionObject.Color = System.Drawing.Color.Transparent;
            this.optionObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.optionObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.optionObject.DisabledColor = System.Drawing.Color.Gray;
            this.optionObject.DisplayMember = "Name";
            this.optionObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.optionObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.optionObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.optionObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.optionObject.FillDropDown = false;
            this.optionObject.FillIndicator = false;
            this.optionObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optionObject.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionObject.ForeColor = System.Drawing.Color.White;
            this.optionObject.FormattingEnabled = true;
            this.optionObject.Icon = null;
            this.optionObject.IndicatorColor = System.Drawing.Color.White;
            this.optionObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.optionObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionObject.ItemForeColor = System.Drawing.Color.White;
            this.optionObject.ItemHeight = 26;
            this.optionObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.optionObject.Location = new System.Drawing.Point(4, 216);
            this.optionObject.Margin = new System.Windows.Forms.Padding(2);
            this.optionObject.Name = "optionObject";
            this.optionObject.Size = new System.Drawing.Size(164, 32);
            this.optionObject.TabIndex = 9;
            this.optionObject.Text = null;
            this.optionObject.ValueMember = "Key";
            this.optionObject.SelectedIndexChanged += new System.EventHandler(this.optionObject_SelectedIndexChanged);
            // 
            // optionSitesName
            // 
            // 
            // ucOptionSitesName (NUEVO CONTROL)
            // 
            this.ucOptionSitesName.BackColor = System.Drawing.Color.Transparent;
            this.ucOptionSitesName.ForeColor = System.Drawing.Color.White;
            this.ucOptionSitesName.Location = new System.Drawing.Point(340, 216);
            this.ucOptionSitesName.Name = "ucOptionObjectName";
            this.ucOptionSitesName.Size = new System.Drawing.Size(195, 32);
            this.ucOptionSitesName.TabIndex = 39;
            // ASIGNAMOS EL EVENTO ORIGINAL A LA PROPIEDAD DEL NUEVO CONTROL
            this.ucOptionSitesName.SelectedIndexChanged += new System.EventHandler(this.optionSitesGroup_SelectedIndexChanged);
            //this.optionSitesName.BackColor = System.Drawing.Color.Transparent;
            //this.optionSitesName.BorderRadius = 1;
            //this.optionSitesName.Color = System.Drawing.Color.Transparent;
            //this.optionSitesName.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.optionSitesName.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            //this.optionSitesName.DisabledColor = System.Drawing.Color.Gray;
            //this.optionSitesName.DisplayMember = "Name";
            //this.optionSitesName.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            //this.optionSitesName.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            //this.optionSitesName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.optionSitesName.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            //this.optionSitesName.FillDropDown = false;
            //this.optionSitesName.FillIndicator = false;
            //this.optionSitesName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.optionSitesName.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //this.optionSitesName.ForeColor = System.Drawing.Color.White;
            //this.optionSitesName.FormattingEnabled = true;
            //this.optionSitesName.Icon = null;
            //this.optionSitesName.IndicatorColor = System.Drawing.Color.White;
            //this.optionSitesName.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            //this.optionSitesName.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.optionSitesName.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.optionSitesName.ItemForeColor = System.Drawing.Color.White;
            //this.optionSitesName.ItemHeight = 26;
            //this.optionSitesName.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            //this.optionSitesName.Location = new System.Drawing.Point(340, 216);
            //this.optionSitesName.Margin = new System.Windows.Forms.Padding(2);
            //this.optionSitesName.Name = "optionSitesName";
            //this.optionSitesName.Size = new System.Drawing.Size(195, 32);
            //this.optionSitesName.TabIndex = 39;
            //this.optionSitesName.Text = null;
            //this.optionSitesName.ValueMember = "Key";
            //this.optionSitesName.SelectedIndexChanged += new System.EventHandler(this.optionSitesGroup_SelectedIndexChanged);
            // 
            // elementsAvailable
            // 
            this.elementsAvailable.AutoSize = true;
            this.elementsAvailable.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.elementsAvailable.ForeColor = System.Drawing.Color.Silver;
            this.elementsAvailable.Location = new System.Drawing.Point(4, 152);
            this.elementsAvailable.Name = "elementsAvailable";
            this.elementsAvailable.Size = new System.Drawing.Size(165, 20);
            this.elementsAvailable.TabIndex = 8;
            this.elementsAvailable.Text = "Elementos disponibles";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelName.ForeColor = System.Drawing.Color.Silver;
            this.labelName.Location = new System.Drawing.Point(5, 58);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 12);
            this.labelName.TabIndex = 7;
            this.labelName.Text = Resources.Name;// "Nombre";
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
            this.txtName.IconPadding = 10;
            this.txtName.IconRight = null;
            this.txtName.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.Lines = new string[0];
            this.txtName.Location = new System.Drawing.Point(8, 90);
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
            this.txtName.Size = new System.Drawing.Size(218, 20);
            this.txtName.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Material;
            this.txtName.TabIndex = 6;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtName.TextMarginBottom = 0;
            this.txtName.TextMarginLeft = 5;
            this.txtName.TextMarginTop = 0;
            this.txtName.TextPlaceholder = "";
            this.txtName.UseSystemPasswordChar = false;
            this.txtName.WordWrap = true;
            this.txtName.Validating += new System.ComponentModel.CancelEventHandler(this.TxtName_Validating);
            // 
            // labelTypeGrid
            // 
            this.labelTypeGrid.AutoSize = true;
            this.labelTypeGrid.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelTypeGrid.ForeColor = System.Drawing.Color.Silver;
            this.labelTypeGrid.Location = new System.Drawing.Point(262, 58);
            this.labelTypeGrid.Name = "labelTypeGrid";
            this.labelTypeGrid.Size = new System.Drawing.Size(62, 12);
            this.labelTypeGrid.TabIndex = 5;
            this.labelTypeGrid.Text = "Tipo de Grilla";
            // 
            // optionGrid
            // 
            this.optionGrid.BackColor = System.Drawing.Color.Transparent;
            this.optionGrid.BorderRadius = 1;
            this.optionGrid.Color = System.Drawing.Color.Transparent;
            this.optionGrid.Cursor = System.Windows.Forms.Cursors.Hand;
            this.optionGrid.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceGroup, "GridId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.optionGrid.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.optionGrid.DisabledColor = System.Drawing.Color.Gray;
            this.optionGrid.DisplayMember = "Name";
            this.optionGrid.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.optionGrid.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.optionGrid.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.optionGrid.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.optionGrid.FillDropDown = false;
            this.optionGrid.FillIndicator = false;
            this.optionGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.optionGrid.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.optionGrid.ForeColor = System.Drawing.Color.White;
            this.optionGrid.FormattingEnabled = true;
            this.optionGrid.Icon = null;
            this.optionGrid.IndicatorColor = System.Drawing.Color.White;
            this.optionGrid.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.optionGrid.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionGrid.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.optionGrid.ItemForeColor = System.Drawing.Color.White;
            this.optionGrid.ItemHeight = 26;
            this.optionGrid.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.optionGrid.Location = new System.Drawing.Point(264, 78);
            this.optionGrid.Margin = new System.Windows.Forms.Padding(2);
            this.optionGrid.Name = "optionGrid";
            this.optionGrid.Size = new System.Drawing.Size(218, 32);
            this.optionGrid.TabIndex = 3;
            this.optionGrid.Text = null;
            this.optionGrid.ValueMember = "Key";
            this.optionGrid.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.optionGrid_DrawItem);
            this.optionGrid.SelectedValueChanged += new System.EventHandler(this.optionGrid_SelectedValueChanged);
            this.optionGrid.Validating += new System.ComponentModel.CancelEventHandler(this.OptionGrid_Validating);
            // 
            // bindingSourceGroup
            // 
            this.bindingSourceGroup.DataSource = typeof(Elipgo.SmartClient.Common.DTOs.ObjectGroupEntity);
            // 
            // errorManager
            // 
            this.errorManager.ContainerControl = this;
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.HeaderText = "order";
            this.dataGridViewImageColumn1.Image = ((System.Drawing.Image)(resources.GetObject("dataGridViewImageColumn1.Image")));
            this.dataGridViewImageColumn1.MinimumWidth = 6;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.Width = 720;
            // 
            // FormGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Name = "FormGroups";
            this.Size = new System.Drawing.Size(745, 542);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuDataObject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataViewGroupBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private Bunifu.UI.WinForms.BunifuDropdown optionGrid;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txtName;
        private System.Windows.Forms.Label labelTypeGrid;
        private System.Windows.Forms.Label labelName;
        //private Bunifu.UI.WinForms.BunifuDropdown optionObjectName;
        private Bunifu.UI.WinForms.BunifuDropdown optionTypeObject;
        // NUEVA VARIABLE AGREGADA
        private Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown ucOptionObjectName;
        private Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown ucOptionSitesName;
        private Bunifu.UI.WinForms.BunifuDropdown optionObject;
        //private Bunifu.UI.WinForms.BunifuDropdown optionSitesName;
        private System.Windows.Forms.Label elementsAvailable;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonAdd;
        private Bunifu.Framework.UI.BunifuCustomLabel tilteForm;
        private System.Windows.Forms.Label labelNameObject;
        private System.Windows.Forms.Label labelTypeObject;
        private System.Windows.Forms.Label labelSitesGroup;
        private System.Windows.Forms.Label labelObject;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator1;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator4;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator3;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator2;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparatorType;
        private Bunifu.UI.WinForms.BunifuDataGridView bunifuDataObject;
        private System.Windows.Forms.BindingSource dataViewGroupBindingSource;
        private System.Windows.Forms.ErrorProvider errorManager;
        private System.Windows.Forms.BindingSource bindingSourceGroup;
        private System.Windows.Forms.Label elementosSeleccionados;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator5;
        private Label label1;
        private Bunifu.UI.WinForms.BunifuDropdown tipoObject;
        private DataGridViewImageColumn dataGridViewImageColumn1;
        private Panel panelPag;
        private DataGridViewTextBoxColumn idDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn objectTitleDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn typeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn nameObjectDataGridViewTextBoxColumn;
        private DataGridViewComboBoxColumn StreamColumn;
        private DataGridViewCheckBoxColumn deleteDataGridViewCheckBoxColumn;
        private DataGridViewImageColumn order;
    }
}
