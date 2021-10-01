using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Dtos;
using BookStore.API.Errors;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    public class AuthorsController : BaseAPIController
    {
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _repository;

        public AuthorsController(IAuthorRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        // GET api/Authors
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            var authors = _mapper.Map<List<AuthorDto>>(await _repository.GetAuthorsAsync());
            return Ok(authors);
        }

        // GET api/Authors/1
        [HttpGet("{id}", Name = "GetAuthorById")]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var author = _mapper.Map<AuthorDto>(await _repository.GetAuthorByIdAsync(id));
            if (author is null) return NotFound(new ApiResponse(404, $"Author with id: {id} is not found"));
            return Ok(author);
        }

        // GET api/Authors/1/books
        [HttpGet("{id}/books")]
        public async Task<IActionResult> GetAuthorBooks(int id)
        {
            var author = await _repository.GetAuthorByIdAsync(id);
            if (author is null) return NotFound(new ApiResponse(404, $"Author with id: {id} is not found"));

            var authorBooks = _mapper.Map<List<BookDto>>(await _repository.GetAuthorBooksAsync(id));
            return Ok(authorBooks);
        }

        // POST api/authors
        [HttpPost]
        public async Task<IActionResult> CreateAuthor(AuthorDto authorDto)
        {
            if(!ModelState.IsValid) return BadRequest();
            var author = _mapper.Map<Author>(authorDto);
            await _repository.CreateAuthor(author);
            var createdAuthor = _repository.GetAuthorByIdAsync(author.Id);
            return CreatedAtRoute("GetAuthorById", new {id = createdAuthor.Id}, createdAuthor);
        }
    }
}