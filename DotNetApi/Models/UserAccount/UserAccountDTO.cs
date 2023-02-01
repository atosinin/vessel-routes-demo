namespace DotNetApi.Models
{
    public class UserAccountDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Picture { get; set; }
        public DateTime TermsAcceptedOn { get; set; }
        public List<UserRoleDTO> Roles { get; set; } = new List<UserRoleDTO>();
    }
}
