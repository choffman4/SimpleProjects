using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Newtonsoft.Json;

namespace DynamodbLibrary.BusinessLogic
{
    public class DynamodbService
    {
        private readonly AmazonDynamoDBClient _client;

        public DynamodbService(string accessKeyId, string secretAccessKey, string region)
        {
            var awsCredentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
            var clientConfig = new AmazonDynamoDBConfig 
            { 
                RegionEndpoint = RegionEndpoint.GetBySystemName(region) 
            };
            _client = new AmazonDynamoDBClient(awsCredentials, clientConfig);
        }

        // Create an item
        public async Task CreateItemAsync<T>(string tableName, T item)
        {
            var table = Table.LoadTable(_client, tableName);

            var document = new Document();
            var json = JsonConvert.SerializeObject(item);
            document = Document.FromJson(json);

            await table.PutItemAsync(document);
        }

        // Read an item by primary key
        public async Task<T> GetItemAsync<T>(string tableName, string primaryKey)
        {
            var table = Table.LoadTable(_client, tableName);

            var document = await table.GetItemAsync(primaryKey);

            if (document == null)
            {
                return default(T);
            }

            var json = document.ToJson();
            return JsonConvert.DeserializeObject<T>(json);
        }

        // Update an item
        public async Task UpdateItemAsync<T>(string tableName, T item)
        {
            var table = Table.LoadTable(_client, tableName);

            var json = JsonConvert.SerializeObject(item);
            var document = Document.FromJson(json);

            await table.PutItemAsync(document);
        }

        // Delete an item by primary key
        public async Task DeleteItemAsync(string tableName, string primaryKey)
        {
            var table = Table.LoadTable(_client, tableName);

            await table.DeleteItemAsync(primaryKey);
        }
    }
}
