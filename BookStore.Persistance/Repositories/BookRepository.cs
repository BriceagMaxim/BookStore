using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Core.Entities;
using BookStore.Persistance.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Persistance.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;

        public BookRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _context.Books.FindAsync(bookId);
        }

        public async Task<IReadOnlyList<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task CreateBook(Book book)
        {
            await _context.Books.AddAsync(book);
        }

        public async Task UpdateBook(Book book)
        {
            var existedBook = await _context.Books.FindAsync(book.Id);
            existedBook.Title = book.Title;
            existedBook.Price = book.Price;
            existedBook.Description = book.Description;
            existedBook.AuthorId = book.AuthorId;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBook(Book book)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }
    }
}