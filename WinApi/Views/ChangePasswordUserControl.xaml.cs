using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinApi.ViewModels;

namespace WinApi.Views
{
    /// <summary>
    /// Interaction logic for ChangePasswordUserControl.xaml
    /// </summary>
    public partial class ChangePasswordUserControl : UserControl
    {
        public ChangePasswordUserControl()
        {
            InitializeComponent();
            this.DataContext = ViewModelLocator.ChangePasswordViewModel;
        }


        private void VizualizePasswordValidation(PasswordBox passwordBox)
        {
            if (string.IsNullOrWhiteSpace(passwordBox.Password) || passwordBox.Password.Length < 5)
            {
                passwordBox.BorderThickness = new Thickness(3);
                passwordBox.ToolTip = "Heslo nemôže byť prázdne alebo obsahovať medzery a musí byť dlhšie ako 5 znakov!";
                passwordBox.Background = Brushes.Red;
                changePasswordButton.IsEnabled = false;
            }
            else
            {
                passwordBox.BorderBrush = Brushes.Black;
                passwordBox.BorderThickness = new Thickness(1);
                passwordBox.ToolTip = null;
                passwordBox.Background = Brushes.White;
                changePasswordButton.IsEnabled = true;
            }
        }

        private void newPasswrod1PassowrdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ChangePasswordViewModel;
            var passwordBox = sender as PasswordBox;
            if (viewModel == null || passwordBox == null)
            {
                return;
            }
            viewModel.Password1 = passwordBox.Password;
            VizualizePasswordValidation(passwordBox);
        }


        private void newPasswrod2PassowrdBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ChangePasswordViewModel;
            var passwordBox = sender as PasswordBox;
            if (viewModel == null || passwordBox == null)
            {
                return;
            }
            viewModel.Password2 = passwordBox.Password;
            VizualizePasswordValidation(passwordBox);
        }
    }
}
