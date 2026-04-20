using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Services;
using Elipgo.SmartClient.Services.Services.Interface;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Elipgo.SmartClient.Common.Enum.TypeAlarms;

namespace Elipgo.SmartClient.ViewModels
{
    public class GroupsViewModel : ReactiveObject, IGenericViewModel
    {
        public long UserId { get; set; }
        public string UserIdGuid { get; set; }
        public string Token { get; set; }
        public long EntityId { get; set; }
        public CatalogDTO Catalog
        {
            get
            {
                return _catalog = _catalogService.CurrentCatalog;
            }
            set
            {
                _catalogService.CurrentCatalog = value;
                this.RaiseAndSetIfChanged(ref _catalog, value); // Solo si necesitas disparar eventos UI
            }
        }
        public System.Resources.ResourceManager Resource { get; set; }
        private readonly ICatalogService _catalogService = Locator.Current.GetService<ICatalogService>();
        private CatalogDTO _catalog = new CatalogDTO();
        public int _dvfid = 0;
        private bool _useMultipleIds = false;
        public List<int> _dvfIds = new List<int>();
        //private LiveViewModel _viewModelLive;

        public async Task<CatalogDTO> GetCatalogAsync()
        {
            if (_useMultipleIds && _dvfIds != null && _dvfIds.Count > 0)
            {
                _catalog = await _catalogService.GetValueCatalog(Token, _dvfIds);
            }
            else
            {
                _catalog = await _catalogService.GetValueCatalog(Token, _dvfid);
            }

            return _catalog;
        }


        private ISmartNotification notification = Locator.Current.GetService<ISmartNotification>();
        public static IDictionary<string, decimal> Duration = new Dictionary<string, decimal>()
        {
            {"15 SEG.",15},
            {"20 SEG.",20},
            {"30 SEG.",30},
            {"40 SEG.",40},
            {"50 SEG.",50},
            {"1 MIN.",60},
            {"2 MIN.",120}
        };
        public async Task<ObjectGroupEntity> SaveOrUpdate(ObjectGroupEntity element)
        {
            if (element.Id == 0)
                return await Vmon5Service.SaveGroup(element, Token);
            else
                return await Vmon5Service.UpdateGroup(element, Token);
        }
        public async Task<List<ObjectGroupEntity>> GetObjectGroup(int userId, int type)
        {
            return await Vmon5Service.GetObjectsGroup(Token, userId, type);
        }

        public async Task<List<CatalogObjectGroup>> GetsCarousel()
        {
            return await Vmon5Service.GetsCarousel(Token);
        }

        public CatalogObjectGroup GetsCarouselObject(ObjectGroupEntity obj)
        {
            var catalog = new CatalogObjectGroup
            {
                GridId = "",
                Id = obj.Id,
                IsPrivate = obj.IsPrivate,
                Name = obj.Name,
                Type = obj.Type,
                Elements = new List<CatalogObjectGroupElement>()
            };
            obj.Elements?.ToList().ForEach(p => catalog.Elements.Add(new CatalogObjectGroupElement
            {
                Id = p.Id,
                Element = new Element
                {
                    Id = p.ObjectId,
                    SiteId = p.SiteId,
                    Type = p.Type
                },
                Time = p.Time.ToString()
            }));

            return catalog;
        }

        public async Task<ObjectGroupEntity> GetGroup(int id)
        {
            return await Vmon5Service.GetObjectGroup(Token, id);
        }

