using FitnessAppAPI.Models;
using FitnessAppAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FitnessAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WorkoutContext _context;

        private readonly string _jwtKey;

        public UsersController(WorkoutContext context, IConfiguration config)
        {
            _context = context;
            _jwtKey = config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not set in configuration.");
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO userDTO)
        {
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userDTO.Username,
                Email = userDTO.Email,
                PasswordHash = "",
                CreatedAt = DateTime.UtcNow
            };

            // Hash plain password from the DTO
            user.PasswordHash = hasher.HashPassword(user, userDTO.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);

            if (user == null)
                return Unauthorized("User not found");

            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);

            if (result == PasswordVerificationResult.Success)
            {
                user.LastLogin = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                var token = GenerateJwtToken(user);
                Console.WriteLine("Login success!");
                return Ok(new { token });
            }
                 

            return Unauthorized("Invalid password");

        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                issuer: "FitnessAppAPI",
                audience: "FitnessAppFrontend",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
