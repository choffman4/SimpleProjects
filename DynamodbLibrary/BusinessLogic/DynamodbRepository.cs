using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using DynamodbLibrary.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SaltedHashLibrary.BusinessLogic;

namespace DynamodbLibrary.BusinessLogic
{
    public class DynamodbRepository : IDynamodbRepository
    {
        private readonly ILogger<DynamodbRepository> _logger;
        private readonly HashService _hashService;

        private static string? _accessKeyId;
        private static string? _secretAccessKey;
        private static string? _region;
        private static string? _tableName;

        public DynamodbRepository(string? accessKeyId, string? secretAccessKey, string? region, string? tableName, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DynamodbRepository>();
            _hashService = new HashService(loggerFactory.CreateLogger<HashService>());

            if (string.IsNullOrEmpty(accessKeyId) || string.IsNullOrEmpty(secretAccessKey) || string.IsNullOrEmpty(region) || string.IsNullOrEmpty(tableName))
            {
                throw new ApplicationException("AWS credentials or region not found in environment variables.");
            } else
            {
                _accessKeyId = accessKeyId;
                _secretAccessKey = secretAccessKey;
                _region = region;
                _tableName = tableName;
            }

            _logger.LogInformation("DynamodbRepository created successfully.");
        }

        public async Task<UserDTO?> GetUserAsync(string username)
        {
            using (var client = ConnectToAwsDynamoDb())
            {
                var request = new GetItemRequest
                {
                    TableName = _tableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        { "username", new AttributeValue { S = username } }
                    }
                };

                var response = await client.GetItemAsync(request);

                if (response.Item == null || !response.Item.Any())
                {
                    _logger.LogError($"User: {username}, not found.");
                    return null;
                }

                var user = new UserDTO(response.Item["username"].S, response.Item["hash"].S, response.Item["salt"].S);

                return user;
            }
        }

        public async Task<bool> SaveUserAsync(UserDTO user, bool isNewUser)
        {
            using (var client = ConnectToAwsDynamoDb())
            {
                var item = new Dictionary<string, AttributeValue>
                {
                    { "username", new AttributeValue { S = user.GetUsername() } },
                    { "hash", new AttributeValue { S = user.GetHash() } },
                    { "salt", new AttributeValue { S = user.GetSalt() } }
                };

                var request = new PutItemRequest
                {
                    TableName = _tableName,
                    Item = item
                };

                if (isNewUser)
                {
                    request.ConditionExpression = "attribute_not_exists(username)";
                }

                try
                {
                    var response = await client.PutItemAsync(request);
                    _logger.LogInformation($"User: {user.GetUsername()} {(isNewUser ? "created" : "updated")} successfully.");
                    return true;
                } catch (ConditionalCheckFailedException)
                {
                    _logger.LogWarning($"User: {user.GetUsername()} already exists.");
                    return false;
                } catch (Exception e)
                {
                    _logger.LogError($"Failed to {(isNewUser ? "create" : "update")} user: {user.GetUsername()}, {e.Message}");
                    return false;
                }
            }
        }

        public async Task<bool> DeleteUserAsync(string username)
        {
            using (var client = ConnectToAwsDynamoDb())
            {
                var key = new Dictionary<string, AttributeValue>
                {
                    { "username", new AttributeValue { S = username } }
                };

                var request = new DeleteItemRequest
                {
                    TableName = _tableName,
                    Key = key
                };

                try
                {
                    await client.DeleteItemAsync(request);
                    _logger.LogInformation($"User: {username} deleted successfully.");
                    return true;
                } catch (Exception e)
                {
                    _logger.LogError(e, $"Error deleting user: {username}.");
                    return false;
                }
            }
        }

        #region Private Helper Methods

        // create client connection
        private AmazonDynamoDBClient ConnectToAwsDynamoDb()
        {
            var awsCredentials = new BasicAWSCredentials(_accessKeyId, _secretAccessKey);
            var clientConfig = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(_region)
            };
            return new AmazonDynamoDBClient(awsCredentials, clientConfig);
        }

        #endregion
    }
}
