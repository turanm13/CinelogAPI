using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.DTOs.Account;
using Service.DTOs.Login;
using Service.Helpers.Responses;
using Service.Services.Interfaces;

namespace CinelogAPI.Controllers.UI
{
    public class AccountController : BaseUIController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;           
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = await _accountService.ConfirmEmailAsync(userId, token);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var confirmationUrlTemplate = $"{Request.Scheme}://{Request.Host}/api/account/ConfirmEmail?userId={{0}}&token={{1}}";

            var result = await _accountService.RegisterAsync(request, confirmationUrlTemplate);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return BadRequest(ModelState);

            var result = await _accountService.DeleteUserAsync(userId);

            if (!result.Success)
                return NotFound(result.Message);

            return Ok(result.Message);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser([FromRoute] string userId,[FromBody] UserUpdateDto user)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            var result = await _accountService.UpdateUserAsync(userId, user);

            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            LoginResponse response = await _accountService.LoginAsync(request);

            if (!response.Success) return BadRequest(response);
            return Ok(response);
        }


    }
}
