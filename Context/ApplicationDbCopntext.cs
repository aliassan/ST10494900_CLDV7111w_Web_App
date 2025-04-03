using Microsoft.EntityFrameworkCore;
using EventEase.Models;

namespace EventEase.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Prevent double bookings
            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.EventId)
                .IsUnique();

            // Seed initial data
            modelBuilder.Entity<Venue>().HasData(
                new Venue { VenueId = 1, VenueName = "Grand Ballroom", Location = "123 Main St", Capacity = 500 },
                new Venue { VenueId = 2, VenueName = "Conference Hall", Location = "456 Oak Ave", Capacity = 200 }
            );

            modelBuilder.Entity<Event>().HasData(
                new Event { EventId = 1, EventName = "Tech Conference", EventDate = DateTime.Now.AddDays(30), Description = "Annual technology summit" },
                new Event { EventId = 2, EventName = "Music Festival", EventDate = DateTime.Now.AddDays(60), Description = "Live bands and performances" }
            );
        }
    }
}