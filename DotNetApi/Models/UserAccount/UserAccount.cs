using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DotNetApi.Models
{
    public class UserAccount : IdentityUser
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string? Picture { get; set; }

        [Required]
        public DateTime TermsAcceptedOn { get; set; } = DateTime.MaxValue;

        public string? RefreshToken { get; set; }
    }
}
