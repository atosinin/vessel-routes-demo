namespace DotNetApi.Repositories.EntityFramework
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly WhateverContext _context;

        // Tables
        public IWhateverRepository Whatevers { get; private set; }

        // Constructor
        public EFUnitOfWork(WhateverContext context)
        {
            _context = context;
            Whatevers = new EFWhateverRepository(_context);
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
