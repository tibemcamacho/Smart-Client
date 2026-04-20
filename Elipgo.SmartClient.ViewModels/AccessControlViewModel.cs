using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services;
using ReactiveUI;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.ViewModels
{
    public class AccessControlViewModel : ReactiveObject, IRoutableViewModel
    {
        private string _viewTitle;

        public MainViewModel MainView { get; set; } = null;

        public AccessControlViewModel()
        {
            ViewTitle = "Control Acceso";
        }

        public string ViewTitle
        {
            get => _viewTitle;
            set => this.RaiseAndSetIfChanged(ref _viewTitle, value);
        }


        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }

        public async Task<List<ACServerDTO>> GetAccessControlServers()
        {
            return await Vmon5Service.GetAccessControlServers(MainView.UserToken);
        }

    }
}
