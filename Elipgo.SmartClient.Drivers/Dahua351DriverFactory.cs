using Elipgo.SmartClient.Common;
using Elipgo.SmartClient.Common.DTOs;
using Elipgo.SmartClient.Common.Enum;
using Elipgo.SmartClient.Common.Helpers;
using Elipgo.SmartClient.Common.Reflections;
using Elipgo.SmartClient.Drivers.Dahua351.NetSDKCS;
using System;
using System.Collections.Generic;
using System.Linq;
using EnumDrivers = Elipgo.SmartClient.Common.Enum.Drivers;

namespace Elipgo.SmartClient.Drivers
{
    public class Dahua351DriverFactory
    {
        List<DriverMetaData> DahuaDrivers;
        public Dahua351DriverFactory()
        {
            DahuaDrivers = new List<DriverMetaData>()
            {
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v2, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v3, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v4, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v5, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v6, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v7, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v8, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v9, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v10, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v11, CountReference = 0},
             new DriverMetaData{ Driver = EnumDrivers.NETSDK_351v12, CountReference = 0},
            };
        }
        private Object sync = new object();
        private EnumDrivers GetNextDriver(string nameTab)
        {
            lock (this.sync)
            {
                var driver = DahuaDrivers.OrderBy(x => x.CountReference).First();
                if (driver.CountReference >= 60)
                {
                    throw new DahuaMaxDriverReferenceException();
                }
                driver.CountReference++;
                //if (!driver.NameTab.Contains(nameTab))
                driver.NameTab.Add(nameTab);
                return driver.Driver;
            }
        }

        private void ReverseCount(EnumDrivers enumDriver)
        {
            lock (this.sync)
            {
                var driver = DahuaDrivers.Where(x => x.Driver == enumDriver).FirstOrDefault();
                driver.CountReference--;
                Logger.Log(string.Format("Driver {0} dispose event was received, actual referent count is {1}", enumDriver.ToString(), driver.CountReference), LogPriority.Information);
            }
        }

        private void DriverDispose(EnumDrivers enumDriver, string nameTab, dynamic driver)
        {
            try
            {
                lock (this.sync)
                {
                    var driverMetaData = DahuaDrivers.Where(x => x.Driver == enumDriver && x.NameTab != null && x.NameTab.Contains(nameTab)).FirstOrDefault();
                    if (driverMetaData != null && driverMetaData.CountReference > 0)
                    {
                        if (driver is IDriverLive)
                        {
                            (driver as IDriverLive).OnDispose -= DriverDispose;
                        }
                        else if (driver is IDriverInstantPlayback)
                        {
                            (driver as IDriverInstantPlayback).OnDispose -= DriverDispose;
                        }
                        else if (driver is IDriverDownload)
                        {
                            (driver as IDriverDownload).OnDispose -= DriverDispose;
                        }
                        driverMetaData.CountReference--;
                        driverMetaData.NameTab.Remove(nameTab);
                        Logger.Log(string.Format("Driver {0} dispose event was received, actual referent count is {1}, name Tab {2}", enumDriver.ToString(), driverMetaData.CountReference, nameTab), LogPriority.Information);

                    }
                }
            }
            catch (DahuaMaxDriverReferenceException ex)
            {
                Logger.Log(string.Format($" Driver reached the maximum refrence number, no instance driver can be created DahuaMaxDriverReferenceException {ex.Message} "), LogPriority.Fatal);
                throw ex;
            }
            catch (Exception ex)
            {
                //ReverseCount(currentDriver);
                Logger.Log(string.Format($"GetDriverLive Exception, the reference count for driver was reverted  --> ex : {ex.Message}"), LogPriority.Fatal);
                throw ex;
            }

        }

        public IDriverLive GetDriverLive(CameraDTO camera, Profile profile, bool initAudio, string nameTab)
        {

            var currentDriver = GetNextDriver(nameTab);
            try
            {
                NETClient.SetDriver(currentDriver);
                Logger.Log(String.Format(" -----> Camera {0} was assigned to Driver Live {1} Name Tab {2}  ", camera.Name, currentDriver.ToString(), nameTab), LogPriority.Information);
                IDriverLive driver;
                driver = new Dahua351.DahuaLiveUserControl(camera, profile, initAudio, nameTab, currentDriver);
                driver.OnDispose += DriverDispose;
                return driver;
            }
            catch (DahuaMaxDriverReferenceException ex)
            {
                Logger.Log(string.Format(" Driver {0} reached the maximum refrence number, no instance driver can be created", currentDriver), LogPriority.Fatal);
                throw ex;
            }
            catch (Exception ex)
            {
                ReverseCount(currentDriver);
                Logger.Log(string.Format("GetDriverLive Exception {0}, the reference count for driver {1} was reverted ", ex, currentDriver), LogPriority.Fatal);
                throw ex;
            }
        }

