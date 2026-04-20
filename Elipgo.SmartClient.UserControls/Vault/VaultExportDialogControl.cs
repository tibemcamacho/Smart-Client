using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Vault
{
    public partial class VaultExportDialogControl : UserControl
    {

        public VaultItemCardDTO SelectedItem { get; set; }

        public object ViewModel { get; set; }

        public VaultExportDialogControl()
        {
            InitializeComponent();

            DataGridView.AutoGenerateColumns = false;
            DataGridView.RowHeadersVisible = false;
            DataGridView.RowTemplate.Height = 28;
            DataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 7.8f, FontStyle.Bold);
            DataGridView.Columns[1].HeaderText = Resources.delete;
            DataGridView.Columns[2].HeaderText = Resources.cameras;
            DataGridView.Columns[3].HeaderText = Resources.From;
            DataGridView.Columns[4].HeaderText = Resources.To;
            DataGridView.Columns[5].HeaderText = "";

            LabelBookmarkName.Text = "";
            LabelDateTime.Text = "";

        }
        public VaultExportDialogControl(VaultItemCardDTO item)
        {

            SelectedItem = item;
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.ParentForm.DialogResult = DialogResult.Cancel;
            this.ParentForm.Close();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.ParentForm.DialogResult = DialogResult.OK;
            this.ParentForm.Close();
        }
    }
}
