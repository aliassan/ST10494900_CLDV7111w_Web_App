using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    // Models/Event.cs
    public class Event
    {
        public int EventId { get; set; }
        
        [Required, StringLength(100)]
        public string? EventName { get; set; }
        
        [Required]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }
        
        public string? Description { get; set; }
        
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = "https://via.placeholder.com/300?text=Event+Image";
        
        // Foreign key relationship
        [Display(Name = "Venue")]
        public int? VenueId { get; set; }
        
        // Navigation property
        public Venue? Venue { get; set; }
    }
}