--CREATE DATABASE APPDCADB

USE APPDCADB

CREATE TABLE LineCdRef(
Line_Code varchar(2),
Primary Key(Line_Code))

CREATE TABLE Station(
Line_Code varchar(2) NOT NULL,
Id int NOT NULL,
Station_Code varchar(4) NOT NULL,
Station_Name varchar(255) NOT NULL,
Primary Key (Station_Code),
Foreign Key(Line_Code) REFERENCES LineCdRef (Line_Code)
)

CREATE TABLE Fare(
Start_Station_Code varchar(4) NOT NULL,
End_Station_Code varchar(4) NOT NULL,
Card_Fare money NULL,
Ticket_Fare money NULL,
Journey_Duration int NULL,
Primary Key(Start_Station_Code,End_Station_Code),
Foreign Key (Start_Station_Code) REFERENCES Station(Station_Code),
Foreign Key (End_Station_Code) REFERENCES Station(Station_Code))

CREATE TABLE FareHistory (
Start_Station_Code VARCHAR(4) NOT NULL,
End_Station_Code VARCHAR(4) NOT NULL,
Date_Queried DATE NOT NULL
Fare_Type CHAR(1) NOT NULL,
Fare MONEY NOT NULL,
Primary Key(Start_Station_Code,End_Station_Code),
Foreign Key (Start_Station_Code,End_Station_Code) REFERENCES Fare(Start_Station_Code,End_Station_Code))

