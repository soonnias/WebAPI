using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using API.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;

        public OrderController(IOrderService orderService, ICartService cartService)
        {
            _orderService = orderService;
            _cartService = cartService;
        }

        // Отримання замовлень користувача
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            // Отримуємо замовлення користувача
            var orders = await _orderService.GetUserOrdersAsync(userId);

            if (orders == null || !orders.Any())
            {
                return NotFound("Замовлення не знайдено.");
            }

            // Перетворюємо замовлення на DTO
            var ordersDto = orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                FirstName = o.User?.FirstName,
                LastName = o.User?.LastName,
                OrderDate = o.OrderDate,
                Price = o.Price,
                Address = o.Address,
                Status = o.Status,
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductName = oi.ProductName,
                    ProductPrice = oi.ProductPrice,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                    Size = oi.Size,
                    ImageUrl = oi.ImageUrl
                }).ToList()
            }).ToList();

            return Ok(ordersDto);
        }


        // Створення замовлення
        [HttpPost("{userId}/{address}")]
        public async Task<IActionResult> CreateOrder(string userId, string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return BadRequest("Адреса не може бути порожньою.");
            }

            // Створення замовлення
            var order = await _orderService.CreateOrderAsync(new User { Id = userId }, address);
            if (order == null)
            {
                return BadRequest("Кошик порожній або користувач не знайдений.");
            }

            // Збереження замовлення
            var saved = await _orderService.SaveOrderAsync(order);
            if (!saved)
            {
                return StatusCode(500, "Не вдалося зберегти замовлення.");
            }

            return Ok(order);
        }



        // Скасування замовлення
        /*[HttpDelete("{orderId}")]
        public async Task<IActionResult> CancelOrder(string orderId)
        {
            var result = await _orderService.CancelOrderAsync(orderId);
            if (!result)
            {
                return NotFound("Замовлення не знайдено.");
            }

            return Ok("Замовлення скасовано.");
        }*/
    }
}
