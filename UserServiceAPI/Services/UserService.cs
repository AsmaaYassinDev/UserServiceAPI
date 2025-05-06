using Microsoft.Azure.Cosmos;
using UserServiceAPI.Models;

namespace UserServiceAPI.Services
{
    public class UserService:IUserService
    {
        private readonly Container _container;

        public UserService(CosmosClient client, IConfiguration config)
        {
            var databaseName = config["CosmosDb:DatabaseName"];
            var containerName = config["CosmosDb:ContainerName"];
            _container = client.GetContainer(databaseName, containerName);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            var query = _container.GetItemQueryIterator<AppUser>("SELECT * FROM c");
            var results = new List<AppUser>();
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
                ItemResponse<AppUser> response = await _container.ReadItemAsync<AppUser>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch
            {
                return null;
            }
        }

        public async Task AddUserAsync(AppUser user)
        {
            user.Id = Guid.NewGuid().ToString();
            await _container.CreateItemAsync(user, new PartitionKey(user.Id));
        }
    }

}
