create Table History(
"Start_Station_Code" varchar(4),
"End_Station_Code" varchar(4),
"Card_Fare" money,
"Ticket_Fare" money,
"Journey_Duration" int,
Primary Key("Start_Station_Code","End_Station_Code"))