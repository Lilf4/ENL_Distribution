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
        }

        private void NavBack(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void Add(object sender, RoutedEventArgs e) {

        }
        private void Update(object sender, RoutedEventArgs e) {

        }
        private void Delete(object sender, RoutedEventArgs e) {

        }
    }
}
