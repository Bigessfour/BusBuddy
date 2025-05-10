-- SQL initialization script for BusBuddy database
-- This script runs when the container is first created

-- Wait for SQL Server to start
WAITFOR DELAY '00:00:15';

-- Create the BusBuddy database if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'BusBuddy')
BEGIN
    CREATE DATABASE BusBuddy;
    PRINT 'Database BusBuddy created.';
END
ELSE
BEGIN
    PRINT 'Database BusBuddy already exists.';
END

-- Set recovery model to simple for better performance
ALTER DATABASE BusBuddy SET RECOVERY SIMPLE;

-- Switch to the BusBuddy database
USE BusBuddy;

-- Create a login for the application if it doesn't exist
IF NOT EXISTS (SELECT name FROM master.sys.server_principals WHERE name = 'BusBuddyApp')
BEGIN
    CREATE LOGIN BusBuddyApp WITH PASSWORD = 'App@P@ss!2025';
    PRINT 'Login BusBuddyApp created.';
END
ELSE
BEGIN
    PRINT 'Login BusBuddyApp already exists.';
END

-- Create a user for the application if it doesn't exist
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'BusBuddyApp')
BEGIN
    CREATE USER BusBuddyApp FOR LOGIN BusBuddyApp;
    ALTER ROLE db_owner ADD MEMBER BusBuddyApp;
    PRINT 'Database user BusBuddyApp created and added to db_owner role.';
END
ELSE
BEGIN
    PRINT 'Database user BusBuddyApp already exists.';
END

PRINT 'SQL Server initialization complete.';
GO
