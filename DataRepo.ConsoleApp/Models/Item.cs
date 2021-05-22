using System;
using SQLite;

namespace DataRepo.ConsoleApp.Models
{
    public class Item : BaseEntity<int>
    {
        public Item()
        {
        }

        [PrimaryKey]
        [AutoIncrement]
        public override int Id { get; set; }

        public string Title { get; set; }

    }
}
