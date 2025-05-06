namespace UserServiceAPI.Models
{
    public class AppUserRequest
    {
       // public string Id { get; } // Cosmos DB requires string Id
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // "creator" or "consumer"
    }
}
