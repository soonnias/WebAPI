using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Infrastructure.Models
{
    public class SizeType
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [StringLength(50)]
        public string Name { get; set; } // Наприклад: "Одяг", "Взуття", "Універсальний"

        // Навігаційні властивості
        public virtual ICollection<Size> Sizes { get; set; } = new List<Size>();
    }
}
