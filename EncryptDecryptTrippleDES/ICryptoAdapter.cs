using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptDecryptTrippleDES
{
    public interface ICryptoAdapter : IDisposable
    {
        string Decrypt(string message);
        string Encrypt(string message);

    }
}
