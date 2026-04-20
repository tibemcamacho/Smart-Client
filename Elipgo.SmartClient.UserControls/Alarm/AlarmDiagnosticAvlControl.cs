using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.ViewModels;

namespace Elipgo.SmartClient.UserControls.Alarm
{
    public partial class AlarmDiagnosticAvlControl : AlarmDiagnosticBase
    {
        public AlarmDiagnosticAvlControl(AlarmViewModel viewModel, CardDto card) : base(viewModel, card)
        {
            InitializeComponent();
        }
    }
}
