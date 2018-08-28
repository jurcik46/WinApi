using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Models;

namespace WinApi.Interfaces.Service
{
    public interface IOptionsService
    {
        ApiOptionModel ApiOptions { get; set; }
        PusherOptionModel PusherOptions { get; set; }
        SignatureOptionModel SignatureOptions { get; set; }
        void LoadOptionsFromSetting();
        void SaveOptionsToSetting();

    }
}
