using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hardcodet.Wpf.TaskbarNotification;

namespace WinApi.Messages
{
    class ShowBallonTipMessage
    {
        private string _title;
        private string _msg;
        private Hardcodet.Wpf.TaskbarNotification.BalloonIcon _iconType;

        public string Title { get => _title; set => _title = value; }
        public string Msg { get => _msg; set => _msg = value; }
        public BalloonIcon IconType { get => _iconType; set => _iconType = value; }
    }
}
