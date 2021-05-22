using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SQLite;

namespace DataRepo
{
    public class SQLiteService : ISQLiteService
    {

        readonly Lazy<SQLiteAsyncConnection> lazyInitializer;

        private const string _databaseFilename = "app.db3";
        private const SQLite.SQLiteOpenFlags _flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public string DatabaseFilename { get; private set; }
        public SQLiteOpenFlags SQLiteOpenFlags { get; private set; }

        public SQLiteService(string databaseFilename = _databaseFilename, SQLiteOpenFlags sqliteOpenFlags = _flags)
        {
            DatabaseFilename = databaseFilename;
            SQLiteOpenFlags = sqliteOpenFlags;
            lazyInitializer = new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(DatabaseFilename, SQLiteOpenFlags));
        }



        public SQLiteAsyncConnection SQLiteAsyncConnection => lazyInitializer.Value;

        public async Task InitializeAsync(params Type[] types)
        {
            await SQLiteAsyncConnection.CreateTablesAsync(CreateFlags.None, types).ConfigureAwait(false);
        }

        public async Task InitializeAsync(IEnumerable<Type> types)
        {
            await InitializeAsync(types.ToArray());
               
        }

        public async Task InitializeAsync(Assembly assembly, params Type[] excludeTypes)
        {
            var types = assembly.GetTypes().Where(x => x.IsAssignableFrom(typeof(IBaseEntity<>)));
            await InitializeAsync(types.Where(x => !excludeTypes.Any(t => t == x)).ToArray());
        }

        public async Task InitializeAsync(string nameSpace, params Type[] excludeTypes)
        {
            var asm = Assembly.Load(nameSpace);
            var types = asm.GetTypes().Where(x => x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseEntity<>)));
            await InitializeAsync(types.Where(x => !excludeTypes.Any(t => t == x)).ToArray());
        }
    }
}
