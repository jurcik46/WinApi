using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Enums
{
    public enum SignatureServiceEvents
    {
        StartSign,
        CreateDirectory,
        CreateDirectoryError,
        SignFile,
        SaveFile,
        SaveFileError,
        SignFileWindowFound,
        SignFileWindowClosed,
        SignFileAfterWait,
        WindowNotFound,
        WindowFound,
        WindowFoundAndClosing
    }
}
