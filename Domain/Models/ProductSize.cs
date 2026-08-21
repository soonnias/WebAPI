using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ProductSize
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ProductId { get; set; }
        public string SizeId { get; set; }
        public bool IsAvailable { get; set; } = true;

        // Навігаційні властивості
        [JsonIgnore]
        public Product Product { get; set; }
        [JsonIgnore]
        public Size Size { get; set; }
    }

}
