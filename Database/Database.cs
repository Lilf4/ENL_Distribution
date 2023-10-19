using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Windows;
using ENF_Dist_Test.Windows;
using Newtonsoft.Json;

namespace ENF_Dist_Test {
    public class Database {
        private config config;
        public bool devState { get { return config.dev; } }

        public static readonly Database Instance = new();

        private SqlConnection getConnection() {
            SqlConnectionStringBuilder sb = new() {
                DataSource = config.dataSource,
                InitialCatalog = config.dataBase,
                UserID = config.UserID,
                Password = config.password
            };
            string connectionString = sb.ToString();
            return new SqlConnection(connectionString);
        }

        public Database() {
            if (!File.Exists("config.json")) {
                File.WriteAllText("config.json", JsonConvert.SerializeObject(new config(), Formatting.Indented));
                invalidConfig("Config file wasn't found a new one has been created\r\n please fill it before running again.");
            }
            string configData = File.ReadAllText("config.json");
            if(configData != null) {
                config = JsonConvert.DeserializeObject<config>(configData);
            }
            if(string.IsNullOrEmpty(config.dataSource) || string.IsNullOrEmpty(config.UserID) || string.IsNullOrWhiteSpace(config.dataBase) || string.IsNullOrWhiteSpace(config.password)) {
                invalidConfig("Not all required configs has been set,\r\n please set them before running again.");
            }
        }

        public void RebuildDatabase() {
            string sql = $"DROP TABLE FinishedOrders;\r\n" +
                         $"DROP TABLE Orders;\r\n" +
                         $"DROP TABLE Products;\r\n" +
                         $"DROP TABLE Locations;\r\n" +
                         $"DROP TABLE Employees;\r\n" +
                         $"CREATE TABLE Employees(EmployeeId INT PRIMARY KEY IDENTITY(1,1),\r\nCompletedOrders INT,\r\nFirstName VARCHAR(64),\r\nLastName VARCHAR(64),\r\nPhoneNumber VARCHAR(16),\r\nEmail VARCHAR(128),\r\nTitle TINYINT\r\n)\r\n\r\nCREATE TABLE Locations(\r\nLocationId VARCHAR(9) PRIMARY KEY,\r\n[Row] INT,\r\n[Column] INT\r\n)\r\n\r\nCREATE TABLE Products(\r\nProductId INT PRIMARY KEY IDENTITY(1,1),\r\n[Name] VARCHAR(64),\r\nQuantity INT,\r\n[Description] TEXT,\r\nLocationId VARCHAR(9) FOREIGN KEY REFERENCES Locations(LocationId)\r\n)\r\n\r\nCREATE TABLE Orders(\r\nOrderId INT PRIMARY KEY IDENTITY(1,1),\r\nEmployeeId INT FOREIGN KEY REFERENCES Employees(EmployeeId),\r\nProductId INT FOREIGN KEY REFERENCES Products(ProductId),\r\nQuantity INT,\r\nOrderStatus TINYINT\r\n)\r\n\r\nCREATE TABLE FinishedOrders(\r\nOrderId INT,\r\nEmployeeFirstName VARCHAR(64),\r\nEmployeeLastName VARCHAR(64),\r\nProduct VARCHAR(64),\r\nQuantity INT,\r\nOrderStatus TINYINT\r\n)";
            execute(sql);
        }

        private void invalidConfig(string msg) {
            new confirm("Invalid Config", msg, false).ShowDialog();
            Application.Current.Shutdown();
        }

        public int GetNextID(string table) {
            string SQL = $"SELECT IDENT_CURRENT('{table}') + IDENT_INCR('{table}')";
            return (int)queryDecimal(SQL);
        }
        private int execute(string sql) {
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;
                int returnVal = command.ExecuteNonQuery();

                connection.Close();

                return returnVal;
            }
        }

