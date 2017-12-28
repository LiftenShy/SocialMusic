using System;
using System.Security.Cryptography;
using System.Text;

namespace SocialMusic.AccessControl.Helper
{
    internal class CryptoService
    {
        public static byte[] Crypto(string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(password))
                {
                    using (SHA256 sha256 = new SHA256CryptoServiceProvider())
                    {
                        return sha256.ComputeHash(Encoding.ASCII.GetBytes(password));
                    }
                }
                throw new ArgumentException();
            }
            catch (EncoderFallbackException encoderException)
            {
                throw encoderException;
            }
        }

        public static string Decrypto(byte[] cryptoPassword)
        {
            throw new NotImplementedException();
        }
    }
}
