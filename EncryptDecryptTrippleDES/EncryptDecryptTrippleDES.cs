using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Core;

//using Azure.Security.KeyVault.Secrets;

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
            //AppSettingsReader settingsReader = new AppSettingsReader();
            //string cipherKey = settingsReader.GetValue("cipherKey", typeof(String)).ToString();
            string cipherKey = "POne";
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
            //AppSettingsReader settingsReader = new AppSettingsReader();
            //string cipherKey = settingsReader.GetValue("cipherKey", typeof(String)).ToString();
            string cipherKey = "POne";
            SecretClientOptions options = new SecretClientOptions(){
                Retry =
                    {
                        Delay= TimeSpan.FromSeconds(2),
                        MaxDelay = TimeSpan.FromSeconds(16),
                        MaxRetries = 5,
                        Mode = RetryMode.Exponential
                     }
            };
            var client = new SecretClient(new Uri("https://dctciphertool.vault.azure.net/secrets/cipherKey/56f3968839d042a081ec66dddb42ea91"), new DefaultAzureCredential(), options);
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
