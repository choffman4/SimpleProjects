using Amazon.DynamoDBv2.DataModel;
using DynamodbLibrary.Models;
using Microsoft.Extensions.Logging;
using SaltedHashLibrary.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DynamodbLibrary.BusinessLogic
{
    public class UserService : IUserService
    {
        private readonly IDynamodbRepository _dynamodbRepository;
        private readonly IHashService _hashService;
        private readonly ILogger<UserService> _logger;

        public UserService(IDynamodbRepository repository, IHashService hashService, ILogger<UserService> logger)
        {
            _logger = logger;
            _dynamodbRepository = repository;
            _hashService = hashService;

            _logger.LogInformation("UserService created successfully.");
        }

        public async Task CreateAccountAsync(string username, string password)
        {
            var (hash, salt) = _hashService.GenerateSaltedHash(password);
            var user = new UserDTO(username, hash, salt);
            try
            {
                await _dynamodbRepository.SaveUserAsync(user, true);
            } catch (Exception e)
            {
                _logger.LogError($"Failed to create user: {username}, {e.Message}");
            }
        }

        public async Task UpdatePasswordAsync(string username, string oldPassword, string newPassword)
        {
            var user = await _dynamodbRepository.GetUserAsync(username);

            if (user != null)
            {
                var oldHash = _hashService.GenerateSaltedHash(oldPassword, user.GetSalt());
                if (oldHash == user.GetHash())
                {
                    var (newHash, newSalt) = _hashService.GenerateSaltedHash(newPassword);
                    var newUserCreds = new UserDTO(username, newHash, newSalt);

                    await _dynamodbRepository.SaveUserAsync(newUserCreds, false);
                    
                } else
                {
                    _logger.LogWarning("Invalid password.");
                }
            }
        }

        public async Task ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await _dynamodbRepository.GetUserAsync(username);

            if (user != null)
            {
                var oldHash = _hashService.GenerateSaltedHash(password, user.GetSalt());
                if (oldHash == user.GetHash())
                {
                    _logger.LogInformation($"User: {username}, validated successfully.");
                } else
                {
                    _logger.LogWarning($"Invalid password.");
                }
            }
        }

        public async Task DeleteUserAsync(string username, string password)
        {
            var user = await _dynamodbRepository.GetUserAsync(username);

            if (user != null)
            {
                var hash = _hashService.GenerateSaltedHash(password, user.GetSalt());
                if (hash == user.GetHash())
                {
                    await _dynamodbRepository.DeleteUserAsync(username);
                } else
                {
                    _logger.LogWarning($"Invalid password.");
                }
            }
        }
    }
}
