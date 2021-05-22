using System;
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
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(this);
                return json;
            }
            catch (Exception ex)
            {
                return base.ToString();
            }
            
        }
    }
}
