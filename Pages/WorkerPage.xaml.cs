using ENF_Dist_Test.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class WorkerPage : Page{
        Employee selectedEmployee;

        public WorkerPage(){
            InitializeComponent();
        }

        private void NavBack(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void Add(object sender, RoutedEventArgs e) {
            WorkerEdit workerEdit = new WorkerEdit(new(), false);
            workerEdit.ShowDialog();
            if (!workerEdit.AddCancel) {
                selectedEmployee = workerEdit.Employee;
            }
        }
        private void Update(object sender, RoutedEventArgs e) {
            WorkerEdit workerEdit = new WorkerEdit(selectedEmployee, true);
            workerEdit.ShowDialog();
            if (!workerEdit.AddCancel) {
                selectedEmployee = workerEdit.Employee;
            }
        }
        private void Delete(object sender, RoutedEventArgs e) {

        }
    }
}