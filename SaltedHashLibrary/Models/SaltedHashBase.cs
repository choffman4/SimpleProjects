using RandomDataGenerator.Randomizers;
using RandomDataGenerator.FieldOptions;
using Microsoft.Extensions.Logging;

namespace SaltedHashLibrary.Models
{
    public abstract class SaltedHashBase : HashBase
    {
        protected string Salt;
        private readonly ILogger _logger;

        public SaltedHashBase(string password, ILogger logger) : base(password)
        {
            _logger = logger;
            Salt = GenerateSalt();
        }

        protected virtual string GenerateSalt(int saltLength)
        {
            IRandomizerString randomSalt = RandomizerFactory.GetRandomizer(new FieldOptionsTextRegex { Pattern = $"[A-Za-z0-9]{{{saltLength}}}" });

            try
            {
                string generatedSalt = randomSalt.Generate() ?? throw new InvalidOperationException("Generated salt is null.");
                _logger.LogInformation($"Salt generated successfully.");
                return generatedSalt;
            } catch (Exception ex)
            {
                _logger.LogError($"Error generating salt: {ex.Message}");
                throw;
            }
        }

        protected virtual string GenerateSalt()
        {
            IRandomizerString randomSalt = RandomizerFactory.GetRandomizer(new FieldOptionsTextRegex { Pattern = "[A-Za-z0-9]{16}" });

            try
            {
                string generatedSalt = randomSalt.Generate() ?? throw new InvalidOperationException("Generated salt is null.");
                _logger.LogInformation($"Salt generated successfully.");
                return generatedSalt;
            } catch (Exception ex)
            {
                _logger.LogError($"Error generating salt: {ex.Message}");
                throw;
            }
        }
    }
}
