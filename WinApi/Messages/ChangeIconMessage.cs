using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Enums;

namespace WinApi.Messages
{
    class ChangeIconMessage
    {
        private TrayIcons _icon;

        public TrayIcons Icon { get => _icon; set => _icon = value; }
    }
}
