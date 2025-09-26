using System;
using System.IO;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;

namespace PgpWrapperLibrary
{
    public static class PgpWrapper
    {
        public static void EncryptFile(string inputFile, string outputFile, string publicKeyFile, bool armor)
        {
            try
            {
                using (var input = File.OpenRead(inputFile))
                using (var output = File.Create(outputFile))
                using (var publicKeyStream = File.OpenRead(publicKeyFile))
                {
                    var publicKey = ReadPublicKey(publicKeyStream);
                    EncryptFile(output, input, publicKey, armor, true);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"PGP Encryption failed: {ex.Message}", ex);
            }
        }

        public static void DecryptFile(string inputFile, string privateKeyFile, char[] password, string outputFile)
        {
            try
            {
                using (var input = File.OpenRead(inputFile))
                using (var output = File.Create(outputFile))
                using (var privateKeyStream = File.OpenRead(privateKeyFile))
                {
                    DecryptFile(input, output, privateKeyStream, password);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"PGP Decryption failed: {ex.Message}", ex);
            }
        }

        private static PgpPublicKey ReadPublicKey(Stream input)
        {
            var pgpPub = new PgpPublicKeyRingBundle(PgpUtilities.GetDecoderStream(input));

            foreach (PgpPublicKeyRing keyRing in pgpPub.GetKeyRings())
            {
                foreach (PgpPublicKey key in keyRing.GetPublicKeys())
                {
                    if (key.IsEncryptionKey)
                    {
                        return key;
                    }
                }
            }

            throw new ArgumentException("Can't find encryption key in key ring.");
        }

        private static PgpPrivateKey FindSecretKey(PgpSecretKeyRingBundle pgpSec, long keyId, char[] pass)
        {
            var pgpSecKey = pgpSec.GetSecretKey(keyId);
            if (pgpSecKey == null)
            {
                return null;
            }

            return pgpSecKey.ExtractPrivateKey(pass);
        }

        private static void EncryptFile(Stream output, Stream input, PgpPublicKey encKey, bool armor, bool withIntegrityCheck)
        {
            if (armor)
            {
                output = new ArmoredOutputStream(output);
            }

            var encGen = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, withIntegrityCheck, new SecureRandom());
            encGen.AddMethod(encKey);

            var encOut = encGen.Open(output, new byte[1 << 16]);

            var comData = new PgpCompressedDataGenerator(CompressionAlgorithmTag.Zip);
            var comOut = comData.Open(encOut);

            var lData = new PgpLiteralDataGenerator();
            var pOut = lData.Open(comOut, PgpLiteralData.Binary, PgpLiteralData.Console, DateTime.UtcNow, new byte[1 << 16]);

            Streams.PipeAll(input, pOut);

            pOut.Close();
            comOut.Close();
            encOut.Close();

            if (armor)
            {
                output.Close();
            }
        }

        private static void DecryptFile(Stream input, Stream output, Stream privateKeyStream, char[] password)
        {
            input = PgpUtilities.GetDecoderStream(input);

            var pgpF = new PgpObjectFactory(input);
            PgpEncryptedDataList enc;

            var o = pgpF.NextPgpObject();
            if (o is PgpEncryptedDataList)
            {
                enc = (PgpEncryptedDataList)o;
            }
            else
            {
                enc = (PgpEncryptedDataList)pgpF.NextPgpObject();
            }

            var sKey = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(privateKeyStream));
            PgpPrivateKey privateKey = null;
            PgpPublicKeyEncryptedData pbe = null;

            foreach (PgpPublicKeyEncryptedData pked in enc.GetEncryptedDataObjects())
            {
                privateKey = FindSecretKey(sKey, pked.KeyId, password);
                if (privateKey != null)
                {
                    pbe = pked;
                    break;
                }
            }

            if (privateKey == null)
            {
                throw new ArgumentException("Secret key for message not found.");
            }

            var clear = pbe.GetDataStream(privateKey);
            var plainFact = new PgpObjectFactory(clear);
            var message = plainFact.NextPgpObject();

            if (message is PgpCompressedData)
            {
                var cData = (PgpCompressedData)message;
                var pgpFact = new PgpObjectFactory(cData.GetDataStream());
                message = pgpFact.NextPgpObject();
            }

            if (message is PgpLiteralData)
            {
                var ld = (PgpLiteralData)message;
                var unc = ld.GetInputStream();
                Streams.PipeAll(unc, output);
            }
            else
            {
                throw new PgpException("Message is not a simple encrypted file - type unknown.");
            }

            if (pbe.IsIntegrityProtected() && !pbe.Verify())
            {
                throw new PgpException("Message failed integrity check");
            }
        }
    }
}