using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Interfaces.Model;

namespace WinApi.Interfaces.Service
{
    public interface IRestService
    {
        ISignatureFileModel GetDocumentToSignature();

    }
}
