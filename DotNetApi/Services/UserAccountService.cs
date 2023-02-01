using AutoMapper;
using DotNetApi.Helpers;
using DotNetApi.Helpers.Exceptions;
using DotNetApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DotNetApi.Services
{
    public interface IUserAccountService
    {
        // Queries

        Task<List<UserAccountDTO>> GetAllUserAccountDTOsAsync();
        Task<UserAccountDTO> GetUserAccountDTOByIdAsync(string userId);

        // Commands

        Task UpdateFromUserAccountDTOAsync(UserAccountDTO myProfileUpdate);
        Task DeleteUserAccountByIdAsync(string userId);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly ClaimsPrincipal _user;

        public UserAccountService(
            UserManager<UserAccount> userManager,
            RoleManager<UserRole> roleManager,
            IMapper mapper,
            ClaimsPrincipal user
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _user = user;
        }

        // Queries

        public async Task<List<UserAccountDTO>> GetAllUserAccountDTOsAsync()
        {
            if (!_user.IsAdmin())
                throw new WhateverBadRequestException("Unauthorized user");
            List<UserAccount> allUserAccounts = await _userManager.Users.ToListAsync();
            // Do not get roles
            return _mapper.Map<List<UserAccount>, List<UserAccountDTO>>(allUserAccounts);
        }

        public async Task<UserAccountDTO> GetUserAccountDTOByIdAsync(string userId)
        {
            if (!_user.HasAccessToUserAccountById(userId))
                throw new WhateverBadRequestException("Unauthorized user");
            UserAccount userAccount = await _userManager.FindByIdAsync(userId);
            if (userAccount == null)
                throw new WhateverBadRequestException("Invalid Id for table 'UserAccount'");
            UserAccountDTO myUserDTO = _mapper.Map<UserAccountDTO>(userAccount);
            // Get roles for User
            List<string> userRoleNames = (await _userManager.GetRolesAsync(userAccount)).ToList();
            foreach (var roleName in userRoleNames)
            {
                UserRole role = await _roleManager.FindByNameAsync(roleName);
                myUserDTO.Roles.Add(_mapper.Map<UserRoleDTO>(role));
            }
            return myUserDTO;
        }

        // Commands 

        public async Task UpdateFromUserAccountDTOAsync(UserAccountDTO myProfileUpdate)
        {
            if (!_user.HasAccessToUserAccountById(myProfileUpdate.Id))
                throw new WhateverBadRequestException("Unauthorized user");
            UserAccount userAccount = await _userManager.FindByIdAsync(myProfileUpdate.Id);
            if (userAccount == null)
                throw new WhateverBadRequestException("Invalid Id for table 'UserAccount'");
            userAccount.UpdateFromUserAccountDTO(myProfileUpdate);
            IdentityResult updateResult = await _userManager.UpdateAsync(userAccount);
            if (!updateResult.Succeeded)
                throw new WhateverBadRequestException(updateResult.AggregateErrors());
        }

        public async Task DeleteUserAccountByIdAsync(string userId)
        {
            if (!_user.HasAccessToUserAccountById(userId))
                throw new WhateverBadRequestException("Unauthorized user");
            UserAccount userAccount = await _userManager.FindByIdAsync(userId);
            if (userAccount == null)
                throw new WhateverBadRequestException("Invalid Id for table 'UserAccount'");
            IdentityResult deleteResult = await _userManager.DeleteAsync(userAccount);
            if (!deleteResult.Succeeded)
                throw new WhateverBadRequestException(deleteResult.AggregateErrors());
        }
    }
}
