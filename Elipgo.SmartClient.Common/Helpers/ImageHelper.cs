using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Elipgo.SmartClient.Common.Helpers
{
    public static class ImageHelper
    {
        public static Image Base64ToImage(string base64String, string imageFormat = null)
        {
            if (string.IsNullOrEmpty(base64String))
                return null;

            try
            {
                // Limpiar el formato si viene especificado
                string cleanFormat = CleanImageFormat(imageFormat);

                // Procesar base64String si contiene el prefijo data:image
                string cleanBase64 = base64String;
                if (base64String.Contains("data:image"))
                {
                    var parts = base64String.Split(new[] { ";base64," }, StringSplitOptions.None);
                    if (parts.Length > 1)
                    {
                        cleanBase64 = parts[1];
                        // Si no se proporcionó formato, extraerlo del data:image
                        if (string.IsNullOrEmpty(cleanFormat))
                        {
                            cleanFormat = CleanImageFormat(parts[0]);
                        }
                    }
                }

                byte[] imageBytes = Convert.FromBase64String(cleanBase64);

                // Detectar el formato solo si aún no se ha determinado
                string detectedFormat = !string.IsNullOrEmpty(cleanFormat)
                    ? cleanFormat
                    : DetectImageFormat(imageBytes);

                Console.WriteLine($"Formato detectado/especificado: {detectedFormat}");

                using (var ms = new MemoryStream(imageBytes))
                {
                    // Para JPEG, usar un enfoque específico
                    if (IsJpegFormat(detectedFormat))
                    {
                        try
                        {
                            // Primer intento: usar Bitmap directamente
                            using (var bmp = new Bitmap(ms))
                            {
                                // Crear una copia nueva de la imagen para evitar problemas de memoria
                                return new Bitmap(bmp);
                            }
                        }
                        catch
                        {
                            // Si falla el primer intento, intentar con un método alternativo
                            ms.Position = 0;
                            using (var jpegImage = Image.FromStream(ms, false, true))
                            {
                                // Crear una nueva imagen con el formato correcto
                                var newBitmap = new Bitmap(jpegImage.Width, jpegImage.Height,
                                                         System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                                using (var graphics = Graphics.FromImage(newBitmap))
                                {
                                    graphics.DrawImage(jpegImage, 0, 0, jpegImage.Width, jpegImage.Height);
                                }

                                return newBitmap;
                            }
                        }
                    }
                    else
                    {
                        // Para otros formatos, usar el método estándar
                        return Image.FromStream(ms, false, true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al convertir base64 a imagen: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return null;
            }
        }
        public static string ImageToBase64(Image image)
        {
            byte[] bytes = (byte[])(new ImageConverter()).ConvertTo(image, typeof(byte[]));
            return Convert.ToBase64String(bytes);
        }

        public static byte[] ImageToArray(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();

            bitmap.Save(ms, ImageFormat.Png);

            if (ms == null)
                return null;

            byte[] imageByteArray = ms.ToArray();

            return imageByteArray;
        }

        public static string ImageToBase64Default()
        {
            //return "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";
            return "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";
        }

        public static Image ProcessBase64Response(string httpResponseBody)
        {
            if (string.IsNullOrWhiteSpace(httpResponseBody))
            {
                return Base64ToImage(ImageToBase64Default(), null);
            }
            var trimmed = httpResponseBody.Trim();

            if (trimmed.Equals("null", StringComparison.OrdinalIgnoreCase) ||
                trimmed.Equals("\"null\"", StringComparison.OrdinalIgnoreCase))
            {
                // Devolvemos la imagen default
                return Base64ToImage(ImageToBase64Default(), null);
            }

            try
            {

                var token = JToken.Parse(trimmed);

                if (token.Type == JTokenType.Null)
                {
                    return Base64ToImage(ImageToBase64Default(), null);
                }

                //Si el token es un string, VERIFICAR que el contenido NO sea exactamente "null"
                if (token.Type == JTokenType.String)
                {
                    var value = token.Value<string>() ?? string.Empty;
                    if (value.Equals("null", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(value))
                    {
                        return Base64ToImage(ImageToBase64Default(), null);
                    }

                    trimmed = value;
                }
                else
                {
                    trimmed = token.ToString();
                    if (string.IsNullOrWhiteSpace(trimmed))
                    {
                        return Base64ToImage(ImageToBase64Default(), null);
                    }
                }

                string base64Data = trimmed;
                string imageFormat = "unknown";

                if (trimmed.Contains(";base64,"))
                {
                    var parts = trimmed.Split(new[] { ";base64," }, StringSplitOptions.None);
                    if (parts.Length > 1)
                    {
                        base64Data = parts[1];
                        // Extraer el formato
                        if (parts[0].StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
                        {
                            imageFormat = parts[0]
                                .Replace("data:image/", "")
                                .ToLowerInvariant();
                            // Por ejemplo: "png", "jpeg", etc.
                        }
                    }
                }

                // Limpiar la cadena (quita comillas escapadas, backslashes, ajustar '-' y '_' de Base64 web-safe)
                base64Data = base64Data
                    .Replace("\"", "")
                    .Replace("\\", "")
                    .Replace('-', '+')
                    .Replace('_', '/')
                    .Trim();

                // Corregir el padding si hace falta
                switch (base64Data.Length % 4)
                {
                    case 2:
                        base64Data += "==";
                        break;
                    case 3:
                        base64Data += "=";
                        break;
                }

                return Base64ToImage(base64Data, imageFormat);
            }
            catch (JsonReaderException)
            {
                // Si JToken.Parse falla (no es JSON válido), podría tratarse de que la API devolvió una cadena
                // de Base64 **sin** comillas. En ese caso, asumimos que `trimmed` ya es puro Base64.
                // Reutilizamos trimmed para procesar.
                try
                {
                    // Limpiar trimmed igual que antes
                    var base64Data = trimmed
                        .Replace("\"", "")
                        .Replace("\\", "")
                        .Replace('-', '+')
                        .Replace('_', '/')
                        .Trim();

                    switch (base64Data.Length % 4)
                    {
                        case 2: base64Data += "=="; break;
                        case 3: base64Data += "="; break;
                    }

                    return Base64ToImage(base64Data, null);
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"Error procesando Base64 puro: {ex2.Message}");
                    return Base64ToImage(ImageToBase64Default(), null);
                }
            }
            catch (Exception ex)
            {
                // Cualquier otro error, devolvemos la default (o quizá null, según tu preferencia)
                Console.WriteLine($"Error general procesando respuesta base64: {ex.Message}");
                return Base64ToImage(ImageToBase64Default(), null);
            }
        }

        private static string DetectImageFormat(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
                return "unknown";

            // Verificar firmas de archivos
            if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
            {
                return "jpeg";
            }
            else if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
            {
                return "png";
            }
            else if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
            {
                return "gif";
            }
            else if (bytes[0] == 0x42 && bytes[1] == 0x4D)
            {
                return "bmp";
            }

            return "unknown";
        }

        private static string CleanImageFormat(string format)
        {
            if (string.IsNullOrEmpty(format))
                return null;

            // Remover comillas
            string cleaned = format.Replace("\"", "").Trim();

            // Si contiene data:image/, extraer solo el formato
            if (cleaned.Contains("data:image/"))
            {
                cleaned = cleaned.Split(new[] { "data:image/" }, StringSplitOptions.None)[1];
            }

            // Limpiar cualquier parámetro adicional
            if (cleaned.Contains(";"))
            {
                cleaned = cleaned.Split(';')[0];
            }

            // Normalizar el formato
            cleaned = cleaned.ToLower().Trim();

            return cleaned;
        }

        private static bool IsJpegFormat(string format)
        {
            if (string.IsNullOrEmpty(format))
                return false;

            string cleanFormat = format.ToLower().Trim();
            return cleanFormat == "jpeg" || cleanFormat == "jpg";
        }
    }
}
