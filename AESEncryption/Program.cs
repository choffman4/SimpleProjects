using System.Security.Cryptography;
using System.Text;

string text = "i went a little bit mad";
string password = "good_day";

byte[] key = GenerateKeyFromPassword("we will rock you");
byte[] encrypted = AESEncrypt(text, key);

string base64 = EncodeTo64(encrypted);
Console.WriteLine(base64);

byte[] cipherText = DecodeFrom64("pRODkt/Ce3oEQcYF66wmeP+vm+z2Rjdj4XtTKZjNoxc=");
string decrypted = AESDecrypt(cipherText, key);
Console.WriteLine(decrypted);

static byte[] AESEncrypt(string text, byte[] key)
{
    byte[] result;
    using (Aes aes = new AesManaged
    {
        KeySize = 128,
        Key = key,
        BlockSize = 128,
        Mode = CipherMode.ECB,
        Padding = PaddingMode.PKCS7
    })
    {
        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using (MemoryStream memorySteam = new MemoryStream())
        {
            using (CryptoStream cyptoStream = new CryptoStream(memorySteam, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(cyptoStream))
                {
                    streamWriter.Write(text);
                }
                result = memorySteam.ToArray();
            }
        }
    }
    return result;
}

static string AESDecrypt(byte[] cipher, byte[] key)
{
    string plaintext = null;
    using (Aes aes = new AesManaged
    {
        KeySize = 128,
        Key = key,
        BlockSize = 128,
        Mode = CipherMode.ECB,
        Padding = PaddingMode.PKCS7
    })
    {
        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using (MemoryStream memoryStream = new MemoryStream(cipher))
        {
            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
            {
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                {
                    plaintext = streamReader.ReadToEnd();
                }
            }
        }
    }
    return plaintext;
}

static byte[] GenerateKeyFromPassword(string password)
{
    byte[] hash = SHA1.HashData(Encoding.UTF8.GetBytes(password));
    byte[] bytes = new byte[16];

    for (int i = 0; i < bytes.Length; i++)
    {
        bytes[i] = hash[i];
    }

    return bytes;
}

string EncodeTo64(byte[] rawbytes) => Convert.ToBase64String(rawbytes);

byte[] DecodeFrom64(string encodedData) => Convert.FromBase64String(encodedData);
