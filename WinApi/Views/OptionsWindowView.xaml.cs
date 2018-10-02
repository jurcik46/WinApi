using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            this.DataContext = ViewModelLocator.OptionsViewModel;
        }
    }

}