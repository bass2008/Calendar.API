using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Calendar.DAL.Services
{
    public class PermissionService
    {
        private readonly CalendarDbContext _dbContext;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionService(IHttpContextAccessor httpContextAccessor, CalendarDbContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> GetUserId()
        {
            return 3;
            // var emailFromJwt = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;
            // 
            // if (string.IsNullOrWhiteSpace(emailFromJwt))
            //     throw new CalendarNonAuthorizedException();
            // 
            // var user = await _dbContext.Set<User>().FirstOrDefaultAsync(x => x.Email == emailFromJwt);
            // if (user == null)
            //     throw new CalendarException("User from JWT is not found in database");
            // 
            // return user.Id;
        }
    }
}
