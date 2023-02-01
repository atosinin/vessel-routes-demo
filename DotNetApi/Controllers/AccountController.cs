using DotNetApi.Config;
using DotNetApi.Helpers.Emails;
using DotNetApi.Helpers.Tokens;
using DotNetApi.Models;
using DotNetApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DotNetApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly FrontendSettings _frontendSettings;
        private readonly IEmailHelpers _emails;

        public AccountController(
            IIdentityService identityService,
            IOptions<FrontendSettings> frontendOptions,
            IEmailHelpers emails)
        {
            _identityService = identityService;
            _frontendSettings = frontendOptions.Value;
            _emails = emails;
        }

        // GET /api/Account/Login
        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return Redirect(_frontendSettings.LoginUrl);
        }

        // POST /api/Account/Login
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            UserAccount userAccount = await _identityService.AuthenticateAsync(login);
            TokensModel tokens = await _identityService.GetTokensAsync(userAccount);
            SetRefreshTokenCookie(tokens.RefreshToken);
            return Ok(new { token = tokens.AccessToken });
        }

        private void SetRefreshTokenCookie(RefreshTokenModel refreshToken)
        {
            // Append cookie with refresh token to the http response
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.ExpiresOn
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        // POST /api/Account/Register
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel register)
        {
            UserAccount userAccount = await _identityService.RegisterAsync(register);
            string confirmationToken = await _identityService.GetEmailConfirmationTokenAsync(userAccount);
            _emails.SendConfirmationLinkToNewUser(userAccount, confirmationToken);
            return Ok();
        }

        // GET /api/Account/ConfirmInvitation/{email}&{token}
        [AllowAnonymous]
        [HttpGet("ConfirmInvitation/{email}&{token}")]
        public async Task<IActionResult> ConfirmInvitation(string email, string token)
        {
            await _identityService.ConfirmInvitationAsync(email, token);
            // Redirect to login page on frontend
            return Redirect(
                string.Concat(_frontendSettings.LoginUrl, $"?email={email}")
            );
        }

        // POST /api/Account/ForgottenPassword
        [AllowAnonymous]
        [HttpPost("ForgottenPassword")]
        public async Task<IActionResult> ForgottenPassword([FromBody] ForgottenPasswordModel forgottenPassword)
        {
            UserAccount userAccount = await _identityService.GetUserAccountForForgottenPasswordModelAsync(forgottenPassword);
            string changePasswordToken = await _identityService.GetChangePasswordTokenAsync(userAccount);
            _emails.SendChangePasswordLink(userAccount, changePasswordToken);
            return Ok();
        }

        // POST /api/Account/ChangePassword
        [AllowAnonymous]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel changePassword)
        {
            await _identityService.ChangePasswordAsync(changePassword);
            UserAccount userAccount = await _identityService.AuthenticateAsync(changePassword);
            TokensModel tokens = await _identityService.GetTokensAsync(userAccount);
            SetRefreshTokenCookie(tokens.RefreshToken);
            return Ok(new { token = tokens.AccessToken });
        }

        // POST /api/Account/RefreshToken
        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            string? refreshToken = Request.Cookies["refreshToken"];
            TokensModel tokens = await _identityService.RefreshTokensAsync(refreshToken);
            SetRefreshTokenCookie(tokens.RefreshToken);
            return Ok(new { token = tokens.AccessToken });
        }

        // POST /api/Account/RevokeToken
        [AllowAnonymous]
        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeToken(string? refreshToken)
        {
            // Accept refresh token in request body or cookie
            var token = refreshToken ?? Request.Cookies["refreshToken"];
            await _identityService.RevokeRefreshTokenAsync(token);
            return Ok();
        }
    }
}
