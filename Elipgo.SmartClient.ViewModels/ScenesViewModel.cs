using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Elipgo.SmartClient.ViewModels
{
    public class ScenesViewModel : IGenericViewModel
    {
        public long UserId { get; set; }
        public string UserIdGuid { get; set; }
        public string Token { get; set; }
        public long EntityId { get; set; }
        public CatalogDTO Catalog { get; set; }
        public ResourceManager Resource { get; set; }
        public async Task<List<ScenesEntity>> GetScenes()
        {
            return await Vmon5Service.GetScenes(Token);
        }

        public async Task<List<OptionObjectDTO>> GetIots(int idSite, string type)
        {
            var list = new List<OptionObjectDTO>();

            var lsIots = await Vmon5Service.GetIots(idSite, type, Token);

            if (lsIots != null)
            {
                foreach (var item in lsIots)
                {
                    var mapper = new OptionObjectDTO();
                    mapper.Name = item.Name;
                    mapper.Key = item.ObjectId.ToString();
                    mapper.Item =  item;
                    list.Add(mapper);
                }
            }

            return list;
        }

        public async Task<List<OptionObjectDTO>> GetScenesIots(int idSite, string type, int take, int page, string search = "")
        {
            var list = new List<OptionObjectDTO>();

            var lsIots = await Vmon5Service.GetSceneIots(idSite, type, Token, take, page, search);

            if (lsIots != null)
            {
                foreach (var item in lsIots)
                {
                    var mapper = new OptionObjectDTO();
                    mapper.Name = item.Name;
                    mapper.Key = item.ObjectId.ToString();
                    mapper.Item = item;
                    list.Add(mapper);
                }
            }

            return list;
        }

        public List<OptionObjectDTO> GetCamerasPtz(SceneElementEntity[] elements)
        {

            var lsPTZs = elements.Where(d => d.ObjectType == "VIN").ToList();
            var list = new List<OptionObjectDTO>();
            if (lsPTZs != null)
            {
                foreach (var ptz in lsPTZs)
                {
                    //var camerasPtz = site.Cameras.Where(c => c.LiveReadPTZ == true).ToList();
                    //if (camerasPtz != null && camerasPtz.Count > 0)
                    //{
                        var optionObjectDTOPtz =  new OptionObjectDTO
                        {
                            Key = ptz.ObjectId.ToString(),
                            Name = ptz.NameObjectSubType,
                            Item = new CatalogIot()
                            {
                                ObjectId = ptz.ObjectId,
                                Name = ptz.NameObjectSubType,
                                Type = ptz.ObjectType,
                                SubType = ptz.ObjectSubId.ToString(),
                                LocationLatitude = 0,
                                LocationLongitude = 0
                            },
                            //Tag = site.Id,
                            IsPtz = true
                        };
                        list.Add(optionObjectDTOPtz);
                    //}
                }
            }
            return list;
        }


        public List<OptionObjectDTO> GetActions()
        {
            return Enum.GetValues(typeof(ActionScenes)).Cast<ActionScenes>().Select
                (
                    p => new OptionObjectDTO
                    {
                        Name = Resource.GetString(p.GetType()
                                .GetMember(p.ToString())
                                .First()
                                .GetCustomAttribute<DisplayAttribute>().Name),
                        Key = ((int)p).ToString()
                    }
                ).ToList();
        }

        public async Task<ScenesEntity> SaveOrUpdate(ScenesEntity element)
        {
            if (element.Id == 0)
                return await Vmon5Service.SaveScenes(element, Token);
            else
                return await Vmon5Service.UpdateScenes(element, Token);
        }

        public async Task<ScenesEntity> GetScene(int id)
        {
            return await Vmon5Service.GetScene(id, Token);
        }

        public async Task<DataViewScenes[]> GetElements(SceneElementEntity[] elements)
        {
            List<OptionObjectDTO> devices = new List<OptionObjectDTO>();
            devices.AddRange( await GetIots(0,"DO")); 
            devices.AddRange(GetCamerasPtz(elements));
            var actions = GetActions();
            var result = new List<DataViewScenes>();

            foreach (var el in elements)
            {
                var i = devices.FirstOrDefault(d => (d.Item as CatalogIot).ObjectId == el.ObjectId);
                if (i == null)
                    continue;
                result.Add(new DataViewScenes
                {
                    Id = el.Id,
                    Action = el.Action,
                    ActionName = actions.FirstOrDefault(a => a.Key == el.Action.ToString())?.Name ?? " - ",
                    DeviceName = i?.Name ?? " - ",
                    IsDeleted = false,
                    ObjectId = el.ObjectId,
                    Order = el.Order,
                    ElementType = el.ObjectType,
                    ObjectSubType = el.ObjectSubType,
                    NameObjectSubType = el.NameObjectSubType
                });
            }

            return result.ToArray();
        }

        public async Task<bool> Delete(int id)
        {
            return await Vmon5Service.DeleteScene(Token, id);
        }

        public async Task<bool> Execute(int id)
        {
            return await Vmon5Service.ExecuteScene(Token, id);
        }

        public async Task<bool> DeleteElementOfScene(int id)
        {
            return await Vmon5Service.DeleteElementScene(Token, id);
        }

        public async Task<List<GuardDTO>> GetGuards(int CameraId)
        {
            return await Vmon5Service.GetGuards(CameraId, Token);
        }

        public async Task<List<PresetDTO>> GetPresets(int CameraId)
        {
            return await Vmon5Service.GetPresets(CameraId, Token);
        }

        public async Task<CameraDTO> GetCamera(int CameraId)
        {
            return await Vmon5Service.GetCamera(CameraId, Token);
        }
    }
}
