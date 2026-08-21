using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models
{
    public class Size
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(20)]
        public string Value { get; set; } // Наприклад: "S", "M", "L" або "38", "40", "42"

        public bool IsAvailable { get; set; } = true; // Чи доступний цей розмір

        public string SizeTypeId { get; set; }
        public virtual SizeType SizeType { get; set; }

        // Навігаційна властивість для зв'язку з продуктом
        public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}
