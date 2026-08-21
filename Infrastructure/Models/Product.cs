using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Models
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

        // Навігаційна властивість для зв'язку з розмірами
        public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

        // Для вибору SizeType
        public string SizeTypeId { get; set; }
        //public virtual SizeType SizeType { get; set; }
    }   
}
