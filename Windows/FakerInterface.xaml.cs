using ENF_Dist_Test.Validators;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace ENF_Dist_Test.Windows {
    public partial class FakerInterface : Window, INotifyPropertyChanged {
        private fakerGenerateInfo generateInfo { get; set; }
        public fakerGenerateInfo GenerateInfo {
            get {
                return generateInfo;
            }
            set {
                generateInfo = value;
                OnPropertyChanged(nameof(GenerateInfo));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public FakerInterface() {
            InitializeComponent();
            GenerateInfo = new();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left) {
                this.DragMove();
            }
        }

        private void FinishBtn_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = false;
            this.Close();
        }
    }
}
