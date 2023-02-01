using DotNetApi.Config;
using DotNetApi.Models;
using System.Security.Claims;

namespace DotNetApi.Helpers
{
    public static class CustomClaimTypes
    {
        public const string UserAccountId = "whatever_user_account_id";
    }

    public static class ClaimsPrincipalExtensions
    {
        // Get custom claims

        public static string GetUserAccountId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.FindFirstValue(CustomClaimTypes.UserAccountId);
        }

        // Control
        
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(UserRoleConfig.AdminName);
        }

        public static bool HasAccessToUserAccountById(this ClaimsPrincipal user, string userId)
        {
            return user.IsInRole(UserRoleConfig.AdminName)
                || user.GetUserAccountId() == userId;
        }

        public static bool HasAccessToWhatever(this ClaimsPrincipal user, Whatever whatever)
        {
            return user.IsInRole(UserRoleConfig.AdminName)
                || user.GetUserAccountId() == whatever.UserAccountId;
        }
    }
}
