using ENF_Dist_Test;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace ENF_Dist_Test {
    public class Database {
        public static readonly Database Instance = new Database();

        private SqlConnection getConnection() {
            SqlConnectionStringBuilder sb = new();
            sb.DataSource = "DESKTOP-J88769P\\MSSQLSERVER01";
            sb.InitialCatalog = "ENL_Distribution";
            sb.UserID = "ENL_Login";
            sb.Password = "ENL_Login";
            string connectionString = sb.ToString();
            return new SqlConnection(connectionString);
        }

        public Database() {

        }

        private DataTable QueryDataTable(string sql) {
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                DataTable dt = new();

                SqlDataAdapter adapter = new SqlDataAdapter(command);

                adapter.Fill(dt);

                connection.Close();

                return dt;
            }

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


        #region Location
        List<Location> LocationQuery(string sql) {
            List<Location> locations = new List<Location>();

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
            string SQL = $"SELECT * FROM Locations WHERE LocationId = {LocationId}";
            return LocationQuery(SQL)[0];
        }
        public List<Location> GetAllLocations() {
            string SQL = $"SELECT * FROM Locations";
            return LocationQuery(SQL);
        }
        public int InsertLocation(Location Location) {
            string SQL = $"INSERT INTO Locations (LocationId, [Row], [Column])\r\n" +
                $"VALUES ('{Location.LocationId}', {Location.Row}, {Location.Column})";
            return execute(SQL);
        }
        public int DeleteLocation(string LocationId) {
            string SQL = $"DELETE FROM Locations WHERE LocationId = {LocationId}";
            return execute(SQL);
        }
        #endregion

        #region Product
        List<Product> ProductQuery(string sql) {
            List<Product> products = new List<Product>();

            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    Product product = new Product() {
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
                $"VALUES ('{Product.Name}', {Product.Quantity}, '{Product.Description}', '{Product.Location.LocationId}')";
            return execute(SQL);
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
            List<Employee> employees = new List<Employee>();

            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    Employee customer = new Employee() {
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
                $"VALUES ('{Employee.FirstName}', '{Employee.LastName}', '{Employee.PhoneNumber}', '{Employee.Email}', {(byte)Employee.Title} ,{Employee.CompletedOrders})";
            return execute(SQL);
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
            List<Order> orders = new List<Order>();

            SqlDataReader reader;
            using (SqlConnection connection = getConnection()) {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                command.CommandText = sql;

                reader = command.ExecuteReader();

                while (reader.Read()) {
                    Order order = new Order() {
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
        public Order GetOrder(int OrderId) {
            string SQL = $"SELECT * FROM Orders WHERE OrderId = {OrderId}";
            return OrderQuery(SQL)[0];
        }
        public List<Order> GetAllOrders() {
            string SQL = $"SELECT * FROM Orders";
            return OrderQuery(SQL);
        }
        public int InsertOrder(Order Order) {
            string SQL = $"INSERT INTO Orders (EmployeeId, ProductId, Quantity, OrderStatus)\r\n" +
                $"VALUES ({Order.Employee.EmployeeId}, {Order.Product.ProductId}, {Order.Quantity}, {(byte)Order.OrderStatus})";
            return execute(SQL);
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