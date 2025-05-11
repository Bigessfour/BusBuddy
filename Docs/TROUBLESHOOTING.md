# BusBuddy Troubleshooting Guide

This document provides solutions for common issues you might encounter while using BusBuddy.

## Database Connection Issues

### Cannot connect to SQL Server
- **Problem**: Application fails to start with "Cannot connect to SQL Server" or "A network-related or instance-specific error occurred".
- **Solution**:
  1. Verify SQL Server service is running:
     ```powershell
     Get-Service -Name "MSSQL$*" | Where-Object {$_.Status -ne "Running"} | Start-Service
     ```
  2. Check instance name in appsettings.json (should be "localhost\\SQLEXPRESS" or "localhost\\SQLEXPRESS01")
  3. Try connecting via SQL Server Management Studio to confirm server availability

### Login failed for user
- **Problem**: Application fails with "Login failed for user" error.
- **Solution**:
  1. Ensure Windows Authentication is enabled on SQL Server
  2. In SQL Server Management Studio, run:
     ```sql
     ALTER SERVER ROLE sysadmin ADD MEMBER [YourWindowsUser];
     ```
  3. Restart the application

### Database does not exist or tables are missing
- **Problem**: Application reports missing tables or database does not exist.
- **Solution**:
  1. Recreate the database:
     ```powershell
     cd c:\Users\steve.mckitrick\Desktop\BusBuddy\BusBuddy
     dotnet ef database drop -f
     dotnet ef database update
     ```

## Form Display Issues

### License expiration alerts not showing
- **Problem**: License expiration alerts are not displayed.
- **Solution**:
  1. Verify driver data has valid license expiration dates in database
  2. Check if license expiration dates are within 30 days of current date
  3. Look for any errors in logs/busbuddy*.log files

### DataGridView not showing data
- **Problem**: Forms load but data grids are empty.
- **Solution**:
  1. Check database connection (see above)
  2. Verify data exists in the relevant tables
  3. Try using the "Refresh" button on the form
  4. Restart the application

### Material theme rendering issues
- **Problem**: UI looks wrong or is slow to update.
- **Solution**:
  1. Ensure your system meets minimum requirements (Windows 10/11, 8GB RAM)
  2. Update graphics drivers
  3. Reduce animations in Windows display settings

## Application Performance

### Slow database operations
- **Problem**: Forms take a long time to load or operations are sluggish.
- **Solution**:
  1. Check SQL Server resource usage
  2. Verify indices exist for frequently used queries:
     ```sql
     SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('RouteData');
     SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('Drivers');
     ```
  3. If indices are missing, run the latest migration:
     ```powershell
     dotnet ef database update
     ```

### Log file size growing too large
- **Problem**: Log files are consuming too much disk space.
- **Solution**:
  1. The application automatically rotates logs daily and keeps only 7 days
  2. To manually clean old logs:
     ```powershell
     Get-ChildItem -Path logs\busbuddy* | Where-Object {$_.LastWriteTime -lt (Get-Date).AddDays(-7)} | Remove-Item
     ```

## Data Backup and Recovery

### Creating a database backup
```powershell
$backupPath = "C:\Backups\BusBuddy_$(Get-Date -Format 'yyyyMMdd').bak"
if (-not (Test-Path "C:\Backups")) { New-Item -Path "C:\Backups" -ItemType Directory }
$query = "BACKUP DATABASE BusBuddy TO DISK='$backupPath'"
Invoke-SqlCmd -ServerInstance "localhost\SQLEXPRESS" -Query $query
```

### Restoring from backup
```powershell
$backupPath = "C:\Backups\BusBuddy_20250510.bak"  # Update with your backup file
$query = @"
USE master;
ALTER DATABASE BusBuddy SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
RESTORE DATABASE BusBuddy FROM DISK='$backupPath' WITH REPLACE;
ALTER DATABASE BusBuddy SET MULTI_USER;
"@
Invoke-SqlCmd -ServerInstance "localhost\SQLEXPRESS" -Query $query
```

## Docker-Related Issues

### Docker container won't start
- **Problem**: Docker container fails to start or is immediately stopping.
- **Solution**:
  1. Check Docker logs for detailed error messages:
     ```powershell
     docker-compose logs sqlserver
     ```
  2. Verify Docker Desktop is running and has enough resources allocated
  3. Check if port 1433 is already in use by another service:
     ```powershell
     netstat -ano | findstr :1433
     ```
  4. Try restarting Docker Desktop

### Cannot connect to Docker database
- **Problem**: Application fails to connect to Docker database with "Connection refused" or timeout errors.
- **Solution**:
  1. Verify the container is running:
     ```powershell
     docker ps | Select-String "busbuddy-sqlserver"
     ```
  2. Check container health:
     ```powershell
     docker inspect --format "{{.State.Health.Status}}" busbuddy-sqlserver
     ```
  3. Try connecting directly using SQL Server Management Studio with these settings:
     - Server: `localhost,1433`
     - Authentication: SQL Server Authentication
     - User: `sa`
     - Password: `BusB#ddy!2025`
  4. Ensure firewall is not blocking port 1433

### Database initialization failed in Docker
- **Problem**: Docker container starts but database is not properly initialized.
- **Solution**:
  1. Run the initialization script manually:
     ```powershell
     docker exec -it busbuddy-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "BusB#ddy!2025" -i /docker-entrypoint-initdb.d/init-db.sql
     ```
  2. Check for script errors in the logs:
     ```powershell
     docker-compose logs sqlserver
     ```

### Docker volume data persistence issues
- **Problem**: Database changes are lost when restarting Docker container.
- **Solution**:
  1. Verify the volume is correctly mounted:
     ```powershell
     docker volume ls | findstr busbuddy
     ```
  2. Check volume data:
     ```powershell
     docker volume inspect busbuddy-sqlserver-data
     ```
  3. Recreate the container without removing volumes:
     ```powershell
     docker-compose down
     docker-compose up -d
     ```
