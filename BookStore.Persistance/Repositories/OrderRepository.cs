using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Core.Entities;
using BookStore.Core.Entities.Identity;
using BookStore.Persistance.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistance.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly BookStoreContext _bookContext;
        private readonly UserManager<User> _userManager;

        public OrderRepository(BookStoreContext bookContext, UserManager<User> userManager)
        {
            _bookContext = bookContext;
            _userManager = userManager;
        }
        public async Task CreateOrder(Order order)
        {
            _bookContext.Orders.Add(order);
            await _bookContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userId)
        {
            return await _bookContext.Orders.Where(el => el.UserId == userId).ToListAsync();
        }
    }
}