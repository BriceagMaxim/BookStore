using System.Text;
using BookStore.Core.Entities.Identity;
using BookStore.Persistance.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BookStore.API.Extensions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddIdentitySettings(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password = new PasswordOptions
                {
                    RequiredLength = 8,
                    RequireUppercase = true,
                    RequireNonAlphanumeric = true,
                    RequireDigit = true,
                    RequireLowercase = true
                };
            })
                .AddEntityFrameworkStores<IdentityContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Token:Issuer"],
                        ValidAudience = config["Token:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    };
                });

            return services;
        }
    }
}