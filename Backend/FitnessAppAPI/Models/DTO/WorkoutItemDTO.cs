namespace FitnessAppAPI.Models.DTO
{
    public class WorkoutItemDTO
    {
        public Guid Id { get; set; }
        public Guid ExerciseId { get; set; }
        public Guid WorkoutId { get; set; }
        public int Order { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public ICollection<WorkoutSetDTO> Sets { get; set; } = new List<WorkoutSetDTO>();
    }
}
