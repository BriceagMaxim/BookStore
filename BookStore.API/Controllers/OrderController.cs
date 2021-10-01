using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Dtos;
using BookStore.API.Errors;
using BookStore.Application.Abstraction.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStore.API.Controllers
{
    public class OrderController : BaseAPIController
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderService _service;

        public OrderController(IOrderService service, IMapper mapper, ILogger<OrderController> logger)
        {
            _logger = logger;
            _mapper = mapper;
            _service = service;
        }

        // GET api/order/userId
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserOrders(string userId)
        {
            var userOrders = await _service.GetUserOrders(userId);
            if(userOrders.Count() == 0) 
                return NotFound(new ApiResponse(404, $"Not found orders for user with Id: {userId}"));

            return Ok(_mapper.Map<List<OrderDto>>(userOrders));
        }

        // POST api/order/userId
        [HttpPost("{userId}")]
        public async Task<IActionResult> CompleteOrder(string userId)
        {
            await _service.CompleteOrder(userId);
            _logger.LogInformation($"Order completed for user with Id: {userId}");
            return Ok(new ApiResponse(200, "Order was successfully completed"));
        }
    }
}