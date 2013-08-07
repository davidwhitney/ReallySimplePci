using System.IO;
using System.Reflection;
using log4net;

namespace ReallySimplePci.Core.Encryption
{
    public class EncryptionKeys : IEncryptionKeys
    {
        private readonly byte[] _defaultSharedIdEncryptionKey = { 251, 9, 67, 66, 237, 158, 138, 150, 255, 97, 103, 128, 45, 65, 76, 161, 7, 79, 200, 225, 146, 190, 51, 123, 118, 167, 45, 10, 184, 181, 202, 190 };
        private readonly byte[] _defaultPrivateCardNumberEncryptionKey = { 54, 9, 67, 9, 237, 158, 138, 150, 255, 97, 67, 128, 183, 65, 76, 56, 7, 65, 244, 225, 146, 180, 51, 123, 118, 167, 45, 10, 184, 181, 202, 190 };

        private readonly byte[] _sharedIdEncryptionKey;
        private readonly byte[] _privateCardNumberEncryptionKey;

        public byte[] SharedIdEncryptionKey
        {
            get { return _sharedIdEncryptionKey ?? _defaultSharedIdEncryptionKey; }
        }

        public byte[] PrivateCardNumberEncryptionKey
        {
            get { return _privateCardNumberEncryptionKey ?? _defaultPrivateCardNumberEncryptionKey; }
        }

        public EncryptionKeys(ILog log)
        {
            _sharedIdEncryptionKey = LoadKey("SharedIdEncryptionKey.bin");
            _privateCardNumberEncryptionKey = LoadKey("PrivateCardNumberEncryptionKey.bin");

            if (SharedIdEncryptionKey == _defaultPrivateCardNumberEncryptionKey)
            {
                log.Warn("Encryption key for Shared Id encryption is the dev key. Unsuitable for production.");
            }

            if (PrivateCardNumberEncryptionKey == _defaultPrivateCardNumberEncryptionKey)
            {
                log.Warn("Encryption key for card number encryption is the dev key. If this is not a dev environment, data is being ruined");
            }
        }

        private static byte[] LoadKey(string keyfile)
        {
            var installLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(@"file:\", "");
            var appData = Path.Combine(installLocation, "App_Data");
            var location = Path.Combine(appData, keyfile);
            
            if (File.Exists(location))
            {
                return File.ReadAllBytes(location);
            }
            
            return null;
        }
    }
}