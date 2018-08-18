using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Interfaces.Model
{
    public interface ISignatureFileModel
    {
        string PdfFilePath { get; set; }
        string Hash { get; set; }
        byte[] File { get; set; }

    }
}
