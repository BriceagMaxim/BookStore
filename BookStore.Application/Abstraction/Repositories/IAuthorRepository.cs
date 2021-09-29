using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Entities;

namespace BookStore.Application.Abstraction.Repositories
{
    public interface IAuthorRepository
    {
        Task<IReadOnlyList<Author>> GetAuthorsAsync();
        Task<Author> GetAuthorByIdAsync();
        Task<IReadOnlyList<Book>> GetAuthorBooksAsync();
    }
}