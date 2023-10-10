using System.Collections.Generic;

namespace ENF_Dist_Test
{
    public class Database{
        public static readonly Database Instance = new Database();

        #region Product
        public Product GetProduct(int ProductId){
            return null;
        }
        public List<Product> GetAllProducts(){
            return null;
        }
        public int InsertProduct(Product Product){
            return -1;
        }
        public int UpdateProduct(Product Product, int ProductId){
            return -1;
        }
        public int DeleteProduct(int ProductId){
            return -1;
        }
        #endregion

        #region Employee
        public Product GetEmployee(int EmployeeId){
            return null;
        }
        public List<Product> GetAllEmployees(){
            return null;
        }
        public int InsertEmployee(Employee Employee){
            return -1;
        }
        public int UpdateEmployee(Employee Employee, int EmployeeId){
            return -1;
        }
        public int DeleteEmployee(int EmployeeId){
            return -1;
        }
        #endregion

        #region Order
        public Product GetOrder(int OrderId){
            return null;
        }
        public List<Product> GetAllOrders(){
            return null;
        }
        public int InsertOrder(Order Order){
            return -1;
        }
        public int UpdateOrder(Order Order, int OrderId){
            return -1;
        }
        public int DeleteOrder(int OrderId){
            return -1;
        }
        #endregion
    }
}