using System;
using System.Configuration;
using System.IO;

namespace Elipgo.SmartClient.Common.Helpers
{
    public static class CefSharpHelper
    {
        public static void InitializeCefSharp()
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

            var ApplicationLocation = AppDomain.CurrentDomain.BaseDirectory;
            //var CefSharpInstallLocation = (string)Registry.GetValue(config.AppSettings.Settings["CefSharpInstallLocation"].Value, "InstallLocation", null);
            var CefSharpInstallLocation = Path.Combine(ApplicationLocation, "Libraries", "cef.redist.x64.85.3.13");

            if (Directory.Exists(CefSharpInstallLocation))
            {
                if (!File.Exists(Path.Combine(ApplicationLocation, "libcef.dll")))
                {
                    CopyFilesRecursively(new DirectoryInfo(CefSharpInstallLocation), new DirectoryInfo(ApplicationLocation));
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
