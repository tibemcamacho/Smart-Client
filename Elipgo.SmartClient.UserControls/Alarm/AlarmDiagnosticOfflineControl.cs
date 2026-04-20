using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.ViewModels;
using System;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public partial class AlarmDiagnosticOfflineControl : AlarmDiagnosticBase
    {

        public AlarmDiagnosticOfflineControl(AlarmViewModel viewModel, CardDto card) : base(viewModel, card)
        {
            InitializeComponent();
            CreatePanel("OFFLINE");
        }

        private void AlarmDiagnosticOfflineControl_Load(object sender, EventArgs e)
        {
            this.LoadStep();
        }
    }
}
