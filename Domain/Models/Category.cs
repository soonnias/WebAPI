using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }

}
