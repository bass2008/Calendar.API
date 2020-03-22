using Microsoft.EntityFrameworkCore;
using Calendar.DAL.Interfaces;
using Calendar.DAL.Services;
using Calendar.Domain;
using Calendar.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Calendar.DAL.Factories;

namespace Calendar.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IDbElement
    {
        protected readonly PermissionService _permissionService;

        protected readonly CalendarDbFactory _CalendarDbFactory;

        public GenericRepository(PermissionService permissionService, CalendarDbFactory CalendarDbFactory)
        {
            _permissionService = permissionService;
            _CalendarDbFactory = CalendarDbFactory;
        }

        public virtual async Task<int> CountAsync()
        {
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {                
                return await dbContext.Set<T>()                   
                    .CountAsync();
            }
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {            
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .Where(filter)
                    .CountAsync();
            }
        }

        public virtual async Task<List<T>> GetAllAsync(int page, int pageSize, Expression<Func<T, bool>> filter)
        {
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .Where(filter)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
        }

        public virtual async Task<List<T>> GetAllAsync(int page, int pageSize)
        {
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .ToListAsync();
            }
        }

        public virtual async Task<T> GetAsync(int id)
        {
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                return await dbContext.Set<T>()
                    .FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public virtual async Task UpdateAsync(int id, T entity)
        {
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                var existingEntity = await dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

                if (existingEntity == null)
                    throw new CalendarException($"Entity with ID {id} is not found");

                if (existingEntity is IUserOwnedElement userOwnedEl)
                {
                    var userId = await _permissionService.GetUserId();
                    if (userOwnedEl.UserId != userId)
                        throw new CalendarException($"Current user is not owner of entity with ID {id}");
                }
                entity.Id = existingEntity.Id;
                dbContext.Entry(existingEntity).State = EntityState.Detached;
                dbContext.Set<T>().Update(entity);
                await dbContext.SaveChangesAsync();
            }
        }

        public virtual async Task DeleteAsync(int id)
        {
            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                var existingEntity = await dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

                if (existingEntity == null)
                    throw new CalendarException($"The entity with Id {id} is not found");

                if (existingEntity is IUserOwnedElement userOwned)
                {
                    var userId = await _permissionService.GetUserId();
                    if (userOwned.UserId != userId)
                        throw new CalendarException($"Current user is not owner of entity with ID {id}");
                }

                if (existingEntity is ISafeDeletableDbElement safeDelitable)
                    safeDelitable.DateDeleted = DateTime.Now.ToUniversalTime();
                else
                    dbContext.Remove(existingEntity);

                await dbContext.SaveChangesAsync();
            }
        }

        public virtual async Task AddRangeAsync(List<T> entities)
        {
            if (entities.Count() == 0)
                return;

            if (entities.First() is IUserOwnedElement)
            {
                var userId = await _permissionService.GetUserId();
                var casted = entities.Cast<IUserOwnedElement>();
                foreach (var item in casted)
                    item.UserId = userId;
            }

            if (entities.First() is IHasDateCreated dateCreatedElem)
            {
                dateCreatedElem.DateCreated = DateTime.Now.ToUniversalTime();
                var casted = entities.Cast<IHasDateCreated>();
                foreach (var item in casted)
                    item.DateCreated = DateTime.Now.ToUniversalTime();
            }

            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                await dbContext.Set<T>().AddRangeAsync(entities);
                await dbContext.SaveChangesAsync();
            }
        }

        public virtual async Task AddAsync(T entity)
        {
            if (entity is IUserOwnedElement userOwned)
            {
                var userId = await _permissionService.GetUserId();
                userOwned.UserId = userId;
            }

            if (entity is IHasDateCreated dateCreatedElem)
            {
                dateCreatedElem.DateCreated = DateTime.Now.ToUniversalTime();
            }

            using (var dbContext = _CalendarDbFactory.CreateDbContext())
            {
                await dbContext.Set<T>().AddAsync(entity);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
