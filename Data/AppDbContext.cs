using Microsoft.EntityFrameworkCore;
using VoteHub.Models;

namespace VoteHub.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Voter> Voters { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<ElectionResult> ElectionResults { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Voter>()
                .HasIndex(v => v.EpicNumber)
                .IsUnique();

            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Email)
                .IsUnique();

            // MySQL: limit string length for unique index columns
            modelBuilder.Entity<Voter>()
                .Property(v => v.EpicNumber)
                .HasMaxLength(50);

            modelBuilder.Entity<Admin>()
                .Property(a => a.Email)
                .HasMaxLength(100);

            base.OnModelCreating(modelBuilder);
        }
    }
}
