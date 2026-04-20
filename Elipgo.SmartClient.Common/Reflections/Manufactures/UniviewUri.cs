using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Reflections.Manufactures.Drivers.Uniview.UNVNet231;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{
    public class UniviewUri : ManufactureUriAbstract
    {
        private CameraDTO Device { get; set; }
        public IOPortState State { get; set; }

        int _UnvState = 0;

        public UniviewUri(CameraDTO camera) : base(camera)
        {
        }

        public UniviewUri(CameraDTO camera, Profile profile) : base(camera, profile)
        {
        }

        public override string AudioConfigUri()
        {
            throw new NotImplementedException();
        }

        public override string AudioTrasmitUri()
        {
            throw new NotImplementedException();
        }

        public override string CallGuardUri(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string CallPresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string DeletePresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string ExportRecordingUri(string recordingId, string starttime, string stoptime)
        {
            throw new NotImplementedException();
        }

        public override string GuardTourGetUri(int guardId)
        {
            throw new NotImplementedException();
        }

        public override string GuardTourUri()
        {
            throw new NotImplementedException();
        }

        public override IOPortState InputPortState()
        {
            IOPortState state = IOPortState.Offline;
            DigitalInput(ref state);
            return state;
            //throw new NotImplementedException();
        }

        public override void OuputPortChangeState(IOPortState state)
        {
            DigitalOutputChange(state);
            //throw new NotImplementedException();
        }

        public override IOPortState OuputPortState()
        {
            IOPortState state = IOPortState.Offline;
            DigitalInput(ref state);
            return state;

            // throw new NotImplementedException();
        }

        private void DigitalInput(ref IOPortState state)
        {

            NETDEV_DEVICE_LOGIN_INFO_S pstDevCameraInfo = new NETDEV_DEVICE_LOGIN_INFO_S();
            //NETDEV_SELOG_INFO_S pstSELogInfo = new NETDEV_SELOG_INFO_S();
            pstDevCameraInfo.szIPAddr = Camera.Host; // Device.Host;
            pstDevCameraInfo.dwPort = Camera.HttpPort;
            pstDevCameraInfo.szUserName = Camera.User;
            pstDevCameraInfo.szPassword = Camera.Password;

            IntPtr lpDevHandle = getLoginHandle();


            if (lpDevHandle == IntPtr.Zero)
            {
                Console.WriteLine(Camera.Host + " : " + Camera.HttpPort, "login", NETDEVSDK.NETDEV_GetLastError());
            }
            else
            {
                NETDEV_ALARM_OUTPUT_LIST_S stAlarmOutputList = new NETDEV_ALARM_OUTPUT_LIST_S();
                stAlarmOutputList.astAlarmOutputInfo = new NETDEV_ALARM_OUTPUT_INFO_S[NETDEVSDK.NETDEV_MAX_ALARM_OUT_NUM];

                Int32 dwBytesReturned = 0;
                int iRet = NETDEVSDK.NETDEV_GetDevConfig(getLoginHandle(), Camera.Channel - 1, (int)NETDEV_CONFIG_COMMAND_E.NETDEV_GET_ALARM_OUTPUTCFG, ref stAlarmOutputList, Marshal.SizeOf(stAlarmOutputList), ref dwBytesReturned);
                if (NETDEVSDK.TRUE != iRet)
                {
                    Console.WriteLine(Camera.Host + " chl:" + Camera.Channel, "Get upnp nat state", NETDEVSDK.NETDEV_GetLastError());
                }
                else
                {
                    state = stAlarmOutputList.astAlarmOutputInfo[0].enDefaultStatus == 1 ? IOPortState.Active : IOPortState.Offline;
                    this._UnvState = stAlarmOutputList.astAlarmOutputInfo[0].enDefaultStatus;
                }

                /* Get protocal port */
                //NETDEV_UPNP_NAT_STATE_S stNatState = new NETDEV_UPNP_NAT_STATE_S();
                //int iRet = NETDEVSDK.NETDEV_GetUpnpNatState(lpDevHandle, ref stNatState);
                //if (NETDEVSDK.TRUE != iRet)
                //{
                //    Console.WriteLine(Camera.Host + " chl:" + Camera.Channel, "Get upnp nat state", NETDEVSDK.NETDEV_GetLastError());
                //}
                //else
                //    state = stNatState.astUpnpPort[0].bEnbale == 1 ? IOPortState.Active : IOPortState.Offline;
            }
        }



        public bool DigitalOutputChange(IOPortState _state)
        {
            NETDEV_DEVICE_LOGIN_INFO_S pstDevCameraInfo = new NETDEV_DEVICE_LOGIN_INFO_S();

            pstDevCameraInfo.szIPAddr = Camera.Host;
            pstDevCameraInfo.dwPort = Camera.HttpPort;
            pstDevCameraInfo.szUserName = Camera.User;
            pstDevCameraInfo.szPassword = Camera.Password;


            NETDEV_ALARM_OUTPUT_INFO_S stAlarmOutputInfo = new NETDEV_ALARM_OUTPUT_INFO_S();

            this._UnvState = _state == IOPortState.Active ? 1 : 2;
            stAlarmOutputInfo.dwChancelId = Camera.Channel - 1;
            stAlarmOutputInfo.dwDurationSec = 1;
            stAlarmOutputInfo.szName = "relay_output0";
            stAlarmOutputInfo.enDefaultStatus = this._UnvState;

            IntPtr lpDevHandle = getLoginHandle();
            int iRet = NETDEVSDK.NETDEV_SetDevConfig(lpDevHandle, Camera.Channel, (int)NETDEV_CONFIG_COMMAND_E.NETDEV_SET_ALARM_OUTPUTCFG, ref stAlarmOutputInfo, Marshal.SizeOf(stAlarmOutputInfo));
            if (NETDEVSDK.TRUE != iRet)
            {
                Console.WriteLine(Camera.Host + " chl:" + Camera.Channel, "Get network cfg", NETDEVSDK.NETDEV_GetLastError()); ;
                return false;
            }

            return true;
        }

        public override string PresetListUri()
        {
            throw new NotImplementedException();
        }

        public override string PtzControlUri(string code = "", string param1 = "", string param2 = "", string action = "stop")
        {
            throw new NotImplementedException();
        }

        public override string RecordingPlaybackUri()
        {
            throw new NotImplementedException();
        }

        public override string RemoveGuardTourUri(GuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string SavePresetUri(PresetDTO preset)
        {
            throw new NotImplementedException();
        }

        public override string StopGuardUri(ActivateGuardDTO guard)
        {
            throw new NotImplementedException();
        }

        public override string StreamLiveKurentoUri()
        {
            throw new NotImplementedException();
        }

        public override string StreamLiveUri()
        {
            throw new NotImplementedException();
        }

        public override string StreamPlaybackKurentoUri(DateTime starttime, DateTime stoptime)
        {
            throw new NotImplementedException();
        }

        public override string StreamPlaybackUri()
        {
            throw new NotImplementedException();
        }
        public override IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false)
        {
            IntPtr loginHandle = IntPtr.Zero;
            try
            {
                loginHandle = getLoginHandle();

                IList<TimelineDTO> timeLineList = new List<TimelineDTO>();

                NETDEV_FILECOND_S stFileCond = new NETDEV_FILECOND_S();
                String beginDateTimeStr = getDataTime(DateTime.Parse(startDate).ToUniversalTime());
                String endDateTimeStr = getDataTime(DateTime.Parse(endDate).ToUniversalTime());

                stFileCond.tBeginTime = this.getLongTime(beginDateTimeStr);
                stFileCond.tEndTime = this.getLongTime(endDateTimeStr);
                stFileCond.dwFileType = (int)NETDEV_PLAN_STORE_TYPE_E.NETDEV_TYPE_STORE_TYPE_ALL;
                stFileCond.dwChannelID = Camera.Channel;

                IntPtr dwFileHandle = NETDEVSDK.NETDEV_FindFile(loginHandle, ref stFileCond);
                if (dwFileHandle == IntPtr.Zero)
                    throw new Exception("Find playBack record File fail " + NETDEVSDK.NETDEV_GetLastError());

                List<NETDEV_FINDDATA_S> lsFindData = new List<NETDEV_FINDDATA_S>();
                NETDEV_FINDDATA_S findData = new NETDEV_FINDDATA_S();
                while (NETDEVSDK.TRUE == NETDEVSDK.NETDEV_FindNextFile(dwFileHandle, ref findData))
                {
                    lsFindData.Add(findData);
                }

                if (NETDEVSDK.FALSE == NETDEVSDK.NETDEV_FindClose(dwFileHandle))
                    throw new Exception("Close find playBack record File fail " + NETDEVSDK.NETDEV_GetLastError());

                foreach (NETDEV_FINDDATA_S find_Data in lsFindData)
                {
                    TimelineDTO timeLine = new TimelineDTO();

                    DateTime startDateTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 
                    DateTime beginDateTime = startDateTime.AddSeconds(find_Data.tBeginTime);
                    DateTime endDateTime = startDateTime.AddSeconds(find_Data.tEndTime);

                    timeLine.StartTime = beginDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                    timeLine.EndTime = endDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                    timeLineList.Add(timeLine);
                }
                return timeLineList;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Unview {0} GetTimeline Exception: {1} ", Camera.Name, ex.Message));
            }
            finally
            {
                logout(loginHandle);
            }
        }

        private void logout(IntPtr loginHandle)
        {
            var result = NETDEVSDK.NETDEV_Logout(loginHandle);
            if (result == NETDEVSDK.FALSE)
                throw new Exception(String.Format("Error logged out uniview {0} {1} {2} {3}", Camera.Name, Camera.Host, Camera.HttpPort.ToString(), Camera.User));
        }

        private IntPtr getLoginHandle()
        {
            int iRet = NETDEVSDK.NETDEV_Init();

            if (NETDEVSDK.FALSE == iRet)
                throw new Exception("NetClient init failed!");

            NETDEV_DEVICE_LOGIN_INFO_S pstDevLoginInfo = new NETDEV_DEVICE_LOGIN_INFO_S();
            NETDEV_SELOG_INFO_S pstSELogInfo = new NETDEV_SELOG_INFO_S();
            pstDevLoginInfo.szIPAddr = Camera.Host;
            pstDevLoginInfo.dwPort = Convert.ToInt32(Camera.HttpPort);


            pstDevLoginInfo.szUserName = Camera.User;
            pstDevLoginInfo.szPassword = Camera.Password;
            pstDevLoginInfo.dwLoginProto = (int)NETDEV_LOGIN_PROTO_E.NETDEV_LOGIN_PROTO_ONVIF;

            IntPtr loginHandle = NETDEVSDK.NETDEV_Login_V30(ref pstDevLoginInfo, ref pstSELogInfo);

            if (loginHandle == IntPtr.Zero)
                throw new Exception("Device Login is Zero " + NETDEVSDK.NETDEV_GetLastError());
            else
                return loginHandle;
        }

        private string getDataTime(DateTime date)
        {
            String dateTimeStr = date.Year.ToString();
            dateTimeStr += ("-" + date.Month.ToString());
            dateTimeStr += ("-" + date.Day.ToString());

            dateTimeStr += (" " + date.Hour.ToString());
            dateTimeStr += (":" + date.Minute.ToString());
            dateTimeStr += (":" + date.Second.ToString());

            return dateTimeStr;
        }

        private long getLongTime(String strTime)
        {
            DateTime dateTime = Convert.ToDateTime(strTime);
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 
            return (long)(dateTime - startTime).TotalSeconds;
        }
        public override string GetFirmwareVersionUri()
        {
            return string.Empty;
        }

    }
}
