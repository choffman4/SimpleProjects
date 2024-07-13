using DynamodbLibrary.Models;

namespace DynamodbLibrary.BusinessLogic
{
    public interface IDynamodbRepository
    {
        Task<bool> DeleteUserAsync(string username);
        Task<UserDTO?> GetUserAsync(string username);
        Task<bool> SaveUserAsync(UserDTO user, bool isNewUser);
    }
}