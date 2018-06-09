using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
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
        /// <summary>
        /// Pri  zapnuty okna nacita nastavenia zo suboru options.json
        /// </summary>
        public Nastavenia()
        {
            opt.LoadOption();
            DataContext = opt;
            InitializeComponent();
        }

        /// <summary>
        /// Event na tlacitko pre ulozenie nastaveni 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                opt.Data.PusherKey = pusherKeyTextBox.Text;
                opt.Data.PusherAuthorizer = pusherHttpAuthorizerTextBox.Text;
                if (pusherOnCheckBox.IsChecked == true)
                {
                    opt.Data.PusherON = true;
                }
                else {
                    opt.Data.PusherON = false;
                }
                opt.SaveOption();

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();

            }
            else {
                Log.Warning("Bolo zadane nespravne heslo pri ukladaní nastaveni  Heslo: {0} ", optionsPassowrdBox.Password.ToString());
                MessageBox.Show("Nespravne heslo", "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }

            
        }
    }
}
