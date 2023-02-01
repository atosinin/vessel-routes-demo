using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DotNetApi.Models
{
    public class UserRole : IdentityRole
    {
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
