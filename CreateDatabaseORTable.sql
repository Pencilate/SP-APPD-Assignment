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
Primary Key ("Station_Code"),
Foreign Key(Line_Code) REFERENCES LineCdRef (Line_Code)
)

CREATE TABLE Fare(
"Start_Station_Code" varchar(4),
"End_Station_Code" varchar(4),
"Card_Fare" money,
"Ticket_Fare" money,
"Journey_Duration" int,
Primary Key("Start_Station_Code","End_Station_Code"),
Foreign Key (Start_Station_Code) REFERENCES Station(Station_Code),
Foreign Key (End_Station_Code) REFERENCES Station(Station_Code))

