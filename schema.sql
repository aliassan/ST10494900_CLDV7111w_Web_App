-- Create database
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
    ImageUrl NVARCHAR(255) DEFAULT 'https://via.placeholder.com/300?text=Venue+Image'
);

-- Event table
CREATE TABLE Events (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(100) NOT NULL,
    EventDate DATETIME2 NOT NULL,
    Description NVARCHAR(MAX),
    ImageUrl NVARCHAR(255) DEFAULT 'https://via.placeholder.com/300?text=Event+Image',
    VenueId INT NULL,
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId) ON DELETE SET NULL
);

-- Booking table
CREATE TABLE Bookings (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL UNIQUE,  -- Prevents double bookings
    VenueId INT NOT NULL,
    BookingDate DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (EventId) REFERENCES Events(EventId) ON DELETE CASCADE,
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId)
);

-- Seed data
INSERT INTO Venues (VenueName, Location, Capacity)
VALUES 
    ('Docker Hall', 'Localhost', 500),
    ('Container Ballroom', '127.0.0.1', 300);

INSERT INTO Events (EventName, EventDate, Description, VenueId)
VALUES 
    ('Local Dev Conference', DATEADD(day, 7, GETDATE()), 'Docker-based workshop', 1),
    ('SQL Training', DATEADD(day, 14, GETDATE()), 'Database design', NULL);