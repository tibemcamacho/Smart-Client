using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services.Services.Interface;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.ViewModels
{
    public class SeprobanViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        private string _viewTitle;
        private readonly IAlarmService _alarmService = Locator.Current.GetService<IAlarmService>();
        private readonly IAppContainer _container = Locator.Current.GetService<IAppContainer>();
        private ISeprobanService _seprobanService = Locator.Current.GetService<ISeprobanService>();

        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }
        public MainViewModel MainView { get; set; } = null;
        public string Token { get; set; }

        private Guid _parent = Guid.Empty;
        public Guid Parent
        {
            get => this._parent;
            set => this.RaiseAndSetIfChanged(ref this._parent, value);
        }
        public void Dispose()
        {

        }

        public void SetName(Apps app)
        {
            if (_container.AppsActives.ContainsKey(Parent) == false)
            {
                _container.AppsActives.Add(Parent, app);
            }

            var n = _container.AppsActives.Where(x => x.Value == app).Count();
            MainView.ApplicationTitle = n > 1 ? $"{Resources.WelcomeSeproban} {n}" : Resources.WelcomeSeproban;
        }

        public string ViewTitle
        {
            get => _viewTitle;
            set => this.RaiseAndSetIfChanged(ref _viewTitle, value);
        }

        public async Task<IList<SiteDTO>> GetSites()
        {
            return await _seprobanService.GetSites(Token);
        }

        public async Task<IList<CameraDTO>> GetCamerasBySite(int sitId, CancellationToken tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.ThrowIfCancellationRequested();
                return new List<CameraDTO>();
            }

            return await _seprobanService.GetCamerasBySite(sitId, Token);
        }

        public async Task<IList<RecorderDTO>> GetNvrsBySite(int sitId, CancellationToken tokenSource)
        {
            if (tokenSource.IsCancellationRequested)
            {
                tokenSource.ThrowIfCancellationRequested();
                return new List<RecorderDTO>();
            }
            return await _seprobanService.GetNvrsBySite(sitId, Token);
        }

    }
}
