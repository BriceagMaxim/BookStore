using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Entities;

namespace BookStore.Application.Abstraction.Repositories
{
    public interface IOrderRepository
    {
         Task<IEnumerable<Order>> GetUserOrders(string userId);
         Task CreateOrder(Order order);
    }
}