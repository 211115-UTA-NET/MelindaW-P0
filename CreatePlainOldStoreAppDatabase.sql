--DROP TABLE Posa.Customer;
--DROP TABLE Posa.Stores;
--DROP TABLE Posa.Products;
--DROP TABLE Posa.CustomerOrders;
--DROP SCHEMA Posa;

CREATE SCHEMA Posa;
GO

CREATE TABLE Posa.Customer
(
    CustomerID UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Address1 NVARCHAR(200) NOT NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    Zip NVARCHAR(10) NOT NULL,
    Email NVARCHAR(250) NOT NULL
);

Create Table Posa.Stores
(
	StoreID INT IDENTITY(1,1) PRIMARY KEY,
	StoreCity NVARCHAR(100) NOT NULL,	
);

CREATE TABLE Posa.Products
(
	ProductID INT IDENTITY(1,1) PRIMARY KEY,
	ProductName NVARCHAR(100) NOT NULL,
	ProductDescription NVARCHAR(200) NOT NULL,
	ProductPrice MONEY NOT NULL,
	ProductQuantiy INT NOT NULL,
	StoreID INT NOT NULL
);

CREATE TABLE Posa.CustomerOrders
(
	OrderLineID INT IDENTITY(1,1) PRIMARY KEY,
	OrderID INT NOT NULL,
	StoreID INT NOT NULL,
	CustomerID UNIQUEIDENTIFIER NOT NULL,
	OrderTime DATETIME NOT NULL,
	ProductID INT NOT NULL,
	Quantity INT NOT NULL
);

INSERT INTO Posa.Products
(
	ProductName,
	ProductDescription,
	ProductPrice,
	ProductQuantiy,
	StoreID
)
VALUES
	('Plain Old T-Shrit', 'Black Cotten T-Shrit', 9.99, 100, 1),
	('Plain Old Jeans', 'Cotten Blue Jeans', 19.99, 100, 1),
	('Plain Old Jean Shorts', 'Cotten Blue Jean Shorts', 12.99, 100, 1),
	('Plain Old Long Sleeve Button-Down Shirt', 'White Cotten Long Sleeve Shrit', 19.99, 100, 1),
	('Plain Old Dress', 'Black Cotten T-Shirt Dress', 19.99, 100, 1),
	('Plain Old Shoes', 'Black', 39.99, 100, 1),
	('Plain Old Shoes', 'White', 39.99, 100, 2);

INSERT INTO Posa.Stores
(
	StoreCity
)
VALUES
	('Mountin View'),
	('San Jose');

SELECT * FROM Posa.Customer;
SELECT * FROM Posa.Stores;
SELECT * FROM Posa.Products;
SELECT * FROM Posa.CustomerOrders;