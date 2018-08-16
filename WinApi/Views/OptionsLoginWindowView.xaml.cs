using System.Windows;
using WinApi.ViewModels;

namespace WinApi.Views
{
    /// <summary>
    /// Interaction logic for OptionsLoginWindowView.xaml
    /// </summary>
    public partial class OptionsLoginWindowView : Window
    {

        public OptionsLoginWindowView()
        {
            InitializeComponent();
            this.DataContext = new OptionsLoginViewModel();
        }
    }
}
