using DotNetApi.Config;
using DotNetApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetApi.Repositories.EntityFramework
{
    public class EFUserAccountConfiguration : IEntityTypeConfiguration<UserAccount>
    {
        public void Configure(EntityTypeBuilder<UserAccount> builder)
        {
            UserAccount firstAdmin = new()
            {
                Id = WhateverAdminsConfig.FirstAdminId,
                UserName = WhateverAdminsConfig.FirstAdminEmail,
                NormalizedUserName = WhateverAdminsConfig.FirstAdminEmail.ToUpper(),
                Email = WhateverAdminsConfig.FirstAdminEmail,
                NormalizedEmail = WhateverAdminsConfig.FirstAdminEmail.ToUpper(),
                EmailConfirmed = true,
                FirstName = WhateverAdminsConfig.FirstAdminFirstName,
                LastName = WhateverAdminsConfig.FirstAdminLastName,
                TermsAcceptedOn = DateTime.MinValue,
                PasswordHash = WhateverAdminsConfig.FirstAdminPasswordHash,
                ConcurrencyStamp = WhateverAdminsConfig.FirstAdminConcurrencyStamp,
                SecurityStamp = WhateverAdminsConfig.FirstAdminSecurityStamp,
            };
            builder.HasData(firstAdmin);
        }
    }
}