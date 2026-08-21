using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // Отримати кошик за ID користувача
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartByUserId(string userId)
        {
            try
            {
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    return NotFound(new { message = "Cart not found." });
                }

                // Маппінг моделі на DTO
                var cartDto = new CartDto
                {
                    CartId = cart.Id,
                    UserId = cart.UserId,
                    TotalPrice = cart.CartItems.Sum(ci => ci.Price * ci.Quantity),
                    Items = cart.CartItems.Select(ci => new CartItemDto
                    {
                        CartItemId = ci.Id,
                        ProductId = ci.ProductId,
                        ProductName = ci.Product.Name,
                        SelectedSize = ci.ProductSize.Size?.Value, // Якщо потрібен конкретний розмір
                        ProductSizeId = ci.ProductSize.Id, // Додаємо ID розміру
                        ProductPrice = ci.Product.Price, // Додаємо ціну продукту
                        Quantity = ci.Quantity,
                        TotalItemPrice = ci.Product.Price * ci.Quantity // Загальна вартість товару
                    }).ToList()
                };

                return Ok(cartDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }



        // Додати товар до кошика
        [HttpPost("addCartItem")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            try
            {
                var cartItem = await _cartService.AddToCartAsync(dto.ProductId, dto.SelectedSizeId, dto.Quantity, dto.UserId);
                return CreatedAtAction(nameof(GetCartByUserId), new { userId = dto.UserId }, cartItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Оновити товар у кошику
        [HttpPut("updateCartItem/{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(string cartItemId, [FromBody] UpdateCartItemDto dto)
        {
            try
            {
                await _cartService.UpdateCartItemAsync(cartItemId, dto.Quantity, dto.Price);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Видалити товар із кошика
        [HttpDelete("removeCartItem/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(string cartItemId)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(cartItemId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // Очистити кошик користувача
        /*[HttpDelete("clearCart/{userId}")]
        public async Task<IActionResult> ClearCart(string userId)
        {
            try
            {
                await _cartService.ClearCartAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }*/
    }
}
