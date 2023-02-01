namespace DotNetApi.Helpers.Tokens
{
    public class RefreshTokenModel
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresOn { get; set; } = DateTime.MinValue;
    }
}
