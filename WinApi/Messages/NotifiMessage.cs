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
        private string _title;
        private string _msg;
        private NotificationType _iconType;
        private int _expTime = 10;

        public string Title { get => _title; set => _title = value; }
        public string Msg { get => _msg; set => _msg = value; }
        public NotificationType IconType { get => _iconType; set => _iconType = value; }
        public int ExpTime { get => _expTime; set => _expTime = value; }



    }
}
