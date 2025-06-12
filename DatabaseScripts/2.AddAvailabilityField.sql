USE EventEase;
GO

-- 4. Add Availability field to Venues table
ALTER TABLE Venues
ADD IsAvailable BIT DEFAULT 1 NOT NULL;
GO

-- 5. Update BookingDetails view
CREATE OR ALTER VIEW dbo.BookingDetails AS
SELECT 
    b.BookingId,
    b.BookingDate,
    e.EventId,
    e.EventName,
    e.EventDate,
    e.EndDate,
    e.Description AS EventDescription,
    e.ImageUrl AS EventImageUrl,
    e.EventTypeId,
    et.TypeName AS EventType,
    v.VenueId,
    v.VenueName,
    v.Location,
    v.Capacity,
    v.IsAvailable,
    v.ImageUrl AS VenueImageUrl
FROM 
    Bookings b
JOIN 
    Events e ON b.EventId = e.EventId
LEFT JOIN
    EventTypes et ON e.EventTypeId = et.EventTypeId
JOIN 
    Venues v ON b.VenueId = v.VenueId;
GO

PRINT 'Database schema updated successfully with:';
PRINT '- Venue availability flag';
PRINT '- Updated BookingDetails view';
GO