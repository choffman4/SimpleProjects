using SaltedHashLibrary.Models;
using Microsoft.Extensions.Logging;

namespace SaltedHashLibrary.BusinessLogic
{
    public class HashService : IHashService
    {
        private readonly ILogger<HashService> _logger;

        public HashService(ILogger<HashService> logger)
        {
            _logger = logger;
            _logger.LogInformation("HashService created successfully.");
        }

        public SHA256SaltedHash SHA256SaltedHash(string password)
        {
            try
            {
                _logger.LogDebug("Creating SHA256SaltedHash...");

                SHA256SaltedHash saltedHash = new SHA256SaltedHash(password, _logger);

                _logger.LogDebug("SHA256SaltedHash created successfully.");

                return saltedHash;
            } catch (Exception ex)
            {
                _logger.LogError($"Error creating SHA256SaltedHash: {ex.Message}");
                throw;
            }
        }
    }
}
