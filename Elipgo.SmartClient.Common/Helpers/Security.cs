using Elipgo.SmartClient.Common.DTOs;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Elipgo.SmartClient.Common.Helpers
{
    public static class Security
    {
        private const string AESKey = "kx6AInMSQOeisBxsatbGMTESbzqtX20yVWGFqc4Oz74=";

        public static string AESDecrypt(string token)
        {
            Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(AESKey);
            aes.IV = Convert.FromBase64String(token.Substring(0, 24));
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            byte[] cipherText = Convert.FromBase64String(token.Substring(24));
            byte[] byteResult = aes.CreateDecryptor().TransformFinalBlock(cipherText, 0, cipherText.Length);

            string result = Encoding.UTF8.GetString(byteResult);
            return result;
        }

        public static string GetVideoHash(string filename)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                char[] trimChars = { '"' };
                var s = Path.GetFileName(filename.Trim(trimChars));
                sb = sb.Append(s).Append("_").Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
                var data = Encoding.UTF8.GetBytes(sb.ToString());
                var key = Encoding.UTF8.GetBytes(Common.Properties.Settings.Default["FFmpegSeed"].ToString());
                using (AesCryptoServiceProvider csp = new AesCryptoServiceProvider())
                {
                    csp.KeySize = 256;
                    csp.BlockSize = 128;
                    csp.Key = key;
                    csp.Padding = PaddingMode.PKCS7;
                    csp.Mode = CipherMode.ECB;
                    ICryptoTransform encrypter = csp.CreateEncryptor();
                    return Convert.ToBase64String(encrypter.TransformFinalBlock(data, 0, data.Length));
                }
            }
            catch (Exception)
            {
                return "";
            }

            //try
            //{
            //    StringBuilder sb = new StringBuilder();
            //    char[] trimChars = { '"' };
            //    var s = Path.GetFileName(filename.Trim(trimChars));
            //    sb = sb.Append(s).Append(DateTime.Now.ToString("yyyyMMddHHmmss"));
            //    var data = Encoding.UTF8.GetBytes(sb.ToString());
            //    Aes aes = Aes.Create();
            //    aes.Key = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["FFmpegSeed"].ToString());
            //    aes.GenerateIV();
            //    aes.Padding = PaddingMode.PKCS7;
            //    aes.Mode = CipherMode.CBC;

            //    byte[] byteResult = aes.CreateEncryptor().TransformFinalBlock(data, 0, data.Length);

            //    string result = Convert.ToBase64String(byteResult);
            //    return Convert.ToBase64String(aes.IV) + result;
            //}catch( Exception ex)
            //{

            //}
            //return "";
        }
        public static string AESDecrypt(string token, string AESKey)
        {
            Aes aes = Aes.Create();
            aes.Key = Convert.FromBase64String(AESKey);
            aes.IV = Convert.FromBase64String(token.Substring(0, 24));
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            byte[] cipherText = Convert.FromBase64String(token.Substring(24));
            byte[] byteResult = aes.CreateDecryptor().TransformFinalBlock(cipherText, 0, cipherText.Length);

            string result = Encoding.UTF8.GetString(byteResult);
            return result;
        }
        public static string GetPasswordDecryptJson(string json)
        {
            var data = AESDecrypt(JsonConvert.DeserializeObject<PasswordDTO>(json).Password, AESKey);
            var pass = JsonConvert.DeserializeObject<PasswordDTO>(data).Password;
            return pass;
        }

        public static string Base64Encode(string textToEncode)
        {
            byte[] textAsBytes = Encoding.UTF8.GetBytes(textToEncode);
            return Convert.ToBase64String(textAsBytes);
        }
    }
}
