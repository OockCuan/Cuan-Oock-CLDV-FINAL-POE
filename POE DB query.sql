

--Database drop, there for testing purposes
DROP DATABASE POEDB

--Creating and using the database 
CREATE DATABASE POEDB 
USE POEDB 

--First venue table
CREATE TABLE Venue ( 
    VenueID INT Identity(1,1) NOT NULL PRIMARY KEY, 
    VenueName VarChar(50) NOT NULL, 
    VenueLocation VarChar(50) NOT NULL, 
    Capacity INT NOT NULL, 
    ImageURL VARCHAR(100) 
); 

--Second event table
CREATE TABLE Event ( 
	--Slight variation in the primary key, starting at 101 so it is not repititive
    EventID INT Identity(101,1) NOT NULL PRIMARY KEY, 
    VenueID INT NOT NULL, 
    EventName VarChar(50) NOT NULL, 
    EventDate VarChar(50) NOT NULL, 
    EventDescription VarChar(200) NOT NULL, 
	--Establishing foreign key
    FOREIGN KEY (VenueID) REFERENCES Venue(VenueID) 
); 

CREATE TABLE Booking ( 
    BookingID INT Identity(1,1) NOT NULL PRIMARY KEY, 
    VenueID INT NOT NULL, 
    EventID INT NOT NULL, 
    BookingDate DATE NOT NULL,
	--Establishing both foreign keys
    FOREIGN KEY (VenueID) REFERENCES Venue(VenueID), 
    FOREIGN KEY (EventID) REFERENCES Event(EventID) 
); 

--Adding in example/sample data into the tables for testing purposes
INSERT INTO VENUE (VenueName, VenueLocation, Capacity, ImageURL) 
VALUES ('Hall 1', 'Location 1', 100, 'exampleurl.image')

INSERT INTO Event (VenueID, EventName, EventDate, EventDescription) 
VALUES (1,'Wedding', '02-02-2025','Example wedding between two clients')

INSERT INTO BOOKING(VenueID, EventID, BookingDate)
VALUES (1,101,'01-01-2025')

--viewing the tables
SELECT * FROM Venue
SELECT * FROM Event
SELECT * FROM Booking
