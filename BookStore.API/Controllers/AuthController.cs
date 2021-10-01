using System;
using System.Linq;
using System.Threading.Tasks;
using BookStore.API.Dtos;
using BookStore.API.Errors;
using BookStore.Application.Abstraction.Services;
using BookStore.Core.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers
{
    public class AuthController : BaseAPIController
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleMgr,
            ITokenService tokenService)
        {
            _roleManager = roleMgr;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("SignIn")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Unauthorized(new ApiResponse(401, "Bad credentials"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return Unauthorized(new ApiResponse(401, "Bad credentials"));

            var userRole = await _userManager.GetRolesAsync(user);
            var response = new SignInResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                DisplayName = user.UserName,
                Token = _tokenService.CreateToken(user, userRole.FirstOrDefault())
            };
            return Ok(response);
        }

        [HttpPost("Seed")]
        public async Task<IActionResult> Seed()
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Customer" });
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }

            if (!_userManager.Users.Any())
            {
                var customer = new User
                {
                    Email = "customer@gmail.com",
                    NormalizedEmail = "CUSTOMER@GMAIL.COM",
                    UserName = "Customer",
                    DisplayName = "Customer",
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
                    DisplayName = "Admin",
                    NormalizedUserName = "ADMIN",
                    PhoneNumber = "+111111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D")
                };


                var password = "Pa$$w0rd";
                await _userManager.CreateAsync(customer, password);
                await _userManager.CreateAsync(admin, password);

                await _userManager.AddToRoleAsync(customer, "Customer");
                await _userManager.AddToRoleAsync(admin, "Admin");
            }

            return Ok();
        }

    }
}