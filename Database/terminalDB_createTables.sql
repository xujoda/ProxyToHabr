CREATE TABLE Passengers(
passenger_id INT IDENTITY(1,1) PRIMARY KEY,
fullname VARCHAR(60),
email VARCHAR(20),
phone_number VARCHAR(12),
age TINYINT
)

CREATE TABLE Trains(
train_id SMALLINT IDENTITY(1,1) PRIMARY KEY,
train_type VARCHAR(15),
train_number SMALLINT,
capacity INT
)

CREATE TABLE Cities(
city_id SMALLINT IDENTITY(1,1) PRIMARY KEY,
city_name VARCHAR(15)
)

CREATE TABLE Roads(
road_id INT IDENTITY(1,1) PRIMARY KEY,
start_city_id SMALLINT,
end_city_id SMALLINT,
FOREIGN KEY (start_city_id) REFERENCES Cities(city_id),
FOREIGN KEY (end_city_id) REFERENCES Cities(city_id)
)

CREATE TABLE Flights(
flight_id INT IDENTITY(1,1) PRIMARY KEY,
road_id INT,
datetime_start DATETIME,
datetime_end DATETIME,
FOREIGN KEY (road_id) REFERENCES Roads(road_id)
)

CREATE TABLE Tickets(
ticket_id INT IDENTITY(1,1) PRIMARY KEY,
passenger_id INT,
flight_id INT,
ticket_cost DECIMAL,
wagon_number SMALLINT,
seat_number SMALLINT,
FOREIGN KEY (flight_id) REFERENCES Flights(flight_id)
)