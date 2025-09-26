using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;


namespace EncryptDecryptTrippleDES
{
    public sealed class EncryptDecryptTrippleDES : ICryptoAdapter
    {
        private readonly EncryptDecryptTrippleDES _context;
        public string Decrypt(string message)
        {
            byte[] keyArr;
            byte[] cipherTextArr = Convert.FromBase64String(message);
            TripleDESCryptoServiceProvider svcProvider = new TripleDESCryptoServiceProvider();            
            string cipherKey = ConfigurationManager.AppSettings["cipherKey"] ?? "POne";
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

        public string Encrypt(string message)
        {
            byte[] keyArr;
            byte[] cipherTextArr = UTF8Encoding.UTF8.GetBytes(message);
            TripleDESCryptoServiceProvider svcProvider = new TripleDESCryptoServiceProvider();
            string cipherKey = ConfigurationManager.AppSettings["cipherKey"] ?? "POne";
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
        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }
    }
}
