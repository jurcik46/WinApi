using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Messages
{
    class BozpStatusPusherMessage
    {
        private string _status;

        public string Status { get => _status; set => _status = value; }
    }
}
