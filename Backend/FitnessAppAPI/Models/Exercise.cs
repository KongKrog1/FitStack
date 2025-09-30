namespace FitnessAppAPI.Models
{
    public class Exercise
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Category { get; set; }
        public bool IsCustom { get; set; } = false;


        public ICollection<WorkoutItem> WorkoutItems { get; set; } = new List<WorkoutItem>();
    }
}
