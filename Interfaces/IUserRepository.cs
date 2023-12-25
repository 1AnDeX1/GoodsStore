using GoodsStore.Models;

namespace GoodsStore.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetUserByIdAsync(string id);
        AppUser GetUserById(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
