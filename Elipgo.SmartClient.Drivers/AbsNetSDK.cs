using Elipgo.SmartClient.Drivers.Dahua351.NetSDKCS;
using System;

namespace Elipgo.SmartClient.Drivers
{
    public abstract class AbsNetSDK
    {
        public abstract bool CLIENT_InitEx(dynamic cbDisConnect, IntPtr dwUser, IntPtr lpInitParam);
        public abstract int CLIENT_GetLastError();
        public abstract void CLIENT_Cleanup();
        public abstract IntPtr CLIENT_LoginEx2(string pchDVRIP, ushort wDVRPort, string pchUserName, string pchPassword, EM_LOGIN_SPAC_CAP_TYPE emSpecCap, IntPtr pCapParam, ref NET_DEVICEINFO_Ex lpDeviceInfo, ref int error);
        public abstract bool CLIENT_Logout(IntPtr lLoginID);
        public abstract void CLIENT_SetAutoReconnect(fHaveReConnectCallBack cbAutoConnect, IntPtr dwUser);
        public abstract void CLIENT_SetNetworkParam(IntPtr pNetParam);
        public abstract IntPtr CLIENT_StartRealPlay(IntPtr lLoginID, int nChannelID, IntPtr hWnd, EM_RealPlayType rType, fRealDataCallBackEx cbRealData, fRealPlayDisConnectCallBack cbDisconnect, IntPtr dwUser, uint dwWaitTime);
        public abstract IntPtr CLIENT_RealPlayEx(IntPtr lLoginID, int nChannelID, IntPtr hWnd, EM_RealPlayType rType);
        public abstract bool CLIENT_StopRealPlayEx(IntPtr lRealHandle);
        public abstract bool CLIENT_SetRealDataCallBackEx2(IntPtr lRealHandle, fRealDataCallBackEx2 cbRealData, IntPtr dwUser, uint dwFlag);
        public abstract bool CLIENT_SaveRealData(IntPtr lRealHandle, string pchFileName);
        public abstract bool CLIENT_StopSaveRealData(IntPtr lRealHandle);
        public abstract void CLIENT_SetSnapRevCallBack(fSnapRevCallBack OnSnapRevMessage, IntPtr dwUser);
        public abstract bool CLIENT_SnapPictureEx(IntPtr lLoginID, ref NET_SNAP_PARAMS par, IntPtr reserved);
        public abstract bool CLIENT_SnapPictureToFile(IntPtr lLoginID, ref NET_IN_SNAP_PIC_TO_FILE_PARAM inParam, ref NET_OUT_SNAP_PIC_TO_FILE_PARAM outParam, int nWaitTime);
        public abstract IntPtr CLIENT_PlayBackByTimeEx2(IntPtr lLoginID, int nChannelID, ref NET_IN_PLAY_BACK_BY_TIME_INFO pstNetIn, ref NET_OUT_PLAY_BACK_BY_TIME_INFO pstNetOut);
        public abstract bool CLIENT_QueryRecordFile(IntPtr lLoginID, int nChannelId, int nRecordFileType, ref NET_TIME tmStart, ref NET_TIME tmEnd, string pchCardid, IntPtr nriFileinfo, int maxlen, ref int filecount, int waittime, bool bTime);
        public abstract bool CLIENT_GetPlayBackOsdTime(IntPtr lPlayHandle, ref NET_TIME lpOsdTime, ref NET_TIME lpStartTime, ref NET_TIME lpEndTime);
        public abstract bool CLIENT_CapturePictureEx(IntPtr hPlayHandle, string pchPicFileName, EM_NET_CAPTURE_FORMATS eFormat);
        public abstract IntPtr CLIENT_DownloadByTimeEx(IntPtr lLoginID, int nChannelId, int nRecordFileType, ref NET_TIME tmStart, ref NET_TIME tmEnd, string sSavedFileName,
            fTimeDownLoadPosCallBack cbTimeDownLoadPos, IntPtr dwUserData,
            fDataCallBack fDownLoadDataCallBack, IntPtr dwDataUser, IntPtr pReserved);
        public abstract IntPtr CLIENT_DownloadByTime(IntPtr lLoginID, int nChannelId, int nRecordFileType, ref NET_TIME tmStart, ref NET_TIME tmEnd, string sSavedFileName, fTimeDownLoadPosCallBack cbTimeDownLoadPos, IntPtr dwUserData);
        public abstract IntPtr CLIENT_AdaptiveDownloadByTime(IntPtr lLoginID, ref NET_IN_ADAPTIVE_DOWNLOAD_BY_TIME pstInParam, ref NET_OUT_ADAPTIVE_DOWNLOAD_BY_TIME pstOutParam, uint dwWaitTime);
        public abstract bool CLIENT_StopDownload(IntPtr lFileHandle);
        public abstract bool CLIENT_GetDownloadPos(IntPtr lFileHandle, ref int nTotalSize, ref int nDownLoadSize);
        public abstract bool CLIENT_DHPTZControlEx2(IntPtr lLoginID, int nChannelID, uint dwPTZCommand, int lParam1, int lParam2, int lParam3, bool dwStop, IntPtr param4);
        public abstract bool CLIENT_PausePlayBack(IntPtr lPlayHandle, bool bPause);
        public abstract bool CLIENT_StopPlayBack(IntPtr lPlayHandle);
        public abstract bool CLIENT_FastPlayBack(IntPtr lPlayHandle);
        public abstract bool CLIENT_SlowPlayBack(IntPtr lPlayHandle);
        public abstract bool CLIENT_NormalPlayBack(IntPtr lPlayHandle);
        public abstract bool CLIENT_SetDeviceMode(IntPtr lLoginID, EM_USEDEV_MODE emType, IntPtr pValue);
        public abstract void CLIENT_SetDVRMessCallBackEx1(fMessCallBackEx cbMessage, IntPtr dwUser);
        public abstract bool CLIENT_StartListenEx(IntPtr lLoginID);
        public abstract bool CLIENT_StopListen(IntPtr lLoginID);
        public abstract IntPtr CLIENT_RealLoadPictureEx(IntPtr lLoginID, int nChannelID, uint dwAlarmType, bool bNeedPicFile, fAnalyzerDataCallBack cbAnalyzerData, IntPtr dwUser, IntPtr reserved);
        public abstract bool CLIENT_StopLoadPic(IntPtr lAnalyzerHandle);
        public abstract bool CLIENT_QuerySystemInfo(IntPtr lLoginID, int nSystemType, IntPtr pSysInfoBuffer, int maxlen, ref int nSysInfolen, int waittime);
        public abstract bool CLIENT_QueryDeviceLog(IntPtr lLoginID, ref NET_QUERY_DEVICE_LOG_PARAM pQueryParam, IntPtr pLogBuffer, int nLogBufferLen, ref int pRecLogNum, int waittime);
        public abstract IntPtr CLIENT_StartTalkEx(IntPtr lLoginID, fAudioDataCallBack pfcb, IntPtr dwUser);
        public abstract bool CLIENT_StopTalkEx(IntPtr lTalkHandle);
        public abstract bool CLIENT_RecordStartEx(IntPtr lLoginID);
        public abstract bool CLIENT_RecordStopEx(IntPtr lLoginID);
        public abstract int CLIENT_TalkSendData(IntPtr lTalkHandle, IntPtr pSendBuf, uint dwBufSize);
        public abstract void CLIENT_AudioDec(IntPtr pAudioDataBuf, uint dwBufSize);
        public abstract bool CLIENT_ControlDevice(IntPtr lLoginID, EM_CtrlType type, IntPtr param, int waittime);
        public abstract bool CLIENT_QueryDevState(IntPtr lLoginID, int nType, IntPtr pBuf, int nBufLen, ref int pRetLen, int waittime);
        public abstract bool CLIENT_QueryNewSystemInfo(IntPtr lLoginID, string szCommand, Int32 nChannelID, IntPtr szOutBuffer, UInt32 dwOutBufferSize, ref UInt32 error, Int32 waittime);
        public abstract bool CLIENT_ParseData(string szCommand, IntPtr szInBuffer, IntPtr lpOutBuffer, UInt32 dwOutBufferSize, IntPtr pReserved);
        public abstract bool CLIENT_FindRecord(IntPtr lLoginID, ref NET_IN_FIND_RECORD_PARAM pInParam, ref NET_OUT_FIND_RECORD_PARAM pOutParam, int waittime);
        public abstract int CLIENT_FindNextRecord(ref NET_IN_FIND_NEXT_RECORD_PARAM pInParam, ref NET_OUT_FIND_NEXT_RECORD_PARAM pOutParam, int waittime);
        public abstract bool CLIENT_FindRecordClose(IntPtr lFindHandle);
        public abstract bool CLIENT_QueryRecordCount(ref NET_IN_QUEYT_RECORD_COUNT_PARAM pInParam, ref NET_OUT_QUEYT_RECORD_COUNT_PARAM pOutParam, int waittime);
        public abstract IntPtr CLIENT_StartFindNumberStat(IntPtr lLoginID, ref NET_IN_FINDNUMBERSTAT pstInParam, ref NET_OUT_FINDNUMBERSTAT pstOutParam);
        public abstract int CLIENT_DoFindNumberStat(IntPtr lFindHandle, ref NET_IN_DOFINDNUMBERSTAT pstInParam, ref NET_OUT_DOFINDNUMBERSTAT pstOutParam);
        public abstract bool CLIENT_StopFindNumberStat(IntPtr lFindHandle);
        public abstract IntPtr CLIENT_AttachVideoStatSummary(IntPtr lLoginID, ref NET_IN_ATTACH_VIDEOSTAT_SUM pInParam, ref NET_OUT_ATTACH_VIDEOSTAT_SUM pOutParam, int nWaitTime);
        public abstract bool CLIENT_DetachVideoStatSummary(IntPtr lAttachHandle);
        public abstract IntPtr CLIENT_CreateTransComChannel(IntPtr lLoginID, int TransComType, uint baudrate, uint databits, uint stopbits, uint parity, fTransComCallBack cbTransCom, IntPtr dwUser);
        public abstract bool CLIENT_SendTransComData(IntPtr lTransComChannel, IntPtr pBuffer, uint dwBufSize);
        public abstract bool CLIENT_DestroyTransComChannel(IntPtr lTransComChannel);
        public abstract bool CLIENT_SetSecurityKey(IntPtr lPlayHandle, string szKey, uint nKeyLen);
        public abstract bool CLIENT_OpenSound(IntPtr hPlayHandle);

