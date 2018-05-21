
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


           

            //  notifyIcon = (TaskbarIcon) FindResource("TrayIcon");

            //  var SignProgramPath = @"c:\program Files (x86)\Notepad++\notepad++.exe";
            //  var SignPrograms = @"c:\program Files (x86)\adobe\acrobat Reader DC\reader\AcroRd32.exe";
            //    var FilePath = @"c:\users\jurco\desktop\test.txt";
            //  string proceName = @FilePath + " - Notepad++";
            /* using (WebClient webClient = new WebClient())
              {
                  webClient.DownloadFile("http://192.168.33.10/test.pdf", "test.pdf");
              }
             */

            string title = "WPF NotifyIcon";
            string text = "This is a standard balloon";
        
            //notifyIcon.ShowBalloonTip(title, text, BalloonIcon.Info);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Options s = new Options();
            ///   notifyIcon = (ResourceDictionary)this.TryFindResource("someResourceName")

            // notifyIcon.ShowBalloonTip("asdasdas", "asdadassa", BalloonIcon.Info);


            // trayIconTaskbar
            PusherConnect tt = new PusherConnect("31d14ddddef4c14b6ab5", s.Data.ApiLink, true, "eu", s.Data.ChannelName,s);

            //Signature test = new Signature(s.Data.ProgramPath, @"c:\users\jurco\desktop\test.txt", s.Data.ProcessName);

            //bool t = test.SignFile();
        }

        private void MenuItem_MouseEnter(object sender, MouseEventArgs e)
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
            
            //nastaveniaWindows.Hide();
            trayIconTaskbar.ShowBalloonTip("asdasdas", "asdadassa", BalloonIcon.Info);
        }
    }


 
}

