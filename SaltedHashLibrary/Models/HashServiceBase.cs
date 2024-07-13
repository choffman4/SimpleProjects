using Microsoft.Extensions.Logging;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using System.Security.Cryptography;
using System.Text;

namespace SaltedHashLibrary.Models
{
    public abstract class HashServiceBase
    {
        private readonly ILogger<HashServiceBase> _logger;

        protected HashServiceBase(ILogger<HashServiceBase> logger)
        {
            _logger = logger;
            _logger.LogInformation("HashServiceBase created successfully.");
        }

        protected virtual string GenerateRandomSalt(int saltLength = 16)
        {
            IRandomizerString randomSalt = RandomizerFactory.GetRandomizer(new FieldOptionsTextRegex { Pattern = $"[A-Za-z0-9]{{{saltLength}}}" });

            try
            {
                string generatedSalt = randomSalt.Generate() ?? throw new InvalidOperationException("Generated salt is null.");
                _logger.LogInformation($"Salt generated successfully.");
                return generatedSalt;
            } catch (Exception ex)
            {
                _logger.LogError($"Error generating random salt: {ex.Message}");
                throw;
            }
        }

        protected virtual string GenerateHash(string password, string salt)
        {
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes($"{password}:{salt}");
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);

                    _logger.LogInformation("SHA256 Salted Hash generated successfully.");
                    return hash;
                }
            } catch (Exception ex)
            {
                _logger.LogError($"Error generating SHA256 Salted Hash: {ex.Message}");
                throw;
            }
        }
    }
}