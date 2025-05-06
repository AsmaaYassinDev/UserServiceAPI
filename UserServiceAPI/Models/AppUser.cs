using System.Text.Json.Serialization;

namespace UserServiceAPI.Models
{
   
    public class AppUser
    {
       
        public string id { get; set; } // Cosmos DB requires string Id
        public string myPartitionKey { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // "creator" or "consumer"
    }
}
