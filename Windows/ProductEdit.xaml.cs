using ENF_Dist_Test.Validators;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;

namespace ENF_Dist_Test.Windows {
    /// <summary>
    /// Interaction logic for ProductEdit.xaml
    /// </summary>
    public partial class ProductEdit : Window, INotifyPropertyChanged {

        public bool AddCancel = true;
        public string oldLoc = string.Empty;

        private Product product { get; set; }
        public Product Product { 
            get {
                return product;
            }
            set {
                product = value;
                OnPropertyChanged(nameof(Product));
            } 
        }
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HashSet<string> locations = new();

        public ProductEdit(Product product, bool AddEdit) {
            InitializeComponent();

            oldLoc = product.Location.LocationId;

            ErrorTxt.Foreground = Brushes.Red;

            Location[] temp = Database.Instance.GetAllLocations().ToArray();
            foreach(Location location in temp) {
                locations.Add(location.LocationId);
            }
            Title.Content = AddEdit ? "Edit Product" : "Add Product";
            Product = product;
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
        
        private void Validate(object sender, TextChangedEventArgs e) {
            string result = "";
            bool canFinish = true;

            if (locations.Contains(Product.Location.LocationId) && Product.Location.LocationId != oldLoc) {
                LocCol.BorderBrush = Brushes.Red;
                LocRow.BorderBrush = Brushes.Red;

                result += "Location is already used. ";
                canFinish = false;
            }
            else {
                LocCol.BorderBrush = Brushes.LightGray;
                LocRow.BorderBrush = Brushes.LightGray;
            }

            FluentValidation.Results.ValidationResult res = new ProductValidator().Validate(Product);

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
