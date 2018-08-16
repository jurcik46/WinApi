using GalaSoft.MvvmLight;
using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.ViewModels
{
    public class TrayIconViewModel : ViewModelBase
    {
        private TaskbarIcon _test;
        private string _aaa;

        public string Aaa { get => _aaa; set => _aaa = value; }
        public TaskbarIcon Test { get => _test; set => _test = value; }
        public string ToolTipText { get; set; } = "Dvojklik pre podpísanie a kliknutím pravým tlačidlom pre menu";
        public TrayIconViewModel()
        {
            Aaa = "/Resources/Icons/online.ico";
            // Test. = Properties.Resources.online;

        }
    }
}
