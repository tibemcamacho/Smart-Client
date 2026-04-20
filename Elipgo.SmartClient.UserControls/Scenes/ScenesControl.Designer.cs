using Elipgo.SmartClient.Common.Properties;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Scenes
{
    partial class ScenesControl
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
        [System.Obsolete]
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScenesControl));
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties5 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties6 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties7 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties8 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelAction = new System.Windows.Forms.Label();
            this.separatorDispositivos = new Bunifu.Framework.UI.BunifuSeparator();
            this.panelPagi = new System.Windows.Forms.Panel();
            this.labelListPresetGuardia = new System.Windows.Forms.Label();
            this.separatorListPresetGuarda = new Bunifu.Framework.UI.BunifuSeparator();
            this.presetListGuardiaObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.labelSitio = new System.Windows.Forms.Label();
            this.separatorSitios = new Bunifu.Framework.UI.BunifuSeparator();
            //this.sitioiotObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.ucSitioiotObject = new Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown();
            this.labelPresetGuardia = new System.Windows.Forms.Label();
            this.separatorPresetGuarda = new Bunifu.Framework.UI.BunifuSeparator();
            this.presetGuardiaObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.elementosSeleccionados = new System.Windows.Forms.Label();
            this.txtNote = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.bunifuDataObject = new Bunifu.UI.WinForms.BunifuDataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Device = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PresetGuard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.deleteDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.order = new System.Windows.Forms.DataGridViewImageColumn();
            this.ObjPtz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.separatorActions = new Bunifu.Framework.UI.BunifuSeparator();
            this.labelDevices = new System.Windows.Forms.Label();
            this.ButtonAdd = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();
            this.elementsAvailable = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.txtName = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.labelNote = new System.Windows.Forms.Label();
            this.tilteForm = new Bunifu.Framework.UI.BunifuCustomLabel();
            //this.iotObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.uciotObject = new Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown();
            this.actionsObject = new Bunifu.UI.WinForms.BunifuDropdown();
            this.errorManager = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorManagerPresetGuard = new System.Windows.Forms.ErrorProvider(this.components);
            this.bindingSourceScenes = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuDataObject)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManagerPresetGuard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceScenes)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelAction);
            this.panel1.Controls.Add(this.separatorDispositivos);
            this.panel1.Controls.Add(this.panelPagi);
            this.panel1.Controls.Add(this.labelListPresetGuardia);
            this.panel1.Controls.Add(this.separatorListPresetGuarda);
            this.panel1.Controls.Add(this.presetListGuardiaObject);
            this.panel1.Controls.Add(this.labelSitio);
            this.panel1.Controls.Add(this.separatorSitios);
            this.panel1.Controls.Add(this.ucSitioiotObject);
            this.panel1.Controls.Add(this.labelPresetGuardia);
            this.panel1.Controls.Add(this.separatorPresetGuarda);
            this.panel1.Controls.Add(this.presetGuardiaObject);
            this.panel1.Controls.Add(this.elementosSeleccionados);
            this.panel1.Controls.Add(this.txtNote);
            this.panel1.Controls.Add(this.bunifuDataObject);
            this.panel1.Controls.Add(this.separatorActions);
            this.panel1.Controls.Add(this.labelDevices);
            this.panel1.Controls.Add(this.ButtonAdd);
            this.panel1.Controls.Add(this.elementsAvailable);
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.labelNote);
            this.panel1.Controls.Add(this.tilteForm);
            this.panel1.Controls.Add(this.uciotObject);
            this.panel1.Controls.Add(this.actionsObject);
            this.panel1.ForeColor = System.Drawing.Color.White;
            this.panel1.Location = new System.Drawing.Point(4, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(744, 535);
            this.panel1.TabIndex = 2;
            // 
            // labelAction
            // 
            this.labelAction.AutoSize = true;
            this.labelAction.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelAction.ForeColor = System.Drawing.Color.Silver;
            this.labelAction.Location = new System.Drawing.Point(364, 183);
            this.labelAction.Name = "labelAction";
            this.labelAction.Size = new System.Drawing.Size(66, 12);
            this.labelAction.TabIndex = 17;
            this.labelAction.Text = "Acción Global";
            this.labelAction.Click += new System.EventHandler(this.labelAction_Click);
            // 
            // separatorDispositivos
            // 
            this.separatorDispositivos.BackColor = System.Drawing.Color.Transparent;
            this.separatorDispositivos.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.separatorDispositivos.LineThickness = 1;
            this.separatorDispositivos.Location = new System.Drawing.Point(196, 222);
            this.separatorDispositivos.Margin = new System.Windows.Forms.Padding(4);
            this.separatorDispositivos.Name = "separatorDispositivos";
            this.separatorDispositivos.Size = new System.Drawing.Size(130, 10);
            this.separatorDispositivos.TabIndex = 20;
            this.separatorDispositivos.Transparency = 255;
            this.separatorDispositivos.Vertical = false;
            this.separatorDispositivos.Load += new System.EventHandler(this.separatorDispositivos_Load);
            // 
            // panelPagi
            // 
            this.panelPagi.Location = new System.Drawing.Point(252, 463);
            this.panelPagi.Name = "panelPagi";
            this.panelPagi.Size = new System.Drawing.Size(201, 26);
            this.panelPagi.TabIndex = 44;
            // 
            // labelListPresetGuardia
            // 
            this.labelListPresetGuardia.AutoSize = true;
            this.labelListPresetGuardia.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelListPresetGuardia.ForeColor = System.Drawing.Color.Silver;
            this.labelListPresetGuardia.Location = new System.Drawing.Point(194, 236);
            this.labelListPresetGuardia.Name = "labelListPresetGuardia";
            this.labelListPresetGuardia.Size = new System.Drawing.Size(31, 12);
            this.labelListPresetGuardia.TabIndex = 38;
            this.labelListPresetGuardia.Text = "Preset";
            this.labelListPresetGuardia.Visible = false;
            // 
            // separatorListPresetGuarda
            // 
            this.separatorListPresetGuarda.BackColor = System.Drawing.Color.Transparent;
            this.separatorListPresetGuarda.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.separatorListPresetGuarda.LineThickness = 1;
            this.separatorListPresetGuarda.Location = new System.Drawing.Point(196, 272);
            this.separatorListPresetGuarda.Margin = new System.Windows.Forms.Padding(4);
            this.separatorListPresetGuarda.Name = "separatorListPresetGuarda";
            this.separatorListPresetGuarda.Size = new System.Drawing.Size(130, 10);
            this.separatorListPresetGuarda.TabIndex = 37;
            this.separatorListPresetGuarda.Transparency = 255;
            this.separatorListPresetGuarda.Vertical = false;
            this.separatorListPresetGuarda.Visible = false;
            // 
            // presetListGuardiaObject
            // 
            this.presetListGuardiaObject.BackColor = System.Drawing.Color.Transparent;
            this.presetListGuardiaObject.BorderRadius = 1;
            this.presetListGuardiaObject.Color = System.Drawing.Color.Transparent;
            this.presetListGuardiaObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.presetListGuardiaObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.presetListGuardiaObject.DisabledColor = System.Drawing.Color.Gray;
            this.presetListGuardiaObject.DisplayMember = "Name";
            this.presetListGuardiaObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.presetListGuardiaObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.presetListGuardiaObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presetListGuardiaObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.presetListGuardiaObject.FillDropDown = false;
            this.presetListGuardiaObject.FillIndicator = false;
            this.presetListGuardiaObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.presetListGuardiaObject.ForeColor = System.Drawing.Color.White;
            this.presetListGuardiaObject.FormattingEnabled = true;
            this.presetListGuardiaObject.Icon = null;
            this.presetListGuardiaObject.IndicatorColor = System.Drawing.Color.White;
            this.presetListGuardiaObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.presetListGuardiaObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.presetListGuardiaObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.presetListGuardiaObject.ItemForeColor = System.Drawing.Color.White;
            this.presetListGuardiaObject.ItemHeight = 26;
            this.presetListGuardiaObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.presetListGuardiaObject.Location = new System.Drawing.Point(196, 244);
            this.presetListGuardiaObject.Margin = new System.Windows.Forms.Padding(2);
            this.presetListGuardiaObject.Name = "presetListGuardiaObject";
            this.presetListGuardiaObject.Size = new System.Drawing.Size(144, 32);
            this.presetListGuardiaObject.TabIndex = 36;
            this.presetListGuardiaObject.Text = null;
            this.presetListGuardiaObject.ValueMember = "Id";
            this.presetListGuardiaObject.Visible = false;
            // 
            // labelSitio
            // 
            this.labelSitio.AutoSize = true;
            this.labelSitio.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelSitio.ForeColor = System.Drawing.Color.Silver;
            this.labelSitio.Location = new System.Drawing.Point(11, 187);
            this.labelSitio.Name = "labelSitio";
            this.labelSitio.Size = new System.Drawing.Size(27, 12);
            this.labelSitio.TabIndex = 35;
            this.labelSitio.Text = "Sitios";
            // 
            // separatorSitios
            // 
            this.separatorSitios.BackColor = System.Drawing.Color.Transparent;
            this.separatorSitios.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.separatorSitios.LineThickness = 1;
            this.separatorSitios.Location = new System.Drawing.Point(10, 219);
            this.separatorSitios.Margin = new System.Windows.Forms.Padding(4);
            this.separatorSitios.Name = "separatorSitios";
            this.separatorSitios.Size = new System.Drawing.Size(130, 10);
            this.separatorSitios.TabIndex = 34;
            this.separatorSitios.Transparency = 255;
            this.separatorSitios.Vertical = false;
            // 
            // sitioiotObject
            // 
            this.ucSitioiotObject.BackColor = System.Drawing.Color.Transparent;
            this.ucSitioiotObject.ForeColor = System.Drawing.Color.White;
            this.ucSitioiotObject.Location = new System.Drawing.Point(7, 197);
            this.ucSitioiotObject.Name = "sitioiotObject";
            this.ucSitioiotObject.Size = new System.Drawing.Size(144, 32);
            this.ucSitioiotObject.TabIndex = 33;
            //this.ucSitioiotObject.isSearchingMode = false;
            this.ucSitioiotObject.SelectedIndexChanged += new System.EventHandler(this.sitioiotObject_SelectedValueChanged);
            //this.sitioiotObject.BackColor = System.Drawing.Color.Transparent;
            //this.sitioiotObject.BorderRadius = 1;
            //this.sitioiotObject.Color = System.Drawing.Color.Transparent;
            //this.sitioiotObject.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.sitioiotObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            //this.sitioiotObject.DisabledColor = System.Drawing.Color.Gray;
            //this.sitioiotObject.DisplayMember = "Name";
            //this.sitioiotObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            //this.sitioiotObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            //this.sitioiotObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.sitioiotObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            //this.sitioiotObject.FillDropDown = false;
            //this.sitioiotObject.FillIndicator = false;
            //this.sitioiotObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.sitioiotObject.ForeColor = System.Drawing.Color.White;
            //this.sitioiotObject.FormattingEnabled = true;
            //this.sitioiotObject.Icon = null;
            //this.sitioiotObject.IndicatorColor = System.Drawing.Color.White;
            //this.sitioiotObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            //this.sitioiotObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.sitioiotObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.sitioiotObject.ItemForeColor = System.Drawing.Color.White;
            //this.sitioiotObject.ItemHeight = 26;
            //this.sitioiotObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            //this.sitioiotObject.Location = new System.Drawing.Point(7, 197);
            //this.sitioiotObject.Margin = new System.Windows.Forms.Padding(2);
            //this.sitioiotObject.Name = "sitioiotObject";
            //this.sitioiotObject.Size = new System.Drawing.Size(144, 32);
            //this.sitioiotObject.TabIndex = 33;
            //this.sitioiotObject.Text = null;
            //this.sitioiotObject.ValueMember = "Id";
            //this.sitioiotObject.SelectedValueChanged += new System.EventHandler(this.sitioiotObject_SelectedValueChanged);
            // 
            // labelPresetGuardia
            // 
            this.labelPresetGuardia.AutoSize = true;
            this.labelPresetGuardia.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelPresetGuardia.ForeColor = System.Drawing.Color.Silver;
            this.labelPresetGuardia.Location = new System.Drawing.Point(12, 236);
            this.labelPresetGuardia.Name = "labelPresetGuardia";
            this.labelPresetGuardia.Size = new System.Drawing.Size(75, 12);
            this.labelPresetGuardia.TabIndex = 32;
            this.labelPresetGuardia.Text = "Preset / Guardia";
            this.labelPresetGuardia.Visible = false;
            // 
            // separatorPresetGuarda
            // 
            this.separatorPresetGuarda.BackColor = System.Drawing.Color.Transparent;
            this.separatorPresetGuarda.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.separatorPresetGuarda.LineThickness = 1;
            this.separatorPresetGuarda.Location = new System.Drawing.Point(11, 273);
            this.separatorPresetGuarda.Margin = new System.Windows.Forms.Padding(4);
            this.separatorPresetGuarda.Name = "separatorPresetGuarda";
            this.separatorPresetGuarda.Size = new System.Drawing.Size(130, 10);
            this.separatorPresetGuarda.TabIndex = 31;
            this.separatorPresetGuarda.Transparency = 255;
            this.separatorPresetGuarda.Vertical = false;
            this.separatorPresetGuarda.Visible = false;
            // 
            // presetGuardiaObject
            // 
            this.presetGuardiaObject.BackColor = System.Drawing.Color.Transparent;
            this.presetGuardiaObject.BorderRadius = 1;
            this.presetGuardiaObject.Color = System.Drawing.Color.Transparent;
            this.presetGuardiaObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.presetGuardiaObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.presetGuardiaObject.DisabledColor = System.Drawing.Color.Gray;
            this.presetGuardiaObject.DisplayMember = "Name";
            this.presetGuardiaObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.presetGuardiaObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.presetGuardiaObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.presetGuardiaObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.presetGuardiaObject.FillDropDown = false;
            this.presetGuardiaObject.FillIndicator = false;
            this.presetGuardiaObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.presetGuardiaObject.ForeColor = System.Drawing.Color.White;
            this.presetGuardiaObject.FormattingEnabled = true;
            this.presetGuardiaObject.Icon = null;
            this.presetGuardiaObject.IndicatorColor = System.Drawing.Color.White;
            this.presetGuardiaObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.presetGuardiaObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.presetGuardiaObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.presetGuardiaObject.ItemForeColor = System.Drawing.Color.White;
            this.presetGuardiaObject.ItemHeight = 26;
            this.presetGuardiaObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.presetGuardiaObject.Location = new System.Drawing.Point(7, 250);
            this.presetGuardiaObject.Margin = new System.Windows.Forms.Padding(2);
            this.presetGuardiaObject.Name = "presetGuardiaObject";
            this.presetGuardiaObject.Size = new System.Drawing.Size(144, 32);
            this.presetGuardiaObject.TabIndex = 30;
            this.presetGuardiaObject.Text = null;
            this.presetGuardiaObject.ValueMember = "Key";
            this.presetGuardiaObject.Visible = false;
            this.presetGuardiaObject.SelectedValueChanged += new System.EventHandler(this.presetGuardiaObject_SelectedValueChanged);
            // 
            // elementosSeleccionados
            // 
            this.elementosSeleccionados.AutoSize = true;
            this.elementosSeleccionados.Location = new System.Drawing.Point(562, 263);
            this.elementosSeleccionados.Name = "elementosSeleccionados";
            this.elementosSeleccionados.Size = new System.Drawing.Size(127, 13);
            this.elementosSeleccionados.TabIndex = 29;
            this.elementosSeleccionados.Text = "Elementos seleccionados";
            // 
            // txtNote
            // 
            this.txtNote.AcceptsReturn = false;
            this.txtNote.AcceptsTab = false;
            this.txtNote.AnimationSpeed = 200;
            this.txtNote.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtNote.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtNote.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.txtNote.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("txtNote.BackgroundImage")));
            this.txtNote.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this.txtNote.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.txtNote.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.txtNote.BorderColorIdle = System.Drawing.Color.DimGray;
            this.txtNote.BorderRadius = 1;
            this.txtNote.BorderThickness = 1;
            this.txtNote.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtNote.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNote.DefaultFont = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.txtNote.DefaultText = "";
            this.txtNote.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.txtNote.ForeColor = System.Drawing.Color.White;
            this.txtNote.HideSelection = true;
            this.txtNote.IconLeft = null;
            this.txtNote.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNote.IconPadding = 0;
            this.txtNote.IconRight = null;
            this.txtNote.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtNote.Lines = new string[0];
            this.txtNote.Location = new System.Drawing.Point(8, 114);
            this.txtNote.MaxLength = 32767;
            this.txtNote.MinimumSize = new System.Drawing.Size(1, 1);
            this.txtNote.Modified = false;
            this.txtNote.Multiline = false;
            this.txtNote.Name = "txtNote";
            stateProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtNote.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.Empty;
            stateProperties2.FillColor = System.Drawing.Color.White;
            stateProperties2.ForeColor = System.Drawing.Color.Empty;
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtNote.OnDisabledState = stateProperties2;
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtNote.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.DimGray;
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            stateProperties4.ForeColor = System.Drawing.Color.White;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtNote.OnIdleState = stateProperties4;
            this.txtNote.PasswordChar = '\0';
            this.txtNote.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.txtNote.PlaceholderText = "";
            this.txtNote.ReadOnly = false;
            this.txtNote.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtNote.SelectedText = "";
            this.txtNote.SelectionLength = 0;
            this.txtNote.SelectionStart = 0;
            this.txtNote.ShortcutsEnabled = true;
            this.txtNote.Size = new System.Drawing.Size(696, 28);
            this.txtNote.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Material;
            this.txtNote.TabIndex = 28;
            this.txtNote.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtNote.TextMarginBottom = 0;
            this.txtNote.TextMarginLeft = 5;
            this.txtNote.TextMarginTop = 0;
            this.txtNote.TextPlaceholder = "";
            this.txtNote.UseSystemPasswordChar = false;
            this.txtNote.WordWrap = true;
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
            this.Device,
            this.PresetGuard,
            this.ActionColumn,
            this.deleteDataGridViewCheckBoxColumn,
            this.order,
            this.ObjPtz});
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
            this.bunifuDataObject.HeaderBgColor = System.Drawing.Color.Empty;
            this.bunifuDataObject.HeaderForeColor = System.Drawing.Color.White;
            this.bunifuDataObject.Location = new System.Drawing.Point(8, 289);
            this.bunifuDataObject.Name = "bunifuDataObject";
            this.bunifuDataObject.RowHeadersVisible = false;
            this.bunifuDataObject.RowHeadersWidth = 51;
            this.bunifuDataObject.RowTemplate.Height = 40;
            this.bunifuDataObject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.bunifuDataObject.ShowCellErrors = false;
            this.bunifuDataObject.ShowCellToolTips = false;
            this.bunifuDataObject.ShowEditingIcon = false;
            this.bunifuDataObject.ShowRowErrors = false;
            this.bunifuDataObject.Size = new System.Drawing.Size(696, 170);
            this.bunifuDataObject.TabIndex = 27;
            this.bunifuDataObject.Theme = Bunifu.UI.WinForms.BunifuDataGridView.PresetThemes.Dark;
            this.bunifuDataObject.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BunifuDataObject_CellMouseDown);
            this.bunifuDataObject.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.BunifuDataObject_CellMouseEnter);
            this.bunifuDataObject.CellMouseLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.bunifuDataObject_CellMouseLeave);
            this.bunifuDataObject.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.BunifuDataObject_CellPainting);
            this.bunifuDataObject.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BunifuDataObject_ColumnHeaderMouseClick);
            this.bunifuDataObject.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.BunifuDataObject_DataError);
            this.bunifuDataObject.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.bunifuDataObject_EditingControlShowing);
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
            // Device
            // 
            this.Device.DataPropertyName = "DeviceName";
            this.Device.FillWeight = 259F;
            this.Device.HeaderText = "Device";
            this.Device.MinimumWidth = 6;
            this.Device.Name = "Device";
            this.Device.ReadOnly = true;
            this.Device.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PresetGuard
            // 
            this.PresetGuard.DataPropertyName = "NameObjectSubType";
            this.PresetGuard.FillWeight = 200F;
            this.PresetGuard.HeaderText = "PresetGuard";
            this.PresetGuard.MinimumWidth = 6;
            this.PresetGuard.Name = "PresetGuard";
            this.PresetGuard.ReadOnly = true;
            this.PresetGuard.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ActionColumn
            // 
            this.ActionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ActionColumn.DataPropertyName = "ActionStr";
            this.ActionColumn.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.ActionColumn.FillWeight = 150F;
            this.ActionColumn.HeaderText = "Action";
            this.ActionColumn.MinimumWidth = 6;
            this.ActionColumn.Name = "ActionColumn";
            this.ActionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
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
            // ObjPtz
            // 
            this.ObjPtz.DataPropertyName = "ObjectSubType";
            this.ObjPtz.HeaderText = "ObjectSubType";
            this.ObjPtz.Name = "ObjPtz";
            this.ObjPtz.Visible = false;
            // 
            // separatorActions
            // 
            this.separatorActions.BackColor = System.Drawing.Color.Transparent;
            this.separatorActions.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.separatorActions.LineThickness = 1;
            this.separatorActions.Location = new System.Drawing.Point(366, 222);
            this.separatorActions.Margin = new System.Windows.Forms.Padding(4);
            this.separatorActions.Name = "separatorActions";
            this.separatorActions.Size = new System.Drawing.Size(130, 10);
            this.separatorActions.TabIndex = 25;
            this.separatorActions.Transparency = 255;
            this.separatorActions.Vertical = false;
            // 
            // labelDevices
            // 
            this.labelDevices.AutoSize = true;
            this.labelDevices.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelDevices.ForeColor = System.Drawing.Color.Silver;
            this.labelDevices.Location = new System.Drawing.Point(194, 187);
            this.labelDevices.Name = "labelDevices";
            this.labelDevices.Size = new System.Drawing.Size(56, 12);
            this.labelDevices.TabIndex = 15;
            this.labelDevices.Text = "Dispositivos";
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
            this.ButtonAdd.Location = new System.Drawing.Point(583, 192);
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
            // elementsAvailable
            // 
            this.elementsAvailable.AutoSize = true;
            this.elementsAvailable.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.elementsAvailable.ForeColor = System.Drawing.Color.Silver;
            this.elementsAvailable.Location = new System.Drawing.Point(3, 162);
            this.elementsAvailable.Name = "elementsAvailable";
            this.elementsAvailable.Size = new System.Drawing.Size(112, 20);
            this.elementsAvailable.TabIndex = 8;
            this.elementsAvailable.Text = "Iot disponibles";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelName.ForeColor = System.Drawing.Color.Silver;
            this.labelName.Location = new System.Drawing.Point(7, 40);
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
            this.txtName.Location = new System.Drawing.Point(8, 57);
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
            this.txtName.Size = new System.Drawing.Size(696, 28);
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
            // labelNote
            // 
            this.labelNote.AutoSize = true;
            this.labelNote.Font = new System.Drawing.Font("Segoe UI", 7.25F);
            this.labelNote.ForeColor = System.Drawing.Color.Silver;
            this.labelNote.Location = new System.Drawing.Point(12, 99);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(26, 12);
            this.labelNote.TabIndex = 5;
            this.labelNote.Text = "Nota";
            // 
            // tilteForm
            // 
            this.tilteForm.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tilteForm.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.tilteForm.ForeColor = System.Drawing.Color.Silver;
            this.tilteForm.Location = new System.Drawing.Point(27, 13);
            this.tilteForm.Name = "tilteForm";
            this.tilteForm.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tilteForm.Size = new System.Drawing.Size(124, 27);
            this.tilteForm.TabIndex = 13;
            this.tilteForm.Text = "Nueva Escena";
            this.tilteForm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // iotObject
            // 
            this.uciotObject.BackColor = System.Drawing.Color.Transparent;
            this.uciotObject.ForeColor = System.Drawing.Color.White;
            this.uciotObject.Location = new System.Drawing.Point(196, 200);
            this.uciotObject.Name = "iotObject";
            this.uciotObject.Size = new System.Drawing.Size(144, 32);
            this.uciotObject.TabIndex = 33;
            //this.uciotObject.isSearchingMode = false;
            this.uciotObject.SelectedIndexChanged += new System.EventHandler(this.iotObject_SelectedValueChanged);
            //this.iotObject.BackColor = System.Drawing.Color.Transparent;
            //this.iotObject.BorderRadius = 1;
            //this.iotObject.Color = System.Drawing.Color.Transparent;
            //this.iotObject.Cursor = System.Windows.Forms.Cursors.Hand;
            //this.iotObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            //this.iotObject.DisabledColor = System.Drawing.Color.Gray;
            //this.iotObject.DisplayMember = "Name";
            //this.iotObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            //this.iotObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            //this.iotObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //this.iotObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            //this.iotObject.FillDropDown = false;
            //this.iotObject.FillIndicator = false;
            //this.iotObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            //this.iotObject.ForeColor = System.Drawing.Color.White;
            //this.iotObject.FormattingEnabled = true;
            //this.iotObject.Icon = null;
            //this.iotObject.IndicatorColor = System.Drawing.Color.White;
            //this.iotObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            //this.iotObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.iotObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            //this.iotObject.ItemForeColor = System.Drawing.Color.White;
            //this.iotObject.ItemHeight = 26;
            //this.iotObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            //this.iotObject.Location = new System.Drawing.Point(196, 200);
            //this.iotObject.Margin = new System.Windows.Forms.Padding(2);
            //this.iotObject.Name = "iotObject";
            //this.iotObject.Size = new System.Drawing.Size(144, 32);
            //this.iotObject.TabIndex = 9;
            //this.iotObject.Text = null;
            //this.iotObject.ValueMember = "Key";
            //this.iotObject.SelectedValueChanged += new System.EventHandler(this.iotObject_SelectedValueChanged);
            // 
            // actionsObject
            // 
            this.actionsObject.BackColor = System.Drawing.Color.Transparent;
            this.actionsObject.BorderRadius = 1;
            this.actionsObject.Color = System.Drawing.Color.Transparent;
            this.actionsObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.actionsObject.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.actionsObject.DisabledColor = System.Drawing.Color.Gray;
            this.actionsObject.DisplayMember = "Name";
            this.actionsObject.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.actionsObject.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.actionsObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.actionsObject.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.actionsObject.FillDropDown = false;
            this.actionsObject.FillIndicator = false;
            this.actionsObject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.actionsObject.ForeColor = System.Drawing.Color.White;
            this.actionsObject.FormattingEnabled = true;
            this.actionsObject.Icon = null;
            this.actionsObject.IndicatorColor = System.Drawing.Color.White;
            this.actionsObject.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.actionsObject.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.actionsObject.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.actionsObject.ItemForeColor = System.Drawing.Color.White;
            this.actionsObject.ItemHeight = 26;
            this.actionsObject.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.actionsObject.Location = new System.Drawing.Point(366, 197);
            this.actionsObject.Margin = new System.Windows.Forms.Padding(2);
            this.actionsObject.Name = "actionsObject";
            this.actionsObject.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.actionsObject.Size = new System.Drawing.Size(144, 32);
            this.actionsObject.TabIndex = 11;
            this.actionsObject.Text = null;
            this.actionsObject.ValueMember = "Key";
            // 
            // errorManager
            // 
            this.errorManager.ContainerControl = this;
            // 
            // errorManagerPresetGuard
            // 
            this.errorManagerPresetGuard.ContainerControl = this;
            // 
            // bindingSourceScenes
            // 
            this.bindingSourceScenes.DataSource = typeof(Elipgo.SmartClient.Common.DTOs.ScenesEntity);
            // 
            // ScenesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ScenesControl";
            this.Size = new System.Drawing.Size(745, 538);
            this.Load += new System.EventHandler(this.FormScenes_load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bunifuDataObject)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManager)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorManagerPresetGuard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceScenes)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Bunifu.UI.WinForms.BunifuDataGridView bunifuDataObject;
        private Bunifu.Framework.UI.BunifuSeparator separatorActions;
        private Bunifu.Framework.UI.BunifuSeparator separatorDispositivos;
        private System.Windows.Forms.Label labelAction;
        private System.Windows.Forms.Label labelDevices;
        private Bunifu.Framework.UI.BunifuCustomLabel tilteForm;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton ButtonAdd;
        private Bunifu.UI.WinForms.BunifuDropdown actionsObject;
        //private Bunifu.UI.WinForms.BunifuDropdown iotObject;
        private Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown uciotObject;
        private System.Windows.Forms.Label elementsAvailable;
        private System.Windows.Forms.Label labelName;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txtName;
        private System.Windows.Forms.Label labelNote;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txtNote;
        private System.Windows.Forms.ErrorProvider errorManager;
        private System.Windows.Forms.ErrorProvider errorManagerPresetGuard;
        private System.Windows.Forms.BindingSource bindingSourceScenes;
        private System.Windows.Forms.Label elementosSeleccionados;
        private System.Windows.Forms.Label labelPresetGuardia;
        private Bunifu.Framework.UI.BunifuSeparator separatorPresetGuarda;
        private Bunifu.UI.WinForms.BunifuDropdown presetGuardiaObject;
        private System.Windows.Forms.Label labelSitio;
        private Bunifu.Framework.UI.BunifuSeparator separatorSitios;
        //private Bunifu.UI.WinForms.BunifuDropdown sitioiotObject;
        private Elipgo.SmartClient.UserControls.GenericForm.SearchableDropdown ucSitioiotObject;
        private System.Windows.Forms.Label labelListPresetGuardia;
        private Bunifu.Framework.UI.BunifuSeparator separatorListPresetGuarda;
        private Bunifu.UI.WinForms.BunifuDropdown presetListGuardiaObject;
        private Panel panelPagi;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn Device;
        private DataGridViewTextBoxColumn PresetGuard;
        private DataGridViewComboBoxColumn ActionColumn;
        private DataGridViewCheckBoxColumn deleteDataGridViewCheckBoxColumn;
        private DataGridViewImageColumn order;
        private DataGridViewTextBoxColumn ObjPtz;
    }
}
