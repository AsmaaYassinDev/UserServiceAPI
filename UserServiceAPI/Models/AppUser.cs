namespace UserServiceAPI.Models
{
    public class AppUser
    {
        public string Id { get; set; } // Cosmos DB requires string Id
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; } // "creator" or "consumer"
    }
}
