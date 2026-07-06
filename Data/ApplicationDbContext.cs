using Microsoft.EntityFrameworkCore;
using GymAkhada.Models;

namespace GymAkhada.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GymCategory> GymCategories { get; set; }
        public DbSet<GymSubcategory> GymSubcategories { get; set; }
        public DbSet<GymMember> GymMembers { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<FeePayment> FeePayments { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<TournamentRegistration> TournamentRegistrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Defines the relationship
            modelBuilder.Entity<GymCategory>()
                .HasMany(c => c.Subcategories)
                .WithOne(s => s.GymCategory)
                .HasForeignKey(s => s.Gym_ID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GymMember>()
                .HasOne(m => m.GymCategory)
                .WithMany()
                .HasForeignKey(m => m.Gym_ID)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
