namespace FitnessAppAPI.Models
{
    public class WorkoutSet
    {
        public Guid Id { get; set; }
        public Guid WorkoutItemId { get; set; }
        public int SetNumber { get; set; }
        public int? Reps { get; set; }
        public float? Weight { get; set; }
        public required string Name { get; set; }

        public WorkoutItem? WorkoutItem { get; set; }
    }
}
