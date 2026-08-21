using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Domain.Models;
using API.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using API.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ISizeService _sizeTypeService;

        public ProductController(IProductService productService, ICategoryService categoryService, ISizeService sizeTypeService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _sizeTypeService = sizeTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string search = "", string categoryIds = "", string sort = "")
        {
            try
            {
                var categoryIdsList = string.IsNullOrEmpty(categoryIds)
                    ? new List<string>()
                    : categoryIds.Split(',').ToList();

                var products = await _productService.GetProductsAsync(search, categoryIdsList, sort);

                if (products == null || !products.Any())
                {
                    return Ok(new List<ProductDto>());
                }

                var productsDto = products.Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    ProductCategories = p.ProductCategories.Select(pc => new ProductCategoryDto
                    {
                        CategoryId = pc.CategoryId,
                        CategoryName = pc.Category.Name
                    }).ToList(),
                    ProductSizes = p.ProductSizes.Select(ps => new ProductSizeDto
                    {
                        Id = ps.Id,
                        SizeId = ps.SizeId,
                        SizeName = ps.Size.Value,
                        IsAvailable = ps.IsAvailable
                    }).ToList(),
                    Reviews = p.Reviews.Select(r => new RewiewDto
                    {
                        Username = r.User.FirstName, // Ім'я користувача, яке записане у відгуку
                        Rating = r.Rating,
                        Comment = r.Comment
                    }).ToList()
                }).ToList();

                return Ok(productsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching products.", error = ex.Message });
            }
        }




        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            try
            {
                // Отримуємо продукт з сервісу
                var product = await _productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }

                // Перетворюємо продукт на DTO
                var productDto = new ProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    ImageUrl = product.ImageUrl,
                    ProductCategories = product.ProductCategories.Select(pc => new ProductCategoryDto
                    {
                        CategoryId = pc.CategoryId,
                        CategoryName = pc.Category.Name // Назва категорії
                    }).ToList(),
                    ProductSizes = product.ProductSizes.Select(ps => new ProductSizeDto
                    {
                        Id = ps.Id,
                        SizeId = ps.SizeId,
                        SizeName = ps.Size.Value, // Назва розміру
                        IsAvailable = ps.IsAvailable
                    }).ToList(),
                    
                    Reviews = product.Reviews.Select(r => new RewiewDto
                    {
                        Username = r.User.FirstName, // Ім'я користувача, яке записане у відгуку
                        Rating = r.Rating,
                        Comment = r.Comment
                    }).ToList()
                };

                return Ok(productDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching the product.", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new { message = "Invalid product data.", errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                }

                var product = new Product
                {
                    Name = productDto.Name,
                    Description = productDto.Description,
                    Price = productDto.Price,
                    SizeTypeId = productDto.SizeTypeId,
                    ImageUrl = productDto.ImageUrl  // Приймаємо URL картинки
                };

                // Додаємо категорії
                product.ProductCategories = productDto.CategoryIds.Select(id => new ProductCategory { CategoryId = id }).ToList();

                // Отримуємо всі розміри для вибраного SizeType
                var sizes = await _sizeTypeService.GetSizesBySizeTypeIdAsync(productDto.SizeTypeId);
                if (sizes == null || !sizes.Any())
                {
                    return BadRequest(new { message = "No sizes found for the selected SizeType." });
                }

                // Додаємо ProductSizes для кожного вибраного розміру
                foreach (var size in sizes)
                {
                    var productSize = new ProductSize
                    {
                        SizeId = size.Id,
                        IsAvailable = true
                    };
                    product.ProductSizes.Add(productSize);
                }

                // Додаємо продукт через сервіс
                await _productService.AddProductAsync(product);

                return Ok(new { message = "Product created successfully", productId = product.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the product.", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, [FromBody] UpdateProductDto productDto)
        {
            try
            {
                // Перевірка на збіг ID продукту
                var existingProduct = await _productService.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }

                // Оновлення полів продукту, якщо вони передані
                if (!string.IsNullOrEmpty(productDto.Name))
                    existingProduct.Name = productDto.Name;

                if (!string.IsNullOrEmpty(productDto.Description))
                    existingProduct.Description = productDto.Description;

                if (productDto.Price.HasValue)
                    existingProduct.Price = productDto.Price.Value;

                if (!string.IsNullOrEmpty(productDto.ImageUrl))
                    existingProduct.ImageUrl = productDto.ImageUrl;

                // Оновлення категорій продукту
                if (productDto.CategoryIds != null && productDto.CategoryIds.Any())
                {
                    // Якщо передано нові категорії, очищаємо старі та додаємо нові
                    existingProduct.ProductCategories.Clear();
                    existingProduct.ProductCategories = productDto.CategoryIds.Select(id => new ProductCategory { CategoryId = id }).ToList();
                }
                else
                {
                    // Якщо нові категорії не передано, потрібно зберегти старі категорії
                    var currentCategoryIds = existingProduct.ProductCategories.Select(c => c.CategoryId).ToList();
                    existingProduct.ProductCategories.Clear();
                    existingProduct.ProductCategories = currentCategoryIds.Select(id => new ProductCategory { CategoryId = id }).ToList();
                }

                // Оновлення продукту через сервіс
                await _productService.UpdateProductAsync(existingProduct);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the product.", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            try
            {
                var existingProduct = await _productService.GetProductByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found." });
                }

                await _productService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the product.", error = ex.Message });
            }
        }
    }
}
