using BookStore.Core.Entities.Identity;

namespace BookStore.Application.Abstraction.Services
{
    public interface ITokenService
    {
         string CreateToken(User user, string role);
    }
}