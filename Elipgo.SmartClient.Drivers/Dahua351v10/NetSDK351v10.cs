using Elipgo.SmartClient.Drivers.Dahua351v10.NetSDKCS;
using System;
using System.Runtime.InteropServices;


namespace Elipgo.SmartClient.Drivers.Dahua351v10
{
    public class NetSDK351v10 : AbsNetSDK
    {

        public override bool CLIENT_InitEx(dynamic cbDisConnect, IntPtr dwUser, IntPtr lpInitParam)
        {
            return OriginalSDK.CLIENT_InitEx(cbDisConnect, dwUser, lpInitParam);
        }

        public override int CLIENT_GetLastError()
        {
            return OriginalSDK.CLIENT_GetLastError();
        }

        public override void CLIENT_Cleanup()
        {
            OriginalSDK.CLIENT_Cleanup();
        }
        public override IntPtr CLIENT_LoginEx2(string pchDVRIP, ushort wDVRPort, string pchUserName, string pchPassword, Dahua351.NetSDKCS.EM_LOGIN_SPAC_CAP_TYPE emSpecCap, IntPtr pCapParam, ref Dahua351.NetSDKCS.NET_DEVICEINFO_Ex lpDeviceInfo, ref int error)
        {
            return OriginalSDK.CLIENT_LoginEx2(pchDVRIP, wDVRPort, pchUserName, pchPassword, emSpecCap, pCapParam, ref lpDeviceInfo, ref error);
        }

        public override bool CLIENT_Logout(IntPtr lLoginID)
        {
            return OriginalSDK.CLIENT_Logout(lLoginID);
        }

        public override void CLIENT_SetAutoReconnect(Dahua351.NetSDKCS.fHaveReConnectCallBack cbAutoConnect, IntPtr dwUser)
        {
            OriginalSDK.CLIENT_SetAutoReconnect(cbAutoConnect, dwUser);
        }

        public override void CLIENT_SetNetworkParam(IntPtr pNetParam)
        {
            OriginalSDK.CLIENT_SetNetworkParam(pNetParam);
        }

        public override IntPtr CLIENT_StartRealPlay(IntPtr lLoginID, int nChannelID, IntPtr hWnd, Dahua351.NetSDKCS.EM_RealPlayType rType, Dahua351.NetSDKCS.fRealDataCallBackEx cbRealData, Dahua351.NetSDKCS.fRealPlayDisConnectCallBack cbDisconnect, IntPtr dwUser, uint dwWaitTime)
        {
            return OriginalSDK.CLIENT_StartRealPlay(lLoginID, nChannelID, hWnd, rType, cbRealData, cbDisconnect, dwUser, dwWaitTime);
        }

        public override IntPtr CLIENT_RealPlayEx(IntPtr lLoginID, int nChannelID, IntPtr hWnd, Dahua351.NetSDKCS.EM_RealPlayType rType)
        {
            return OriginalSDK.CLIENT_RealPlayEx(lLoginID, nChannelID, hWnd, rType);
        }
        public override bool CLIENT_StopRealPlayEx(IntPtr lRealHandle)
        {
            return OriginalSDK.CLIENT_StopRealPlayEx(lRealHandle);
        }

