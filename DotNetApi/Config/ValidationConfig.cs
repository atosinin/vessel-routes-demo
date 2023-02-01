namespace DotNetApi.Config
{
    public static class ValidationConfig
    {
        public const int EmailMinimalLength = 7;
        public const int EmailMaximalLength = 128;

        public const int PasswordMinimalLength = 8;
        public const int PasswordMaximalLength = 128;
        public const string PasswordRegularExpression = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9]).*$";
    }
}
