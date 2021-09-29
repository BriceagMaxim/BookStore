using Microsoft.AspNetCore.Identity;

namespace BookStore.Core.Entities.Identity
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}