
using UserServiceAPI.Models;

namespace UserServiceAPI.Services
{
    public interface IUserService
    {
        Task<IEnumerable<AppUser>> GetUsersAsync();
        Task<AppUser> GetUserByIdAsync(string id);
        Task AddUserAsync(AppUser user);
    }
}
