using ENF_Dist_Test.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class ProductPage : Page{

        public ProductPage(){
            InitializeComponent();
            UpdateButtons(null, null);
            UpdateTable();
        }
        private void UpdateTable() {
            DataGrid.ItemsSource = Database.Instance.GetAllProducts();
        }
        public void UpdateButtons(object? sender, RoutedEventArgs? e) {
            UpdateBtn.Opacity = HasSelected ? 1 : 0.5;
            UpdateBtn.IsEnabled = HasSelected;

            DeleteBtn.Opacity = HasSelected ? 1 : 0.5;
            DeleteBtn.IsEnabled = HasSelected;
        }

        public bool HasSelected {
            get { return DataGrid.SelectedItem != null; }
        }
        private void NavBack(object sender, RoutedEventArgs e){
            NavigationService.GoBack();
        }

        private void Add(object sender, RoutedEventArgs e) {
            ProductEdit productEdit = new(new() { ProductId = Database.Instance.GetNextID("Products")}, false);
            productEdit.ShowDialog();
            if (!productEdit.AddCancel) {
                Database.Instance.InsertLocation(productEdit.Product.Location);
                Database.Instance.InsertProduct(productEdit.Product);
                UpdateTable();
            }
        }
        private void Update(object sender, RoutedEventArgs e) {
            ProductEdit productEdit = new((Product)DataGrid.SelectedItem, true);
            string prevLocation = productEdit.Product.Location.LocationId;
            productEdit.ShowDialog();
            if (!productEdit.AddCancel) {
                if(productEdit.oldLoc != productEdit.Product.Location.LocationId) {
                    Database.Instance.InsertLocation(productEdit.Product.Location);
                }
                Database.Instance.UpdateProduct(productEdit.Product, productEdit.Product.ProductId);

                if (productEdit.oldLoc != productEdit.Product.Location.LocationId) {
                    Database.Instance.DeleteLocation(prevLocation);
                }
                UpdateTable();
            }
        }
        private void Delete(object sender, RoutedEventArgs e) {
            Product product = (Product)DataGrid.SelectedItem;
            if (new confirm("Delete", $"Are you sure you want to delete {product}?").ShowDialog().Value) {
                try {
                    Database.Instance.DeleteProduct(product.ProductId);
                    Database.Instance.DeleteLocation(product.Location.LocationId);
                } catch(Exception ex) {
                    new confirm("Error", "There is active orders referencing this product\r\nFinish/Delete orders to continue", false).ShowDialog();
                }
                UpdateTable();
            }
        }
    }
}