using SaltedHashLibrary.Models;

namespace SaltedHashLibrary.BusinessLogic
{
    public interface IHashService
    {
        (string SaltedHash, string Salt) GenerateSaltedHash(string password);
        string GenerateSaltedHash(string password, string? salt);
    }
}