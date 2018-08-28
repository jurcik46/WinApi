using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Interfaces.Model;

namespace WinApi.Models
{
    public class SignatureOptionModel : ISignatureOptionModel
    {
        private string _programPath;
        private string _processName;

        public string ProgramPath { get => _programPath; set => _programPath = value; }
        public string ProcessName { get => _processName; set => _processName = value; }
    }
}
