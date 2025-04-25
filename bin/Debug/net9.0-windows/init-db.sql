-- Initialize the database schema for BusBuddy with proper field names and formats
-- Creates initial schema with standardized naming and data types
-- Use 'YYYY-MM-DD' for TEXT date fields and camelCase for column names

-- SchemaVersion Table (For migration tracking)
CREATE TABLE IF NOT EXISTS SchemaVersion (
    RowId INTEGER PRIMARY KEY,
    Version INTEGER NOT NULL DEFAULT 0
);

-- Trips Table
CREATE TABLE IF NOT EXISTS Trips (
    TripID INTEGER PRIMARY KEY AUTOINCREMENT,
    TripType TEXT NOT NULL,
    TripDate TEXT NOT NULL,  -- 'YYYY-MM-DD'
    BusNumber INTEGER NOT NULL,
    DriverID INTEGER NOT NULL,
    StartTime TEXT,
    EndTime TEXT,
    TotalHoursDriven REAL,
    Destination TEXT,
    AMBeginMileage INTEGER,
    AMEndMileage INTEGER,
    NumRiders INTEGER,
    PMStartMileage INTEGER,
    PMEndingMileage INTEGER,
    NumPMRiders INTEGER,
    FOREIGN KEY (BusNumber) REFERENCES Vehicles (BusNumber),
    FOREIGN KEY (DriverID) REFERENCES Drivers (DriverID)
);

-- Drivers Table
CREATE TABLE IF NOT EXISTS Drivers (
    DriverID INTEGER PRIMARY KEY AUTOINCREMENT,
    DriverName TEXT NOT NULL,
    Address TEXT,
    City TEXT,
    State TEXT,
    ZipCode TEXT,
    PhoneNumber TEXT,
    EmailAddress TEXT,
    IsStipendPaid BOOLEAN,
    DLType TEXT
);

-- Vehicles Table
CREATE TABLE IF NOT EXISTS Vehicles (
    VehicleID INTEGER PRIMARY KEY AUTOINCREMENT,
    BusNumber INTEGER UNIQUE NOT NULL,
    Make TEXT,
    Model TEXT,
    ModelYear INTEGER,
    VIN TEXT UNIQUE,
    PlateNumber TEXT,
    SeatingCapacity INTEGER,
    IsOperational INTEGER DEFAULT 1,
    PurchaseDate TEXT,  -- 'YYYY-MM-DD'
    LastInspectionDate TEXT,  -- 'YYYY-MM-DD'
    CurrentOdometer INTEGER,
    PurchasePrice REAL,
    AnnualInspection TEXT
);

-- Fuel Table
CREATE TABLE IF NOT EXISTS Fuel (
    FuelID INTEGER PRIMARY KEY AUTOINCREMENT,
    BusNumber INTEGER NOT NULL,
    FuelGallons REAL NOT NULL,
    FuelDate TEXT NOT NULL,  -- 'YYYY-MM-DD'
    FuelType TEXT,
    OdometerReading INTEGER,
    Notes TEXT,
    FOREIGN KEY (BusNumber) REFERENCES Vehicles (BusNumber)
);

-- Activities Table
CREATE TABLE IF NOT EXISTS Activities (
    ActivityID INTEGER PRIMARY KEY AUTOINCREMENT,
    ActivityDate TEXT NOT NULL,  -- 'YYYY-MM-DD'
    BusNumber INTEGER NOT NULL,
    Destination TEXT NOT NULL,
    LeaveTime TEXT NOT NULL,
    DriverID INTEGER NOT NULL,
    HoursDriven REAL,
    StudentsDriven INTEGER,
    FOREIGN KEY (BusNumber) REFERENCES Vehicles (BusNumber),
    FOREIGN KEY (DriverID) REFERENCES Drivers (DriverID)
);

