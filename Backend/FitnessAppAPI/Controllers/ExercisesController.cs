using FitnessAppAPI.Models;
using FitnessAppAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class ExercisesController : ControllerBase
    {
        private readonly WorkoutContext _context;

        public ExercisesController(WorkoutContext context)
        {
            _context = context;
        }

        // GET /api/exercises
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExerciseDTO>>> GetExercises()
        {
            return await _context.Exercises
                .Select(e => new ExerciseDTO
                {
                    Id = e.Id,
                    Name = e.Name,
                    Category = e.Category,
                    IsCustom = e.IsCustom
                })
                .ToListAsync();
        }

        // POST /api/exercises
        [HttpPost]
        public async Task<ActionResult<ExerciseDTO>> PostExercise(ExerciseDTO dto)
        {
            var exercise = new Exercise
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Category = dto.Category,
                IsCustom = dto.IsCustom
            };

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            dto.Id = exercise.Id;
            return CreatedAtAction(nameof(GetExercises), new { id = exercise.Id }, dto);
        }
    }
}