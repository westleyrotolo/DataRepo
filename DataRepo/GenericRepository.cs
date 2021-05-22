using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using SQLite;

namespace DataRepo
{ 
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class, new()
    {
        private readonly SQLiteAsyncConnection _conn;

        public event EventHandler<IEnumerable<TEntity>> ItemAdded;
        public event EventHandler<IEnumerable<TEntity>> ItemUpdated;
        public event EventHandler<IEnumerable<TEntity>> ItemRemoved;

        public GenericRepository(ISQLiteService sqliteService)
        {
            _conn = sqliteService.SQLiteAsyncConnection;
        }

        public async Task<TKey> AddAsync(TEntity entity, bool replaceifExists = false)
        {
            if (typeof(TEntity).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseEntity<>)))
            {
                (entity as IBaseEntity<TKey>).UpdatedAt = DateTime.Now;
                (entity as IBaseEntity<TKey>).CreatedAt = DateTime.Now;
            }
            if (replaceifExists)
            {
                await _conn.InsertOrReplaceAsync(entity);
            }
            else
            {
                await _conn.InsertAsync(entity);
            }

            var map = await _conn.GetMappingAsync<TEntity>();

            ItemAdded?.Invoke(this, new List<TEntity> { entity });

            return (TKey)map.PK.GetValue(entity);
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            ItemAdded?.Invoke(this, entities);
            return _conn.InsertAllAsync(entities);
        }

        public Task<int> CountAsync()
        {
            return _conn.Table<TEntity>().CountAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _conn.Table<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _conn.Table<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public Task<TEntity> GetAsync(TKey pk)
        {
            return _conn.FindAsync<TEntity>(pk);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _conn.Table<TEntity>().ToListAsync();
        }

        public async Task<bool> RemoveAsync(TEntity entity)
        {
            ItemRemoved?.Invoke(this, new List<TEntity> { entity });
            return await _conn.DeleteAsync(entity) > 0;
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            ItemRemoved?.Invoke(this, entities);
            foreach (var entity in entities)
                await _conn.DeleteAsync(entity);
        }

        public async Task TruncateAsync()
        {
            await _conn.TruncateAsync<TEntity>();
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            ItemUpdated?.Invoke(this, new List<TEntity> { entity });

            if (typeof(TEntity).GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseEntity<>)))
            {
                (entity as IBaseEntity<TKey>).UpdatedAt = DateTime.Now;
            }
            return await _conn.UpdateAsync(entity) > 0;
        }

        public Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            ItemUpdated?.Invoke(this, entities);
            return _conn.UpdateAllAsync(entities);
        }

        public async Task<IEnumerable<dynamic>> ExecuteQuery(string query, params object[] args)
        {
            var data = await _conn.QueryAsync<TEntity>(query, args);
            return data;
        }

        public async Task<IEnumerable<T>> ExecuteQueryScalar<T>(string query, params object[] args)
        {
            var data = await _conn.QueryScalarsAsync<T>(query, args);
            return data;
        }
    }
    public static class SQLiteAsyncConnectionExtensions
    {
        public static async Task DeleteManyAsync(this SQLiteAsyncConnection conn, IEnumerable entities)
        {
            foreach (var entity in entities)
                await conn.DeleteAsync(entity);
        }


        public static async Task TruncateAsync<TEntity>(this SQLiteAsyncConnection conn) where TEntity : new()
        {
            await conn.DeleteAllAsync<TEntity>();
        }
    }
}
