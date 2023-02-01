using DotNetApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetApi.Repositories.EntityFramework
{
    public class WhateverContext : IdentityDbContext<UserAccount, UserRole, string>
    {
        // Tables for UserAccount and UserRole are managed by Microsoft Identity

        // Whatever tables

        public DbSet<Whatever> Whatevers => Set<Whatever>();

        // Constructors

        public WhateverContext() : base() { }
        public WhateverContext(DbContextOptions<WhateverContext> options) : base(options) { }

        // Init

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed database with user roles and admins
            modelBuilder.ApplyConfiguration(new EFUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new EFUserAccountConfiguration());
            modelBuilder.ApplyConfiguration(new EFIdentityUserRoleConfiguration());
        }
    }
}
