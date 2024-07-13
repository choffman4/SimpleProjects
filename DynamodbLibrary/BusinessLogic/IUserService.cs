using DynamodbLibrary.Models;

namespace DynamodbLibrary.BusinessLogic
{
    public interface IUserService
    {
        Task CreateAccountAsync(string username, string password);
        Task DeleteUserAsync(string username, string password);
        Task UpdatePasswordAsync(string username, string oldPassword, string newPassword);
        Task ValidateUserCredentialsAsync(string username, string password);
    }
}