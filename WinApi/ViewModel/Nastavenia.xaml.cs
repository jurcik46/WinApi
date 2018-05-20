using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinApi.DataModels;
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

        }
    }
}
