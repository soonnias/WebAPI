using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public string SizeTypeId { get; set; }

        // Бізнес-логіка: перевірка коректності ціни
        public void ValidatePrice()
        {
            if (Price <= 0)
                throw new InvalidOperationException("Price must be greater than zero.");
        }

        // Бізнес-логіка: додавання категорії
        public void AddCategory(Category category)
        {
            if (ProductCategories.Any(pc => pc.CategoryId == category.Id))
                throw new InvalidOperationException("This category is already added.");
            ProductCategories.Add(new ProductCategory { ProductId = Id, CategoryId = category.Id });
        }
    }

}
