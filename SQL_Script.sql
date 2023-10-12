CREATE TABLE Employees(
    EmployeeId INT PRIMARY KEY IDENTITY(1,1),
    CompletedOrders INT,
    FirstName VARCHAR(64),
    LastName VARCHAR(64),
    PhoneNumber VARCHAR(16),
    Email VARCHAR(128),
    Title TINYINT
)

CREATE TABLE Locations(
    LocationId VARCHAR(9) PRIMARY KEY,
    [Row] INT,
    [Column] INT
)

CREATE TABLE Products(
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    [Name] VARCHAR(64),
    Quantity INT,
    [Description] TEXT,
    LocationId VARCHAR(9) FOREIGN KEY REFERENCES Locations(LocationId)
)

CREATE TABLE Orders(
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    EmployeeId INT FOREIGN KEY REFERENCES Employees(EmployeeId),
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    Quantity INT,
    OrderStatus TINYINT
)