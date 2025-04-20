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