        public async Task<DataViewGroup> GetElement(ObjectGroupElementEntity obj)
        {
            var item = Vmon5Service.GetConfigGroupByType().FirstOrDefault(p => p.Type == obj.Type);

            if (item != null)//&& Catalog != null
            {
                if (obj.Type == "Alarms_Map" || obj.Type == "AlarmsMap")
                {
                    var site = Catalog.Sites.FirstOrDefault(p => p.Id == obj.SiteId);
                    if (site == null)               
                    {
                        return null;
                    }

                    return new DataViewGroup
                    {
                        Id = obj.SiteId.ToString(),
                        item = site,
                        nameObject = site.Name,
                        type = item.Type,
                        objectTitle = Resource.GetString(item.Group),
                        siteId = obj.SiteId,
                    };
                }
                else if (obj.Type == "MAP")
                {
                    var _catalog = await Vmon5Service.GetSiteLocationInfo(Token, obj.SiteId);
                    var site = _catalog?.Sites.FirstOrDefault(s => s.Id == obj.SiteId);
                    if (site == null)
                    {
                        return null;
                    }
                    var itemCat = site.Locations.FirstOrDefault(p => p.ObjectId == obj.ObjectId);

                    if (itemCat != null)
                    {
                        return new DataViewGroup
                        {
                            Id = obj.Id.ToString(),
                            item = itemCat,
                            nameObject = itemCat.Name,
                            type = Resource.GetString(item.TypeName),
                            objectTitle = Resource.GetString(item.Group),
                            siteId = obj.SiteId,
                        };
                    }
                }
                else if (obj.Type == "GEO")
                {
                    // Para GEO, ObjectId = SiteId. Usamos el sitio del catálogo.
                    var site = Catalog?.Sites.FirstOrDefault(s => s.Id == obj.SiteId);
                    if (site == null)
                    {
                        // Si no está en catálogo, intentar cargarlo
                        var _catalog = await Vmon5Service.GetSiteLocationInfo(Token, obj.SiteId);
                        site = _catalog?.Sites.FirstOrDefault(s => s.Id == obj.SiteId);
                    }
                    if (site != null)
                    {
                        return new DataViewGroup
                        {
                            Id = obj.Id.ToString(),
                            item = site,
                            nameObject = site.Name,
                            type = Resource.GetString(item.TypeName),
                            objectTitle = Resource.GetString(item.Group),
                            siteId = obj.SiteId,
                        };
                    }
                }
                else if (obj.Type != "CAR")
                {
                    //_viewModelLive._dvfid = obj.ObjectId;
                    var _catalog = await Vmon5Service.GetSiteInfo(Token, obj.ObjectId);
                    //GetSiteInfo
                    var site = _catalog.Sites.Where(s => s.Id == obj.SiteId).FirstOrDefault();
                    if (site == null)
                    {
                        return null;
                    }
                    try
                    {

                        var itemCat2 = site.Cameras.FirstOrDefault(p => p.ObjectId == obj.ObjectId && p.Type == obj.Type);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    //var list = (site.GetType().InvokeMember(item.PropertyListName, System.Reflection.BindingFlags.GetProperty, null, site, null) as System.Collections.IList).Cast<IElementGroup>();
                    var itemCat = site.Cameras.FirstOrDefault(p => p.ObjectId == obj.ObjectId && p.Type == obj.Type);

                    if (itemCat != null)
                    {
                        return new DataViewGroup
                        {
                            Id = obj.Id.ToString(),
                            item = itemCat,
                            nameObject = itemCat.Name,
                            type = Resource.GetString(item.TypeName),
                            objectTitle = Resource.GetString(item.Group),
                            siteId = obj.SiteId,
                            streamProfile = GetProfilesStreamValue(obj.ProfileStream)
                        };
                    }
                }
                else
                {
                    var itemCat = Catalog.Carousels.FirstOrDefault(p => p.Id == obj.ObjectId);

                    if (itemCat != null)
                    {
                        return new DataViewGroup
                        {
                            Id = obj.Id.ToString(),
                            item = itemCat,
                            nameObject = itemCat.Name,
                            type = ElementType.Carousel.ToString(),
                            objectTitle = Resource.GetString(item.Group),
                            streamProfile = GetProfilesStreamValue(obj.ProfileStream)
                        };
                    }
                }
            }
            return null;
        }

        public async Task<DataViewCarousel> GetElementCarousel(ObjectGroupElementEntity obj)
        {
            var item = Vmon5Service.GetConfigGroupByType().FirstOrDefault(p => p.Type == obj.Type);

            if (item != null && Catalog != null)
            {
                if (obj.Type != "CAR")
                {
                    _dvfid = obj.ObjectId;
                    var dataCatalog = await this.GetCatalogAsync();
                    var site = Catalog.Sites.FirstOrDefault(p => p.Id == obj.SiteId);

                    var list = (site.GetType()
                                    .InvokeMember(item.PropertyListName, System.Reflection.BindingFlags.GetProperty, null, site, null) as System.Collections.IList)
                                    .Cast<IElementGroup>();
                    var itemCat = list?.FirstOrDefault(p => p.ObjectId == obj.ObjectId && p.Type == obj.Type);

                    if (itemCat != null)
                    {
                        return new DataViewCarousel
                        {
                            Id =  obj.ObjectId,
                            Item = itemCat,
                            DeviceName = itemCat.Name,
                            Type = Resource.GetString(item.TypeName),
                            SiteId = obj.SiteId,
                            SiteStr = site.Name,
                            Duration = "15",
                            StreamProfile = GetProfilesStreamValue(obj.ProfileStream)
                        };
                    }
                }
            }
            return null;
        }

        public async Task<List<DataViewCarousel>> GetElementCarousel(List<ObjectGroupElementEntity> objs)
        {
            var result = new List<DataViewCarousel>();

            foreach (var obj in objs)
            {
                if (obj.Type == "GROUP")
                {
                    //var cam = (site.Cameras ?? new List<CatalogCamera>()).FirstOrDefault(c => c.ObjectId == obj.ObjectId);
                    var group = await GetGroup((int)obj.ObjectId);

                    if (group != null)
                        result.Add(
                            new DataViewCarousel
                            {
                                Id = obj.Id,
                                Item = group.Id,
                                DeviceName = group.Name,
                                Type = obj.Type,
                                Duration = obj.Time == 0 ? "15" : obj.Time.ToString(),
                                SiteId = obj.SiteId,
                                SiteStr = String.Empty,
                                StreamProfile = GetProfilesStreamValue(obj.ProfileStream)
                            });
                }
                else
                {
                    //var site = Catalog.Sites.FirstOrDefault(p => p.Id == obj.SiteId);
                    var _catalog = await Vmon5Service.GetSiteInfo(Token, obj.ObjectId);
                    var site =  _catalog.Sites.Where(s => s.Id == obj.SiteId).FirstOrDefault();

                    if (site == null)
                        continue;

                    var cam = (site.Cameras ?? new List<CatalogCamera>()).FirstOrDefault(c => c.ObjectId == obj.ObjectId);
                    if (cam != null)
                        result.Add(
                            new DataViewCarousel
                            {
                                Id = obj.Id,
                                Item = cam,
                                DeviceName = cam.Name,
                                Type = obj.Type,
                                Duration = obj.Time == 0 ? "15" : obj.Time.ToString(),
                                SiteId = obj.SiteId,
                                SiteStr = site.Name,
                                StreamProfile = GetProfilesStreamValue(obj.ProfileStream)
                            });
                }
            }

            return result;
        }

        public async Task<SidebarElementDTO> GetSidebarElementDTOAsync(ObjectGroupElementEntity obj)
        {
            var item = Vmon5Service.GetConfigGroupByType().FirstOrDefault(p => p.Type == obj.Type);

            if (item != null && Catalog != null)
            {
                CatalogDTO _catalog;
                if (obj.Type == "MAP" || obj.Type == "GEO")
                {
                    // Usar GetSiteLocationInfo con SiteId para obtener Locations/GeoLocations correctamente
                    _catalog = await Vmon5Service.GetSiteLocationInfo(Token, obj.SiteId);
                }
                else
                {
                    _dvfid = obj.ObjectId;
                    _catalog = await this.GetCatalogAsync();
                }
                var site = _catalog?.Sites.FirstOrDefault(p => p.Id == obj.SiteId);
                if (obj.Type != "CAR" && site == null)
                {
                    notification.Show(string.Format(Common.Properties.Resources.SiteNoExist, obj.SiteId), null);
                    return null;
                }

                if (obj.Type != "CAR" && obj.Type != "Alarms_Map" && obj.Type != "AlarmsMap")
                {
                    var list = (site.GetType().InvokeMember(item.PropertyListName, System.Reflection.BindingFlags.GetProperty, null, site, null) as System.Collections.IList)?.Cast<IElementGroup>();
                    var itemCat = list?.FirstOrDefault(p => p.ObjectId == obj.ObjectId);

                    if (itemCat != null)
                    {
                        return new SidebarElementDTO
                        {
                            DeviceType = item.ElementType,
                            ElementId = obj.ObjectId,
                            Name = itemCat.Name,
                            GroupName = site.Name,
                            Key = Guid.NewGuid(),
                            DeviceTypeStr = obj.Type,
                            SiteId = obj.SiteId,
                            ProfileStream = obj.ProfileStream,
                        };
                    }
                }
                else if (obj.Type == "Alarms_Map" || obj.Type == "AlarmsMap")
                {
                    var itemCat = Catalog.Sites.FirstOrDefault(p => p.Id == obj.ObjectId);

                    return new SidebarElementDTO
                    {
                        DeviceType = item.ElementType,
                        ElementId = obj.ObjectId,
                        Name = itemCat.Name,
                        GroupName = site.Name,
                        Key = Guid.NewGuid(),
                        DeviceTypeStr = obj.Type,
                        SiteId = obj.SiteId,
                    };
                }
                else
                {
                    var itemCat = Catalog.Carousels.FirstOrDefault(p => p.Id == obj.ObjectId);

                    if (itemCat != null)
                    {
                        return new SidebarElementDTO
                        {
                            DeviceType = Common.Enum.ElementType.Carousel,
                            ElementId = obj.ObjectId,
                            Name = itemCat.Name,
                            GroupName = "",
                            Key = Guid.NewGuid(),
                            DeviceTypeStr = obj.Type,
                            SiteId = obj.SiteId,
                            ProfileStream = obj.ProfileStream,
                        };
                    }
                }
            }
            return null;
        }

        public async Task<bool> DeleteGroup(int id)
        {
            return await Vmon5Service.DeleteObjectGroup(Token, id);
        }

        public async Task<bool> DeleteElementOfGroup(int id)
        {
            return await Vmon5Service.DeleteElementObjectGroup(Token, id);
        }

        public List<OptionObjectDTO> GetDuration()
        {
            return Duration.Select
                (
                    p => new OptionObjectDTO
                    {
                        Name = p.Key,
                        Key = p.Value.ToString()
                    }
                ).ToList();
        }

        public List<OptionObjectDTO> GetProfilesStream()
        {
            return Enum.GetValues(typeof(Profile))
              .Cast<Profile>()
              .Where(p => p != Profile.None)
              .Select(p => new OptionObjectDTO
              {
                  Name = p.ToString(),
                  Key = ((int)p).ToString()
              })
              .ToList();
        }

        public List<OptionObjectDTO> GetTypeCarousel()
        {
            return new[] { new { k = false, v = "public" }, new { k = true, v = "private" } }.Select
                (
                    p => new OptionObjectDTO
                    {
                        Name = Resource.GetString(p.v),
                        IsPrivate = p.k
                    }
                ).ToList();
        }

        public List<OptionObjectDTO> GetTypeCarousel(bool type)
        {
            List<OptionObjectDTO> optionObjectDTO = new List<OptionObjectDTO>();
            if (type)
            {
                optionObjectDTO.Add(new OptionObjectDTO { IsPrivate = true, Name = "private" });
                optionObjectDTO.Add(new OptionObjectDTO { IsPrivate = false, Name = "public" });
            }
            else
            {
                optionObjectDTO.Add(new OptionObjectDTO { IsPrivate = false, Name = "public" });
                optionObjectDTO.Add(new OptionObjectDTO { IsPrivate = true, Name = "private" });
            }
            return optionObjectDTO;
        }
        private string GetProfilesStreamValue(Profile? profileStream)
        {
            return profileStream.HasValue ? ((int)profileStream).ToString() : ((int)Profile.SubStream).ToString();
        }
        public Profile ConvertToProfile(string item)
        {
            if (int.TryParse(item, out int profileValue))
            {
                if (Enum.IsDefined(typeof(Profile), profileValue))
                {
                    return (Profile)profileValue;
                }
            }
            return Profile.SubStream;
        }

        //public List<OptionObjectDTO> GetLocations(int page, int take, int typeElement, int userId, GroupType typeGroup, string filter, string search = "")
        //{
        //    var list = new List<OptionObjectDTO>();

        //    var _sites = Vmon5Service.GetSiteCatalog(Token, page, take, typeElement, userId, typeGroup, filter, search);

        //    if (_sites != null && _sites.Count>0)
        //    {
        //        list.AddRange(_sites.Elements.Select(y => new OptionObjectDTO
        //        {
        //            Name = y.Name,
        //            Key = y.Id.ToString(),
        //            Tag = y.Id

        //        }).ToList());
        //    }
        //    return list;
        //}

        //public List<OptionObjectDTO> GetCameras(int siteid, int page, int take, SidebarElement type, string[] filter, string search)
        //{
        //    var list = new List<OptionObjectDTO>();
        //    var _sitesDevices = Vmon5Service.GetSiteDevices(Token, siteid, page, take, (int)type, filter, search);

        //    if (_sitesDevices != null )
        //    {

        //        list.AddRange(_sitesDevices.Elements.Select(y => new OptionObjectDTO
        //        {
        //            Name = y.Name,
        //            Key = y.ElementId.ToString(),
        //            Tag = y.ElementId,
        //            Item = y,
        //            count = _sitesDevices.Count
        //        }).ToList());
        //    }
        //    return list;
        //}
    }
}
