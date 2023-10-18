using ENF_Dist_Test.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ENF_Dist_Test.Pages {
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
            UpdateBtn.Opacity = HasSelected ? 1 : 0.5;
            UpdateBtn.IsEnabled = HasSelected;
            
            DeleteBtn.Opacity = HasSelected ? 1 : 0.5;
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
            if (new confirm("Delete", $"Are you sure you want to delete Order #{order.OrderId}\r\nOrder content: {order.Product} ({order.Quantity})").ShowDialog().Value) {
                Database.Instance.DeleteOrder(order.OrderId);
                UpdateTable();
            }
        }
    }
}
