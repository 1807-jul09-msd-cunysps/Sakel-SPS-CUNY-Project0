USE Coding_Challenge2;

CREATE TABLE Products (
ID int PRIMARY KEY IDENTITY(1,1),
Name VARCHAR(50),
Price MONEY,
);

CREATE TABLE Customers( 
ID int PRIMARY KEY IDENTITY(1,1),
Firstname varchar(50),
Lastname varchar (50),
CardNUmber INT,
);

CREATE TABLE Orders (
ID int PRIMARY KEY IDENTITY(1,1),
ProductID int,
CustomerID int,
FOREIGN KEY (ProductID) REFERENCES Products(ID),
FOREIGN KEY (CustomerID) REFERENCES Customers(ID),
);


INSERT INTO Products (Name, Price) 
VALUES
	('Laptop','599.99'),
    ('IPhone','1000'),
    ('Galaxy8',	'850')
	;
    
INSERT INTO Customers(Firstname, Lastname, CardNUmber)
VALUES
	('Tina','Smith', '12345678'),
	('Anne','Thompson', '23456789'),
	('Joe','Chen', '34567890')
	;

INSERT INTO Orders (ProductID, CustomerID)
VALUES
	(1,2),
	(2,1),
	(1,3)
	;

SELECT ID, Firstname, Lastname FROM Customers;
SELECT ProductID, CustomerID from Orders;
SELECT Name, Price FROM Products;

SELECT c.Firstname, c.Lastname, o.CustomerID, o.ProductID FROM Customers AS c
JOIN
Orders AS o ON c.ID = o.CustomerID
WHERE c.Firstname = 'Tina' AND c.Lastname = 'Smith'
;

SELECT SUM(p.Price) FROM Products AS p
JOIN
Orders AS o ON p.ID = o.ProductID
GROUP BY p.Name
HAVING p.Name = 'Iphone'
;

UPDATE Products
SET Price = '1200'
WHERE Products.Name = 'Iphone';
SELECT Name, Price FROM Products;





