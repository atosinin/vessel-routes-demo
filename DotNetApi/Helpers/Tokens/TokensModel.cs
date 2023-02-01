namespace DotNetApi.Helpers.Tokens
{
    public class TokensModel
    {
        public string AccessToken { get; set; } = string.Empty;
        public RefreshTokenModel RefreshToken { get; set; } = new();
    }
}
