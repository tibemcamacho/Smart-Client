using ReactiveUI;

namespace Elipgo.SmartClient.ViewModels
{
    public class InvoiceViewModel : ReactiveObject, IRoutableViewModel
    {
        private string _viewTitle;

        public MainViewModel MainView { get; set; } = null;

        public InvoiceViewModel()
        {
            ViewTitle = "Invoice";
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