        public override bool CLIENT_SetRealDataCallBackEx2(IntPtr lRealHandle, Dahua351.NetSDKCS.fRealDataCallBackEx2 cbRealData, IntPtr dwUser, uint dwFlag)
        {
            return OriginalSDK.CLIENT_SetRealDataCallBackEx2(lRealHandle, cbRealData, dwUser, (uint)dwFlag);
        }
        public override bool CLIENT_SaveRealData(IntPtr lRealHandle, string pchFileName)
        {
            return OriginalSDK.CLIENT_SaveRealData(lRealHandle, pchFileName);
        }
        public override bool CLIENT_StopSaveRealData(IntPtr lRealHandle)
        {
            return OriginalSDK.CLIENT_StopSaveRealData(lRealHandle);
        }
        public override void CLIENT_SetSnapRevCallBack(Dahua351.NetSDKCS.fSnapRevCallBack OnSnapRevMessage, IntPtr dwUser)
        {
            OriginalSDK.CLIENT_SetSnapRevCallBack(OnSnapRevMessage, dwUser);
        }
        public override bool CLIENT_SnapPictureEx(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_SNAP_PARAMS par, IntPtr reserved)
        {
            return OriginalSDK.CLIENT_SnapPictureEx(lLoginID, ref par, reserved);
        }
        public override bool CLIENT_SnapPictureToFile(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_SNAP_PIC_TO_FILE_PARAM inParam, ref Dahua351.NetSDKCS.NET_OUT_SNAP_PIC_TO_FILE_PARAM outParam, int nWaitTime)
        {
            return OriginalSDK.CLIENT_SnapPictureToFile(lLoginID, ref inParam, ref outParam, nWaitTime);
        }
        public override IntPtr CLIENT_PlayBackByTimeEx2(IntPtr lLoginID, int nChannelID, ref Dahua351.NetSDKCS.NET_IN_PLAY_BACK_BY_TIME_INFO pstNetIn, ref Dahua351.NetSDKCS.NET_OUT_PLAY_BACK_BY_TIME_INFO pstNetOut)
        {
            return OriginalSDK.CLIENT_PlayBackByTimeEx2(lLoginID, nChannelID, ref pstNetIn, ref pstNetOut);
        }
        public override bool CLIENT_QueryRecordFile(IntPtr lLoginID, int nChannelId, int nRecordFileType, ref Dahua351.NetSDKCS.NET_TIME tmStart, ref Dahua351.NetSDKCS.NET_TIME tmEnd, string pchCardid, IntPtr nriFileinfo, int maxlen, ref int filecount, int waittime, bool bTime)
        {
            return OriginalSDK.CLIENT_QueryRecordFile(lLoginID, nChannelId, (int)nRecordFileType, ref tmStart, ref tmEnd, pchCardid, nriFileinfo, maxlen, ref filecount, waittime, bTime);
        }
        public override bool CLIENT_GetPlayBackOsdTime(IntPtr lPlayHandle, ref Dahua351.NetSDKCS.NET_TIME lpOsdTime, ref Dahua351.NetSDKCS.NET_TIME lpStartTime, ref Dahua351.NetSDKCS.NET_TIME lpEndTime)
        {
            return OriginalSDK.CLIENT_GetPlayBackOsdTime(lPlayHandle, ref lpOsdTime, ref lpStartTime, ref lpEndTime);
        }
        public override bool CLIENT_CapturePictureEx(IntPtr hPlayHandle, string pchPicFileName, Dahua351.NetSDKCS.EM_NET_CAPTURE_FORMATS eFormat)
        {
            return OriginalSDK.CLIENT_CapturePictureEx(hPlayHandle, pchPicFileName, eFormat);
        }
        public override IntPtr CLIENT_DownloadByTimeEx(IntPtr lLoginID, int nChannelId, int nRecordFileType, ref Dahua351.NetSDKCS.NET_TIME tmStart, ref Dahua351.NetSDKCS.NET_TIME tmEnd, string sSavedFileName,
            Dahua351.NetSDKCS.fTimeDownLoadPosCallBack cbTimeDownLoadPos, IntPtr dwUserData,
            Dahua351.NetSDKCS.fDataCallBack fDownLoadDataCallBack, IntPtr dwDataUser, IntPtr pReserved)
        {
            return OriginalSDK.CLIENT_DownloadByTimeEx(lLoginID, nChannelId, (int)nRecordFileType, ref tmStart, ref tmEnd, sSavedFileName,
                                                     cbTimeDownLoadPos, dwUserData, fDownLoadDataCallBack, dwDataUser, pReserved);
        }
        public override IntPtr CLIENT_DownloadByTime(IntPtr lLoginID, int nChannelId, int nRecordFileType, ref Dahua351.NetSDKCS.NET_TIME tmStart, ref Dahua351.NetSDKCS.NET_TIME tmEnd, string sSavedFileName, Dahua351.NetSDKCS.fTimeDownLoadPosCallBack cbTimeDownLoadPos, IntPtr dwUserData)
        {
            return OriginalSDK.CLIENT_DownloadByTime(lLoginID, nChannelId, (int)nRecordFileType, ref tmStart, ref tmEnd, sSavedFileName,
                                                     cbTimeDownLoadPos, dwUserData);
        }
        public override IntPtr CLIENT_AdaptiveDownloadByTime(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_ADAPTIVE_DOWNLOAD_BY_TIME pstInParam, ref Dahua351.NetSDKCS.NET_OUT_ADAPTIVE_DOWNLOAD_BY_TIME pstOutParam, uint dwWaitTime)
        {
            return OriginalSDK.CLIENT_AdaptiveDownloadByTime(lLoginID, ref pstInParam, ref pstOutParam, dwWaitTime);
        }
        public override bool CLIENT_StopDownload(IntPtr lFileHandle)
        {
            return OriginalSDK.CLIENT_StopDownload(lFileHandle);
        }
        public override bool CLIENT_GetDownloadPos(IntPtr lFileHandle, ref int nTotalSize, ref int nDownLoadSize)
        {
            return OriginalSDK.CLIENT_GetDownloadPos(lFileHandle, ref nTotalSize, ref nDownLoadSize);
        }
        public override bool CLIENT_DHPTZControlEx2(IntPtr lLoginID, int nChannelID, uint dwPTZCommand, int lParam1, int lParam2, int lParam3, bool dwStop, IntPtr param4)
        {
            return OriginalSDK.CLIENT_DHPTZControlEx2(lLoginID, nChannelID, (uint)dwPTZCommand, lParam1, lParam2, lParam3, dwStop, param4);
        }
        public override bool CLIENT_PausePlayBack(IntPtr lPlayHandle, bool bPause)
        {
            return OriginalSDK.CLIENT_PausePlayBack(lPlayHandle, bPause);
        }
        public override bool CLIENT_StopPlayBack(IntPtr lPlayHandle)
        {
            return OriginalSDK.CLIENT_StopPlayBack(lPlayHandle);
        }
        public override bool CLIENT_FastPlayBack(IntPtr lPlayHandle)
        {
            return OriginalSDK.CLIENT_FastPlayBack(lPlayHandle);
        }
        public override bool CLIENT_SlowPlayBack(IntPtr lPlayHandle)
        {
            return OriginalSDK.CLIENT_SlowPlayBack(lPlayHandle);
        }
        public override bool CLIENT_NormalPlayBack(IntPtr lPlayHandle)
        {
            return OriginalSDK.CLIENT_NormalPlayBack(lPlayHandle);
        }
        public override bool CLIENT_SetDeviceMode(IntPtr lLoginID, Dahua351.NetSDKCS.EM_USEDEV_MODE emType, IntPtr pValue)
        {
            return OriginalSDK.CLIENT_SetDeviceMode(lLoginID, emType, pValue);
        }
        public override void CLIENT_SetDVRMessCallBackEx1(Dahua351.NetSDKCS.fMessCallBackEx cbMessage, IntPtr dwUser)
        {
            OriginalSDK.CLIENT_SetDVRMessCallBackEx1(cbMessage, dwUser);
        }
        public override bool CLIENT_StartListenEx(IntPtr lLoginID)
        {
            return OriginalSDK.CLIENT_StartListenEx(lLoginID);
        }
        public override bool CLIENT_StopListen(IntPtr lLoginID)
        {
            return OriginalSDK.CLIENT_StopListen(lLoginID);
        }
        public override IntPtr CLIENT_RealLoadPictureEx(IntPtr lLoginID, int nChannelID, uint dwAlarmType, bool bNeedPicFile, Dahua351.NetSDKCS.fAnalyzerDataCallBack cbAnalyzerData, IntPtr dwUser, IntPtr reserved)
        {
            return OriginalSDK.CLIENT_RealLoadPictureEx(lLoginID, nChannelID, dwAlarmType, bNeedPicFile, cbAnalyzerData, dwUser, reserved);
        }
        public override bool CLIENT_StopLoadPic(IntPtr lAnalyzerHandle)
        {
            return OriginalSDK.CLIENT_StopLoadPic(lAnalyzerHandle);
        }
        public override bool CLIENT_QuerySystemInfo(IntPtr lLoginID, int nSystemType, IntPtr pSysInfoBuffer, int maxlen, ref int nSysInfolen, int waittime)
        {
            return OriginalSDK.CLIENT_QuerySystemInfo(lLoginID, (int)nSystemType, pSysInfoBuffer, maxlen, ref nSysInfolen, waittime);
        }
        public override bool CLIENT_QueryDeviceLog(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_QUERY_DEVICE_LOG_PARAM pQueryParam, IntPtr pLogBuffer, int nLogBufferLen, ref int pRecLogNum, int waittime)
        {
            return OriginalSDK.CLIENT_QueryDeviceLog(lLoginID, ref pQueryParam, pLogBuffer, nLogBufferLen, ref pRecLogNum, waittime);
        }
        public override IntPtr CLIENT_StartTalkEx(IntPtr lLoginID, Dahua351.NetSDKCS.fAudioDataCallBack pfcb, IntPtr dwUser)
        {
            return OriginalSDK.CLIENT_StartTalkEx(lLoginID, pfcb, dwUser);
        }
        public override bool CLIENT_StopTalkEx(IntPtr lTalkHandle)
        {
            return OriginalSDK.CLIENT_StopTalkEx(lTalkHandle);
        }
        public override bool CLIENT_RecordStartEx(IntPtr lLoginID)
        {
            return OriginalSDK.CLIENT_RecordStartEx(lLoginID);
        }
        public override bool CLIENT_RecordStopEx(IntPtr lLoginID)
        {
            return OriginalSDK.CLIENT_RecordStopEx(lLoginID);
        }
        public override int CLIENT_TalkSendData(IntPtr lTalkHandle, IntPtr pSendBuf, uint dwBufSize)
        {
            return OriginalSDK.CLIENT_TalkSendData(lTalkHandle, pSendBuf, dwBufSize);
        }
        public override void CLIENT_AudioDec(IntPtr pAudioDataBuf, uint dwBufSize)
        {
            OriginalSDK.CLIENT_AudioDec(pAudioDataBuf, dwBufSize);
        }
        public override bool CLIENT_ControlDevice(IntPtr lLoginID, Dahua351.NetSDKCS.EM_CtrlType type, IntPtr param, int waittime)
        {
            return OriginalSDK.CLIENT_ControlDevice(lLoginID, type, param, waittime);
        }
        public override bool CLIENT_QueryDevState(IntPtr lLoginID, int nType, IntPtr pBuf, int nBufLen, ref int pRetLen, int waittime)
        {
            return OriginalSDK.CLIENT_QueryDevState(lLoginID, nType, pBuf, nBufLen, ref pRetLen, waittime);
        }
        public override bool CLIENT_QueryNewSystemInfo(IntPtr lLoginID, string szCommand, Int32 nChannelID, IntPtr szOutBuffer, UInt32 dwOutBufferSize, ref UInt32 error, Int32 waittime)
        {
            return OriginalSDK.CLIENT_QueryNewSystemInfo(lLoginID, szCommand, nChannelID, szOutBuffer,
                                                         dwOutBufferSize, ref error, waittime);
        }
        public override bool CLIENT_ParseData(string szCommand, IntPtr szInBuffer, IntPtr lpOutBuffer, UInt32 dwOutBufferSize, IntPtr pReserved)
        {
            return OriginalSDK.CLIENT_ParseData(szCommand, szInBuffer, lpOutBuffer, (UInt32)Marshal.SizeOf(dwOutBufferSize), pReserved);
        }
        public override bool CLIENT_FindRecord(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_FIND_RECORD_PARAM pInParam, ref Dahua351.NetSDKCS.NET_OUT_FIND_RECORD_PARAM pOutParam, int waittime)
        {
            return OriginalSDK.CLIENT_FindRecord(lLoginID, ref pInParam, ref pOutParam, waittime);
        }
        public override int CLIENT_FindNextRecord(ref Dahua351.NetSDKCS.NET_IN_FIND_NEXT_RECORD_PARAM pInParam, ref Dahua351.NetSDKCS.NET_OUT_FIND_NEXT_RECORD_PARAM pOutParam, int waittime)
        {
            return OriginalSDK.CLIENT_FindNextRecord(ref pInParam, ref pOutParam, waittime);
        }
        public override bool CLIENT_FindRecordClose(IntPtr lFindHandle)
        {
            return OriginalSDK.CLIENT_FindRecordClose(lFindHandle);
        }
        public override bool CLIENT_QueryRecordCount(ref Dahua351.NetSDKCS.NET_IN_QUEYT_RECORD_COUNT_PARAM pInParam, ref Dahua351.NetSDKCS.NET_OUT_QUEYT_RECORD_COUNT_PARAM pOutParam, int waittime)
        {
            return OriginalSDK.CLIENT_QueryRecordCount(ref pInParam, ref pOutParam, waittime);
        }
        public override IntPtr CLIENT_StartFindNumberStat(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_FINDNUMBERSTAT pstInParam, ref Dahua351.NetSDKCS.NET_OUT_FINDNUMBERSTAT pstOutParam)
        {
            return OriginalSDK.CLIENT_StartFindNumberStat(lLoginID, ref pstInParam, ref pstOutParam);
        }
        public override int CLIENT_DoFindNumberStat(IntPtr lFindHandle, ref Dahua351.NetSDKCS.NET_IN_DOFINDNUMBERSTAT pstInParam, ref Dahua351.NetSDKCS.NET_OUT_DOFINDNUMBERSTAT pstOutParam)
        {
            return OriginalSDK.CLIENT_DoFindNumberStat(lFindHandle, ref pstInParam, ref pstOutParam);
        }
        public override bool CLIENT_StopFindNumberStat(IntPtr lFindHandle)
        {
            return OriginalSDK.CLIENT_StopFindNumberStat(lFindHandle);
        }
        public override IntPtr CLIENT_AttachVideoStatSummary(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_ATTACH_VIDEOSTAT_SUM pInParam, ref Dahua351.NetSDKCS.NET_OUT_ATTACH_VIDEOSTAT_SUM pOutParam, int nWaitTime)
        {
            return OriginalSDK.CLIENT_AttachVideoStatSummary(lLoginID, ref pInParam, ref pOutParam, nWaitTime);
        }
        public override bool CLIENT_DetachVideoStatSummary(IntPtr lAttachHandle)
        {
            return OriginalSDK.CLIENT_DetachVideoStatSummary(lAttachHandle);
        }
        public override IntPtr CLIENT_CreateTransComChannel(IntPtr lLoginID, int TransComType, uint baudrate, uint databits, uint stopbits, uint parity, Dahua351.NetSDKCS.fTransComCallBack cbTransCom, IntPtr dwUser)
        {
            return OriginalSDK.CLIENT_CreateTransComChannel(lLoginID, TransComType, baudrate, databits, stopbits, parity, cbTransCom, dwUser);
        }
        public override bool CLIENT_SendTransComData(IntPtr lTransComChannel, IntPtr pBuffer, uint dwBufSize)
        {
            return OriginalSDK.CLIENT_SendTransComData(lTransComChannel, pBuffer, (uint)dwBufSize);

        }
        public override bool CLIENT_DestroyTransComChannel(IntPtr lTransComChannel)
        {
            return OriginalSDK.CLIENT_DestroyTransComChannel(lTransComChannel);
        }

