using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Text;

namespace HackChain.Utilities
{
    public class CryptoUtils
    {
        public static AsymmetricCipherKeyPair GenerateRandomKeys(int keySize = 256)
        {
            ECKeyPairGenerator gen = new ECKeyPairGenerator();
            SecureRandom secureRandom = new SecureRandom();
            KeyGenerationParameters keyGenParam = new KeyGenerationParameters(secureRandom, keySize);
            gen.Init(keyGenParam);

            return gen.GenerateKeyPair();
        }

        //https://stackoverflow.com/questions/29423997/bouncy-castle-sign-and-verify-sha256-certificate-with-c-sharp

        public static string SignData(string msg, ECPrivateKeyParameters privKey)
        {
            try
            {
                byte[] msgBytes = Encoding.UTF8.GetBytes(msg);

                ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
                signer.Init(true, privKey);
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                byte[] sigBytes = signer.GenerateSignature();

                return Convert.ToBase64String(sigBytes);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Signing Failed: " + exc.ToString());
                return null;
            }
        }

        public static bool VerifySignature(ECPublicKeyParameters pubKey, string signature, string msg)
        {
            try
            {
                byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
                byte[] sigBytes = Convert.FromBase64String(signature);

                ISigner signer = SignerUtilities.GetSigner("SHA-256withECDSA");
                signer.Init(false, pubKey);
                signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
                return signer.VerifySignature(sigBytes);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Verification failed with the error: " + exc.ToString());
                return false;
            }
        }
    }
}
