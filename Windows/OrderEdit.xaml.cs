using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace ENF_Dist_Test.Windows {
    /// <summary>
    /// Interaction logic for ProductEdit.xaml
    /// </summary>
    public partial class OrderEdit : Window, INotifyPropertyChanged {

        public bool AddCancel = true;

        private Order order { get; set; }
        public Order Order { 
            get {
                return order;
            }
            set {
                order = value;
                OnPropertyChanged(nameof(Order));
            } 
        }
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OrderEdit(Order order, bool AddEdit) {
            InitializeComponent(); 
            Title.Content = AddEdit ? "Edit Order" : "Add Order";
            Order = order;

            EmployeeCombo.ItemsSource = Database.Instance.GetAllEmployees();
            for(int i = 0; i < EmployeeCombo.Items.Count; i++) {
                if (((Employee)EmployeeCombo.Items[i]).EmployeeId == order.Employee.EmployeeId) {
                    EmployeeCombo.SelectedIndex = i;
                    break;
                }
            }

            ProductCombo.ItemsSource = Database.Instance.GetAllProducts();
            ProductCombo.SelectedValue = Order.Product;
            for (int i = 0; i < ProductCombo.Items.Count; i++) {
                if (((Product)ProductCombo.Items[i]).ProductId == order.Product.ProductId) {
                    ProductCombo.SelectedIndex = i;
                    break;
                }
            }

            StatusCombo.ItemsSource = Enum.GetValues(typeof(Order.Status)).Cast<Order.Status> ();
            StatusCombo.SelectedIndex = (int)Order.OrderStatus;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Finish(object sender, RoutedEventArgs e) {
            AddCancel = false;
            this.Close();
        }
        public void Cancel(object sender, RoutedEventArgs e) {
            this.Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private bool IsMaximized = false;

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1080;
                    this.Height = 720;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;
                }
            }
        }
    }
}
