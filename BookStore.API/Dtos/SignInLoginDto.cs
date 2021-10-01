using System.ComponentModel.DataAnnotations;

namespace BookStore.API.Dtos
{
    public class SignInLoginDto
    {
        [Required(ErrorMessage = "Email is required field")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required field")]
        [MinLength(8, ErrorMessage = "Minimum length of password is 8 caracters")]
        public string Password { get; set; }
    }
}