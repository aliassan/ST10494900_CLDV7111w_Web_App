using EventEase.Context;
using Microsoft.EntityFrameworkCore;

public interface IVenueAvailabilityService
{
    Task<bool> IsVenueAvailableAsync(int venueId, DateTime start, DateTime end);
}

public class VenueAvailabilityService : IVenueAvailabilityService
{
    private readonly ApplicationDbContext _db;

    public VenueAvailabilityService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> IsVenueAvailableAsync(int venueId, DateTime start, DateTime end)
    {
        return !await _db.VenueAvailability
            .FromSqlInterpolated($@"
                SELECT * FROM VenueAvailability WITH (NOEXPAND)
                WHERE VenueId = {venueId}
                AND StartTime < {end}
                AND EndTime > {start}")
            .AnyAsync();
    }
}