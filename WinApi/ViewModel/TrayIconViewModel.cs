using System;
using Serilog;
using PusherClient;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi
{
    class TrayIconViewModel
    {

        public TrayIconViewModel() {

            //  var SignProgramPath = @"c:\program Files (x86)\Notepad++\notepad++.exe";
            //  var SignPrograms = @"c:\program Files (x86)\adobe\acrobat Reader DC\reader\AcroRd32.exe";
            //    var FilePath = @"c:\users\jurco\desktop\test.txt";
            //  string proceName = @FilePath + " - Notepad++";
            /* using (WebClient webClient = new WebClient())
              {
                  webClient.DownloadFile("http://192.168.33.10/test.pdf", "test.pdf");
              }
             */
            // PusherConnect test = new PusherConnect("31d14ddddef4c14b6ab5", "http://192.168.33.10/", true, "eu", "channel");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();




            // Signature test = new Signature(@"c:\program Files (x86)\Notepad++\notepad++.exe", @"c:\users\jurco\desktop\test.txt", @"c:\users\jurco\desktop\test.txt - Notepad++");

            //   bool t = test.SignFile();
        }

        public ICommand ExitApplicationCommand
        {
            get
            {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }




    }


    /// <summary>
    /// Simplistic delegate command for the demo.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter)
        {
            CommandAction();
        }

        public bool CanExecute(object parameter)
        {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
