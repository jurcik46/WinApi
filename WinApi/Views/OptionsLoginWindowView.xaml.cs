using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using WinApi.ViewModels;
using WinApi.Interfaces;

namespace WinApi.Views
{
    /// <summary>
    /// Interaction logic for OptionsLoginWindowView.xaml
    /// </summary>
    public partial class OptionsLoginWindowView : Window, IClosable
    {

        public OptionsLoginWindowView()
        {

            InitializeComponent();
            this.DataContext = ViewModelLocator.OptionsLoginViewModel;
        }

        private void optionsPassowrdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as OptionsLoginViewModel;
            var passwordBox = sender as PasswordBox;
            if (viewModel == null || passwordBox == null)
            {
                return;
            }

            viewModel.Passwrod = passwordBox.Password;
            VizualizePasswordValidation(passwordBox);

        }

        private void VizualizePasswordValidation(PasswordBox passwordBox)
        {
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.BorderThickness = new Thickness(3);
                passwordBox.ToolTip = "Heslo nemôže byť prázdne alebo obsahovať medzery!";
                passwordBox.Background = Brushes.Red;
                enterPasswordButton.IsEnabled = false;
            }
            else
            {
                passwordBox.BorderBrush = Brushes.Black;
                passwordBox.BorderThickness = new Thickness(1);
                passwordBox.ToolTip = null;
                passwordBox.Background = Brushes.White;
                enterPasswordButton.IsEnabled = true;

            }
        }
    }
}
