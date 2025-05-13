USE EventEase;
GO

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
    v.VenueId,
    v.VenueName,
    v.Location,
    v.Capacity,
    v.ImageUrl AS VenueImageUrl
FROM 
    Bookings b
JOIN 
    Events e ON b.EventId = e.EventId
JOIN 
    Venues v ON b.VenueId = v.VenueId;
GO