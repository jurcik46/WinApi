﻿using System;
using Serilog;
using Hardcodet.Wpf.TaskbarNotification;
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
        public TrayIconViewModel()
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
            // PusherConnect test = new PusherConnect("31d14ddddef4c14b6ab5", "http://192.168.33.10/", true, "eu", "channel");
            string title = "WPF NotifyIcon";
            string text = "This is a standard balloon";

            //notifyIcon.ShowBalloonTip(title, text, BalloonIcon.Info);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File("logs\\log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            ///   notifyIcon = (ResourceDictionary)this.TryFindResource("someResourceName")

            // notifyIcon.ShowBalloonTip("asdasdas", "asdadassa", BalloonIcon.Info);
            //    trayIconTaskbar.ShowBalloonTip("asdasdas", "asdadassa", BalloonIcon.Info);

            // trayIconTaskbar

            //      Signature test = new Signature(@"c:\program Files (x86)\Notepad++\notepad++.exe", @"c:\users\jurco\desktop\test.txt", @"c:\users\jurco\desktop\test.txt - Notepad++");

            //bool t = test.SignFile();
        }

    
    }




}
