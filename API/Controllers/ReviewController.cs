using API.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // Отримання всіх відгуків для продукту за ID
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetReviewsByProductId(string productId)
        {
            try
            {
                // Викликаємо сервіс для отримання відгуків за productId
                var reviews = await _reviewService.GetReviewsByProductIdAsync(productId);

                if (reviews == null || reviews.Count == 0)
                {
                    return NotFound(new { message = "No reviews found for this product." });
                }

                // Маппінг на DTO
                var reviewDtos = reviews.Select(r => new ReviewAllInfoDto
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    ProductId = r.ProductId,
                    Username = r.User?.FirstName, // Перевіряємо, що User не null
                    Rating = r.Rating,
                    Comment = r.Comment, 
                    CreatedAt = r.CreatedAt
                }).ToList();

                return Ok(reviewDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching reviews.", error = ex.Message });
            }
        }


        // Створення нового відгуку
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createReviewDto)
        {
            try
            {
                if (createReviewDto.Rating < 1 || createReviewDto.Rating > 5)
                {
                    return BadRequest(new { message = "Rating must be between 1 and 5." });
                }

                if (string.IsNullOrEmpty(createReviewDto.Comment) || createReviewDto.Comment.Length > 200)
                {
                    return BadRequest(new { message = "Comment cannot exceed 200 characters." });
                }

                var review = new Review
                {
                    UserId = createReviewDto.UserId,
                    ProductId = createReviewDto.ProductId,
                    Rating = createReviewDto.Rating,
                    Comment = createReviewDto.Comment,
                    CreatedAt = DateTime.UtcNow
                };

                var createdReview = await _reviewService.CreateReviewAsync(review);
                return CreatedAtAction(nameof(GetReviewsByProductId), new { productId = review.ProductId }, createdReview);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the review.", error = ex.Message });
            }
        }

        // Видалення відгуку
        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(string reviewId)
        {
            try
            {
                await _reviewService.DeleteReviewAsync(reviewId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the review.", error = ex.Message });
            }
        }
    }
}
