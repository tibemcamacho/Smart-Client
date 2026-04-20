using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Services.Services.Interface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Elipgo.SmartClient.Services.Services.Implement
{
    public class AlarmService : IAlarmService
    {
        public async Task<List<CardDto>> Get(FilterDTO filter, string token, bool mapMode = false)
        {
            try
            {
                if (!mapMode)
                {
                    return await Client.Client.Instance.PostAsync<List<CardDto>>(filter, "/v1/alarms/Appendpage", token);
                }
                else
                {
                    return await Client.Client.Instance.PostAsync<List<CardDto>>(filter, $"/v1/alarms/Appendpage/{true}", token);
                }

            }
            catch (Exception)
            {
                return new List<CardDto>();
            }
        }

        public async Task<string> GetSnapshot(string token, int alarmId)
        {
            try
            {
                var result = await await Client.Client.Instance.GetAsync<dynamic>($"/v1/alarms/snapshot/{alarmId}", token);

                return result as string;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public async Task<Dictionary<int, string>> GetSnapshot(string token, List<int> alarmIds)
        {
            // Validación temprana
            if (alarmIds == null || alarmIds.Count == 0)
                return new Dictionary<int, string>();

            if (string.IsNullOrEmpty(token))
                return new Dictionary<int, string>();

            try
            {
                var result = await Client.Client.Instance.PutAsync<Dictionary<int, string>>(alarmIds, "/v1/alarms/snapshots", token);
                return result ?? new Dictionary<int, string>();
            }
            catch (Exception ex)
            {
                // Loguear para diagnóstico pero no propagar
                System.Diagnostics.Debug.WriteLine($"GetSnapshot error: {ex.Message}");
                return new Dictionary<int, string>();
            }
        }

        public async Task<int> GetUnattended(string token)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<dynamic>($"/v1/alarms/unattended", token);
                return (int)result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUnattended error: {ex.Message}");
                throw;
            }
        }

        public async Task<List<string>> GetTypesAlarms(string token)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<dynamic>($"/v1/alarms/type", token);
                return ((Newtonsoft.Json.Linq.JToken)result).Root.ToObject<List<string>>();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }

        public async Task<AlarmDTO> Get(string token, int alarmId)
        {
            try
            {
                var result = await Client.Client.Instance.GetAsync<AlarmDTO>($"/v1/alarms/{alarmId}", token);

                return result as AlarmDTO;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<DeviceFeatureDTO> GetDeviceFeatures(string token, int deviceFeatureId)
        {
            try
            {
                return await Client.Client.Instance.GetAsync<DeviceFeatureDTO>($"/v1/devicefeatures/{deviceFeatureId}", token);

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async void Attend(string token, AlarmDTO alarm)
        {
            try
            {
                var result = await Client.Client.Instance.PutAsync<dynamic>(alarm, $"/v1/alarms/attend", token);
            }
            catch (Exception ex)
            {
                Logger.Log($"Attend: Error al atender alarma Id={alarm?.Id}: {ex.Message}", LogPriority.Fatal);
            }
        }

        public async Task<CardDto> Discard(string token, AlarmDTO alarm)
        {
            try
            {
                var result = await Client.Client.Instance.PutAsync<CardDto>(alarm, $"/v1/alarms", token);
                return (CardDto)result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DiscardAllAlarms(string token, DiscardAllAlarms obj)
        {
            try
            {
                var result = await Client.Client.Instance.PutAsync<dynamic>(obj, $"/v1/alarms/DiscardAllAlarms", token);
                return (bool)result;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<CardDto>> GetHistory(FilterDTO filter, string token, bool mapMode = false)
        {
            try
            {
                if (!mapMode)
                {
                    return await Client.Client.Instance.PostAsync<List<CardDto>>(filter, "/v1/alarms/GetHistoricAlarmsAppend", token);
                }
                else
                {
                    return await Client.Client.Instance.PostAsync<List<CardDto>>(filter, $"/v1/alarms/GetHistoricAlarmsAppend/{true}", token);
                }

            }
            catch (Exception)
            {
                return new List<CardDto>();
            }
        }

        public async Task<PagedReportDTO<AlarmDTO>> GetFiltered(int device, string type, int site, DateTime startDate, DateTime endDate, int size, int page, string name, string token)
        {
            try
            {
                string startTimeSring = startDate.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                string endTimeString = endDate.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);

                return await Client.Client.Instance.GetAsync<PagedReportDTO<AlarmDTO>>($"/v1/alarms/{device}/{type}/{site}/{startTimeSring}/{endTimeString}/{size}/{page}/{name}", token);
            }
            catch (Exception)
            {
                return new PagedReportDTO<AlarmDTO>();
            }
        }
    }
}
