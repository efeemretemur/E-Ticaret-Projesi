using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nora.Shop.Core.Entities;
using Nora.Shop.WebAPI.Models;

namespace Nora.Shop.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                Address = request.Address
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors.Select(error => error.Description));
            }

            await _userManager.AddToRoleAsync(user, "User");
            return CreatedAtAction(nameof(Profile), new { email = user.Email }, new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Address
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized("E-posta veya sifre hatali.");
            }

            return Ok("Giris basarili.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }

        [HttpGet("profile/{email}")]
        public async Task<IActionResult> Profile(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return NotFound();
            }

            return Ok(new
            {
                user.Id,
                user.FullName,
                user.Email,
                user.Address
            });
        }
    }
}
