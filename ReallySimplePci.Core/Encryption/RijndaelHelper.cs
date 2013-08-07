using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace ReallySimplePci.Core.Encryption
{
    public class RijndaelHelper : IDisposable
    {
        private readonly Rijndael _rijndael;
        private readonly UTF8Encoding _encoding;

        public RijndaelHelper(byte[] key, byte[] vector)
        {
            _encoding = new UTF8Encoding();
            _rijndael = Rijndael.Create();
            _rijndael.Key = key;
            _rijndael.IV = vector;
        }

        public static byte[] CreateKey(string password)
        {
            var salt = new byte[] { 1, 2, 23, 234, 23, 41, 136, 63, 248, 9 };
            const int iterations = 9872;

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return rfc2898DeriveBytes.GetBytes(32); // 256bit
            }
        }

        public static byte[] CreateUniqueIv()
        {
            var salt = new byte[] { 1, 9, 23, 23, 23, 41, 64, 12, 248, 26 };
            var password = Membership.GeneratePassword(128, 64);
            const int iterations = 9872;

            using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, iterations))
            {
                return rfc2898DeriveBytes.GetBytes(Rijndael.Create().BlockSize / 8);
            }
        }

        public byte[] Encrypt(string valueToEncrypt)
        {
            var bytes = _encoding.GetBytes(valueToEncrypt);
            using (var encryptor = _rijndael.CreateEncryptor())
            using (var stream = new MemoryStream())
            using (var crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                crypto.Write(bytes, 0, bytes.Length);
                crypto.FlushFinalBlock();
                stream.Position = 0;
                var encrypted = new byte[stream.Length];
                stream.Read(encrypted, 0, encrypted.Length);
                return encrypted;
            }
        }

        public string Decrypt(byte[] encryptedValue)
        {
            using (var decryptor = _rijndael.CreateDecryptor())
            using (var stream = new MemoryStream())
            using (var crypto = new CryptoStream(stream, decryptor, CryptoStreamMode.Write))
            {
                crypto.Write(encryptedValue, 0, encryptedValue.Length);
                crypto.FlushFinalBlock();
                stream.Position = 0;
                var decryptedBytes = new Byte[stream.Length];
                stream.Read(decryptedBytes, 0, decryptedBytes.Length);
                return _encoding.GetString(decryptedBytes);
            }
        }

        public void Dispose()
        {
            if (_rijndael != null)
            {
                _rijndael.Dispose();
            }
        }
    }
}