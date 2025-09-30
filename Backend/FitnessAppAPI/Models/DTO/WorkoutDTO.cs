namespace FitnessAppAPI.Models.DTO
{
    public class WorkoutDTO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public required string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public required List<WorkoutItemDTO> Items { get; set; } = new();

    }
}
