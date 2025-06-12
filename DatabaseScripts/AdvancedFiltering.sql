USE EventEase;
GO

-- 1. Create EventTypes table
CREATE TABLE EventTypes (
    EventTypeId INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

-- 2. Add EventTypeId to Events table with ON DELETE NO ACTION
ALTER TABLE Events
ADD EventTypeId INT NULL,
    CONSTRAINT FK_Events_EventTypes FOREIGN KEY (EventTypeId) 
    REFERENCES EventTypes(EventTypeId) ON DELETE SET NULL;
GO

-- 3. Add Availability field to Venues table
ALTER TABLE Venues
ADD IsAvailable BIT DEFAULT 1 NOT NULL;
GO

-- 4. Insert predefined event types
INSERT INTO EventTypes (TypeName, Description)
VALUES 
    ('Conference', 'Professional gatherings for knowledge sharing'),
    ('Workshop', 'Hands-on training sessions'),
    ('Seminar', 'Educational presentations'),
    ('Social', 'Networking and social gatherings'),
    ('Exhibition', 'Product or art displays'),
    ('Concert', 'Musical performances');
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
PRINT '- EventTypes table with ON DELETE SET NULL constraint';
PRINT '- Venue availability flag';
PRINT '- Updated BookingDetails view';
GO