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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookStoreContext _context;

        public AuthorRepository(BookStoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Book>> GetAuthorBooksAsync(int authorId)
        {
            return await _context.Books.Where(el => el.AuthorId == authorId).ToListAsync();
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            return await _context.Authors.FindAsync(authorId);
        }

        public async Task<IReadOnlyList<Author>> GetAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }
    }
}