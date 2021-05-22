using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DataRepo.ConsoleApp.Repositories;

namespace DataRepo.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();
            await host.Services.GetService<App>().Run();
        }
       

        static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
                 .ConfigureServices((_, services) =>
                     services.AddTransient<ISQLiteService>(s => new SQLiteService())
                             .AddTransient<IItemRepository, ItemRepository>()
                             .AddTransient<App>());
    }
}
