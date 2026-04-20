using System.Windows.Forms;
using Elipgo.SmartClient.Common.Properties;
namespace Elipgo.SmartClient.UserControls.Carrusel
{
    partial class CarouselControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CarouselControl));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges2 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties5 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties6 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties7 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties8 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bunifuSeparator4 = new Bunifu.Framework.UI.BunifuSeparator();
            this.bunifuSeparator1 = new Bunifu.Framework.UI.BunifuSeparator();
            this.bunifuSeparator3 = new Bunifu.Framework.UI.BunifuSeparator();
            this.labelGroup = new System.Windows.Forms.Label();
            this.groupObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.labelObject = new System.Windows.Forms.Label();
            this.optionObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.panelPag = new System.Windows.Forms.Panel();
            this.lblWarninMessage = new Elipgo.SmartClient.UserControls.Alarm.LabelRoundCorners();
            this.elementosSeleccionados = new System.Windows.Forms.Label();
            this.bunifuSeparator2 = new Bunifu.Framework.UI.BunifuSeparator();
            this.tipoObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.bunifuDataObject = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Device = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.StreamColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.deleteDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.order = new System.Windows.Forms.DataGridViewImageColumn();
            this.labelCamera = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.tilteForm = new Bunifu.Framework.UI.BunifuCustomLabel();
            this.ButtonAdd = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.ucCamerasObject = new Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown();
            this.ucLocationObject = new Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown();
            this.elementsAvailable = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.txtName = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.labelTipo = new System.Windows.Forms.Label();
            this.errorManager = new System.Windows.Forms.ErrorProvider(this.components);
            this.bindingSourceCarousel = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuDataObject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCarousel)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bunifuSeparator4);
            this.panel1.Controls.Add(this.bunifuSeparator1);
            this.panel1.Controls.Add(this.bunifuSeparator3);
            this.panel1.Controls.Add(this.labelGroup);
            this.panel1.Controls.Add(this.groupObject);
            this.panel1.Controls.Add(this.labelObject);
            this.panel1.Controls.Add(this.optionObject);
            this.panel1.Controls.Add(this.panelPag);
            this.panel1.Controls.Add(this.lblWarninMessage);
            this.panel1.Controls.Add(this.elementosSeleccionados);
            this.panel1.Controls.Add(this.bunifuSeparator2);
            this.panel1.Controls.Add(this.tipoObject);
            this.panel1.Controls.Add(this.bunifuDataObject);
            this.panel1.Controls.Add(this.labelCamera);
            this.panel1.Controls.Add(this.labelLocation);
            this.panel1.Controls.Add(this.tilteForm);
            this.panel1.Controls.Add(this.ButtonAdd);
            this.panel1.Controls.Add(this.ucCamerasObject);
            this.panel1.Controls.Add(this.ucLocationObject);
            this.panel1.Controls.Add(this.elementsAvailable);
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.labelTipo);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(4, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(744, 535);
            this.panel1.TabIndex = 3;
            // 
            // bunifuSeparator4
            // 
            this.bunifuSeparator4.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator4.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator4.LineThickness = 1;
            this.bunifuSeparator4.Location = new System.Drawing.Point(13, 241);
            this.bunifuSeparator4.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator4.Name = "bunifuSeparator4";
            this.bunifuSeparator4.Size = new System.Drawing.Size(117, 8);
            this.bunifuSeparator4.TabIndex = 35;
            this.bunifuSeparator4.Transparency = 255;
            this.bunifuSeparator4.Vertical = false;
            // 
            // bunifuSeparator1
            // 
            this.bunifuSeparator1.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator1.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator1.LineThickness = 1;
            this.bunifuSeparator1.Location = new System.Drawing.Point(157, 241);
            this.bunifuSeparator1.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator1.Name = "bunifuSeparator1";
            this.bunifuSeparator1.Size = new System.Drawing.Size(199, 8);
            this.bunifuSeparator1.TabIndex = 20;
            this.bunifuSeparator1.Transparency = 255;
            this.bunifuSeparator1.Vertical = false;
            // 
            // bunifuSeparator3
            // 
            this.bunifuSeparator3.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator3.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator3.LineThickness = 1;
            this.bunifuSeparator3.Location = new System.Drawing.Point(385, 241);
            this.bunifuSeparator3.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator3.Name = "bunifuSeparator3";
            this.bunifuSeparator3.Size = new System.Drawing.Size(189, 8);
            this.bunifuSeparator3.TabIndex = 25;
            this.bunifuSeparator3.Transparency = 255;
            this.bunifuSeparator3.Vertical = false;
            // 
            // labelGroup
            // 
            this.labelGroup.AutoSize = true;
            this.labelGroup.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelGroup.ForeColor = System.Drawing.Color.Silver;
            this.labelGroup.Location = new System.Drawing.Point(159, 133);
            this.labelGroup.Name = "labelGroup";
            this.labelGroup.Size = new System.Drawing.Size(33, 12);
            this.labelGroup.TabIndex = 37;
            this.labelGroup.Text = "Grupo";
            this.labelGroup.Visible = false;
            // 
            // groupObject
            // 
            this.groupObject.BackColor = System.Drawing.Color.Transparent;
            this.groupObject.BorderRadius = 1;
            this.groupObject.Color = System.Drawing.Color.Transparent;
            this.groupObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.groupObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.groupObject.DisabledColor = System.Drawing.Color.Gray;
            this.groupObject.DisplayMember = "Name";
            this.groupObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.groupObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.groupObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.groupObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.groupObject.FillDropDown = false;
            this.groupObject.FillIndicator = false;
            this.groupObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupObject.ForeColor = System.Drawing.Color.White;
            this.groupObject.FormattingEnabled = true;
            this.groupObject.Icon = null;
            this.groupObject.IndicatorColor = System.Drawing.Color.White;
            this.groupObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.groupObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.groupObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.groupObject.ItemForeColor = System.Drawing.Color.White;
            this.groupObject.ItemHeight = 26;
            this.groupObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.groupObject.Location = new System.Drawing.Point(161, 152);
            this.groupObject.Margin = new System.Windows.Forms.Padding(2);
            this.groupObject.Name = "groupObject";
            this.groupObject.Size = new System.Drawing.Size(214, 32);
            this.groupObject.TabIndex = 36;
            this.groupObject.Text = null;
            this.groupObject.ValueMember = "Key";
            this.groupObject.Visible = false;
            // 
            // labelObject
            // 
            this.labelObject.AutoSize = true;
            this.labelObject.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelObject.ForeColor = System.Drawing.Color.Silver;
            this.labelObject.Location = new System.Drawing.Point(11, 197);
            this.labelObject.Name = "labelObject";
            this.labelObject.Size = new System.Drawing.Size(35, 12);
            this.labelObject.TabIndex = 34;
            this.labelObject.Text = "Objeto";
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
            this.optionObject.Location = new System.Drawing.Point(9, 216);
            this.optionObject.Margin = new System.Windows.Forms.Padding(2);
            this.optionObject.Name = "optionObject";
            this.optionObject.Size = new System.Drawing.Size(134, 32);
            this.optionObject.TabIndex = 33;
            this.optionObject.Text = null;
            this.optionObject.ValueMember = "Key";
            this.optionObject.SelectedValueChanged += new System.EventHandler(this.optionObject_SelectedValueChanged);
            // 
            // panelPag
            // 
            this.panelPag.Location = new System.Drawing.Point(246, 463);
            this.panelPag.Name = "panelPag";
            this.panelPag.Size = new System.Drawing.Size(199, 26);
            this.panelPag.TabIndex = 32;
            // 
            // lblWarninMessage
            // 
            this.lblWarninMessage._BackColor = System.Drawing.Color.Empty;
            this.lblWarninMessage.AutoSize = true;
            this.lblWarninMessage.Location = new System.Drawing.Point(6, 266);
            this.lblWarninMessage.Name = "lblWarninMessage";
            this.lblWarninMessage.Size = new System.Drawing.Size(340, 13);
            this.lblWarninMessage.TabIndex = 31;
            this.lblWarninMessage.Text = Resources.WarningCarousel;
            // 
            // elementosSeleccionados
            // 
            this.elementosSeleccionados.AutoSize = true;
            this.elementosSeleccionados.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.elementosSeleccionados.ForeColor = System.Drawing.Color.Silver;
            this.elementosSeleccionados.Location = new System.Drawing.Point(548, 266);
            this.elementosSeleccionados.Name = "elementosSeleccionados";
            this.elementosSeleccionados.Size = new System.Drawing.Size(135, 13);
            this.elementosSeleccionados.TabIndex = 30;
            this.elementosSeleccionados.Text = "Elementos seleccionados";
            // 
            // bunifuSeparator2
            // 
            this.bunifuSeparator2.BackColor = System.Drawing.Color.Transparent;
            this.bunifuSeparator2.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.bunifuSeparator2.LineThickness = 1;
            this.bunifuSeparator2.Location = new System.Drawing.Point(395, 100);
            this.bunifuSeparator2.Margin = new System.Windows.Forms.Padding(4);
            this.bunifuSeparator2.Name = "bunifuSeparator2";
            this.bunifuSeparator2.Size = new System.Drawing.Size(325, 5);
            this.bunifuSeparator2.TabIndex = 29;
            this.bunifuSeparator2.Transparency = 255;
            this.bunifuSeparator2.Vertical = false;
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
            this.tipoObject.Location = new System.Drawing.Point(398, 74);
            this.tipoObject.Margin = new System.Windows.Forms.Padding(2);
            this.tipoObject.Name = "tipoObject";
            this.tipoObject.Size = new System.Drawing.Size(336, 32);
            this.tipoObject.TabIndex = 28;
            this.tipoObject.Text = null;
            this.tipoObject.ValueMember = "IsPrivate";
            // 
            // bunifuDataObject
            // 
            this.bunifuDataObject.AllowCustomTheming = false;
            this.bunifuDataObject.AllowDrop = true;
            this.bunifuDataObject.AllowUserToAddRows = false;
            this.bunifuDataObject.AllowUserToResizeColumns = false;
            this.bunifuDataObject.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(48)))), ((int)(((byte)(52)))));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.bunifuDataObject.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.bunifuDataObject.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.bunifuDataObject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.bunifuDataObject.CausesValidation = false;
            this.bunifuDataObject.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.bunifuDataObject.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI Semibold", 11.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(63)))), ((int)(((byte)(63)))), ((int)(((byte)(65)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.bunifuDataObject.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.bunifuDataObject.ColumnHeadersHeight = 40;
            this.bunifuDataObject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.Location,
            this.Device,
            this.TimeColumn,
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
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(117)))), ((int)(((byte)(119)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.bunifuDataObject.DefaultCellStyle = dataGridViewCellStyle7;
            this.bunifuDataObject.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.bunifuDataObject.EnableHeadersVisualStyles = false;
            this.bunifuDataObject.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(56)))), ((int)(((byte)(62)))));
            this.bunifuDataObject.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(16)))), ((int)(((byte)(18)))));
            //this.bunifuDataObject.HeaderBgColor = System.Drawing.Color.Empty;
            this.bunifuDataObject.HeaderForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.Location = new System.Drawing.Point(0, 287);
            this.bunifuDataObject.Name = "bunifuDataObject";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.bunifuDataObject.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
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
            this.bunifuDataObject.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.bunifuDataObject_CellMouseLeave);
            this.bunifuDataObject.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.BunifuDataObject_CellPainting);
            this.bunifuDataObject.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BunifuDataObject_ColumnHeaderMouseClick);
            this.bunifuDataObject.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.BunifuDataObject_DataError);
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
            this.Id.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Id.Visible = false;
            // 
            // Location
            // 
            this.Location.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Location.DataPropertyName = "SiteStr";
            this.Location.FillWeight = 300F;
            this.Location.HeaderText = "Ubicacion";
            this.Location.MinimumWidth = 6;
            this.Location.Name = "Location";
            this.Location.ReadOnly = true;
            this.Location.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Device
            // 
            this.Device.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Device.DataPropertyName = "DeviceName";
            this.Device.FillWeight = 300F;
            this.Device.HeaderText = "Device";
            this.Device.MinimumWidth = 6;
            this.Device.Name = "Device";
            this.Device.ReadOnly = true;
            this.Device.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Device.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // TimeColumn
            // 
            this.TimeColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.TimeColumn.DataPropertyName = "Duration";
            this.TimeColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.TimeColumn.FillWeight = 150F;
            this.TimeColumn.HeaderText = "Duración";
            this.TimeColumn.MinimumWidth = 6;
            this.TimeColumn.Name = "TimeColumn";
            this.TimeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.TimeColumn.Width = 90;
            // 
            // StreamColumn
            // 
            this.StreamColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.StreamColumn.DataPropertyName = "StreamProfile";
            this.StreamColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.StreamColumn.FillWeight = 150F;
            this.StreamColumn.HeaderText = "Stream";
            this.StreamColumn.MinimumWidth = 6;
            this.StreamColumn.Name = "StreamColumn";
            this.StreamColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.StreamColumn.Width = 90;
            // 
            // deleteDataGridViewCheckBoxColumn
            // 
            this.deleteDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.deleteDataGridViewCheckBoxColumn.DataPropertyName = "IsDeleted";
            this.deleteDataGridViewCheckBoxColumn.HeaderText = "Delete";
            this.deleteDataGridViewCheckBoxColumn.MinimumWidth = 6;
            this.deleteDataGridViewCheckBoxColumn.Name = "deleteDataGridViewCheckBoxColumn";
            this.deleteDataGridViewCheckBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // order
            // 
            this.order.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.order.HeaderText = "order";
            this.order.Image = ((System.Drawing.Image)(resources.GetObject("order.Image")));
            this.order.MinimumWidth = 6;
            this.order.Name = "order";
            this.order.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // labelCamera
            // 
            this.labelCamera.AutoSize = true;
            this.labelCamera.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelCamera.ForeColor = System.Drawing.Color.Silver;
            this.labelCamera.Location = new System.Drawing.Point(383, 197);
            this.labelCamera.Name = "labelCamera";
            this.labelCamera.Size = new System.Drawing.Size(42, 12);
            this.labelCamera.TabIndex = 17;
            this.labelCamera.Text = "Cámaras";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelLocation.ForeColor = System.Drawing.Color.Silver;
            this.labelLocation.Location = new System.Drawing.Point(155, 197);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(49, 12);
            this.labelLocation.TabIndex = 15;
            this.labelLocation.Text = "Ubicacion";
            // 
            // tilteForm
            // 
            this.tilteForm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tilteForm.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.tilteForm.ForeColor = System.Drawing.Color.Silver;
            this.tilteForm.Location = new System.Drawing.Point(4, 23);
            this.tilteForm.Name = "tilteForm";
            this.tilteForm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tilteForm.Size = new System.Drawing.Size(140, 27);
            this.tilteForm.TabIndex = 13;
            this.tilteForm.Text = Resources.newCarousel;
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
            borderEdges2.BottomLeft = true;
            borderEdges2.BottomRight = true;
            borderEdges2.TopLeft = true;
            borderEdges2.TopRight = true;
            this.ButtonAdd.CustomizableEdges = borderEdges2;
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
            this.ButtonAdd.Location = new System.Drawing.Point(599, 215);
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
            // camerasObject
            // 
            this.ucCamerasObject.BackColor = System.Drawing.Color.Transparent;
            this.ucCamerasObject.ForeColor = System.Drawing.Color.White;
            this.ucCamerasObject.Location = new System.Drawing.Point(385, 216);
            this.ucCamerasObject.Name = "ucOptionObjectName";
            this.ucCamerasObject.Size = new System.Drawing.Size(198, 32);
            this.ucCamerasObject.TabIndex = 11;
            this.ucCamerasObject.SelectedIndexChanged += new System.EventHandler(this.CameraObject_SelectedIndexChanged);
            //this.camerasObject.BackColor = System.Drawing.Color.Transparent;
            //this.camerasObject.BorderRadius = 1;
            //this.camerasObject.Color = System.Drawing.Color.Transparent;
            //this.camerasObject.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.camerasObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            //this.camerasObject.DisabledColor = System.Drawing.Color.Gray;
            //this.camerasObject.DisplayMember = "Name";
            //this.camerasObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            //this.camerasObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            //this.camerasObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.camerasObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            //this.camerasObject.FillDropDown = false;
            //this.camerasObject.FillIndicator = false;
            //this.camerasObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.camerasObject.ForeColor = System.Drawing.Color.White;
            //this.camerasObject.FormattingEnabled = true;
            //this.camerasObject.Icon = null;
            //this.camerasObject.IndicatorColor = System.Drawing.Color.White;
            //this.camerasObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            //this.camerasObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.camerasObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.camerasObject.ItemForeColor = System.Drawing.Color.White;
            //this.camerasObject.ItemHeight = 26;
            //this.camerasObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            //this.camerasObject.Location = new System.Drawing.Point(385, 216);
            //this.camerasObject.Margin = new System.Windows.Forms.Padding(2);
            //this.camerasObject.Name = "camerasObject";
            //this.camerasObject.Size = new System.Drawing.Size(198, 32);
            //this.camerasObject.TabIndex = 11;
            //this.camerasObject.Text = null;
            //this.camerasObject.ValueMember = "Key";
            // 
            // locationObject
            // 
            this.ucLocationObject.BackColor = System.Drawing.Color.Transparent;
            this.ucLocationObject.ForeColor = System.Drawing.Color.White;
            this.ucLocationObject.Location = new System.Drawing.Point(157, 216);
            this.ucLocationObject.Name = "ucOptionObjectName";
            this.ucLocationObject.Size = new System.Drawing.Size(214, 32);
            this.ucLocationObject.TabIndex = 9;
            this.ucLocationObject.SelectedIndexChanged += new System.EventHandler(this.LocationObject_SelectedIndexChanged);
            //this.locationObject.BackColor = System.Drawing.Color.Transparent;
            //this.locationObject.BorderRadius = 1;
            //this.locationObject.Color = System.Drawing.Color.Transparent;
            //this.locationObject.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.locationObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            //this.locationObject.DisabledColor = System.Drawing.Color.Gray;
            //this.locationObject.DisplayMember = "Name";
            //this.locationObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            //this.locationObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            //this.locationObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.locationObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            //this.locationObject.FillDropDown = false;
            //this.locationObject.FillIndicator = false;
            //this.locationObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.locationObject.ForeColor = System.Drawing.Color.White;
            //this.locationObject.FormattingEnabled = true;
            //this.locationObject.Icon = null;
            //this.locationObject.IndicatorColor = System.Drawing.Color.White;
            //this.locationObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            //this.locationObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.locationObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.locationObject.ItemForeColor = System.Drawing.Color.White;
            //this.locationObject.ItemHeight = 26;
            //this.locationObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            //this.locationObject.Location = new System.Drawing.Point(157, 216);
            //this.locationObject.Margin = new System.Windows.Forms.Padding(2);
            //this.locationObject.Name = "locationObject";
            //this.locationObject.Size = new System.Drawing.Size(214, 32);
            //this.locationObject.TabIndex = 9;
            //this.locationObject.Text = null;
            //this.locationObject.ValueMember = "Key";
            //this.locationObject.SelectedIndexChanged += new System.EventHandler(this.LocationObject_SelectedIndexChanged);
            // 
            // elementsAvailable
            // 
            this.elementsAvailable.AutoSize = true;
            this.elementsAvailable.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.elementsAvailable.ForeColor = System.Drawing.Color.Silver;
            this.elementsAvailable.Location = new System.Drawing.Point(4, 152);
            this.elementsAvailable.Name = "elementsAvailable";
            this.elementsAvailable.Size = new System.Drawing.Size(152, 20);
            this.elementsAvailable.TabIndex = 8;
            this.elementsAvailable.Text = "Camaras disponibles";
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
            this.txtName.IconPadding = 0;
            this.txtName.IconRight = null;
            this.txtName.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtName.Lines = new string[0];
            this.txtName.Location = new System.Drawing.Point(7, 75);
            this.txtName.MaxLength = 32767;
            this.txtName.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtName.Modified = false;
            this.txtName.Multiline = false;
            this.txtName.Name = "txtName";
            stateProperties5.BorderColor = System.Drawing.Color.DodgerBlue;
            stateProperties5.FillColor = System.Drawing.Color.Empty;
            stateProperties5.ForeColor = System.Drawing.Color.Empty;
            stateProperties5.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtName.OnActiveState = stateProperties5;
            stateProperties6.BorderColor = System.Drawing.Color.Empty;
            stateProperties6.FillColor = System.Drawing.Color.White;
            stateProperties6.ForeColor = System.Drawing.Color.Empty;
            stateProperties6.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtName.OnDisabledState = stateProperties6;
            stateProperties7.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties7.FillColor = System.Drawing.Color.Empty;
            stateProperties7.ForeColor = System.Drawing.Color.Empty;
            stateProperties7.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtName.OnHoverState = stateProperties7;
            stateProperties8.BorderColor = System.Drawing.Color.DimGray;
            stateProperties8.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            stateProperties8.ForeColor = System.Drawing.Color.White;
            stateProperties8.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtName.OnIdleState = stateProperties8;
            this.txtName.PasswordChar = '\0';
            this.txtName.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.txtName.PlaceholderText = "";
            this.txtName.ReadOnly = false;
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtName.SelectedText = "";
            this.txtName.SelectionLength = 0;
            this.txtName.SelectionStart = 0;
            this.txtName.ShortcutsEnabled = true;
            this.txtName.Size = new System.Drawing.Size(336, 28);
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
            // labelTipo
            // 
            this.labelTipo.AutoSize = true;
            this.labelTipo.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelTipo.ForeColor = System.Drawing.Color.Silver;
            this.labelTipo.Location = new System.Drawing.Point(401, 58);
            this.labelTipo.Name = "labelTipo";
            this.labelTipo.Size = new System.Drawing.Size(24, 12);
            this.labelTipo.TabIndex = 5;
            this.labelTipo.Text = "Tipo";
            // 
            // errorManager
            // 
            this.errorManager.ContainerControl = this;
            // 
            // bindingSourceCarousel
            // 
            this.bindingSourceCarousel.DataSource = typeof(Elipgo.SmartClient.Common.DTOs.ObjectGroupEntity);
            // 
            // CarouselControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CarouselControl";
            this.Size = new System.Drawing.Size(745, 535);
            this.Load += new System.EventHandler(this.CarouselControl_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuDataObject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCarousel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Bunifu.UI.WinForms.BunifuDataGridView bunifuDataObject;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator3;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator1;
        private System.Windows.Forms.Label labelCamera;
        private System.Windows.Forms.Label labelLocation;
        private Bunifu.Framework.UI.BunifuCustomLabel tilteForm;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonAdd;
        //private Bunifu.UI.WinForms.BunifuDropdown camerasObject;
        //private Bunifu.UI.WinForms.BunifuDropdown locationObject;
        private Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown ucCamerasObject;
        private Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown ucLocationObject;
        private System.Windows.Forms.Label elementsAvailable;
        private System.Windows.Forms.Label labelName;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txtName;
        private System.Windows.Forms.Label labelTipo;
        private System.Windows.Forms.BindingSource bindingSourceCarousel;
        private System.Windows.Forms.ErrorProvider errorManager;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator2;
        private Bunifu.UI.WinForms.BunifuDropdown tipoObject;
        private System.Windows.Forms.Label elementosSeleccionados;
        private Alarm.LabelRoundCorners lblWarninMessage;
        private Panel panelPag;
        private DataGridViewTextBoxColumn Id;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        private DataGridViewTextBoxColumn Location;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        private DataGridViewTextBoxColumn Device;
        private DataGridViewComboBoxColumn TimeColumn;
        private DataGridViewComboBoxColumn StreamColumn;
        private DataGridViewCheckBoxColumn deleteDataGridViewCheckBoxColumn;
        private DataGridViewImageColumn order;
        private Bunifu.Framework.UI.BunifuSeparator bunifuSeparator4;
        private Label labelObject;
        private Bunifu.UI.WinForms.BunifuDropdown optionObject;
        private Label labelGroup;
        private Bunifu.UI.WinForms.BunifuDropdown groupObject;
    }
}
