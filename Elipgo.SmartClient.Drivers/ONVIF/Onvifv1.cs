using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Elipgo.SmartClient.Drivers.ONVIF
{
    //DAHUA
    public class Onvifv1 : AbsOnvif
    {
        public Onvifv1(string user, string pass, string host, int http, int rtcp) : base(user, pass, host, http, rtcp)
        {
            this.ONVIF_GET_PROFILE = "/onvif/media/GetProfile";
            this.ONVIF_GET_STREAM_URI = "/onvif/media/GetStreamUri";
            this.ONVIF_PTZ = "/onvif/ptz/AbsoluteMove";
            this.ONVIF_PTZ_STOP = "/onvif/ptz/Stop";
            this.ONVIF_SNAPSHOT = "/onvif/media/GetSnapshotUri";
            this.ONVIF_PRESET_LIST = "/onvif/ptz/GetPresets";
            this.ONVIF_CALL_PRESET = "/onvif/ptz/GotoPreset";
            this.ONVIf_REMOVE_PRESET = "/onvif/ptz/RemovePreset";
            this.ONVIf_SAVE_PRESET = "/onvif/ptz/SetPreset";
        }

        public override async Task<string> getStreamUri()
        {
            await getInternalBaseUri();
            string urlStr = await getInternalStreamUri();
            return urlStr;
        }

        public async override Task<List<PresetDTO>> GetPresetList()
        {
            var listPreset = new List<PresetDTO>();
            try
            {

                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                string urlStr = "";

                urlStr = "http://" + host + ":" + httpPort + ONVIF_PRESET_LIST;

                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver20/ptz/wsdl"">
" + "\n" +
                @"   <soap:Header/>
" + "\n" +
                @"   <soap:Body>
" + "\n" +
                @"      <wsdl:GetPresets>
" + "\n" +
                @"         <wsdl:ProfileToken>" + profileToken + "</wsdl:ProfileToken>"
+ "\n" +
                @"      </wsdl:GetPresets>
" + "\n" +
                @"   </soap:Body>
" + "\n" +
                @"</soap:Envelope>";

                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                string resultContent = await tem.Content.ReadAsStringAsync();

                XmlDocument xmlResponse = new XmlDocument();
                xmlResponse.LoadXml(resultContent);
                XmlNode root = xmlResponse.LastChild;
                XmlNode bodyNode = root.LastChild;
                XmlNode getPresetResponse = bodyNode.FirstChild;
                XmlNodeList tptzPresetTokenList = getPresetResponse.ChildNodes;

                for (int i = 0; i < tptzPresetTokenList.Count; i++)
                {
                    XmlElement elem = (XmlElement)tptzPresetTokenList.Item(i);
                    string tokenStr = elem.Attributes[0].Value;
                    int token = Int32.Parse(tokenStr);
                    listPreset.Add(new PresetDTO() { Id = token, Name = "Preset " + tptzPresetTokenList.Item(i).InnerText });
                }
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" GetPresetList excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }

            return listPreset;
        }

        public async override void callPreset(PresetDTO preset)
        {
            try
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                string urlStr = "";

                urlStr = "http://" + host + ":" + httpPort + ONVIF_CALL_PRESET;

                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver20/ptz/wsdl"" xmlns:sch=""http://www.onvif.org/ver10/schema"">
" + "\n" +
                @"   <soap:Header/>
" + "\n" +
                @"   <soap:Body>
" + "\n" +
                @"      <wsdl:GotoPreset>
" + "\n" +
                @"         <wsdl:ProfileToken>" + profileToken + "</wsdl:ProfileToken>"
+ "\n" +
                @"         <wsdl:PresetToken>" + preset.Id + "</wsdl:PresetToken>"
+ "\n" +
                @"      </wsdl:GotoPreset>
" + "\n" +
                @"   </soap:Body>
" + "\n" +
                @"</soap:Envelope>";

                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                string resultContent = await tem.Content.ReadAsStringAsync();
                tem.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" callPreset excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }

        }

        public async override void RemovePreset(PresetDTO preset)
        {
            try
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                string urlStr = "";

                urlStr = "http://" + host + ":" + httpPort + ONVIf_REMOVE_PRESET;

                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver20/ptz/wsdl"">
" + "\n" +
                 @"   <soap:Header/>
" + "\n" +
                 @"   <soap:Body>
" + "\n" +
                 @"      <wsdl:RemovePreset>
" + "\n" +
                 @"         <wsdl:ProfileToken>" + profileToken + "</wsdl:ProfileToken>"
 + "\n" +
                 @"         <wsdl:PresetToken>" + preset.Id + "</wsdl:PresetToken>"
 + "\n" +
                 @"      </wsdl:RemovePreset>
" + "\n" +
                 @"   </soap:Body>
" + "\n" +
                 @"</soap:Envelope>";

                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                string resultContent = await tem.Content.ReadAsStringAsync();
                tem.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" RemovePreset excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }

        }

        public async override void SavePreset(PresetDTO preset)
        {
            try
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                string urlStr = "";

                urlStr = "http://" + host + ":" + httpPort + ONVIf_SAVE_PRESET;

                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver20/ptz/wsdl"">
" + "\n" +
                @"   <soap:Header/>
" + "\n" +
                @"   <soap:Body>
" + "\n" +
                @"      <wsdl:SetPreset>
" + "\n" +
                @"         <wsdl:ProfileToken>" + profileToken + "</wsdl:ProfileToken>"
 + "\n" +
                @"         <wsdl:PresetName>" + preset.Name + "</wsdl:PresetName>"
 + "\n" +
                @"      </wsdl:SetPreset>
" + "\n" +
                @"   </soap:Body>
" + "\n" +
                @"</soap:Envelope>";

                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                string resultContent = await tem.Content.ReadAsStringAsync();
                tem.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" SavePreset excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }


        }

    }
}
