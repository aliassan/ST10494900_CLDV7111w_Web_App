-- Venue and Event Management Database Migration Script

-- Venues table data

SET IDENTITY_INSERT [dbo].[Venues] ON
GO
INSERT [dbo].[Venues] ([VenueId], [VenueName], [Location], [Capacity], [ImageUrl]) VALUES (1, 'Docker Hall', 'Localhost', 500, 'https://placehold.co/600?text=Venue+Image')
GO
INSERT [dbo].[Venues] ([VenueId], [VenueName], [Location], [Capacity], [ImageUrl]) VALUES (2, 'Container Ballroom', '127.0.0.1', 300, 'https://placehold.co/600?text=Venue+Image')
GO
SET IDENTITY_INSERT [dbo].[Venues] OFF
GO

-- Events table data

SET IDENTITY_INSERT [dbo].[Events] ON
GO
INSERT [dbo].[Events] ([EventId], [EventName], [EventDate], [Description], [ImageUrl], [VenueId]) VALUES (1, 'Local Dev Conference', '2025-04-09T06:09:54.2033333', 'Docker-based workshop', 'https://placehold.co/600?text=Event+Image', 1)
GO
INSERT [dbo].[Events] ([EventId], [EventName], [EventDate], [Description], [ImageUrl], [VenueId]) VALUES (2, 'SQL Training', '2025-04-16T06:09:54.2030000', 'Database design', 'https://placehold.co/600?text=Event+Image', 2)
GO
SET IDENTITY_INSERT [dbo].[Events] OFF
GO

-- Bookings table data

SET IDENTITY_INSERT [dbo].[Bookings] ON
GO
INSERT [dbo].[Bookings] ([VenueId], [BookingId], [BookingDate], [EventId]) VALUES (1, 3, '2025-04-21T17:50:00', 1)
GO
SET IDENTITY_INSERT [dbo].[Bookings] OFF
GO


