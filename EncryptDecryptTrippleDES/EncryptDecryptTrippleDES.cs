using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;


namespace EncryptDecryptTrippleDES
{
    public sealed class EncryptDecryptTrippleDES
    {
        public static string decrypt(string message)
        {
            byte[] keyArr;
            byte[] cipherTextArr = Convert.FromBase64String(message);
            TripleDESCryptoServiceProvider svcProvider = new TripleDESCryptoServiceProvider();
            AppSettingsReader settingsReader = new AppSettingsReader();
            string cipherKey = settingsReader.GetValue("cipherKey", typeof(String)).ToString();
            //keyArr = UTF8Encoding.UTF8.GetBytes(cipherKey);
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArr = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(cipherKey));
            hashmd5.Clear();
            svcProvider.Key = keyArr;
            svcProvider.Mode = CipherMode.ECB;
            svcProvider.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = svcProvider.CreateDecryptor();
            byte[] result = cTransform.TransformFinalBlock(cipherTextArr, 0, cipherTextArr.Length);
            svcProvider.Clear();
            return UTF8Encoding.UTF8.GetString(result);
        }

        public static string encrypt(string message)
        {
            byte[] keyArr;
            byte[] cipherTextArr = UTF8Encoding.UTF8.GetBytes(message);
            TripleDESCryptoServiceProvider svcProvider = new TripleDESCryptoServiceProvider();
            AppSettingsReader settingsReader = new AppSettingsReader();
            string cipherKey = settingsReader.GetValue("cipherKey", typeof(String)).ToString();
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArr = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(cipherKey));
            hashmd5.Clear();
            svcProvider.Key = keyArr;
            svcProvider.Mode = CipherMode.ECB;
            svcProvider.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = svcProvider.CreateEncryptor();
            byte[] result = cTransform.TransformFinalBlock(cipherTextArr, 0, cipherTextArr.Length);
            svcProvider.Clear();
            return Convert.ToBase64String(result, 0, result.Length);
        }
    }
}
