using Microsoft.AspNetCore.Mvc;
using FitnessAppAPI.Models;
using FitnessAppAPI.Models.DTO;

namespace FitnessAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly WorkoutContext _context;

        public UsersController(WorkoutContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(UserDTO UserDTO)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = UserDTO.Username,
                Email = UserDTO.Email,
                PasswordHash = UserDTO.PasswordHash,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(PostUser), new { id = user.Id }, user);
        }
    }
}
