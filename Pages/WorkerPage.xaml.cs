using ENF_Dist_Test.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ENF_Dist_Test.Pages{
    public partial class WorkerPage : Page {

        public WorkerPage(){
            InitializeComponent();
            UpdateButtons(null, null);
            UpdateTable();
        }

        private void UpdateTable() {
            DataGrid.ItemsSource = Database.Instance.GetAllEmployees();
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
        private void NavBack(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void Add(object sender, RoutedEventArgs e) {
            WorkerEdit workerEdit = new(new() { EmployeeId = Database.Instance.GetNextID("Employees")}, false);
            workerEdit.ShowDialog();
            if (!workerEdit.AddCancel) {
                Database.Instance.InsertEmployee(workerEdit.Employee);
                UpdateTable();
            }
        }
        private void Update(object sender, RoutedEventArgs e) {
            WorkerEdit workerEdit = new((Employee)DataGrid.SelectedItem, true);
            workerEdit.ShowDialog();
            if (!workerEdit.AddCancel) {
                Database.Instance.UpdateEmployee(workerEdit.Employee, workerEdit.Employee.EmployeeId);
                UpdateTable();
            }
        }
        private void Delete(object sender, RoutedEventArgs e) {
            Employee employee = (Employee)DataGrid.SelectedItem;
            if(new confirm("Delete", $"Are you sure you want to delete {employee}?").ShowDialog().Value) {
                try {
                    Database.Instance.DeleteEmployee(employee.EmployeeId);
                } catch(Exception ex) {
                    new confirm("Error", "There is active orders referencing employee\r\nFinish/Delete orders to continue", false).ShowDialog();
                }
            UpdateTable();
            }
        }
    }
}