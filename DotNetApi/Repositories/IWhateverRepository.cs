using DotNetApi.Models;

namespace DotNetApi.Repositories
{
    public interface IWhateverRepository : IRepository<Whatever>
    {
        List<Whatever> GetAllByUserAccountId(string userId);
    }
}
