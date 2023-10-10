using System;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class WorkerPage : Page{
        public WorkerPage(){
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