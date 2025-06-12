USE EventEase;
GO

-- 1. Create EventTypes table
CREATE TABLE EventTypes (
    EventTypeId INT IDENTITY(1,1) PRIMARY KEY,
    TypeName NVARCHAR(50) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

-- 2. Add EventTypeId to Events table with ON DELETE NO ACTION
ALTER TABLE Events
ADD EventTypeId INT NULL,
    CONSTRAINT FK_Events_EventTypes FOREIGN KEY (EventTypeId) 
    REFERENCES EventTypes(EventTypeId) ON DELETE SET NULL;
GO

-- 3. Insert predefined event types
INSERT INTO EventTypes (TypeName, Description)
VALUES 
    ('Conference', 'Professional gatherings for knowledge sharing'),
    ('Workshop', 'Hands-on training sessions'),
    ('Seminar', 'Educational presentations'),
    ('Social', 'Networking and social gatherings'),
    ('Exhibition', 'Product or art displays'),
    ('Concert', 'Musical performances');
GO

PRINT 'Database schema updated successfully with:';
PRINT '- EventTypes table with ON DELETE SET NULL constraint';
GO