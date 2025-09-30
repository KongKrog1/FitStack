namespace FitnessAppAPI.Models.DTO
{
    public class WorkoutSetDTO
    {
        public required Guid Id { get; set; }
        public Guid WorkoutItemId { get; set; }
        public required int SetNumber { get; set; }
        public int? Reps { get; set; }
        public float? Weight { get; set; }
        public required string Name { get; set; }
    }
}
