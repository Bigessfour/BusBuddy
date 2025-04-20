CREATE TABLE IF NOT EXISTS Trips (
         TripID INTEGER PRIMARY KEY AUTOINCREMENT,
         TripType TEXT NOT NULL,
         Date TEXT NOT NULL,
         BusNumber INTEGER NOT NULL,
         DriverName TEXT NOT NULL,
         StartTime TEXT,
         EndTime TEXT,
         Total_Hours_Driven REAL,
         Destination TEXT
     );

     CREATE TABLE IF NOT EXISTS Drivers (
         DriverID INTEGER PRIMARY KEY AUTOINCREMENT,
         "Driver Name" TEXT NOT NULL,
         Address TEXT,
         City TEXT,
         State TEXT,
         "Zip Code" TEXT,
         "Phone Number" TEXT,
         "Email Address" TEXT,
         "Is Stipend Paid" BOOLEAN,
         "DL Type" TEXT
     );

     CREATE TABLE IF NOT EXISTS Vehicles (
         "Bus Number" INTEGER PRIMARY KEY
     );

     CREATE TABLE IF NOT EXISTS Fuel (
         Fuel_ID INTEGER PRIMARY KEY AUTOINCREMENT,
         "Bus Number" INTEGER NOT NULL,
         "Fuel Gallons" REAL NOT NULL,
         "Fuel Date" TEXT NOT NULL,
         "Fuel Type" TEXT,
         "Odometer Reading" INTEGER,
         FOREIGN KEY ("Bus Number") REFERENCES Vehicles ("Bus Number")
     );

     CREATE TABLE IF NOT EXISTS Activities (
         ActivityID INTEGER PRIMARY KEY AUTOINCREMENT,
         Date TEXT NOT NULL,
         BusNumber INTEGER NOT NULL,
         Destination TEXT NOT NULL,
         LeaveTime TEXT NOT NULL,
         Driver TEXT NOT NULL,
         HoursDriven TEXT,
         StudentsDriven INTEGER,
         FOREIGN KEY (BusNumber) REFERENCES Vehicles ("Bus Number")
     );

     -- New Tables for Routes and School Calendar
     CREATE TABLE IF NOT EXISTS Routes (
         RouteId INTEGER PRIMARY KEY AUTOINCREMENT,
         RouteName TEXT NOT NULL,
         DefaultBusNumber INTEGER NOT NULL,
         DefaultDriverName TEXT NOT NULL,
         Description TEXT,
         FOREIGN KEY (DefaultBusNumber) REFERENCES Vehicles ("Bus Number")
     );

     CREATE TABLE IF NOT EXISTS SchoolCalendarDays (
         CalendarDayId INTEGER PRIMARY KEY AUTOINCREMENT,
         Date TEXT NOT NULL,
         IsSchoolDay BOOLEAN NOT NULL DEFAULT 1,
         DayType TEXT NOT NULL DEFAULT 'Regular',
         Notes TEXT
     );

     CREATE TABLE IF NOT EXISTS ScheduledRoutes (
         ScheduledRouteId INTEGER PRIMARY KEY AUTOINCREMENT,
         CalendarDayId INTEGER NOT NULL,
         RouteId INTEGER NOT NULL,
         AssignedBusNumber INTEGER NOT NULL,
         AssignedDriverName TEXT NOT NULL,
         FOREIGN KEY (CalendarDayId) REFERENCES SchoolCalendarDays (CalendarDayId),
         FOREIGN KEY (RouteId) REFERENCES Routes (RouteId),
         FOREIGN KEY (AssignedBusNumber) REFERENCES Vehicles ("Bus Number")
     );

     -- Insert default routes if they don't exist
     INSERT OR IGNORE INTO Routes (RouteId, RouteName, DefaultBusNumber, DefaultDriverName, Description)
     SELECT 1, 'Truck Plaza Route', MIN("Bus Number"), '', 'Route to the Truck Plaza area'
     FROM Vehicles
     WHERE NOT EXISTS (SELECT 1 FROM Routes WHERE RouteId = 1);

     INSERT OR IGNORE INTO Routes (RouteId, RouteName, DefaultBusNumber, DefaultDriverName, Description)
     SELECT 2, 'East Route', MIN("Bus Number"), '', 'Eastern district route'
     FROM Vehicles
     WHERE NOT EXISTS (SELECT 1 FROM Routes WHERE RouteId = 2);

     INSERT OR IGNORE INTO Routes (RouteId, RouteName, DefaultBusNumber, DefaultDriverName, Description)
     SELECT 3, 'West Route', MIN("Bus Number"), '', 'Western district route'
     FROM Vehicles
     WHERE NOT EXISTS (SELECT 1 FROM Routes WHERE RouteId = 3);

     INSERT OR IGNORE INTO Routes (RouteId, RouteName, DefaultBusNumber, DefaultDriverName, Description)
     SELECT 4, 'SPED Route', MIN("Bus Number"), '', 'Special Education route'
     FROM Vehicles
     WHERE NOT EXISTS (SELECT 1 FROM Routes WHERE RouteId = 4);