using System;
using SQLite;

namespace DataRepo
{
    public class BaseEntity<TKey> : IBaseEntity<TKey>
    {
        [PrimaryKey]
        public virtual TKey Id { get; internal set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public override string ToString()
        {
            try
            {
                return System.Text.Json.JsonSerializer.Serialize(this);
            }
            catch (Exception)
            {
                return base.ToString();
            }
            
        }

    }
}
