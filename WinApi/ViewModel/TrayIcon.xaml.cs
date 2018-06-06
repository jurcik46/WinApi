
using Serilog;
using System.Diagnostics;
using System.Windows;
using Hardcodet.Wpf.TaskbarNotification;
using PusherClient;
using WinApi.Models;
using WinApi.Code;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;

namespace WinApi.ViewModel
{
    public partial class TrayIcon 
    {
        private Nastavenia nastaveniaWindows = null;
        private PusherConnect pusher = null;
        private Options s = null;
        public string workingIcon = @"Icons/working.ico";
        public string onlineIcon = @"Icons/online.ico";
        public string offlineIcon = @"Icons/offline.ico";
        private bool on;
        public TrayIcon()
        {
           
         
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();


            s = new Options();
            if (s.Data.Succes)
            {
                pusher = new PusherConnect("31d14ddddef4c14b6ab5", "http://192.168.33.10/", true, "eu", s);
                if (pusher._pusher != null)
                {
                    pusher._pusher.ConnectionStateChanged += _pusher_ConnectionStateChanged;
                    pusher._pusher.Error += _pusher_Error;
                }
            }
         

        }

        public void _pusher_ConnectionStateChanged(object sender, ConnectionState state)
        {

          //  Console.WriteLine("Connection state: " + state.ToString());
            if (state == ConnectionState.Connected)
            {
                trayIconTaskbar.Icon = new System.Drawing.Icon(onlineIcon);
                trayIconTaskbar.ShowBalloonTip("Status pripojenia", "Aplikacia je pripojenia k internetu", BalloonIcon.Info);
                on = true;
            }
            if (state == ConnectionState.Disconnected) {
               
                if (on)
                {
                    trayIconTaskbar.Icon = new System.Drawing.Icon(offlineIcon);
                    trayIconTaskbar.ShowBalloonTip("Status pripojenia", "Aplikacia stratila pripojenie k internetu", BalloonIcon.Warning);
                    on = false;
                }
            }
            if(state == ConnectionState.Connecting)
            {
                pusher = new PusherConnect("31d14ddddef4c14b6ab5", "http://192.168.33.10/", true, "eu", s); 

            }

           

        }

        public void _pusher_Error(object sender, PusherException error)
        {
            if (error != null)
                trayIconTaskbar.ShowBalloonTip("Chyba", error.ToString(), BalloonIcon.Error);
            

        }


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            //pusher.send("my-event", "test", "asddddd");
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

        private void trayIconTaskbar_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {

            if (s.Data.Succes) {
              //  if (pusher.CheckConnection(@"https://www.google.com/")) {
                    if (!s.Data.InProcess)
                    {
                        try
                        {
                            trayIconTaskbar.Icon = new System.Drawing.Icon(workingIcon);
                            pusher.GetInfo();
                        }
                        catch (MyException ex )
                        {
                        trayIconTaskbar.ShowBalloonTip("WinApi", ex.Message, BalloonIcon.Info);
                         }                       
                        finally {
                            s.Data.InProcess = false;
                        }
                    }
                    else {
                        trayIconTaskbar.ShowBalloonTip("Info", "Prave sa vykonva podpisovanie", BalloonIcon.Info);
                    }
                /*}else
                {

                    trayIconTaskbar.ShowBalloonTip("Info", "Pripojenie k internetu je nefuknce", BalloonIcon.Warning);

                }*/
            }
            else
            {
                trayIconTaskbar.ShowBalloonTip("Nastavenia", "Vyplnte nastavenia", BalloonIcon.Info);

            }


            trayIconTaskbar.Icon = new System.Drawing.Icon(onlineIcon);

        }
    }


 
}

