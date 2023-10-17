using ENF_Dist_Test.Validators;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace ENF_Dist_Test.Windows {
    /// <summary>
    /// Interaction logic for ProductEdit.xaml
    /// </summary>
    public partial class EmployeeEdit : Window, INotifyPropertyChanged {

        public bool AddCancel = true;

        private Employee employee { get; set; }
        public Employee Employee { 
            get {
                return employee;
            }
            set {
                employee = value;
                OnPropertyChanged(nameof(Employee));
            } 
        }
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public EmployeeEdit(Employee employee, bool AddEdit) {
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

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
        }
        
        private void Validate(object sender, SelectionChangedEventArgs e) {
            Validate(new(), new TextChangedEventArgs(e.RoutedEvent, UndoAction.Undo));
        }

        private void Validate(object sender, TextChangedEventArgs e) {
            string result = "";
            bool canFinish = true;

            FluentValidation.Results.ValidationResult res = new EmployeeValidator().Validate(Employee);

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
