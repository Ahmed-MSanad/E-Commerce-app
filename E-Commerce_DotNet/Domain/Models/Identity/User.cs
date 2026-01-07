using Microsoft.AspNetCore.Identity;
namespace Domain.Models.Identity
{
    public class User : IdentityUser
    {
        public string DisplayName { get; set; }
        public Address Address { get; set; }
    }
}
