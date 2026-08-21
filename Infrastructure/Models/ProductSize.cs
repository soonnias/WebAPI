
using System.Text.Json.Serialization;

namespace Infrastructure.Models
{
    public class ProductSize
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string ProductId { get; set; }
        [JsonIgnore]
        public virtual Product Product { get; set; }

            public string SizeId { get; set; }
        [JsonIgnore]
        public virtual Size Size { get; set; }

            public bool IsAvailable { get; set; } = true; // Доступність конкретного розміру для продукту
        }
    }

