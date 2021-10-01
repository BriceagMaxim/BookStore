using AutoMapper;
using BookStore.API.Controllers;
using BookStore.API.Dtos;
using BookStore.API.Helpers;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace BookStore.Tests
{
    public class BookControllerTests
    {
        private readonly Book _book;
        private readonly Mock<IBookRepository> _repository;
        private readonly Mock<IAuthorRepository> _authRepository;
        private readonly IMapper _mapper;

        public BookControllerTests()
        {
            _book = new Book
            {
                Id = 1,
                Title = "B1",
                Description = "D1",
                Price = 10,
                AuthorId = 1,
                Author = new Author
                {
                    FullName = "name"
                }
            };
            _repository = new();
            _repository.Setup(el => el.GetBookByIdAsync(1))
            .ReturnsAsync(_book);

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingStoreEntities()); //your automapperprofile 
            });
            _mapper = mockMapper.CreateMapper();
        }

        [Fact]
        public void GetBookById_ShouldReturn_Ok_WithJsonResponse()
        {
            BooksController controller = new BooksController(
                    _repository.Object,
                    _authRepository.Object,
                    _mapper);

            var okResult = controller.GetBookById(1);
            var responseObject = okResult.Result as OkObjectResult;
            var item = Assert.IsType<BookDto>(responseObject.Value);

            Assert.IsType<OkObjectResult>(okResult.Result);
            Assert.Equal(_book.Title, item.Title);
            Assert.Equal(_book.Description, item.Description);
            Assert.Equal(_book.Price, item.Price);
            Assert.Equal(_book.AuthorId, item.AuthorId);
            Assert.Equal(_book.Author.FullName, item.AuthorName);
        }

        [Fact]
        public void GetBookById_ShouldReturn_NotFound()
        {
            BooksController controller = new BooksController(
                _repository.Object,
                _authRepository.Object,
            _mapper);

            var notFoundResult = controller.GetBookById(2);

            Assert.IsType<NotFoundObjectResult>(notFoundResult.Result);
        }
    }
}