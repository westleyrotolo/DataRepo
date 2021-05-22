using System.Linq;
using System.Threading.Tasks;
using DataRepo.ConsoleApp.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;


namespace DataRepo.Test
{
    [TestFixture]
    public class Tests
    {


        private readonly IItemRepository _itemRepository;
        private readonly ISQLiteService _sqliteService;
        public Tests()
        {
            _sqliteService = new SQLiteService("testDatabase.db3");
            _itemRepository = new ItemRepository(_sqliteService);
        }

        [OneTimeSetUp]
        public void SetupAsync()
        {
            _sqliteService.InitializeAsync("DataRepo.ConsoleApp").Wait();
        }



        [Test, Order(1)]
        public async Task TestAddItemAsync()
        {
            var id = await _itemRepository.AddAsync(new ConsoleApp.Models.Item
            {
                Id = 1,
                Title = "TestTitle"
            });
            Assert.AreEqual(id, 1);
        }

        [Test, Order(2)]
        public async Task TestGetItem()
        {
            var item = await _itemRepository.GetAsync(1);
            Assert.AreEqual(item.Id, 1);
        }

        [Test, Order(3)]
        public async Task TestFindItem()
        {
            var items = await _itemRepository.FindAsync(x => x.Id > 0);
            Assert.GreaterOrEqual(items?.Count(), 1);
        }

        [Test, Order(4)]
        public async Task TestTruncateTable()
        {
            await _itemRepository.TruncateAsync();
            var count = await _itemRepository.CountAsync();
            Assert.AreEqual(count, 0);
        }
    }

}