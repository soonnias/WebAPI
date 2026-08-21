using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using API.DTOs;
using Infrastructure.Models;

namespace API.Controllers
{
    [Route("api/manager/orders")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrderManagementController : ControllerBase
    {
        private readonly IOrderManagementService _orderManagementService;

        public OrderManagementController(IOrderManagementService orderManagementService)
        {
            _orderManagementService = orderManagementService;
        }

        // GET: api/manager/orders
        [HttpGet("")]
        public async Task<IActionResult> GetOrders(string? status)
        {
            IEnumerable<Domain.Models.Order> orders;

            if (!string.IsNullOrEmpty(status))
            {
                orders = await _orderManagementService.GetOrdersByStatusAsync(status);
            }
            else
            {
                orders = await _orderManagementService.GetAllOrdersAsync();
            }

            // Перевірка, чи є замовлення
            if (orders == null || !orders.Any())
            {
                return Ok(new List<OrderDto>());  // Повертаємо порожній масив
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


        // GET: api/manager/orders/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails(string id)
        {
            // Отримуємо замовлення за ID
            var order = await _orderManagementService.GetOrderByIdAsync(id);

            // Якщо замовлення не знайдено, повертаємо 404
            if (order == null)
            {
                return NotFound("Замовлення не знайдено.");
            }

            // Перетворення на DTO
            var orderDto = new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                FirstName = order.User?.FirstName,
                LastName = order.User?.LastName,
                OrderDate = order.OrderDate,
                Price = order.Price,
                Address = order.Address,
                Status = order.Status,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductName = oi.ProductName,
                    ProductPrice = oi.ProductPrice,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                    Size = oi.Size,
                    ImageUrl = oi.ImageUrl
                }).ToList() // Не забудьте викликати ToList() для перетворення в список
            };

            // Повертаємо 200 OK з даними
            return Ok(orderDto);
        }


        // PUT: api/manager/orders/{id}/{status}
        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> UpdateOrderStatus(string id, string status)
        {
            if (string.IsNullOrEmpty(status))
            {
                return BadRequest("Статус не може бути порожнім.");
            }

            await _orderManagementService.UpdateOrderStatusAsync(id, status);
            return Ok(); // Повертаємо 204, якщо статус оновлено успішно
        }

    }
}
