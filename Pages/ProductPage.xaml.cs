using ENF_Dist_Test.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class ProductPage : Page{
        public Product selectedProduct { get; set; } = new Product();

        public ProductPage(){
            InitializeComponent();
        }

        private void NavBack(object sender, RoutedEventArgs e){
            NavigationService.GoBack();
        }

        private void Add(object sender, RoutedEventArgs e) {
            ProductEdit productEdit = new ProductEdit(new(), false);
            productEdit.ShowDialog();
            if (!productEdit.AddCancel) {
                selectedProduct = productEdit.Product;
            }
        }
        private void Update(object sender, RoutedEventArgs e) {
            ProductEdit productEdit = new ProductEdit(selectedProduct, true);
            productEdit.ShowDialog();
            if (!productEdit.AddCancel) {
                selectedProduct = productEdit.Product;
            }
        }
        private void Delete(object sender, RoutedEventArgs e) {

        }
    }
}