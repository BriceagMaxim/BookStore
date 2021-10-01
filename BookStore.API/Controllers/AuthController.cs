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
using Microsoft.Extensions.Logging;

namespace BookStore.API.Controllers
{
    public class AuthController : BaseAPIController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public AuthController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleMgr,
            ITokenService tokenService,
            ILogger<AuthController> logger)
        {
            _logger = logger;
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
            if (user is null) 
            {
                _logger.LogInformation($"User with  email: {email}, doesn't exist");
                return Unauthorized(new ApiResponse(401, "Bad credentials"));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) 
            {
                _logger.LogInformation($"Wrong password for user with email: {email}");
                return Unauthorized(new ApiResponse(401, "Bad credentials"));
            }

            var userRole = await _userManager.GetRolesAsync(user);
            var response = new SignInResponseDto
            {
                UserId = user.Id,
                Email = user.Email,
                DisplayName = user.UserName,
                Token = _tokenService.CreateToken(user, userRole.FirstOrDefault())
            };
             _logger.LogInformation($"Succes authentication for user: {user.Email}");
            return Ok(response);
        }

        [HttpPost("Seed")]
        public async Task<IActionResult> Seed()
        {
            if (!_roleManager.Roles.Any())
            {
                _logger.LogInformation($"Start seeding roles");
                await _roleManager.CreateAsync(new IdentityRole { Name = "Customer" });
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
                _logger.LogInformation($"Finish seeding roles");
            }

            if (!_userManager.Users.Any())
            {
                _logger.LogInformation($"Start seeding users");
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
                _logger.LogInformation($"Finish seeding roles");

                _logger.LogInformation($"Start seeding userroles");
                await _userManager.AddToRoleAsync(customer, "Customer");
                await _userManager.AddToRoleAsync(admin, "Admin");
                _logger.LogInformation($"Finish seeding userroles");
            }

            return Ok();
        }

    }
}