using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FitnessAppAPI.Models;
using FitnessAppAPI.Models.DTO;

namespace FitnessAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutsController : ControllerBase
    {
        private readonly WorkoutContext _context;

        public WorkoutsController(WorkoutContext context)
        {
            _context = context;
        }

        // GET: api/Workouts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkoutDTO>>> GetWorkouts()
        {
            return await _context.Workouts
            .Include(w => w.WorkoutItems)
                .ThenInclude(wi => wi.Sets)
            .Select(w => new WorkoutDTO
            {
                Id = w.Id,
                UserId = w.UserId,
                Name = w.Name,
                Date = w.Date,
                Items = w.WorkoutItems.Select(wi => new WorkoutItemDTO
                {
                    Id = wi.Id,
                    WorkoutId = wi.WorkoutId,
                    ExerciseId = wi.ExerciseId,
                    Order = wi.Order,
                    Sets = wi.Sets.Select(ws => new WorkoutSetDTO
                    {
                        Id = ws.Id,
                        WorkoutItemId = ws.WorkoutItemId,
                        Name = ws.Name,
                        SetNumber = ws.SetNumber,
                        Reps = ws.Reps,
                        Weight = ws.Weight,
                    }).ToList()
                }).ToList()
            })
            .ToListAsync();
        }

        // GET: api/Workouts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkoutDTO>> GetWorkout(Guid id)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutItems)
                    .ThenInclude(wi => wi.Sets)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workout == null)
                return NotFound();

            return new WorkoutDTO
            {
                Id = workout.Id,
                Name = workout.Name,
                Date = workout.Date,
                Items = workout.WorkoutItems.Select(wi => new WorkoutItemDTO
                {
                    Id = wi.Id,
                    ExerciseId = wi.ExerciseId,
                    Order = wi.Order,
                    Sets = wi.Sets.Select(ws => new WorkoutSetDTO
                    {
                        Id = ws.Id,
                        Name = ws.Name,
                        SetNumber = ws.SetNumber,
                        Reps = ws.Reps,
                        Weight = ws.Weight
                    }).ToList()
                }).ToList()
            };
        }

        // POST: api/Workouts
        [HttpPost]
        public async Task<ActionResult<WorkoutDTO>> PostWorkout(WorkoutDTO dto)
        {
            var workout = new Workout
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                Name = dto.Name,
                Date = dto.Date,
                WorkoutItems = dto.Items.Select(i => new WorkoutItem
                {
                    Id = Guid.NewGuid(),
                    ExerciseId = i.ExerciseId,
                    Order = i.Order,
                    Sets = i.Sets.Select(s => new WorkoutSet
                    {
                        Id = Guid.NewGuid(),
                        SetNumber = s.SetNumber,
                        Name = s.Name,
                        Reps = s.Reps,
                        Weight = s.Weight
                    }).ToList()
                }).ToList()
            };

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            // optional: reload with includes if you want the full entity back
            return CreatedAtAction(nameof(GetWorkout), new { id = workout.Id }, dto);
        }

        // DELETE: api/Workouts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkout(Guid id)
        {
            var workout = await _context.Workouts
                .Include(w => w.WorkoutItems)
                .ThenInclude(wi => wi.Sets)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (workout == null)
                return NotFound();

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
