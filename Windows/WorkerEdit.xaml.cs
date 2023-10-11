using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace ENF_Dist_Test.Windows {
    /// <summary>
    /// Interaction logic for ProductEdit.xaml
    /// </summary>
    public partial class WorkerEdit : Window, INotifyPropertyChanged {

        public bool AddCancel = true;

        private Employee employee { get; set; }
        public Employee Employee { 
            get {
                return employee;
            }
            set {
                employee = value;
                OnPropertyChanged("Employee");
            } 
        }
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WorkerEdit(Employee employee, bool AddEdit) {
            InitializeComponent();
            Title.Content = AddEdit ? "Edit Employee" : "Add Employee";
            Employee = employee;

            TitleCombo.ItemsSource = Enum.GetValues(typeof(Employee.JobTitle)).Cast<Employee.JobTitle>();
            TitleCombo.SelectedIndex = (int)Employee.Title;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Finish(object sender, RoutedEventArgs e) {
            AddCancel = false;
            this.Close();
        }
        public void Cancel(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
