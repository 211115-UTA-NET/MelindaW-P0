--DROP TABLE Posa.Customer;
--DROP TABLE Posa.Stores;
--DROP TABLE Posa.Products;
--DROP TABLE Posa.CustomerOrders;
--DROP SCHEMA Posa;

CREATE SCHEMA Posa;
GO

CREATE TABLE Posa.Customer
(
    CustomerID UNIQUEIDENTIFIER PRIMARY KEY NOT NULL,
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
	StoreID INT PRIMARY KEY NOT NULL,
	StoreCity NVARCHAR(100) NOT NULL,	
);

CREATE TABLE Posa.Products
(
	ProductID INT PRIMARY KEY NOT NULL,
	ProductName NVARCHAR(100) NOT NULL,
	ProductDescription NVARCHAR(200) NOT NULL,
	ProductPrice MONEY NOT NULL,
	ProductQuantiy INT NOT NULL,
	StoreID INT NOT NULL
);

CREATE TABLE Posa.CustomerOrders
(
	OrderLineID INT PRIMARY KEY NOT NULL,
	OrderID INT NOT NULL,
	StoreID INT NOT NULL,
	CustomerID UNIQUEIDENTIFIER NOT NULL,
	OrderTime DATETIME NOT NULL,
	ProductID INT NOT NULL,
	Quantity INT NOT NULL
);

INSERT INTO Posa.Products
(
	ProductID,
	ProductName,
	ProductDescription,
	ProductPrice,
	ProductQuantiy,
	StoreID
)
VALUES
	(1, 'Plain Old T-Shrit', 'Black Cotten T-Shrit', 9.99, 100, 1),
	(2, 'Plain Old Jeans', 'Cotten Blue Jeans', 19.99, 100, 1),
	(3, 'Plain Old Jean Shorts', 'Cotten Blue Jean Shorts', 12.99, 100, 1),
	(4, 'Plain Old Long Sleeve Button-Down Shirt', 'White Cotten Long Sleeve Shrit', 19.99, 100, 1),
	(5, 'Plain Old Dress', 'Black Cotten T-Shirt Dress', 19.99, 100, 1),
	(6, 'Plain Old Shoes', 'Black', 39.99, 100, 1),
	(7, 'Plain Old Shoes', 'White', 39.99, 100, 2);

INSERT INTO Posa.Stores
(
	StoreID,
	StoreCity
)
VALUES
	(1, 'Mountin View'),
	(2, 'San Jose');

SELECT * FROM Posa.Customer;
SELECT * FROM Posa.Stores;
SELECT * FROM Posa.Products;
SELECT * FROM Posa.CustomerOrders;