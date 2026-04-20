using Elipgo.SmartClient.Common.Helpers;
using System;
using System.Diagnostics;
using System.IO;

namespace Elipgo.SmartClient.Drivers
{
    public class VideoConversorServices
    {
        private static VideoConversorServices _instance;
        private static readonly object _lock = new object();

        private string FFmpegInstallLocation { get; set; } = string.Empty;

        private VideoConversorServices()
        {
            var exeFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var location = Path.GetDirectoryName(exeFilePath);
            if (location != null)
            {
                FFmpegInstallLocation = Path.Combine(location, "ffmpeg", "ffmpeg.exe");
            }
        }

        public static VideoConversorServices Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new VideoConversorServices();
                    }
                    return _instance;
                }
            }
        }
        public bool ConvertFileMediaToMp4(string fileName)
        {
            bool success;
            try
            {
                if (FFmpegInstallLocation == string.Empty && !File.Exists(FFmpegInstallLocation))
                    return false;

                if (!File.Exists(fileName))
                    return false;

                var directory = Path.GetDirectoryName(fileName);
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                var fileNameMp4 = "\"" + Path.Combine(directory, fileNameWithoutExtension + ".mp4") + "\"";
                var tmpfileName = "\"" + fileName + "\"";
                var hash = "\"" + Security.GetVideoHash(fileNameMp4) + "\"";
                if (File.Exists(fileNameMp4))
                    File.Delete(fileNameMp4);
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = FFmpegInstallLocation,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        Arguments = $" -loglevel quiet -i {tmpfileName} -metadata copyright={hash} -vcodec copy -an {fileNameMp4}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    }
                };

                process.Start();
                success = !process.StandardError.ReadToEnd().Contains("Error");
                process.WaitForExit();
                try
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }
                }
                catch (IOException)
                {
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }
    }
}
