using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Entities;

namespace BookStore.Application.Abstraction.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetUserOrders(string userId);
        Task CompleteOrder(string userId);
    }
}