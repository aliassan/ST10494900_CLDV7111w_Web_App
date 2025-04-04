using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        [Display(Name = "Booking Date")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        // Foreign Keys
        [Required]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        [Required]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        // Navigation Properties
        public Event? Event { get; set; }
        public Venue? Venue { get; set; }
    }
}