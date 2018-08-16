using System.Windows;
using WinApi.ViewModels;

namespace WinApi.Views
{
    /// <summary>
    /// Interaction logic for OptionsWindowView.xaml
    /// </summary>
    public partial class OptionsWindowView : Window
    {
        public OptionsWindowView()
        {
            InitializeComponent();
            this.DataContext = new OptionsViewModel();
        }
    }
}
