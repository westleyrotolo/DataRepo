using System;
using DataRepo.ConsoleApp.Models;

namespace DataRepo.ConsoleApp.Repositories
{
    public class ItemRepository : GenericRepository<Item, int>, IItemRepository
    {
        public ItemRepository(ISQLiteService sqliteService) : base(sqliteService)
        {

        }
    }
}
