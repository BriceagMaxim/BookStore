using BookStore.API.Extensions;
using BookStore.API.Helpers;
using BookStore.API.Middleware;
using BookStore.Persistance.Data.SeedData;
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
            services.SetBookContext(_configuration);
            services.AddRepositories();
            services.AddBusinessServices();
            

            services.AddControllers();
            services.AddSwaggerDocumentation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwaggerDocumentation();
            
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookStore.API v1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.SeedUsers();
        }
    }
}
