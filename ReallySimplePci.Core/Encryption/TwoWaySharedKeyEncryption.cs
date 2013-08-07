using System;

namespace ReallySimplePci.Core.Encryption
{
    public class TwoWaySharedKeyEncryption : ITwoWaySharedKeyEncryption
    {
        public string Encrypt(byte[] encryptionKey, string cardNumber)
        {
            var iv = RijndaelHelper.CreateUniqueIv();

            using (var h = new RijndaelHelper(encryptionKey, iv))
            {
                var bytes = h.Encrypt(cardNumber);
                return Convert.ToBase64String(iv) + ":" + Convert.ToBase64String(bytes);
            }
        }

        public string Decrypt(byte[] encryptionKey, string encryptedBlock)
        {
            var parts = encryptedBlock.Split(new []{":"}, StringSplitOptions.None);
            
            var ivbytes = Convert.FromBase64String(parts[0]);
            var valuebytes = Convert.FromBase64String(parts[1]);

            using (var h = new RijndaelHelper(encryptionKey, ivbytes))
            {
                return h.Decrypt(valuebytes);
            }
        }
    }
}