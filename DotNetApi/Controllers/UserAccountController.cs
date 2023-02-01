using DotNetApi.Models;
using DotNetApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAccountController : ControllerBase
    {
        private readonly IUserAccountService _userAccountServices;

        public UserAccountController(
            IUserAccountService userAccountServices)
        {
            _userAccountServices = userAccountServices;
        }

        // GET /api/UserAccount
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            List<UserAccountDTO> allUserAccountDTOs = await _userAccountServices.GetAllUserAccountDTOsAsync();
            return Ok(allUserAccountDTOs);
        }

        // GET /api/UserAccount/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(string id)
        {
            UserAccountDTO userAccountDTO = await _userAccountServices.GetUserAccountDTOByIdAsync(id);
            return Ok(userAccountDTO);
        }

        // PUT /api/UserAccount
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update([FromBody] UserAccountDTO userAccountUpdate)
        {
            await _userAccountServices.UpdateFromUserAccountDTOAsync(userAccountUpdate);
            return Ok();
        }

        // DELETE /api/UserAccount/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            await _userAccountServices.DeleteUserAccountByIdAsync(id);
            return Ok();
        }
    }
}