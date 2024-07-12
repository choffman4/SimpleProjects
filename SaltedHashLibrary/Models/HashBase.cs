using Microsoft.Extensions.Logging;
using SaltedHashLibrary.BusinessLogic;

namespace SaltedHashLibrary.Models
{
    public abstract class HashBase
    {
        protected string Hash;

        protected abstract string GenerateHash(string password);

        protected HashBase(string password)
        {
            Hash = $"Plaintext password: {password}";
        }
    }
}