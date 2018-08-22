using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Messages
{
    class NotifiMessage
    {
        private string _appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " - ";
        private string _title;
        private string _msg;
        private NotificationType _iconType;
        private int _expTime = 5;

        public string Title { get => _title; set => _title = value; }
        public string Msg { get => _msg; set => _msg = value; }
        public NotificationType IconType { get => _iconType; set => _iconType = value; }
        public int ExpTime { get => _expTime; set => _expTime = value; }
        public string AppName { get => _appName; set => _appName = value; }
    }
}
