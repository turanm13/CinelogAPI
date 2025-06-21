using Service.DTOs.Account;
using Service.DTOs.Login;
using Service.Helpers.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interfaces
{
    public interface IAccountService
    {
        Task<RegisterResponse> RegisterAsync(RegisterDto model, string confirmationUrlTemplate);
        Task CreateRoleAsync();
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<IEnumerable<RoleDto>> GetAllRolesAsync();
        Task<OperationResult> AddRoleToUserAsync(string userId, string roleId);
        Task<OperationResult> RemoveUserFromRoleAsync(string userId, string roleId);
        Task<OperationResult> DeleteUserAsync(string userId);
        Task<OperationResult> UpdateUserAsync(string userId, UserUpdateDto user);
        Task<UserDto> GetUserByIdAsync(string userId);
        Task<LoginResponse> LoginAsync(LoginDto model);
        Task<OperationResult> ConfirmEmailAsync(string userId, string token);
        Task<UserDto> GetCurrentUserAsync();
    }
}


