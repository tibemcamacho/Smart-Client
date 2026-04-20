using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;

namespace Elipgo.SmartClient.Common.Helpers
{
    public class MediaSuiteHelper
    {
        public static void InitializeMediaSuite()
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

            var ApplicationLocation = AppDomain.CurrentDomain.BaseDirectory;
            var MediaSuiteInstallLocation = (string)Registry.GetValue(config.AppSettings.Settings["MediaSuiteInstallLocation"].Value, "InstallLocation", null);

            if (Directory.Exists(MediaSuiteInstallLocation) && File.Exists(Path.Combine(MediaSuiteInstallLocation, "MediaSuite.dll")))
            {
                if (!File.Exists(Path.Combine(ApplicationLocation, "MediaSuite.dll")))
                {
                    File.Copy(Path.Combine(MediaSuiteInstallLocation, "MediaSuite.dll"), Path.Combine(ApplicationLocation, "MediaSuite.dll"), true);
                    File.Copy(Path.Combine(MediaSuiteInstallLocation, "MediaBase.dll"), Path.Combine(ApplicationLocation, "MediaBase.dll"), true);
                }
            }
        }
    }
}
