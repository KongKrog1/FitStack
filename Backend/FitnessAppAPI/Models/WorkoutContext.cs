using Microsoft.EntityFrameworkCore;

namespace FitnessAppAPI.Models
{
    public class WorkoutContext : DbContext
    {
        public WorkoutContext(DbContextOptions<WorkoutContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Workout> Workouts { get; set; } = null!;
        public DbSet<WorkoutItem> WorkoutItems { get; set; } = null!;
        public DbSet<WorkoutSet> WorkoutSets { get; set; } = null!;
        public DbSet<Exercise> Exercises { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User → Workout. One to many
            modelBuilder.Entity<User>()
                .HasMany(u => u.Workouts)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Workout → WorkoutItem. One to many
            modelBuilder.Entity<Workout>()
                .HasMany(w => w.WorkoutItems)
                .WithOne(wi => wi.Workout)
                .HasForeignKey(wi => wi.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            // Exercise → WorkoutItem. One to many
            modelBuilder.Entity<Exercise>()
                .HasMany(e => e.WorkoutItems)
                .WithOne(wi => wi.Exercise)
                .HasForeignKey(wi => wi.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            // WorkoutItem → WorkoutSet. One to many
            modelBuilder.Entity<WorkoutItem>()
                .HasMany(wi => wi.Sets)
                .WithOne(ws => ws.WorkoutItem)
                .HasForeignKey(ws => ws.WorkoutItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }
    }
}
