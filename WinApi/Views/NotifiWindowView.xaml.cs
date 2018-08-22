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
using System.Windows.Shapes;
using WinApi.ViewModels;
using WpfAnimatedGif;

namespace WinApi.Views
{
    /// <summary>
    /// Interaction logic for NotifiWindowView.xaml
    /// </summary>
    public partial class NotifiWindowView : Window
    {
        public NotifiWindowView()
        {
            InitializeComponent();
            this.DataContext = ViewModelLocator.NotifiWindowViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            //this.Left = desktopWorkingArea.Right - this.Width;
            this.Left = desktopWorkingArea.Left;
            this.Top = desktopWorkingArea.Bottom - this.Height;

            //var image = new BitmapImage();
            //image.BeginInit();
            //image.UriSource = new Uri(@"pack://application:,,,/Resources/img/Reload-1s-50px.gif");
            //image.EndInit();
            //ImageBehavior.SetAnimatedSource(loading, image);
        }
    }
}