        private decimal queryDecimal(string sql) {
            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                decimal result = -1;

                while (reader.Read()) {
                    result = reader.GetDecimal(0);
                }

                connection.Close();

                return result;
            }
        }
        private int queryInt(string sql) {
            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                int result = -1;

                while (reader.Read()) {
                    result = reader.GetInt32(0);
                }

                connection.Close();

                return result;
            }
        }

        #region Location
        List<Location> LocationQuery(string sql) {
            List<Location> locations = new();

            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    Location location = new() {
                        Row = reader.GetInt32(1),
                        Column = reader.GetInt32(2)
                    };
                    locations.Add(location);
                }

                connection.Close();
            }

            return locations;
        }
        public Location GetLocation(string LocationId) {
            string SQL = $"SELECT * FROM Locations WHERE LocationId = '{LocationId}'";
            return LocationQuery(SQL)[0];
        }
        public List<Location> GetAllLocations() {
            string SQL = $"SELECT * FROM Locations";
            return LocationQuery(SQL);
        }
        public int InsertLocation(Location Location) {
            string SQL = $"INSERT INTO Locations (LocationId, [Row], [Column])\r\n" +
                $"OUTPUT INSERTED.LocationId\r\n" +
                $"VALUES ('{Location.LocationId}', {Location.Row}, {Location.Column})";
            return execute(SQL);
        }
        public int DeleteLocation(string LocationId) {
            string SQL = $"DELETE FROM Locations WHERE LocationId = '{LocationId}'";
            return execute(SQL);
        }
        #endregion

        #region Product
        List<Product> ProductQuery(string sql) {
            List<Product> products = new();

            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    Product product = new() {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Quantity = reader.GetInt32(2),
                        Description = reader.GetString(3),
                        Location = GetLocation(reader.GetString(4))
                    };
                    products.Add(product);
                }

                connection.Close();
            }

            return products;
        }
        public Product GetProduct(int ProductId) {
            string SQL = $"SELECT * FROM Products WHERE ProductId = {ProductId}";
            return ProductQuery(SQL)[0];
        }
        public List<Product> GetAllProducts() {
            string SQL = $"SELECT * FROM Products";
            return ProductQuery(SQL);
        }
        public int InsertProduct(Product Product) {
            string SQL = $"INSERT INTO Products (Name, Quantity, Description, LocationId)\r\n" +
                $"OUTPUT INSERTED.ProductId\r\n" +
                $"VALUES ('{Product.Name}', {Product.Quantity}, '{Product.Description}', '{Product.Location.LocationId}')";
            return queryInt(SQL);
        }
        public int UpdateProduct(Product Product, int ProductId) {
            string SQL = $"UPDATE Products\r\n" +
                $"SET Name = '{Product.Name}', Quantity = {Product.Quantity}, Description = '{Product.Description}', LocationId = '{Product.Location.LocationId}'\r\n" +
                $"WHERE ProductId = {ProductId}";
            return execute(SQL);
        }
        public int DeleteProduct(int ProductId) {
            string SQL = $"DELETE FROM Products WHERE ProductId = {ProductId}";
            return execute(SQL);
        }
        #endregion

        #region Employee
        List<Employee> EmployeeQuery(string sql) {
            List<Employee> employees = new();

            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    Employee customer = new() {
                        EmployeeId = reader.GetInt32(0),
                        CompletedOrders = reader.GetInt32(1),
                        FirstName = reader.GetString(2),
                        LastName = reader.GetString(3),
                        PhoneNumber = reader.GetString(4),
                        Email = reader.GetString(5),
                        Title = (Employee.JobTitle)reader.GetByte(6)
                    };
                    employees.Add(customer);
                }

                connection.Close();
            }

            return employees;
        }

        public Employee GetEmployee(int EmployeeId) {
            string SQL = $"SELECT * FROM Employees WHERE EmployeeId = {EmployeeId}";
            return EmployeeQuery(SQL)[0];
        }
        public List<Employee> GetAllEmployees() {
            string SQL = $"SELECT * FROM Employees";
            return EmployeeQuery(SQL);
        }
        public int InsertEmployee(Employee Employee) {
            string SQL = $"INSERT INTO Employees (FirstName, LastName, PhoneNumber, Email, Title, CompletedOrders)\r\n" +
                $"OUTPUT INSERTED.EmployeeId\r\n" +
                $"VALUES ('{Employee.FirstName}', '{Employee.LastName}', '{Employee.PhoneNumber}', '{Employee.Email}', {(byte)Employee.Title} ,{Employee.CompletedOrders})";
            return queryInt(SQL);
        }
        public int UpdateEmployee(Employee Employee, int EmployeeId) {
            string SQL = $"UPDATE Employees\r\n" +
                $"SET FirstName = '{Employee.FirstName}', LastName = '{Employee.LastName}', PhoneNumber = '{Employee.PhoneNumber}', Email = '{Employee.Email}', Title = {(byte)Employee.Title}, CompletedOrders = {Employee.CompletedOrders}\r\n" +
                $"WHERE EmployeeId = {EmployeeId}";
            return execute(SQL);
        }
        public int DeleteEmployee(int EmployeeId) {
            string SQL = $"DELETE FROM Employees WHERE EmployeeId = {EmployeeId}";
            return execute(SQL);
        }
        #endregion

        #region Order
        List<Order> OrderQuery(string sql) {
            List<Order> orders = new();

            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    Order order = new() {
                        OrderId = reader.GetInt32(0),
                        Employee = GetEmployee(reader.GetInt32(1)),
                        Product = GetProduct(reader.GetInt32(2)),
                        Quantity = reader.GetInt32(3),
                        OrderStatus = (Order.Status)reader.GetByte(4)
                    };
                    orders.Add(order);
                }

                connection.Close();
            }

            return orders;
        }
        List<Order> FinishedOrderQuery(string sql) {
            List<Order> orders = new();

            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    Order order = new() {
                        OrderId = reader.GetInt32(0),
                        Employee = new() { FirstName = reader.GetString(1), LastName = reader.GetString(2)},
                        Product = new() { Name = reader.GetString(3)},
                        Quantity = reader.GetInt32(4),
                        OrderStatus = (Order.Status)reader.GetByte(5)
                    };
                    orders.Add(order);
                }

                connection.Close();
            }

            return orders;
        }
        public Order GetOrder(int OrderId) {
            string SQL = $"SELECT * FROM Orders WHERE OrderId = {OrderId}";
            return OrderQuery(SQL)[0];
        }
        public List<Order> GetAllOrders() {
            string SQL1 = $"SELECT * FROM Orders";
            string SQL2 = $"SELECT * FROM FinishedOrders";
            List<Order> orders = OrderQuery(SQL1);
            List<Order> orders2 = FinishedOrderQuery(SQL2);
            orders.AddRange(orders2);
            return orders;
        }
        public int InsertOrder(Order Order) {
            string SQL = $"INSERT INTO Orders (EmployeeId, ProductId, Quantity, OrderStatus)\r\n" +
                $"OUTPUT INSERTED.OrderId\r\n" +
                $"VALUES ({Order.Employee.EmployeeId}, {Order.Product.ProductId}, {Order.Quantity}, {(byte)Order.OrderStatus})";
            return queryInt(SQL);
        }
        public int InsertFinishedOrder(Order Order) {
            string SQL = $"INSERT INTO FinishedOrders (OrderId, EmployeeFirstName, EmployeeLastName, Product, Quantity, OrderStatus)\r\n" +
                $"OUTPUT INSERTED.OrderId\r\n" +
                $"VALUES ({Order.OrderId}, '{Order.Employee.FirstName}', '{Order.Employee.LastName}', '{Order.Product.Name}', {Order.Quantity}, {(byte)Order.OrderStatus})";
            return queryInt(SQL);
        }
        public int UpdateOrder(Order Order, int OrderId) {
            string SQL = $"UPDATE Orders\r\n" +
                $"SET EmployeeId = '{Order.Employee.EmployeeId}', ProductId = {Order.Product.ProductId}, Quantity = {Order.Quantity}, OrderStatus = {(byte)Order.OrderStatus}\r\n" +
                $"WHERE OrderId = {OrderId}";
            return execute(SQL);
        }
        public int DeleteOrder(int OrderId) {
            string SQL = $"DELETE FROM Orders WHERE OrderId = {OrderId}";
            return execute(SQL);
        }
        #endregion
    }
}