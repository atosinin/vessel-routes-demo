using DotNetApi.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetApi.Repositories.EntityFramework
{
    public class EFIdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = WhateverAdminsConfig.FirstAdminId,
                    RoleId = UserRoleConfig.AdminId
                }
            );
        }
    }
}