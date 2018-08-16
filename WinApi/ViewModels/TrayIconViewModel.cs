using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.ViewModels
{
    public class TrayIconViewModel
    {
        private string _aaa;

        public string Aaa { get => _aaa; set => _aaa = value; }

        public TrayIconViewModel()
        {
            Aaa = "/Resources/Icons/online.ico";
        }
    }
}
