using ReactiveUI;

namespace Elipgo.SmartClient.ViewModels
{
    public class ReportViewModel : ReactiveObject, IRoutableViewModel
    {
        private string _viewTitle;

        public MainViewModel MainView { get; set; } = null;

        public ReportViewModel()
        {
            ViewTitle = "Report";
        }

        public string ViewTitle
        {
            get => _viewTitle;
            set => this.RaiseAndSetIfChanged(ref _viewTitle, value);
        }

        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }

    }
}
