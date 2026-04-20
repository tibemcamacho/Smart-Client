using Elipgo.SmartClient.Common.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IPresetService
    {
        Task<List<PresetDTO>> GetPresets(CameraDTO camera);
        Task ChangePreset(CameraDTO camera, int presetNro);
    }
}
