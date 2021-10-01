using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Entities;

namespace BookStore.Application.Abstraction.Repositories
{
    public interface ICartItemRepository
    {
        Task<IList<CartItem>> GetByUserIdAsync(string userId);
        Task<CartItem> GetByCartItemIdAsync(int id);
        Task<CartItem> GetItemByIdFromCartAsync(int id);
        Task CreateAsync(CartItem cart);
        Task UpdateASync(CartItem cart);
        Task DeleteAsync(CartItem cart);
        Task SaveAsync();
    }
}