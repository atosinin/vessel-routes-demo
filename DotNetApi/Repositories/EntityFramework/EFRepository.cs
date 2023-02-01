namespace DotNetApi.Repositories.EntityFramework
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly VesselContext _efContext;

        public EFRepository(VesselContext context)
        {
            _efContext = context;
        }

        // Basic queries

        public List<TEntity> GetAll()
        {
            return _efContext.Set<TEntity>().ToList();
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await Task.Run(() => _efContext.Set<TEntity>().ToList());
        }

        public TEntity? GetById(int id)
        {
            return _efContext.Set<TEntity>().Find(id);
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await _efContext.Set<TEntity>().FindAsync(id);
        }

        // Basic commands

        public void Create(TEntity entityToCreate)
        {
            _efContext.Set<TEntity>().Add(entityToCreate);
        }

        public async Task CreateAsync(TEntity entityToCreate)
        {
            await _efContext.Set<TEntity>().AddAsync(entityToCreate);
        }

        public void Update(TEntity entityToUpdate)
        {
            _efContext.Set<TEntity>().Update(entityToUpdate);
        }

        public async Task UpdateAsync(TEntity entityToUpdate)
        {
            await Task.Run(() => _efContext.Set<TEntity>().Update(entityToUpdate));
        }

        public void Delete(TEntity entityToDelete)
        {
            _efContext.Set<TEntity>().Remove(entityToDelete);
        }

        public async Task DeleteAsync(TEntity entityToDelete)
        {
            await Task.Run(() => _efContext.Set<TEntity>().Remove(entityToDelete));
        }
    }
}
