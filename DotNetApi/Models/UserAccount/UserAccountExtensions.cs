namespace DotNetApi.Models
{
    public static class UserAccountExtensions
    {
        public static void CreateFromRegisterModel(this UserAccount user, RegisterModel registerModel)
        {
            user.Email = registerModel.Email.Trim();
            user.UserName = user.Email;
            user.FirstName = registerModel.FirstName.Trim();
            user.LastName = registerModel.LastName.Trim();
            user.TermsAcceptedOn = DateTime.UtcNow;
        }

        public static void UpdateFromUserAccountDTO(this UserAccount user, UserAccountDTO accountDTO)
        {
            user.FirstName = accountDTO.FirstName.Trim();
            user.LastName = accountDTO.LastName.Trim();
            user.Picture = accountDTO.Picture;
        }
    }
}
