using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetCartItemAsync(string cartId, string productId, string selectedSizeId, string userId);
        Task<CartItem> GetCartItemByIdAsync(string cartItemId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task RemoveCartItemAsync(CartItem cartItem);
        Task RemoveRangeCartItemsAsync(IEnumerable<CartItem> cartItems);
    }
}
