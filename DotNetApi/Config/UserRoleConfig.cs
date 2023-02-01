namespace DotNetApi.Config
{
    public static class UserRoleConfig
    {
        // Do not change role names as they are hardcoded within AuthorizeAttribute in controllers

        public const string AdminName = "Admin";
        public const string AdminId = "ddc6498f-f7ff-46d6-b0e5-5a9735375c05";
        public const string AdminDescription = "Admin";

        public const string SimpleUserName = "SimpleUser";
        public const string SimpleUserId = "aeec8ad7-2d4f-4f1d-b14a-c97cdec553a6";
        public const string SimpleUserDescription = "User";
    }
}
