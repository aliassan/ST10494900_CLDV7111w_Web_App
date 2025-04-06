-- Booking table
CREATE TABLE Bookings (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL UNIQUE,  -- Prevents double bookings
    VenueId INT NOT NULL,
    BookingDate DATETIME2 DEFAULT GETDATE(),
    FOREIGN KEY (EventId) REFERENCES Events(EventId) ON DELETE CASCADE,
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId)
);