using BookStore.Application.Abstraction.Services;
using BookStore.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<ICartService, CartService>();
            return services;
        }
    }
}