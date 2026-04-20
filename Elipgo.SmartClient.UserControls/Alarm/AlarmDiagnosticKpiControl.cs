using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.ViewModels;
using System;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public partial class AlarmDiagnosticKpiControl : AlarmDiagnosticBase
    {

        public AlarmDiagnosticKpiControl(AlarmViewModel viewModel, CardDto card) : base(viewModel, card)
        {
            InitializeComponent();
            CreatePanel("KPI");
        }

        private void AlarmDiagnosticKpiControl_Load(object sender, EventArgs e)
        {
            this.LoadStep();
        }
    }
}
