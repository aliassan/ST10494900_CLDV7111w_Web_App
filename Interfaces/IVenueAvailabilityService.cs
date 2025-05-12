namespace EventEase.Services;
public interface IVenueAvailabilityService
{
    Task<bool> IsVenueAvailableAsync(int venueId, DateTime start, DateTime end);
}