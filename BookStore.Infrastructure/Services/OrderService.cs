using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Application.Abstraction.Services;
using BookStore.Core.Entities;

namespace BookStore.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly ICartService _service;

        public OrderService(IOrderRepository repository, ICartService service)
        {
            _repository = repository;
            _service = service;
        }
        public async Task CompleteOrder(string userId)
        {
            var totalAmount = await _service.GetTotalCount(userId);
            var order = new Order
            {
                UserId = userId,
                TotalAmount = totalAmount
            };
            await _repository.CreateOrder(order);
            await _service.ClearBasket(userId);
        }

        public async Task<IEnumerable<Order>> GetUserOrders(string userId)
        {
            return await _repository.GetUserOrders(userId);
        }
    }
}