namespace DotNetApi.Repositories.EntityFramework
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly VesselContext _context;

        // Tables
        public IVesselRepository Vessels { get; private set; }
        public IPositionRepository Positions { get; private set; }

        // Constructor
        public EFUnitOfWork(VesselContext context)
        {
            _context = context;
            Vessels = new EFVesselRepository(_context);
            Positions = new EFPositionRepository(_context);
        }

        // Methods
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                GC.SuppressFinalize(this);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
