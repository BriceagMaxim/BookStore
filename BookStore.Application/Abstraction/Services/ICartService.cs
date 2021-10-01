using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Entities;

namespace BookStore.Application.Abstraction.Services
{
    public interface ICartService
    {
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);
        Task<CartItem> AddBookToCartAsync(CartItem item);
        Task<IEnumerable<CartItem>> ChangeCartItem(int id, int quantity);
        Task<IEnumerable<CartItem>> DeleteBookFromCartById(int id);
        Task ClearBasket(string userId);
    }
}