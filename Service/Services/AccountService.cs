using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.DTOs.Account;
using Service.DTOs.Login;
using Service.Helpers;
using Service.Helpers.Enums;
using Service.Helpers.Responses;
using Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly JWTSetting _jwt;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<AppUser> userManager,
                              RoleManager<IdentityRole> roleManager,
                              IOptions<JWTSetting> options,
                              IHttpContextAccessor httpContextAccessor,
                              IEmailSender emailSender,
                              ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = options.Value;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<OperationResult> AddRoleToUserAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with Id {UserId} not found while adding role.", userId);
                return OperationResult.FailureResult("User not found!");
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                _logger.LogWarning("Role with Id {RoleId} not found.", roleId);
                return OperationResult.FailureResult("Role not found!");
            }

            var roleName = await _roleManager.GetRoleNameAsync(role);
            if (string.IsNullOrWhiteSpace(roleName))
            {
                _logger.LogWarning("Role name is empty for role Id {RoleId}.", roleId);
                return OperationResult.FailureResult("InvalId role name.");
            }

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);
            if (isInRole)
            {
                _logger.LogInformation("User with Id {UserId} is already in role {RoleName}.", userId, roleName);
                return OperationResult.FailureResult("User already in this role.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to add role {RoleName} to user {UserId}: {Errors}", roleName, userId, errors);
                return OperationResult.FailureResult(errors);
            }

            _logger.LogInformation("Role {RoleName} successfully added to user {UserId}.", roleName, userId);
            return OperationResult.SuccessResult("Role added to user successfully");
        }

        public async Task<OperationResult> ConfirmEmailAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("Email confirmation failed: user with Id {UserId} not found.", userId);
                return new OperationResult
                {
                    Success = false,
                    Message = "User not found!"
                };
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                _logger.LogInformation("User {UserId} successfully confirmed their email.", userId);
                return new OperationResult
                {
                    Success = true,
                    Message = "Email successfully verified."
                };
            }

            var errorDescriptions = string.Join("; ", result.Errors.Select(e => e.Description));
            _logger.LogError("Email confirmation failed for user {UserId}. Errors: {Errors}", userId, errorDescriptions);

            return new OperationResult
            {
                Success = false,
                Message = errorDescriptions
            };
        }

        public async Task CreateRoleAsync()
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
                }
            }
        }

        public async Task<OperationResult> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("DeleteUser failed: user with Id {UserId} not found.", userId);
                return OperationResult.FailureResult("User not found!");
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to delete user {UserId}: {Errors}", userId, errors);
                return OperationResult.FailureResult($"Failed to delete user: {errors}");
            }

            _logger.LogInformation("User {UserId} deleted successfully.", userId);
            return OperationResult.SuccessResult("User removed successfully");
        }

        public async Task<IEnumerable<RoleDto>> GetAllRolesAsync()
        {
            var roles = new List<RoleDto>();

            var rolesDb = await _roleManager.Roles.ToListAsync();

            if (rolesDb == null || !rolesDb.Any())
            {
                _logger.LogWarning("No roles found in the system.");
                return roles; 
            }

            foreach (var role in rolesDb)
            {
                try
                {
                    var users = await _userManager.GetUsersInRoleAsync(role.Name);

                    roles.Add(new RoleDto
                    {
                        Name = role.Name,
                        Users = users.Select(m => new UsersInRolesDto
                        {
                            FullName = m.FullName,
                            UserName = m.UserName,
                            Email = m.Email,
                            Age = m.Age
                        }).ToArray()
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while retrieving users for role: {RoleName}", role.Name);                    
                }
            }

            return roles;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = new List<UserDto>();

            var usersDb = await _userManager.Users.ToListAsync();

            if (usersDb == null || !usersDb.Any())
            {
                _logger.LogWarning("No users found in the system.");
                return users; 
            }

            foreach (var item in usersDb)
            {
                try
                {
                    var userRoles = await _userManager.GetRolesAsync(item);

                    users.Add(new UserDto
                    {
                        FullName = item.FullName,
                        UserName = item.UserName,
                        Email = item.Email,
                        Age = item.Age,
                        Roles = userRoles.ToArray()
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while retrieving roles for user: {UserName}", item.UserName);
                }
            }

            return users;
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("Username is null or empty in GetCurrentUserAsync.");
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                _logger.LogWarning("User not found with username: {Username}", username);
                throw new KeyNotFoundException("User not found.");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Age = user.Age,
                Email = user.Email,
                Roles = userRoles.ToArray(),
            };
        }

        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException($"User with Id {userId} not found.");
            }

            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                Age = user.Age,
                Roles = roles.ToArray()
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginDto model)
        {
            AppUser user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);

            if (user == null)
                user = await _userManager.FindByNameAsync(model.UserNameOrEmail);

            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found for {UserNameOrEmail}", model.UserNameOrEmail);
                return new LoginResponse { Success = false, Token = null, ErrorMessage = "User not found!" };
            }

            bool result = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!result)
            {
                _logger.LogWarning("Login failed: Incorrect password for user {UserName}", user.UserName);
                return new LoginResponse { Success = false, Token = null, ErrorMessage = "Password is wrong" };
            }

            var roles = await _userManager.GetRolesAsync(user);

            string token = GenerateJwtToken(user.Id, user.UserName, roles.ToList());

            _logger.LogInformation("User {UserName} logged in successfully.", user.UserName);

            return new LoginResponse { Success = true, Token = token, ErrorMessage = null };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterDto model,string confirmationUrlTemplate)
        {
            var newUser = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
                Age = model.Age
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning("User registration failed for {Email}. Errors: {Errors}",
                    model.Email, string.Join(", ", result.Errors.Select(e => e.Description)));

                return new RegisterResponse
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToArray()
                };
            }

            _logger.LogInformation("User created successfully: {UserId}, {Email}", newUser.Id, newUser.Email);

            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var confirmationUrl = string.Format(confirmationUrlTemplate, newUser.Id, HttpUtility.UrlEncode(token));

            var emailBody = $@"
                                    <p>Please click the button below to confirm your email:</p>
                                    <a href='{confirmationUrl}' 
                                       style='
                                          display: inline-block;
                                          padding: 12px 32px;
                                          background: #E50914;
                                          color: #fff !important;
                                          border-radius: 6px;
                                          font-size: 16px;
                                          font-weight: bold;
                                          text-decoration: none;
                                          text-align: center;
                                          box-shadow: 0 2px 8px rgba(229,9,20,0.10);
                                          letter-spacing: 1px;
                                          margin: 16px 0;
                                          transition: background 0.2s;
                                       '
                                       target='_blank'>
                                       Confirm Email
                                    </a>
