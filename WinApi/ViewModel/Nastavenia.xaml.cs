using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinApi.Models;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;

namespace WinApi.ViewModel
{
    /// <summary>
    /// Interaction logic for Nastavenia.xaml
    /// </summary>
    public partial class Nastavenia : Window
    {
        private Options opt = new Options();

        public Nastavenia()
        {
            opt.LoadOption();
            DataContext = opt;
            InitializeComponent();
        }


        private void CheckPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            //optionsPassowrdBox

            if (opt.ComperPassword(optionsPassowrdBox.Password.ToString()))
            {
                opt.Data.ApiLink = apiTextBox.Text;
                opt.Data.UserID = userIdTextBox.Text;
                opt.Data.ObjecID = objectIdTextBox.Text;
                opt.Data.ModuleID = moduleIdTextBox.Text;
                opt.Data.ProgramPath = programPathTextBox.Text;
                opt.Data.ProcessName = processNameTextBox.Text;

                opt.SaveOption();

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();

            }
            else {
                MessageBox.Show("Nespravne heslo", "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }

            
        }
    }
}
