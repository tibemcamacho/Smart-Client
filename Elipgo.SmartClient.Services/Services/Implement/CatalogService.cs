using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Services.Services.Interface;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class CatalogService : ICatalogService
    {
        private readonly IAppAuthorization appAuthorization = Locator.Current.GetService<IAppAuthorization>();

        private CatalogDTO _catalog = new CatalogDTO();
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IAppContainer _container = Locator.Current.GetService<IAppContainer>();

        public List<RecorderDTO> GetRecorders(CameraDTO camera)
        {
            var recorders = new List<RecorderDTO>();
            if (appAuthorization.Exist("auth.app.apps.playback.showRecorder.Vrec"))
            {
                camera.Recorders.ForEach(r => recorders.Add(r));
            }

            if (appAuthorization.Exist("auth.app.apps.playback.showRecorder.Edge") && camera.EdgeEnabled)
            {
                recorders.Insert(0, new RecorderDTO()
                {
                    Id = camera.Id,
                    Name = camera.Name,
                    Host = camera.Host,
                    HttpPort = camera.HttpPort,
                    VideoPort = camera.VideoPort,
                    RtspPort = camera.RtspPort,
                    HttpProtocol = camera.Protocol,
                    Gmt = camera.Gmt,
                    Username = camera.User,
                    Driver = camera.Driver.ToString(),
                    Password = camera.Password,
                    RecorderType = RecorderType.EDGE,
                    Channel = camera.Channel
                });
            }
            return recorders;
        }


        public CatalogDTO CurrentCatalog
        {
            get => _catalog;
            set => _catalog = value;
        }

        public async Task<CatalogDTO> GetValueCatalog(string token, int dvfid)
        {
            try
            {
                // Usamos WaitAsync en lugar de lock
                await _semaphore.WaitAsync();
                try
                {
                    var existDVF = false;
                    if (_catalog != null && _catalog.Sites != null  && _catalog.Sites.Count > 0)
                        existDVF = _catalog.Sites.Any(c => c.Cameras.Any(e => e.ObjectId == dvfid));
                    else
                        _catalog = new CatalogDTO();

                    if (!existDVF)
                    {
                        // Ahora usamos await para obtener el resultado real
                        var _catalogDVF = await Vmon5Service.GetSiteInfo(token, dvfid);

                        if (_catalogDVF != null && _catalogDVF.Sites != null && _catalogDVF.Sites.Any())
                        {
                            var newSiteData = _catalogDVF.Sites[0];
                            var existingSite = _catalog.Sites.FirstOrDefault(s => s.Id == newSiteData.Id);

                            if (existingSite != null)
                            {
                                if (!existingSite.Cameras.Any(e => e.ObjectId == dvfid))
                                {
                                    existingSite.Cameras.AddRange(newSiteData.Cameras);
                                }
                            }
                            else
                            {
                                if (_catalog.Sites.Count >= 1)
                                    _catalog.Sites.AddRange(_catalogDVF.Sites);
                                else
                                    _catalog = _catalogDVF;
                            }
                        }
                    }

                    Logger.Log(_catalog, "CatalogService/Update", string.Empty, string.Empty, LogPriority.Information);
                    SaveCatalog(_catalog);
                    return _catalog;
                }
                finally
                {
                    // SIEMPRE liberar el semáforo en el finally
                    _semaphore.Release();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error en GetValueCatalog: {ex.Message}", LogPriority.Sentry);
                throw;
            }
        }

        public async Task<CatalogDTO> GetValueCatalog(string token, List<int> dvfIds)
        {
            try
            {
                if (_catalog == null) _catalog = new CatalogDTO();
                if (_catalog.Sites == null) _catalog.Sites = new List<CatalogSite>();

                // La petición se hace fuera del semáforo para no bloquear a otros hilos mientras esperamos la red
                var _catalogDVF = await Vmon5Service.GetSiteDevicesInfo(token, dvfIds);

                // Usamos el semáforo para proteger la escritura en el catálogo local
                await _semaphore.WaitAsync();
                try
                {
                    if (_catalogDVF != null && _catalogDVF.Sites != null)
                    {
                        foreach (var siteFromService in _catalogDVF.Sites)
                        {
                            var existingSite = _catalog.Sites.FirstOrDefault(s => s.Id == siteFromService.Id);

                            if (existingSite != null)
                            {
                                foreach (var cam in siteFromService.Cameras)
                                {
                                    if (!existingSite.Cameras.Any(e => e.ObjectId == cam.ObjectId))
                                    {
                                        existingSite.Cameras.Add(cam);
                                    }
                                }
                            }
                            else
                            {
                                _catalog.Sites.Add(siteFromService);
                            }
                        }
                    }

                    Logger.Log(_catalog, "CatalogService/Update", string.Empty, string.Empty, LogPriority.Information);
                    SaveCatalog(_catalog);
                    return _catalog;
                }
                finally
                {
                    _semaphore.Release(); // Es vital liberar siempre el semáforo
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error en GetValueCatalog (List): {ex.Message}", LogPriority.Sentry);
                throw;
            }
        }

        public void SaveCatalog(CatalogDTO catalog)
        {
            _container.SaveCatalog(catalog);
        }

    }
}
