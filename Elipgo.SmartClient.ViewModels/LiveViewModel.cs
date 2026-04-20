using DynamicData;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.Services.Services.Interface;
using Elipgo.SmartClient.SignalR.Connection;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Elipgo.SmartClient.Common.Enum.Enums;
using static Elipgo.SmartClient.Common.Enum.TypeAlarms;

namespace Elipgo.SmartClient.ViewModels
{
    public class LiveViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        private string _viewTitle;
        private List<SidebarGroupElementDTO> _sidebarElements = new List<SidebarGroupElementDTO>();
        private CatalogDTO _catalog = new CatalogDTO();
        private Guid _parent;
        private readonly IAppContainer _container = Locator.Current.GetService<IAppContainer>();
        private readonly IIotService _iotService = Locator.Current.GetService<IIotService>();
        private readonly IAuditService _auditService = Locator.Current.GetService<IAuditService>();
        private readonly IAppAuthorization _appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private readonly IAlarmService _alarmService = Locator.Current.GetService<IAlarmService>();
        private readonly VmonitoringManagerConnection _signal; // = Locator.Current.GetService<VmonitoringManagerConnection>();
        private readonly ICatalogService _catalogService = Locator.Current.GetService<ICatalogService>();
        public int _dvfid = 0;
        private bool _useMultipleIds = false;
        public List<int> _dvfIds = new List<int>(); 
        public MainViewModel MainView { get; set; } = null;

        public LiveViewModel()
        {
            ViewTitle = "Live";
            if (MainView != null)
            {
                _signal = MainView.Signal;
                _signal.IotStatusEventAction += IotStatusEvent;
            }
        }

        public void IotStatusEvent(dynamic d)
        {
            if (d != null)
            {
                this.StatusIot = d;
            }
        }
        public bool UseMultipleIds
        {
            get => _useMultipleIds;
            set => this.RaiseAndSetIfChanged(ref _useMultipleIds, value);
        }
        public CatalogDTO Catalog
        {
            get
            {
                return _catalog;
            }
            set
            {
                _catalogService.CurrentCatalog = value;
                this.RaiseAndSetIfChanged(ref _catalog, value); // Solo si necesitas disparar eventos UI
            }
        }

        public async Task<CatalogDTO> GetCatalogAsync()
        {
            if (_useMultipleIds && _dvfIds != null && _dvfIds.Count > 0)
            {
                _catalog = await _catalogService.GetValueCatalog(MainView.UserToken, _dvfIds);
            }
            else
            {
                _catalog = await _catalogService.GetValueCatalog(MainView.UserToken, _dvfid);
            }

            return _catalog;
        }

        //private readonly object _lock = new object();

        public string ViewTitle
        {
            get => _viewTitle;
            set => this.RaiseAndSetIfChanged(ref _viewTitle, value);
        }

        public List<SidebarGroupElementDTO> SidebarElements
        {
            get => _sidebarElements;
            set => this.RaiseAndSetIfChanged(ref _sidebarElements, value);
        }

        public Guid Parent
        {
            get => this._parent;
            set => this.RaiseAndSetIfChanged(ref this._parent, value);
        }

