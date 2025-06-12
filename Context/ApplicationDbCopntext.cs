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
        public DbSet<EventType> EventTypes { get; set; } // New DbSet
        public virtual DbSet<VenueAvailability> VenueAvailability { get; set; }
        public virtual DbSet<BookingDetail> BookingDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // // Configure EventType relationship
            // // modelBuilder.Entity<Event>()
            // //     .HasOne(e => e.EventType)
            // //     .WithMany()
            // //     .HasForeignKey(e => e.EventTypeId)
            // //     .OnDelete(DeleteBehavior.SetNull);

            // // Prevent double bookings
            // modelBuilder.Entity<Booking>()
            //     .HasIndex(b => b.EventId)
            //     .IsUnique();

            // modelBuilder.Entity<BookingDetail>(entity =>
            // {
            //     entity.ToView("BookingDetails");
            //     entity.HasNoKey();

            //     // Explicit column mappings
            //     // entity.Property(e => e.BookingEventTypeId).HasColumnName("EventTypeId");
            //     // entity.Property(e => e.EventTypeName).HasColumnName("EventType");
            // });

            // // Seed initial data
            // modelBuilder.Entity<Venue>().HasData(
            //     new Venue { VenueId = 1, VenueName = "Grand Ballroom", Location = "123 Main St", Capacity = 500 },
            //     new Venue { VenueId = 2, VenueName = "Conference Hall", Location = "456 Oak Ave", Capacity = 200 }
            // );

            // modelBuilder.Entity<Event>().HasData(
            //     new Event { EventId = 1, EventName = "Tech Conference", EventDate = DateTime.Now.AddDays(30), Description = "Annual technology summit" },
            //     new Event { EventId = 2, EventName = "Music Festival", EventDate = DateTime.Now.AddDays(60), Description = "Live bands and performances" }
            // );
            
            // Configure EventType relationships
            modelBuilder.Entity<EventType>(entity =>
            {
                entity.HasKey(e => e.EventTypeId);
                entity.Property(e => e.TypeName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(255);
            });

            // Configure Event relationships
            // modelBuilder.Entity<Event>(entity =>
            // {
            //     entity.HasOne(e => e.EventType)
            //           .WithMany()
            //           .HasForeignKey(e => e.EventTypeId)
            //           .OnDelete(DeleteBehavior.NoAction);
            // });

            // Prevent double bookings
            modelBuilder.Entity<Booking>()
                .HasIndex(b => b.EventId)
                .IsUnique();

            // Configure view mappings
            modelBuilder.Entity<BookingDetail>(entity => 
            {
                entity.ToView("BookingDetails");
                entity.HasNoKey();
            });

            modelBuilder.Entity<VenueAvailability>(entity =>
            {
                entity.ToView("VenueAvailability");
                entity.HasNoKey();
            });

            // Seed initial data
            modelBuilder.Entity<Venue>().HasData(
                new Venue { 
                    VenueId = 1, 
                    VenueName = "Grand Ballroom", 
                    Location = "Johannesburg", 
                    Capacity = 500,
                    IsAvailable = true 
                },
                new Venue { 
                    VenueId = 2, 
                    VenueName = "Tech Hub", 
                    Location = "Cape Town", 
                    Capacity = 200,
                    IsAvailable = true 
                }
            );

            modelBuilder.Entity<EventType>().HasData(
                new EventType { EventTypeId = 1, TypeName = "Conference", Description = "Professional gatherings" },
                new EventType { EventTypeId = 2, TypeName = "Workshop", Description = "Hands-on training" },
                new EventType { EventTypeId = 3, TypeName = "Seminar", Description = "Educational presentations" },
                new EventType { EventTypeId = 4, TypeName = "Social", Description = "Networking events" },
                new EventType { EventTypeId = 5, TypeName = "Exhibition", Description = "Product displays" },
                new EventType { EventTypeId = 6, TypeName = "Concert", Description = "Musical performances" }
            );

            modelBuilder.Entity<Event>().HasData(
                new Event { 
                    EventId = 1, 
                    EventName = "Tech Conference", 
                    EventDate = DateTime.Now.AddDays(30),
                    EndDate = DateTime.Now.AddDays(30).AddHours(8),
                    Description = "Annual technology summit",
                    VenueId = 1,
                    EventTypeId = 1
                },
                new Event { 
                    EventId = 2, 
                    EventName = "Music Festival", 
                    EventDate = DateTime.Now.AddDays(60),
                    EndDate = DateTime.Now.AddDays(60).AddHours(6),
                    Description = "Live bands and performances",
                    VenueId = 2,
                    EventTypeId = 6
                }
            );
        }
    }

    [Keyless]
    public class VenueAvailability
    {
        public int VenueId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}