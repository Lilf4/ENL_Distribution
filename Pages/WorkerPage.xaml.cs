using System;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class WorkerPage : Page{
        public WorkerPage(){
            InitializeComponent();
            test.Text = DateTime.Now.ToString();
        }

        private void NavBack(object sender, RoutedEventArgs e){
            NavigationService.GoBack();
        }
    }
}