using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required, StringLength(100)]
        public string? EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        public string? Description { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; } = "https://via.placeholder.com/300?text=Event+Image";

        public int? VenueId { get; set; }  // Nullable for pre-booking
        public Venue? Venue { get; set; }

        public Booking? Booking { get; set; }  // 1:1 relationship
    }
}