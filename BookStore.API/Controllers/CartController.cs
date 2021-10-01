using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Dtos;
using BookStore.API.Errors;
using BookStore.Application.Abstraction.Services;
using BookStore.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    public class CartController : BaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly ICartService _service;

        public CartController(ICartService service, IMapper mapper)
        {
            _mapper = mapper;
            _service = service;
        }

        // GET api/Cart/userId
        [HttpGet("{userId}", Name = "GetCartByUserId")]
        public async Task<IActionResult> GetCartByUserId(string userId)
        {
            var cart = await _service.GetCartItemsAsync(userId);
            if (cart.Count() == 0)
                return NotFound(new ApiResponse(404, $"Not found cart items for user with id: {userId}"));

            var cartItems = _mapper.Map<List<CartItemDto>>(cart);
            return Ok(cartItems);
        }

        // POST api/Cart
        [HttpPost]
        public async Task<IActionResult> AddItemToCart(CartItemForCreateDto cartItemDto)
        {
            if (!ModelState.IsValid) return BadRequest();
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            var updatedCart = await _service.AddBookToCartAsync(cartItem);
            var updatedCartDto = _mapper.Map<CartItemDto>(updatedCart);
            return CreatedAtRoute("GetCartByUserId", new { userId = updatedCartDto.UserId }, updatedCartDto);
        }

        // PUT api/Cart/ChangeItemQuantity/5/4
        [HttpPut("ChangeItemQuantity/{itemId}/{quantity}")]
        public async Task<IActionResult> ChangeItemQuantity(int itemId, int quantity)
        {
            var cartItemsList = await _service.ChangeCartItem(itemId, quantity);

            if (cartItemsList is null)
                return NotFound(new ApiResponse(400, "Item not found in the cart, please check the cartItemId"));
            return Ok(_mapper.Map<List<CartItemDto>>(cartItemsList));
        }

        // DELETE api/Cart/ClearBasket/1
        [HttpDelete("ClearCart/{userId}")]
        public async Task<IActionResult> ClearBasket(string userId)
        {
            await _service.ClearBasket(userId);
            return NoContent();
        }

        // DELETE api/Cart/DeleteItemFromBasket/1
        [HttpDelete("DeleteItemFromBasket/{cartItemId}")]
        public async Task<IActionResult> RemoveCart(int cartItemId)
        {
            var cartItemsList = await _service.DeleteBookFromCartById(cartItemId);

            if (cartItemsList is null)
                return NotFound(new ApiResponse(400, "Item not found in the cart, please check the cartItemId"));
            return Ok(_mapper.Map<List<CartItemDto>>(cartItemsList));
        }
    }
}