using AutoMapper;
using DotNetApi.Config;
using DotNetApi.Helpers;
using DotNetApi.Helpers.Exceptions;
using DotNetApi.Helpers.Tokens;
using DotNetApi.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DotNetApi.Services
{
    public interface IIdentityService
    {
        // Register

        Task<UserAccount> RegisterAsync(RegisterModel register);
        Task<string> GetEmailConfirmationTokenAsync(UserAccount userAccount);
        Task<UserAccount> ConfirmInvitationAsync(string email, string token);

        // Login

        Task<UserAccount> AuthenticateAsync(LoginModel login);
        Task<TokensModel> GetTokensAsync(UserAccount userAccount);

        // Change password

        Task<string> GetChangePasswordTokenAsync(UserAccount userAccount);
        Task ChangePasswordAsync(ChangePasswordModel changePassword);
        Task<UserAccount> AuthenticateAsync(ChangePasswordModel changePassword);

        // Forgotten password

        Task<UserAccount> GetUserAccountForForgottenPasswordModelAsync(ForgottenPasswordModel forgottenPassword);

        // Refresh tokens

        Task<TokensModel> RefreshTokensAsync(string? refreshToken);
        Task RevokeRefreshTokenAsync(string? refreshToken);
    }

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly ITokenHelpers _jwtHelpers;
        private readonly IMapper _mapper;

        public IdentityService(
            UserManager<UserAccount> userManager,
            SignInManager<UserAccount> signInManager,
            ITokenHelpers jwtHelpers,
            IMapper mapper
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtHelpers = jwtHelpers;
            _mapper = mapper;
        }

        // Register, login, change and forgotten password

        public async Task<UserAccount> RegisterAsync(RegisterModel register)
        {
            UserAccount userToCreate = new() ;
            userToCreate.CreateFromRegisterModel(register);
            // Password
            IdentityResult userResult = await _userManager.CreateAsync(userToCreate, register.Password);
            if (!userResult.Succeeded)
                throw new WhateverBadRequestException(userResult.AggregateErrors());
            // Assign SimpleUser role
            UserAccount createdUser = await _userManager.FindByEmailAsync(userToCreate.Email);
            IdentityResult roleResult = await _userManager.AddToRoleAsync(userToCreate, UserRoleConfig.SimpleUserName);
            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(createdUser);
                throw new WhateverBadRequestException(roleResult.AggregateErrors());
            }
            return createdUser;
        }

        public async Task<string> GetEmailConfirmationTokenAsync(UserAccount userAccount)
        {
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(userAccount);
            string hexToken = token.EncodeToHexString();
            return hexToken;
        }

        public async Task<UserAccount> ConfirmInvitationAsync(string email, string hexToken)
        {
            UserAccount userAccount = await _userManager.FindByEmailAsync(email);
            if (userAccount == null)
                throw new WhateverUserMessageLoggedException("Invalid email");
            if (!userAccount.EmailConfirmed)
            {
                string token;
                try
                {
                    token = hexToken.DecodeFromHexString();
                }
                catch (Exception)
                {
                    // An exception is raised if token is not hexadecimal
                    throw new WhateverUserMessageLoggedException("Invalid token");
                }
                IdentityResult emailResult = await _userManager.ConfirmEmailAsync(userAccount, token);
                if (!emailResult.Succeeded)
                    throw new WhateverBadRequestException(emailResult.AggregateErrors());
            }
            return userAccount;
        }

        public async Task<UserAccount> AuthenticateAsync(LoginModel login)
        {
            SignInResult signInResult = await _signInManager.PasswordSignInAsync(login.Email, login.Password, true, false);
            if (!signInResult.Succeeded)
                throw new WhateverUserMessageLoggedException("Invalid credentials");
            UserAccount userAccount = await _userManager.FindByEmailAsync(login.Email);
            return userAccount;
        }

        public async Task<TokensModel> GetTokensAsync(UserAccount userAccount)
        {
            // Add Email, UserAccountId to Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAccount.Email),
                new Claim(CustomClaimTypes.UserAccountId, userAccount.Id),
            };
            // Retrieve roles and add to Claims
            IList<string> roles = await _userManager.GetRolesAsync(userAccount);
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            // Get tokens
            TokensModel result = _jwtHelpers.GenerateTokens(claims);
            await RotateRefreshTokenForUserAccount(result.RefreshToken, userAccount);
            return result;
        }

        private async Task RotateRefreshTokenForUserAccount(RefreshTokenModel refreshToken, UserAccount userAccount)
        {
            userAccount.RefreshToken = refreshToken.Token;
            IdentityResult updateResult = await _userManager.UpdateAsync(userAccount);
            if (!updateResult.Succeeded)
                throw new WhateverBadRequestException(updateResult.AggregateErrors());
        }

        public Task<UserAccount> AuthenticateAsync(ChangePasswordModel changePassword)
        {
            LoginModel login = _mapper.Map<LoginModel>(changePassword);
            return AuthenticateAsync(login);
        }

        public async Task<string> GetChangePasswordTokenAsync(UserAccount userAccount)
        {
            string passwordToken = await _userManager.GeneratePasswordResetTokenAsync(userAccount);
            string hexPasswordToken = passwordToken.EncodeToHexString();
            return hexPasswordToken;
        }

        public async Task ChangePasswordAsync(ChangePasswordModel changePassword)
        {
            UserAccount userAccount = await _userManager.FindByEmailAsync(changePassword.Email);
            if (userAccount == null)
                throw new WhateverUserMessageLoggedException("Invalid email");
            if (!userAccount.EmailConfirmed)
                throw new WhateverUserMessageOnlyException("Confirm email first");
            string token;
            try
            {
                token = changePassword.Token.DecodeFromHexString();
            }
            catch (Exception)
            {
                // An exception is raised if token is not hexadecimal
                throw new WhateverUserMessageLoggedException("Invalid token");
            }
            IdentityResult changePasswordResult = await _userManager.ResetPasswordAsync(userAccount, token, changePassword.Password);
            if (!changePasswordResult.Succeeded)
                throw new WhateverBadRequestException(changePasswordResult.AggregateErrors());
        }

        public async Task<UserAccount> GetUserAccountForForgottenPasswordModelAsync(ForgottenPasswordModel forgottenPassword)
        {
            UserAccount userAccount = await _userManager.FindByEmailAsync(forgottenPassword.Email);
            if (userAccount == null)
                throw new WhateverUserMessageLoggedException("Invalid email");
            return userAccount;
        }

        public async Task<TokensModel> RefreshTokensAsync(string? refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new WhateverUserMessageLoggedException("Invalid token");
            List<Claim> claims = _jwtHelpers.ValidateJwtToken(refreshToken);
            string userId = claims.First(c => c.Type == CustomClaimTypes.UserAccountId).Value;
            UserAccount userAccount = await _userManager.FindByIdAsync(userId);
            if (userAccount == null)
                throw new WhateverUserMessageLoggedException("Invalid token");
            if (userAccount.RefreshToken != refreshToken)
            {
                await RevokeRefreshTokenForUserAccount(userAccount);
                throw new WhateverUserMessageLoggedException("Invalid token");
            }
            return await GetTokensAsync(userAccount);
        }

        private async Task RevokeRefreshTokenForUserAccount(UserAccount userAccount)
        {
            userAccount.RefreshToken = null;
            IdentityResult updateResult = await _userManager.UpdateAsync(userAccount);
            if (!updateResult.Succeeded)
                throw new WhateverBadRequestException(updateResult.AggregateErrors());
        }

        public async Task RevokeRefreshTokenAsync(string? refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new WhateverBadRequestException("No refresh token to revoke");
            List<Claim> claims = _jwtHelpers.ValidateJwtToken(refreshToken);
            string tokenUserId = claims.First(c => c.Type == CustomClaimTypes.UserAccountId).Value;
            UserAccount userAccount = await _userManager.FindByIdAsync(tokenUserId);
            await RevokeRefreshTokenForUserAccount(userAccount);
        }
    }
}
