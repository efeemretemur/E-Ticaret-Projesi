using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NoraShop.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private static List<LocalUser> _users = new List<LocalUser>();

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterDto model)
        {
            if (_users.Any(u => u.Email.ToLower() == model.Email.ToLower()))
            {
                return BadRequest("Bu e-posta adresi zaten kayıtlı!");
            }

            var newUser = new LocalUser
            {
                Email = model.Email,
                Password = model.Password
            };

            _users.Add(newUser);
            return Ok(new { message = "Kullanıcı başarıyla kayıt edildi!" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto model)
        {
            var user = _users.FirstOrDefault(u => u.Email.ToLower() == model.Email.ToLower() && u.Password == model.Password);
            if (user == null)
            {
                return BadRequest("Kullanıcı bulunamadı veya şifre hatalı!");
            }

            var jwtSettings = _configuration.GetSection("Jwt");
            var keyString = jwtSettings["Key"] ?? "NoraShopSuperSecretKeyBurayaEnAz16Karakter";

            var key = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"] ?? "NoraShop",
                audience: jwtSettings["Audience"] ?? "NoraShopUser",
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                message = "Giriş başarılı!"
            });
        }
    }

    public class LocalUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}