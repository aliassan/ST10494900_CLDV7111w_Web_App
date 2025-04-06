CREATE TABLE [dbo].[Bookings] (
    [BookingId]   INT           IDENTITY (1, 1) NOT NULL,
    [EventId]     INT           NOT NULL,
    [VenueId]     INT           NOT NULL,
    [BookingDate] DATETIME2 (7) DEFAULT (getdate()) NULL,
    PRIMARY KEY CLUSTERED ([BookingId] ASC),
    FOREIGN KEY ([EventId]) REFERENCES [dbo].[Events] ([EventId]) ON DELETE CASCADE,
    FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venues] ([VenueId]),
    UNIQUE NONCLUSTERED ([EventId] ASC)
);


GO

