using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class CreateProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }  // Заміна IFormFile на string для URL картинки

        public string SizeTypeId { get; set; }

        public List<string> CategoryIds { get; set; }
    }

    public class UpdateProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public List<string>? CategoryIds { get; set; }
    }


    public class ProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public List<ProductCategoryDto> ProductCategories { get; set; }
        public List<ProductSizeDto> ProductSizes { get; set; }
        public List<RewiewDto> Reviews { get; set; }
    }

    public class ProductCategoryDto
    {
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
    }

    public class ProductSizeDto
    {
        public string Id { get; set; }
        public string SizeId { get; set; }
        public string SizeName { get; set; }
        public bool IsAvailable { get; set; }
    }

}
