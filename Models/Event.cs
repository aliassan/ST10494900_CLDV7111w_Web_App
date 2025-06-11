using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    public class Event
    {
        public int EventId { get; set; }

        [Required, StringLength(100)]
        public string? EventName { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public DateTime EventDate { get; set; }  // Renamed for clarity

        [Required]
        [Display(Name = "End Time")]
        public DateTime EndDate { get; set; }     // New property for duration

        public string? Description { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = "https://placehold.co/300?text=Event+Image";

        [Display(Name = "Venue")]
        public int? VenueId { get; set; }

        public Venue? Venue { get; set; }

        // Add a property for EventType
        [Display(Name = "Event Type")]
        public int? EventTypeId { get; set; }
        
        // [ForeignKey("EventTypeId")]
        public EventType? EventType { get; set; }
    }
}