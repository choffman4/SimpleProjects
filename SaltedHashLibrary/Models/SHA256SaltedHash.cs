using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace SaltedHashLibrary.Models
{
    public class SHA256SaltedHash : SaltedHashBase
    {
        private readonly ILogger _logger;

        public SHA256SaltedHash(string password, ILogger logger) : base(password, logger)
        {
            _logger = logger;
            Hash = GenerateHash(password);
            _logger.LogInformation("SHA256SaltedHash created successfully.");
        }

        public string GetHash()
        {
            return Hash;
        }

        public string GetSalt()
        {
            return Salt;
        }

        protected override string GenerateHash(string password)
        {
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes($"{password}:{Salt}");
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

                    if (string.IsNullOrEmpty(hash))
                    {
                        _logger.LogError("SHA256 hash is null or empty.");
                        throw new InvalidOperationException("Generated SHA256 hash is null or empty.");
                    }

                    _logger.LogInformation("SHA256 hash generated successfully.");
                    return hash;
                }
            } catch (Exception ex)
            {
                _logger.LogError($"Error generating SHA256 hash: {ex.Message}");
                throw;
            }
        }
    }
}
