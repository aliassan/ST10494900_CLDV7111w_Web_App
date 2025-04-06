-- Venue table
CREATE TABLE Venues (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName NVARCHAR(50) NOT NULL,
    Location NVARCHAR(100) NOT NULL,
    Capacity INT NOT NULL,
    ImageUrl NVARCHAR(255) DEFAULT 'https://via.placeholder.com/300?text=Venue+Image'
);

-- GO

-- -- Seed data
-- INSERT INTO Venues (VenueName, Location, Capacity)
-- VALUES 
--     ('Docker Hall', 'Localhost', 500),
--     ('Container Ballroom', '127.0.0.1', 300);