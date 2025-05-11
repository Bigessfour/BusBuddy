-- Development SQL Server initialization script
-- This runs after the main init-db.sql script to add development-specific configuration

PRINT 'Running development SQL Server configuration...'

-- Switch to the BusBuddy database
USE BusBuddy;
GO

-- Enable additional features for development
EXEC sp_configure 'show advanced options', 1;
RECONFIGURE;
EXEC sp_configure 'optimize for ad hoc workloads', 1;
RECONFIGURE;

-- Create a test user for the dashboard
IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = 'TestDashboardUser')
BEGIN
    CREATE LOGIN TestDashboardUser WITH PASSWORD = 'Test@P@ss!2025';
    PRINT 'Login TestDashboardUser created.';
END
ELSE
BEGIN
    PRINT 'Login TestDashboardUser already exists.';
END

-- Create database user for the test user
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'TestDashboardUser')
BEGIN
    CREATE USER TestDashboardUser FOR LOGIN TestDashboardUser;
    ALTER ROLE db_datareader ADD MEMBER TestDashboardUser;
    PRINT 'Database user TestDashboardUser created and added to db_datareader role.';
END
ELSE
BEGIN
    PRINT 'Database user TestDashboardUser already exists.';
END

-- Enable additional database features useful for development
ALTER DATABASE BusBuddy SET AUTO_CREATE_STATISTICS ON;
ALTER DATABASE BusBuddy SET AUTO_UPDATE_STATISTICS ON;

PRINT 'Development SQL Server configuration completed.'
GO
