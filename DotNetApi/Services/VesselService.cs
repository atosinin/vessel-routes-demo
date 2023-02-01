using AutoMapper;
using DotNetApi.Helpers.Exceptions;
using DotNetApi.Models;
using DotNetApi.Repositories;

namespace DotNetApi.Services
{
    public interface IVesselService
    {
        List<VesselDTO> GetAllVesselDTOs();
        VesselDTO GetVesselDTOById(int vesselId);
        void CreateFromVesselDTO(VesselDTO vesselCreateDTO);
        void UpdateFromVesselDTO(VesselDTO vesselUpdateDTO);
        void DeleteVesselById(int vesselId);
    }

    public class VesselService : IVesselService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public VesselService(
            IUnitOfWork uow,
            IMapper mapper
        )
        {
            _uow = uow;
            _mapper = mapper;
        }

        public List<VesselDTO> GetAllVesselDTOs()
        {
            List<Vessel> allVessels = _uow.Vessels.GetAll();
            return _mapper.Map<List<Vessel>, List<VesselDTO>>(allVessels);
        }

        public VesselDTO GetVesselDTOById(int vesselId)
        {
            Vessel? vessel = _uow.Vessels.GetById(vesselId);
            if (vessel is null)
                throw new BadRequestException("Invalid Id for table 'Vessel'");
            return _mapper.Map<VesselDTO>(vessel);
        }

        public void CreateFromVesselDTO(VesselDTO vesselCreateDTO)
        {
            Vessel vesselToCreate = new();
            vesselToCreate.CreateFromVesselDTO(vesselCreateDTO);
            _uow.Vessels.Create(vesselToCreate);
            _uow.SaveChanges();
        }

        public void UpdateFromVesselDTO(VesselDTO vesselUpdateDTO)
        {
            Vessel? vessel = _uow.Vessels.GetById(vesselUpdateDTO.VesselId);
            if (vessel is null)
                throw new BadRequestException("Invalid Id for table 'Vessel'");
            vessel.UpdateFromVesselDTO(vesselUpdateDTO);
            _uow.SaveChanges();
        }

        public void DeleteVesselById(int vesselId)
        {
            Vessel? vessel = _uow.Vessels.GetById(vesselId);
            if (vessel is null)
                throw new BadRequestException("Invalid Id for table 'Vessel'");
            _uow.Vessels.Delete(vessel);
            _uow.SaveChanges();
        }
    }
}
