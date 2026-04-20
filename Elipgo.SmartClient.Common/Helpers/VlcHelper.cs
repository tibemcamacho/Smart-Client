using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;

namespace Elipgo.SmartClient.Common.Helpers
{
    public static class VlcHelper
    {
        public static void InitializeVlc()
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

            var applicationLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libraries", "Vlc");
            var vlcInstallLocation = (string)Registry.GetValue(config.AppSettings.Settings["VlcInstallLocation"].Value, "InstallLocation", null);

            if (Directory.Exists(vlcInstallLocation) && File.Exists(Path.Combine(vlcInstallLocation, "libvlc.dll")))
            {
                if (!File.Exists(Path.Combine(applicationLocation, "libvlc.dll")))
                {
                    CopyFilesRecursively(new DirectoryInfo(vlcInstallLocation), new DirectoryInfo(applicationLocation));
                }
            }
        }

        private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            foreach (DirectoryInfo dir in source.GetDirectories())
            {
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name));
            }

            foreach (FileInfo file in source.GetFiles())
            {
                if (!File.Exists(Path.Combine(target.FullName, file.Name)))
                    file.CopyTo(Path.Combine(target.FullName, file.Name));
            }
        }
    }
}
