BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Drivers" (
	"DriverID"	INTEGER,
	"Driver Name"	TEXT,
	"Address"	TEXT,
	"City"	TEXT,
	"State"	TEXT,
	"Zip Code"	TEXT,
	"Phone Number"	TEXT,
	"Email Address"	TEXT,
	"Is Stipend Paid"	TEXT,
	"DL Type"	TEXT
);
CREATE TABLE IF NOT EXISTS "Fuel" (
	"Fuel_ID"	INTEGER,
	"Bus Number"	INTEGER,
	"Fuel Gallons"	INTEGER,
	"Fuel Date"	TEXT,
	"Fuel Type"	TEXT,
	"Odometer Reading"	INTEGER
);
CREATE TABLE IF NOT EXISTS "Maintenance" (
	"ID"	INTEGER,
	"Bus Number"	INTEGER,
	"Date of Service"	TEXT,
	"Vendor"	TEXT,
	"Invoice Number"	TEXT,
	"Service Done"	TEXT,
	"Cost"	TEXT,
	"Scanned Invoice"	INTEGER,
	"Mileage"	TEXT
);
CREATE TABLE IF NOT EXISTS "Routes" (
	"RouteID"	INTEGER,
	"RouteName"	TEXT NOT NULL,
	"StartTime"	TEXT NOT NULL,
	"EndTime"	TEXT NOT NULL,
	"BusNumber"	TEXT NOT NULL,
	PRIMARY KEY("RouteID")
);
CREATE TABLE IF NOT EXISTS "Trips" (
    TripID INTEGER PRIMARY KEY,
    TripType TEXT NOT NULL,
    Date TEXT NOT NULL,
    StartTime TEXT, -- Allow NULL since we don’t have this data
    EndTime TEXT, -- Allow NULL since we don’t have this data
    Destination TEXT, -- Allow NULL since some rows are NULL
    BusNumber INTEGER NOT NULL,
    DriverName TEXT, -- Allow NULL since some rows might lack both AM/PM drivers
    AM_Begin_Mileage INTEGER,
    AM_End_Mileage INTEGER,
    Num_Riders INTEGER,
    PM_Start_Mileage INTEGER,
    PM_Ending_Mileage TEXT,
    Num_PM_Riders TEXT,
    Total_Hours_Driven TEXT
);
CREATE TABLE IF NOT EXISTS "Vehicles" (
	"VehicleID"	INTEGER,
	"Bus Number"	INTEGER,
	"Model Year"	INTEGER,
	"VIN"	TEXT,
	"Make"	TEXT,
	"Model"	TEXT,
	"Purchase Date"	TEXT,
	"Plate Number"	TEXT,
	"Seating Capacity"	INTEGER,
	"Annual Inspection"	TEXT,
	"Purchase Price"	TEXT
);
CREATE TABLE IF NOT EXISTS "Activities" (
	"ActivityID"	INTEGER PRIMARY KEY AUTOINCREMENT,
	"Date"	TEXT NOT NULL,
	"BusNumber"	INTEGER NOT NULL,
	"Destination"	TEXT NOT NULL,
	"LeaveTime"	TEXT NOT NULL,
	"Driver"	TEXT NOT NULL,
	"HoursDriven"	TEXT,
	"StudentsDriven"	INTEGER
);
INSERT INTO "Drivers" ("DriverID","Driver Name","Address","City","State","Zip Code","Phone Number","Email Address","Is Stipend Paid","DL Type") VALUES (4,'Sheilah Anderson',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (5,'Tandy Bitner',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (6,'Riley Brookshire',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (7,'Samantha Brookshire',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (8,'Erin Brophy',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (9,'Heidi Choat',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (10,'Tia Christie',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (11,'Ashley Duvall',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (12,'Dusty Eikenberg',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','CDL'),
 (13,'Cody Elwin',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (14,'Mark Grasmick',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (15,'Chad Krentz',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (16,'Shae Krentz',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (17,'Neil Mauch',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','CDL'),
 (18,'Hadlie McDowell',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (19,'Brad Phillips',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (20,'Dustyn Reeder',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (21,'Mayra Salgado',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (22,'Yancy Shelton',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (23,'Daylon Spitz',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (24,'Ashley Tixier',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (25,'Dean Thompson',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (26,'Samantha Weeks',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger'),
 (27,'Tim Weeks',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','CDL'),
 (28,'Aiden Michaels',NULL,NULL,NULL,NULL,NULL,NULL,'TRUE','Passenger'),
 (33,'Whitney Lubbers','34247 CR 7','Lamar','CO','81052','(806) 223-3437','whitney.lubbers@wileyschool.org','TRUE','Passenger'),
 (34,'Alyssa Carty','22 Mayhew Drive','Lamar','CO','81052','(719) 688-5931','alyssa.carty@wileyschool.org','TRUE','Passenger'),
 (38,'Draven Adame',NULL,NULL,NULL,NULL,'719-691-1559',NULL,'FALSE','CDL'),
 (40,'Stephen McKitrick','508 Gordon St.','Wiley','CO','81092','719-640-2230','steve.mckitrick@wileyschool.org','FALSE','CDL'),
 (42,'Kolby Stegman',NULL,NULL,NULL,NULL,NULL,NULL,'FALSE','Passenger');
INSERT INTO "Fuel" ("Fuel_ID","Bus Number","Fuel Gallons","Fuel Date","Fuel Type","Odometer Reading") VALUES (1,9,12,'2/26/2025 0:00','Unleaded',130069),
 (3,11,15,'3/10/2025 8:13','Unleaded',135784),
 (4,16,21,'2/28/2025 8:14','Unleaded',12955),
 (7,17,16,'2/28/2025 8:18','Diesel',50928),
 (8,9,8,'3/3/2025 8:19','Unleaded',130275),
 (9,16,13,'3/3/2025 8:19','Unleaded',13084),
 (10,8,16,'3/3/2025 8:23','Unleaded',146940),
 (11,16,20,'3/7/2025 8:24','Unleaded',13277),
 (12,9,9,'3/10/2025 8:24','Unleaded',131114),
 (13,9,9,'3/10/2025 8:24','Unleaded',131114),
 (14,17,46,'3/10/2025 8:26','Diesel',51855),
 (15,8,20,'3/10/2025 8:32','Unleaded',147086),
 (16,12,35,'3/17/2025 8:33','Diesel',83136),
 (17,17,47,'3/20/2025 8:33','Diesel',52244),
 (18,9,8,'3/24/2025 8:34','Unleaded',131321),
 (19,16,30,'3/25/2025 8:34','Unleaded',13798),
 (20,9,8,'3/25/2025 8:35','Unleaded',131528),
 (21,9,12,'3/27/2025 8:35','Unleaded',131837),
 (22,7,62,'3/27/2025 8:36','Diesel',92840),
 (23,14,7.1,'3/29/2025 8:36','Unleaded',113184),
 (30,7,72.1,'4/3/2025 9:18','Diesel',93343),
 (31,9,11,'4/4/2025 9:19','Unleaded',132711),
 (32,11,14.4,'4/4/2025 9:19','Unleaded',136066);
INSERT INTO "Maintenance" ("ID","Bus Number","Date of Service","Vendor","Invoice Number","Service Done","Cost","Scanned Invoice","Mileage") VALUES (2,6,'2/18/25','Ron Austin Repair','J001831','Repair/Replace Oxygen Sensor','$448.52',0,NULL),
 (3,18,'2/18/25','ACE Tire','1-146260','Replace Tire, valve stems','$214.90',0,NULL),
 (4,11,'2/16/25','NAPA','778753','Low Freon','$75.00',0,NULL),
 (5,14,'3/3/25','ACE Tire','1-146536','Replace Tires','$938.20',0,NULL),
 (6,18,'3/5/25','Rupp''s Truck and Trailer','5532','Activity Bus Fiasco','$5,496.57',0,NULL),
 (7,17,'3/7/25','NAPA','779986','Tire Chains, Bungee, DEF','$244.11',0,NULL),
 (8,17,'3/18/25','Double K Car Wash',NULL,'Wash','$6.79',0,NULL),
 (9,14,'3/24/25','Double K Car Wash',NULL,'Wash','$2.59',0,NULL),
 (10,16,'3/25/25','Double K Car Wash',NULL,'Wash','$7.16',0,NULL),
 (11,14,'3/26/25','NAPA',NULL,'Oil Change','$33.98',0,NULL);
INSERT INTO "Routes" ("RouteID","RouteName","StartTime","EndTime","BusNumber") VALUES (1,'Morning Run','07:00','08:00','101'),
 (2,'Afternoon Run','14:30','15:30','102'),
 (3,'Late Activity','17:00','18:00','103');
INSERT INTO "Vehicles" ("VehicleID","Bus Number","Model Year","VIN","Make","Model","Purchase Date","Plate Number","Seating Capacity","Annual Inspection","Purchase Price") VALUES (3,9,2013,'2FMGK5B84DBD19104','Ford','Flex','10/5/2012','767-YUV',5,'7/9/2024','$10,000.00'),
 (4,8,2005,'1GDJG31U441224306','Bluebird','Microbus','8/30/2008','AEG-564',14,'8/6/2024','$10,000.00'),
 (5,7,2008,'1BAKGCPA38F250498','Bluebird','School Bus','2/8/2008','TWQ-507',71,'8/5/2024','$10,000.00'),
 (6,6,2006,'1GDJ631UX51225476','Thomas','Microbus','11/25/2019','WOU-720',14,'8/6/2024','$10,000.00'),
 (7,17,2021,'1T88Y9D23M1169830','Thomas','School Bus','3/25/2024','AIO-A14',83,'4/7/2025','$104,000.00'),
 (8,16,2023,'TGZ67UB78PN013733','Bluebird','Micro Bus','12/15/2023','DMH-324',14,'1/9/2024','$80,000.00'),
 (9,14,2018,'1FBZX2YM0JKB01863','Ford','Transit','10/21/2021','CDO-N51',11,'7/15/2024','$8,585.00'),
 (10,12,2009,'1BAKGCPA89F262485','Bluebird','School Bus','2/10/2021','BSK-012',65,'8/6/2024','$23,222.00');
COMMIT;
