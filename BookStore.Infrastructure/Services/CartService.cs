using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Application.Abstraction.Services;
using BookStore.Core.Entities;

namespace BookStore.Infrastructure.Services
{
    public class CartService : ICartService
    {
        private readonly ICartItemRepository _repository;
        private readonly IBookRepository _bookRepository;

        public CartService(
            ICartItemRepository repository, 
            IBookRepository bookRepository)
        {
            _repository = repository;
            _bookRepository = bookRepository;
        }

        public async Task<CartItem> AddBookToCartAsync(CartItem item)
        {
            var book = await _bookRepository.GetBookByIdAsync(item.BookId);
            if(book is null) throw new NullReferenceException("Book is not found");

            var existedItem = await _repository.GetItemByIdFromCartAsync(item.BookId);
            if(existedItem != null)
                return await UpdateQuantity(item.Quantity, existedItem);

            await _repository.CreateAsync(item);
            await _repository.SaveAsync();
            return item;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            return await _repository.GetByUserIdAsync(userId);
        }

        public async Task ClearBasket(string userId)
        {
            var cartItems = await _repository.GetByUserIdAsync(userId);

            foreach (var cartItem in cartItems)
                await _repository.DeleteAsync(cartItem);

            await _repository.SaveAsync();
        }

        public async Task<IEnumerable<CartItem>> ChangeCartItem(int id, int quantity)
        {
            var cartItem = await _repository.GetByCartItemIdAsync(id);

            if (cartItem is null) return null;

            cartItem.Quantity = quantity;

            await _repository.UpdateASync(cartItem);
            await _repository.SaveAsync();
            return await GetCartItemsAsync(cartItem.UserId);
        }

        private async Task<CartItem> UpdateQuantity(int quantity, CartItem cartItem)
        {
            cartItem.Quantity += quantity;

            await _repository.UpdateASync(cartItem);
            await _repository.SaveAsync();
            return cartItem;
        }

        public async Task<IEnumerable<CartItem>> DeleteBookFromCartById(int id)
        {
            var cartItem = await _repository.GetItemByIdFromCartAsync(id);

            if (cartItem != null)
            {
                await _repository.DeleteAsync(cartItem);
                await _repository.SaveAsync();
            }
            return await GetCartItemsAsync(cartItem.UserId);
        }
    }
}