using Microsoft.EntityFrameworkCore;
using Calendar.DAL.Factories;
using Calendar.DAL.Interfaces;
using Calendar.DAL.Services;
using Calendar.Domain;
using Calendar.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Calendar.DAL.Repositories
{
    public class UserOwnedGenericRepository<T> : GenericRepository<T>, IUserOwnedGenericRepository<T> where T : class, IUserOwnedElement
    {
        public UserOwnedGenericRepository(PermissionService permissionService, CalendarDbFactory CalendarDbFactory) : base(permissionService, CalendarDbFactory)
        {
        }

        private async Task<Expression<Func<T, bool>>> GetFilterByUserId()
        {
            var userId = await _permissionService.GetUserId();
            Expression<Func<T, bool>> filterByUserId = x => x.UserId == userId;
            return filterByUserId;
        }

        public override async Task<int> CountAsync()
        {
            var filterByUserId = await GetFilterByUserId();
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .Where(filterByUserId)
                    .CountAsync();
            }
        }

        public override async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            var filterByUserId = await GetFilterByUserId();
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .Where(filterByUserId)
                    .Where(filter)
                    .CountAsync();
            }
        }

        public override async Task<List<T>> GetAllAsync(int page, int pageSize, Expression<Func<T, bool>> filter)
        {
            var filterByUserId = await GetFilterByUserId();
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .Where(filterByUserId)
                    .Where(filter)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
        }

        public override async Task<List<T>> GetAllAsync(int page, int pageSize)
        {
            var filterByUserId = await GetFilterByUserId();
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .Where(filterByUserId)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
        }

        public override async Task<List<T>> GetAllAsync()
        {
            var filterByUserId = await GetFilterByUserId();
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .Where(filterByUserId)
                    .ToListAsync();
            }
        }

        public virtual async Task<T> GetSingleUserOwnedAsync()
        {
            var filterByUserId = await GetFilterByUserId();
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                var item = await dbContext.Set<T>()
                    .Where(filterByUserId)
                    .FirstOrDefaultAsync();

                if (item != null)
                    return item;

                var elem = (T)Activator.CreateInstance(typeof(T));
                elem.UserId = await _permissionService.GetUserId();
                return elem;
            }
        }

        public virtual async Task AddOrUpdateSingleUserOwnedAsync(T entity)
        {
            if (entity is IUserOwnedElement userOwned)
            {
                var userId = await _permissionService.GetUserId();
                userOwned.UserId = userId;

                var filterByUserId = await GetFilterByUserId();
                using (var dbContext = _CalendarDbFactory.CreateDbContext())
                {
                    var item = await dbContext.Set<T>()
                        .Where(filterByUserId)
                        .FirstOrDefaultAsync();

                    if (item == null)
                        await AddAsync(entity);
                    else
                        await UpdateAsync(item.Id, entity);

                }
            }
            else
                throw new CalendarException($"Type {typeof(T).FullName} is not assignable from IUserOwnedElement");

        }

        public override async Task<T> GetAsync(int id)
        {
            var filterByUserId = await GetFilterByUserId();
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .Where(filterByUserId)
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
        }
    }
}
