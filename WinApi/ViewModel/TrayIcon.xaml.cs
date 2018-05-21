
using Serilog;
using System.Diagnostics;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using PusherClient;
using WinApi.DataModels;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.ViewModel
{
    public partial class TrayIcon 
    {
        private Nastavenia nastaveniaWindows = null;

        public TrayIcon()
        {
            //notifyIcon.ShowBalloonTip(title, text, BalloonIcon.Info);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Options s = new Options();
          

            // notifyIcon.ShowBalloonTip("asdasdas", "asdadassa", BalloonIcon.Info);


        
            PusherConnect tt = new PusherConnect("31d14ddddef4c14b6ab5", s.Data.ApiLink, true, "eu", s.Data.ChannelName,s);

           
        }
        
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
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
    }


 
}

