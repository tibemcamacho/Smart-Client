using ReactiveUI;

namespace Elipgo.SmartClient.ViewModels
{
    public class FaceViewModel : ReactiveObject, IRoutableViewModel
    {
        private string _viewTitle;

        public MainViewModel MainView { get; set; } = null;

        public FaceViewModel()
        {
            ViewTitle = "Face";
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
