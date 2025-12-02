using HotelAPI.Data;
using HotelAPI.Helpers;
using HotelAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly HotelDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(HotelDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
                return BadRequest("Email već postoji.");

            // Hash-uj plaintext lozinku koja dolazi u PasswordHash
            user.PasswordHash = PasswordHelper.CreatePasswordHash(user.PasswordHash);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Korisnik registrovan" });
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginData)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email);
            if (user == null)
                return Unauthorized("Pogrešan email ili lozinka.");

            if (!PasswordHelper.VerifyPassword(loginData.PasswordHash, user.PasswordHash))
                return Unauthorized("Pogrešan email ili lozinka.");

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }



        private string GenerateJwtToken(User user)
        {
            var jwtKey = _config["Jwt:Key"] ?? "OvoJeMojTajniJWTKey1234567890123456";
            var jwtIssuer = _config["Jwt:Issuer"] ?? "HotelAPI";

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: null,
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
