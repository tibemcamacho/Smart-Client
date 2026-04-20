using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.SignalR.Connection.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services
{
    public class Vmon5Service
    {
        // Cache estático para AppAuthorization - evita llamadas HTTP repetidas
        private static List<AppAuthorizationEntity> _appAuthorizationCache = null;
        private static readonly object _appAuthorizationLock = new object();
        private static DateTime _appAuthorizationCacheTime = DateTime.MinValue;
        private static readonly TimeSpan _appAuthorizationCacheDuration = TimeSpan.FromMinutes(30); // Cache por 30 minutos

        //public static CatalogObject GetSiteCatalog(string token, int page, int take, int typeElement, int userId, GroupType typeGroup, string filter, string search = "")
        //{
        //    try
        //    {
        //        //var tem = Client.Client.Instance.GetAsync<List<CatalogSite>>($"/v1/user/GetSiteCatalog/{page}/{take}", token);
        //        var tem = Client.Client.Instance.GetAsync<CatalogObject>($"/v1/user/searchSidebarElement/{page}/{take}/{typeElement}/{userId}/{typeGroup}/{filter}/{search}", token);

        //        return tem;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        public static async Task<CatalogObject> GetSiteCatalog(string token, int page, int take, int typeElement, int userId, GroupType typeGroup, string filter, string search = "")
        {
            try
            {
                // 1. Creamos el objeto que representa el JSON del body
                var requestBody = new
                {
                    Page = page,
                    Take = take,
                    TypeElement = typeElement,
                    UserId = userId,
                    TypeGroup = (int)typeGroup, // Asegúrate de enviarlo como entero si el API lo espera así
                    Filter = filter,
                    Search = search
                };

                // 2. Cambiamos la URL para que no lleve los parámetros en el path
                string url = "/v1/user/searchSidebarElement";

                // 3. Usamos PostAsync en lugar de GetAsync
                // Nota: Asumo que tu clase 'Instance' tiene un método PostAsync que acepta el objeto body
                var tem = await Client.Client.Instance.PostAsync<CatalogObject>(requestBody,url, token);

                return tem;
            }
            catch (Exception)
            {
                // Es recomendable loguear el error 'ex' aquí
                return null;
            }
        }


        public static async Task<CatalogObjectDetails> GetSiteDevices(string token,int idSite, int page, int take, int typeElement, string[] filter,string search)
        {
            try
            {
                var requestBody = new
                {
                    SiteId = idSite,
                    Page = page,
                    Take = take,
                    TypeElement = typeElement,
                    Filter = filter,
                    Search = search
                };
                var tem = await Client.Client.Instance.PostAsync<CatalogObjectDetails>(requestBody, $"/v1/user/searchSidebarElementDetails", token);
                //var tem = Client.Client.Instance.GetAsync<CatalogObjectDetails>($"/v1/user/searchSidebarElementDetails/{idSite}/{page}/{take}/{typeElement}/{(string.IsNullOrEmpty(filter) ? "\"\"" : filter)}/{search}", token);
                return tem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<CatalogObjectDetails> GetDevicesCatalog(string token, int page, int take, int typeElement, int userId, GroupType typeGroup, string search = "")
        {
            try
            {
                // 1. Creamos el objeto que representa el JSON del body
                var requestBody = new
                {
                    Page = page,
                    Take = take,
                    TypeElement = typeElement,
                    UserId = userId,
                    TypeGroup = (int)typeGroup, // Asegúrate de enviarlo como entero si el API lo espera así
                    Search = search
                };

                // 2. Cambiamos la URL para que no lleve los parámetros en el path
                string url = "/v1/user/searchDeviceElement";

                // 3. Usamos PostAsync en lugar de GetAsync
                // Nota: Asumo que tu clase 'Instance' tiene un método PostAsync que acepta el objeto body
                var tem = await Client.Client.Instance.PostAsync<CatalogObjectDetails>(requestBody, url, token);

                return tem;
            }
            catch (Exception)
            {
                // Es recomendable loguear el error 'ex' aquí
                return null;
            }
        }


        public static async Task<CatalogDTO> GetSiteInfo(string token, int dvf_id = 0)
        {
            try
            {
                var tem = await Client.Client.Instance.GetAsync<CatalogDTO>($"/v1/user/GetSiteInfo/{dvf_id}", token);
                return tem;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<CatalogDTO> GetSiteLocationInfo(string token, int dvf_id = 0)
        {
            try
            {
                var tem = await Client.Client.Instance.GetAsync<CatalogDTO>($"/v1/user/GetSiteLocationInfo/{dvf_id}", token);
                return tem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<CatalogDTO> GetSiteDevicesInfo(string token,List<int> dvf_id)
        {
            try
            {
                var tem = await Client.Client.Instance.PostAsync<CatalogDTO>(dvf_id,$"/v1/user/GetDeviceSiteInfo", token);
                return tem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<CatalogDTO> GetCarouselInfo(string token, int dvf_id = 0)
        {
            try
            {
                var tem = await Client.Client.Instance.GetAsync<CatalogDTO>($"/v1/user/GetCarouselInfo/{dvf_id}", token);
                return tem;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static async Task<CatalogObjectDetails> GetSidebarElementDetailsSite(string token, int siteId, int typeElement, string filter = "")
        {
            try
            {
                var tem = await Client.Client.Instance.GetAsync<CatalogObjectDetails>($"/v1/user/SidebarElementDetailsSite/{siteId}/{typeElement}/{filter}", token);
                return tem;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static HttpResponseMessage GetImage(string token, string imageType)
        {
            try
            {
                return Client.Client.Instance.GetAsync($"/v1/catalog/GetImage/{imageType}", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<ObjectGroupEntity>> GetObjectsGroup(string token, int userId, int type)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<List<ObjectGroupEntity>>($"/v1/objectgroups/{userId}/{type}", token);
            }
            catch (Exception)
            {
                return null;    
            }
        }

        public static async Task<List<CatalogObjectGroup>> GetsCarousel(string token)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<List<CatalogObjectGroup>>($"/v1/catalog/carousels/get", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<ScenesEntity>> GetScenes(string token)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<List<ScenesEntity>>("/v1/scenes", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<ObjectGroupEntity> GetObjectGroup(string token, int id)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<ObjectGroupEntity>($"/v1/objectgroups/{id}", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<ScenesEntity> GetScene(int id, string token)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<ScenesEntity>($"/v1/scenes/{id}", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSite">Si el id es 0 omite la condicion del site</param>
        /// <param name="type"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<List<CatalogIot>> GetIots(int idSite,string type, string token)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<List<CatalogIot>>($"/v1/devicefeatures/IODevices/{idSite}/{type}/", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSite">Si el id es 0 omite la condicion del site</param>
        /// <param name="type"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static async Task<List<CatalogIot>> GetSceneIots(int idSite, string type, string token, int take, int page, string search)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<List<CatalogIot>>($"/v1/devicefeatures/IOSceneDevices/{idSite}/{type}/{take}/{page}/{search}", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<AlarmGeoMapDTO>> GetAlarmsGeoMap(string token, int siteId)
        {
            try
            {
                //return Client.Client.Instance.GetAsync<CatalogDTO>("/v1/user/catalog", token);
                var response = await Client.Client.Instance.GetAsync<List<AlarmGeoMapDTO>>($"/v1/alarms/geomap/{siteId}", token);
                return response;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<OptionObjectDTO> GetFiltersType()
        {

            List<OptionObjectDTO> list = new List<OptionObjectDTO>
            {
                new OptionObjectDTO
                {
                    Key = "Devices",
                    Name = "DEVICES"
                },
                new OptionObjectDTO
                {
                    Key = "Analytics",
                    Name = "ANALYTICS"
                },
                new OptionObjectDTO
                {
                    Key = "Carousels",
                    Name = "CAROUSELS"
                },
                new OptionObjectDTO
                {
                    Key = "Locations",
                    Name = "LOCATIONS"
                },
                new OptionObjectDTO
                {
                    Key = "AlarmsMap",
                    Name = "ALARMS"
                }
            };
            return list;
        }

        public static async Task<List<OptionObjectDTO>> GetFiltersTypeRemove(string token)
        {
            List<OptionObjectDTO> list = new List<OptionObjectDTO>();

            try
            {
                var response = await Client.Client.Instance.GetAsync<List<OptionSidebarEnabled>>($"/v1/user/GetPermissionSidebarDDL", token);
                if (response != null)
                {
                    foreach (var item in response)
                    {
                        if (item.SidebarElement == 2 && !item.IsEnabled)
                        {
                            ///Analitics
                            list.Add(new OptionObjectDTO { Key = "Analytics", Name = "ANALYTICS" });
                        }

                        if (item.SidebarElement == 5 && !item.IsEnabled)
                        {
                            ///GEOMAP
                            list.Add(new OptionObjectDTO { Key = "AlarmsMap", Name = "ALARMS" });
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("GetFiltersTypeRemove Exception {0} ", ex), LogPriority.Fatal);
                return list;
            }

        }

        // Si se van a agregar nuevas opciones, empezamos AQUI...
        public static List<ObjectStateFilter> GetFilterElement()
        {
            List<ObjectStateFilter> listFilter = new List<ObjectStateFilter>();

            //Devices
            //
            //
            List<CheckElementDTO> listOptions = new List<CheckElementDTO>();
            var device = new ObjectStateFilter
            {
                Type = ElementSidebar.Device
            };
            listOptions.Add(new CheckElementDTO
            {
                Key = "All",
                Name = "ALL",
                State = true
            });

            listOptions.Add(new CheckElementDTO
            {
                Key = "SEPARATOR",
                Separator = true
            });

            listOptions.Add(new CheckElementDTO
            {
                Key = "Camera",
                Name = "CAMERAS",
                State = true
            });
            listOptions.Add(new CheckElementDTO
            {
                Key = "Iot_In",
                Name = "IOT_IN",
                State = true
            });
            listOptions.Add(new CheckElementDTO
            {
                Key = "Iot_Out",
                Name = "IOT_OUT",
                State = true
            });
            device.options = listOptions;
            listFilter.Add(device);

            //
            // Locations. => New option.
            //
            List<CheckElementDTO> listOptionsLocations = new List<CheckElementDTO>();
            var locations = new ObjectStateFilter
            {
                Type = ElementSidebar.Locations
            };
            listOptionsLocations.Add(new CheckElementDTO
            {
                Key = "All",
                Name = "ALL",
                State = true
            });
            listOptionsLocations.Add(new CheckElementDTO
            {
                Key = "Map",
                Name = "MAP",
                State = true
            });
            listOptionsLocations.Add(new CheckElementDTO
            {
                Key = "Geomap",
                Name = "GEO",
                State = true
            });
            listOptionsLocations.Add(new CheckElementDTO
            {
                Key = "AlarmsMap",
                Name = "ALARMS",
                State = true
            });
            locations.options = listOptionsLocations;
            listFilter.Add(locations);

            //
            //Analytics
            //
            List<CheckElementDTO> listOptionsAnaltycs = new List<CheckElementDTO>();
            var analytics = new ObjectStateFilter
            {
                Type = ElementSidebar.Analytics
            };
            listOptionsAnaltycs.Add(new CheckElementDTO
            {
                Key = "All",
                Name = "ALL",
                State = true
            });
            listOptionsAnaltycs.Add(new CheckElementDTO
            {
                Key = "Kpi",
                Name = "KPI",
                State = true
            });
            listOptionsAnaltycs.Add(new CheckElementDTO
            {
                Key = "Lpr",
                Name = "LPR",
                State = true
            });
            listOptionsAnaltycs.Add(new CheckElementDTO
            {
                Key = "Face",
                Name = "FACE",
                State = true
            });
            analytics.options = listOptionsAnaltycs;
            listFilter.Add(analytics);

            return listFilter;
        }

        public static async Task<bool> DeleteObjectGroup(string token, int id)
        {
            try
            {
                await Client.Client.Instance.DeleteAsync<ObjectGroupEntity>($"/v1/objectgroups/{id}", token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> DeleteScene(string token, int id)
        {
            try
            {
                await Client.Client.Instance.DeleteAsync<ObjectGroupEntity>($"/v1/scenes/{id}", token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> ExecuteScene(string token, int id)
        {
            try
            {
                await Client.Client.Instance.PostAsync<ObjectGroupEntity>(null, $"/v1/scenes/{id}/call", token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<CameraDTO> GetCamera(int id, string token, bool ui = true)
        {
            try
            {
                string path = "/v1/devicefeatures/data/" + id;
                var result = await Client.Client.Instance.GetAsync<CameraDTO>(path, token, ui);
                if (result != null)
                {
                    Logger.Log(result, "GetCamera", path, token, LogPriority.Information);

                    result.Password = Security.AESDecrypt(result.Password);
                    foreach (RecorderDTO it in result.Recorders)
                    {
                        try
                        {
                            it.Password = Security.AESDecrypt(it.Password);
                        }
                        catch
                        {
                        }
                    }

                    //if (!result.EdgeEnabled)
                    //{
                    //    if (result.Recorders != null && result.Recorders.Count > 0)
                    //    {
                    //        result.RecorderId = result.Recorders[0].Id;
                    //    }
                    //}
                    return result;
                }
                else
                    return new CameraDTO();
                
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("GetCamera Exception {0} ", ex), LogPriority.Fatal);
                return null;
            }
        }

        public static async Task<List<GuardDTO>> GetGuards(int id, string token)
        {
            try
            {
                string path = string.Format("/v2/cameras/{0}/guards", id);
                var result = await Client.Client.Instance.GetAsync<List<GuardDTO>>(path, token);
                Logger.Log(result, "GetGuards", path, token, LogPriority.Information);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<PresetDTO>> GetPresets(int id, string token)
        {
            try
            {
                string path = string.Format("/v2/cameras/{0}/presets", id);
                var result = await Client.Client.Instance.GetAsync<List<PresetDTO>>(path, token);
                Logger.Log(result, "GetPresets", path, token, LogPriority.Information);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<GuardForCreationDTO> GetGuard(int id, int guardid, string token)
        {
            try
            {
                string path = string.Format("/v2/cameras/{0}/guard/{1}", id, guardid);
                var result = await Client.Client.Instance.GetAsync<GuardForCreationDTO>(path, token);
                Logger.Log(result, "GetGuard", path, token, LogPriority.Information);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<(bool Success, string Message)> SaveGuard(int id, string token, GuardForCreationDTO guard, string message)
        {
            try
            {
                string path = string.Format("/v2/cameras/{0}/guardAdd", id);
                var result = await Client.Client.Instance.PutAsync<dynamic>(guard, path, token);
                Logger.Log(result, "SaveGuard", path, token, LogPriority.Information);
                if (result == null)
                {
                    return (false, message);
                }
                return (true, message);
            }
            catch (Exception ex)
            {
                message = ex.Message.Replace("{", "").Replace("}", "").Replace("\"", "");
                return (false, message);
            }
        }

        public static async Task<bool> DeleteGuard(int id, string token, GuardDTO guard)
        {
            try
            {
                string path = string.Format("/v2/cameras/{0}/guardDelete", id);
                var result = await Client.Client.Instance.PutAsync<dynamic>(guard, path, token);
                Logger.Log(result, "DeleteGuard", path, token, LogPriority.Information);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        //TMC
        public static async Task<bool> UpdateGuard(int id, string token, GuardForCreationDTO guard)
        {
            try
            {
                string path = string.Format("/v2/cameras/{0}/guardUpdate", id);
                var result = await Client.Client.Instance.PostAsync<dynamic>(guard, path, token);
                Logger.Log(result, "UpdateGuard", path, token, LogPriority.Information);
                return true;
            }
            catch (Exception)
            {
                //message = ex.Message;
                return false;
            }
        }

        public static async Task<bool> CallGuard(int id, string token, ActivateGuardDTO guard)
        {
            try
            {
                string path = string.Format("/v2/cameras/{0}/activateGuard", id);
                var result = await Client.Client.Instance.PostAsync<dynamic>(guard, path, token);
                Logger.Log(result, "CallGuard", path, token, LogPriority.Information);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> StopGuard(int id, string token, ActivateGuardDTO guard)
        {
            try
            {
                string path = string.Format("/v2/cameras/{0}/activateGuard", id);
                var result = await Client.Client.Instance.PostAsync<dynamic>(guard, path, token);
                Logger.Log(result, "StopGuard", path, token, LogPriority.Information);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<CameraCatalogVscDTO> GeVisualSearchs(int dvfID, string token, bool ui = true)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<CameraCatalogVscDTO>("/v1/visualsearchcache/cameras/" + dvfID, token, ui);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<CameraDTO>> GetCameraArrays(List<int> ids, string token)
        {
            try
            {
                var result = await Client.Client.Instance.PutAsync<List<CameraDTO>>(ids, "/v1/devicefeatures/dataArray", token);
                foreach (CameraDTO it in result)
                {
                    if (!string.IsNullOrEmpty(it.Password))
                    {
                        it.Password = Security.AESDecrypt(it.Password);
                        foreach (RecorderDTO it2 in it.Recorders)
                        {
                            try
                            {
                                //Se agrega try por si truena al desencriptar es por que no es un pass encriptado
                                it2.Password = Security.AESDecrypt(it2.Password);
                            }
                            catch
                            {
                                //Se se hacer nada por que no esta encriptado el pass
                            }

                        }
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<bool> DeleteElementObjectGroup(string token, int id)
        {
            try
            {
                await Client.Client.Instance.DeleteAsync<ObjectGroupElementEntity>($"/v1/objectgroupelements/{id}", token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<bool> DeleteElementScene(string token, int id)
        {
            try
            {
                await Client.Client.Instance.DeleteAsync<ObjectGroupEntity>($"/v1/sceneelements/{id}", token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<ScenesEntity> SaveScenes(ScenesEntity el, string token)
        {
            try
            {
                return await Client.Client.Instance.PostAsync<ScenesEntity>(el, "/v1/scenes", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<ScenesEntity> UpdateScenes(ScenesEntity el, string token)
        {
            try
            {
                return await Client.Client.Instance.PutAsync<ScenesEntity>(el, "/v1/scenes", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<ObjectGroupEntity> SaveGroup(ObjectGroupEntity el, string token)
        {
            try
            {
                return await Client.Client.Instance.PostAsync<ObjectGroupEntity>(el, "/v1/objectgroups", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<ObjectGroupEntity> UpdateGroup(ObjectGroupEntity el, string token)
        {
            try
            {
                return await Client.Client.Instance.PutAsync<ObjectGroupEntity>(el, "/v1/objectgroups", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<FaceAlarmsDTO>> GetFacesAlarms(int id, string token)
        {
            int limit = 10;
            try
            {
                var result = await Client.Client.Instance.GetAsync<List<FaceAlarmsDTO>>($"/v1/face/alarms/{id}/last/{limit}", token);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static List<DataGroupsByTypeDTO> GetConfigGroupByType()
        {
            return new List<DataGroupsByTypeDTO>
            {
                new DataGroupsByTypeDTO{
                    Group = "Locations",
                    PropertyListName = "Locations",
                    Type = "MAP",
                    TypeName="Map",
                    ElementType = ElementType.Blueprint
                },
                new DataGroupsByTypeDTO{
                    Group = "Carousels",
                    PropertyListName = "Carousels",
                    Type = "CAR",
                    TypeName="Car",
                    ElementType = ElementType.Carousel
                },
                new DataGroupsByTypeDTO{
                    Group = "Analytics",
                    PropertyListName = "Kpis",
                    Type = "KPI",
                    TypeName="Kpi",
                    ElementType = ElementType.Kpi
                },
                new DataGroupsByTypeDTO{
                    Group = "Analytics",
                    PropertyListName = "Lprs",
                    Type = "LPR",
                    TypeName="Lpr",
                    ElementType = ElementType.Lpr
                },
                new DataGroupsByTypeDTO{
                    Group = "Analytics",
                    PropertyListName = "Faces",
                    Type = "FACE",
                    TypeName="Face",
                    ElementType = ElementType.Face
                },
                new DataGroupsByTypeDTO{
                    Group = "Locations",
                    PropertyListName = "GeoLocations",
                    Type = "GEO",
                    TypeName="Geo",
                    ElementType = ElementType.Geomap
                },
                new DataGroupsByTypeDTO{
                    Group = "Devices",
                    PropertyListName = "Cameras",
                    Type = "VIN",
                    TypeName="Camera",
                    ElementType = ElementType.Camera
                },
                new DataGroupsByTypeDTO{
                    Group = "Devices",
                    PropertyListName = "Iots",
                    Type = "DI",
                    TypeName="Iot_In",
                    ElementType = ElementType.Iot_In
                },
                new DataGroupsByTypeDTO{
                    Group = "Devices",
                    PropertyListName = "Iots",
                    Type = "DO",
                    TypeName="Iot_Out",
                    ElementType = ElementType.Iot_Out
                },
                new DataGroupsByTypeDTO{
                    Group = "AlarmsMap",
                    PropertyListName = "AlarmsMap",
                    Type = "AlarmsMap",
                    TypeName="AlarmsMap",
                    ElementType = ElementType.AlarmsMap
                }

            };
        }

        public static async Task<List<LprAlarmDTO>> GetLprAlarms(int id, string token)
        {
            int limit = 10;
            try
            {
                var result = await Client.Client.Instance.GetAsync<List<LprAlarmDTO>>($"/v1/lprPlate/alarms/{id}/last/{limit}", token);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<LprSnapshotDTO> GetLprSnapshot(int id, string token)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<LprSnapshotDTO>($"/v1/lprPlate/getPlateSnapshot/{id}", token);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<BlueprintDTO> GetBlueprint(int id, string token)
        {
            try
            {
                var vmap = await Client.Client.Instance.GetAsync<BlueprintDTO>($"/v1/vmaps/{id} ", token);
                if (vmap != null)
                {
                    var vmapElements = await Client.Client.Instance.GetAsync<List<BlueprintElementDTO>>($"/v1/vmaps/{vmap.Id}/vmapelements", token);
                    vmap.Elements = vmapElements;
                }

                return vmap;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<BookmarkGroupDTO>> GetBookmarkGroups(string token)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<List<BookmarkGroupDTO>>($"/v1/bookmarkgroups", token);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<BookmarkGroupDTO>> GetBookmarkGroupsByFilter(string token, BookmarkFilterDTO filter)
        {
            try
            {
                var result = await Client.Client.Instance.PostAsync<List<BookmarkGroupDTO>>(filter, $"/v1/bookmarkgroups/filter", token);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<BookmarkGroupByPageDTO> GetBookmarkGroupsByPage(string token, BookmarkFilterDTO filter)
        {
            try
            {
                var result = await Client.Client.Instance.PostAsync<BookmarkGroupByPageDTO>(filter, $"/v1/bookmarkgroups/page", token);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<bool> ChangeStatusBookmarkGroup(int bookmarkGroupId, int status, string token)
        {
            try
            {
                var result = await Client.Client.Instance.PostAsync<bool>(null, $"/v1/bookmarkgroups/changestatus/{bookmarkGroupId}/{status}", token);
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<BookmarkGroupDTO> UpdateBookmarkGroup(BookmarkGroupDTO bookmark, string token)
        {
            try
            {
                return await Client.Client.Instance.PutAsync<BookmarkGroupDTO>(bookmark, $"/v1/bookmarkgroups", token);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static async Task<List<AppAuthorizationEntity>> AppAuthorization(string token)
        {
            try
            {
                // Usar cache si está disponible y no ha expirado
                lock (_appAuthorizationLock)
                {
                    if (_appAuthorizationCache != null &&
                        (DateTime.UtcNow - _appAuthorizationCacheTime) < _appAuthorizationCacheDuration)
                    {
                        return _appAuthorizationCache;
                    }
                }

                // Cache expirado o vacío - hacer llamada HTTP
                var result = await Client.Client.Instance.GetAsync<List<AppAuthorizationEntity>>($"/v1/userprofiles/appauthorization", token);

                // Guardar en cache
                lock (_appAuthorizationLock)
                {
                    _appAuthorizationCache = result;
                    _appAuthorizationCacheTime = DateTime.UtcNow;
                }

                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Invalida el cache de AppAuthorization (llamar al hacer logout o cambio de usuario)
        /// </summary>
        public static void InvalidateAppAuthorizationCache()
        {
            lock (_appAuthorizationLock)
            {
                _appAuthorizationCache = null;
                _appAuthorizationCacheTime = DateTime.MinValue;
            }
        }

        public static Image GetProfileImage(string email, string token)
        {
            try
            {
                var httpResponse = Client.Client.Instance.GetAsync($"/v1/User/ImageProfile/" + email, token);
                if (httpResponse == null || httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                    return null;
                httpResponse.EnsureSuccessStatusCode();
                string httpResponseBody = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                return ImageHelper.ProcessBase64Response(httpResponseBody);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<string>> GetTypesAlarms(string token)
        {
            return await Client.Client.Instance.GetAsync<List<string>>("/v1/alarms/type", token);
        }

        public static async Task<ApplicationEntitiesDTO> GetByEntityApplication(string token)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<ApplicationEntitiesDTO>($"/v1/entities/GetByEntityApplication", token);
            }
            catch
            {
                return null;
            }
        }

        public static async Task<List<TimelineDTO>> GetTimeLine(int cameraId, int recorderId, string startDate, string endDate, string token)
        {
            try
            {
                string path = string.Format("/v2/timeline?cameraId={0}&startDate={1}&endDate={2}&RecorderId={3}", cameraId, startDate, endDate, recorderId);
                return await Client.Client.Instance.GetAsync<List<TimelineDTO>>(path, token);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("GetTimeLine Exception {0} ", ex), LogPriority.Fatal);
                return null;
            }
        }
        
        public static async Task<string> Snapshot(int id, string token)
        {
            try
            {
                string path = string.Format("/v2/cameras/{0}/snapshot", id);
                var result = await Client.Client.Instance.GetAsync<object>(path, token);
                Logger.Log(result, "Snapshot", path, token, LogPriority.Information);
                return result?.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static async Task<bool> ChangeStatusBookmarkGroup(List<int> bookmarkGroupIds, int status, string token)
        {
            try
            {
                var result = await Client.Client.Instance.PutAsync<bool>(bookmarkGroupIds, $"/v1/bookmarkgroups/changestatuslist/{status}", token); return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static async Task<List<TimelineDTO>> getCameraStatus(int elementId, string token)
        {
            try
            {
                string path = string.Format($"/v1/DeviceFeaturesStateLive/lastState/test/{elementId}");
                return await Client.Client.Instance.GetAsync<List<TimelineDTO>>(path, token);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("getTimeLine Exception {0} ", ex), LogPriority.Fatal);
                return null;
            }
        }

        public static async Task<TestDeviceFeatureAvailableDTO> getCameraStatusTest(int elementId, string token)
        {
            try
            {
                string path = string.Format($"/v1/DeviceFeaturesStateLive/lastState/test/{elementId}");
                return await Client.Client.Instance.GetAsync<TestDeviceFeatureAvailableDTO>(path, token);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("getStatus Exception {0} ", ex), LogPriority.Fatal);
                return null;
            }
        }

        public static async Task<TestDeviceFeatureAvailableDTO> postDeviceStatusTest(List<CameraStatesLiveDTO> elementId, string token)
        {
            try
            {
                string path = string.Format($"/v1/DeviceFeaturesStateLive/lastStateDevices/test");
                return await Client.Client.Instance.PostAsync<TestDeviceFeatureAvailableDTO>(elementId, path, token);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("getStatus Exception {0} ", ex), LogPriority.Fatal);
                return null;
            }
        }

        public static async Task<TestDeviceFeatureRecordDTO> postCameraStatusRecordTest(List<CameraStatesDTO> elementId, string token)
        {
            try
            {
                string path = string.Format($"/v1/DeviceFeaturesStateLive/lastStatesRecords/test");
                return await Client.Client.Instance.PostAsync<TestDeviceFeatureRecordDTO>(elementId, path, token);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("getStatusRecord Exception {0} ", ex), LogPriority.Fatal);
                return null;
            }
        }

        public static async Task<TestDeviceFeatureRecordDTO> getCameraStatusRecordTest(int elementId, string token)
        {
            try
            {
                string path = string.Format($"/v1/DeviceFeaturesStateLive/lastStateRecord/test/{elementId}");
                return await Client.Client.Instance.GetAsync<TestDeviceFeatureRecordDTO>(path, token);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("getStatusRecord Exception {0} ", ex), LogPriority.Fatal);
                return null;
            }
        }

        public static async Task<List<ACServerDTO>> GetAccessControlServers(string token)
        {
            try
            {
                string path = string.Format($"/v1/controlAcceso/customer/GetAssignedByUsp");
                return await Client.Client.Instance.PostAsync<List<ACServerDTO>>(null, path, token);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("getStatusRecord Exception {0} ", ex), LogPriority.Fatal);
                return null;
            }
        }

        public static async Task<bool> UpdateLastLogin(string email, string token)
        {
            try
            {
                string path = string.Format($"/v1/user/UpdateLastLogin?email={email}");
                await Client.Client.Instance.PutAsync<dynamic>(null, path, token);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("UpdateLastLogin Exception {0} ", ex), LogPriority.Fatal);
                return false;
            }
        }

        public static async Task<LastLoginDTO> GetUserLastLoginDate(string email, string token)
        {
            try
            {
                var tem = await Client.Client.Instance.GetAsync<LastLoginDTO>($"/v1/user/GetUserLastLoginDate/{email}", token);
                return tem;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}