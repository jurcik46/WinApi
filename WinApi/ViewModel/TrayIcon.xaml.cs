
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
        private string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        private Vstup vstupWindows = null;
        private PusherConnect pusher = null;
        private Options option = null;
        public string workingIcon = @"Icons/working.ico";
        public string onlineIcon = @"Icons/online.ico";
        public string offlineIcon = @"Icons/offline.ico";
        private bool on;
        public TrayIcon()
        {
           
         
            Log.Logger = new LoggerConfiguration()         
                .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
      

            // pri uspesnom nacitany nastaveni sa vytvori trieda pusher ktora sluzi na komunikaciu pomocou pushera a APIcka
            option = new Options();
            if (option.Data.Succes)
            {
                pusher = new PusherConnect(true, "eu", option);
                if (pusher._pusher != null)
                {
                    pusher._pusher.ConnectionStateChanged += _pusher_ConnectionStateChanged;
                    pusher._pusher.Error += _pusher_Error;
                    pusher_binding();
                }
            }
         

        }
        /// <summary>
        /// Event pre kontrolu  connectivity pre pusher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="state"></param>
        public void _pusher_ConnectionStateChanged(object sender, ConnectionState state)
        {

           // Console.WriteLine("Connection state: " + state.ToString());
            if (state == ConnectionState.Connected)
            {
                trayIconTaskbar.Icon = new System.Drawing.Icon(onlineIcon);
                trayIconTaskbar.ShowBalloonTip(appName +" Status pripojenia", "Aplikácia  je pripojena k internetu", BalloonIcon.Info);
                on = true;
            }
            if (state == ConnectionState.Disconnected) {
               
                if (on)
                {
                    trayIconTaskbar.Icon = new System.Drawing.Icon(offlineIcon);
                    trayIconTaskbar.ShowBalloonTip(appName + " Status pripojenia", "Aplikácia stratila pripojenie k internetu", BalloonIcon.Warning);
                    on = false;
                }
            }
            if(state == ConnectionState.Connecting)
            {
                pusher._pusher.Connect();
                
               

            }

           

        }
        /// <summary>
        /// Event pre chybova hlasky od pushera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="error"></param>
        public void _pusher_Error(object sender, PusherException error)
        {
            if (error != null)
                trayIconTaskbar.ShowBalloonTip("Chyba", error.ToString(), BalloonIcon.Error);
                Log.Error("Pusher Error Msg = {0}", error.ToString());


        }

        /// <summary>
        /// Menu event na zobrazenie okna option nastavenim 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
          

            //pusher.send("my-event", "test", "asddddd");
            if (vstupWindows != null && vstupWindows.IsLoaded)
            {
                vstupWindows.Topmost = true;
                vstupWindows.WindowState = WindowState.Normal;
                vstupWindows.Topmost = false;
            }
            else
            {
                vstupWindows = new Vstup();
                vstupWindows.Show();
            }
           
        }

        /// <summary>
        /// Event pre dvojite kliknutnie na tray icon  spusti proces podpisovania
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void trayIconTaskbar_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {

            if (option.Data.Succes) {             
                    if (!option.Data.InProcess)
                    {
                        try
                        {
                            trayIconTaskbar.Icon = new System.Drawing.Icon(workingIcon);
                            pusher.GetInfo();
                        }
                        catch (MyException ex )
                        {
                        trayIconTaskbar.ShowBalloonTip(appName, ex.Message, BalloonIcon.Info);
                         }                       
                        finally {
                            option.Data.InProcess = false;
                        }
                    }
                    else {
                        trayIconTaskbar.ShowBalloonTip("Info", "Pravé sa vykonáva podpisovanie", BalloonIcon.Info);
                    }
            
            }
            else
            {
                trayIconTaskbar.ShowBalloonTip("Nastavenia", "Vyplňte nastavenia", BalloonIcon.Info);

            }


            trayIconTaskbar.Icon = new System.Drawing.Icon(onlineIcon);

        }

        private void pusher_binding() {
          
            pusher._channel.Bind(String.Format("event-{0}", option.Data.UserID), (dynamic data) =>
            {
               
                try
                {
                    if (!option.Data.InProcess)
                    {
                        Log.Information("Bind na event : event-{0}", option.Data.UserID);
                        pusher.GetInfo();

                    }
                }
                catch (MyException ex)
                {
                    trayIconTaskbar.ShowBalloonTip(appName, ex.Message, BalloonIcon.Info);
                }
                finally
                {
                    option.Data.InProcess = false;
                }
              
            });
        }
    }


 
}

