using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Order order { get; set; }
        public Order Order { 
            get {
                return order;
            }
            set {
                order = value;
                OnPropertyChanged("Order");
            } 
        }
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public OrderEdit(Order order, bool AddEdit) {
            InitializeComponent(); 
            Title.Content = AddEdit ? "Edit Order" : "Add Order";
            Order = order;

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
    }
}
