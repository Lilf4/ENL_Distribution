using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Printing;
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

        private void Validate(object sender, TextChangedEventArgs e) {
            string result = "";
            bool canFinish = true;

            if (locations.Contains(product.Location.LocationId) && product.Location.LocationId != oldLoc) {
                LocCol.BorderBrush = Brushes.Red;
                LocRow.BorderBrush = Brushes.Red;

                result += "Location is already used\r\n";
                canFinish = false;
            }
            else {
                LocCol.BorderBrush = Brushes.LightGray;
                LocRow.BorderBrush = Brushes.LightGray;
            }

            FinishBtn.IsEnabled = canFinish;
            ErrorTxt.Content = result;
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
