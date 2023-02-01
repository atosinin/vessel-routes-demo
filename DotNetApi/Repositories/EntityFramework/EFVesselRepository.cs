using DotNetApi.Models;

namespace DotNetApi.Repositories.EntityFramework
{
    public class EFVesselRepository : EFRepository<Vessel>, IVesselRepository
    {
        public EFVesselRepository(VesselContext context) : base(context)
        {
        }
    }
}
