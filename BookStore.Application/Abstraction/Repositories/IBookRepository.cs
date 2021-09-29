using System.Collections.Generic;
using System.Threading.Tasks;
using BookStore.Core.Entities;

namespace BookStore.Application.Abstraction.Repositories
{
    public interface IBookRepository
    {
        Task<IReadOnlyList<Book>> GetBooksAsync();
        Task<Book> GetBookByIdAsync { get; set; }
        Task CreateBook(Book book);
        Task UpdateBook(Book book);
        Task DeleteBook(int bookId);
    }
}