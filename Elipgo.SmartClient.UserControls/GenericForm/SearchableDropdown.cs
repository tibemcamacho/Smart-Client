using Bunifu.UI.WinForms;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.GenericForm
{
    public partial class SearchableDropdown : UserControl
    {
        // Eventos personalizados para comunicarse con el formulario padre
        public event EventHandler<string> SearchRequested;
        public event EventHandler SelectedIndexChanged;

        // Estado del control
        public bool isSearchingMode = false;

        public SearchableDropdown()
        {
            InitializeComponent();
            ApplyCustomStyles();
            SwitchToSelectionMode(); // Iniciar en modo selección
        }

        private void ApplyCustomStyles()
        {
            // --- ESTILOS EXTRAÍDOS DE TU CÓDIGO DESIGNER ---

            // Estilos del contenedor principal
            this.BackColor = Color.FromArgb(30, 30, 30);
            this.ForeColor = Color.White;
            //this.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

            // Estilos del Dropdown (cmbDropdownList) - Replicando optionObjectName
            //cmbDropdownList.BackColor = Color.FromArgb(30, 30, 30);
            //cmbDropdownList.Color = Color.FromArgb(30, 30, 30); // Color de fondo principal
            //cmbDropdownList.ForeColor = Color.White;
            cmbDropdownList.BorderRadius = 1;
            cmbDropdownList.DropdownBorderThickness = BunifuDropdown.BorderThickness.Thin;
            cmbDropdownList.DropDownStyle = ComboBoxStyle.DropDownList; // MODO LECTURA PARA ESTILO CORRECTO
            cmbDropdownList.FlatStyle = FlatStyle.Flat;
            //cmbDropdownList.IndicatorColor = Color.White;
            // Colores de la lista desplegable
            //cmbDropdownList.ItemBackColor = Color.FromArgb(49, 49, 49);
            //cmbDropdownList.ItemBorderColor = Color.FromArgb(49, 49, 49);
            //cmbDropdownList.ItemForeColor = Color.White;
            //cmbDropdownList.ItemHighLightColor = Color.FromArgb(70, 70, 70);
            cmbDropdownList.ItemHeight = 26;

            // Estilos del TextBox de Búsqueda (txtSearchBox) - Para que coincida visualmente
            //txtSearchBox.BackColor = Color.FromArgb(30, 30, 30);
            //txtSearchBox.FillColor = Color.FromArgb(30, 30, 30);
            txtSearchBox.ForeColor = Color.White;
            //txtSearchBox.BorderColorIdle = Color.FromArgb(105, 105, 105); // Un gris para el borde
            txtSearchBox.BorderColorActive = Color.DodgerBlue;
            txtSearchBox.BorderRadius = 1;
            txtSearchBox.BorderThickness = 1;
            txtSearchBox.TextPlaceholder = Resources.search;
            txtSearchBox.PlaceholderForeColor = Color.Gray;
        }

        // Botón Lupa Click
        private void btnSearchToggle_Click(object sender, EventArgs e)
        {
            if (!isSearchingMode)
            {
                // Entrar en modo búsqueda
                SwitchToSearchMode();
            }
            else
            {
                // Ejecutar búsqueda y volver a modo selección
                var searchText = txtSearchBox.Text.Trim();
                // Disparamos el evento para que el Form padre filtre la lista
                SearchRequested?.Invoke(this, searchText);
                SwitchToSelectionMode();
            }
        }

        private void SwitchToSearchMode()
        {
            isSearchingMode = true;
            cmbDropdownList.Visible = false;
            txtSearchBox.Visible = true;
            txtSearchBox.Focus();
            txtSearchBox.Text = string.Empty;
            // Opcional: Cambiar icono de lupa a un "Check" o "X"
            // btnSearchToggle.Image = Resources.CheckIcon; 
        }

        public void SwitchToSelectionMode()
        {
            isSearchingMode = false;
            txtSearchBox.Visible = false;
            cmbDropdownList.Visible = true;
            // Opcional: Restaurar icono de lupa
            btnSearchToggle.IdleIconLeftImage = FileResources.icon_search_input;
            btnSearchToggle.onHoverState.IconLeftImage = FileResources.icon_search_input;
            btnSearchToggle.OnPressedState.IconLeftImage = FileResources.icon_search_input;
            btnSearchToggle.ButtonText = "";
        }

        // Propagar el evento de cambio de selección del dropdown interno
        private void cmbDropdownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedIndexChanged?.Invoke(this, e);
        }

        // --- PROPIEDADES PÚBLICAS PARA USAR DESDE EL FORMULARIO ---

        [Category("Data")]
        public object DataSource { get => cmbDropdownList.DataSource; set => cmbDropdownList.DataSource = value; }

        [Category("Data")]
        public string DisplayMember { get => cmbDropdownList.DisplayMember; set => cmbDropdownList.DisplayMember = value; }

        [Category("Data")]
        public string ValueMember { get => cmbDropdownList.ValueMember; set => cmbDropdownList.ValueMember = value; }

        [Category("Data")]
        public object SelectedValue { get => cmbDropdownList.SelectedValue; set => cmbDropdownList.SelectedValue = value; }

        [Category("Data")]
        public object SelectedItem { get => cmbDropdownList.SelectedItem; set => cmbDropdownList.SelectedItem = value; }
        [Category("Data")]
        public int SelectedIndex { get => cmbDropdownList.SelectedIndex; set => cmbDropdownList.SelectedIndex = value; }


        [Category("Data")]
        public ComboBox.ObjectCollection Items { get => cmbDropdownList.Items; }

        [Category("Data")]
        public int ItemHeight { get => cmbDropdownList.ItemHeight; set => cmbDropdownList.ItemHeight = value; }


        [Browsable(false)]
        public BunifuDropdown InnerDropdown => cmbDropdownList; // Acceso directo si es necesario
    }
}
