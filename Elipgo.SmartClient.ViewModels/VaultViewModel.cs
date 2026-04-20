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
    public class VaultViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        private string _viewTitle;
        private readonly IAppContainer _container = Locator.Current.GetService<IAppContainer>();
        private readonly IBookmarkService _bookmarkService = Locator.Current.GetService<IBookmarkService>();
        private readonly IAppAuthorization _appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private readonly ICatalogService _catalogService = Locator.Current.GetService<ICatalogService>();
        private Guid _parent = Guid.Empty;
        private CatalogDTO _catalog = null;
        private List<SidebarElementDTO> _deviceElements = new List<SidebarElementDTO>();
        public int _dvfid = 0;

        public MainViewModel MainView { get; set; } = null;

        public CatalogDTO Catalog
        {
            get
            {
                return _catalog;
                //_catalog = _catalogService.GetValueCatalog(MainView.UserToken, _dvfid);
            }
            set
            {
                _catalogService.CurrentCatalog = value;
                this.RaiseAndSetIfChanged(ref _catalog, value); // Solo si necesitas disparar eventos UI
            }
        }

        public async Task<CatalogDTO> GetCatalogAsync()
        {
            return _catalog = await _catalogService.GetValueCatalog(MainView.UserToken, _dvfid);
        }
        public Guid Parent
        {
            get => this._parent;
            set => this.RaiseAndSetIfChanged(ref this._parent, value);
        }

        public VaultViewModel()
        {
            ViewTitle = "Vault Mode";
        }

        public string ViewTitle
        {
            get => _viewTitle;
            set => this.RaiseAndSetIfChanged(ref _viewTitle, value);
        }

        public void SetName(Apps app)
        {
            if (_container.AppsActives.ContainsKey(Parent) == false)
            {
                _container.AppsActives.Add(Parent, app);
            }

            var n = _container.AppsActives.Where(x => x.Value == app).Count();
            MainView.ApplicationTitle = n > 1 ? $"{Resources.WelcomeVault} {n}" : Resources.WelcomeVault;
        }

        public void ShowSpinner(bool show)
        {
            MainView.Spinner = show;
        }

        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }

        public async Task<List<BookmarkGroupDTO>> GetBookmarkGroups()
        {
            return await Vmon5Service.GetBookmarkGroups(MainView.UserToken);
        }

        public async Task<List<BookmarkGroupDTO>> GetBookmarkGroupsByFilter(BookmarkFilterDTO filter)
        {
            return await Vmon5Service.GetBookmarkGroupsByFilter(MainView.UserToken, filter);
        }

        public async Task<BookmarkGroupByPageDTO> GetBookmarkGroupsByPage(BookmarkFilterDTO filter)
        {
            return await Vmon5Service.GetBookmarkGroupsByPage(MainView.UserToken, filter);
        }

        public async Task<CameraDTO> GetCamera(int elementId)
        {
            return await Vmon5Service.GetCamera(elementId, MainView.UserToken);
        }

        public async Task<bool> ChangeStatusBookmarkGroup(int bookmarkGroupId, VaultItemCardState status)
        {
            return await Vmon5Service.ChangeStatusBookmarkGroup(bookmarkGroupId, (int)status, MainView.UserToken);
        }

        public async Task<bool> DeleteBookmark(List<int> bookmarkGroupId)
        {
            return await _bookmarkService.DeleteBookmark(bookmarkGroupId, MainView.UserToken);
        }

        //public bool IsEdge(int id)
        //{ 
        //    _dvfid = id;
        //    var cameras = Catalog.Sites.SelectMany(x => x.Cameras).Distinct().ToList();
        //    return !cameras.Find(x => x.ObjectId == id).EdgeEnabled.Value;
        //}

        public async Task<ApplicationEntitiesDTO> GetByEntityApplication()
        {
            return await Vmon5Service.GetByEntityApplication(MainView.UserToken);
        }

        public async Task<bool> ChangeStatusBookmarkGroup(List<int> bookmarkGroupIds, VaultItemCardState status)
        {
            return await Vmon5Service.ChangeStatusBookmarkGroup(bookmarkGroupIds, (int)status, MainView.UserToken);
        }


        public async Task<CatalogObjectDetails> GetDevices(int page,int take,int typeElement, int userId, GroupType typeGroup,string search = "")
        {
            _deviceElements = new List<SidebarElementDTO>();
            var devices = new CatalogObjectDetails();

           var device = await Vmon5Service.GetDevicesCatalog(MainView.UserToken, page, take, typeElement,userId,typeGroup,search);


            if (device != null && device.Elements.Count != 0)
            {

                var list = device.Elements.Select(y => new SidebarElementDTO
                {
                    ElementId = y.ElementId, // O y.ObjectId si viene de otra clase
                    GroupName = y.GroupName,
                    Name = y.Name,
                    DeviceType = ElementType.Camera,
                    DeviceTypeStr = y.DeviceTypeStr,
                    Status = DeviceStatus.Online,
                    Key = Guid.NewGuid(),
                    SiteId = y.SiteId,
                    RecorderName = y.RecorderName,
                    RecorderType = RecorderType.EDGE,
                    ShowDvfId = _appAuthorization.Exist("auth.app.apps.playback.showDVF_ID")
                }).ToList();

                _deviceElements.AddRange(list);
            }
            devices.Count = device.Count;
            devices.Elements = _deviceElements;
            return devices;
        }

        public void Dispose()
        {
            _deviceElements.Clear();
        }
    }
}
