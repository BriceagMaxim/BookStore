using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Dtos;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookRepository _repository;

        public BooksController(IBookRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllBooks()
        {
            var books = _mapper.Map<List<BookDto>>(await _repository.GetBooksAsync());
            return Ok(books);
        }

        [HttpGet("{id}", Name = "BookById")]
        public async Task<ActionResult> GetBookById(int Id)
        {
            var book = _mapper.Map<BookDto>(await _repository.GetBookByIdAsync(Id));

            if (book is null) return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook([FromBody] BookForCreateDto newBook)
        {
            if (!ModelState.IsValid) return BadRequest();

            var book = _mapper.Map<Book>(newBook);
            await _repository.CreateBook(book);

            var createdBook = _mapper.Map<BookDto>(book);

            return CreatedAtRoute("BookById", new { Id = book.Id }, createdBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookForCreateDto bookDto)
        {
            if (!ModelState.IsValid) return BadRequest();

            var book = await _repository.GetBookByIdAsync(id);

            if (book is null) return NotFound();

            _mapper.Map(bookDto, book);
            await _repository.UpdateBook(book);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int Id)
        {
            var book = await _repository.GetBookByIdAsync(Id);

            if (book is null) return NotFound();

            await _repository.DeleteBook(book);

            return NoContent();
        }
    }
}