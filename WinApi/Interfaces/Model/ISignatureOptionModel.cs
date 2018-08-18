using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Interfaces.Model
{
    public interface ISignatureOptionModel
    {
        string ProgramPath { get; set; }
        string ProcessName { get; set; }
    }
}
