using AutoMapper;

namespace DotNetApi.Models
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<ChangePasswordModel, LoginModel>();
        }
    }
}
