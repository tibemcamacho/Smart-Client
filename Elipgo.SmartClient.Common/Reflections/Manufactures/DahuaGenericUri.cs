using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Reflections.Drivers.Dahua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Elipgo.SmartClient.Common.Reflections.Manufactures
{
    public class DahuaGenericUri : DahuaUri
    {
        private fDisConnectCallBack m_DisConnectCallback; //call back for device disconnect.
        protected ManufactureUriAbstract manufactureUri;
        private const int m_WaitTime = 5000;
        public CameraDTO Camera { get; set; }

        public DahuaGenericUri(CameraDTO camera) : base(camera)
        {
            Camera = camera;
            manufactureUri = new DahuaUri(camera);
        }

        public override IList<TimelineDTO> GetTimeline(string startDate, string endDate, bool dst = false)
        {
            List<TimelineDTO> timeLine = new List<TimelineDTO>();
            IntPtr loginId = IntPtr.Zero;
            try
            {
                m_DisConnectCallback = new fDisConnectCallBack(DisconnectCallBack); //set disconnect callback.

                bool initNETClient = NETClient.Init(m_DisConnectCallback, IntPtr.Zero, null);
                if (!initNETClient)
                {
                    throw new Exception("NetClient init failed!");
                }
                NET_DEVICEINFO_Ex deviceInfo = new NET_DEVICEINFO_Ex();

                var dev_port = (UInt16)Camera.VideoPort;
                loginId = NETClient.Login(Camera.Host, dev_port, Camera.User, Camera.Password, EM_LOGIN_SPAC_CAP_TYPE.TCP, IntPtr.Zero, ref deviceInfo);
                if (loginId != IntPtr.Zero)
                {
                    int fileCount = 0;
                    NET_RECORDFILE_INFO[] recordFileArray = new NET_RECORDFILE_INFO[5000];
                    bool ret = QueryFile(loginId, Convert.ToDateTime(startDate.Replace("T", " ").Replace("Z", "")), Convert.ToDateTime(endDate.Replace("T", " ").Replace("Z", "")), ref recordFileArray, ref fileCount);
                    if (!ret)
                    {
                        throw new Exception("Error in to QueryFile!");
                    }
                    if (0 == fileCount)
                    {
                        Logger.Log("None Record file!");
                        return new List<TimelineDTO>();
                    }

                    timeLine = recordFileArray.Where(x => x.starttime.dwYear != 0).GroupBy(f => f.starttime.dwHour).Select(f => new TimelineDTO
                    {
                        StartTime = f.Min(g => g.starttime.ToDateTime()).ToString("yyyy-MM-ddTHH:mm:ss") + "Z",
                        EndTime = f.Max(g => g.endtime.ToDateTime()).ToString("yyyy-MM-ddTHH:mm:ss") + "Z"
                    }).ToList();

                }
                else
                {
                    Logger.Log(string.Format("Device Login {0} is Zero {1} ", Camera.Host + ":" + Camera.HttpPort, NETClient.GetLastError()), LogPriority.Information);
                }
            }
            finally
            {
                NETClient.Logout(loginId);
                NETClient.CloseSound();
                NETClient.StopSaveRealData(loginId);
                NETClient.StopRealPlay(loginId);
                NETClient.Cleanup();
            }
            return timeLine;
        }

        private bool QueryFile(IntPtr loginId, DateTime startTime, DateTime endTime, ref NET_RECORDFILE_INFO[] infos, ref int fileCount)
        {
            //set stream type 设置码流类型
            EM_STREAM_TYPE streamType = EM_STREAM_TYPE.MAIN;
            IntPtr pStream = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)));
            Marshal.StructureToPtr((int)streamType, pStream, true);
            NETClient.SetDeviceMode(loginId, EM_USEDEV_MODE.RECORD_STREAM_TYPE, pStream);
            //query record file 查询录像文件
            bool ret = NETClient.QueryRecordFile(loginId, Camera.Channel - 1, EM_QUERY_RECORD_TYPE.ALL, startTime, endTime, null, ref infos, ref fileCount, m_WaitTime, false);
            if (false == ret)
            {
                return false;
            }
            return true;
        }

        private void DisconnectCallBack(IntPtr lLoginID, IntPtr pchDVRIP, int nDVRPort, IntPtr dwUser)
        {
        }
    }
}
