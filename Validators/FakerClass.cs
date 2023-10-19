using Bogus;
using Fare;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ENF_Dist_Test.Validators {
    internal class FakerClass {

        public void generateData(fakerGenerateInfo genInfo) {

            Random rand = new Random();
            var phone = new Xeger("((^\\d{8})|(^\\d{2}[ ]\\d{2}[ ]\\d{2}[ ]\\d{2})|(^\\d{4}[ ]\\d{4}))$");
            List<Location> usedLocations = new();

            var Product = new Faker<Product>()
                .StrictMode(false)
                .RuleFor(p => p.Name, (f) => f.Commerce.ProductName().Replace("\'", "''"))
                .RuleFor(p => p.Description, (f, u) => f.Commerce.ProductDescription().Replace("\'", "''"))
                .RuleFor(p => p.Quantity, (f) => f.Random.Number(0, genInfo.maxProdQuantity))
                .FinishWith((f, u) => {
                    u.Location = genUniqueLocation(genInfo.maxProducts, usedLocations);
                    usedLocations.Add(u.Location);
                    Database.Instance.InsertLocation(u.Location);
                });

            var Products = Product.Generate(rand.Next(1, genInfo.maxProducts));

            foreach (var _product in Products) {
                _product.ProductId = Database.Instance.InsertProduct(_product);
            }

            List<Order> orders = new();

            var Employee = new Faker<Employee>()
                .StrictMode(false)
                .RuleFor(e => e.FirstName, (f) => f.Name.FirstName().Replace("\'", "''"))
                .RuleFor(e => e.LastName, (f) => f.Name.LastName().Replace("\'", "''"))
                .RuleFor(e => e.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(e => e.PhoneNumber, (f, u) => phone.Generate().Remove(0, 1))
                .RuleFor(e => e.Title, (f, u) => f.Random.Enum<Employee.JobTitle>())
                .FinishWith((f, u) => {
                    u.EmployeeId = Database.Instance.InsertEmployee(u);
                    Faker<Order> order = new Faker<Order>()
                        .RuleFor(o => o.Quantity, (l) => l.Random.Number(1, genInfo.maxOrdQuantity))
                        .RuleFor(o => o.Product, (l) => l.PickRandom(Products))
                        .RuleFor(o => o.Employee, (l, k) => k.Employee = u)
                        .FinishWith((l, k) => {
                            int validStatus = 1;
                            if (k.Quantity <= k.Product.Quantity) { validStatus += 1; }
                            k.OrderStatus = (Order.Status)rand.Next(0, validStatus + 1);
                            if (k.OrderStatus == Order.Status.Finished) {
                                u.CompletedOrders++;
                                k.Product.Quantity -= k.Quantity;
                                Database.Instance.UpdateProduct(k.Product, k.Product.ProductId);
                            }
                        });
                    orders.AddRange(order.Generate(rand.Next(genInfo.minOrdPerEmployee, genInfo.maxOrdPerEmployee)));

                    Database.Instance.UpdateEmployee(u, u.EmployeeId);
                });

            var Employees = Employee.Generate(rand.Next(genInfo.minEmployees, genInfo.maxEmployees));

            int orderCount = orders.Count();
            for (int i = 0; i < orderCount; i++) {
                int orderIndex = rand.Next(0, orders.Count());
                Order order = orders[orderIndex];
                if (order.OrderStatus == Order.Status.Finished) {
                    order.OrderId = Database.Instance.InsertOrder(order);
                    Database.Instance.DeleteOrder(order.OrderId);
                    Database.Instance.InsertFinishedOrder(order);
                }
                else {
                    Database.Instance.InsertOrder(order);
                }
                orders.RemoveAt(orderIndex);
            }
        }

        public Location genUniqueLocation(int maxValue, List<Location> Exclusions) {
            Random rand = new Random();
            Location location = new Location();

            while (true) {
                location.Row = rand.Next(1, maxValue + 1);
                location.Column = rand.Next(1, maxValue + 1);
                var match = Exclusions.Where(i => (i.LocationId == location.LocationId));
                if (match.Count() <= 0) { break; }
            }

            return (location);
        }
    }
}