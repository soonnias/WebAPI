using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Models;
using API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace API.Controllers
{  
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] string search = "", [FromQuery] string sort = "")
        {
            try
            {
                var categories = await _categoryService.GetAllCategories(search, sort);
                return Ok(categories);
            }
            catch (Exception ex)
            {
                // Логування помилки (додатково, якщо використовується логгер)
                Console.WriteLine($"Error in {nameof(GetAllCategories)}: {ex.Message}");

                // Повернення детальної помилки
                return StatusCode(500, new
                {
                    Message = "An error occurred while processing your request.",
                    Error = ex.Message, // Основне повідомлення про помилку
                    Details = ex.StackTrace // Детальний стек помилок
                });
            }
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CreateCategoryDto categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.Name))
                return BadRequest("Name is required.");

            var category = new Category
            {
                Name = categoryDto.Name
            };

            var newCategory = await _categoryService.AddCategory(category);

            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, newCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] UpdateCategoryDto categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.Name))
                return BadRequest("Invalid data.");

            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
                return NotFound();

            category.Name = categoryDto.Name;

            await _categoryService.UpdateCategory(category);

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok();
        }
    }
}