        public IDriverInstantPlayback GetDriverInstantPlayback(CameraDTO camera, Profile profile, RecorderDTOSmall recorder, DateTime selectedDateTime, string nameTab, bool hideControls = false, bool isDiagnostic = false,
            DateTime? selectedEndDateTime = null)
        {
            var currentDriver = GetNextDriver(nameTab);
            try
            {
                NETClient.SetDriver(currentDriver);
                Logger.Log(String.Format(" -----> Camera {0} was assigned to Driver Instant Playback {1} Name Tab {2} ", camera.Name, currentDriver.ToString(), nameTab), LogPriority.Information);
                IDriverInstantPlayback driver;
                var offset = DateTimeOffset.Now.Offset.TotalMinutes * -1;
                selectedDateTime = selectedDateTime.AddMinutes(offset).AddMinutes(camera.Gmt);
                driver = new Dahua351.DahuaInstantPlaybackUserControl(camera, profile, hideControls, selectedDateTime, recorder, nameTab, currentDriver, NETClient.driver, isDiagnostic, selectedEndDateTime);
                driver.OnDispose += DriverDispose;
                return driver;
            }
            catch (DahuaMaxDriverReferenceException ex)
            {
                Logger.Log(string.Format(" Driver {0} reached the maximum refrence number, no instance driver can be created", currentDriver), LogPriority.Fatal);
                throw ex;
            }
            catch (Exception ex)
            {
                ReverseCount(currentDriver);
                Logger.Log(string.Format("GetDriverLive Exception {0}, the reference count for driver {1} was reverted ", ex, currentDriver), LogPriority.Fatal);
                throw ex;
            }
        }

        public IManufactureUri GetDriverApiCgi(CameraDTO camera)
        {
            throw new NotImplementedException();
        }

        public IDriverDownload GetDriverDownload(BookmarkGroupElementDTO bookmarkGroupElement, CameraDTO camera, string fileName, bool isEdge = false)
        {
            try
            {
                lock (this.sync)
                {
                    Logger.Log(String.Format(" -----> Camera {0} was assigned to Driver Download Playback {1} ", camera.Name, EnumDrivers.NETSDK_351.ToString()), LogPriority.Information);
                    var driver = DahuaDrivers.Where(x => x.Driver == EnumDrivers.NETSDK_351).FirstOrDefault();
                    if (driver.CountReference >= 60)
                    {
                        throw new DahuaMaxDriverReferenceException();
                    }
                }
                NETClient.SetDriver(EnumDrivers.NETSDK_351);
                IDriverDownload dr = new Dahua351.DahuaDownload(bookmarkGroupElement, camera, fileName);
                dr.OnDispose += DriverDispose;
                return dr;
            }
            catch (DahuaMaxDriverReferenceException ex)
            {
                Logger.Log(string.Format(" Driver {0} reached the maximum refrence number, no instance driver can be created", EnumDrivers.NETSDK_351.ToString()), LogPriority.Fatal);
                throw ex;
            }
            catch (Exception ex)
            {
                ReverseCount(EnumDrivers.NETSDK_351);
                Logger.Log(string.Format("GetDriverLive Exception {0}, the reference count for driver {1} was reverted ", ex, EnumDrivers.NETSDK_351), LogPriority.Fatal);
                throw ex;
            }
        }

        public IDriverDownloadVisualSearch GetDriverVisualSerachDownload(CameraDTO camera)
        {
            try
            {
                lock (this.sync)
                {
                    Logger.Log(String.Format(" -----> Camera {0} was assigned to Driver Visual Search Download Playback {1} ", camera.Name, EnumDrivers.NETSDK_351.ToString()), LogPriority.Information);
                    var driver = DahuaDrivers.Where(x => x.Driver == EnumDrivers.NETSDK_351).FirstOrDefault();
                    if (driver.CountReference >= 60)
                    {
                        throw new DahuaMaxDriverReferenceException();
                    }
                }
                NETClient.SetDriver(EnumDrivers.NETSDK_351);
                IDriverDownloadVisualSearch dr = new Dahua351.DahuaDownloadVisualSearch(camera);
                dr.OnDispose += DriverDispose;
                return dr;
            }
            catch (DahuaMaxDriverReferenceException ex)
            {
                Logger.Log(string.Format(" Driver {0} reached the maximum refrence number, no instance driver can be created", EnumDrivers.NETSDK_351.ToString()), LogPriority.Fatal);
                throw ex;
            }
            catch (Exception ex)
            {
                ReverseCount(EnumDrivers.NETSDK_351);
                Logger.Log(string.Format("GetDriverLive Exception {0}, the reference count for driver {1} was reverted ", ex, EnumDrivers.NETSDK_351), LogPriority.Fatal);
                throw ex;
            }
        }
    }
}
