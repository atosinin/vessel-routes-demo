namespace DotNetApi.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        // Tables associées aux UserAccount et UserAccountRole sont gérés par Microsoft Identity

        // Tables

        IVesselRepository Vessels { get; }
        IPositionRepository Positions { get; }

        // Methods

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
