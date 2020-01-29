using Calendar.Domain.Models;
using System.Threading.Tasks;

namespace Calendar.DAL.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<UserPreference> GetUserPreferences(int userId);

        Task UpdateLoginAsync(string email);
    }
}
