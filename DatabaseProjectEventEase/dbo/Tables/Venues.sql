CREATE TABLE [dbo].[Venues] (
    [VenueId]   INT            IDENTITY (1, 1) NOT NULL,
    [VenueName] NVARCHAR (50)  NOT NULL,
    [Location]  NVARCHAR (100) NOT NULL,
    [Capacity]  INT            NOT NULL,
    [ImageUrl]  NVARCHAR (255) DEFAULT ('https://placehold.co/600?text=Venue+Image') NULL,
    PRIMARY KEY CLUSTERED ([VenueId] ASC)
);


GO

