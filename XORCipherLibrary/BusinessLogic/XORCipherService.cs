using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptDecryptLibrary.BusinessLogic
{
    public class XORCipherService : IXORCipher
    {
        private readonly ILogger<XORCipherService> _logger;

        public XORCipherService(ILogger<XORCipherService> logger)
        {
            _logger = logger;
        }

        public string Decrypt(string encryptedText, string key)
        {
            encryptedText = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encryptedText));
            return System.Text.Encoding.UTF8.GetString(EncryptDecrypt(encryptedText, key));
        }

        public string Encrypt(string plaintText, string key)
        {
            string encryptedtext = Convert.ToBase64String(EncryptDecrypt(plaintText, key));
            return encryptedtext;
        }

        private byte[] EncryptDecrypt(string text, string key)
        {
            var textArray = text.ToCharArray();
            var keyArray = new char[text.Length];

            // Initialize keyArray with key characters cyclically
            for (int i = 0, k = 0; i < text.Length; i++)
            {
                if (k >= key.Length)
                {
                    k = 0; // Reset k when it exceeds key length
                }

                keyArray[i] = key[k++];
            }

            var result = new byte[text.Length];

            // Encrypt or decrypt using XOR operation
            for (int i = 0; i < text.Length; i++)
            {
                result[i] = (byte)(textArray[i] ^ keyArray[i]);
            }

            return result;
        }
    }
}
