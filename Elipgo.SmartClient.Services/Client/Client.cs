using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.SignalR.Connection;
using Elipgo.SmartClient.SignalR.Connection.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Client
{
    public delegate void IHttpProgress(bool progress);
    public delegate void IHttpLogOut();
    public class Client
    {
        public event IHttpProgress HttpProgress;
        public event IHttpLogOut HttpLogOut;

        private readonly string VMON_URL;

        private static Lazy<Client> _instance = new Lazy<Client>(() => new Client());
        private static readonly HttpClient httpClient;
        private static Configuration config = SmartClientEnvironmentUtils.GetConfiguration();
        private static double _timeOutClient = double.Parse(config.AppSettings.Settings["ClientTimeOut"].Value);

        static Client()
        {
            httpClient = new HttpClient() { Timeout = TimeSpan.FromMinutes(_timeOutClient) };
        }

        public Client()
        {
            VMON_URL = config.AppSettings.Settings["VMON5_URL"].Value;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072 | SecurityProtocolType.Tls13;
        }

        public static Client Instance
        {
            get => _instance.Value;
        }

        internal async Task<T> PostAsync<T>(object obj, string path, string token) where T : new()
        {
            HttpResponseMessage httpResponse = null;

            try
            {
                var json = JsonConvert.SerializeObject(obj);
                var stringContent = new StringContent(json, Encoding.UTF8, "application/json");


                var headers = httpClient.DefaultRequestHeaders;
                headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var requestUri = new Uri(VMON_URL + path);

                try
                {
                    // Logger.Log("-->> start PostAsync");

                    httpResponse = await httpClient
                        .PostAsync(requestUri, stringContent)
                        .ConfigureAwait(false);

                    httpResponse.EnsureSuccessStatusCode();

                    var httpResponseBody = await httpResponse.Content
                        .ReadAsStringAsync()
                        .ConfigureAwait(false);

                    var result = JsonConvert.DeserializeObject<T>(httpResponseBody);

                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format(
                        "-->> Elipgo.SmartClient.Services.Client.PostAsync -->> {0} | {1} Exception: {2} | stackTrace: {3} | Token: {4}",
                        VMON_URL, path, ex.Message, ex.StackTrace, token),
                        LogPriority.Sentry);

                    if (httpResponse != null)
                    {
                        switch (httpResponse.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                Logger.Log("-->> PostAsync Unauthorized", LogPriority.Sentry);
                                Logger.Log("-->> HttpLogOut.Invoke()", LogPriority.Important);
                                break;

                            case HttpStatusCode.InternalServerError:
                                Logger.Log("-->> PostAsync InternalServerError", LogPriority.Sentry);
                                break;

                            default:
                                Logger.Log("-->> PostAsync StatusCode: " + httpResponse.StatusCode, LogPriority.Sentry);
                                break;
                        }
                    }

                    return default(T);
                }

            }
            catch (Exception ex)
            {
                Logger.Log(string.Format(
                    "-->> Elipgo.SmartClient.Services.Client.PostAsync EX -->> {0} | {1} Exception: {2} | stackTrace: {3} | Token: {4}",
                    VMON_URL, path, ex.Message, ex.StackTrace, token),
                    LogPriority.Sentry);

                return default(T);
            }
        }

        internal async Task<T> DeleteAsync<T>(string path, string token) where T : new()
        {
            try
            {
                // Usamos 'using' para asegurar que el HttpClient se libere (Dispose) 
                // incluso si ocurre un error, evitando fugas de memoria.
                var headers = httpClient.DefaultRequestHeaders;
                headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Uri requestUri = new Uri(VMON_URL + path);

                HttpResponseMessage httpResponse = null;
                try
                {
                    // Logger.Log... (Inicio)

                    // 1. Llamada asíncrona real
                    httpResponse = await httpClient.DeleteAsync(requestUri);

                    // 2. Validación
                    httpResponse.EnsureSuccessStatusCode();

                    // 3. Lectura asíncrona del cuerpo
                    string httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                    // Logger.Log... (Response Body)

                    var result = JsonConvert.DeserializeObject<T>(httpResponseBody);

                    // No es necesario llamar a Dispose() manualmente gracias al 'using'

                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format("-->> Elipgo.SmartClient.Services.Client.DeleteAsync -->>  {0} | {1}  Exception: {2} | stackTrace : {3} | Token: {4} ", VMON_URL, path, ex.Message, ex.StackTrace, token), LogPriority.Sentry);

                    if (httpResponse != null)
                    {
                        switch (httpResponse.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                Logger.Log("-->> DeleteAsync UnAuthorized recevived Path: " + path + ", Token: " + token, LogPriority.Sentry);
                                Logger.Log("-->> HttpLogOut.Invoke()", LogPriority.Important);
                                // HttpLogOut.Invoke();
                                break;

                            case HttpStatusCode.InternalServerError:
                                Logger.Log("-->> DeleteAsync InternalServerError recevived URL:" + VMON_URL + "  Path: " + path + ", Token: " + token, LogPriority.Sentry);
                                break;

                            default:
                                Logger.Log("-->> DeleteAsync Default recevived URL:" + VMON_URL + "  Path: " + path + ", Token: " + token + ", StatusCode: " + httpResponse.StatusCode, LogPriority.Sentry);
                                break;
                        }
                    }
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("-->> Elipgo.SmartClient.Services.Client.DeleteAsync EX -->>  {0} | {1}  Exception: {2} | stackTrace : {3} | Token: {4} ", VMON_URL, path, ex.Message, ex.StackTrace, token), LogPriority.Sentry);
                return default(T);
            }
        }

        internal async Task<T> PutAsync<T>(object obj, string path, string token) where T : new()
        {
            var json = JsonConvert.SerializeObject(obj);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            // 'using' asegura que el socket se cierre correctamente (Dispose)

            var headers = httpClient.DefaultRequestHeaders;
            headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            Uri requestUri = new Uri(VMON_URL + path);

            HttpResponseMessage httpResponse = null;
            string httpResponseBody = "";

            try
            {
                // Logger.Log("-->> start PutAsync...", LogPriority.Important);

                // 1. Envío asíncrono
                httpResponse = await httpClient.PutAsync(requestUri, stringContent);
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
                httpResponse.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<T>(httpResponseBody);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("-->> Elipgo.SmartClient.Services.Client.PutAsync -->>  {0} | {1}  Exception: {2} | stackTrace : {3} | Token: {4} ", VMON_URL, path, ex.Message, ex.StackTrace, token), LogPriority.Sentry);

                if (httpResponse != null)
                {
                    switch (httpResponse.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            Logger.Log("-->> PutAsync UnAuthorized recevived  URL:" + VMON_URL + " Path: " + path + ", Token: " + token, LogPriority.Sentry);
                            Logger.Log("-->> HttpLogOut.Invoke()", LogPriority.Important);
                            // HttpLogOut.Invoke();
                            break;

                        case HttpStatusCode.InternalServerError:
                            Logger.Log("-->> PutAsync InternalServerError recevived URL:" + VMON_URL + "  Path: " + path + ", Token: " + token, LogPriority.Sentry);
                            break;

                        case HttpStatusCode.BadRequest:
                            // Aquí usamos httpResponseBody que ya fue leído arriba
                            Logger.Log("-->> PutAsync BadRequest recevived URL:" + VMON_URL + "  Path: " + path + ", Token: " + token + ", httpResponseBody:" + httpResponseBody, LogPriority.Sentry);
                            throw new Exception(httpResponseBody);

                        default:
                            Logger.Log(" -->> PutAsync Default recevived URL:" + VMON_URL + "  Path: " + path + ", Token: " + token + ", StatusCode: " + httpResponse.StatusCode, LogPriority.Sentry);
                            break;
                    }
                }
                return default(T);
            }
        }

        internal async Task<T> GetAsync<T>(string path, string token, bool ui = true) where T : new()
        {
            try
            {

                var headers = httpClient.DefaultRequestHeaders;
                headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Uri requestUri = new Uri(VMON_URL + path);
                HttpResponseMessage httpResponse = null;

                try
                {
                    // Logger.Log("-->> start GetAsync URL:" + VMON_URL + " Path: " + path);

                    httpResponse = await httpClient.GetAsync(requestUri);


                    httpResponse.EnsureSuccessStatusCode();

                    string httpResponseBody =
                        await httpResponse.Content.ReadAsStringAsync()
                                                  .ConfigureAwait(false);

                    if (requestUri.LocalPath.IndexOf("entities/cameras") != -1)
                    {
                        httpResponseBody = SortCameras(httpResponseBody);
                    }

                    var result = JsonConvert.DeserializeObject<T>(httpResponseBody);
                    return result;
                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format(
                        "-->> Elipgo.SmartClient.Services.Client.GetAsync -->> {0} | {1} Exception: {2} | stackTrace : {3} | Token: {4}",
                        VMON_URL, path, ex.Message, ex.StackTrace, token),
                        LogPriority.Sentry);

                    if (httpResponse != null)
                    {
                        switch (httpResponse.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                Logger.Log("-->> GetAsync UnAuthorized received",
                                    LogPriority.Sentry);
                                // HttpLogOut.Invoke();
                                break;

                            case HttpStatusCode.InternalServerError:
                                Logger.Log("-->> GetAsync InternalServerError received",
                                    LogPriority.Sentry);
                                break;

                            default:
                                Logger.Log("-->> GetAsync Default received StatusCode: "
                                    + httpResponse.StatusCode,
                                    LogPriority.Sentry);
                                break;
                        }
                    }

                    if (ui)
                        HttpProgress?.Invoke(false);

                    return default(T);
                }

            }
            catch (Exception ex)
            {
                Logger.Log(string.Format(
                    "-->> Elipgo.SmartClient.Services.Client.GetAsync EX -->> {0} | {1} Exception: {2} | stackTrace : {3} | Token: {4}",
                    VMON_URL, path, ex.Message, ex.StackTrace, token),
                    LogPriority.Sentry);

                return default(T);
            }
        }

        internal HttpResponseMessage GetAsync(string path, string token, bool ui = true)
        {
            try
            {
                var headers = httpClient.DefaultRequestHeaders;
                headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Uri requestUri = new Uri(VMON_URL + path);

                HttpResponseMessage httpResponse = null;
                try
                {
                    //Logger.Log("-->> start HttpResponseMessage GetAsync URL:" + VMON_URL + "  Path: " + path + ", Token: " + token, LogPriority.Important);
                    httpResponse = httpClient.GetAsync(requestUri).GetAwaiter().GetResult();
                    //Logger.Log("-->> HttpResponseMessage GetAsync httpResponse URL:" + VMON_URL + "  Path: " + path + ", Token: " + token, LogPriority.Important);
                    httpResponse.EnsureSuccessStatusCode();
                    //httpClient.Dispose();
                    //Logger.Log("-->> HttpResponseMessage GetAsync finall URL:" + VMON_URL + "  Path: " + path + ", Token: " + token, LogPriority.Important);
                    return httpResponse;
                }
                catch (Exception ex)
                {
                    //Logger.Log(ex);
                    Logger.Log(string.Format("-->> Elipgo.SmartClient.Services.Client.HttpResponseMessage GetAsync -->>  {0} | {1}  Exception: {2} | stackTrace : {3} | Token: {4} ", VMON_URL, path, ex.Message, ex.StackTrace, token), LogPriority.Sentry);
                    if (httpResponse != null)
                    {
                        switch (httpResponse.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                Logger.Log("-->> HttpResponseMessage GetAsync UnAuthorized recevived  URL:" + VMON_URL + " Path: " + path + ", Token: " + token, LogPriority.Sentry);
                                Logger.Log("-->> HttpLogOut.Invoke()", LogPriority.Important); //HttpLogOut.Invoke();
                                break;
                            case HttpStatusCode.InternalServerError:
                                Logger.Log("-->> HttpResponseMessage GetAsync InternalServerError recevived URL:" + VMON_URL + "  Path: " + path + ", Token: " + token, LogPriority.Sentry);
                                break;
                            default:
                                Logger.Log("-->> HttpResponseMessage GetAsync Default recevived URL:" + VMON_URL + "  Path: " + path + ", Token: " + token + ", StatusCode: " + httpResponse.StatusCode, LogPriority.Sentry);
                                break;
                        }
                    }
                    if (ui)
                        HttpProgress?.Invoke(false);

                    return default;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("-->> Elipgo.SmartClient.Services.Client.HttpResponseMessage GetAsync EX -->>  {0} | {1}  Exception: {2} | stackTrace : {3} | Token: {4} ", VMON_URL, path, ex.Message, ex.StackTrace, token), LogPriority.Sentry);
                return default;
            }
        }

        internal HttpResponseMessage GetAsyncByUri(string uri, string token = "")
        {
            try
            {
                var headers = httpClient.DefaultRequestHeaders;
                headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Uri requestUri = new Uri(uri);

                HttpResponseMessage httpResponse = null;
                try
                {
                    httpResponse = httpClient.GetAsync(requestUri).GetAwaiter().GetResult();
                    httpResponse.EnsureSuccessStatusCode();
                    //httpClient.Dispose();
                    return httpResponse;
                }
                catch (Exception ex)
                {
                    //Logger.Log(ex);
                    Logger.Log(string.Format(" {0} Exception: {1}", uri, ex), LogPriority.Fatal);
                    if (httpResponse != null)
                    {
                        switch (httpResponse.StatusCode)
                        {
                            case HttpStatusCode.InternalServerError:
                                Logger.Log(string.Format(" {0} HttpStatusCode.InternalServerError: {1}", uri, HttpStatusCode.InternalServerError), LogPriority.Fatal);
                                break;
                            default:
                                break;
                        }
                    }
                    return default;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("-->> Elipgo.SmartClient.Services.Client.HttpResponseMessage GetAsyncByUri EX -->>  {0} | {1}  Exception: {2} | stackTrace : {3} | Token: {4} ", VMON_URL, uri, ex.Message, ex.StackTrace, token), LogPriority.Sentry);
                return default;
            }
        }

        internal T GetAsyncByUri<T>(string uri, string token = "")
        {
            try
            {
                var headers = httpClient.DefaultRequestHeaders;
                headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                Uri requestUri = new Uri(uri);

                HttpResponseMessage httpResponse = null;
                try
                {
                    //Logger.Log("start GetAsyncByUri URL:" + VMON_URL + "  uri: " + uri + ", Token: " + token, LogPriority.Important);
                    httpResponse = httpClient.GetAsync(requestUri).GetAwaiter().GetResult();
                    //Logger.Log("-->> GetAsyncByUri httpResponse URL:" + VMON_URL + "  uri: " + uri + ", Token: " + token, LogPriority.Important);
                    httpResponse.EnsureSuccessStatusCode();
                    string httpResponseBody = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    //Logger.Log("-->> GetAsyncByUri httpResponseBody URL:" + VMON_URL + "  uri: " + uri + ", Token: " + token + " | ResponseBody: " + httpResponseBody, LogPriority.Important);
                    var result = JsonConvert.DeserializeObject<T>(httpResponseBody);
                    //httpClient.Dispose();
                    //Logger.Log("-->> GetAsyncByUri finall URL:" + VMON_URL + "  uri: " + uri + ", Token: " + token, LogPriority.Important);
                    return result;
                }
                catch (Exception ex)
                {
                    // Logger.Log(ex);
                    Logger.Log(string.Format("-->> Elipgo.SmartClient.Services.Client.GetAsyncByUri -->>  {0} | {1}  Exception: {2} | stackTrace : {3} | Token: {4} ", VMON_URL, uri, ex.Message, ex.StackTrace, token), LogPriority.Sentry);
                    if (httpResponse != null)
                    {
                        switch (httpResponse.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                Logger.Log("-->> GetAsyncByUri UnAuthorized recevived  URL:" + VMON_URL + " uri: " + uri + ", Token: " + token, LogPriority.Sentry);
                                Logger.Log("-->> HttpLogOut.Invoke()", LogPriority.Important); //HttpLogOut.Invoke();
                                break;
                            case HttpStatusCode.InternalServerError:
                                Logger.Log("-->> GetAsyncByUri InternalServerError recevived URL:" + VMON_URL + "  uri: " + uri + ", Token: " + token, LogPriority.Sentry);
                                break;
                            default:
                                Logger.Log("-->> GetAsyncByUri Default recevived URL:" + VMON_URL + "  uri: " + uri + ", Token: " + token + ", StatusCode:" + httpResponse.StatusCode, LogPriority.Sentry);
                                break;
                        }
                    }

                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("-->> Elipgo.SmartClient.Services.Client.GetAsyncByUri EX -->>  {0} | {1}  Exception: {2} | stackTrace : {3} | Token: {4} ", VMON_URL, uri, ex.Message, ex.StackTrace, token), LogPriority.Sentry);
                return default(T);
            }

        }

        #region Sort cameras
        private string SortCameras(string json)
        {
            var listObj = JsonConvert.DeserializeObject<List<RootObject>>(json);
            foreach (var item in listObj)
            {
                List<Cameras> lstCameras = new List<Cameras>();
                lstCameras = item.Cameras;
                List<Cameras> lstCamerasSort = new List<Cameras>();
                lstCamerasSort = lstCameras.OrderBy(x => x.Name.Trim()).ToList();
                item.Cameras = lstCamerasSort;
            }
            return JsonConvert.SerializeObject(listObj);
        }

        public class RootObject
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<Cameras> Cameras { get; set; }
        }

        public class Cameras
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public List<Sources> Sources { get; set; }
        }

        public class Sources
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string Address { get; set; }
            public string Port { get; set; }
            public string VirtualPath { get; set; }
            public string HttpProtocol { get; set; }
        }
        #endregion


    }
}
