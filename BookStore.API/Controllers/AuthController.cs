using System.Linq;
using System.Threading.Tasks;
using BookStore.API.Dtos;
using BookStore.API.Errors;
using BookStore.Application.Abstraction.Services;
using BookStore.Core.Entities.Identity;
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
        public async Task<IActionResult> SignIn(SignInLoginDto loginDto)
        {
            if(!ModelState.IsValid) return BadRequest();

            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401, "Bad credentials"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
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
    }
}