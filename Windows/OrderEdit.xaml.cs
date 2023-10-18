using ENF_Dist_Test.Validators;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ENF_Dist_Test.Windows {
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
            if(order.Employee != null) {
                for(int i = 0; i < EmployeeCombo.Items.Count; i++) {
                    if (((Employee)EmployeeCombo.Items[i]).EmployeeId == order.Employee.EmployeeId) {
                        EmployeeCombo.SelectedIndex = i;
                        break;
                    }
                }
            }

            ProductCombo.ItemsSource = Database.Instance.GetAllProducts();
            if (order.Product != null) {
                for (int i = 0; i < ProductCombo.Items.Count; i++) {
                    if (((Product)ProductCombo.Items[i]).ProductId == order.Product.ProductId) {
                        ProductCombo.SelectedIndex = i;
                        break;
                    }
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
        
        private void Validate(object sender, SelectionChangedEventArgs e) {
            Validate(new(), new TextChangedEventArgs(e.RoutedEvent, UndoAction.Undo));
        }

        private void Validate(object sender, TextChangedEventArgs e) {
            string result = "";
            bool canFinish = true;

            if(order.OrderStatus == Order.Status.Finished && order.Quantity > order.Product.Quantity) {
                result += $"There's not enough {order.Product} to finish order,";
                canFinish = false;
            }


            FluentValidation.Results.ValidationResult res = new OrderValidator().Validate(Order);

            if (!res.IsValid) {
                canFinish = false;
            }
            result += res.ToString(",");

            FinishBtn.Opacity = canFinish ? 1 : 0.5;
            FinishBtn.IsEnabled = canFinish;
            ErrorTxt.Content = result;
        }
    }
}
