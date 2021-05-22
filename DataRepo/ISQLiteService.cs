using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using SQLite;

namespace DataRepo
{
    public interface ISQLiteService
    {
        SQLiteAsyncConnection SQLiteAsyncConnection { get; }
        Task InitializeAsync(params Type[] types);
        Task InitializeAsync(IEnumerable<Type> types);
        Task InitializeAsync(Assembly assembly, params Type[] excludeTypes);
        Task InitializeAsync(string nameSpace, params Type[] excludeTypes);
    }
}
