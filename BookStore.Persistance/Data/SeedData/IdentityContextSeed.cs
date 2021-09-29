using System.Linq;
using System.Threading.Tasks;
using BookStore.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Persistance.Data.SeedData
{
    public class IdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> rolemanager)
        {
            var customerRole = new IdentityRole { Name = "Customer" };
            var adminRole = new IdentityRole { Name = "Admin" };
            if (!rolemanager.Roles.Any())
            {
                await rolemanager.CreateAsync(customerRole);
                await rolemanager.CreateAsync(adminRole);
            }

            if (!userManager.Users.Any())
            {
                var customer = new User
                {
                    DisplayName = "customer",
                    Email = "customer@gmail.com",
                    UserName = "customer",
                };
                await userManager.CreateAsync(customer, "Pa$$w0rd");
                await userManager.AddToRoleAsync(customer, adminRole.Name);

                var admin = new User
                {
                    DisplayName = "Max",
                    Email = "briceagmaxim@gmail.com",
                    UserName = "Max",
                };
                await userManager.CreateAsync(admin, "Pa$$w0rd");
                await userManager.AddToRoleAsync(admin, adminRole.Name);
            }
        }
    }
}