using DotNetApi.Helpers;
using DotNetApi.Helpers.Exceptions;
using DotNetApi.Models;
using DotNetApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserAccountService _userAccountServices;

        public ProfileController(
            IUserAccountService userAccountServices)
        {
            _userAccountServices = userAccountServices;
        }

        // GET /api/Profile
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            UserAccountDTO userAccountDTO = await _userAccountServices.GetUserAccountDTOByIdAsync(User.GetUserAccountId());
            return Ok(userAccountDTO);
        }

        // PUT /api/Profile
        [HttpPut]
        public async Task<IActionResult> UpdateProfile([FromBody] UserAccountDTO userAccountUpdate)
        {
            await _userAccountServices.UpdateFromUserAccountDTOAsync(userAccountUpdate);
            return Ok();
        }

        // POST /api/Profile/SetCulture
        [AllowAnonymous]
        [HttpPost("SetCulture")]
        public IActionResult SetCulture(string culture)
        {
            try
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1),
                        IsEssential = true,
                        Secure = true, // To make it work with consent cookie
                        HttpOnly = true
                    }
                );
                return Ok();
            }
            catch (Exception ex)
            {
                throw new WhateverBadRequestException(ex.Message);
            }
        }
    }
}