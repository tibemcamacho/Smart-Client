using DynamicData;
using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Properties;
using Elipgo.SmartClient.Common.Reflections;
using Elipgo.SmartClient.Drivers;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.Services.Services.Interface;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using static Elipgo.SmartClient.Common.Enum.Enums;
using static Elipgo.SmartClient.Common.Enum.TypeAlarms;

namespace Elipgo.SmartClient.ViewModels
{
    public class PlaybackViewModel : ReactiveObject, IRoutableViewModel, IDisposable
    {
        private readonly IAppContainer _container = Locator.Current.GetService<IAppContainer>();
        private readonly IDriverFactory _driverFactory = Locator.Current.GetService<IDriverFactory>();
        private readonly IAppAuthorization _appAuthorization = Locator.Current.GetService<IAppAuthorization>();
        private string _viewTitle;
        private CatalogDTO _catalog = null;
        private Guid _parent = Guid.Empty;
        private List<SidebarGroupElementDTO> _sidebarElements = new List<SidebarGroupElementDTO>();

        public LiveViewModel ViewModel { get; set; }
        public MainViewModel MainView { get; set; } = null;

        //public CatalogDTO Catalog
        //{
        //    get => this._catalog;
        //    set => this.RaiseAndSetIfChanged(ref this._catalog, value);
        //}

        public Guid Parent
        {
            get => this._parent;
            set => this.RaiseAndSetIfChanged(ref this._parent, value);
        }

        public PlaybackViewModel()
        {
            ViewTitle = "Playback Mode";
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
            MainView.ApplicationTitle = n > 1 ? $"{Resources.WelcomePlayback} {n}" : Resources.WelcomePlayback;
        }

        public void ShowSpinner(bool show)
        {
            MainView.Spinner = show;
        }

        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment { get; protected set; }


        //public List<OptionObjectDTO> GetAllSites(string typeObject, string classObject)
        //{
        //    var list = new List<OptionObjectDTO>();                 /* Objeto devuelto. */
        //    var listAll = new List<OptionObjectDTO>();              /* Lista temporal para recolectar los dispositivos. */

        //    /* Cargamos primero los dispositivos para agruparlos por Id de sitio. */
        //    switch (typeObject)
        //    {
        //        case "Locations":
        //            listAll = GetLocationsName().GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
        //            break;
        //        case "Devices":
        //            listAll = GetDevicesName(classObject).GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
        //            break;
        //        case "Analytics":
        //            listAll = GetAnalyticsName(classObject).GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
        //            break;
        //        case "Carousels":
        //            listAll = GetCarouselsName().GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
        //            break;
        //        default:
        //            listAll = GetDevicesName(classObject).GroupBy(u => u.Tag).Select(x => x.First()).OrderByDescending(u => u.Tag).ToList();
        //            break;
        //    }

        //    /* Después, por tipo de objeto, filtramos las sucursales. */
        //    if (listAll.Count > 0)
        //    {
        //        var listSitesStr = listAll.Select(u => u.Tag).ToList();

        //        /* Genero la lista de sucursales. */
        //        var AllSites = this.Catalog.Sites.Select(u => new { u.Id, u.Name }).Where(u => listSitesStr.Contains(u.Id)).ToList();

        //        /* Cargo las sucursales finales. */
        //        if (AllSites != null | AllSites.Count > 0)
        //        {
        //            /* Limpio la lista. */
        //            list.Clear();
        //            foreach (var item in AllSites)
        //            {
        //                list.Add(new OptionObjectDTO { Name = item.Name, Key = item.Id.ToString(), Tag = item.Id.ToString() });
        //            }
        //        }
        //        else
        //        {
        //            /* Lanzo la excepción. */
        //            throw new ArgumentNullException();
        //        }
        //    }
        //    else
        //    {
        //        /* Lanzo la excepción. */
        //        //throw new ArgumentNullException();
        //        Logger.Log("Sites not founds");
        //        return new List<OptionObjectDTO>();
        //    }

        //    /* Devuelvo el resultado final. */
        //    return list;
        //}

        //public void GoToAlarm()
        //{
        //    _container.JumpToApp(Apps.Alarms);
        //}

        public void GoToVisualSearch(SidebarElementDTO selectSidebarElement, DateTime? selectedDateTime)
        {
            _container.JumpToApp(Apps.VisualSearch, selectSidebarElement, selectedDateTime);
        }

        //public List<SidebarGroupElementDTO> GetDevices()
        //{
        //    _sidebarElements = new List<SidebarGroupElementDTO>();
        //    if (_catalog == null)
        //        _catalog = new CatalogDTO();

        //    if (_catalog.Sites == null)
        //    {
        //        _catalog.Sites = Vmon5Service.GetSiteInfo(MainView.UserToken).Sites;
        //    }

