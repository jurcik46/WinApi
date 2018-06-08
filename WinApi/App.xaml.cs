using System;
using Hardcodet.Wpf.TaskbarNotification;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace WinApi
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private TaskbarIcon notifyIcon;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in TrayIcon.xaml
            notifyIcon = (TaskbarIcon)FindResource("TrayIconTaskbar");

            
        //    notifyIcon.ShowBalloonTip("asdasdas", "asdadassa", BalloonIcon.Info  );
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }


   
}
