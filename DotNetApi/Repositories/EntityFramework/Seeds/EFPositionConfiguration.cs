using DotNetApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetApi.Repositories.EntityFramework
{
    public class EFPositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> builder)
        {
            builder.HasData(
                new Position
                {
                    VesselId = 1,
                    X = 30,
                    Y = 5,
                    Timestamp = DateTime.ParseExact("2020-01-01T08:20Z", "yyyy-MM-dd'T'HH:mm'Z", null),
                }
            ); ;
        }
    }
}
