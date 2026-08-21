using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProductCategory
    {
        public string ProductId { get; set; }
        
        public string CategoryId { get; set; }

        // Навігаційні властивості
        [JsonIgnore]
        public Product Product { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
    }

}
