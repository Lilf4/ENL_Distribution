using ENF_Dist_Test.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ENF_Dist_Test.Pages {
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class OrderPage : Page {
        public OrderPage() {
            InitializeComponent();
            UpdateButtons(null, null);
            UpdateTable();
        }
        private void UpdateTable() {
            DataGrid.ItemsSource = Database.Instance.GetAllOrders();
        }

        public void UpdateButtons(object? sender, RoutedEventArgs? e) {
            UpdateBtn.IsEnabled = HasSelected;
            DeleteBtn.IsEnabled = HasSelected;
        }

        public bool HasSelected {
            get { 
                if(DataGrid.SelectedItem != null) {
                    return ((Order)DataGrid.SelectedItem).OrderStatus != Order.Status.Finished;
                } 
                return false;
            }
        }

        private void NavBack(object? sender, RoutedEventArgs? e) {
            NavigationService.GoBack();
        }

        private void FinishOrder(Order order) {
            order.Employee.CompletedOrders++;
            order.Product.Quantity -= order.Quantity;
            Database.Instance.UpdateProduct(order.Product, order.Product.ProductId);
            Database.Instance.UpdateEmployee(order.Employee, order.Employee.EmployeeId);
        }

        private void Add(object? sender, RoutedEventArgs? e) {
            OrderEdit orderEdit = new(new() { OrderId = Database.Instance.GetNextID("Orders") }, false);
            orderEdit.ShowDialog();
            if (!orderEdit.AddCancel) {
                if (orderEdit.Order.OrderStatus == Order.Status.Finished) {
                    Database.Instance.InsertFinishedOrder(orderEdit.Order);
                    FinishOrder(orderEdit.Order);
                }
                else {
                    Database.Instance.InsertOrder(orderEdit.Order);
                }
                UpdateTable();
            }
        }
        private void Update(object? sender, RoutedEventArgs? e) {
            OrderEdit orderEdit = new((Order)DataGrid.SelectedItem, true);
            orderEdit.ShowDialog();
            if (!orderEdit.AddCancel) {
                if (orderEdit.Order.OrderStatus == Order.Status.Finished) {
                    if (orderEdit.Order.Product.Quantity - orderEdit.Order.Quantity >= 0) {
                        Database.Instance.DeleteOrder(orderEdit.Order.OrderId);
                        Database.Instance.InsertFinishedOrder(orderEdit.Order);
                        FinishOrder(orderEdit.Order);
                    }
                }
                else {
                    Database.Instance.UpdateOrder(orderEdit.Order, orderEdit.Order.OrderId);
                }
                UpdateTable();
            }
        }
        private void Delete(object? sender, RoutedEventArgs? e) {
            Order order = (Order)DataGrid.SelectedItem;
            if (MessageBox.Show($"Are you sure you want to delete {order}?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Database.Instance.DeleteOrder(order.OrderId);
                UpdateTable();
            }
        }
    }
}
