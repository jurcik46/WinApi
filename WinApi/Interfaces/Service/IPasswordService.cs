using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Interfaces.Service
{
    public interface IPasswordService
    {
        string Password { get; set; }
        string EnteredPassword { get; set; }

        byte[] GetHash(string inputString);
        string GetHashString(string inputString);
        void CreatePass(string heslo);
        bool ComperPassword(string enteredPassword);
    }
}
