using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Interfaces;

namespace CinelogAPI.Controllers.Admin
{
    public class AccountController : BaseAdminController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateRoles()
        //{
        //    await _accountService.CreateRoleAsync();
        //    return Ok();
        //}
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _accountService.GetAllUsersAsync());
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesWithUsers()
        {
            return Ok(await _accountService.GetAllRolesAsync());
        }
        [HttpPut]
        public async Task<IActionResult> AddUserToRole(string userId, string roleId)
        {
            var result= await _accountService.AddRoleToUserAsync(userId, roleId);

            if (!result.Success) 
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveUserFromRole(string userId, string roleId)
        {
            var result = await _accountService.RemoveUserFromRoleAsync(userId, roleId);
            if (!result.Success) 
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute]string userId)
        {
            var user = await _accountService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound($"User with Id {userId} not found.");

            return Ok(user);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _accountService.GetCurrentUserAsync();

            return Ok(user);
        }




    }
}
