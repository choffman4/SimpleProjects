using SaltedHashLibrary.Models;

namespace SaltedHashLibrary.BusinessLogic
{
    public interface IHashService
    {
        SHA256SaltedHash SHA256SaltedHash(string password);
    }
}