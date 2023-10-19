using ENF_Dist_Test.Validators;
using ENF_Dist_Test.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class MainPage : Page{

        public MainPage(){
            InitializeComponent();
            RecreateDBBtn.Visibility = Database.Instance.devState ? Visibility.Visible : Visibility.Hidden;
        }

        private void NavEmployee(object sender, RoutedEventArgs e){
            NavigationService.Navigate(new EmployeePage());
        }
        private void NavProduct(object sender, RoutedEventArgs e){
            NavigationService.Navigate(new ProductPage());
        }
        private void NavOrder(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new OrderPage());
        }
        private void RecreateDB(object sender, RoutedEventArgs e) {
            FakerInterface FI = new FakerInterface();
            if (FI.ShowDialog() == true) {
                if (new confirm("Recreate database?", "Are you sure you want to recreate the database?\r\nThis can take a while.").ShowDialog() == true) {
                    FakerClass fakerClass = new FakerClass();
                    Database.Instance.RebuildDatabase();
                    fakerClass.generateData(FI.GenerateInfo);
                }
            }
        }

        private void NavExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}