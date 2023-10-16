using System.Windows;
using System.Windows.Input;

namespace ENF_Dist_Test.Windows {
    /// <summary>
    /// Interaction logic for confirm.xaml
    /// </summary>
    public partial class confirm : Window {

        public confirm(string title = "Are you sure?", string description = "", bool ConfirmOk = true) {
            InitializeComponent();

            FinishBtn.Visibility = ConfirmOk ? Visibility.Visible : Visibility.Hidden;
            CancelBtn.Visibility = ConfirmOk ? Visibility.Visible : Visibility.Hidden;
            OkBtn.Visibility = !ConfirmOk ? Visibility.Visible : Visibility.Hidden;

            Title.Content = title;
            Description.Content = description;
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
