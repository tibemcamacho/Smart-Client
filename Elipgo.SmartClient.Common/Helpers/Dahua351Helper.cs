using System;
using System.Configuration;
using System.IO;

namespace Elipgo.SmartClient.Common.Helpers
{
    public class Dahua351Helper
    {
        private const int versionSDK = 12;
        public static void InitializeDahuaSdk()
        {
            Configuration config = SmartClientEnvironmentUtils.GetConfiguration();

            var ApplicationLocation = AppDomain.CurrentDomain.BaseDirectory;
            var SourceDahuaSDKLocation = Path.Combine(ApplicationLocation, "Libraries", "Dahua351");
            var TargetDahuaSDKLocation = Path.Combine(ApplicationLocation, "Libraries", "Dahua351");

            if (Directory.Exists(SourceDahuaSDKLocation))
            {
                CopyFilesRecursively(new DirectoryInfo(SourceDahuaSDKLocation), new DirectoryInfo(TargetDahuaSDKLocation));
            }
        }

        private static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target)
        {
            for (int version = 2; version <= versionSDK; version++)
            {
                var targetFullName = target.FullName + $"v{version}";

                if (!Directory.Exists(targetFullName))
                    Directory.CreateDirectory(targetFullName);

                foreach (FileInfo file in source.GetFiles())
                {
                    if (!File.Exists(Path.Combine(targetFullName, file.Name)))
                        file.CopyTo(Path.Combine(targetFullName, file.Name));
                }
            }
        }
    }
}
