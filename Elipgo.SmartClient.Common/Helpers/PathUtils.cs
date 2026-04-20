using System;
using System.IO;

namespace Elipgo.SmartClient.Common.Helpers
{
    public static class PathUtils
    {
        public static string FilePath(string type)
        {
            string filePath = Common.Properties.Settings.Default["DefaultLocation"].ToString() + @"\" + type + @"\";
            return filePath;
        }

        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        public static string CreateFileName(string type, string extension, string alterFileName = null)
        {
            string fileName = (String.IsNullOrEmpty(alterFileName) ? type + "-" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss") : alterFileName) + "." + extension;
            return fileName;
        }

        public static string CreatePathAndFileName(string type, string extension, string alterFileName = null)
        {
            var path = FilePath(type);
            CreateDirectory(path);
            return Path.Combine(path, CreateFileName(type, extension, alterFileName));
        }

        public static string CreatePathAndFileName(string path, string fileName = null)
        {
            CreateDirectory(path);
            return Path.Combine(path, fileName);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void CleanOldWebView2Caches(string baseCacheFolder, string currentInstanceId)
        {
            if (!Directory.Exists(baseCacheFolder))
            {
                return;
            }

            foreach (var dir in Directory.GetDirectories(baseCacheFolder))
            {
                var folderName = Path.GetFileName(dir);

                // No borrar la carpeta del proceso actual
                if (folderName.Equals(currentInstanceId, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                try
                {
                    Directory.Delete(dir, true);
                }
                catch (IOException) { /* Está en uso */ }
                catch (UnauthorizedAccessException) { /* Sin permisos */ }
                catch (Exception)
                {
                    // Debug.WriteLine($"Error al borrar carpeta {dir}: {ex.Message}");
                }
            }
        }
    }
}
