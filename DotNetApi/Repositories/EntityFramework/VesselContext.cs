using DotNetApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetApi.Repositories.EntityFramework
{
    public class VesselContext : DbContext
    {
        // Tables for UserAccount and UserRole are managed by Microsoft Identity

        // Vessel tables

        public DbSet<Vessel> Vessels => Set<Vessel>();
        public DbSet<Position> Positions => Set<Position>();

        // Constructors

        public VesselContext() : base() { }
        public VesselContext(DbContextOptions<VesselContext> options) : base(options) { }

        // Init

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed database with user roles and admins
            modelBuilder.ApplyConfiguration(new EFVesselConfiguration());
            modelBuilder.ApplyConfiguration(new EFPositionConfiguration());
        }
    }
}
