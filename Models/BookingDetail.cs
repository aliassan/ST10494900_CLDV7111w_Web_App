// Models/BookingDetail.cs
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Models
{
[Keyless] // Important for views
public class BookingDetail
{
    public int BookingId { get; set; }
    public DateTime BookingDate { get; set; }
    
    // Event Info
    public int EventId { get; set; }
    public required string EventName { get; set; }
    public DateTime EventDate { get; set; }
    public DateTime EndDate { get; set; }
    public string? EventDescription { get; set; }
    public string? EventImageUrl { get; set; }
    
    // Venue Info
    public int VenueId { get; set; }
    public required string VenueName { get; set; }
    public required string Location { get; set; }
    public int Capacity { get; set; }
    public string? VenueImageUrl { get; set; }
}
}
