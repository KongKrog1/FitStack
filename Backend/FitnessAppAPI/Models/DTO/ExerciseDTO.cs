namespace FitnessAppAPI.Models.DTO
{
    public class ExerciseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Category { get; set; }
        public bool IsCustom { get; set; }
    }
}
