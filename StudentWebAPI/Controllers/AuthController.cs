using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentWebAPI.Auth.DTOs;
using StudentWebAPI.Identity;
using StudentWebAPI.Service;

namespace StudentWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenService _tokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var roleExists =
                await _roleManager.RoleExistsAsync(dto.Role);

            if (!roleExists)
            {
                return BadRequest(
                    $"Role '{dto.Role}' does not exist.");
            }

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var createResult =
                await _userManager.CreateAsync(
                    user,
                    dto.Password);

            if (!createResult.Succeeded)
            {
                return BadRequest(createResult.Errors);
            }

            var roleResult =
                await _userManager.AddToRoleAsync(
                    user,
                    dto.Role);

            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            return Ok(new
            {
                Message = "User Registered Successfully"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user =
                await _userManager.FindByEmailAsync(
                    dto.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid Email or Password"
                });
            }

            var isPasswordValid =
                await _userManager.CheckPasswordAsync(
                    user,
                    dto.Password);

            if (!isPasswordValid)
            {
                return Unauthorized(new
                {
                    Message = "Invalid Email or Password"
                });
            }

            var roles =
                await _userManager.GetRolesAsync(user);

            var token =
                _tokenService.CreateToken(
                    user,
                    roles);

            return Ok(new
            {
                Token = token,
                UserName = user.FullName,
                user.Email,
                Roles = roles
            });
        }
    }
}