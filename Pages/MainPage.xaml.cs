using System;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class MainPage : Page{

        public MainPage(){
            InitializeComponent();
        }

        private void NavWorker(object sender, RoutedEventArgs e){
            NavigationService.Navigate(new WorkerPage());
        }
        private void NavProduct(object sender, RoutedEventArgs e){
            NavigationService.Navigate(new ProductPage());
        }
        private void NavOrder(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new OrderPage());
        }
        private void NavExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}