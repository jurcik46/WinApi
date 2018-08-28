using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinApi.Interfaces.Model;

namespace WinApi.Models
{
    public class SignatureFileModel : ISignatureFileModel
    {
        private string _pdfFilePath;
        private string _hash;
        private byte[] _file;

        public string PdfFilePath { get => _pdfFilePath; set => _pdfFilePath = value; }
        public string Hash { get => _hash; set => _hash = value; }
        public byte[] File { get => _file; set => _file = value; }

        public SignatureFileModel(API.Model.SignatureFileModel signatueFile)
        {
            PdfFilePath = signatueFile.PdfFilePath;
            Hash = signatueFile.Hash;
            File = Convert.FromBase64String(signatueFile.File);
        }
    }
}
