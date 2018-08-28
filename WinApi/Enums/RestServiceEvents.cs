using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApi.Enums
{
    public enum RestServiceEvents
    {
        GetDocumentToSignature,
        GetDocumentToSignatureNotFound,
        GetDocumentToSignatureError,
        UploadSignedDocument,
        UploadSignedDocumentFailed,
        UploadSignedDocumentError

    }
}
