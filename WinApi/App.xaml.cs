using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using GalaSoft.MvvmLight.Threading;
using WinApi.ViewModels;
using Serilog.Events;
using System;

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

            //create the notifyicon (it'option a resource declared in TrayIcon.xaml
            notifyIcon = (TaskbarIcon)FindResource("TrayIconTaskbar");
            DispatcherHelper.Initialize();

            ViewModelLocator.LoggingLevelSwitch.MinimumLevel = LogEventLevel.Information;
            var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var currentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            Logger.LoggerInit.ApplicationName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            Logger.LoggerInit.Version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            Console.WriteLine(Logger.LoggerInit.Version);

            Serilog.Log.Logger = Logger.LoggerInit.InitializeApplicationLogger(ViewModelLocator.LoggingLevelSwitch, currentCulture, currentUICulture);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }



}
