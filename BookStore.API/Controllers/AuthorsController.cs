using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Dtos;
using BookStore.Application.Abstraction.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _repository;

        public AuthorsController(IAuthorRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = _mapper.Map<List<AuthorDto>>(await _repository.GetAuthorsAsync());
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = _mapper.Map<AuthorDto>(await _repository.GetAuthorByIdAsync(id));
            if (author is null) return NotFound();
            return Ok(author);
        }

        [HttpGet("{id}/books")]
        public async Task<IActionResult> GetAuthorBooks(int id)
        {
            var author = await _repository.GetAuthorByIdAsync(id);
            if (author is null) return NotFound();

            var authorBooks = _mapper.Map<List<BookDto>>(await _repository.GetAuthorBooksAsync(id));
            return Ok(authorBooks);
        }
    }
}