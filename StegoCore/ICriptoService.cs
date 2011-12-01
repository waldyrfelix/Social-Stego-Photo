namespace StegoCore
{
    public interface ICriptoService
    {
        byte[] Encrypt(string plainText, byte[] key, byte[] iv);
        string Decrypt(byte[] cipherText, byte[] Key, byte[] iv);
    }
}