        public override bool CLIENT_SetSecurityKey(IntPtr lPlayHandle, string szKey, uint nKeyLen)
        {
            return OriginalSDK.CLIENT_SetSecurityKey(lPlayHandle, szKey, (uint)szKey.Length);
        }

        public override bool CLIENT_OpenSound(IntPtr hPlayHandle)
        {
            return OriginalSDK.CLIENT_OpenSound(hPlayHandle);
        }

        public override bool CLIENT_CloseSound()
        {
            return OriginalSDK.CLIENT_CloseSound();
        }

        public override bool CLIENT_QueryMatrixCardInfo(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_MATRIX_CARD_LIST pstuCardList, int nWaitTime)
        {
            return OriginalSDK.CLIENT_QueryMatrixCardInfo(lLoginID, ref pstuCardList, nWaitTime);
        }

        public override bool CLIENT_SetNewDevConfig(IntPtr lLoginID, string szCommand, int nChannelId, IntPtr szInBuffer, uint dwInBufferSize, ref int error, ref int restart, int waittime)
        {
            return OriginalSDK.CLIENT_SetNewDevConfig(lLoginID, szCommand, nChannelId, szInBuffer, dwInBufferSize, ref error, ref restart, waittime);
        }

        public override bool CLIENT_GetNewDevConfig(IntPtr lLoginId, string szCommand, int nChannelId, IntPtr szOutBUffer, uint dwOutBufferSize, out int error, int nwaitTime)
        {
            return OriginalSDK.CLIENT_GetNewDevConfig(lLoginId, szCommand, nChannelId, szOutBUffer, dwOutBufferSize, out error, nwaitTime);
        }
        public override bool CLIENT_GetSplitCaps(IntPtr lLoginId, int nChannel, ref Dahua351.NetSDKCS.NET_SPLIT_CAPS pstuCaps, int nWaitTime)
        {
            return OriginalSDK.CLIENT_GetSplitCaps(lLoginId, nChannel, ref pstuCaps, nWaitTime);
        }

