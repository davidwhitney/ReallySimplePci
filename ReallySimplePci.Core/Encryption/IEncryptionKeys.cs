namespace ReallySimplePci.Core.Encryption
{
    public interface IEncryptionKeys
    {
        byte[] SharedIdEncryptionKey { get; }
        byte[] PrivateCardNumberEncryptionKey { get; }
    }
}
