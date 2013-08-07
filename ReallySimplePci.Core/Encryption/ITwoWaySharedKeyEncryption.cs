namespace ReallySimplePci.Core.Encryption
{
    public interface ITwoWaySharedKeyEncryption
    {
        string Encrypt(byte[] encryptionKey, string cardNumber);
        string Decrypt(byte[] encryptionKey, string encryptedBlock);
    }
}