namespace FitnessAppAPI.Models
{
    public class WorkoutItem
    {
        public Guid Id { get; set; }
        public Guid WorkoutId { get; set; }
        public Workout? Workout { get; set; }
        public Guid ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }
        public int Order { get; set; }

        public List<WorkoutSet> Sets { get; set; } = new List<WorkoutSet>();
    }
}