        public override bool CLIENT_SetSplitMode(IntPtr lLoginID, int nChannel, ref Dahua351.NetSDKCS.NET_SPLIT_MODE_INFO pstuSplitInfo, int nWaitTime)
        {
            return OriginalSDK.CLIENT_SetSplitMode(lLoginID, nChannel, ref pstuSplitInfo, nWaitTime);
        }
        public override bool CLIENT_OpenSplitWindow(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_SPLIT_OPEN_WINDOW pInParam, ref Dahua351.NetSDKCS.NET_OUT_SPLIT_OPEN_WINDOW pOutParam, int nWaitTime)
        {
            return OriginalSDK.CLIENT_OpenSplitWindow(lLoginID, ref pInParam, ref pOutParam, nWaitTime);
        }
        public override bool CLIENT_MatrixGetCameras(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_MATRIX_GET_CAMERAS pInParam, ref Dahua351.NetSDKCS.NET_OUT_MATRIX_GET_CAMERAS pOutParam, int nWaitTime)
        {
            return OriginalSDK.CLIENT_MatrixGetCameras(lLoginID, ref pInParam, ref pOutParam, nWaitTime);
        }
        public override bool CLIENT_SetSplitSource(IntPtr lLoginID, int nChannel, int nWindow, IntPtr pstuSplitSrc, int nSrcCount, int nWaitTime)
        {
            return OriginalSDK.CLIENT_SetSplitSource(lLoginID, nChannel, nWindow, pstuSplitSrc, nSrcCount, nWaitTime);
        }
        public override bool CLIENT_SetSplitSourceEx(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_SET_SPLIT_SOURCE pInparam, ref Dahua351.NetSDKCS.NET_OUT_SET_SPLIT_SOURCE pOutParam, int nWaitTime)
        {
            return OriginalSDK.CLIENT_SetSplitSourceEx(lLoginID, ref pInparam, ref pOutParam, nWaitTime);
        }
        public override bool CLIENT_PacketData(string szCommand, IntPtr lpInBuffer, uint dwInBufferSize, IntPtr szOutBuffer, uint dwOutFufferSize)
        {
            return OriginalSDK.CLIENT_PacketData(szCommand, lpInBuffer, dwInBufferSize, szOutBuffer, dwOutFufferSize);
        }


    }
}
