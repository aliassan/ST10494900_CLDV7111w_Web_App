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

-- GO 

-- INSERT INTO Events (EventName, EventDate, Description, VenueId)
-- VALUES 
--     ('Local Dev Conference', DATEADD(day, 7, GETDATE()), 'Docker-based workshop', 1),
--     ('SQL Training', DATEADD(day, 14, GETDATE()), 'Database design', NULL);