using BookStore.Application.Abstraction.Repositories;
using BookStore.Persistance.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Extensions
{
    public static class RepositoryExtension
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            return services;
        }
    }
}