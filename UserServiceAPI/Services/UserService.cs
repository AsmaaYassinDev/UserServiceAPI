using Microsoft.Azure.Cosmos;
//using Microsoft.Azure.Cosmos.Serialization.HybridRow.Schemas;
using Newtonsoft.Json.Linq;

using UserServiceAPI.Models;

namespace UserServiceAPI.Services
{
    public class UserService : IUserService
    {
        private readonly Container _container;
        private readonly CosmosClient _cosmosClient;
        public UserService(CosmosClient cosmosClient, IConfiguration config)
        {
            // Get database and container names from configuration
            _cosmosClient = cosmosClient;

            // Retrieve the CosmosDb settings from configuration
            var cosmosDbConfig = config.GetSection("CosmosDb");
            var databaseName = cosmosDbConfig["DatabaseName"];
            var containerName = cosmosDbConfig["ContainerName"];

            _container = _cosmosClient.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            // Query for all users, paginate if necessary
            var query = _container.GetItemQueryIterator<AppUser>("SELECT * FROM c");
            var results = new List<AppUser>();

            // Use continuation tokens for pagination
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();
                results.AddRange(response);
            }

            return results;
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            try
            {
                // Try reading the user by id with partition key
                ItemResponse<AppUser> response = await _container.ReadItemAsync<AppUser>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                // Return null if user is not found
                return null;
            }
            catch (CosmosException ex)
            {
                // Log other Cosmos DB related exceptions
                throw new Exception($"Error retrieving user: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Handle any other exceptions
                throw new Exception($"An unexpected error occurred: {ex.Message}", ex);
            }
        }

        public async Task AddUserAsync(AppUser user)
        {
            user.myPartitionKey = user.id;
            var response = await _container.CreateItemAsync(user, new Microsoft.Azure.Cosmos.PartitionKey(user.myPartitionKey));
      
        }
    }
}
