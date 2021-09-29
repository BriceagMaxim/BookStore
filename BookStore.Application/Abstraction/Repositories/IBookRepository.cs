using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Entities;

namespace BookStore.Application.Abstraction.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync(int bookId);
        Task CreateBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(Book book);
    }
}