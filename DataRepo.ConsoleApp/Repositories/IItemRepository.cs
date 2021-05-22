using System;
using DataRepo.ConsoleApp.Models;

namespace DataRepo.ConsoleApp.Repositories
{
    public interface IItemRepository : IGenericRepository<Item, int>
    {
    }
}
