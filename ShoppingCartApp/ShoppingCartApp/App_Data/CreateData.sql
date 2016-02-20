DROP TABLE ProductVisit;
DROP TABLE Product;
DROP TABLE Visit;
 
CREATE TABLE Visit
(
	sessionID    VARCHAR(44) PRIMARY KEY, 
	started      DATETiME
);
CREATE TABLE Product
(
	productID    INTEGER NOT NULL PRIMARY KEY,
	productName  VARCHAR(30), 
	price	     MONEY
		
);

CREATE TABLE Images 
(
	productID    INTEGER PRIMARY KEY,
	imageTitle	 VARCHAR(50)
	FOREIGN KEY (productID)  REFERENCES Product(productID)
)

CREATE TABLE   ProductVisit
(
	sessionID   VARCHAR(44)	 NOT NULL,
	productID    INTEGER NOT NULL,
	qtyOrdered   INTEGER,
	updated      DATETiME,
	PRIMARY KEY (sessionID, productID),
	FOREIGN KEY (sessionID) REFERENCES Visit(sessionID),
	FOREIGN KEY (productID)  REFERENCES Product(productID)
);
GO

INSERT INTO Product VALUES(107, 'Bike Gloves', 14.99);
INSERT INTO Product VALUES(218, 'Hockey Stick', 23.97);
INSERT INTO Product VALUES(329, 'Jersey', 56.32);
INSERT INTO Visit VALUES('asdf234dfa23lkjkj232', '20140101 10:34:09 AM')
INSERT INTO ProductVisit VALUES('asdf234dfa23lkjkj232', 107, 2, '20140101 10:34:09 AM')
GO
