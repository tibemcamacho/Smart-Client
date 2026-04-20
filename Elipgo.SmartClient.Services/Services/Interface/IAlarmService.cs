using Elipgo.SmartClient.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Interface
{
    public interface IAlarmService
    {

        Task<List<CardDto>> Get(FilterDTO filter, string token, bool mapMode = false);

        Task<string> GetSnapshot(string token, int alarmId);
        Task<Dictionary<int, string>> GetSnapshot(string token, List<int> alarmIds);

        Task<int> GetUnattended(string token);

        Task<AlarmDTO> Get(string token, int alarmId);

        void Attend(string token, AlarmDTO alarm);

        Task<CardDto> Discard(string token, AlarmDTO alarm);

        Task<List<string>> GetTypesAlarms(string token);

        Task<bool> DiscardAllAlarms(string token, DiscardAllAlarms obj);

        Task<DeviceFeatureDTO> GetDeviceFeatures(string token, int deviceFeatureId);

        Task<List<CardDto>> GetHistory(FilterDTO filter, string token, bool mapMode = false);
        Task<PagedReportDTO<AlarmDTO>> GetFiltered(int device, string type, int site, DateTime startTime, DateTime endTime, int size, int page, string name, string token);

    }
}
