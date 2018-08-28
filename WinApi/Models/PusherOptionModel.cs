using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Interfaces.Model;

namespace WinApi.Models
{
    public class PusherOptionModel : IPusherOptionModel
    {
        private string _pusherKey;
        private string _pusherAuthorizer;
        private bool _pusherOn;

        public string PusherKey { get => _pusherKey; set => _pusherKey = value; }
        public string PusherAuthorizer { get => _pusherAuthorizer; set => _pusherAuthorizer = value; }
        public bool PusherON { get => _pusherOn; set => _pusherOn = value; }
    }
}