        private bool _statusIot;
        public bool StatusIot
        {
            get => this._statusIot;
            set => this.RaiseAndSetIfChanged(ref this._statusIot, value);
        }

        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }

        public async Task<List<SidebarGroupElementDTO>> GetLocationAlarmGeoMap()
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
                    SidebarGroupElementDTO group = new SidebarGroupElementDTO
                    {
                        Name = item.Name
                    };
                    List<SidebarElementDTO> list = new List<SidebarElementDTO>
                    {
                        new SidebarElementDTO()
                        {
                            ElementId = item.Id,
                            Name = item.Name,
                            DeviceType = ElementType.AlarmsMap,
                            Key = Guid.NewGuid(),
                            RecorderName = item.Name,
                            RecorderType = RecorderType.EDGE,
                            DeviceTypeStr = "AlarmsMap",
                            SiteId = item.Id,
                        }
                    };
                    group.SidebarElements = list;
                    _sidebarElements.Add(group);
                }

            }
            return _sidebarElements;

        }
        public List<SidebarGroupElementDTO> GetLocations()
        {
            _sidebarElements = new List<SidebarGroupElementDTO>();
            //if (_catalog.Sites == null)
            //{
            //    _catalog.Sites = Vmon5Service.GetUserInfo(MainView.UserToken).Sites;
            //}

            if (_catalog != null && _catalog.Sites != null)
            {
                foreach (var item in _catalog.Sites)
                {
                    SidebarGroupElementDTO group = new SidebarGroupElementDTO
                    {
                        Name = item.Name
                    };
                    List<SidebarElementDTO> list = new List<SidebarElementDTO>
                    {
                        new SidebarElementDTO()
                        {
                            ElementId = item.Id,
                            Name = item.Name,
                            DeviceType = ElementType.Geomap,
                            Key = Guid.NewGuid(),
                            RecorderName = item.Name,
                            RecorderType = RecorderType.EDGE,
                            DeviceTypeStr = "GEO",
                            SiteId = item.Id,
                        },
                        //new SidebarElementDTO()
                        //{
                        //    ElementId = item.Id,
                        //    Name = item.Name,
                        //    DeviceType = ElementType.Alarms_Map,
                        //    Key = Guid.NewGuid(),
                        //    RecorderName = item.Name,
                        //    RecorderType = RecorderType.EDGE,
                        //    DeviceTypeStr = "AlarmsMap",
                        //    SiteId = item.Id,
                        //}
                    };

                    list.AddRange(item.Locations.Select(y => new SidebarElementDTO
                    {
                        ElementId = y.ObjectId,
                        Name = y.Name,
                        DeviceType = ElementType.Location,
                        Key = Guid.NewGuid(),
                        SiteId = item.Id,
                        DeviceTypeStr = y.Type,
                        RecorderName = y.Name,
                        RecorderType = RecorderType.EDGE
                    }).ToList());

                    group.SidebarElements = list;
                    _sidebarElements.Add(group);
                }

            }
            return _sidebarElements;
        }

        public async Task<List<OptionObjectDTO>> GetAlarmGeoMap()
        {
            var listMap = new List<OptionObjectDTO>();
            if (_catalog == null)
                _catalog = await Vmon5Service.GetCarouselInfo(MainView.UserToken);
            if (_catalog != null && _catalog.Sites != null)
            {
                foreach (var item in _catalog.Sites)
                {
                    List<OptionObjectDTO> list = new List<OptionObjectDTO>
                    {
                        new OptionObjectDTO()
                        {
                            Name = item.Name,
                            Key = Guid.NewGuid().ToString(),
                            Tag = item.Id,
                            Item = item,
                        }
                    };
                    listMap.Add(list);
                }
            }
            return listMap;

        }

        public List<OptionObjectDTO> GetLocationsName()
        {
            var list = new List<OptionObjectDTO>();
            if (_catalog == null || _catalog.Sites == null)
            {
                //_catalog.Sites = Vmon5Service.GetSiteCatalog(MainView.UserToken,1,50, (int)ElementSidebar.Locations,0, GroupType.LIVE,"-1",string.Empty).;
            }

            if (_catalog != null && _catalog.Sites != null)
            {
                foreach (var item in _catalog.Sites)
                {
                    list.AddRange(item.Locations.Select(y => new OptionObjectDTO
                    {
                        Name = y.Name,
                        Key = y.ObjectId.ToString(),
                        Tag = item.Id,
                        Item = y
                    }).ToList());
                }

            }
            return list;
        }

        public Element GetElement(TypesFilters type, int idElement)
        {
            var list = new List<OptionObjectDTO>();
            if (_catalog.Sites == null)
            {
                //_catalog.Sites = Vmon5Service.GetUserInfo(MainView.UserToken).Sites;
            }

            Element element = new Element();
            if (type == TypesFilters.Device)
            {
                if (_catalog != null && _catalog.Sites != null)
                {
                    foreach (var item in _catalog.Sites)
                    {
                        var camera = item.Cameras.Where(c => c.ObjectId == idElement).FirstOrDefault();
                        if (camera != null)
                        {
                            element.SiteId = item.Id;
                            element.Type = ElementType.Camera.ToString();
                        }
                        if (element.Type != null)
                        {
                            return element;
                        }

                        var iot = item.Iots.Where(c => c.ObjectId == idElement).FirstOrDefault();
                        if (iot != null)
                        {
                            element.SiteId = item.Id;
                            element.Type = iot.Type == "DI" ? ElementType.Iot_In.ToString() : ElementType.Iot_Out.ToString();
                        }
                        if (element.Type != null)
                        {
                            return element;
                        }
                    }
                }
            }

            else if (type == TypesFilters.Locations)
            {
                if (_catalog != null && _catalog.Sites != null)
                {
                    foreach (var item in _catalog.Sites)
                    {
                        var location = item.Locations.Where(c => c.ObjectId == idElement).FirstOrDefault();
                        if (location != null)
                        {
                            element.SiteId = item.Id;
                            element.Type = ElementType.Location.ToString();
                        }
                        if (element.Type != null)
                        {
                            return element;
                        }
                    }
                }
            }
            else if (type == TypesFilters.AlarmsMap)
            {
                if (_catalog != null && _catalog.Sites != null)
                {
                    foreach (var item in _catalog.Sites)
                    {
                        if (item != null)
                        {
                            element.SiteId = item.Id;
                            element.Type = ElementType.AlarmsMap.ToString();
                        }
                        if (element.Type != null)
                        {
                            return element;
                        }
                    }
                }
            }

            else if (type == TypesFilters.Analytics)
            {
                if (_catalog != null && _catalog.Sites != null)
                {
                    foreach (var item in _catalog.Sites)
                    {
                        var kpi = item.Kpis.Where(c => c.ObjectId == idElement).FirstOrDefault();
                        if (kpi != null)
                        {
                            element.SiteId = item.Id;
                            element.Type = ElementType.Kpi.ToString();
                        }
                        if (element.Type != null)
                        {
                            return element;
                        }

                        var face = item.Faces.Where(c => c.ObjectId == idElement).FirstOrDefault();
                        if (face != null)
                        {
                            element.SiteId = item.Id;
                            element.Type = ElementType.Face.ToString();
                        }
                        if (element.Type != null)
                        {
                            return element;
                        }

                        var lpr = item.Lprs.Where(c => c.ObjectId == idElement).FirstOrDefault();
                        if (lpr != null)
                        {
                            element.SiteId = item.Id;
                            element.Type = ElementType.Lpr.ToString();
                        }
                        if (element.Type != null)
                        {
                            return element;
                        }
                    }
                }
            }
            else
            {
                if (_catalog != null && _catalog.Carousels != null)
                {
                    var carousel = _catalog.Carousels.Where(c => c.Id == idElement).FirstOrDefault();
                    if (carousel != null)
                    {
                        element.Type = ElementType.Carousel.ToString();
                    }
                    if (element.Type != null)
                    {
                        return element;
                    }
                }
            }
            return element;
        }

        public async Task<List<OptionObjectDTO>> GetAllSites(string typeObject, string classObject)
        {
            var list = new List<OptionObjectDTO>();                 /* Objeto devuelto. */
            var listAll = new List<OptionObjectDTO>();              /* Lista temporal para recolectar los dispositivos. */

            /* Cargamos primero los dispositivos para agruparlos por Id de sitio. */
            switch (typeObject)
            {
                case "Locations":
                    listAll = GetLocationsName().GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
                    break;
                case "Devices":
                    listAll = GetDevicesName(classObject).GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
                    break;
                case "Analytics":
                    listAll = GetAnalyticsName(classObject).GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
                    break;
                case "Carousels":
                    listAll = GetCarouselsName().GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
                    break;
                default:
                    listAll = GetDevicesName(classObject).GroupBy(u => u.Tag).Select(x => x.FirstOrDefault()).OrderByDescending(u => u.Tag).ToList();
                    break;
            }

            /* Después, por tipo de objeto, filtramos las sucursales. */
            if (listAll.Count > 0)
            {
                var listSitesStr = listAll.Select(u => u.Tag).ToList();

                /* Genero la lista de sucursales. */
                var AllSites = (await this.GetCatalogAsync()).Sites.Select(u => new { u.Id, u.Name }).Where(u => listSitesStr.Contains(u.Id)).ToList();

                /* Cargo las sucursales finales. */
                if (AllSites != null && AllSites.Count > 0)
                {
                    /* Limpio la lista. */
                    list.Clear();
                    foreach (var item in AllSites)
                    {
                        list.Add(new OptionObjectDTO { Name = item.Name, Key = item.Id.ToString(), Tag = item.Id.ToString() });
                    }
                }
                else
                {
                    /* Lanzo la excepción. */
                    throw new ArgumentNullException();
                }
            }
            else
            {
                /* Lanzo la excepción. */
                //throw new ArgumentNullException();
                Logger.Log("Sites not founds");
                return new List<OptionObjectDTO>();
            }

            /* Devuelvo el resultado final. */
            return list;
        }

        public List<OptionObjectDTO> GetDevicesName(string type)
        {
            var list = new List<OptionObjectDTO>();
            if (_catalog?.Sites == null)
            {
                //_catalog.Sites = Vmon5Service.GetUserInfo(MainView.UserToken).Sites;
            }

            if (_catalog != null && _catalog.Sites != null)
            {
                foreach (var item in _catalog.Sites)
                {
                    if (type == "All")
                    {
                        /* Solo en "Camaras", tienen este dato de DeviceName. Los demás, no lo tienen. */
                        list.AddRange(item.Cameras.Select(y => new OptionObjectDTO
                        {
                            /* Nombre del dispositivo - Canal de video. => Anterior: Name = (y.Name != y.DeviceName) ? y.Name : y.Name  + " - " + y.DeviceName, */
                            Name = (y.Name != y.DeviceName) ? y.DeviceName + " - " + y.Name : y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y
                        }).ToList());


                        var filter = "";

                        if (type == "Iot_In")
                        {
                            filter = "DI";
                        }
                        else if (type == "Iot_Out")
                        {
                            filter = "DO";
                        }

                        list.AddRange(item.Iots.Where(y => (filter == "" ? y.Type != filter : y.Type == filter)).Select(y => new OptionObjectDTO
                        {
                            Name = y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y
                        }).ToList());
                    }
                    else if (type == "Camera")
                    {
                        list.AddRange(item.Cameras.Select(y => new OptionObjectDTO
                        {
                            /* Nombre del dispositivo - Canal de video. => Anterior: Name = (y.Name != y.DeviceName) ? y.Name : y.Name  + " - " + y.DeviceName, */
                            Name = (y.Name != y.DeviceName) ? y.DeviceName + " - " + y.Name : y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y
                        }).ToList());
                    }
                    else
                    {
                        var filter = "";

                        if (type == "Iot_In")
                        {
                            filter = "DI";
                        }
                        else if (type == "Iot_Out")
                        {
                            filter = "DO";
                        }

                        list.AddRange(item.Iots.Where(y => (filter == "" ? y.Type != filter : y.Type == filter)).Select(y => new OptionObjectDTO
                        {
                            Name = y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y
                        }).ToList());
                    }
                }

            }
            return list;
        }

        public List<OptionObjectDTO> GetAnalyticsName(string type)
        {
            var list = new List<OptionObjectDTO>();
            //if (_catalog.Sites == null)
            //{
            //    _catalog.Sites = Vmon5Service.GetSiteInfo(MainView.UserToken).Sites;
            //}

            if (_catalog != null && _catalog.Sites != null)
            {
                foreach (var item in _catalog.Sites)
                {
                    if (type == "All")
                    {
                        list.AddRange(item.Kpis.Select(y => new OptionObjectDTO
                        {
                            Name = y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y

                        }).ToList());
                        list.AddRange(item.Faces.Select(y => new OptionObjectDTO
                        {
                            Name = y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y

                        }).ToList());
                        list.AddRange(item.Lprs.Select(y => new OptionObjectDTO
                        {
                            Name = y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y

                        }).ToList());
                    }
                    else if (type == "Kpi")
                    {
                        list.AddRange(item.Kpis.Select(y => new OptionObjectDTO
                        {
                            Name = y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y

                        }).ToList());
                    }
                    else if (type == "Face")
                    {
                        list.AddRange(item.Faces.Select(y => new OptionObjectDTO
                        {
                            Name = y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y

                        }).ToList());
                    }
                    else if (type == "Lpr")
                    {
                        list.AddRange(item.Lprs.Select(y => new OptionObjectDTO
                        {
                            Name = y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y

                        }).ToList());
                    }


                }

            }
            return list;
        }

        public List<OptionObjectDTO> GetCarouselsName()
        {
            var list = new List<OptionObjectDTO>();
            if (_catalog?.Carousels == null)
            {
                //_catalog.Carousels = Vmon5Service.GetCarouselInfo(MainView.UserToken).Carousels;
            }

            if (_catalog != null && _catalog.Carousels != null)
            {

                list.AddRange(_catalog.Carousels.Select(y => new OptionObjectDTO
                {
                    Name = y.Name,
                    Key = y.Id.ToString(),
                    Item = y
                }).ToList());
            }
            return list;
        }

        public async Task<List<SidebarGroupElementDTO>> GetCarousels()
        {
            _sidebarElements = new List<SidebarGroupElementDTO>();
            var carousels = await Vmon5Service.GetCarouselInfo(MainView.UserToken);

            if (carousels != null)
            {
                _sidebarElements = carousels.Carousels.Select(x => new SidebarGroupElementDTO
                {
                    ElementId = x.Id,
                    Name = x.Name,
                    DeviceType = ElementType.Carousel,
                    Key = Guid.NewGuid(),
                    DeviceTypeStr = "CAR"
                }).ToList();
            }
            return _sidebarElements;
        }

        public List<SidebarGroupElementDTO> GetAnalytics()
        {
            _sidebarElements = new List<SidebarGroupElementDTO>();
            //if (_catalog.Sites == null)
            //{
            //    _catalog.Sites = Vmon5Service.GetSiteInfo(MainView.UserToken).Sites;
            //}

            if (_catalog != null && _catalog.Sites != null)
            {
                foreach (var item in _catalog.Sites)
                {
                    SidebarGroupElementDTO group = new SidebarGroupElementDTO();
                    group.Name = item.Name;
                    List<SidebarElementDTO> list = new List<SidebarElementDTO>();
                    list.AddRange(item.Kpis.Select(y => new SidebarElementDTO
                    {
                        ElementId = y.ObjectId,
                        GroupName = item.Name,
                        Name = y.Name,
                        DeviceType = ElementType.Kpi,
                        Key = Guid.NewGuid(),
                        SiteId = item.Id,
                        DeviceTypeStr = y.Type,
                        RecorderName = y.Name,
                        RecorderType = RecorderType.EDGE
                    }).ToList());
                    list.AddRange(item.Faces.Select(y => new SidebarElementDTO
                    {
                        ElementId = y.ObjectId,
                        GroupName = item.Name,
                        Name = y.Name,
                        DeviceType = ElementType.Face,
                        Key = Guid.NewGuid(),
                        SiteId = item.Id,
                        DeviceTypeStr = y.Type,
                        RecorderName = y.Name,
                        RecorderType = RecorderType.EDGE

                    }).ToList());
                    list.AddRange(item.Lprs.Select(y => new SidebarElementDTO
                    {
                        ElementId = y.ObjectId,
                        GroupName = item.Name,
                        Name = y.Name,
                        DeviceType = ElementType.Lpr,
                        Key = Guid.NewGuid(),
                        SiteId = item.Id,
                        DeviceTypeStr = y.Type,
                        RecorderName = y.Name,
                        RecorderType = RecorderType.EDGE
                    }).ToList());
                    group.SidebarElements = list;
                    _sidebarElements.Add(group);
                }

            }
            return _sidebarElements;
        }

        public void GoToAlarm()
        {
            _container.JumpToApp(Apps.Alarms);
        }

        public void GoToVisualSearch(SidebarElementDTO selectSidebarElementDTO)
        {
            _container.JumpToApp(Apps.VisualSearch, selectSidebarElementDTO);
        }

        public List<SidebarGroupElementDTO> GetDevices()
        {
            _sidebarElements = new List<SidebarGroupElementDTO>();

            //if (_catalog.Sites == null || _catalog.Sites.Count == 0)
            //{
            //    _catalog.Sites = Vmon5Service.GetSiteInfo(MainView.UserToken).Sites;
            //}

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
                        RecorderName = y.DeviceName,
                        RecorderType = RecorderType.EDGE,
                        ShowDvfId = _appAuthorization.Exist("auth.app.apps.playback.showDVF_ID")

                        /*
                                              ElementId = y.ObjectId,
                                                GroupName = item.Name,
                                                Name = y.Name,
                                                DeviceType = ElementType.Camera,
                                                DeviceTypeStr = y.Type,
                                                Status = DeviceStatus.Online,
                                                Key = Guid.NewGuid(),
                                                SiteId = item.Id
                        */
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
                        RecorderName = y.Name,
                        RecorderType = RecorderType.EDGE,
                        ShowDvfId = _appAuthorization.Exist("auth.app.apps.playback.showDVF_ID")
                    }).ToList());
                    group.SidebarElements = list;
                    _sidebarElements.Add(group);
                }
            }
            return _sidebarElements;
        }

        //public void SaveGroup(ObjectGroupEntity group)
        //{
        //    Vmon5Service.SaveGroup(group, MainView.UserToken);
        //}

        public List<ObjectStateFilter> GetFilterElement()
        {
            // 1. Aquí es donde se agregan nuevas opciones en el filtro de Live.
            return Vmon5Service.GetFilterElement();
        }

        public List<OptionObjectDTO> GetFiltersType()
        {
            return Vmon5Service.GetFiltersType();
        }

        public async Task<List<OptionObjectDTO>> GetFiltersTypeRemove()
        {
            return await Vmon5Service.GetFiltersTypeRemove(MainView.UserToken);
        }

        public async Task<CameraDTO> GetCamera(int elementId)
        {
            return await Vmon5Service.GetCamera(elementId, MainView.UserToken);
        }

        public async Task<ObjectGroupEntity> GetGroup(int id)
        {
            return await Vmon5Service.GetObjectGroup(MainView.UserToken, id);
        }

        public async Task<CatalogCamera> GetCatalogCamera(int siteId, int deviceFeatureId)
        {
            var devices = new List<CatalogCamera>();
            var catalogData = new CatalogDTO();

            _dvfid = deviceFeatureId;
            catalogData = await GetCatalogAsync();            

            if (catalogData != null && catalogData.Sites != null)
            {
                devices = catalogData.Sites.Where(s => s.Id == siteId).First().Cameras.ToList();
            }
            return devices.Find(x => x.ObjectId == deviceFeatureId);
        }

        public List<CatalogIot> GetCatalogIOTs(int sitId, int dvfId)
        {
            if (_catalog?.Sites == null)
                return new List<CatalogIot>();

            var site = _catalog.Sites.FirstOrDefault(x => x.Id == sitId);

            if (site?.Iots == null || !site.Iots.Any() || site.Cameras == null)
                return new List<CatalogIot>();

            var camera = site.Cameras.FirstOrDefault(x => x.ObjectId == dvfId);

            if (camera == null)
                return new List<CatalogIot>();

            return site.Iots.Where(x => x.Type == "DO" && x.DveId == camera.DveId).ToList();
        }

        public async Task<List<CatalogCamera>> GetGeomapElements(int elementId)
        {
            var devices = new List<CatalogCamera>();
            var dataLocation = await GetGeoMap(elementId);
            if (dataLocation != null && dataLocation.Sites != null)
            {
                devices = dataLocation.Sites.Where(s => s.Id == elementId).First().Cameras.ToList();
            }
            return devices;
        }

        public async Task<List<CatalogSite>> GetGeomapCatalog(int SiteId)
        {
            var sites = new List<CatalogSite>();
            var dataLocation = await GetGeoMap(SiteId);
            if (dataLocation != null && dataLocation.Sites != null)
            {
                sites = dataLocation.Sites.Where(s => s.Id == SiteId).ToList();
            }
            return sites;
        }

        public async Task<CatalogDTO> GetGeoMap(int SiteId)
        {
            var dataLocation = new CatalogDTO();
            if (_catalog == null)
                _catalog = new CatalogDTO();


            if (!_catalog.Sites.Exists(s => s.Id == SiteId))
            {
                dataLocation = (await Vmon5Service.GetSiteLocationInfo(MainView.UserToken, SiteId));
                var existingSite = dataLocation.Sites.FirstOrDefault();
                _catalog.Sites.Add(existingSite);
            }
            else
            {
                if (_catalog.Sites.Exists(s => s.Id == SiteId && s.Cameras.Count > 0))
                {
                    dataLocation = _catalog;
                }
                else
                {
                    dataLocation = (await Vmon5Service.GetSiteLocationInfo(MainView.UserToken, SiteId));
                    var existingCamera = dataLocation.Sites.Select(c => c.Cameras).FirstOrDefault();
                    _catalog.Sites.FirstOrDefault(s => s.Id == SiteId).Cameras.Add(existingCamera);
                }

            }
            return dataLocation;
        }

        public async Task<List<AlarmGeoMapDTO>> GetAlarmsGeoMap(int siteId)
        {
            var alarms = new List<AlarmGeoMapDTO>();
            alarms = await Vmon5Service.GetAlarmsGeoMap(MainView.UserToken, siteId);
            return alarms;
        }

        public KpiDTO GetKpi(int elementId)
        {
            var token = MainView.UserToken;
            var kpi = new KpiDTO();
            //if (_catalog.Sites == null)
            //{
            //    _catalog.Sites = Vmon5Service.GetSiteInfo(token).Sites;
            //}

            if (_catalog != null && _catalog.Sites != null)
            {
                foreach (var item in _catalog.Sites)
                {
                    var find = item.Kpis.Find(x => x.ObjectId == elementId);
                    if (find != null)
                    {
                        kpi.ObjectId = find.ObjectId;
                        kpi.SiteId = item.Id;
                        kpi.Type = find.PrdCode;
                        kpi.Token = token;
                        break;
                    }
                }
            }
            return kpi;
        }

        public void SetName(Apps app)
        {
            if (_container.AppsActives.ContainsKey(Parent) == false)
            {
                _container.AppsActives.Add(Parent, app);
            }

            var n = _container.AppsActives.Where(x => x.Value == app).Count();
            MainView.ApplicationTitle = n > 1 ? $"{Resources.WelcomeLive} {n}" : Resources.WelcomeLive;
        }

        public async Task<List<CarouselDTO>> GetCarouselElements(int elementId)
        {
            List<CarouselDTO> sidebarElements = new List<CarouselDTO>();
            if (_catalog == null)
                _catalog = new CatalogDTO();
            if (_catalog.Carousels == null)
            {
                _catalog.Carousels = (await Vmon5Service.GetCarouselInfo(MainView.UserToken, elementId)).Carousels;
            }
            else if(!_catalog.Carousels.Exists(s => s.Id == elementId))
            {
                _catalog.Carousels = (await Vmon5Service.GetCarouselInfo(MainView.UserToken, elementId)).Carousels;
            }

            if (_catalog != null && _catalog.Carousels != null)
            {
                var carousel = _catalog.Carousels.Where(s => s.Id == elementId).FirstOrDefault();
                var devices = carousel == default ? new List<CatalogObjectGroupElement>() : carousel.Elements.ToList();

                foreach (var d in devices)
                {
                    ElementType type = ElementType.None;
                    string name = "";
                    object data = null;
                    _dvfid = d.Element.Id;
                    var dataCatalog = await this.GetCatalogAsync();
                    switch (d.Element.Type)
                    {
                        case "VIN":
                            type = ElementType.Camera;
                            name = dataCatalog.Sites.Find(y => y.Id == d.Element.SiteId).Cameras.Find(x => x.ObjectId == d.Element.Id).Name;
                            data = GetCamera(d.Element.Id);
                            break;
                        case "DI":
                            type = ElementType.Iot_In;
                            name = dataCatalog.Sites.Find(y => y.Id == d.Element.SiteId).Iots.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "DO":
                            type = ElementType.Iot_Out;
                            name = dataCatalog.Sites.Find(y => y.Id == d.Element.SiteId).Iots.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "LPR":
                            type = ElementType.Lpr;
                            name = dataCatalog.Sites.Find(y => y.Id == d.Element.SiteId).Lprs.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "FACE":
                            type = ElementType.Face;
                            name = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Faces.Find(x => x.ObjectId == d.Element.Id).Name;
                            data = GetFaceElements(d.Element.Id);
                            break;
                        case "KPI":
                            type = ElementType.Kpi;
                            name = dataCatalog.Sites.Find(y => y.Id == d.Element.SiteId).Kpis.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "MAP":
                            type = ElementType.Blueprint;
                            name = dataCatalog.Sites.Find(y => y.Id == d.Element.SiteId).Locations.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "GEO":
                            type = ElementType.Geomap;
                            name = dataCatalog.Sites.Find(y => y.Id == d.Element.SiteId).Name;
                            data = GetGeomapElements(d.Element.Id);
                            break;
                        default:
                            type = ElementType.None;
                            name = "";
                            break;

                    }
                    await this._auditService.Add(new AuditDTO()
                    {
                        CodeAction = AuditAction.VIEW_CAROUSELS.ToString(),
                        Param01 = string.IsNullOrEmpty(name) ? "" : d.Element.Id.ToString()
                    }, MainView.UserToken);

                    int.TryParse(d.Time, out int time);
                    if (d.Element.Type != "GROUP")
                    {
                        sidebarElements.Add(new CarouselDTO
                        {
                            ElementId = d.Element.Id,
                            Name = name,
                            GroupName = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Name,
                            DeviceType = type,
                            Key = Guid.NewGuid(),
                            Status = DeviceStatus.Online,
                            Time = time,
                            Data = data
                        });
                    }
                }
            }
            return sidebarElements;
        }

        public async Task<List<CarouselDTO>> GetCarouselElements(List<int> elementIds)
        {
            List<CarouselDTO> sidebarElements = new List<CarouselDTO>();
            if (_catalog.Carousels == null)
            {
                _catalog.Carousels = (await Vmon5Service.GetCarouselInfo(MainView.UserToken)).Carousels;
            }

            if (_catalog != null && _catalog.Carousels != null)
            {
                var carousel = _catalog.Carousels.Where(s => elementIds.Contains(s.Id)).FirstOrDefault();
                if (carousel == null)
                    return sidebarElements;

                var devices = carousel.Elements.ToList();

                foreach (var d in devices)
                {
                    ElementType type = ElementType.None;
                    string name = "";
                    object data = null;
                    switch (d.Element.Type)
                    {
                        case "VIN":
                            type = ElementType.Camera;
                            name = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Cameras.Find(x => x.ObjectId == d.Element.Id).Name;
                            data = GetCamera(d.Element.Id);
                            break;
                        case "DI":
                            type = ElementType.Iot_In;
                            name = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Iots.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "DO":
                            type = ElementType.Iot_Out;
                            name = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Iots.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "LPR":
                            type = ElementType.Lpr;
                            name = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Lprs.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "FACE":
                            type = ElementType.Face;
                            name = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Faces.Find(x => x.ObjectId == d.Element.Id).Name;
                            data = GetFaceElements(d.Element.Id);
                            break;
                        case "KPI":
                            type = ElementType.Kpi;
                            name = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Kpis.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "MAP":
                            type = ElementType.Blueprint;
                            name = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Locations.Find(x => x.ObjectId == d.Element.Id).Name;
                            break;
                        case "GEO":
                            type = ElementType.Geomap;
                            name = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Name;
                            data = GetGeomapElements(d.Element.Id);
                            break;
                        default:
                            type = ElementType.None;
                            name = "";
                            break;

                    }
                    await _auditService.Add(new AuditDTO()
                    {
                        CodeAction = AuditAction.VIEW_CAROUSELS.ToString(),
                        Param01 = string.IsNullOrEmpty(name) ? "" : d.Element.Id.ToString()
                    }, MainView.UserToken).ConfigureAwait(false);

                    int.TryParse(d.Time, out int time);
                    sidebarElements.Add(new CarouselDTO
                    {
                        ElementId = d.Element.Id,
                        Name = name,
                        GroupName = _catalog.Sites.Find(y => y.Id == d.Element.SiteId).Name,
                        DeviceType = type,
                        Key = Guid.NewGuid(),
                        Status = DeviceStatus.Online,
                        Time = time,
                        Data = data
                    });
                }
            }
            return sidebarElements;
        }
        public async Task<List<FaceAlarmsDTO>> GetFaceElements(int elementId)
        {
            return await Vmon5Service.GetFacesAlarms(elementId, MainView.UserToken);
        }

        public async Task<List<LprAlarmDTO>> GetLprElements(int elementId)
        {
            return await Vmon5Service.GetLprAlarms(elementId, MainView.UserToken);
        }

        public async Task<LprSnapshotDTO> GetLprSnapshot(int elementId)
        {
            return await Vmon5Service.GetLprSnapshot(elementId, MainView.UserToken);
        }

        public void ShowSpinner(bool show)
        {
            MainView.Spinner = show;
        }

        public async Task<List<CatalogSite>> GetSite()
        {
            return (await this.GetCatalogAsync()).Sites;
        }

        public async Task<BlueprintDTO> GetBlueprint(int id)
        {
            return await Vmon5Service.GetBlueprint(id, MainView.UserToken);
        }

        public void SendNotificationIot(IotDTO request)
        {
            this._iotService.Update(request, MainView.UserToken);
        }

        public void Dispose()
        {
            if (_signal != null)
            {
                _signal.IotStatusEventAction -= IotStatusEvent;
            }

            this._sidebarElements.Clear();
        }

        public async Task<bool> HasRecorders(int id)
        {
            var CatalogData = new CatalogDTO();
            _dvfid = id;
            CatalogData = await this.GetCatalogAsync();
            var cameras = CatalogData.Sites.SelectMany(x => x.Cameras).Distinct().ToList();
            var camera = cameras.Find(x => x.ObjectId == id);
            if (camera != null)
                return camera.EdgeEnabled.Value || camera.Recorders.Any();
            else
                return false;

        }
        public async Task<AlarmDTO> GetAlarm(int idAlarm)
        {
            return await this._alarmService.Get(MainView.UserToken, idAlarm);
        }

        public async Task<DeviceFeatureDTO> GetDeviceFeature(int deviceFeatureId)
        {
            return await this._alarmService.GetDeviceFeatures(MainView.UserToken, deviceFeatureId);
        }
        public async Task<List<CatalogObjectGroupElement>> getCarouselGroup(int elementId)
        {
            if (_catalog == null)
                _catalog = new CatalogDTO();

            _catalog.Carousels =(await Vmon5Service.GetCarouselInfo(MainView.UserToken, elementId)).Carousels;
            var devicesCarousel = _catalog.Carousels.Where(s => s.Id == elementId).First().Elements.ToList();
            var carouselGroup = devicesCarousel.Where(c => c.Element.Type == "GROUP").ToList();
            return carouselGroup;
        }

        public void ShowToolbarButtonHiddenIcon()
        {
            _container.ShowToolbarHidenButton(MainView.ID);
        }

        public async Task<List<OptionObjectDTO>> GetSiteDevicesName(int siteid, int page, int take,SidebarElement type, string[] filter, string search)
        {
            var list = new List<OptionObjectDTO>();
            var _sitesDevices = await Vmon5Service.GetSiteDevices(MainView.UserToken, siteid, page, take, (int)type, filter, search);

            if (_sitesDevices != null)
            {
                foreach (var item in _sitesDevices.Elements)
                {
                    //if (type == "All")
                    //{
                        /* Solo en "Camaras", tienen este dato de DeviceName. Los demás, no lo tienen. */
                        list.Add(new OptionObjectDTO
                        {
                            /* Nombre del dispositivo - Canal de video. => Anterior: Name = (y.Name != y.DeviceName) ? y.Name : y.Name  + " - " + y.DeviceName, */
                            Name = item.Name,
                            Key = item.ElementId.ToString(),
                            Tag = item.Key,
                            Item = item,
                            count = _sitesDevices.Count
                        });
                    //}
                }

            }
            return list;
        }


        public async Task<List<OptionObjectDTO>> GetSitesName(int page, int take, int typeElement, int userId, GroupType typeGroup, string filter, string search = "")
        {
            var list = new List<OptionObjectDTO>();
            var _sites = await Vmon5Service.GetSiteCatalog(MainView.UserToken, page, take, typeElement, userId, typeGroup, filter, search );


            if (_sites != null)
            {
                foreach (var item in _sites.Elements)
                {
                    List<OptionObjectDTO> listSite = new List<OptionObjectDTO>
                    {
                     new OptionObjectDTO()
                     {
                         Name = item.Name,
                         Key = item.Id.ToString(),
                         Tag = item.Id,
                         Item = item,
                         count = _sites.Count
                     }
                    };
                    list.Add(listSite);
                }

            }
            return list;
        }

        public async Task<List<OptionObjectDTO>> GetScenesIots(int idSite, string type, int take, int page, string search)
        {
            var list = new List<OptionObjectDTO>();

            var lsIots = await Vmon5Service.GetSceneIots(idSite, type, MainView.UserToken, take, page, search);

            if (lsIots != null)
            {
                foreach (var item in lsIots)
                {
                    var mapper = new OptionObjectDTO();
                    mapper.Name = item.Name;
                    mapper.Key = item.ObjectId.ToString();
                    mapper.Item = item;
                    mapper.count = item.count;
                    mapper.IsPtz = item.IsPtz;
                    list.Add(mapper);
                }
            }

                return list;
        }
    }
}
