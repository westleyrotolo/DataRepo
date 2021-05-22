using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataRepo
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        Task<int> CountAsync();

        Task<TEntity> GetAsync(TKey pk);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);

        Task<TKey> AddAsync(TEntity entity, bool replaceIfExists = false);
        Task AddRangeAsync(IEnumerable<TEntity> entities);

        Task<bool> RemoveAsync(TEntity entity);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

        Task<bool> UpdateAsync(TEntity entity);
        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        Task TruncateAsync();

        Task<IEnumerable<T>> ExecuteQueryScalar<T>(string query, params object[] args);
        Task<IEnumerable<dynamic>> ExecuteQuery(string query, params object[] args); 


        event EventHandler<IEnumerable<TEntity>> ItemAdded;
        event EventHandler<IEnumerable<TEntity>> ItemUpdated;
        event EventHandler<IEnumerable<TEntity>> ItemRemoved;
    }
}
