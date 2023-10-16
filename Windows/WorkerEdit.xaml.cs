using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
                OnPropertyChanged(nameof(Employee));
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