        //    if (_catalog != null && _catalog.Sites != null)
        //    {
        //        foreach (var item in _catalog.Sites)
        //        {
        //            SidebarGroupElementDTO group = new SidebarGroupElementDTO
        //            {
        //                Name = item.Name,
        //                ElementId = item.Id
        //            };

        //            List<SidebarElementDTO> list = new List<SidebarElementDTO>();

        //            foreach (var catalogCamera in item.Cameras)
        //            {
        //                if (catalogCamera.EdgeEnabled == true)
        //                {
        //                    list.Add(new SidebarElementDTO
        //                    {
        //                        ElementId = catalogCamera.ObjectId,
        //                        GroupName = item.Name,
        //                        Name = catalogCamera.Name,
        //                        DeviceType = ElementType.Camera,
        //                        DeviceTypeStr = catalogCamera.Type,
        //                        Status = DeviceStatus.Online,
        //                        Key = Guid.NewGuid(),
        //                        SiteId = item.Id,
        //                        RecorderName = catalogCamera.DeviceName,
        //                        RecorderType = RecorderType.EDGE,
        //                        ShowDvfId = _appAuthorization.Exist("auth.app.apps.playback.showDVF_ID")
        //                    });
        //                }

        //                foreach (var catalogRecorder in catalogCamera.Recorders)
        //                {
        //                    list.Add(new SidebarElementDTO
        //                    {
        //                        ElementId = catalogCamera.ObjectId,
        //                        GroupName = item.Name,
        //                        Name = catalogCamera.Name,
        //                        DeviceType = ElementType.Camera,
        //                        DeviceTypeStr = catalogCamera.Type,
        //                        Status = DeviceStatus.Online,
        //                        Key = Guid.NewGuid(),
        //                        SiteId = item.Id,
        //                        RecorderName = catalogRecorder.Name,
        //                        RecorderType = RecorderType.VREC,
        //                        RecorderId = catalogRecorder.Id,
        //                        RecorderDriver = catalogRecorder.Driver,
        //                        ShowDvfId = _appAuthorization.Exist("auth.app.apps.playback.showDVF_ID")
        //                    });
        //                }

        //                /*
        //                    if (catalogCamera.EdgeEnabled == true && appAuthorization.Exist("auth.app.apps.playback.showRecorder.Edge")) 
        //                    {
        //                        list.Add(new SidebarElementDTO
        //                        {
        //                            ElementId = catalogCamera.ObjectId,
        //                            GroupName = item.Name,
        //                            Name = catalogCamera.Name,
        //                            DeviceType = ElementType.Camera,
        //                            DeviceTypeStr = catalogCamera.Type,
        //                            Status = DeviceStatus.Online,
        //                            Key = Guid.NewGuid(),
        //                            SiteId = item.Id,
        //                            RecorderName = catalogCamera.DeviceName,
        //                            RecorderType = RecorderType.EDGE,
        //                            ShowDvfId = appAuthorization.Exist("auth.app.apps.playback.showDVF_ID")
        //                        });
        //                    }
        //                    if (appAuthorization.Exist("auth.app.apps.playback.showRecorder.Vrec"))
        //                    {
        //                        foreach (var catalogRecorder in catalogCamera.Recorders)
        //                        {
        //                            list.Add(new SidebarElementDTO
        //                            {
        //                                ElementId = catalogCamera.ObjectId,
        //                                GroupName = item.Name,
        //                                Name = catalogCamera.Name,
        //                                DeviceType = ElementType.Camera,
        //                                DeviceTypeStr = catalogCamera.Type,
        //                                Status = DeviceStatus.Online,
        //                                Key = Guid.NewGuid(),
        //                                SiteId = item.Id,
        //                                RecorderName = catalogRecorder.Name,
        //                                RecorderType = RecorderType.VREC,
        //                                RecorderId = catalogRecorder.Id,
        //                                RecorderDriver = catalogRecorder.Driver,
        //                                ShowDvfId = appAuthorization.Exist("auth.app.apps.playback.showDVF_ID")
        //                            });
        //                        }
        //                    }
        //                */
        //            }

        //            group.SidebarElements = list;
        //            _sidebarElements.Add(group);
        //        }
        //    }
        //    return _sidebarElements;
        //}

        public async Task<object> GetCamera(int elementId)
        {
            return await Vmon5Service.GetCamera(elementId, MainView.UserToken);
        }

        public List<ObjectStateFilter> GetFilterElement()
        {
            return Vmon5Service.GetFilterElement();
        }

        public List<OptionObjectDTO> GetFiltersType()
        {
            List<OptionObjectDTO> originFiltersType = Vmon5Service.GetFiltersType();

            return originFiltersType.Where(o => o.Key == "Devices").ToList();
        }

