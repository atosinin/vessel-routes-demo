using AutoMapper;

namespace DotNetApi.Models
{
    public class PositionProfile : Profile
    {
        public PositionProfile()
        {
            CreateMap<Position, PositionDTO>();
        }
    }
}
