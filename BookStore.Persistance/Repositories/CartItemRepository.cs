using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Core.Entities;
using BookStore.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistance.Repositories
{
    public class CartItemRepository : ICartItemRepository
    {
        private readonly BookStoreContext _context;

        public CartItemRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IList<CartItem>> GetByUserIdAsync(string userId)
        {
            return await _context.CartItems
                .Where(el => el.UserId == userId)
                .Include(el => el.Book)
                .Include(el => el.Book.Author).ToListAsync();
        }

        public async Task<CartItem> GetByCartItemIdAsync(int id)
        {
            return await _context.CartItems.Include(el => el.Book).FirstOrDefaultAsync(el => el.Id == id);
        }

        public async Task<CartItem> GetItemByIdFromCartAsync(int id)
        {
            return await _context.CartItems.Include(el => el.Book).FirstOrDefaultAsync(el => el.BookId == id);
        }

        public async Task CreateAsync(CartItem cart)
        {
            await Task.Run(() => { _context.CartItems.Add(cart); });
        }

        public async Task UpdateASync(CartItem cart)
        {
            await Task.Run(() => { _context.CartItems.Update(cart); });
        }

        public async Task DeleteAsync(CartItem cart)
        {
            await Task.Run(() => { _context.CartItems.Remove(cart); });
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}