using GalaSoft.MvvmLight.Messaging;
using Notifications.Wpf;
using System;
using System.Windows;
using WinApi.Messages;

namespace WinApi.Views
{
    /// <summary>
    /// Interaction logic for NotifiWindowView.xaml
    /// </summary>
    public partial class NotifiWindowView : Window
    {

        private readonly NotificationManager _notificationManager = new NotificationManager();
        private NotificationContent conetn;


        public NotifiWindowView()
        {
            InitializeComponent();
            //this.DataContext = ViewModelLocator.NotifiWindowViewModel;
            conetn = new NotificationContent();

            Messenger.Default.Register<NotifiMessage>(this, (message) =>
            {
                conetn.Title = message.Title;
                conetn.Message = message.Msg;
                conetn.Type = message.IconType;

                this._notificationManager.Show(conetn, expirationTime: System.TimeSpan.FromSeconds(message.ExpTime), areaName: "WindowArea");
            });
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Width = desktopWorkingArea.Width;
            this.Height = desktopWorkingArea.Height;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;

            //var image = new BitmapImage();
            //image.BeginInit();
            //image.UriSource = new Uri(@"pack://application:,,,/Resources/img/Reload-1s-50px.gif");
            //image.EndInit();
            //ImageBehavior.SetAnimatedSource(loading, image);
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
    }
}
