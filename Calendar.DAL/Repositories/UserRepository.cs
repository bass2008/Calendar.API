using Microsoft.EntityFrameworkCore;
using Calendar.DAL.Factories;
using Calendar.DAL.Interfaces;
using Calendar.DAL.Services;
using Calendar.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Calendar.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(PermissionService permissionService, CalendarDbFactory CalendarDbFactory) : base(permissionService, CalendarDbFactory) { }
        
        public async Task UpdateLoginAsync(string email)
        {
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                var user = await dbContext.Set<User>().FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());
                user.LastVisitDate = DateTime.Now.ToUniversalTime();
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<UserPreference> GetUserPreferences(int userId)
        {
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                var data = await dbContext
                    .Set<UserPreference>()
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (data != null)
                    return data;

                return new UserPreference { UserId = userId };
            }
        }
    }
}
