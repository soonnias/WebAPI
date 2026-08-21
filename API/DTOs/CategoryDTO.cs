using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }

    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; }
    }
}
