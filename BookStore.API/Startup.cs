using BookStore.API.Extensions;
using BookStore.API.Helpers;
using BookStore.API.Middleware;
using BookStore.Application.Abstraction.Repositories;
using BookStore.Application.Abstraction.Services;
using BookStore.Infrastructure.Services;
using BookStore.Persistance.Data;
using BookStore.Persistance.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BookStore.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingStoreEntities));

            services.AddIdentityContext(_configuration.GetConnectionString("Identity"));
            services.AddIdentitySettings(_configuration);
            
            services.AddDbContext<BookStoreContext>(
                el => el.UseSqlServer(
                    _configuration.GetConnectionString("BookStore")
                    ));

            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();

            services.AddTransient<ICartService, CartService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookStore.API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore.API v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
