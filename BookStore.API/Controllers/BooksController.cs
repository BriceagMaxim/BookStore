using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Dtos;
using BookStore.API.Errors;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BookStore.API.Controllers
{
    public class BooksController : BaseAPIController
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<BooksController> _logger;
        private readonly IMapper _mapper;
        private readonly IBookRepository _repository;

        public BooksController(
            IBookRepository repository, 
            IAuthorRepository authorRepository, 
            IMapper mapper,
            ILogger<BooksController> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _mapper = mapper;
            _repository = repository;
        }

        // GET api/Books
        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            var books = _mapper.Map<List<BookDto>>(await _repository.GetBooksAsync());
            return Ok(books);
        }

        // GET api/Books/1
        [HttpGet("{id}", Name = "BookById")]
        public async Task<ActionResult> GetBookById(int id)
        {
            var book = _mapper.Map<BookDto>(await _repository.GetBookByIdAsync(id));

            if (book is null) return NotFound(new ApiResponse(404, $"Book with id: {id} is not found"));

            return Ok(book);
        }

        // POST api/Books
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookForCreateDto newBook)
        {
            if (!ModelState.IsValid) return BadRequest();

            var author = await _authorRepository.GetAuthorByIdAsync(newBook.AuthorId);
            if(author is null) 
                return NotFound(new ApiResponse(404, $"Author with id: {newBook.AuthorId} is not found"));
                
            var book = _mapper.Map<Book>(newBook);
            await _repository.CreateBook(book);
            _logger.LogInformation($"Add new book {newBook.Title}");
            var createdBook = _mapper.Map<BookDto>(book);

            return CreatedAtRoute("BookById", new { Id = book.Id }, createdBook);
        }

        // PUT api/Books/1
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookForCreateDto bookDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var book = await _repository.GetBookByIdAsync(id);

            if (book is null) return NotFound(new ApiResponse(404, $"Book with id: {id} is not found"));

            _mapper.Map(bookDto, book);
            await _repository.UpdateBook(book);
            _logger.LogInformation($"Update book with Id: {book.Id}");

            return NoContent();
        }

        // DELETE api/Books/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _repository.GetBookByIdAsync(id);

            if (book is null) return NotFound(new ApiResponse(404, $"Book with id: {id} is not found"));

            await _repository.DeleteBook(book);
            _logger.LogInformation($"Delete book with Id: {book.Id}");
            return NoContent();
        }
    }
}