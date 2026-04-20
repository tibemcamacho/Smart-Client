using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Elipgo.SmartClient.Drivers.ONVIF
{
    public abstract class AbsOnvif
    {
        protected string userOnvif;
        protected string pwsOnvif;
        protected string host;
        protected int httpPort;
        protected int rtcpPort;
        protected string ONVIF_GET_PROFILE;
        protected string ONVIF_GET_STREAM_URI;
        protected string ONVIF_AXIS;
        protected string profileToken;
        protected Common.Enum.Drivers typeOnvif;
        protected string urlStr;
        protected string urlStream;
        protected string ONVIF_PTZ;
        protected string ONVIF_PTZ_STOP = "/onvif/ptz/Stop";
        protected int zoom = 0;
        protected string ONVIF_SNAPSHOT;
        protected string urlSnapShot = "";
        protected string ONVIF_PRESET_LIST;
        protected string ONVIF_CALL_PRESET;
        protected string ONVIf_REMOVE_PRESET;
        protected string ONVIf_SAVE_PRESET;

        public AbsOnvif(string user, string pass, string host, int http, int rtcp)
        {

            this.UserOnvif = user;
            this.PwsOnvid = pass;
            this.Host = host;
            this.HttpPort = http;
            this.RtcpPort = rtcp;
        }

        public string UserOnvif { get => userOnvif; set => userOnvif = value; }
        public string PwsOnvid { get => pwsOnvif; set => pwsOnvif = value; }
        public string Host { get => host; set => host = value; }
        public int HttpPort { get => httpPort; set => httpPort = value; }
        public int RtcpPort { get => rtcpPort; set => rtcpPort = value; }

        protected async virtual Task<string> getInternalBaseUri()
        {
            try
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                urlStr = "http://" + Host + ":" + HttpPort + ONVIF_GET_PROFILE;
                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(UserOnvif, PwsOnvid));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(UserOnvif, PwsOnvid));
                httpClientHandler.Credentials = credentialCache;

                HttpClient client = new HttpClient(httpClientHandler);

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver10/media/wsdl"">
                      " + "\n" + @"   <soap:Header/> " + "\n" + @"   <soap:Body> " + "\n" + @"      <wsdl:GetProfiles/> " + "\n" + @"   </soap:Body> " + "\n" + @"</soap:Envelope>";
                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                tem.EnsureSuccessStatusCode();
                string resultContent = await tem.Content.ReadAsStringAsync();
                string token = resultContent.Substring(resultContent.IndexOf("token=") + "token=".Length);
                profileToken = token.Substring(0, token.IndexOf(" "));
                profileToken = profileToken.Replace("\"", "");
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" getInternalBaseUri excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }

            return profileToken;
        }

        protected async virtual Task<string> getInternalStreamUri()
        {

            try
            {

                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                string urlStr = "";

                urlStr = "http://" + host + ":" + httpPort + ONVIF_GET_STREAM_URI;

                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(
                  new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver10/media/wsdl"" xmlns:sch=""http://www.onvif.org/ver10/schema"">
                        " + "\n" +
                                              @"   <soap:Header/>
                        " + "\n" +
                                              @"   <soap:Body>
                        " + "\n" +
                                              @"      <wsdl:GetStreamUri>
                        " + "\n" +
                                              @"         <wsdl:StreamSetup>
                        " + "\n" +
                                              @"            <sch:Stream>RTP-Unicast</sch:Stream>
                        " + "\n" +
                                              @"            <sch:Transport>
                        " + "\n" +
                                              @"               <sch:Protocol>RTSP</sch:Protocol>
                        " + "\n" +
                                              @"               <!--Optional:-->
                        " + "\n" +
                                              @"               <sch:Tunnel><sch:Protocol/></sch:Tunnel>
                        " + "\n" +
                                              @"            </sch:Transport>
                        " + "\n" +
                                              @"            <!--You may enter ANY elements at this point-->
                        " + "\n" +
                                              @"         </wsdl:StreamSetup>
                        " + "\n" +
                                              @"         <wsdl:ProfileToken>" + profileToken + "</wsdl:ProfileToken>"
                            + "\n" +
                                              @"      </wsdl:GetStreamUri>
                        " + "\n" +
                                              @"   </soap:Body>
                        " + "\n" +
                                              @"</soap:Envelope>";

                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                tem.EnsureSuccessStatusCode();
                string resultContent = await tem.Content.ReadAsStringAsync();
                string rtsp = resultContent.Substring(resultContent.IndexOf("<tt:Uri>") + "<tt:Uri>".Length);
                string rtspUri = rtsp.Substring(0, rtsp.IndexOf("</tt:Uri>")).Replace("\"", "");
                Uri uriRtcp = new Uri(rtspUri);
                string HostR = uriRtcp.Host;
                string Query = uriRtcp.Query;
                string AbsolutePath = uriRtcp.AbsolutePath;
                urlStream = "rtsp://" + userOnvif + ":" + pwsOnvif + "@" + host + ":" + RtcpPort + AbsolutePath + Query;
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" getInternalStreamUri excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }
            return urlStream;

        }

        public abstract Task<string> getStreamUri();

        public async virtual void PTZControl(PtzMovement moviento, int valor, int isstopo)
        {
            try
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                string urlStr = "";
                string movementX = "";
                string movementY = "";

                urlStr = "http://" + host + ":" + httpPort + ONVIF_PTZ;

                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(
                  new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                switch (moviento)
                {
                    case PtzMovement.Up:
                        movementX = "0";
                        movementY = "0.5";
                        //request = string.Format(body, "0", valor);
                        break;
                    case PtzMovement.Down:
                        //request = string.Format(body, "0", valor * -1);
                        movementX = "0";
                        movementY = "-0.5";
                        break;
                    case PtzMovement.Left:
                        movementX = "-0.5";
                        movementY = "0";
                        break;
                    case PtzMovement.Right:
                        movementX = "0.5";
                        movementY = "0";
                        break;
                    case PtzMovement.UpLeft:
                        movementX = "-0.5";
                        movementY = "0.5";
                        break;
                    case PtzMovement.UpRight:
                        movementX = "0.5";
                        movementY = "0.5";
                        break;
                    case PtzMovement.DownLeft:
                        movementX = "-0.5";
                        movementY = "-0.5";
                        break;
                    case PtzMovement.DownRight:
                        movementX = "0.5";
                        movementY = "-0.5";
                        break;
                }

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver20/ptz/wsdl"" xmlns:sch=""http://www.onvif.org/ver10/schema"">
" + "\n" +
                @"   <soap:Header/>
" + "\n" +
                @"   <soap:Body>
" + "\n" +
                @"      <wsdl:ContinuousMove>
" + "\n" +
                @"         <wsdl:ProfileToken>" + profileToken + "</wsdl:ProfileToken>" + "\n" +
                @"         <wsdl:Velocity>
" + "\n" +
                @"            <!--Optional:-->
" + "\n" +
                @"<sch:PanTilt x=" + "\"" + movementX + "\"" + " y=" + "\"" + movementY + "\"" + " space=" + "\"" + "\"" + "/>" + "\n" +
                @"            <!--Optional:-->
" + "\n" +
                @"            <sch:Zoom x=""0.000000"" space=""""/>
" + "\n" +
                @"         </wsdl:Velocity>

" +
                @"      </wsdl:ContinuousMove>
" + "\n" +
                @"   </soap:Body>
" + "\n" +
                @"</soap:Envelope>";

                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                tem.EnsureSuccessStatusCode();
                string resultContent = await tem.Content.ReadAsStringAsync();
                string rtsp = resultContent;
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" PTZControl excepcion {0}", e), LogPriority.Fatal);
            }

        }

        public async virtual void PTZControlStop(PtzMovement moviento, int valor, int isstopo)
        {
            try
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                string urlStr = "";

                urlStr = "http://" + host + ":" + httpPort + ONVIF_PTZ;

                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(
                  new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver20/ptz/wsdl"">
" + "\n" +
                     @"   <soap:Header/>
" + "\n" +
                     @"   <soap:Body>
" + "\n" +
                     @"      <wsdl:Stop>
" + "\n" +
                     @"         <wsdl:ProfileToken>" + profileToken + "</wsdl:ProfileToken>"
+ "\n" +
                     @"         <!--Optional:-->
" + "\n" +
                     @"         <wsdl:PanTilt>true</wsdl:PanTilt>
" + "\n" +
                     @"         <!--Optional:-->
" + "\n" +
                     @"         <wsdl:Zoom>true</wsdl:Zoom>
" + "\n" +
                     @"      </wsdl:Stop>
" + "\n" +
                     @"   </soap:Body>
" + "\n" +
                     @"</soap:Envelope>";

                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                tem.EnsureSuccessStatusCode();
                string resultContent = await tem.Content.ReadAsStringAsync();
                string rtsp = resultContent;
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" PTZControl excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }

        }

        public async virtual void PTZControlSZoom(PtzMovement zommType, int valor, int isstopo)
        {
            try
            {
                double zoomMax = 9;
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                string urlStr = "";

                urlStr = "http://" + host + ":" + httpPort + ONVIF_PTZ;

                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(
                  new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                switch (zommType)
                {
                    case PtzMovement.ZoomIn:
                        if (zoom < zoomMax)
                        {
                            zoom++;
                        }
                        break;
                    case PtzMovement.ZoomOut:
                        if (zoom != 0)
                        {
                            zoom--;
                        }

                        break;
                }

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver20/ptz/wsdl"" xmlns:sch=""http://www.onvif.org/ver10/schema"">
" + "\n" +
                @"   <soap:Header/>
" + "\n" +
                @"   <soap:Body>
" + "\n" +
                @"      <wsdl:AbsoluteMove>
" + "\n" +
                @"         <wsdl:ProfileToken>" + profileToken + "</wsdl:ProfileToken>"
+ "\n" +
                @"         <wsdl:Position>            
" + "\n" +
                @"            <sch:Zoom x=" + "\"" + "0." + zoom + "\"" + " space=" + "\"" + "http://www.onvif.org/ver10/tptz/ZoomSpaces/PositionGenericSpace" + "\"" + "/>" + "\n"
+ "\n" +
                @"         </wsdl:Position>
" + "\n" +
                @"      </wsdl:AbsoluteMove>
" + "\n" +
                @"   </soap:Body>
" + "\n" +
                @"</soap:Envelope>";

                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                tem.EnsureSuccessStatusCode();
                string resultContent = await tem.Content.ReadAsStringAsync();
                string rtsp = resultContent;
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" PTZControl excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }
        }

        protected async virtual Task<string> getSnapshotUri()
        {
            try
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                string urlStr = "";

                urlStr = "http://" + host + ":" + httpPort + ONVIF_SNAPSHOT;

                Uri uri = new Uri(urlStr);

                var credentialCache = new CredentialCache();
                credentialCache.Add(
                  new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                var body = @"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:wsdl=""http://www.onvif.org/ver10/media/wsdl"">
" + "\n" +
                @"   <soap:Header/>
" + "\n" +
                @"   <soap:Body>
" + "\n" +
                @"      <wsdl:GetSnapshotUri>
" + "\n" +
                @"         <wsdl:ProfileToken>" + profileToken + "</wsdl:ProfileToken>"
 + "\n" +
                @"      </wsdl:GetSnapshotUri>
" + "\n" +
                @"   </soap:Body>
" + "\n" +
                @"</soap:Envelope>";

                var content = new StringContent(body, Encoding.UTF8, "application/xml");
                var tem = client.PostAsync(uri, content).Result;
                string resultContent = await tem.Content.ReadAsStringAsync();
                string imgUri = resultContent.Substring(resultContent.IndexOf("<tt:Uri>") + "<tt:Uri>".Length);
                string img = imgUri.Substring(0, imgUri.IndexOf("</tt:Uri>")).Replace("\"", "");
                Uri uriSnapshot = new Uri(img);
                string HostR = uriSnapshot.Host;
                string Query = uriSnapshot.Query;
                string AbsolutePath = uriSnapshot.AbsolutePath;
                string snapShotUri = "http://" + host + ":" + httpPort + AbsolutePath + Query;
                return snapShotUri;
            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" getSnapshotUri excepcion {0}", e), LogPriority.Fatal);
                throw e;
            }

        }

        public async virtual void downloadSnapShot(string path)
        {
            try
            {
                if (string.IsNullOrEmpty(this.urlSnapShot))
                {
                    this.urlSnapShot = await getSnapshotUri();
                }
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                httpClientHandler.AllowAutoRedirect = false;
                Uri uri = new Uri(urlSnapShot);

                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Digest", new NetworkCredential(userOnvif, pwsOnvif));
                credentialCache.Add(new Uri(uri.GetLeftPart(UriPartial.Authority)), "Basic", new NetworkCredential(userOnvif, pwsOnvif));
                httpClientHandler.Credentials = credentialCache;
                HttpClient client = new HttpClient(httpClientHandler);

                var tem = client.GetAsync(uri).Result;
                tem.EnsureSuccessStatusCode();
                var resultContent = await tem.Content.ReadAsByteArrayAsync();
                var stringBase = Convert.ToBase64String(resultContent);
                var image = ImageHelper.Base64ToImage(stringBase);
                image.Save(path);

            }
            catch (Exception e)
            {
                Logger.Log(string.Format(" downloadSnapShot excepcion {0}", e), LogPriority.Fatal);
            }

        }

        public async virtual Task<List<PresetDTO>> GetPresetList()
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
                XmlNode bodyNode = root.FirstChild;
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

        public async virtual void callPreset(PresetDTO preset)
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

        public async virtual void RemovePreset(PresetDTO preset)
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

        public async virtual void SavePreset(PresetDTO preset)
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
