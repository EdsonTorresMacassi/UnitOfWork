USE KodotiSells
GO

CREATE TABLE Clients(
id int identity(1,1) primary key,
Name varchar(100)	
)

CREATE TABLE Invoices(
id int identity(1,1) primary key,
ClienteId int,
Iva decimal(18,2),
SubTotal decimal(18,2),
Total decimal(18,2)
)

create table Products(
id int identity(1,1) primary key,
Name varchar(100),
Price decimal(18,2)
)

create table InvoiceDetail(
id int identity(1,1) primary key,
InvoiceId int,
ProductId int,
Quantity int,
Price decimal(18,2),
Iva decimal(18,2),
SubTotal decimal(18,2),
Total decimal(18,2)
)

Insert into Clients (Name) Values('Edson')
Insert into Clients (Name) Values('Caarlos')
Insert into Clients (Name) Values('Waldir')

SELECT * FROM Clients

insert into Invoices (ClienteId, Iva, SubTotal, Total) values(1, 405, 1845,  2250)
insert into Invoices (ClienteId, Iva, SubTotal, Total) values(2, 292.50, 1332.50, 1625)

--Update a SET a.ClienteId = @ClienteId, a.Iva = @Iva, a.SubTotal = @SubTotal a.Total = @Total FROM Invoices a WHERE a.Id = @Id

SELECT * FROM Invoices

insert into InvoiceDetail (InvoiceId, ProductId, Quantity, Price, Iva, SubTotal, Total) values(1, 1, 1, 1.5, 270, 1230, 1500)
insert into InvoiceDetail (InvoiceId, ProductId, Quantity, Price, Iva, SubTotal, Total) values(1, 2, 1, 3, 270, 1230, 1500)
insert into InvoiceDetail (InvoiceId, ProductId, Quantity, Price, Iva, SubTotal, Total) values(2, 1, 1, 1.5, 270, 1230, 1500)
insert into InvoiceDetail (InvoiceId, ProductId, Quantity, Price, Iva, SubTotal, Total) values(2, 2, 1, 3, 270, 1230, 1500)
SELECT * FROM InvoiceDetail 

SELECT * FROM Invoices --where Id = 4
SELECT * FROM InvoiceDetail --where InvoiceId = 4


insert into Products (Name, Price) values('Laptop 14 Acer core I3', 1500)
insert into Products (Name, Price) values('Laptop 15 Asus core I3', 1700)
insert into Products (Name, Price) values('Laptop 15 Asus core I5', 2200)
insert into Products (Name, Price) values('Laptop 15 Acer core I5', 1900)
insert into Products (Name, Price) values('Disco Solido 1TB Samsung', 250.00)
insert into Products (Name, Price) values('Disco Solido 500MB Kingston', 125.00)

SELECT * FROM Products





No hay libertad de elegir la fruta debido a que siempre encontramos las mismas opciones, con mayor varidad de fruta las personas se motivan a consumirlas