        public abstract bool CLIENT_CloseSound();

        public abstract bool CLIENT_QueryMatrixCardInfo(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_MATRIX_CARD_LIST pstuCardList, int nWaitTime);
        public abstract bool CLIENT_SetNewDevConfig(IntPtr lLoginID, string szCommand, int nChannelId, IntPtr szInBuffer, uint dwInBufferSize, ref int error, ref int restart, int waittime);

        public abstract bool CLIENT_GetNewDevConfig(IntPtr lLoginId, string szCommand, int nChannelId, IntPtr szOutBUffer, uint dwOutBufferSize, out int error, int nwaitTime);
        public abstract bool CLIENT_GetSplitCaps(IntPtr lLoginId, int nChannel, ref Dahua351.NetSDKCS.NET_SPLIT_CAPS pstuCaps, int nWaitTime);
        public abstract bool CLIENT_SetSplitMode(IntPtr lLoginID, int nChannel, ref Dahua351.NetSDKCS.NET_SPLIT_MODE_INFO pstuSplitInfo, int nWaitTime);
        public abstract bool CLIENT_OpenSplitWindow(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_SPLIT_OPEN_WINDOW pInParam, ref Dahua351.NetSDKCS.NET_OUT_SPLIT_OPEN_WINDOW pOutParam, int nWaitTime);
        public abstract bool CLIENT_MatrixGetCameras(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_MATRIX_GET_CAMERAS pInParam, ref Dahua351.NetSDKCS.NET_OUT_MATRIX_GET_CAMERAS pOutParam, int nWaitTime);
        public abstract bool CLIENT_SetSplitSource(IntPtr lLoginID, int nChannel, int nWindow, IntPtr pstuSplitSrc, int nSrcCount, int nWaitTime);
        public abstract bool CLIENT_SetSplitSourceEx(IntPtr lLoginID, ref Dahua351.NetSDKCS.NET_IN_SET_SPLIT_SOURCE pInparam, ref Dahua351.NetSDKCS.NET_OUT_SET_SPLIT_SOURCE pOutParam, int nWaitTime);
        public abstract bool CLIENT_PacketData(string szCommand, IntPtr lpInBuffer, uint dwInBufferSize, IntPtr szOutBuffer, uint dwOutFufferSize);
    }
}
