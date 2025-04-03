using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required, StringLength(50)]
        public string? VenueName { get; set; }

        [Required, StringLength(100)]
        public string? Location { get; set; }

        [Required]
        public int Capacity { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; } = "https://via.placeholder.com/300?text=Venue+Image";

        public ICollection<Event>? Events { get; set; }
    }
}