using Microsoft.EntityFrameworkCore;
using ThreadsLite.API.Models;

namespace ThreadsLite.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        // Constructor that accepts options to configure the context.
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // Define the DbSets for the tables in the database
        public DbSet<User> Users { get; set; }
        public DbSet<Follow> Follows { get; set; }

        // Configure the relationships between tables
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Follow relationship for followers and following
            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Follower)  // Each Follow has one Follower
                .WithMany(u => u.Following)  // User can have many followers
                .HasForeignKey(f => f.FollowerId)  // The foreign key in Follow table
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes

            modelBuilder.Entity<Follow>()
                .HasOne(f => f.Following)  // Each Follow has one Following user
                .WithMany(u => u.Followers)  // User can have many followed users
                .HasForeignKey(f => f.FollowingId)  // The foreign key in Follow table
                .OnDelete(DeleteBehavior.Restrict);  // Prevent cascading deletes
        }
    }
}
