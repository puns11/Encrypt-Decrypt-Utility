using EncryptDecryptTrippleDES;
using System;

namespace CryptoController
{
    public class CryptoFactory : IDisposable
    {
        private readonly CryptoFactory _context;
        public ICryptoAdapter CreateCryptoAdapter(string cryptoType)
        {
            if (cryptoType == "TrippleDES")
                return new EncryptDecryptTrippleDES.EncryptDecryptTrippleDES();
            else
                return new EncryptDecryptTrippleDES.EncryptDecryptAES();
        }

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }

    }
}
