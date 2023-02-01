using AutoMapper;

namespace DotNetApi.Models
{
    public class WhateverProfile : Profile
    {
        public WhateverProfile()
        {
            CreateMap<Whatever, WhateverDTO>();
        }
    }
}
