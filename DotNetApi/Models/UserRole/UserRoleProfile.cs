using AutoMapper;

namespace DotNetApi.Models
{
    public class UserRoleProfile : Profile
    {
        public UserRoleProfile()
        {
            CreateMap<UserRole, UserRoleDTO>();
        }
    }
}
