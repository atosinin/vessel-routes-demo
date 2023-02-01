using AutoMapper;
using DotNetApi.Helpers.Exceptions;
using DotNetApi.Models;
using DotNetApi.Repositories;

namespace DotNetApi.Services
{
    public interface IPositionService
    {
        List<PositionDTO> GetAllPositionDTOs();
        List<PositionDTO> GetAllPositionDTOsByVesselId(int vesselId);
        PositionDTO GetPositionDTOById(int positionId);
        void CreateFromPositionDTO(PositionDTO positionCreateDTO);
        void UpdateFromPositionDTO(PositionDTO positionUpdateDTO);
        void DeletePositionById(int positionId);
    }

    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public PositionService(
            IUnitOfWork uow,
            IMapper mapper
        )
        {
            _uow = uow;
            _mapper = mapper;
        }

        public List<PositionDTO> GetAllPositionDTOs()
        {
            List<Position> allPositions = _uow.Positions.GetAll();
            return _mapper.Map<List<Position>, List<PositionDTO>>(allPositions);
        }

        public List<PositionDTO> GetAllPositionDTOsByVesselId(int vesselId)
        {
            List<Position> allPositions = _uow.Positions.GetAllByVesselId(vesselId);
            return _mapper.Map<List<Position>, List<PositionDTO>>(allPositions);
        }

        public PositionDTO GetPositionDTOById(int positionId)
        {
            Position? position = _uow.Positions.GetById(positionId);
            if (position is null)
                throw new BadRequestException("Invalid Id for table 'Position'");
            return _mapper.Map<PositionDTO>(position);
        }

        public void CreateFromPositionDTO(PositionDTO positionCreateDTO)
        {
            Vessel? vessel = _uow.Vessels.GetById(positionCreateDTO.VesselId);
            if (vessel is null)
                throw new BadRequestException("Invalid Id for table 'Vessel'");
            Position positionToCreate = new()
            {
                VesselId = positionCreateDTO.VesselId
            };
            positionToCreate.CreateFromPositionDTO(positionCreateDTO);
            _uow.Positions.Create(positionToCreate);
            _uow.SaveChanges();
        }

        public void UpdateFromPositionDTO(PositionDTO positionUpdateDTO)
        {
            Position? position = _uow.Positions.GetById(positionUpdateDTO.PositionId);
            if (position is null)
                throw new BadRequestException("Invalid Id for table 'Position'");
            position.UpdateFromPositionDTO(positionUpdateDTO);
            _uow.SaveChanges();
        }

        public void DeletePositionById(int positionId)
        {
            Position? position = _uow.Positions.GetById(positionId);
            if (position is null)
                throw new BadRequestException("Invalid Id for table 'Position'");
            _uow.Positions.Delete(position);
            _uow.SaveChanges();
        }
    }
}
