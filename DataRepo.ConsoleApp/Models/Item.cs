using System;
using SQLite;

namespace DataRepo.ConsoleApp.Models
{
    public class Item : BaseEntity<int>
    {
        public Item()
        {
        }

        public string Title { get; set; }

    }
}
