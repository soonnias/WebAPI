using System.Text.Json.Serialization;

namespace Infrastructure.Models
{
    public class ProductCategory
    {
        public string ProductId { get; set; }
        [JsonIgnore]
        public Product Product { get; set; }

        public string CategoryId { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
    }
}
