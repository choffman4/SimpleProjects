namespace EncryptDecryptLibrary.BusinessLogic
{
    public interface IXORCipher
    {
        public string Decrypt(string encryptedText, string key);
        public string Encrypt(string plainText, string key);
    }
}