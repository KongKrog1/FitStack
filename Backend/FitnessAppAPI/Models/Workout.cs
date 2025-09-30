namespace FitnessAppAPI.Models
{
    public class Workout
    {
        public Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public User? User { get; set; }
        public required string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;


        public ICollection<WorkoutItem> WorkoutItems { get; set; } = new List<WorkoutItem>();
    }
}
