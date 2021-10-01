using System;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Core.Entities.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Persistance.Data.SeedData
{
    public static class IdentityContextSeed
    {

        public static void SeedUsers(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                Console.WriteLine("--> Start check data");
                SeedData(serviceScope.ServiceProvider.GetService<IdentityContext>(), serviceScope.ServiceProvider);
                Console.WriteLine("--> Finish checking data");

            }
        }
        private static async void SeedData(IdentityContext context, IServiceProvider services)
        {
            string[] roles = new string[] { "Customer", "Admin" };
            var roleStore = new RoleStore<IdentityRole>(context);

            foreach (string role in roles)
            {
                if (!context.Roles.Any(r => r.Name == role))
                {
                    await roleStore.CreateAsync(new IdentityRole(role));
                }
            }

            var customer = new User
            {
                Email = "customer@gmail.com",
                NormalizedEmail = "CUSTOMER@GMAIL.COM",
                UserName = "Customer",
                NormalizedUserName = "CUSTOMER",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            var admin = new User
            {
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                PhoneNumber = "+111111111111",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };


            if (!context.Users.Any())
            {
                var password = new PasswordHasher<User>();
                var hashed = password.HashPassword(customer, "Pa$$w0rd");
                customer.PasswordHash = hashed;
                admin.PasswordHash = hashed;

                var userStore = new UserStore<User>(context);
                await userStore.CreateAsync(customer);
                await userStore.CreateAsync(admin);
            }

            UserManager<User> _userManager = services.GetService<UserManager<User>>();
            await _userManager.AddToRoleAsync(customer, "Customer");
            await _userManager.AddToRoleAsync(admin, "Admin");


            await context.SaveChangesAsync();
        }
    }
}
