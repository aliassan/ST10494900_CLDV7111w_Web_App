-- File: Update_ForeignKey_Constraints.sql
-- Purpose: Updates foreign key constraints to use ON DELETE NO ACTION
USE EventEase;
GO

BEGIN TRY
    BEGIN TRANSACTION;
    
    -- Drop existing constraints
    DECLARE @sql NVARCHAR(MAX) = '';
    
    -- Find and drop the EventId foreign key constraint
    SELECT @sql = 'ALTER TABLE Bookings DROP CONSTRAINT ' + name + ';'
    FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID('Bookings') 
    AND referenced_object_id = OBJECT_ID('Events');
    
    EXEC sp_executesql @sql;
    
    -- Find and drop the VenueId foreign key constraint
    SET @sql = '';
    SELECT @sql = 'ALTER TABLE Bookings DROP CONSTRAINT ' + name + ';'
    FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID('Bookings') 
    AND referenced_object_id = OBJECT_ID('Venues');
    
    EXEC sp_executesql @sql;
    
    -- Recreate constraints with ON DELETE NO ACTION
    ALTER TABLE Bookings
    ADD CONSTRAINT FK_Bookings_Events
    FOREIGN KEY (EventId) REFERENCES Events(EventId) ON DELETE NO ACTION;
    
    ALTER TABLE Bookings
    ADD CONSTRAINT FK_Bookings_Venues
    FOREIGN KEY (VenueId) REFERENCES Venues(VenueId) ON DELETE NO ACTION;
    
    -- Verify the changes
    SELECT 
        fk.name AS ConstraintName,
        OBJECT_NAME(fk.parent_object_id) AS ChildTable,
        OBJECT_NAME(fk.referenced_object_id) AS ParentTable,
        delete_referential_action_desc AS OnDeleteAction
    FROM sys.foreign_keys fk
    WHERE fk.parent_object_id IN (OBJECT_ID('Bookings'), OBJECT_ID('Events'));
    
    COMMIT TRANSACTION;
    PRINT 'Successfully updated foreign key constraints';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    
    PRINT 'Error updating constraints: ' + ERROR_MESSAGE();
    THROW;
END CATCH