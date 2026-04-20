using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;

namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{
    public class VrecUri : ManufactureUriAbstract
    {
        public VrecUri(CameraDTO camera) : base(camera)
        {
        }

        public VrecUri(CameraDTO camera, Profile profile) : base(camera, profile)
        {
        }

        public override IOPortState InputPortState()
        {
            throw new NotImplementedException();
        }

        public override void OuputPortChangeState(IOPortState state)
        {
            throw new NotImplementedException();
        }

        public override IOPortState OuputPortState()
        {
            throw new NotImplementedException();
        }

        public override string StreamLiveUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

            query["cameraid"] = Camera.Id.ToString();
            query["u"] = UserBase64();
            query["vt"] = PasswordMD5();


            return new UriBuilder()
            {
                Scheme = "axrtsp",
                Host = Camera.Recorders[0].Host,
                Port = Camera.Recorders[0].RtspPort,
                Path = "/hlive",
                Query = query.ToString()
            }.ToString();

        }
        public override string StreamLiveKurentoUri()
        {
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

            query["cameraid"] = Camera.Id.ToString();
            query["u"] = UserBase64();
            query["vt"] = PasswordMD5();


            return new UriBuilder()
            {
                Scheme = "rtsp",
                UserName = Camera.User,
                Password = Camera.Password,
                Host = Camera.Recorders[0].Host,
                Port = Camera.Recorders[0].RtspPort,
                Path = "/hlive",
                Query = query.ToString()
            }.ToString();

        }

        public override string StreamPlaybackUri()
        {
            throw new NotImplementedException();
        }

        public override string StreamPlaybackKurentoUri(System.DateTime starttime, System.DateTime stoptime)
        {
            throw new NotImplementedException();
        }

        public override string RecordingPlaybackUri()
        {
            throw new NotImplementedException();
        }

        public override string PtzControlUri(string code = "", string param1 = "", string param2 = "", string action = "stop")
        {
            throw new NotImplementedException();
        }

        public override string AudioConfigUri()
        {
            throw new NotImplementedException();
        }

        public override string AudioTrasmitUri()
        {
            throw new NotImplementedException();
        }

        public override string PresetListUri()
        {
            throw new NotImplementedException();
        }

        public override string GuardTourUri()
        {
            throw new NotImplementedException();
        }

        public override string CallPresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string CallGuardUri(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string StopGuardUri(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string SavePresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string DeletePresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string RemoveGuardTourUri(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string GuardTourGetUri(int guardId)
        {
            throw new NotImplementedException();
        }

        public override string ExportRecordingUri(string recordingId, string starttime, string stoptime)
        {
            throw new NotImplementedException();
        }

        private string PasswordMD5()
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] originalBytes = encoder.GetBytes(this.Camera.Recorders[0].Username + ":vrec.elipgo.com:" + this.Camera.Recorders[0].Password);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes).Replace("-", "").ToUpper();
        }

        private string UserBase64()
        {
            var bytes = Encoding.ASCII.GetBytes(this.Camera.Recorders[0].Username);
            return Convert.ToBase64String(bytes, 0, bytes.Length);
        }

        public override IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false)
        {
            var recorder = new RecorderDTO();
            if (this.Camera.RecorderId != 0)
            {
                recorder = this.Camera.Recorders.Where(r => r.Id == this.Camera.RecorderId).First();
            }
            else
            {
                recorder = this.Camera.Recorders[0];
            }

            var client = CreateClient(recorder.HttpProtocol.ToUpper() == "HTTPS");

            var template = $"{client.Endpoint.Address.Uri.Scheme}://{client.Endpoint.Address.Uri.Host}:{client.Endpoint.Address.Uri.Port}";
            var address = $"{recorder.HttpProtocol}://{recorder.Host}:{recorder.HttpPort}";

            var url = client.Endpoint.Address.Uri.ToString().Replace(template, address);
            client.Endpoint.Address = new EndpointAddress(url);
            client.Open();

            string pass = "";
            try
            {
                pass = Security.GetPasswordDecryptJson(recorder.Password);
            }
            catch
            {
                try
                {
                    pass = Security.AESDecrypt(recorder.Password);
                }
                catch
                {
                    pass = recorder.Password;
                }
            }

            IList<TimelineDTO> timeLineList = new List<TimelineDTO>();
            try
            {
                var result = client.loginSystem(recorder.Username, pass);
                VRec4Service.ArrayOfString videoList = null;
                VRec4Service.Status status = client.FileSystem_GetVideoList(Camera.Id.ToString(), ref videoList);
                if (status.Equals(VRec4Service.Status.OK))
                {
                    foreach (string video in videoList)
                    {
                        if (video.Substring(0, 8) == startDate.Replace("-", "").Substring(0, 8) && int.Parse(video.Substring(8, 2)) >= DateTime.Parse(startDate).ToUniversalTime().Hour
                            && int.Parse(video.Substring(8, 2)) <= (DateTime.Parse(endDate).ToUniversalTime().Hour == 0 ? 24 : DateTime.Parse(endDate).ToUniversalTime().Hour))
                        {
                            TimelineDTO timeLine = new TimelineDTO();
                            timeLine.StartTime = video.Substring(0, 4) + "-" + video.Substring(4, 2) + "-" + video.Substring(6, 2) + "T" + video.Substring(8, 2) + ":00:00Z";
                            var endValue = int.Parse(video.Substring(8, 2)) == 23 ? 00 : int.Parse(video.Substring(8, 2)) + 1;
                            var endTime = (video.Substring(0, 8) + endValue.ToString()).Length == video.Length ? video.Substring(0, 8) + endValue.ToString() : video.Substring(0, 8) + "0" + endValue.ToString();
                            timeLine.EndTime = endTime.Substring(0, 4) + "-" + endTime.Substring(4, 2) + "-" + endTime.Substring(6, 2) + "T" + (endValue.ToString().Length == 1 ? "0" + endValue.ToString() : endValue.ToString()) + ":00:00Z";
                            timeLineList.Add(timeLine);
                        }
                    }
                    return timeLineList;
                }
            }
            catch (Exception ex)
            {
                Logger.Log("GetTimeline --> " + ex);
            }
            return timeLineList;
        }

        private VRec4Service.WebServiceConfigurationSoapClient CreateClient(bool https)
        {
            BasicHttpBinding basicHttpbinding = new BasicHttpBinding();
            basicHttpbinding.Name = "WebServiceConfigurationSoap";
            basicHttpbinding.MaxReceivedMessageSize = 2000000;
            basicHttpbinding.MaxBufferSize = 2000000;
            basicHttpbinding.MaxBufferPoolSize = 2000000;
            basicHttpbinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            basicHttpbinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;

            if (https)
            {
                basicHttpbinding.Security.Mode = BasicHttpSecurityMode.Transport;
            }

            EndpointAddress endpointAddress = new EndpointAddress("http://190.111.238.106:81/WebServiceConfigurationVMonitoring/WebServiceConfiguration.asmx");
            return new VRec4Service.WebServiceConfigurationSoapClient(basicHttpbinding, endpointAddress);
        }

        public override string GetFirmwareVersionUri()
        {
            return string.Empty;
        }

    }
}
