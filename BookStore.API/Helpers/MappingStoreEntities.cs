using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookStore.API.Dtos;
using BookStore.Core.Entities;

namespace BookStore.API.Helpers
{
    public class MappingStoreEntities : Profile
    {
        public MappingStoreEntities()
        {
            // Source -> Target

            // BookController
            CreateMap<Book, BookDto>()
                .ForMember(el => el.AuthorName, o => o.MapFrom(b => b.Author.FullName));

            CreateMap<BookDto, Book>();
            CreateMap<BookForCreateDto, Book>();

            //AuthorController
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();

            //CartController
            CreateMap<CartItem, CartItemDto>();
            CreateMap<CartItemDto, CartItem>();
            CreateMap<CartItemForCreateDto, CartItem>();
        }
    }
}