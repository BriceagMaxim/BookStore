using BookStore.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Extensions
{
    public static class BookContextExtension
    {
        public static IServiceCollection SetBookContext(
                this IServiceCollection services,
                IConfiguration configuration)
        {
            services.AddDbContext<BookStoreContext>(
                el => el.UseSqlServer(
                    configuration.GetConnectionString("BookStore")
                    ));

            return services;
        }
    }
}