namespace BookStore.API.Dtos
{
    public class SignInResponseDto
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Token { get; set; }
    }
}