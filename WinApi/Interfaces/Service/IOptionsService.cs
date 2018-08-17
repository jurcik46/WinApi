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
        OptionsModel Options { get; set; }

        void LoadOptionsFromSetting();
        void SaveOptionsToSetting();

    }
}
