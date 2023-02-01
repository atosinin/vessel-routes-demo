using DotNetApi.Models;
using DotNetApi.Repositories.EntityFramework;

namespace DotNetApi.Repositories
{
    public class EFWhateverRepository : EFRepository<Whatever>, IWhateverRepository
    {
        public EFWhateverRepository(WhateverContext context) : base(context)
        {
        }

        public List<Whatever> GetAllByUserAccountId(string userId)
        {
            return _efContext.Whatevers
                .Where(w => w.UserAccountId == userId)
                .ToList();
        }
    }
}
