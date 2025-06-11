using System.ComponentModel.DataAnnotations;

namespace EventEase.Models
{
    public class EventType
    {
        public int EventTypeId { get; set; }

        // [Required]
        [StringLength(50)]
        public string? TypeName { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        // Navigation property
        // public ICollection<Event>? Events { get; set; }
    }
}