";

            await _emailSender.SendEmailAsync(newUser.Email, "Confirm your email.", emailBody);

            _logger.LogInformation("Confirmation email sent to {Email}", newUser.Email);

            return new RegisterResponse { Success = true, Errors = null };
        }

        public async Task<OperationResult> RemoveUserFromRoleAsync(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with Id {UserId} not found.", userId);
                return OperationResult.FailureResult("User not found!");
            }

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                _logger.LogWarning("Role with Id {RoleId} not found.", roleId);
                return OperationResult.FailureResult("Role not found!");
            }

            var roleName = await _roleManager.GetRoleNameAsync(role);
            if (string.IsNullOrWhiteSpace(roleName))
            {
                _logger.LogWarning("Role name for role Id {RoleId} is invalId or empty.", roleId);
                return OperationResult.FailureResult("InvalId role name.");
            }

            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                _logger.LogInformation("User {UserId} is not in role {RoleName}. No action taken.", userId, roleName);
                return OperationResult.FailureResult("User is not in this role.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("Failed to remove role {RoleName} from user {UserId}. Errors: {Errors}", roleName, userId, errors);
                return OperationResult.FailureResult($"Error happened while removing role: {errors}");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles == null || !userRoles.Any())
            {
                var addMemberResult = await _userManager.AddToRoleAsync(user, "Member");
                if (!addMemberResult.Succeeded)
                {
                    var errors = string.Join(", ", addMemberResult.Errors.Select(e => e.Description));
                    _logger.LogError("Role {RoleName} removed from user {UserId}, but failed to assign 'Member' role. Errors: {Errors}", roleName, userId, errors);
                    return OperationResult.FailureResult($"Role removed but failed to assign 'Member' role: {errors}");
                }

                _logger.LogInformation("Role {RoleName} removed from user {UserId}. Assigned 'Member' role by default.", roleName, userId);
                return OperationResult.SuccessResult("Role removed successfully. 'Member' role assigned as default.");
            }

            _logger.LogInformation("Role {RoleName} successfully removed from user {UserId}.", roleName, userId);
            return OperationResult.SuccessResult("Role removed successfully from user.");
        }

        public async Task<OperationResult> UpdateUserAsync(string userId, UserUpdateDto user)
        {
            var existUser = await _userManager.FindByIdAsync(userId);
            if (existUser == null)
            {
                _logger.LogWarning("UpdateUser failed: User with Id {UserId} not found.", userId);
                return OperationResult.FailureResult("User not found!");
            }

            if (!string.IsNullOrEmpty(user.UserName) && user.UserName != existUser.UserName)
            {
                var userWithSameUsername = await _userManager.FindByNameAsync(user.UserName);

                if (userWithSameUsername != null && userWithSameUsername.Id != userId)
                {
                    _logger.LogWarning("UpdateUser failed: Username '{UserName}' is already taken by another user.", user.UserName);
                    return OperationResult.FailureResult("This username is already taken!");
                }

                _logger.LogInformation("Username for user {UserId} updated from '{OldUserName}' to '{NewUserName}'.", userId, existUser.UserName, user.UserName);
                existUser.UserName = user.UserName;
            }

            if (!string.IsNullOrEmpty(user.Email) && user.Email != existUser.Email)
            {
                var emailWithSameEmail = await _userManager.FindByEmailAsync(user.Email);

                if (emailWithSameEmail != null && emailWithSameEmail.Id != userId)
                {
                    _logger.LogWarning("UpdateUser failed: Email '{Email}' is already taken by another user.", user.Email);
                    return OperationResult.FailureResult("This email is already taken!");
                }

                _logger.LogInformation("Email for user {UserId} updated from '{OldEmail}' to '{NewEmail}'.", userId, existUser.Email, user.Email);
                existUser.Email = user.Email;
            }

            if (!string.IsNullOrEmpty(user.FullName))
            {
                _logger.LogInformation("FullName for user {UserId} updated to '{FullName}'.", userId, user.FullName);
                existUser.FullName = user.FullName;
            }

            if (user.Age.HasValue)
            {
                _logger.LogInformation("Age for user {UserId} updated to {Age}.", userId, user.Age.Value);
                existUser.Age = user.Age.Value;
            }

            var result = await _userManager.UpdateAsync(existUser);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogError("UpdateUser failed: Could not update user {UserId}. Errors: {Errors}", userId, errors);
                return OperationResult.FailureResult($"Update failed: {errors}");
            }

            _logger.LogInformation("User {UserId} successfully updated.", userId);
            return OperationResult.SuccessResult("User successfully updated");

        }
        private string GenerateJwtToken(string userId,string username, List<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username)
            };

            roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwt.ExpireDays));

            var token = new JwtSecurityToken(
                _jwt.Issuer,
                _jwt.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
