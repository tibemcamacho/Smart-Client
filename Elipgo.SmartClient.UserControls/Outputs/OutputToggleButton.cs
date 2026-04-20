using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.UserControls.Shared;
using Elipgo.SmartClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Elipgo.SmartClient.UserControls.Outputs
{
    public partial class OutputToggleButton : UserControl
    {
        public LiveViewModel ViewModel { get; set; }
        private List<CatalogIot> Catalog { get; set; }

        public OutputToggleButton(List<CatalogIot> catalog, LiveViewModel viewModel)
        {
            InitializeComponent();

            ViewModel = viewModel;
            Catalog = catalog;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var content = new OutputPanelControl(ViewModel);
            content.SetContent(Catalog);
            PoperContainer poper = new PoperContainer(content);
            poper.Show(this);
        }
    }
}
