CREATE TABLE [dbo].[Events] (
    [EventId]     INT            IDENTITY (1, 1) NOT NULL,
    [EventName]   NVARCHAR (100) NOT NULL,
    [EventDate]   DATETIME2 (7)  NOT NULL,
    [Description] NVARCHAR (MAX) NULL,
    [ImageUrl]    NVARCHAR (255) DEFAULT ('https://placehold.co/600?text=Event+Image') NULL,
    [VenueId]     INT            NULL,
    PRIMARY KEY CLUSTERED ([EventId] ASC),
    FOREIGN KEY ([VenueId]) REFERENCES [dbo].[Venues] ([VenueId]) ON DELETE SET NULL
);


GO

