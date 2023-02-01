using DotNetApi.Config;
using DotNetApi.Helpers.Exceptions;
using DotNetApi.Localization;
using DotNetApi.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DotNetApi.Helpers.Tokens
{
    public interface ITokenHelpers
    {
        TokensModel GenerateTokens(List<Claim> claims);
        List<Claim> ValidateJwtToken(string token);
    }

    public class TokenHelpers : ITokenHelpers
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IStringLocalizer<SharedLocalizer> _sharedLocalizer;

        public TokenHelpers(IOptions<JwtSettings> jwtOptions, IStringLocalizer<SharedLocalizer> sharedLocalizer)
        {
            _jwtSettings = jwtOptions.Value;
            _sharedLocalizer = sharedLocalizer;
        }

        public TokensModel GenerateTokens(List<Claim> claims)
        {
            TokensModel result = new();
            DateTime mainTokenExpiresOn = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenLifeSpanInMinutes);
            result.AccessToken = GenerateJwtToken(claims, mainTokenExpiresOn);
            DateTime refreshExpiresOn = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifeSpanInDays);
            claims.RemoveAll(c => c.Type != CustomClaimTypes.UserAccountId);
            result.RefreshToken.Token = GenerateJwtToken(claims, refreshExpiresOn);
            result.RefreshToken.ExpiresOn = refreshExpiresOn;
            return result;
        }

        private string GenerateJwtToken(List<Claim> claims, DateTime expiresOn)
        {
            var signingCredentials = new SigningCredentials(_jwtSettings.SecretSymmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = expiresOn,
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public List<Claim> ValidateJwtToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new WhateverUserMessageLoggedException("No token to validate");
            JwtSecurityTokenHandler tokenHandler = new();
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = _jwtSettings.SecretSymmetricSecurityKey,
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    // Set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken validatedToken
            );
            JwtSecurityToken jwtToken = (JwtSecurityToken)validatedToken;
            return jwtToken.Claims.ToList();
        }
    }
}
