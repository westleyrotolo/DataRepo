using System;
using System.Linq;
using System.Threading.Tasks;
using DataRepo.ConsoleApp.Repositories;

namespace DataRepo.ConsoleApp
{
    public class App
    {
        private readonly IItemRepository _itemRepository;
        private readonly ISQLiteService _sqliteService;
        public App(IItemRepository itemRepository, ISQLiteService sqliteService)
        {
            _itemRepository = itemRepository;
            _sqliteService = sqliteService;
        }
        public async Task Run()
        {
            Console.WriteLine("Hello DataRepo!");
            await _sqliteService.InitializeAsync("DataRepo.ConsoleApp");
            var title = string.Empty;

            while (string.IsNullOrEmpty(title))
            {
                Console.Write("Insert title: ");
                title = Console.ReadLine();
            }

            var item = new Models.Item
            {
                Title = title
            };

            await _itemRepository.AddAsync(item);
            var items = await _itemRepository.GetAllAsync();
            items?.ToList().ForEach(x => Console.WriteLine(x));
        }
    }
}
