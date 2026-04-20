using Elipgo.SmartClient.Common.Properties;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    partial class SearchableDropdown
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchableDropdown));
            Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges borderEdges1 = new Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderEdges();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties1 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties2 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties3 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();
            Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties stateProperties4 = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox.StateProperties();

            this.cmbDropdownList = new Bunifu.UI.WinForms.BunifuDropdown();
            this.txtSearchBox = new Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox();
            this.btnSearchToggle = new Bunifu.UI.WinForms.BunifuButton.BunifuButton();

            this.SuspendLayout();

            // 
            // cmbDropdownList
            // 
            //this.cmbDropdownList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cmbDropdownList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.cmbDropdownList.BorderRadius = 1;
            //this.cmbDropdownList.Color = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.cmbDropdownList.Color = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.cmbDropdownList.Direction = Bunifu.UI.WinForms.BunifuDropdown.Directions.Down;
            this.cmbDropdownList.DisabledColor = System.Drawing.Color.Gray;
            this.cmbDropdownList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbDropdownList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cmbDropdownList.DropdownBorderThickness = Bunifu.UI.WinForms.BunifuDropdown.BorderThickness.Thin;
            this.cmbDropdownList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDropdownList.DropDownTextAlign = Bunifu.UI.WinForms.BunifuDropdown.TextAlign.Left;
            this.cmbDropdownList.FillDropDown = false;
            this.cmbDropdownList.FillIndicator = false;
            this.cmbDropdownList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbDropdownList.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbDropdownList.ForeColor = System.Drawing.Color.White;
            this.cmbDropdownList.FormattingEnabled = true;
            this.cmbDropdownList.Icon = null;
            // INDICADOR TRANSPARENTE para que no se vea debajo de nuestro botón
            //this.cmbDropdownList.IndicatorColor = System.Drawing.Color.Transparent;
            this.cmbDropdownList.IndicatorColor = System.Drawing.Color.White;
            this.cmbDropdownList.IndicatorLocation = Bunifu.UI.WinForms.BunifuDropdown.Indicator.Right;
            this.cmbDropdownList.ItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.cmbDropdownList.ItemBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.cmbDropdownList.ItemForeColor = System.Drawing.Color.White;
            this.cmbDropdownList.ItemHeight = 26;
            this.cmbDropdownList.ItemHighLightColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.cmbDropdownList.Location = new System.Drawing.Point(0, 0);
            this.cmbDropdownList.Name = "cmbDropdownList";
            this.cmbDropdownList.Size = new System.Drawing.Size(292, 34);
            this.cmbDropdownList.TabIndex = 1;
            this.cmbDropdownList.Text = null;
            this.cmbDropdownList.SelectedIndexChanged += new System.EventHandler(this.cmbDropdownList_SelectedIndexChanged);

            // 
            // txtSearchBox
            // 
            this.txtSearchBox.AcceptsReturn = false;
            this.txtSearchBox.AcceptsTab = false;
            this.txtSearchBox.AnimationSpeed = 200;
            this.txtSearchBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtSearchBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtSearchBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            
            this.txtSearchBox.BorderColorActive = System.Drawing.Color.DodgerBlue;
            this.txtSearchBox.BorderColorDisabled = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(161)))), ((int)(((byte)(161)))));
            this.txtSearchBox.BorderColorHover = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            //this.txtSearchBox.BorderColorIdle = stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30))))); //System.Drawing.Color.DimGray;
            this.txtSearchBox.BorderColorIdle = System.Drawing.Color.DimGray;
            this.txtSearchBox.BorderRadius = 1;
            this.txtSearchBox.BorderThickness = 1;
            this.txtSearchBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtSearchBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchBox.DefaultFont = new System.Drawing.Font("Segoe UI Semibold", 9.75F);
            this.txtSearchBox.DefaultText = "";
            this.txtSearchBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            this.txtSearchBox.ForeColor = System.Drawing.Color.White;
            this.txtSearchBox.HideSelection = true;
            this.txtSearchBox.IconLeft = null;
            this.txtSearchBox.IconLeftCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchBox.IconPadding = 10;
            this.txtSearchBox.IconRight = null;
            this.txtSearchBox.IconRightCursor = System.Windows.Forms.Cursors.IBeam;
            this.txtSearchBox.Location = new System.Drawing.Point(0, 0);
            this.txtSearchBox.MaxLength = 32767;
            this.txtSearchBox.Modified = false;
            this.txtSearchBox.Multiline = false;
            this.txtSearchBox.Name = "txtSearchBox";
            stateProperties1.BorderColor = System.Drawing.Color.DodgerBlue;
            //stateProperties1.FillColor = System.Drawing.Color.Empty;
            stateProperties1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            stateProperties1.ForeColor = System.Drawing.Color.Empty;
            stateProperties1.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtSearchBox.OnActiveState = stateProperties1;
            stateProperties2.BorderColor = System.Drawing.Color.DimGray;
            //stateProperties2.FillColor = System.Drawing.Color.White;
            //stateProperties2.ForeColor = System.Drawing.Color.Empty;
            stateProperties2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            stateProperties2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            stateProperties2.PlaceholderForeColor = System.Drawing.Color.Silver;
            this.txtSearchBox.OnDisabledState = stateProperties2;
            //stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            stateProperties3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            //stateProperties3.FillColor = System.Drawing.Color.Empty;
            stateProperties3.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49)))));
            stateProperties3.ForeColor = System.Drawing.Color.Empty;
            stateProperties3.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtSearchBox.OnHoverState = stateProperties3;
            stateProperties4.BorderColor = System.Drawing.Color.DimGray;
            stateProperties4.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(49)))), ((int)(((byte)(49))))); // Fondo gris oscuro
            stateProperties4.ForeColor = System.Drawing.Color.White;
            stateProperties4.PlaceholderForeColor = System.Drawing.Color.Empty;
            this.txtSearchBox.OnIdleState = stateProperties4;
            this.txtSearchBox.PasswordChar = '\0';
            this.txtSearchBox.PlaceholderForeColor = System.Drawing.Color.DarkGray;
            this.txtSearchBox.PlaceholderText = "Buscar...";
            this.txtSearchBox.ReadOnly = false;
            this.txtSearchBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtSearchBox.SelectedText = "";
            this.txtSearchBox.SelectionLength = 0;
            this.txtSearchBox.SelectionStart = 0;
            this.txtSearchBox.ShortcutsEnabled = true;
            this.txtSearchBox.Size = new System.Drawing.Size(287, 32);
            this.txtSearchBox.Style = Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox._Style.Material;
            this.txtSearchBox.TabIndex = 2;
            this.txtSearchBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtSearchBox.TextMarginBottom = 0;
            this.txtSearchBox.TextMarginLeft = 5;
            this.txtSearchBox.TextMarginTop = 0;
            this.txtSearchBox.TextPlaceholder = "Buscar...";
            this.txtSearchBox.UseSystemPasswordChar = false;
            this.txtSearchBox.Visible = false;
            this.txtSearchBox.WordWrap = true;

            // 
            // btnSearchToggle
            // 
            this.btnSearchToggle.AllowToggling = false;
            this.btnSearchToggle.AnimationSpeed = 200;
            this.btnSearchToggle.AutoGenerateColors = false;
            this.btnSearchToggle.BackColor = System.Drawing.Color.Transparent;
            this.btnSearchToggle.BackColor1 = System.Drawing.Color.Transparent;
            this.btnSearchToggle.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;

            // CAMBIO: FLECHA HACIA ABAJO (▼) en lugar de Lupa
            //this.btnSearchToggle.ButtonText = "▼";
            this.btnSearchToggle.ButtonTextMarginLeft = 0;

            this.btnSearchToggle.ColorContrastOnClick = 45;
            this.btnSearchToggle.ColorContrastOnHover = 45;
            this.btnSearchToggle.Cursor = System.Windows.Forms.Cursors.Hand;

            borderEdges1.BottomLeft = true;
            borderEdges1.BottomRight = true;
            borderEdges1.TopLeft = true;
            borderEdges1.TopRight = true;
            this.btnSearchToggle.CustomizableEdges = borderEdges1;

            this.btnSearchToggle.DialogResult = System.Windows.Forms.DialogResult.None;
            this.btnSearchToggle.DisabledBorderColor = System.Drawing.Color.Empty;
            this.btnSearchToggle.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.btnSearchToggle.DisabledForecolor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(160)))), ((int)(((byte)(168)))));

            this.btnSearchToggle.Dock = System.Windows.Forms.DockStyle.Right;
            //this.btnSearchToggle.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;

            this.btnSearchToggle.FocusState = Bunifu.UI.WinForms.BunifuButton.BunifuButton.ButtonStates.Pressed;

            // AJUSTE: Fuente más pequeña (9F) para que la flecha se vea estética y no gigante
            this.btnSearchToggle.Font = new System.Drawing.Font("Segoe UI Symbol", 9F, System.Drawing.FontStyle.Regular);
            this.btnSearchToggle.ForeColor = System.Drawing.Color.White;

            this.btnSearchToggle.IconLeftCursor = System.Windows.Forms.Cursors.Hand;
            this.btnSearchToggle.IconMarginLeft = 5;
            this.btnSearchToggle.IconPadding = 5;
            this.btnSearchToggle.IconRightCursor = System.Windows.Forms.Cursors.Hand;

            // IDLE: Transparente
            this.btnSearchToggle.IdleBorderColor = System.Drawing.Color.Transparent;
            this.btnSearchToggle.IdleBorderRadius = 1;
            this.btnSearchToggle.IdleBorderThickness = 1;
            this.btnSearchToggle.IdleFillColor = System.Drawing.Color.Transparent;
            this.btnSearchToggle.IdleIconLeftImage = null;
            this.btnSearchToggle.IdleIconRightImage = null;
            this.btnSearchToggle.IndicateFocus = false;

            //this.btnSearchToggle.Location = new System.Drawing.Point(253, 1);
            this.btnSearchToggle.Name = "btnSearchToggle";

            // HOVER
            this.btnSearchToggle.onHoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.btnSearchToggle.onHoverState.BorderRadius = 1;
            this.btnSearchToggle.onHoverState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnSearchToggle.onHoverState.BorderThickness = 1;
            this.btnSearchToggle.onHoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(181)))), ((int)(((byte)(255)))));
            this.btnSearchToggle.onHoverState.ForeColor = System.Drawing.Color.White;
            this.btnSearchToggle.onHoverState.IconLeftImage = null;
            this.btnSearchToggle.onHoverState.IconRightImage = null;

            // PRESSED
            this.btnSearchToggle.OnPressedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnSearchToggle.OnPressedState.BorderRadius = 1;
            this.btnSearchToggle.OnPressedState.BorderStyle = Bunifu.UI.WinForms.BunifuButton.BunifuButton.BorderStyles.Solid;
            this.btnSearchToggle.OnPressedState.BorderThickness = 1;
            this.btnSearchToggle.OnPressedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(96)))), ((int)(((byte)(144)))));
            this.btnSearchToggle.OnPressedState.ForeColor = System.Drawing.Color.White;
            this.btnSearchToggle.OnPressedState.IconLeftImage = null;
            this.btnSearchToggle.OnPressedState.IconRightImage = null;

            this.btnSearchToggle.Size = new System.Drawing.Size(33, 30);
            this.btnSearchToggle.TabIndex = 3;
            this.btnSearchToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnSearchToggle.TextMarginLeft = 0;
            this.btnSearchToggle.UseDefaultRadiusAndThickness = true;
            this.btnSearchToggle.Visible = true;
            this.btnSearchToggle.Click += new System.EventHandler(this.btnSearchToggle_Click);

            // 
            // SearchableDropdown
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;

            this.Controls.Add(this.btnSearchToggle);
            this.Controls.Add(this.txtSearchBox);
            this.Controls.Add(this.cmbDropdownList);

            this.Name = "SearchableDropdown";
            this.Size = new System.Drawing.Size(287, 32);
            this.ResumeLayout(false);

        }

        #endregion
        private Bunifu.UI.WinForms.BunifuDropdown cmbDropdownList;
        private Bunifu.UI.WinForms.BunifuTextbox.BunifuTextBox txtSearchBox;
        private Bunifu.UI.WinForms.BunifuButton.BunifuButton btnSearchToggle;
    }
}