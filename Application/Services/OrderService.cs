using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;
    
        public OrderService(IOrderRepository orderRepository, ICartService cartService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userId)
        {
            // Використовуємо репозиторій для отримання замовлень користувача
            var allOrders = await _orderRepository.GetAllOrdersAsync();
            return allOrders.Where(o => o.User.Id.ToString() == userId).OrderByDescending(o => o.OrderDate).ToList();
        }

        public async Task<Order> CreateOrderAsync(User user, string address)
        {
            // Отримання користувацького кошика
            var cart = await _cartService.GetCartByUserIdAsync(user.Id.ToString());

            if (cart == null || !cart.CartItems.Any())
            {
                return null;
            }

            // Формування замовлення
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                Address = address,
                Status = "Pending",
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    ProductPrice = ci.Product.Price,
                    Quantity = ci.Quantity,
                    TotalPrice = ci.Product.Price * ci.Quantity, // Обчислення TotalPrice для кожного елемента
                    Size = ci.ProductSize?.Size.Value,
                    ImageUrl = ci.Product.ImageUrl,
                }).ToList()
            };

            // Розрахунок загальної ціни замовлення
            order.Price = order.OrderItems.Sum(oi => oi.TotalPrice);

            // Очищення кошика
            foreach (var cartItem in cart.CartItems)
            {
                await _cartService.RemoveFromCartAsync(cartItem.Id);
            }

            return order;
        }


        public async Task<bool> SaveOrderAsync(Order order)
        {
            // Збереження через репозиторій
            await _orderRepository.SaveOrderAsync(order);
            return true;
        }

        public async Task<Order> FindOrderByIdAsync(string orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<bool> CancelOrderAsync(string orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                await _orderRepository.UpdateOrderStatusAsync(orderId, "Cancelled");
                return true;
            }
            return false;
        }
    }
}