        public async Task<List<OptionObjectDTO>> GetSitesName(int page, int take, int typeElement, int userId, GroupType typeGroup, string filter, string search = "")
        {
            var list = new List<OptionObjectDTO>();
            var _sites = await Vmon5Service.GetSiteCatalog(MainView.UserToken, page, take, typeElement, userId, typeGroup, filter, search);


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
                     }
                    };
                    list.Add(listSite);
                }

            }
            return list;
        }

        public List<OptionObjectDTO> GetDevicesName(string type)
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
                        list.AddRange(item.Cameras.Select(y => new OptionObjectDTO
                        {
                            /* Nombre del dispositivo - Canal de video. */
                            Name = (y.Name != y.DeviceName) ? y.DeviceName + " - " + y.Name : y.Name,
                            Key = y.ObjectId.ToString(),
                            Tag = item.Id,
                            Item = y
                        }).ToList());


                        list.AddRange(item.Iots.Select(y => new OptionObjectDTO
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
                            /* Nombre del dispositivo - Canal de video. */
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

        public List<OptionObjectDTO> GetLocationsName()
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
            //if ( _catalog.Carousels == null)
            //{
            //    _catalog.Carousels = Vmon5Service.GetCarouselInfo(MainView.UserToken).Carousels;
            //}

            if (_catalog != null && _catalog.Carousels != null)
            {
                /* No tiene el atributo "DeviceName". */
                list.AddRange(_catalog.Carousels.Select(y => new OptionObjectDTO
                {
                    Name = y.Name,
                    Key = y.Id.ToString(),
                    Item = y
                }).ToList());
            }
            return list;
        }

        public Element GetElement(TypesFilters type, int idElement)
        {
            var list = new List<OptionObjectDTO>();
            ViewModel._dvfid = idElement;
            //if (_catalog.Sites == null)
            //{
            //    _catalog.Sites = Vmon5Service.GetSiteInfo(MainView.UserToken).Sites;
            //}

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

        public async Task<List<TimelineDTO>> GetTimeLine(int cameraId, string startDate, string endDate, int recorderId = 0)
        {
            return await Vmon5Service.GetTimeLine(cameraId, recorderId, startDate, endDate, MainView.UserToken);
        }

        public List<TimelineDTO> GetTimeLineSC(CameraDTO device, string startDate, string endDate, bool dts, int recorderId = 0)
        {
            List<TimelineDTO> timeline = new List<TimelineDTO>();
            try
            {
                if (device != null)
                {
                    device.RecorderId = recorderId;
                    IManufactureUri driver = _driverFactory.GetDriverApiCgi(device, recorderId);
                    timeline = (List<TimelineDTO>)driver.GetTimeline(startDate, endDate, dts);
                    TrimTimelineToCurrentTime(timeline, startDate);
                }
            }
            catch (Exception ex)
            {
                Logger.Log("GetTimeLineSC Exception: " + ex.Message, LogPriority.Fatal);
            }
            return timeline;
        }

        private void TrimTimelineToCurrentTime(List<TimelineDTO> list, string startDateStr)
        {
            if (list == null || !DateTimeOffset.TryParse(startDateStr, out var rangeStartOffset))
                return;

            DateTime rangeEnd = DateTime.Now;
            DateTime rangeStart = rangeStartOffset.DateTime;

            // Iterate backwards to allow safe removal of items without shifting indices.
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];

                if (DateTimeOffset.TryParse(item.StartTime, out var startOffset) &&
                    DateTimeOffset.TryParse(item.EndTime, out var endOffset))
                {
                    DateTime itemStart = startOffset.DateTime;
                    DateTime itemEnd = endOffset.DateTime;

                    if (itemEnd <= rangeStart || itemStart >= rangeEnd)
                    {
                        list.RemoveAt(i);
                        continue;
                    }

                    // Start time trim: Adjust if the item starts before the allowed range.
                    if (itemStart < rangeStart)
                    {
                        item.StartTime = rangeStart.ToString("yyyy-MM-ddTHH:mm:ss");
                    }

                    // End time trim: Adjust if the item extends beyond the current time.
                    if (itemEnd > rangeEnd)
                    {
                        item.EndTime = rangeEnd.ToString("yyyy-MM-ddTHH:mm:ss");
                    }
                }
                else
                {
                    list.RemoveAt(i);
                }
            }
        }

        public void Dispose()
        {
            this._sidebarElements.Clear();
        }

        public void ShowToolbarButtonHiddenIcon()
        {
            _container.ShowToolbarHidenButton(MainView.ID);
        }
        public async Task<List<OptionObjectDTO>> GetSiteDevicesName(int siteid, int page, int take, SidebarElement type, string[] filter, string search)
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
    }
}
