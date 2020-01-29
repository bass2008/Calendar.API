using Calendar.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Calendar.DAL.Interfaces
{
    public interface IUserOwnedGenericRepository<T> : IGenericRepository<T> where T : class, IUserOwnedElement
    {
        Task<T> GetSingleUserOwnedAsync();

        Task AddOrUpdateSingleUserOwnedAsync(T entity);
    }

    public interface IGenericRepository<T> where T : class, IDbElement
    {
        Task<int> CountAsync(Expression<Func<T, bool>> filter);

        Task<int> CountAsync();

        Task<List<T>> GetAllAsync(int page, int pageSize, Expression<Func<T, bool>> filter);

        Task<List<T>> GetAllAsync(int page, int pageSize);

        Task<List<T>> GetAllAsync();

        Task<T> GetAsync(int id);

        Task UpdateAsync(int id, T entity);

        Task DeleteAsync(int id);

        Task AddRangeAsync(List<T> entities);

        Task AddAsync(T entity);
    }
}
