using GalaSoft.MvvmLight;
using Hardcodet.Wpf.TaskbarNotification;
using Notifications.Wpf;
using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using WinApi.Messages;
using GalaSoft.MvvmLight.Messaging;

namespace WinApi.ViewModels
{
    public class TrayIconViewModel : ViewModelBase
    {
        private string _aaa;
        private readonly NotificationManager _notificationManager = new NotificationManager();
        public string Aaa { get => _aaa; set => _aaa = value; }

        public string ToolTipText { get; set; } = "Dvojklik pre podpísanie a kliknutím pravým tlačidlom pre menu";
        public TrayIconViewModel()
        {
            Aaa = "/Resources/Icons/online.ico";
            //var notificationManager = new NotificationManager();
            _notificationManager.Show("asdsadasdasd");
            _notificationManager.Show(new NotificationContent { Title = "asdad", Message = "asdsad" });
            Messenger.Default.Send<TestMessage>(new TestMessage() { AaT = "22222222222222" });

            //notificationManager.Show(new NotificationContent
            //{
            //    Title = "Sample notification",
            //    Message = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
            //    Type = NotificationType.Success
            //}, expirationTime: TimeSpan.FromSeconds(60));


            //var timer = new System.Timers.Timer { Interval = 1000 };
            //timer.Elapsed += (sender, args) => _notificationManager.Show("Pink string from another thread!");
            //timer.Start();

            // Test. = Properties.Resources.online;

        }


    }
}
