using ENF_Dist_Test.Windows;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class WorkerPage : Page{

        public WorkerPage(){
            InitializeComponent();
            updateTable();
        }

        private void updateTable() {
            DataGrid.ItemsSource = Database.Instance.GetAllEmployees();
        }

        private void NavBack(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void Add(object sender, RoutedEventArgs e) {
            WorkerEdit workerEdit = new WorkerEdit(new() { EmployeeId = Database.Instance.GetNextID("Employees")}, false);
            workerEdit.ShowDialog();
            if (!workerEdit.AddCancel) {
                Database.Instance.InsertEmployee(workerEdit.Employee);
                updateTable();
            }
        }
        private void Update(object sender, RoutedEventArgs e) {
            WorkerEdit workerEdit = new WorkerEdit((Employee)DataGrid.SelectedItem, true);
            workerEdit.ShowDialog();
            if (!workerEdit.AddCancel) {
                Database.Instance.UpdateEmployee(workerEdit.Employee, workerEdit.Employee.EmployeeId);
                updateTable();
            }
        }
        private void Delete(object sender, RoutedEventArgs e) {
            Employee employee = (Employee)DataGrid.SelectedItem;
            if(MessageBox.Show($"Are you sure you want to delete {employee}?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes) {
                Database.Instance.DeleteEmployee(employee.EmployeeId);
                updateTable();
            }
        }
    }
}