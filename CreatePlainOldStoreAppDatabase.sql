--DROP TABLE Posa.Customer;
--DROP TABLE Posa.Stores;
--DROP TABLE Posa.Products;
--DROP TABLE Posa.Inventory;
--DROP TABLE Posa.CustomerOrders;
--DROP TABLE Posa.OrdersInvoice;
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
	ProductPrice MONEY NOT NULL
);


CREATE TABLE Posa.Inventory
(
	InventoryId INT IDENTITY(1,1) PRIMARY KEY,
	StoreID INT FOREIGN KEY REFERENCES Posa.Stores(StoreID) NOT NULL,
	ProductID INT FOREIGN KEY REFERENCES Posa.Products(ProductID) NOT NULL,
	Quantity INT NOT NULL
);

CREATE TABLE Posa.OrdersInvoice
(
	OrdersInvoiceID INT IDENTITY(1,1) PRIMARY KEY,
	CustomerID UNIQUEIDENTIFIER FOREIGN KEY REFERENCES Posa.Customer(CustomerID) NOT NULL,
	StoreID INT NOT NULL,
	OrderTime DATETIME DEFAULT CURRENT_TIMESTAMP NOT NULL,
	OrderTotal MONEY NOT NULL
);

CREATE TABLE Posa.CustomerOrders
(
	OrderLineID INT IDENTITY(1,1) PRIMARY KEY,
	OrdersInvoiceID INT FOREIGN KEY REFERENCES Posa.OrdersiNvoice(OrdersInvoiceID) NOT NULL,
	ProductID INT FOREIGN KEY REFERENCES Posa.Products(ProductID) NOT NULL,
	ProductPrice MONEY Not NULL,
	Quantity INT NOT NULL
);


INSERT INTO Posa.Products
(
	ProductName,
	ProductDescription,
	ProductPrice
)
VALUES
	('Plain Old T-Shirt', 'Black Cotten T-Shirt', 12.99),
	('Plain Old Jeans', 'Cotten Blue Jeans', 19.99),
	('Plain Old Jean Shorts', 'Cotten Blue Jean Shorts', 12.99),
	('Plain Old Long Sleeve Button-Down Shirt', 'White Cotten Long Sleeve Shirt', 19.99),
	('Plain Old Dress', 'Black Cotten T-Shirt Dress', 19.99),
	('Plain Old Shoes', 'Black', 39.99),
	('Plain Old Shoes', 'White', 39.99);

INSERT INTO Posa.Inventory
(
	StoreID,
	ProductID,
	Quantity
)
VALUES
	(1, 1, 100),
	(1, 2, 100),
	(1, 3, 100),
	(1, 4, 100),
	(1, 5, 100),
	(1, 6, 100),
	(1, 7, 100),
	(2, 1, 100),
	(2, 2, 100),
	(2, 3, 100),
	(2, 4, 100),
	(2, 5, 100),
	(2, 6, 100),
	(2, 7, 100);

INSERT INTO Posa.Stores
(
	StoreCity
)
VALUES
	('Mountin View'),
	('San Jose');


SELECT * FROM Posa.Customer;
SELECT * FROM Posa.Stores;
SELECT * FROM Posa.Inventory;
SELECT * FROM Posa.Products;
SELECT * FROM Posa.CustomerOrders;
SELECT * FROM Posa.OrdersInvoice;
