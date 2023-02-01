using DotNetApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetApi.Repositories.EntityFramework
{
    public class EFVesselConfiguration : IEntityTypeConfiguration<Vessel>
    {
        public void Configure(EntityTypeBuilder<Vessel> builder)
        {
            builder.HasData(
                new Vessel
                {
                    Name = "Vessel 1",
                },
                new Vessel
                {
                    Name = "Vessel 2",
                },
                new Vessel
                {
                    Name = "Vessel 3",
                }
            );
        }
    }
}
