using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.Services.Services.Interface;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.ViewModels
{
    public class VisualSearchViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        private string _viewTitle;
        private List<SidebarGroupElementDTO> _sidebarElements = new List<SidebarGroupElementDTO>();
        private CatalogDTO _catalog = null;
        private IAppContainer _container = Locator.Current.GetService<IAppContainer>();
        private readonly IAppAuthorization _appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        public MainViewModel MainView { get; set; } = null;
        public SidebarElementDTO SelectSidebarElement { get; set; }
        public DateTime? SelectedDateTime { get; set; }
        public RoutingState Router { get; }
        private Guid _parent = Guid.Empty;

        public Guid Parent
        {
            get => this._parent;
            set => this.RaiseAndSetIfChanged(ref this._parent, value);
        }

        public void Dispose()
        {
            this._sidebarElements.Clear();
        }

        public void ShowToolbarButtonHiddenIcon()
        {
            _container.ShowToolbarHidenButton(MainView.ID);
        }

        public VisualSearchViewModel()
        {

        }

        public string ViewTitle
        {
            get => _viewTitle;
            set => this.RaiseAndSetIfChanged(ref _viewTitle, value);
        }

        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }

        public void SetName(Apps app)
        {
            if (_container.AppsActives.ContainsKey(Parent) == false)
            {
                _container.AppsActives.Add(Parent, app);
            }

            var n = _container.AppsActives.Where(x => x.Value == app).Count();
            MainView.ApplicationTitle = n > 1 ? $"{Resources.WelcomeVisualSearch} {n}" : Resources.WelcomeVisualSearch;
        }

        public void ShowSpinner(bool show)
        {
            MainView.Spinner = show;
        }

        //public CatalogDTO Catalog
        //{
        //    get => this._catalog;
        //    set => this.RaiseAndSetIfChanged(ref this._catalog, value);
        //}

        public async Task<List<SidebarGroupElementDTO>> GetDevices()
        {
            _sidebarElements = new List<SidebarGroupElementDTO>();

            if (_catalog.Sites == null)
            {
                _catalog.Sites = (await Vmon5Service.GetSiteInfo(MainView.UserToken)).Sites;
            }
            if (_catalog != null && _catalog.Sites != null)
            {
                foreach (var item in _catalog.Sites)
                {
                    SidebarGroupElementDTO group = new SidebarGroupElementDTO();
                    group.Name = item.Name;
                    group.ElementId = item.Id;

                    List<SidebarElementDTO> list = new List<SidebarElementDTO>();

                    list.AddRange(item.Cameras.Select(y => new SidebarElementDTO
                    {
                        ElementId = y.ObjectId,
                        GroupName = item.Name,
                        Name = y.Name,
                        DeviceType = ElementType.Camera,
                        DeviceTypeStr = y.Type,
                        Status = DeviceStatus.Online,
                        Key = Guid.NewGuid(),
                        SiteId = item.Id,
                        ShowDvfId = _appAuthorization.Exist("auth.app.apps.playback.showDVF_ID")

                    }).ToList());

                    list.AddRange(item.Iots.Select(y => new SidebarElementDTO
                    {
                        ElementId = y.ObjectId,
                        GroupName = item.Name,
                        Name = y.Name,
                        DeviceType = y.Type == "DI" ? ElementType.Iot_In : ElementType.Iot_Out,
                        Status = DeviceStatus.Online,
                        Key = Guid.NewGuid(),
                        SiteId = item.Id,
                        DeviceTypeStr = y.Type,
                        ShowDvfId = _appAuthorization.Exist("auth.app.apps.playback.showDVF_ID")
                    }).ToList());
                    group.SidebarElements = list;
                    _sidebarElements.Add(group);
                }
            }
            return _sidebarElements;
        }
    }
}
