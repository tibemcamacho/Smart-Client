using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services;
using ReactiveUI;

namespace Elipgo.SmartClient.ViewModels
{
    public class SmartEventViewModel : ReactiveObject, IRoutableViewModel
    {
        private string _viewTitle;

        public MainViewModel MainView { get; set; } = null;

        public SmartEventViewModel()
        {
            ViewTitle = "SmartEvent";
        }

        public string ViewTitle
        {
            get => _viewTitle;
            set => this.RaiseAndSetIfChanged(ref _viewTitle, value);
        }


        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }
        //TODO
        public async Task<CameraDTO> GetCamera(int elementId)
        {
            return await Vmon5Service.GetCamera(elementId, MainView.UserToken);
        }
    }
}
