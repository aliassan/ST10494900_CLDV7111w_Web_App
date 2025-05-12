CREATE DATABASE EventEase;
GO

USE EventEase;
GO

-- Venue table
CREATE TABLE Venues (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName NVARCHAR(50) NOT NULL,
    Location NVARCHAR(100) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl NVARCHAR(255) DEFAULT 'https://placehold.co/300?text=Venue+Image'
);
GO

-- Event table
CREATE TABLE Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(100) NOT NULL,
    EventDate DATETIME2 NOT NULL,
    EndDate DATETIME2 NOT NULL,
    Description NVARCHAR(MAX),
    ImageUrl NVARCHAR(255) DEFAULT 'https://placehold.co/300?text=Event+Image',
    VenueId INT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId) ON DELETE NO ACTION
);
GO

-- Booking table
CREATE TABLE Bookings (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL UNIQUE,
    VenueId INT NOT NULL,
    BookingDate DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (EventId) REFERENCES Events(EventId) ON DELETE NO ACTION,
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId) ON DELETE NO ACTION
);
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

-- Sample data (separate batch)
INSERT INTO Venues (VenueName, Location, Capacity)
VALUES 
    ('Grand Ballroom', 'Johannesburg', 500),
    ('Tech Hub', 'Cape Town', 200);
GO

INSERT INTO Events (EventName, EventDate, EndDate, Description, VenueId)
VALUES 
    ('Tech Conference', 
     '2024-03-15 09:00:00', 
     '2024-03-15 17:00:00',
     'Annual tech event', 1),
     
    ('Workshop', 
     '2024-03-16 10:00:00', 
     '2024-03-16 12:00:00',
     'Hands-on training', NULL);
GO

INSERT INTO Bookings (EventId, VenueId)
VALUES (1, 1);
GO