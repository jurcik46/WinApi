using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinApi.Code;
using WinApi.Models;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WinApi.ViewModel
{

    /// <summary>
    /// Interaction logic for Vstup.xaml
    /// </summary>
    public partial class Vstup : Window
    {
        private Options option = null;
        private Nastavenia nastaveniaWindows = null;
        public Vstup()
        {           
            InitializeComponent();
            option = new Options();
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (option.ComperPassword(optionsPassowrdBox.Password.ToString()))
            {
                this.Close();
                if (nastaveniaWindows != null && nastaveniaWindows.IsLoaded)
                {
                    nastaveniaWindows.Topmost = true;
                    nastaveniaWindows.WindowState = WindowState.Normal;
                    nastaveniaWindows.Topmost = false;
                }
                else
                {
                    nastaveniaWindows = new Nastavenia();
                    nastaveniaWindows.Show();
                }
            }
            else {

                MessageBox.Show("Nesprávne heslo!!!", "Chyba", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
