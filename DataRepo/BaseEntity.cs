using System;
using System.Text.Json;
using SQLite;

namespace DataRepo
{
    public class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        [PrimaryKey]
        public virtual TKey Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            try
            {
                return JsonSerializer.Serialize(this);
            }
            catch (Exception)
            {
                return base.ToString();
            }
            
        }

    }
}
