using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services;
using ReactiveUI;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.ViewModels
{
    public class OcrViewModel : ReactiveObject, IRoutableViewModel
    {
        private string _viewTitle;

        public MainViewModel MainView { get; set; } = null;

        public OcrViewModel()
        {
            ViewTitle = "Ocr";
        }

        public string ViewTitle
        {
            get => _viewTitle;
            set => this.RaiseAndSetIfChanged(ref _viewTitle, value);
        }


        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }

        public async Task<CameraDTO> GetCamera(int elementId)
        {
            return await Vmon5Service.GetCamera(elementId, MainView.UserToken);
        }
    }
}
