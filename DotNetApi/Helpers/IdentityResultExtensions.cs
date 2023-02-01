using Microsoft.AspNetCore.Identity;

namespace DotNetApi.Helpers
{
    public static class IdentityResultExtensions
    {
        public static string AggregateErrors(this IdentityResult identityResult)
        {
            return identityResult.Errors.Select(e => e.Description).Aggregate<string>((a, b) => string.Concat(a, Environment.NewLine, b));
        }
    }
}
