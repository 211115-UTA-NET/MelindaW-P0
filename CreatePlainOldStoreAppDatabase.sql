USE [211115-SQL-WaggonerM]

IF (EXISTS (SELECT OBJECT_ID FROM sys.tables WHERE name='Customer'))
BEGIN
DROP TABLE Customer;
END

CREATE TABLE Customer
(
    CustomerID INT NOT NULL,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Address1 NVARCHAR(200) NOT NULL,
    Address2 NVARCHAR(200) NULL,
    City NVARCHAR(100) NOT NULL,
    State NVARCHAR(100) NOT NULL,
    Zip VARCHAR(10) NOT NULL,
    Country VARCHAR(100) NOT NULL,
    Email VARCHAR(250) NOT NULL,
    StoreId INT NOT NULL
)