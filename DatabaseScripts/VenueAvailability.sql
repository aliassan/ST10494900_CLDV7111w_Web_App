-- View for checking venue availability to prevent double bookings
USE EventEase;
GO

-- Materialized view for fast availability checks (indexed)
CREATE VIEW dbo.VenueAvailability WITH SCHEMABINDING AS
SELECT 
    b.VenueId,
    e.EventDate AS StartTime,
    e.EndDate AS EndTime,
    COUNT_BIG(*) AS Count
FROM dbo.Bookings b
JOIN dbo.Events e ON b.EventId = e.EventId
GROUP BY b.VenueId, e.EventDate, e.EndDate;
GO

-- Index must be in a separate batch
CREATE UNIQUE CLUSTERED INDEX IX_VenueAvailability 
ON dbo.VenueAvailability (VenueId, StartTime, EndTime);
GO