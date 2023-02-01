using DotNetApi.Models;

namespace DotNetApi.Repositories.EntityFramework
{
    public class EFPositionRepository : EFRepository<Position>, IPositionRepository
    {
        public EFPositionRepository(VesselContext context) : base(context)
        {
        }

        public List<Position> GetAllByVesselId(int vesselId)
        {
            return _efContext.Positions
                .Where(p => p.VesselId == vesselId)
                .ToList();
        }
    }
}
