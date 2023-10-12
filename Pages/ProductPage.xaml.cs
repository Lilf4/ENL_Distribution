using ENF_Dist_Test.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class ProductPage : Page{

        public ProductPage(){
            InitializeComponent();
            updateButtons(null, null);
            updateTable();
        }
        private void updateTable() {
            DataGrid.ItemsSource = Database.Instance.GetAllProducts();
        }
        public void updateButtons(object sender, RoutedEventArgs e) {
            UpdateBtn.IsEnabled = hasSelected;
            DeleteBtn.IsEnabled = hasSelected;
        }

        public bool hasSelected {
            get { return DataGrid.SelectedItem != null; }
        }
        private void NavBack(object sender, RoutedEventArgs e){
            NavigationService.GoBack();
        }

        private void Add(object sender, RoutedEventArgs e) {
            ProductEdit productEdit = new ProductEdit(new() { ProductId = Database.Instance.GetNextID("Products")}, false);
            productEdit.ShowDialog();
            if (!productEdit.AddCancel) {
                Database.Instance.InsertLocation(productEdit.product.Location);
                Database.Instance.InsertProduct(productEdit.product);
                updateTable();
            }
        }
        private void Update(object sender, RoutedEventArgs e) {
            ProductEdit productEdit = new ProductEdit((Product)DataGrid.SelectedItem, true);
            string prevLocation = productEdit.product.Location.LocationId;
            productEdit.ShowDialog();
            if (!productEdit.AddCancel) {
                Database.Instance.InsertLocation(productEdit.Product.Location);
                Database.Instance.UpdateProduct(productEdit.product, productEdit.product.ProductId);
                Database.Instance.DeleteLocation(prevLocation);
                updateTable();
            }
        }
        private void Delete(object sender, RoutedEventArgs e) {
            Product product = (Product)DataGrid.SelectedItem;
            if (MessageBox.Show($"Are you sure you want to delete {product}?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Database.Instance.DeleteProduct(product.ProductId);
                Database.Instance.DeleteLocation(product.Location.LocationId);
                updateTable();
            }
        }
    }
}