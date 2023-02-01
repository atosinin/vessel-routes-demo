using DotNetApi.Config;
using DotNetApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetApi.Repositories.EntityFramework
{
    public class EFUserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasData(
                new UserRole
                {
                    Id = UserRoleConfig.AdminId,
                    Name = UserRoleConfig.AdminName,
                    NormalizedName = UserRoleConfig.AdminName.ToUpper(),
                    Description = UserRoleConfig.AdminDescription,
                    ConcurrencyStamp = "57f5af88-1fe9-4ad6-b2c9-403bae09b4c2"
                },
                new UserRole
                {
                    Id = UserRoleConfig.SimpleUserId,
                    Name = UserRoleConfig.SimpleUserName,
                    NormalizedName = UserRoleConfig.SimpleUserName.ToUpper(),
                    Description = UserRoleConfig.SimpleUserDescription,
                    ConcurrencyStamp = "bedabfa6-b645-488c-990f-aa200e7308bf"
                }
            );
        }
    }
}
