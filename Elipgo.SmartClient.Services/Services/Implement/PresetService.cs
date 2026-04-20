using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class PresetService : IPresetService
    {
        public async Task<List<PresetDTO>> GetPresets(CameraDTO camera)
        {
            var response = new List<PresetDTO>();
            try
            {
                var result = await Client.Client.Instance.GetAsync<dynamic>(
                    $"{camera.Host}/axis-cgi/com/ptz.cgi?camera={camera.Id}&query=presetposcam", "");
                string strResult = result.ToString();
                var items = strResult.Split(Convert.ToChar(13));
                foreach (var item in items)
                {
                    if (item.Substring(0, 11) == "presetposno")
                    {
                        int equalIndex = item.IndexOf('=', 0);
                        response.Add(new PresetDTO()
                        {
                            Id = int.Parse(item.Substring(10, equalIndex - 10)),
                            Name = item.Substring(equalIndex)
                        }); ;
                    }
                }
            }
            catch (Exception)
            {
            }
            return response;
        }

        public async Task ChangePreset(CameraDTO camera, int presetNro)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<dynamic>(
                    $"{camera.Host}/axis-cgi/com/ptz.cgi?camera={camera.Id}&gotoserverpresetno={presetNro}", "");
            }
            catch (Exception)
            {
            }
        }


    }
}
