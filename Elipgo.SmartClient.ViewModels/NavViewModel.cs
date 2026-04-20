using ReactiveUI;

namespace Elipgo.SmartClient.ViewModels
{
    public class NavViewModel : ReactiveObject, IRoutableViewModel
    {
        private string _viewTitle;

        public NavViewModel()
        {
            ViewTitle = "Nav Mode";
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