-- Routes Table
CREATE TABLE IF NOT EXISTS Routes (
    RouteID INTEGER PRIMARY KEY AUTOINCREMENT,
    RouteName TEXT NOT NULL,
    DefaultBusNumber INTEGER,
    DefaultDriverID INTEGER,
    Description TEXT,
    StartTime TEXT NOT NULL,
    EndTime TEXT NOT NULL,
    FOREIGN KEY (DefaultBusNumber) REFERENCES Vehicles (BusNumber),
    FOREIGN KEY (DefaultDriverID) REFERENCES Drivers (DriverID)
);

-- SchoolCalendarDays Table
CREATE TABLE IF NOT EXISTS SchoolCalendarDays (
    CalendarDayID INTEGER PRIMARY KEY AUTOINCREMENT,
    CalendarDate TEXT NOT NULL UNIQUE,  -- 'YYYY-MM-DD'
    IsSchoolDay BOOLEAN NOT NULL DEFAULT 1,
    DayType TEXT NOT NULL DEFAULT 'Regular',
    Notes TEXT
);

-- ScheduledRoutes Table
CREATE TABLE IF NOT EXISTS ScheduledRoutes (
    ScheduledRouteID INTEGER PRIMARY KEY AUTOINCREMENT,
    CalendarDayID INTEGER NOT NULL,
    RouteID INTEGER NOT NULL,
    AssignedBusNumber INTEGER NOT NULL,
    AssignedDriverID INTEGER NOT NULL,
    FOREIGN KEY (CalendarDayID) REFERENCES SchoolCalendarDays (CalendarDayID),
    FOREIGN KEY (RouteID) REFERENCES Routes (RouteID),
    FOREIGN KEY (AssignedBusNumber) REFERENCES Vehicles (BusNumber),
    FOREIGN KEY (AssignedDriverID) REFERENCES Drivers (DriverID)
);

-- Maintenance Table
CREATE TABLE IF NOT EXISTS Maintenance (
    MaintenanceID INTEGER PRIMARY KEY AUTOINCREMENT,
    BusNumber INTEGER NOT NULL,
    DatePerformed TEXT NOT NULL,  -- 'YYYY-MM-DD'
    Description TEXT NOT NULL,
    Cost REAL,
    OdometerReading INTEGER,
    FOREIGN KEY (BusNumber) REFERENCES Vehicles (BusNumber)
);

-- Insert initial SchemaVersion
INSERT OR IGNORE INTO SchemaVersion (RowId, Version) VALUES (1, 0);

-- Insert default vehicle
INSERT OR IGNORE INTO Vehicles (BusNumber, Make, Model, ModelYear, SeatingCapacity)
VALUES (101, 'Bluebird', 'School Bus', 2008, 71);

-- Insert default routes
INSERT OR IGNORE INTO Routes (RouteName, DefaultBusNumber, DefaultDriverID, Description, StartTime, EndTime)
VALUES 
    ('Truck Plaza Route', 101, NULL, 'Route to the Truck Plaza area', '07:00', '08:00'),
    ('East Route', 101, NULL, 'Eastern district route', '07:00', '08:00'),
    ('West Route', 101, NULL, 'Western district route', '07:00', '08:00'),
    ('SPED Route', 101, NULL, 'Special Education route', '07:00', '08:00');

-- Add indexes for performance
CREATE INDEX IF NOT EXISTS idx_trips_date ON Trips (TripDate);
CREATE INDEX IF NOT EXISTS idx_trips_bus_number ON Trips (BusNumber);
CREATE INDEX IF NOT EXISTS idx_fuel_bus_number ON Fuel (BusNumber);
CREATE INDEX IF NOT EXISTS idx_activities_date ON Activities (ActivityDate);
CREATE INDEX IF NOT EXISTS idx_maintenance_bus_number ON Maintenance (BusNumber);
CREATE INDEX IF NOT EXISTS idx_scheduled_routes_calendar_day_id ON ScheduledRoutes (CalendarDayID);