using DotNetApi.Models;

namespace DotNetApi.Repositories
{
    public interface IPositionRepository : IRepository<Position>
    {
        List<Position> GetAllByVesselId(int vesselId);
    }
}
