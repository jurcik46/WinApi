using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Interfaces.Service
{
    public interface IPusherService
    {
        void StartPusher();
        void SendBozpStatus(string msg);
    }
}
