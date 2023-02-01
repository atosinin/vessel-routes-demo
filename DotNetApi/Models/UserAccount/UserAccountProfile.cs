using AutoMapper;

namespace DotNetApi.Models
{
    public class UserAccountProfile : Profile
    {
        public UserAccountProfile()
        {
            CreateMap<UserAccount, UserAccountDTO>();
        }
    }
}
