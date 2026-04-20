using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Text;

namespace Elipgo.SmartClient.Common.Reflections
{
    public abstract class ManufactureUriAbstract : IManufactureUri
    {
        protected DTOs.CameraDTO Camera { get; set; }
        protected Enum.Profile Profile { get; set; } = Enum.Profile.None;

        public ManufactureUriAbstract(DTOs.CameraDTO camera)
        {
            Camera = camera;
        }

        public ManufactureUriAbstract(DTOs.CameraDTO camera, Enum.Profile profile)
        {
            Camera = camera;
            Profile = profile;
        }

        public string SendRequest(string url, HttpMethod method, string data = "", bool vRec5 = false)
        {
            string response = string.Empty;
            Uri uri = new Uri(url);
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = method.ToString();

                if (!HttpMethod.Get.Equals(method))
                {
                    if (data.Length != 0)
                    {
                        var bytes = Encoding.ASCII.GetBytes(data);
                        webRequest.ContentLength = bytes.Length;
                        using (var requestStream = webRequest.GetRequestStream())
                        {
                            requestStream.Write(bytes, 0, bytes.Length);
                            requestStream.Close();
                        }
                    }
                }
                CredentialCache cache = new CredentialCache();
                NetworkCredential networkCredential = new NetworkCredential(Camera.User, Camera.Password);
                cache.Add(new Uri(url), "Basic", networkCredential);
                cache.Add(new Uri(url), "Digest", networkCredential);

                webRequest.PreAuthenticate = true;
                webRequest.Credentials = cache;

                string password = string.Empty;
                if (vRec5)
                {
                    try
                    {
                        password = Security.AESDecrypt(Camera.Password);
                    }
                    catch (Exception)
                    {
                        password = Camera.Password;
                    }
                    webRequest.Headers.Add($"Authorization", $"Basic {Security.Base64Encode($"{Camera.User}:{password}")}");
                }

                if (url.ToUpper().Contains("HTTPS"))
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };
                }

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                Stream stream = webResponse.GetResponseStream();

                StreamReader streamReader = new StreamReader(stream);
                response = streamReader.ReadToEnd();
                stream.Close();
                webResponse.Close();
            }
            catch (Exception)
            {
                try
                {
                    DigestAuthFixer digest = new DigestAuthFixer($"{uri.Scheme}://{uri.Host}:{uri.Port}", Camera.User, Camera.Password);
                    response = digest.GrabResponse(uri.PathAndQuery);
                    Logger.Log($"------> Request: {url}", LogPriority.Important);
                }
                catch (Exception ex)
                {
                    var except = new Exception($"Request: {url}", ex);
                    Logger.Log(except, LogPriority.Fatal);
                    throw ex;
                }

            }
            return response;
        }

        public abstract string StreamLiveUri();
        public abstract string StreamLiveKurentoUri();
        public string KurentoServer()
        {
            return string.Format(Camera.Ws_Uri, Camera.Ws_Protocol, Camera.Ws_HttpPort);
        }
        public abstract string RecordingPlaybackUri();
        public abstract string StreamPlaybackUri();
        public abstract string StreamPlaybackKurentoUri(System.DateTime starttime, System.DateTime stoptime);
        public abstract string PtzControlUri(string code = "", string param1 = "", string param2 = "", string action = "stop");
        public abstract IOPortState InputPortState();
        public abstract IOPortState OuputPortState();
        public abstract void OuputPortChangeState(IOPortState state);
        public abstract string AudioConfigUri();
        public abstract string AudioTrasmitUri();
        public abstract string PresetListUri();
        public abstract string GuardTourUri();
        public abstract string CallPresetUri(PresetDTO preset);
        public abstract string CallGuardUri(ActivateGuardDTO guard);
        public abstract string StopGuardUri(ActivateGuardDTO guard);
        public abstract string SavePresetUri(PresetDTO preset);
        public abstract string DeletePresetUri(PresetDTO preset);
        public abstract string RemoveGuardTourUri(GuardDTO guard);
        public abstract string GuardTourGetUri(int guardId);
        public abstract string ExportRecordingUri(string recordingId, string starttime, string stoptime);
        public abstract IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false);
        public abstract string GetFirmwareVersionUri();

    }
}
