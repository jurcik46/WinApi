using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Interfaces.Model
{
    public interface IApiOptionModel
    {
        string ApiLink { get; set; }
        string Apikey { get; set; }
        string UserID { get; set; }
        string ObjecID { get; set; }
    }
}
