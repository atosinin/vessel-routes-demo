using AutoMapper;

namespace DotNetApi.Models
{
    public class VesselProfile : Profile
    {
        public VesselProfile()
        {
            CreateMap<Vessel, VesselDTO>();
        }
    }
}
