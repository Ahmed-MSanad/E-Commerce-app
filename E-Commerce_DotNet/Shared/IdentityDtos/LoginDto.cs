using System.ComponentModel.DataAnnotations;

namespace Shared.IdentityDtos
{
    public record LoginDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
