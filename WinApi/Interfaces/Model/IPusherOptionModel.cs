using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Interfaces.Model
{
    public interface IPusherOptionModel
    {
        string PusherKey { get; set; }
        string PusherAuthorizer { get; set; }
        bool PusherON { get; set; }
    }
}
