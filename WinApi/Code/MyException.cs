using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Code
{
    public class MyException : Exception
    {

        public MyException()
        { }

        public MyException(string message)
            : base(message) 
        { }

        public MyException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
