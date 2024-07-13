using SaltedHashLibrary.Models;
using Microsoft.Extensions.Logging;

namespace SaltedHashLibrary.BusinessLogic
{
    public class HashService : HashServiceBase, IHashService
    {
        private readonly ILogger<HashService> _logger;

        public HashService(ILogger<HashService> logger) : base(logger)
        {
            _logger = logger;
            _logger.LogInformation("HashService created successfully.");
        }

        public (string SaltedHash, string Salt) GenerateSaltedHash(string password)
        {
            string salt = GenerateRandomSalt();
            string saltedHash = GenerateHash(password, salt);
            return (saltedHash, salt);
        }

        public string GenerateSaltedHash(string password, string salt)
        {
            return GenerateHash(password, salt);
        }
    }
}
