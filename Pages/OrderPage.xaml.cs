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
            updateButtons(null, null);
            updateTable();
        }
        private void updateTable() {
            DataGrid.ItemsSource = Database.Instance.GetAllOrders();
        }

        public void updateButtons(object sender, RoutedEventArgs e) {
            UpdateBtn.IsEnabled = hasSelected;
            DeleteBtn.IsEnabled = hasSelected;
        }

        public bool hasSelected {
            get { return DataGrid.SelectedItem != null; }
        }

        private void NavBack(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void Add(object sender, RoutedEventArgs e) {
            OrderEdit orderEdit = new OrderEdit(new() { OrderId = Database.Instance.GetNextID("Orders") }, false);
            orderEdit.ShowDialog();
            if (!orderEdit.AddCancel) {
                Database.Instance.InsertOrder(orderEdit.order);
                updateTable();
            }
        }
        private void Update(object sender, RoutedEventArgs e) {
            OrderEdit orderEdit = new OrderEdit((Order)DataGrid.SelectedItem, true);
            orderEdit.ShowDialog();
            if (!orderEdit.AddCancel) {
                Database.Instance.UpdateOrder(orderEdit.order, orderEdit.order.OrderId);
                updateTable();
            }
        }
        private void Delete(object sender, RoutedEventArgs e) {
            Order order = (Order)DataGrid.SelectedItem;
            if (MessageBox.Show($"Are you sure you want to delete {order}?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Database.Instance.DeleteOrder(order.OrderId);
                updateTable();
            }
        }
    }
}
