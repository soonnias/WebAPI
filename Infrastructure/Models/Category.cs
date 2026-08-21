using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    }

}
