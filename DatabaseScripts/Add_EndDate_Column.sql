-- File: Add_EndDate_Column.sql
-- Purpose: Adds EndDate column to Events table with default value
USE EventEase;
GO

-- First add the column as nullable
ALTER TABLE Events
ADD EndDate DATETIME2 NULL;
GO

-- Update existing records first
UPDATE Events 
SET EndDate = DATEADD(HOUR, 2, EventDate);
GO

-- Now alter the column to be NOT NULL with a default constraint
ALTER TABLE Events
ALTER COLUMN EndDate DATETIME2 NOT NULL;
GO

-- Add default constraint separately
ALTER TABLE Events
ADD CONSTRAINT DF_Events_EndDate 
DEFAULT DATEADD(HOUR, 2, GETDATE()) FOR EndDate;
GO

-- Verify the column was added
SELECT EventId, EventName, EventDate, EndDate 
FROM Events;
GO

PRINT 'Successfully added EndDate column to Events table';