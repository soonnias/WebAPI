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
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductSizeService _productSizeService;

        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IProductSizeService productSizeService)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productSizeService = productSizeService;
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            return await _cartRepository.GetCartByUserIdAsync(userId);
        }

        public async Task<CartItem> AddToCartAsync(string productId, string selectedSizeId, int quantity, string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId) ?? new Cart { UserId = userId };
            var cartItem = await _cartItemRepository.GetCartItemAsync(cart.Id, productId, selectedSizeId, userId);

            var productSize = await _productSizeService.GetProductSizeByIdJustAsync(selectedSizeId);
            if (productSize == null)
            {
                throw new ArgumentException("The selected product size does not exist.");
            }

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;

                // Оновлення існуючого елемента
                await _cartItemRepository.UpdateCartItemAsync(cartItem);
            }
            else
            {
                cartItem = new CartItem
                {
                    ProductId = productId,
                    ProductSizeId = selectedSizeId,
                    Quantity = quantity,
                    CartId = cart.Id,
                    UserId = userId
                };

                // Додавання нового елемента
                await _cartItemRepository.AddCartItemAsync(cartItem);
            }

            return cartItem;
        }


        public async Task UpdateCartItemAsync(string cartItemId, int quantity, decimal price)
        {
            var cartItem = await _cartItemRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                cartItem.Price = price * quantity;
                await _cartItemRepository.UpdateCartItemAsync(cartItem);
            }
        }

        public async Task RemoveFromCartAsync(string cartItemId)
        {
            var cartItem = await _cartItemRepository.GetCartItemByIdAsync(cartItemId);
            if (cartItem != null)
            {
                await _cartItemRepository.RemoveCartItemAsync(cartItem);
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart != null)
            {
                await _cartItemRepository.RemoveRangeCartItemsAsync(cart.CartItems);
            }
        }
    }
}
